using bepensa_biz.Interfaces;
using bepensa_models.DataModels;
using bepensa_ss_op_web.Areas.FuerzaVenta.Filters;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bepensa_ss_op_web.Areas.FuerzaVenta.Controllers
{
    [Area("FuerzaVenta")]
    [Route("fuerza-de-venta")]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [ValidaSesionFDV]
    public class HomeController : Controller
    {
        private IAccessSession _sesion { get; set; }
        private readonly IFuerzaVenta _fdv;

        public HomeController(IAccessSession sesion, IFuerzaVenta fdv)
        {
            _sesion = sesion;
            _fdv = fdv;
        }

        [HttpGet("home")]
        public IActionResult Index()
        {
            return View(new BuscarFDVRequest());
        }

        [HttpGet("buscar-usuario/{idUsuario}")]
        public async Task<IActionResult> ConsultarUsuario(int idUsuario)
        {

            var resultado = await _fdv.ConsultarUsuario(idUsuario, _sesion.FuerzaVenta.IdCanal);

            if (!resultado.Exitoso || resultado.Data == null)
            {
                ViewData["msgError"] = resultado.Mensaje;

                return RedirectToAction("Index", "Home", new { area = "FuerzaVenta" });
            }

            _sesion.UsuarioActual = resultado.Data;

            return RedirectToAction("Index", "Home", new { area = "Socio" });
        }

        [HttpPost("consultar-clientes")]
        public async Task<JsonResult> ConsultarUsuarios(BuscarFDVRequest pFDV)
        {
            pFDV.IdFDV = _sesion.FuerzaVenta.Id;

            var resultado = await _fdv.ConsultarUsuarios(pFDV);

            return Json(resultado);
        }
    }
}
