using bepensa_biz;
using bepensa_biz.Interfaces;
using bepensa_ss_web.Filters;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bepensa_ss_web.Areas.Socio.Controllers
{
    [Area("Socio")]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [ValidaSesionUsuario]
    public class PromocionesController : Controller
    {
        private readonly IAccessSession _sesion;
        private readonly IApp _app;

        public PromocionesController(IAccessSession sesion, IApp app)
        {
            _sesion = sesion;
            _app = app;
        }

        [HttpGet("promociones")]
        public async Task<IActionResult> Index()
        {
            var resultado = await _app.ConsultaImgPromociones(_sesion.UsuarioActual.IdCanal);

            return View(resultado.Data);
        }
    }
}
