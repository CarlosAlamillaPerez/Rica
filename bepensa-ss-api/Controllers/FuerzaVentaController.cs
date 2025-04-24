using bepensa_biz.Interfaces;
using bepensa_data.models;
using bepensa_models.App;
using bepensa_models.DataModels;
using bepensa_models.DTO;
using bepensa_models.Enums;
using bepensa_models.General;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace bepensa_ss_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FuerzaVentaController : ControllerBase
    {
        private readonly IFuerzaVenta _fuerzaVenta;

        public FuerzaVentaController(IFuerzaVenta fuerzaVenta)
        {
            _fuerzaVenta = fuerzaVenta;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("ValidaAcceso/{idCanal}")]
        public async Task<ActionResult<Respuesta<FuerzaVentaDTO>>> ValidaAcceso(LoginApp loginApp, int idCanal)
        {
            Respuesta<FuerzaVentaDTO> resultado = new();

            try
            {
                resultado = await _fuerzaVenta.ValidaAcceso(loginApp, idCanal);

                return Ok(resultado);
            }
            catch (Exception)
            {
                resultado.Exitoso = false;
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Data = null;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();

                return BadRequest(resultado);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("ConsultarUsuarios")]
        public async Task<ActionResult<Respuesta<List<UsuarioDTO>>>> ConsultarUsuarios(BuscarFDVRequest loginApp)
        {
            Respuesta<List<UsuarioDTO>> resultado = new();

            try
            {
                resultado = await _fuerzaVenta.ConsultarUsuarios(loginApp);

                return Ok(resultado);
            }
            catch (Exception)
            {
                resultado.Exitoso = false;
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Data = null;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();

                return BadRequest(resultado);
            }
        }
    }
}
