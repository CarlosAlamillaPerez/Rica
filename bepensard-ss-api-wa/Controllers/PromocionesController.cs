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
    public class PromocionesController : ControllerBase
    {
        private readonly IConsultasPromocionesRDProxy _movementsApp;
        public PromocionesController(IConsultasPromocionesRDProxy movementsApp)
        {
            _movementsApp = movementsApp;

        }

        // <summary>
        // Consulta puntos de usuario en mes y año
        // </summary>        
        [HttpPost("promociones")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public Respuesta<List<PromocionesRDDTOWa>> ConsultaPromociones(RequestCliente data)
        {
            Respuesta<List<PromocionesRDDTOWa>> resultado = _movementsApp.ConsultaPromociones(data);
            return resultado;
        }
    }
}
