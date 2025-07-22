using bepensa_biz.Interfaces;
using bepensa_models.Enums;
using bepensa_models.General;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace bepensa_ss_api.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/[controller]")]
    [ApiController]
    public class OperacionesController : ControllerBase
    {
        private readonly IOperacion _operacion;

        public OperacionesController(IOperacion operacion)
        {
            _operacion = operacion;
        }

        [HttpGet("ActualizarEstatusRedenciones")]
        public async Task<ActionResult<Respuesta<Empty>>> ActualizarEstatusRedenciones()
        {
            Respuesta<Empty> resultado = new();

            try
            {
                resultado = await _operacion.ActualizarEstatusRedenciones();

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
