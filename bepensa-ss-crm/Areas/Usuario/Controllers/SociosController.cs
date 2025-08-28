using bepensa_biz.Interfaces;
using bepensa_data.models;
using bepensa_models.CRM;
using bepensa_models.DataModels;
using bepensa_models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bepensa_ss_crm.Areas.Usuario.Controllers
{
    [Area("Usuario")]
    [Authorize]
    public class SociosController : Controller
    {
        private IAccessSession _sesion { get; set; }
        private readonly IDireccion _colonia;
        private readonly IUsuario _usuario;

        public SociosController(IUsuario usuario, IAccessSession sesion, IDireccion colonia)
        {
            _usuario = usuario;
            _sesion = sesion;
            _colonia = colonia;
        }

        [HttpGet("socios")]
        public IActionResult Index()
        {
            _sesion.UsuarioActual = null;

            return View();
        }

        [HttpPost("socios/buscar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Buscar(BuscarRequest pBuscar)
        {
            List<UsuarioDTO> model = [];

            var resultado = await _usuario.BuscarUsuario(pBuscar);

            if (resultado.Data != null && resultado.Data.Count > 0)
            {
                model = resultado.Data;
            }

            return PartialView("_sociosTable", model);
        }

        [HttpGet("socios/detalle/{idUsuario}")]
        public async Task<IActionResult> Socio(int idUsuario, string? msg)
        {
            if (_sesion.UsuarioActual == null || _sesion.UsuarioActual.Id != idUsuario || _sesion.ForzarCambio)
            {
                var resultado = await _usuario.BuscarUsuario(idUsuario);

                _sesion.UsuarioActual = resultado.Data;

                _sesion.ForzarCambio = false;
            }
            if (msg != null)
                TempData["SuccessMensaje"] = msg;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActualizarSocio(UsuarioRequest usuarioRequest)
        {

            var resultado = await _usuario.Actualizar(usuarioRequest, _sesion.OperadorActual.Id);

            _sesion.UsuarioActual = resultado.Data;

            //ViewBag.SuccessMensaje = resultado.Mensaje;

            return RedirectToAction("Socio", new { idUsuario = usuarioRequest.Id, msg = resultado.Mensaje });


            //return PartialView("_sociosTable", model);
        }
    }
}
