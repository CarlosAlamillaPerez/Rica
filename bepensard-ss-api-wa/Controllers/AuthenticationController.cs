using bepensa_biz.Interfaces;
using bepensa_models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bepensard_ss_api_wa.Controllers
{
    [Route("api/Autenticar")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        private readonly ISecurity _security;
        public AuthenticationController(ISecurity security)
        {
            _security = security;
        }

        #region Login
        [HttpPost()]
        public ActionResult<RespuestaAutenticacion> Login(ProviderDTO provider)
        {
            try
            {
                var resultado = _security.ValidaApiKey(provider);

                if (resultado.Data)
                {
                    return _security.GenerarToken(provider); // Se construye el token y se devuelve
                }
                else
                {
                    return BadRequest("Credenciales no válidas");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }

        #endregion
    }
}
