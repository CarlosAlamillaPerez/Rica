using bepensa_biz.Interfaces;
using bepensa_data.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bepensa_ss_crm.Areas.Home.Controllers
{
    [Area("Home")]
    [Authorize]
    public class IniciosController : Controller
    {
        private readonly IAccessSession _sesion;
        private readonly IDireccion _colonia;

        public IniciosController(IAccessSession sesion, IDireccion colonia)
        {
            _sesion = sesion;
            _colonia = colonia;
        }

        [HttpGet("home")]
        public IActionResult Index()
        {
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
