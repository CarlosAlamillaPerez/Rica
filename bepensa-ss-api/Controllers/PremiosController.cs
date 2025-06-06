using bepensa_biz.Interfaces;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PremiosController : ControllerBase
    {
        private readonly IPremio _premio;

        public PremiosController(IPremio premio)
        {
            _premio = premio;
        }

        [HttpGet("Consultar/Categorias")]
        public ActionResult<Respuesta<List<CategoriaDePremioDTO>>> ConsultarCategorias()
        {
            Respuesta<List<CategoriaDePremioDTO>> resultado = new();

            try
            {
                resultado = _premio.ConsultarCategorias();

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

        [HttpGet("Consultar/Categorias/{idCategoriaDePremio}/Premios")]
        [HttpGet("Consultar/{idUsuario}/Usuario/{idCategoriaDePremio}/Categorias/Premios")]
        public ActionResult<Respuesta<List<PremioDTO>>> ConsultarPremios(int idCategoriaDePremio, int? idUsuario)
        {
            Respuesta<List<PremioDTO>> resultado = new();

            try
            {
                resultado = _premio.ConsultarPremios(idCategoriaDePremio, idUsuario);

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

        [HttpGet("Consultar/{pId}/Premio")]
        [HttpGet("Consultar/{idUsuario}/Usuario/{pId}/Premio")]
        public ActionResult<Respuesta<PremioDTO>> ConsultarPremio(int pId, int? idUsuario)
        {
            Respuesta<PremioDTO> resultado = new();

            try
            {
                resultado = _premio.ConsultarPremioById(pId, idUsuario);

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
