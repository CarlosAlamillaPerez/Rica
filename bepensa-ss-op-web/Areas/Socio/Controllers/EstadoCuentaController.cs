using bepensa_biz.Interfaces;
using bepensa_models.DataModels;
using bepensa_models.DTO;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bepensa_ss_op_web.Areas.Socio.Controllers
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
            var resultado = _periodo.ConsultarPeriodos().Data?.OrderBy(x => x.Id);

            return View(resultado?.ToList());
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

        [HttpGet("estado-de-cuenta/consultar/{idPeriodo}/canjes")]
        public async Task<JsonResult> ConsultarCanjes(int idPeriodo)
        {
            var resultado = await _edoCta.ConsultarCanjes(new UsuarioPeriodoRequest
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
        #endregion
    }
}
