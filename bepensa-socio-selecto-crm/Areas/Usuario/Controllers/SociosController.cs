using bepensa_socio_selecto_biz.Interfaces;
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
        private readonly IUsuario _usuario;

        public SociosController(IUsuario usuario)
        {
            _usuario = usuario;
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
    }
}
