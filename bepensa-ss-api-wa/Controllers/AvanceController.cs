using bepensa_biz.Interfaces;
using bepensa_models.ApiWa;
using bepensa_models.DTO;
using bepensa_models.General;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bepensa_ss_api_wa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[ApiController]
    public class AvanceController : ControllerBase
    {
        private readonly IConsultasAvanceProxy _movementsApp;
        public AvanceController(IConsultasAvanceProxy movementsApp)
        {
            _movementsApp = movementsApp;

        }

        /// <summary>
        /// Consulta puntos de usuario en mes y año
        /// </summary>        
        [HttpPost("avances")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public Respuesta<AvanceWa> ConsultaAvance(RequestCliente data)
        {
            Respuesta<AvanceWa> resultado = _movementsApp.ConsultaAvance(data);
            return resultado;
        }
    }
}
