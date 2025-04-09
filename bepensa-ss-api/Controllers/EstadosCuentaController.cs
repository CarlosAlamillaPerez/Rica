using bepensa_biz.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace bepensa_ss_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadosCuentaController : ControllerBase
    {
        private readonly IEstadoCuenta _estadoCuenta;

        public EstadosCuentaController(IEstadoCuenta estadoCuenta)
        {
            _estadoCuenta = estadoCuenta;
        }


    }
}
