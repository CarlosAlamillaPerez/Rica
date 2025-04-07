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
    public class PremiosController : ControllerBase
    {
        private readonly IPremio _premio;

        public PremiosController(IPremio premio)
        {
            _premio = premio;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("Consultar/Categorias/{idCategoriaDePremio}/Premios")]
        public ActionResult<Respuesta<List<PremioDTO>>> ConsultarPremios(int idCategoriaDePremio)
        {
            Respuesta<List<PremioDTO>> resultado = new();

            try
            {
                resultado = _premio.ConsultarPremios(idCategoriaDePremio);

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
        [HttpGet("Consultar/{pId}/Premio")]
        public ActionResult<Respuesta<PremioDTO>> ConsultarPremio(int pId)
        {
            Respuesta<PremioDTO> resultado = new();

            try
            {
                resultado = _premio.ConsultarPremioById(pId);

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
