using bepensa_biz.Interfaces;
using bepensa_models.DataModels;
using bepensa_models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bepensa_ss_crm.Areas.Usuario.Controllers
{
    [Area("Usuario")]
    [Authorize]
    public class ObjetivosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private IAccessSession _sesion { get; set; }
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

        public IActionResult Ejecucion()
        {
            var resultado = _objetivo.ConsultarEjecucionTradicional(new RequestByIdUsuario { IdUsuario = _sesion.UsuarioActual.Id });

            return View(resultado.Data);
        }

        [HttpGet("objetivos/actividades-especiales")]
        public IActionResult ActividadEspecial()
        {
            var resultado = _objetivo.ConsultarCumplimientosDeEnfriador(new RequestByIdUsuario { IdUsuario = _sesion.UsuarioActual.Id });
            return View(resultado.Data);
        }

        [HttpGet("objetivos/cumplimiento-enfriador")]
        public IActionResult CumplimientoEnfriador()
        {
            if (_sesion.UsuarioActual.IdCanal != (int)TipoCanal.Comidas)
            {
                return RedirectToAction("Index", "Index", new { area = "Socio" });
            }
            var resultado = _objetivo.ConsultarCumplimientosDeEnfriador(new RequestByIdUsuario { IdUsuario = _sesion.UsuarioActual.Id });

            return View(resultado.Data);
        }
    }
}
