using bepensa_biz.Interfaces;
using bepensa_biz.Settings;
using bepensa_data.models;
using bepensa_models.DataModels;
using bepensa_models.DTO;
using bepensa_ss_op_web.Filters;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace bepensa_ss_op_web.Areas.Socio.Controllers
{
    [Area("Socio")]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [ValidaSesionUsuario]
    public class EstadoCuentaController : Controller
    {
        private readonly GlobalSettings _ajustes;

        private IAccessSession _sesion { get; set; }
        private readonly IPeriodo _periodo;
        private readonly IEdoCta _edoCta;
        private readonly IBitacoraEnvioCorreo _bitacoraEnvioCorreo;

        public EstadoCuentaController(IOptionsSnapshot<GlobalSettings> ajustes, IAccessSession sesion, IPeriodo periodo, IEdoCta edoCta, IBitacoraEnvioCorreo bitacoraEnvioCorreo)
        {
            _ajustes = ajustes.Value;
            _sesion = sesion;
            _periodo = periodo;
            _edoCta = edoCta;
            _bitacoraEnvioCorreo = bitacoraEnvioCorreo;
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

        [HttpGet("estado-de-cuenta/consultar/canje/{idCanje}")]
        public async Task<JsonResult> ConsultarCanje(long idCanje)
        {
            var resultado = await _edoCta.ConsultarCanje(new RequestByIdCanje
            {
                IdUsuario = _sesion.UsuarioActual.Id,
                IdCanje = idCanje
            });

            return Json(resultado);
        }

        #region Vistas Parciales (Components)
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

        [HttpPost]
        public IActionResult Canje([FromBody] DetalleCanjeDTO resultado)
        {
            return PartialView("_verCanje", resultado);
        }

        [HttpGet("docs/pdf/estado-de-cuenta/{pIdPeriodo}")]
        public IActionResult EstadoCuentaPDF(int pIdPeriodo)
        {
            if (_sesion.FuerzaVenta == null)
            {
                return RedirectToAction("Index", "Home", new { area = "Socio" });
            }

            var resultado = _bitacoraEnvioCorreo.ConsultarPlantilla("edo-cta-ss-op", _sesion.UsuarioActual.Id, pIdPeriodo);

            if (!resultado.Exitoso || resultado.Data == null)
            {
                TempData["msgError"] = resultado.Mensaje;

                return RedirectToAction("Index", "EstadoCuenta", new { area = "Socio" });
            }

            var html = resultado.Data.Html;

            ViewBag.HtmlContent = html.Replace("@RUTA", _ajustes.RutaLocalImg);

            return View();
        }
        #endregion
    }
}
