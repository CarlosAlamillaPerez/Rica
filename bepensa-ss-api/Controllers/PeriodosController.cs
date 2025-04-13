using bepensa_biz.Interfaces;
using bepensa_models.App;
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
    public class PeriodosController : ControllerBase
    {
        private readonly IPeriodo _periodo;

        public PeriodosController(IPeriodo periodo)
        {
            _periodo = periodo;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("Consultar/PeriodosActivos")]
        public ActionResult<Respuesta<List<PeriodoDTO>>> ConsultarPeriodos()
        {
            Respuesta<List<PeriodoDTO>> resultado = new();

            try
            {
                resultado = _periodo.ConsultarPeriodos();

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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("Consultar/PeriodosEstadoCuenta/{idUsuario}")]
        public ActionResult<Respuesta<List<PeriodoDTO>>> ConsultarPeriodos(int idUsuario)
        {
            Respuesta<List<PeriodoDTO>> resultado = new();

            try
            {
                resultado = _periodo.PeriodosEdoCta(idUsuario);

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
