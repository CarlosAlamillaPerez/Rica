using bepensa_biz.Interfaces;
using bepensa_biz.Settings;
using bepensa_data.models;
using bepensa_models.DataModels;
using bepensa_models.App;
using bepensa_models.Enums;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Json;

namespace bepensa_ss_web.Areas.Autenticacion.Controllers
{
    [Area("Autenticacion")]
    public class CuentasController : Controller
    {
        private readonly GlobalSettings _ajustes;

        private IAccessSession _sesion { get; set; }
        private readonly IUsuario _usuario;
        private readonly IFuerzaVenta _fdv;
        private readonly IEncuesta _encuesta;

        public CuentasController(IOptionsSnapshot<GlobalSettings> ajustes, IAccessSession sesion, IUsuario usuario, IFuerzaVenta fdv, IEncuesta encuesta)
        {
            _ajustes = ajustes.Value;
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

            _sesion.Credenciales = new LoginRequest()
            {
                AccessControl = new()
            };

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequest credenciales)
        {
            try
            {
                bool esFDV = credenciales.FuerzaVenta();

                DateTime fechaAcceso = DateTime.Now;
                var ctrAcceso = _sesion.Credenciales;

                ctrAcceso.Usuario = credenciales.Usuario;
                ctrAcceso.Password = credenciales.Password;

                if (string.IsNullOrEmpty(ctrAcceso.AccessControl.Usuario))
                {
                    ctrAcceso.AccessControl.Usuario = credenciales.Usuario;
                }
                else
                {
                    if (esFDV) goto FDV;

                    if (ctrAcceso.AccessControl.Usuario == credenciales.Usuario)
                    {
                        if (ctrAcceso.AccessControl.Intentos >= _ajustes.Autenticacion.Intentos)
                        {
                            if (ctrAcceso.AccessControl.Bloqueado == false)
                            {
                                var bloquear = await _usuario.BloquearUsuario(credenciales);

                                ctrAcceso.AccessControl.Bloqueado = bloquear.Exitoso;
                            }

                            ctrAcceso.AccessControl.TiempoDesbloqueo = (fechaAcceso - ctrAcceso.AccessControl.FechaAcceso).TotalMinutes;
                            ViewData["msgError"] = "Has superado el  número de intentos permitido, por seguridad tu cuenta ha sido bloqueada, comunícate al 01 800  000 00.";

                            _sesion.Credenciales = ctrAcceso;

                            return View(credenciales);
                        }
                    }
                    else
                    {
                        ctrAcceso.AccessControl.Intentos = 0;
                        ctrAcceso.AccessControl.FechaAcceso = DateTime.Now;
                        ctrAcceso.AccessControl.TiempoDesbloqueo = 0;
                        ctrAcceso.AccessControl.CambiaPassword = false;
                        ctrAcceso.AccessControl.Usuario = credenciales.Usuario;
                    }
                }

                if (esFDV) goto FDV;

                var validarUsuario = await _usuario.ValidaAcceso(credenciales, (int)TipoOrigen.Web);

                if (!validarUsuario.Exitoso || validarUsuario.Data == null)
                {
                    ctrAcceso.AccessControl.Intentos++;
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

                //_sesion.SetCookie("TipoDeAcceso", _encryptor.Pack(CookieAuthenticationDefaults.AuthenticationScheme), TimeSpan.FromDays(1));

                //if (_sesion.UsuarioActual.CambiarPass)
                //{
                //    return RedirectToAction("CambiarPassword", "MiCuenta", new { area = "Socio" });
                //}

                var encuesta = _encuesta.ConsultarEncuestas(_sesion.UsuarioActual.Id).Data?.Where(x => x.Encuesta.Codigo.Equals("clvKitBienvenida")).FirstOrDefault();

                if (encuesta != null)
                {
                    TempData["clvKitBienvenida"] = JsonSerializer.Serialize(encuesta);
                }

                return RedirectToAction("Index", "Home", new { area = "Socio" });

            FDV:

                var validaFDV = await _fdv.ValidaAcceso(new LoginApp
                {
                    Usuario = credenciales.Usuario,
                    Password = credenciales.Password
                }, (int)TipoCanal.Tradicional
                , (int)TipoOrigen.Web);

                if (!validaFDV.Exitoso || validaFDV.Data == null)
                {
                    ctrAcceso.AccessControl.Intentos++;

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
