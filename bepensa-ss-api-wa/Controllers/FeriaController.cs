using bepensa_biz.Interfaces;
using bepensa_models.ApiWa;
using bepensa_models.General;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bepensa_ss_api_wa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]    
    public class FeriaController : ControllerBase
    {
        private readonly IConsultasFeriaProxy _movementsApp;
        public FeriaController(IConsultasFeriaProxy movementsApp)
        {
            _movementsApp = movementsApp;

        }

        /// <summary>
        /// Consulta puntos de usuario en mes y año
        /// </summary>        
        //[HttpPost("ferias")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //public Respuesta<ResponseFerias> ConsultaCliente(RequestCliente data)
        //{
        //    Respuesta<ResponseFerias> resultado = _movementsApp.ConsultaCliente(data);
        //    return resultado;
        //}
    }
}
