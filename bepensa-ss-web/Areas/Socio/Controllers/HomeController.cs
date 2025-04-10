using bepensa_biz.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bepensa_ss_web.Areas.Socio.Controllers
{
    [Area("Socio")]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class HomeController : Controller
    {
        private IAccessSession _session { get; set; }
        private readonly IObjetivo _objetivo;

        public HomeController(IAccessSession session, IObjetivo objetivo)
        {
            _session = session;
            _objetivo = objetivo;
        }

        [HttpGet("home")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("mi-cuenta")]
        public IActionResult MiCuenta()
        {
            return View(_session.UsuarioActual);
        }
    }
}
