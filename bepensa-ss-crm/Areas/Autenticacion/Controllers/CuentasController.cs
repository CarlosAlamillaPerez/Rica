using bepensa_biz.Interfaces;
using bepensa_biz.Settings;
using bepensa_models.CRM;
using bepensa_models.Enums;
using bepensa_models.General;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using bepensa_models.DataModels;
using bepensa_models.DTO;

namespace bepensa_ss_crm.Areas.Autenticacion.Controllers
{
    [Area("Autenticacion")]
    public class CuentasController : Controller
    {
        private readonly GlobalSettings _ajustes;
        private IAccessSession _sesion { get; set; }
        private readonly IOperador _operador;

        public CuentasController(IOptionsSnapshot<GlobalSettings> ajustes, IAccessSession sesion, IOperador operador)
        {
            _ajustes = ajustes.Value;
            _sesion = sesion;
            _operador = operador;
            _operador = operador;
        }

        [HttpGet]
        public IActionResult Login(string? authUrl)
        {
            if (authUrl != null)
            {
                TempData["msgAlert"] = CodigoDeError.SesionCaducada.GetDescription();
            }

            _sesion.CredencialesCRM = new LoginCRMRequest();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginCRMRequest pCredenciales)
        {
            DateTime fechaAcceso = DateTime.Now;

            var ctrAcceso = _sesion.CredencialesCRM;

            ctrAcceso.Email = pCredenciales.Email;
            ctrAcceso.Password = pCredenciales.Password;

            if (string.IsNullOrEmpty(ctrAcceso.ControlAcceso.Usuario))
            {
                ctrAcceso.ControlAcceso.Usuario = pCredenciales.Email;
            }
            else
            {
                if (ctrAcceso.ControlAcceso.Usuario == pCredenciales.Email)
                {
                    if (ctrAcceso.ControlAcceso.Intentos >= _ajustes.Autenticacion.Intentos)
                    {
                        if (ctrAcceso.ControlAcceso.Bloqueado == false)
                        {
                            var bloquear = await _operador.BloquearOperador(new EmailRequest { Email = pCredenciales.Email });

                            ctrAcceso.ControlAcceso.Bloqueado = bloquear.Exitoso;
                        }

                        ctrAcceso.ControlAcceso.TiempoDesbloqueo = (fechaAcceso - ctrAcceso.ControlAcceso.FechaAcceso).TotalMinutes;

                        TempData["msgError"] = CodigoDeError.IntentoDeSesion.GetDescription();

                        _sesion.CredencialesCRM = ctrAcceso;

                        return View(pCredenciales);
                    }
                }
                else
                {
                    ctrAcceso.ControlAcceso.Intentos = 0;
                    ctrAcceso.ControlAcceso.FechaAcceso = DateTime.Now;
                    ctrAcceso.ControlAcceso.TiempoDesbloqueo = 0;
                    ctrAcceso.ControlAcceso.CambiaPassword = false;
                    ctrAcceso.ControlAcceso.Usuario = pCredenciales.Email;
                }
            }

            var validarOperador = await _operador.ValidaAcceso(pCredenciales);

            if (!validarOperador.Exitoso || validarOperador.Data == null)
            {
                ctrAcceso.ControlAcceso.Intentos++;

                TempData["msgError"] = validarOperador.Mensaje;

                _sesion.CredencialesCRM = ctrAcceso;

                return View(pCredenciales);
            }

            _sesion.OperadorActual = validarOperador.Data;

            string nombreCompleto = validarOperador.Data.Nombre + " " + validarOperador.Data.Apellidos;

            string iniciales = validarOperador.Data.Nombre.Substring(0, 1) + validarOperador.Data.Apellidos.Substring(0, 1);

            string sesionId = validarOperador.Data.SessionId != null ? validarOperador.Data.SessionId : new Guid().ToString();

            //Menú Dinámico
            int _idrol = validarOperador.Data.IdRol;

            Respuesta<List<SeccionDTO>> _respuestaMenu = await _operador.ConsultaMenuOperador(_idrol);

            _sesion.CrmMenuOperador = _respuestaMenu.Data == null ? new List<SeccionDTO>() : _respuestaMenu.Data;

            var claims = new List<Claim>
            {
                new(ClaimTypes.Email, validarOperador.Data.Email),
                new(ClaimTypes.Name, nombreCompleto ),
                new("Iniciales", iniciales ),
                new("IdRol", validarOperador.Data.IdRol.ToString()),
                new("Sesion", sesionId),
                new(ClaimTypes.Role, validarOperador.Data.Rol.Nombre),
                new(ClaimTypes.NameIdentifier, validarOperador.Data.Id.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var operadorPrincipal = new ClaimsPrincipal(identity);

            double expireTime = _ajustes.Autenticacion.Expiracion;

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, operadorPrincipal, new AuthenticationProperties
            {
                ExpiresUtc = DateTime.UtcNow.AddMinutes(expireTime)
            });

            return RedirectToAction("Index", "Inicios", new { area = "Home" });
        }

        #region Logout
        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                _sesion.Logout();

                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                foreach (var cookie in Request.Cookies.Keys)
                {
                    Response.Cookies.Delete(cookie);
                }
            }
            catch (Exception)
            {
                //colocar registro de logger
            }

            return RedirectToAction("Login", "Cuentas", new { area = "Autenticacion" });
        }
        #endregion
    }
}
