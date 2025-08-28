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
    //[ApiController]
    public class PremiosController : ControllerBase
    {
        private readonly IConsultasPremiosProxy _movementsApp;
        public PremiosController(IConsultasPremiosProxy movementsApp)
        {
            _movementsApp = movementsApp;

        }

        /// <summary>
        /// Consulta puntos de usuario en mes y año
        /// </summary>        
        [HttpPost("sugeridos")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public Respuesta<ResponseConsultaPremiosSugeridos> PremiosSugeridos(RequestCliente data)
        {
            Respuesta<ResponseConsultaPremiosSugeridos> resultado = _movementsApp.PremiosSugeridos(data);
            return resultado;
        }

        // <summary>
        // Consulta puntos de usuario en mes y año
        // </summary>        
        [HttpPost("Canjeados")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public Respuesta<List<PremioDTOWa>> PremiosCanjeados(RequestClienteCanje data)
        {
            Respuesta<List<PremioDTOWa>> resultado = _movementsApp.PremiosCanjeados(data);
            return resultado;
        }

        [HttpPost("canjeados-detalle")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public Respuesta<List<PremioDTOWa>> PremioCanjeadoDetalle(RequestClienteCanjeDetalle data)
        {
            Respuesta<List<PremioDTOWa>> resultado = _movementsApp.PremioCanjeadoDetalle(data);
            return resultado;
        }
    }
}
