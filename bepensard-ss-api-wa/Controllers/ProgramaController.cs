using bepensa_biz.Interfaces;
using bepensa_models.ApiWa;
using bepensa_models.DTO;
using bepensa_models.General;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bepensard_ss_api_wa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgramaController : ControllerBase
    {
        private readonly IConsultasProgramasRDProxy _movementsApp;
        public ProgramaController(IConsultasProgramasRDProxy movementsApp)
        {
            _movementsApp = movementsApp;

        }

        // <summary>
        // Consulta puntos de usuario en mes y año
        // </summary>        
        [HttpPost("mecanica-de-acumulacion")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public Respuesta<List<ConceptosDeAcumulacionRDDTOWa>> ConsultaMecanicas(RequestCliente data)
        {
            Respuesta<List<ConceptosDeAcumulacionRDDTOWa>> resultado = _movementsApp.ConsultaMecanicas(data);
            return resultado;
        }

        //<summary>
        //Consulta puntos de usuario en mes y año
        //</summary>        
        [HttpPost("bonos-exclusivos")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public Respuesta<List<ConceptosDeAcumulacionRDDTOWa>> ConsultaBonos(RequestCliente data)
        {
            Respuesta<List<ConceptosDeAcumulacionRDDTOWa>> resultado = _movementsApp.ConsultaBonos(data);
            return resultado;
        }

        // <summary>
        // Consulta puntos de usuario en mes y año
        // </summary>        
        [HttpPost("canales-de-comunicacion")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public Respuesta<List<OrigeneRDDTOWa>> ConsultaCanalesComunicacion(RequestCliente data)
        {
            Respuesta<List<OrigeneRDDTOWa>> resultado = _movementsApp.ConsultaCanalesComunicacion(data);
            return resultado;
        }

        // <summary>
        // Consulta puntos de usuario en mes y año
        // </summary>        
        [HttpPost("vigencia-canjes")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public Respuesta<List<ConceptosDeAcumulacionRDDTOWa>> ConsultaVigencia(RequestCliente data)
        {
            Respuesta<List<ConceptosDeAcumulacionRDDTOWa>> resultado = _movementsApp.ConsultaVigencia(data);
            return resultado;
        }
    }
}