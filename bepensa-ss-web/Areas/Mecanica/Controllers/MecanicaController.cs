using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bepensa_ss_web.Areas.Mecanica.Controllers
{
    [Area("Mecanica")]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class MecanicaController : Controller
    {
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

        [HttpGet("mecanica/promociones")]
        public IActionResult Promociones()
        {
            return View();
        }

        [HttpGet("mecanica/foto-de-exito")]
        public IActionResult FotoExito()
        {
            return View();
        }

        [HttpGet("mecanica/actividades-especiales")]
        public IActionResult ActividadesEspeciales()
        {
            return View();
        }

        [HttpGet("mecanica/bonos")]
        public IActionResult Bonos()
        {
            return View();
        }
    }
}
