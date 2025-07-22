﻿using bepensa_biz.Interfaces;
using bepensa_models.DataModels;
using bepensa_models.DTO;
using bepensa_ss_crm.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bepensa_ss_crm.Areas.Usuario.Controllers
{
    [Area("Usuario")]
    [Authorize]
    [ValidaSesionUsuario]
    public class EstadoCuentaController : Controller
    {
        private IAccessSession _sesion { get; set; }

        private readonly IUsuario _usuario;

        private readonly IEdoCta _edoCta;

        public EstadoCuentaController(IAccessSession sesion, IUsuario usuario, IEdoCta edoCta)
        {
            _sesion = sesion;
            _usuario = usuario;
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
        #endregion
    }
}
