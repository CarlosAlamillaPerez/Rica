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
    //[ApiController]
    public class PremiosController : ControllerBase
    {
        private readonly IConsultasPremiosRDProxy _movementsApp;
        public PremiosController(IConsultasPremiosRDProxy movementsApp)
        {
            _movementsApp = movementsApp;

        }

        /// <summary>
        /// Consulta puntos de usuario en mes y año
        /// </summary>        
        [HttpPost("sugeridos")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public Respuesta<List<PremioRDDTOWa>> PremiosSugeridosRD(RequestCliente data)
        {
            Respuesta<List<PremioRDDTOWa>> resultado = _movementsApp.PremiosSugeridosRD(data);
            return resultado;
        }

        // <summary>
        // Consulta puntos de usuario en mes y año
        // </summary>        
        [HttpPost("Canjeados")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public Respuesta<List<PremioRDDTOWa>> PremiosCanjeadosRD(RequestClienteCanje data)
        {
            Respuesta<List<PremioRDDTOWa>> resultado = _movementsApp.PremiosCanjeadosRD(data);
            return resultado;
        }

        [HttpPost("canjeados-detalle")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public Respuesta<List<PremioRDDTOWa>> PremioCanjeadoDetalleRD(RequestClienteCanjeDetalle data)
        {
            Respuesta<List<PremioRDDTOWa>> resultado = _movementsApp.PremioCanjeadoDetalleRD(data);
            return resultado;
        }
    }
}