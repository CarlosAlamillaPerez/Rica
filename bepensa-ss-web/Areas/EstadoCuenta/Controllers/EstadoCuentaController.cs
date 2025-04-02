using Microsoft.AspNetCore.Mvc;

namespace bepensa_ss_web.Areas.EstadoCuenta.Controllers
{
    [Area("EstadoCuenta")]
    public class EstadoCuentaController : Controller
    {
        [HttpGet("estado-de-cuenta")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
