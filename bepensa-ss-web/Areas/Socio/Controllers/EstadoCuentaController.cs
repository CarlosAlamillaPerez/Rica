using bepensa_biz.Interfaces;
using bepensa_data.models;
using bepensa_models.DataModels;
using bepensa_models.DTO;
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
        private readonly IEdoCta _edoCta;

        public EstadoCuentaController(IAccessSession sesion, IPeriodo periodo, IEdoCta edoCta)
        {
            _sesion = sesion;
            _periodo = periodo;
            _edoCta = edoCta;
        }

        [HttpGet("estado-de-cuenta")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("estado-de-cuenta/consultar/{idPeriodo}")]
        public async Task<JsonResult> ConsultarEdoCta(int idPeriodo)
        {
            var resultado = await _edoCta.ConsultarEstatdoCuenta(new UsuarioPeriodoRequest
            {
                IdUsuario = _sesion.UsuarioActual.Id,
                IdPeriodo = idPeriodo
            });

            return Json(resultado);
        }

        [HttpGet("estado-de-cuenta/consultar/canjes/{idPeriodo}")]
        [HttpGet("estado-de-cuenta/consultar/canjes")]
        public async Task<JsonResult> ConsultarCanjes(int? idPeriodo)
        {
            var resultado = await _edoCta.ConsultarCanjes(new UsuarioByEmptyPeriodoRequest
            {
                IdUsuario = _sesion.UsuarioActual.Id,
                IdPeriodo = idPeriodo
            });

            return Json(resultado);
        }

        #region Vistas Parciales
        [HttpPost]
        public IActionResult ConceptosAcumulacion([FromBody] List<AcumulacionEdoCtaDTO> resultado)
        {
            return PartialView("_conceptos", resultado);
        }

        [HttpPost]
        public IActionResult ListaCanjes([FromBody] List<DetalleCanjeDTO> resultado)
        {
            return PartialView("_verCanjes", resultado);
        }
        #endregion
    }
}
