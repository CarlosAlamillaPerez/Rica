using bepensa_biz.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bepensa_ss_web.Areas.Home.Controllers
{
    [Area("Home")]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class HomeController : Controller
    {
        private IAccessSession _session { get; set; }

        public HomeController(IAccessSession session)
        {
            _session = session;
        }

        [HttpGet("home")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
