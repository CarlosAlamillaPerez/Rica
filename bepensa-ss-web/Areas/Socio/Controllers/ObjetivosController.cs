using bepensa_biz.Interfaces;
using bepensa_biz;
using bepensa_models.DataModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using bepensa_models.Enums;
using System.Collections.Generic;
using bepensa_ss_web.Filters;
using bepensa_models.DTO;


namespace bepensa_ss_web.Areas.Socio.Controllers
{
    [Area("Socio")]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [ValidaSesionUsuario]
    public class ObjetivosController : Controller
    {
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
        public IActionResult FotoExito()
        {
            var resultado = _objetivo.ConsultarCumplimientoFotoExito(new UsuarioByEmptyPeriodoRequest
            {
                IdUsuario = _sesion.UsuarioActual.Id
            });

            List<PeriodosEmpaquesDTO> model = resultado.Data?.OrderByDescending(x => x.IdPeriodo).Take(6).ToList() ?? new List<PeriodosEmpaquesDTO>();


            return View(model.OrderBy(x => x.IdPeriodo).ToList());
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
