using bepensa_biz.Interfaces;
using bepensa_biz.Settings;
using bepensa_data.models;
using bepensa_models.DataModels;
using bepensa_models.Enums;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using bepensa_models.App;
using System.Text.Json;
using bepensa_web_common;

namespace bepensa_ss_op_web.Areas.Autenticacion.Controllers
{
    [Area("Autenticacion")]
    public class CuentasController : Controller
    {
        private readonly GlobalSettings _ajustes;
        private readonly IAppCookies _appCookies;

        private IAccessSession _sesion { get; set; }
        private readonly IUsuario _usuario;
        private readonly IFuerzaVenta _fdv;
        private readonly IEncuesta _encuesta;

        public CuentasController(IOptionsSnapshot<GlobalSettings> ajustes, IAppCookies appCookies,
            IAccessSession sesion, IUsuario usuario, IFuerzaVenta fdv, IEncuesta encuesta)
        {
            _ajustes = ajustes.Value;
            _appCookies = appCookies;
            _sesion = sesion;
            _usuario = usuario;
            _fdv = fdv;
            _encuesta = encuesta;
        }

        #region Login
        [HttpGet]
        public IActionResult Login(string? authUrl)
        {
            if (authUrl != null)
            {
                ViewData["msgError"] = CodigoDeError.SesionCaducada.GetDescription();
            }

            _sesion.Credenciales = new LoginApp();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginApp credenciales)
        {
            try
            {
                bool esFDV = credenciales.FuerzaVenta();

                DateTime fechaAcceso = DateTime.Now;
                var ctrAcceso = _sesion.Credenciales;

                ctrAcceso.Usuario = credenciales.Usuario;
                ctrAcceso.Password = credenciales.Password;

                if (esFDV) goto FDV;

                var validarUsuario = await _usuario.ValidaAcceso(credenciales, (int)TipoOrigen.Web);

                if (!validarUsuario.Exitoso || validarUsuario.Data == null)
                {
                    ViewData["msgError"] = validarUsuario.Mensaje;

                    _sesion.Credenciales = ctrAcceso;

                    return View(credenciales);
                }

                _sesion.UsuarioActual = validarUsuario.Data;

                string nombreCompleto = validarUsuario.Data.Nombre + " " + validarUsuario.Data.ApellidoPaterno;

                string iniciales = validarUsuario.Data.Nombre.Substring(0, 1) + validarUsuario.Data.ApellidoPaterno.Substring(0, 1);

                if (!string.IsNullOrEmpty(validarUsuario.Data.ApellidoMaterno))
                {
                    nombreCompleto += ' ' + validarUsuario.Data.ApellidoMaterno;
                    iniciales += validarUsuario.Data.ApellidoMaterno.Substring(0, 1);
                }

                string sesionId = validarUsuario.Data.Sesion != null ? validarUsuario.Data.Sesion : new Guid().ToString();

                var claims = new List<Claim>
                {
                    new(ClaimTypes.Email, validarUsuario.Data.Email ?? Guid.NewGuid().ToString
                    ()),
                    new(ClaimTypes.Name, nombreCompleto ),
                    new("Iniciales", iniciales ),
                    new("Sesion", sesionId)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var usuarioPrincipal = new ClaimsPrincipal(identity);

                double expireTime = _ajustes.Autenticacion.Expiracion;

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, usuarioPrincipal, new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(expireTime)
                });

                var encuesta = _encuesta.ConsultarEncuestas(_sesion.UsuarioActual.Id).Data?.Where(x => x.Encuesta.Codigo.Equals("clvKitBienvenida")).FirstOrDefault();

                if (encuesta != null)
                {
                    TempData["clvKitBienvenida"] = JsonSerializer.Serialize(encuesta);
                }

                if (_appCookies.Premio.Value != null && _appCookies.Premio.Value.Cat > 0 && _appCookies.Premio.Value.Premio > 0)
                {
                    return RedirectToAction("Premios", "Premios", new { area = "Socio", pIdCategoriaDePremio = _appCookies.Premio.Value.Cat });
                }

                return RedirectToAction("Index", "Home", new { area = "Socio" });

            FDV:
                var validaFDV = await _fdv.ValidaAcceso(new LoginApp
                {
                    Usuario = credenciales.Usuario,
                    Password = credenciales.Password
                }, (int)TipoCanal.Comidas
                , (int)TipoOrigen.Web);

                if (!validaFDV.Exitoso || validaFDV.Data == null)
                {
                    ViewData["msgError"] = validaFDV.Mensaje;

                    _sesion.Credenciales = ctrAcceso;

                    return View(credenciales);
                }

                _sesion.FuerzaVenta = validaFDV.Data;

                string fdv = validaFDV.Data.Usuario;

                string inicial = validaFDV.Data.Usuario.Substring(0, 1);

                string sesionFDV = validaFDV.Data.SesionId != null ? validaFDV.Data.SesionId : new Guid().ToString();

                var claimsFDV = new List<Claim>
                {
                    new(ClaimTypes.Email, validaFDV.Data.Usuario ?? Guid.NewGuid().ToString
                    ()),
                    new(ClaimTypes.Name, fdv ),
                    new("Iniciales", inicial ),
                    new("Sesion", sesionFDV)
                };

                var identityFDV = new ClaimsIdentity(claimsFDV, CookieAuthenticationDefaults.AuthenticationScheme);
                var fdvPrincipal = new ClaimsPrincipal(identityFDV);

                double expireTimedfv = _ajustes.Autenticacion.Expiracion;

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, fdvPrincipal, new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(expireTimedfv)
                });

                return RedirectToAction("Index", "Home", new { area = "FuerzaVenta" });
            }
            catch (Exception)
            {
                TempData["msgError"] = CodigoDeError.Excepcion.GetDescription();

                return RedirectToAction("Login", "Cuentas", new { area = "Autenticacion" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> RecuperarPassword(RestablecerPassRequest data)
        {
            var resultado = await _usuario.RecuperarContrasenia(data);

            return Json(resultado);
        }
        #endregion

        #region Logout
        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                //if (_sesion.UsuarioActual != null && _sesion.UsuarioActual.CambiarPass)
                //{
                //    TempData["Password"] = "¡Tu contraseña debe ser actualizada!";

                //    return RedirectToAction("CambiarPassword", "MiCuenta", new { area = "Socio" });
                //}

                _sesion.Logout();

                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                foreach (var cookie in Request.Cookies.Keys)
                {
                    Response.Cookies.Delete(cookie);
                }
            }
            catch (Exception)
            {
                // Aquí colocar un registro de logger cuándo se tenga acceso a la BD de control de errores.
            }

            return RedirectToAction("Login", "Cuentas", new { area = "Autenticacion" });
        }
        #endregion
    }
}
