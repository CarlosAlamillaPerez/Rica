using bepensa_biz.Interfaces;
using bepensa_models.ApiResponse;
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
    public class CarritoController : ControllerBase
    {
        private readonly ICarrito _carrito;

        public CarritoController(ICarrito carrito)
        {
            _carrito = carrito;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("AgregarPremio")]
        public async Task<ActionResult<Respuesta<Empty>>> AgregarPremio(AgregarPremioRequest pUsuario)
        {
            Respuesta<Empty> resultado = new();

            try
            {
                resultado = await _carrito.AgregarPremio(pUsuario);

                return Ok(resultado);
            }
            catch (Exception)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Data = null;
                resultado.Exitoso = false;

                return BadRequest(resultado);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("EliminarPremio")]
        public async Task<ActionResult<Respuesta<CarritoDTO>>> EliminarPremio(RequestByIdPremio pUsuario)
        {
            Respuesta<CarritoDTO> resultado = new();

            try
            {
                resultado = await _carrito.EliminarPremio(pUsuario);

                return Ok(resultado);
            }
            catch (Exception)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Data = null;
                resultado.Exitoso = false;

                return BadRequest(resultado);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("ModificarPremio")]
        public async Task<ActionResult<Respuesta<CarritoDTO>>> ModificarPremio(ActPremioRequest pUsuario)
        {
            Respuesta<CarritoDTO> resultado = new();

            try
            {
                resultado = await _carrito.ModificarPremio(pUsuario);
                
                return Ok(resultado);
            }
            catch (Exception)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Data = null;
                resultado.Exitoso = false;

                return BadRequest(resultado);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("ConsultarCarrito")]
        public ActionResult<Respuesta<CarritoDTO>> ConsultarCarrito(RequestByIdUsuario pUsuario)
        {
            Respuesta<CarritoDTO> resultado = new();

            try
            {
                resultado = _carrito.ConsultarCarrito(pUsuario);

                return Ok(resultado);
            }
            catch (Exception)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Data = null;
                resultado.Exitoso = false;

                return BadRequest(resultado);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("ProcesarCarrito")]
        public async Task<ActionResult<Respuesta<List<ProcesaCarritoResultado>>>> ConsultarCarrito(ProcesarCarritoRequest pUsuario)
        {
            Respuesta<List<ProcesaCarritoResultado>> resultado = new();

            try
            {
                resultado = await _carrito.ProcesarCarrito(pUsuario);

                return Ok(resultado);
            }
            catch (Exception)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Data = null;
                resultado.Exitoso = false;

                return BadRequest(resultado);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("ExistePremioFisico/{idUsuario}")]
        public ActionResult<Respuesta<Empty>> ExistePremioFisico(int idUsuario)
        {
            Respuesta<Empty> resultado = new();

            try
            {
                resultado = _carrito.ExistePremioFisico(idUsuario);

                return Ok(resultado);
            }
            catch (Exception)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Data = null;
                resultado.Exitoso = false;

                return BadRequest(resultado);
            }
        }
    }
}
