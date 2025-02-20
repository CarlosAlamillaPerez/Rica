using bepensa_biz.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bepensa_ss_crm.Areas.Home.Controllers
{
    [Area("Home")]
    [Authorize]
    public class IniciosController : Controller
    {
        private readonly IAccessSession _sesion;

        public IniciosController(IAccessSession sesion)
        {
            _sesion = sesion;
        }

        [HttpGet("home")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
