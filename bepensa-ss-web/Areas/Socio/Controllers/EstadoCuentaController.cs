using bepensa_biz.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bepensa_ss_web.Areas.Socio.Controllers
{
    [Area("Socio")]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class EstadoCuentaController : Controller
    {
        private IAccessSession _sesion { get; set; }
        private readonly IPeriodo _periodo;

        public EstadoCuentaController(IAccessSession sesion, IPeriodo periodo)
        {
            _sesion = sesion;
            _periodo = periodo;
        }

        [HttpGet("estado-de-cuenta")]
        public IActionResult Index()
        {
            var resultado = _periodo.ConsultarPeriodos().Data?.OrderBy(x => x.Id);

            return View(resultado?.ToList());
        }
    }
}
