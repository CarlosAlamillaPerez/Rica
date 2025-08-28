using bepensa_biz.Interfaces;
using bepensa_models.ApiWa;
using bepensa_models.General;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bepensard_ss_api_wa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IConsultasClienteProxy _movementsApp;
        public ClienteController(IConsultasClienteProxy movementsApp)
        {
            _movementsApp = movementsApp;

        }

        /// <summary>
        /// Consulta puntos de usuario en mes y año
        /// </summary>        
        [HttpPost("Cliente")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public Respuesta<ResponseCliente> ConsultaCliente(RequestCliente data)
        {
            Respuesta<ResponseCliente> resultado = _movementsApp.ConsultaCliente(data);
            return resultado;
        }

        /// <summary>
        /// Consulta puntos de usuario en mes y año
        /// </summary>        
        [HttpPost("puntos")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public Respuesta<ResponsePuntos> ConsultaPuntos(RequestClientePeriodo data)
        {
            Respuesta<ResponsePuntos> resultado = _movementsApp.ConsultaPuntos(data);
            return resultado;
        }
    }
}
