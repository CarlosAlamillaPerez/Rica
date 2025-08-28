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
    public class SugerenciasController : ControllerBase
    {
        private readonly IConsultasSugerenciaProxy _movementsApp;
        public SugerenciasController(IConsultasSugerenciaProxy movementsApp)
        {
            _movementsApp = movementsApp;

        }

        // <summary>
        // Consulta puntos de usuario en mes y año
        // </summary>        
        [HttpPost("sugerencias")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public Respuesta<ResponseSugerencia> RegistraSugerencia(RequestClienteSugerencia data)
        {
            Respuesta<ResponseSugerencia> resultado = _movementsApp.RegistraSugerencia(data);
            return resultado;
        }
    }
}
