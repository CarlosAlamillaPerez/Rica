using bepensa_biz.Interfaces;
using bepensa_data.models;
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
    public class ProgramaController : ControllerBase
    {
        private readonly IConsultasProgramasProxy _movementsApp;
        public ProgramaController(IConsultasProgramasProxy movementsApp)
        {
            _movementsApp = movementsApp;

        }

        // <summary>
        // Consulta puntos de usuario en mes y año
        // </summary>        
        [HttpPost("mecanica-de-acumulacion")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public Respuesta<List<SubConceptosDeAcumulacionDTO>> ConsultaMecanicas(RequestCliente data)
        {
            Respuesta<List<SubConceptosDeAcumulacionDTO>> resultado = _movementsApp.ConsultaMecanicas(data);
            return resultado;
        }

         //<summary>
         //Consulta puntos de usuario en mes y año
         //</summary>        
        [HttpPost("bonos-exclusivos")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public Respuesta<List<SubConceptosDeAcumulacionDTO>> ConsultaBonos(RequestCliente data)
        {
            Respuesta<List<SubConceptosDeAcumulacionDTO>> resultado = _movementsApp.ConsultaBonos(data);
            return resultado;
        }

        // <summary>
        // Consulta puntos de usuario en mes y año
        // </summary>        
        [HttpPost("canales-de-comunicacion")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public Respuesta<List<OrigeneDTOWA>> ConsultaCanalesComunicacion(RequestCliente data)
        {
            Respuesta<List<OrigeneDTOWA>> resultado = _movementsApp.ConsultaCanalesComunicacion(data);
            return resultado;
        }

        // <summary>
        // Consulta puntos de usuario en mes y año
        // </summary>        
        [HttpPost("vigencia-canjes")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public Respuesta<List<SubConceptosDeAcumulacionDTO>> ConsultaVigencia(RequestCliente data)
        {
            Respuesta<List<SubConceptosDeAcumulacionDTO>> resultado = _movementsApp.ConsultaVigencia(data);
            return resultado;
        }
    }
}