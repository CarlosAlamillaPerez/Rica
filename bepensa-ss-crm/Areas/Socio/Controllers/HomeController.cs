using bepensa_biz.Interfaces;
using bepensa_models.DataModels;
using bepensa_models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bepensa_ss_crm.Areas.Socio.Controllers
{
    [Area("Socio")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IAccessSession _sesion;
        private readonly IUsuario _usuario;

        public HomeController(IAccessSession sesion, IUsuario usuario)
        {
            _sesion = sesion;
            _usuario = usuario;
        }

        [HttpPost("socios/desbloquear-usuario")]
        [ValidateAntiForgeryToken]
        public IActionResult Desbloquear(TextoRequest notas)
        {
            int idUsuario = _sesion.UsuarioActual.Id;

            int idOperador = _sesion.OperadorActual.Id;

            var resultado = _usuario.Desbloquear(idUsuario, idOperador, notas.Texto, (int)TipoOrigen.CallCenter);

            if (resultado.Exitoso)
            {
                TempData["SuccessMensaje"] = resultado.Mensaje;
            }
            else
            {
                TempData["ErrorMensaje"] = resultado.Mensaje;
            }

            _sesion.ForzarCambio = true;

            return RedirectToAction("Socio", "Socios", new { area = "Usuario", idUsuario = idUsuario });
        }
    }
}
