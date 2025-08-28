using bepensa_biz.Interfaces;
using bepensa_data.models;
using bepensa_models.ApiWa;
using bepensa_models.DataModels;
using bepensa_models.Enums;
using bepensa_models.General;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bepensa_ss_api_wa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[ApiController]
    public class CuentaController : ControllerBase
    {
        private readonly IConsultasCuentaProxy _movementsApp;
        private readonly IUsuario _usuario;
        public CuentaController(IConsultasCuentaProxy movementsApp, IUsuario usuarios)
        {
            _movementsApp = movementsApp;
            _usuario = usuarios;

        }

        /// <summary>
        /// Consulta de avisos de privacidad
        /// </summary>        
        [HttpPost("aviso-de-privacidad")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public Respuesta<ResponseCuentaPrivacidad> ConsultaCliente(RequestCliente data)
        {
            Respuesta<ResponseCuentaPrivacidad> resultado = _movementsApp.ConsultaCliente(data);
            return resultado;
        }

        /// <summary>
        /// Consulta puntos de usuario en mes y año
        /// </summary>        
        [HttpPost("recupera-contrasenia")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<Respuesta<Empty>>> RecuperarPassword(RequestRecuperaPass data)
        {
            Respuesta<Empty> resultado = new();

            try
            {
                var task = await _movementsApp.RecuperarPassword(data);

                if (!task.Exitoso)
                {
                    resultado.Exitoso = false;
                    resultado.Codigo = task.Codigo;
                    resultado.Mensaje = task.Mensaje;
                    return BadRequest(resultado);
                }

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                resultado.Exitoso = false;
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = ex.Message;
                return BadRequest(resultado);
            }
        }
    }
}
