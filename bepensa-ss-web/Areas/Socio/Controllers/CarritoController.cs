using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using bepensa_models.DTO;

namespace bepensa_ss_web.Areas.Socio.Controllers
{
    [Area("Socio")]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class CarritoController : Controller
    {
        [HttpGet("carrito")]
        public IActionResult Index()
        {
            return View(new CarritoDTO());
        }
    }
}
