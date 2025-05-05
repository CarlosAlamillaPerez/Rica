using bepensa_biz.Interfaces;
using bepensa_models.Enums;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bepensa_ss_op_web.Areas.Mecanica.Controllers
{
    [Area("Mecanica")]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class MecanicaController : Controller
    {
        private readonly IAccessSession _sesion;

        public MecanicaController(IAccessSession sesion)
        {
            _sesion = sesion;
        }

        [HttpGet("mecanica/meta-compra")]
        public IActionResult MetaCompra()
        {
            return View();
        }

        [HttpGet("mecanica/ejecucion-de-mercadeo")]
        public IActionResult EjecucionMercadeo()
        {
            return View();
        }

        [HttpGet("mecanica/portafolio")]
        public IActionResult Portafolio()
        {
            return View();
        }

        [HttpGet("mecanica/enfriador")]
        public IActionResult Enfriador()
        {
            return View();
        }

        [HttpGet("mecanica/bonos")]
        public IActionResult Bonos()
        {
            return View();
        }

        [HttpGet("mecanica/compra-por-app")]
        public IActionResult CompraPorApp()
        {
            return View();
        }
    }
}
