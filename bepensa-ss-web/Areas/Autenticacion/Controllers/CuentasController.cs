using bepensa_biz.Interfaces;
using bepensa_biz.Settings;
using bepensa_data.models;
using bepensa_models.DataModels;
using bepensa_models.DTO;
using bepensa_models.Enums;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace bepensa_ss_web.Areas.Autenticacion.Controllers
{
    [Area("Autenticacion")]
    public class CuentasController : Controller
    {
        private readonly GlobalSettings _ajustes;
        private readonly IEncryptor _encryptor;

        private readonly IAccessSession _sesion;
        private readonly IUsuario _usuario;

        public IActionResult Index()
        {
            return View();
        }

        public CuentasController(IOptionsSnapshot<GlobalSettings> ajustes, IEncryptor encryptor, IAccessSession sesion, IUsuario usuario)
        {
            _ajustes = ajustes.Value;
            _encryptor = encryptor;
            _sesion = sesion;
            _usuario = usuario;
        }

        #region Login
        [HttpGet]
        public IActionResult Login(string? authUrl)
        {
            if (authUrl != null)
            {
                TempData["msgAlert"] = CodigoDeError.SesionCaducada.GetDescription();
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


            var validarUsuario = await _usuario.ValidaAcceso(credenciales);

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
                new(ClaimTypes.Email, validarUsuario.Data.Email),
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
            //    return RedirectToAction("CambiarPassword", "MiCuenta", new { area = "Home" });
            //}

            return RedirectToAction("Index", "Home", new { area = "Home" });
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

                //    return RedirectToAction("CambiarPassword", "MiCuenta", new { area = "Home" });
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
