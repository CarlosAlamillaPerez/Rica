using bepensa_biz.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bepensa_ss_crm.Areas.Usuario.Controllers
{
    [Area("Usuario")]
    [Authorize]
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
