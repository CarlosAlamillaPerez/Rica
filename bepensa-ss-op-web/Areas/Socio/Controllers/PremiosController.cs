using bepensa_biz.Interfaces;
using bepensa_models.DTO;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bepensa_ss_op_web.Areas.Socio.Controllers
{
    [Area("Socio")]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class PremiosController : Controller
    {
        private readonly IAccessSession _sesion;
        private readonly IPremio _premio;

        public PremiosController(IAccessSession sesion, IPremio premio)
        {
            _sesion = sesion;
            _premio = premio;
        }

        [HttpGet("premios")]
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

        [HttpGet("premios/{pIdCategoriaDePremio}/categoria")]
        public IActionResult Premios(int pIdCategoriaDePremio)
        {
            List<PremioDTO> premios = [];

            var resultado = _premio.ConsultarPremios(pIdCategoriaDePremio).Data;

            if (resultado != null)
            {
                premios = resultado;
            }

            return View(premios);
        }

        [HttpGet("premios/detalle/{idProducto}")]
        public IActionResult PremioBySku(int idProducto)
        {
            var resultado = _premio.ConsultarPremioById(idProducto);

            return PartialView("_verProducto", resultado.Data ?? new());
        }
    }
}
