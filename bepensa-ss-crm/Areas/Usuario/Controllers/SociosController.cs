using bepensa_biz.Interfaces;
using bepensa_data.models;
using bepensa_models.CRM;
using bepensa_models.DataModels;
using bepensa_models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bepensa_ss_crm.Areas.Usuario.Controllers
{
    [Area("Usuario")]
    [Authorize]
    public class SociosController : Controller
    {
        private IAccessSession _sesion { get; set; }
        private readonly IDireccion _colonia;
        private readonly IUsuario _usuario;

        public SociosController(IUsuario usuario, IAccessSession sesion, IDireccion colonia)
        {
            _usuario = usuario;
            _sesion = sesion;
            _colonia = colonia;

            _sesion.SesionCRM = _sesion.SesionCRM ?? new SesionCRM();
        }

        [HttpGet("socios")]
        public IActionResult Index()
        {
            _sesion.SesionCRM = new SesionCRM();

            return View();
        }

        [HttpPost("socios/buscar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Buscar(BuscarRequest pBuscar)
        {
            List<UsuarioDTO> model = [];

            var resultado = await _usuario.BuscarUsuario(pBuscar);

            if (resultado.Data != null && resultado.Data.Count > 0)
            {
                model = resultado.Data;
            }

            return PartialView("_sociosTable", model);
        }

        [HttpGet("socios/buscar-socio/{idUsuario}")]
        public IActionResult Socio(int idUsuario)
        {
            if (_sesion.SesionCRM.Usuario == null || _sesion.SesionCRM.Usuario.Id != idUsuario)
            {
                var resultado = _usuario.ConsultarUsuario(idUsuario);

                _sesion.SesionCRM.Usuario = resultado.Data;
            }

            return View();
        }

        #region Dirección
        /// <summary>
        /// Consulta las colonias con base al código postal
        /// </summary>
        /// <param name="CP"></param>
        /// <returns>Lista de colonias</returns>
        [HttpGet("consulta/colonias/{CP}")]
        public async Task<JsonResult> ConsultarColonia(string CP)
        {
            var resultado = await _colonia.ConsultarColonias(CP);

            return Json(resultado);
        }

        [HttpGet("consulta/municipio/{idColonia}")]
        public async Task<JsonResult> ConsultarMunicipio(int idColonia)
        {
            var resultado = await _colonia.ConsultarMunicipio(idColonia);

            return Json(resultado);
        }

        [HttpGet("consulta/estado/{idMunicipio}")]
        public async Task<JsonResult> ConsultarEstado(int idMunicipio)
        {
            var resultado = await _colonia.ConsultarEstado(idMunicipio);

            return Json(resultado);
        }
        #endregion
    }
}
