using bepensa_biz.Interfaces;
using bepensa_biz.Settings;
using bepensa_models.DTO;
using bepensa_models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace bepensa_ss_op_web.Controllers
{
    public class ActivaController : Controller
    {
        private readonly GlobalSettings _ajustes;
        private readonly IAppEmail appEmail;
        private readonly IUsuario _usuario;
        private readonly IBitacoraEnvioCorreo _bitacoraEnvioCorreo;

        public ActivaController(IOptionsSnapshot<GlobalSettings> ajustes, IAppEmail appEmail, IUsuario usuario, IBitacoraEnvioCorreo bitacoraEnvioCorreo)
        {
            _ajustes = ajustes.Value;
            this.appEmail = appEmail;
            _usuario = usuario;
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
                TempData["msgError"] = CodigoDeError.ErrorLigaRecPass.GetDescription();

                return RedirectToAction("Login", "Cuentas", new { area = "Autenticacion" });
            }

            var validaToken = _bitacoraEnvioCorreo.ConsultaByToken(token);

            if (validaToken.Exitoso && validaToken.Data != null)
            {
                double minutos = (fechaAcceso - validaToken.Data.FechaEnvio).TotalMinutes;

                double minutosexpira = _ajustes.RecuperacionPassword.Expiracion;

                if (minutos > minutosexpira)
                {
                    TempData["msgError"] = CodigoDeError.ExpiradaLigaRecPass.GetDescription();

                    return RedirectToAction("Login", "Cuentas", new { area = "Autenticacion" });
                }
            }
            else
            {
                TempData["msgError"] = validaToken.Mensaje;

                return RedirectToAction("Login", "Cuentas", new { area = "Autenticacion" });
            }

            return View(datos);
        }

        [HttpGet("restablecer-contrasena")]
        public IActionResult RestablecerPassword(CambiarPasswordRequest datos)
        {
            if (datos.Token == null)
            {
                TempData["msgError"] = CodigoDeError.ExpiradaLigaRecPass.GetDescription();

                return RedirectToAction("Login", "Cuentas", new { area = "Autenticacion" });
            }

            return View(datos);
        }

        [HttpPost("restablecer-contrasena")]
        [ValidateAntiForgeryToken]
        public IActionResult CambiarPassword(CambiarPasswordRequest datos)
        {
            var valida = _usuario.CambiarContraseniaByToken(datos);

            if (!valida.Exitoso)
            {
                TempData["msgError"] = valida.Mensaje;

                return View("RestablecerPassword", datos);
            }
            
            TempData["msgSuccess"] = valida.Mensaje;

            return RedirectToAction("Login", "Cuentas", new { area = "Autenticacion" });
        }

        [HttpGet("/{clave}")]
        public IActionResult RedirectShortUrl(string clave)
        {
            var resultado = appEmail.ObtenerUrlOriginal(clave);

            if (!resultado.Exitoso || resultado.Data == null)
            {
                TempData["msgError"] = resultado.Mensaje;

                return RedirectToAction("Login", "Cuentas", new { area = "Autenticacion" });
            }

            return Redirect(resultado.Data);
        }
    }
}
