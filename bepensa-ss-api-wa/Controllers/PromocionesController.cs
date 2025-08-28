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
    public class PromocionesController : ControllerBase
    {
        private readonly IConsultasPromocionesProxy _movementsApp;
        public PromocionesController(IConsultasPromocionesProxy movementsApp)
        {
            _movementsApp = movementsApp;

        }

        // <summary>
        // Consulta puntos de usuario en mes y año
        // </summary>        
        [HttpPost("promociones")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public Respuesta<List<PromocionesDTOWa>> ConsultaPromociones(RequestCliente data)
        {
            Respuesta<List<PromocionesDTOWa>> resultado = _movementsApp.ConsultaPromociones(data);
            return resultado;
        }
    }
}
