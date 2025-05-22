using bepensa_biz.Interfaces;
using bepensa_models.DataModels;
using bepensa_models.DTO;
using bepensa_models.Enums;
using bepensa_models.General;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace bepensa_ss_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EncuestasController : ControllerBase
    {
        private readonly IEncuesta _encuesta;
        public EncuestasController(IEncuesta encuesta)
        {
            _encuesta = encuesta;
        }

        [HttpGet("Consultar/{pIdUsuario}")]
        public ActionResult<Respuesta<List<BitacoraEncuestaDTO>>> ConsultarEncuestas(int pIdUsuario)
        {
            Respuesta<List<BitacoraEncuestaDTO>> resultado = new();

            try
            {
                resultado = _encuesta.ConsultarEncuestas(pIdUsuario);

                return Ok(resultado);
            }
            catch (Exception)
            {
                resultado.Exitoso = false;
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Data = null;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();

                return BadRequest(resultado);
            }
        }
    }
}
