using bepensa_biz.Interfaces;
using bepensa_models.DTO;
using bepensa_ss_inscripcion.Areas.Inscripcion.Filters;
using Microsoft.AspNetCore.Mvc;

namespace bepensa_ss_inscripcion.Areas.Inscripcion.Controllers
{
    [Area("Inscripcion")]
    [ValidaSesionInscripcion]
    public class RegistrosController : Controller
    {
        private readonly IAccessSession _sesion;
        private readonly IInscripcion _inscripcion;
        private readonly IDireccion _colonia;

        public RegistrosController(IAccessSession sesion, IInscripcion inscripcion, IDireccion colonia)
        {
            _sesion = sesion;
            _inscripcion = inscripcion;
            _colonia = colonia;
        }

        [HttpGet("inscripcion")]
        public IActionResult Inscripcion()
        {
            var pInscripcion = _sesion.CredencialesInscripcion.Inscripcion;

            if (pInscripcion != null && pInscripcion.Inscripcion)
            {
                ViewData["msgSuccess"] = "FORMATO COMPLETADO";
            }

            return View(pInscripcion);
        }

        [HttpPost("inscripcion")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Inscripcion(InscripcionDTO pInscripcion)
        {
            var registrar = await _inscripcion.Registrar(pInscripcion);

            if (!registrar.Exitoso)
            {
                ViewData["msgError"] = registrar.Mensaje;
            }
            else
            {
                ViewData["msgSuccess"] = "FORMATO COMPLETADO";

                var model = _sesion.CredencialesInscripcion;

                model.Inscripcion = pInscripcion;
                model.Inscripcion.Inscripcion = true;

                _sesion.CredencialesInscripcion = model;

                return RedirectToAction("Inscripcion", "Registros", new { area = "Inscripcion" });
            }

            return View(pInscripcion);
        }

        #region Dirección
        /// <summary>
        /// Consulta las colonias con baser al código postal
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

        [HttpGet("inscripcion/salir")]
        public IActionResult Salir()
        {
            return RedirectToAction("Login", "Inscripciones", new { area = "Autenticacion" });
        }
    }
}
