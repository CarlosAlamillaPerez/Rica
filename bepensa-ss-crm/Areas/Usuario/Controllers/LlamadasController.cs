using bepensa_biz.Interfaces;
using bepensa_models.DataModels;
using bepensa_ss_crm.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bepensa_ss_crm.Areas.Usuario.Controllers
{
    [Area("Usuario")]
    [Authorize]
    [ValidaSesionUsuario]
    public class LlamadasController : Controller
    {
        private IAccessSession _sesion { get; set; }

        private readonly IDropDownList _listas;
        private readonly ILlamada _llamada;

        public LlamadasController(IAccessSession sesion, ILlamada llamada, IDropDownList listas)
        {
            _sesion = sesion;
            _llamada = llamada;
            _listas = listas;
        }

        [HttpGet("llamadas")]
        public IActionResult Index()
        {
            return View(new LlamadaRequest());
        }

        [HttpGet("llamadas/historico")]
        public IActionResult Historico()
        {
            var resultado = _llamada.ConsultarLlamadas(_sesion.UsuarioActual.Id);

            return View(resultado.Data);
        }

        public IActionResult CapturaRapida()
        {
            return PartialView("_llamadaModal");
        }

        [HttpGet("llamadas/{id}/subcategorias-llamada")]
        public JsonResult Subcategoria(int id)
        {
            var resultado = _listas.SubcategoriasLlamada(id);

            return Json(resultado);
        }

        [HttpPost("llamadas/registrar-llamada")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> RegistrarLlamada(LlamadaRequest pLlamada)
        {
            pLlamada.IdOperador = _sesion.OperadorActual.Id;

            var resultado = await _llamada.RegistrarLlamada(pLlamada);

            return Json(resultado);
        }

        [HttpGet("llamadas/{id}/seguimiento-llamada")]
        public JsonResult SeguimientoLlamada(int id)
        {
            var resultado = _listas.SuperLlamadas(id);

            return Json(resultado);
        }
    }
}
