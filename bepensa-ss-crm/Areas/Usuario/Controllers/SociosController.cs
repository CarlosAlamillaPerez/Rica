using bepensa_biz.Interfaces;
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
        private readonly IUsuario _usuario;

        public SociosController(IUsuario usuario, IAccessSession sesion)
        {
            _usuario = usuario;
            _sesion = sesion;

            _sesion.SesionCRM = _sesion.SesionCRM ?? new SesionCRM();
        }

        [HttpGet("socios")]
        public IActionResult Index()
        {
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

        [HttpGet("socios/buscar-socio/{idUsuario}")]
        public async Task<IActionResult> Socio(int idUsuario)
        {
            if (_sesion.SesionCRM.Usuario == null)
            {
                var resultado = await _usuario.BuscarUsuario(idUsuario);

                _sesion.SesionCRM.Usuario = resultado.Data;
            }

            return View();
        }
    }
}
