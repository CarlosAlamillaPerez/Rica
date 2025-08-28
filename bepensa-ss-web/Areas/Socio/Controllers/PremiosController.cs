using bepensa_biz.Interfaces;
using bepensa_models.DataModels;
using bepensa_models.DTO;
using bepensa_ss_web.Filters;
using bepensa_web_common;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bepensa_ss_web.Areas.Socio.Controllers
{
    [Area("Socio")]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [ValidaSesionUsuario]
    public class PremiosController : Controller
    {
        private readonly IAccessSession _sesion;
        private readonly IPremio _premio;
        private readonly ICarrito _carrito;

        private readonly IAppCookies _appCookies;

        public PremiosController(IAppCookies appCookies, IAccessSession sesion, IPremio premio, ICarrito carrito)
        {
            _appCookies = appCookies;
            _sesion = sesion;
            _premio = premio;
            _carrito = carrito;
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
        public async Task<IActionResult> Premios(int pIdCategoriaDePremio)
        {

            var resultado = await _premio.ConsultarPremios(pIdCategoriaDePremio, _sesion.UsuarioActual.Id);

            var premios = resultado.Data ?? new List<PremioDTO>();

            if (_appCookies.Premio.Value != null)
            {
                ViewBag.PopupPremio = _appCookies.Premio.Value.Premio;

                _appCookies.Premio.Delete();
            }

            return View(premios);
        }

        [HttpGet("premios/detalle/{idProducto}")]
        public async Task<IActionResult> PremioBySku(int idProducto)
        {
            var resultado = await _premio.ConsultarPremioById(idProducto, _sesion.UsuarioActual.Id);

            return PartialView("_verProducto", resultado.Data ?? new());
        }

        [HttpPost("premios/agregar-premio")]
        public async Task<JsonResult> AgregarPremio([FromBody] AgregarPremioRequest pPremio)
        {
            pPremio.IdUsuario = _sesion.UsuarioActual.Id;

            var resultado = await _carrito.AgregarPremio(pPremio);

            return Json(resultado);
        }

        [HttpGet("catalogo/premios")]
        [AllowAnonymous]
        public IActionResult PremioByApp(int cid, int pid, int tid)
        {
            _appCookies.Premio.Value = new bepensa_web_common.Data.PremioCookie()
            {
                Cat = cid,
                Premio = pid
            };

            if (_sesion.UsuarioActual == null)
            {

                return RedirectToAction("Login", "Cuentas", new { area = "Autenticacion" });
            }
            else
            {
                return RedirectToAction("Premios", "Premios", new { area = "Socio", pIdCategoriaDePremio = cid });
            }
        }
    }
}
