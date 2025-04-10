using bepensa_biz.Interfaces;
using bepensa_biz;
using bepensa_models.DataModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace bepensa_ss_web.Areas.Socio.Controllers
{
    [Area("Socio")]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class ObjetivosController : Controller
    {
        private readonly IAccessSession _sesion;
        private readonly IApp _app;
        private readonly IObjetivo _objetivo;

        public ObjetivosController(IAccessSession sesion, IApp app, IObjetivo objetivo)
        {
            _sesion = sesion;
            _app = app;
            _objetivo = objetivo;
        }

        [HttpGet("objetivos/meta-de-compra")]
        public IActionResult MetaCompra()
        {
            var resultado = _objetivo.ConsultarMetasMensuales(new RequestByIdUsuario { IdUsuario = _sesion.UsuarioActual.Id });

            return View(resultado.Data);
        }

        [HttpGet("objetivos/portafolio-prioritario")]
        public IActionResult PortafolioPrioritario()
        {
            var resultado = _objetivo.ConsultarPortafoliosPrioritarios(new RequestByIdUsuario { IdUsuario = _sesion.UsuarioActual.Id });

            return View(resultado.Data);
        }

        [HttpGet("objetivos/foto-de-exito")]
        public async Task<IActionResult> FotoExito()
        {
            var resultado = await _app.ConsultaImgPromociones(_sesion.UsuarioActual.IdCanal);

            return View(resultado.Data);
        }
    }
}
