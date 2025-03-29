using bepensa_biz.Interfaces;
using bepensa_biz.Settings;
using bepensa_models.DTO;
using bepensa_models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace bepensa_ss_web.Controllers
{
    public class ActivaController : Controller
    {
        private readonly GlobalSettings _ajustes;
        private IAccessSession _sesion { get; set; }
        private readonly IAppEmail appEmail;
        private readonly IBitacoraEnvioCorreo _bitacoraEnvioCorreo;

        public ActivaController(IOptionsSnapshot<GlobalSettings> ajustes, IAccessSession sesion, IAppEmail appEmail, IBitacoraEnvioCorreo bitacoraEnvioCorreo)
        {
            _ajustes = ajustes.Value;
            _sesion = sesion;
            this.appEmail = appEmail;
            _bitacoraEnvioCorreo = bitacoraEnvioCorreo;
        }

        public IActionResult Lectura(Guid? token)
        {
            appEmail.Lectura(token);

            return Ok();
        }

        [HttpGet("/restablecer-contrasena/{token}")]
        public IActionResult RestablecerPassword(Guid? token)
        {
            DateTime fechaAcceso = DateTime.Now;

            CambiarPasswordRequest datos = new()
            {
                Token = token
            };

            if (token == null)
            {
                _sesion.SetSesion("msgError", CodigoDeError.ErrorLigaRecPass.GetDescription());

                return RedirectToAction("Login", "Cuentas", new { area = "Autenticacion" });
            }

            var validaToken = _bitacoraEnvioCorreo.ConsultaByToken(token);

            if (validaToken.Exitoso && validaToken.Data != null)
            {
                double minutos = (fechaAcceso - validaToken.Data.FechaEnvio).TotalMinutes;

                double minutosexpira = _ajustes.RecuperacionPassword.Expiracion;

                if (minutos > minutosexpira)
                {
                    _sesion.SetSesion("msgError", CodigoDeError.ExpiradaLigaRecPass.GetDescription());

                    return RedirectToAction("Login", "Cuentas", new { area = "Autenticacion" });
                }
            }
            else
            {
                _sesion.SetSesion("msgError", validaToken.Mensaje);

                return RedirectToAction("Login", "Cuentas", new { area = "Autenticacion" });
            }

            return View(datos);
        }

        [HttpGet("restablecer-contrasena")]
        public IActionResult RestablecerPassword(CambiarPasswordRequest datos)
        {
            if (datos.Token == null)
            {
                _sesion.SetSesion("msgError", CodigoDeError.ExpiradaLigaRecPass.GetDescription());

                return RedirectToAction("Login", "Cuentas", new { area = "Autenticacion" });
            }

            return View(datos);
        }
    }
}
