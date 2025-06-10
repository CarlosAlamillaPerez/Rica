using bepensa_biz.Interfaces;
using bepensa_models.App;
using bepensa_models.General;
using bepensa_ss_crm.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace bepensa_ss_crm.Areas.Usuario.Controllers
{
    [Area("Usuario")]
    [Authorize]
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
            Respuesta<List<ImagenesPromocionesDTO>> resultado = new();

            resultado = await _app.ConsultaImgPromociones(_sesion.UsuarioActual.IdCanal);

            return View(resultado.Data);
        }
    }
}
