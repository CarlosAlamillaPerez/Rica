using bepensa_biz.Interfaces;
using bepensa_models.DataModels;
using bepensa_models.DTO;
using bepensa_models.Enums;
using bepensa_ss_crm.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bepensa_ss_crm.Areas.Usuario.Controllers
{
    [Area("Usuario")]
    [Authorize]
    [ValidaSesionUsuario]
    public class PremiosController : Controller
    {
        private readonly IAccessSession _sesion;
        private readonly IPremio _premio;
        private readonly ICarrito _carrito;

        public PremiosController(IAccessSession sesion, IPremio premio, ICarrito carrito)
        {
            _sesion = sesion;
            _premio = premio;
            _carrito = carrito;
        }

        [HttpGet("socios/premios")]
        public IActionResult Index()
        {
            List<CategoriaDePremioDTO> model = [];

            var resultado = _premio.ConsultarCategorias().Data;

            if (resultado != null)
            {
                model = resultado;
            }

            return View(model);
        }

        [HttpGet("socios/premios/{pIdCategoriaDePremio}/categoria")]
        public IActionResult Premios(int pIdCategoriaDePremio)
        {
            List<PremioDTO> premios = [];

            var resultado = _premio.ConsultarPremios(pIdCategoriaDePremio, _sesion.UsuarioActual.Id).Data;

            if (resultado != null)
            {
                premios = resultado;
            }

            return View(premios);
        }

        [HttpGet("socios/premios/detalle/{idProducto}")]
        public IActionResult PremioBySku(int idProducto)
        {
            var resultado = _premio.ConsultarPremioById(idProducto, _sesion.UsuarioActual.Id);

            return PartialView("_verProducto", resultado.Data ?? new());
        }

        [HttpPost("socios/premios/agregar-premio")]
        public async Task<JsonResult> AgregarPremio([FromBody] AgregarPremioRequest pPremio)
        {
            pPremio.IdUsuario = _sesion.UsuarioActual.Id;
            pPremio.IdOperador = _sesion.OperadorActual.Id;

            var resultado = await _carrito.AgregarPremio(pPremio, (int)TipoOrigen.CallCenter);

            return Json(resultado);
        }
    }
}
