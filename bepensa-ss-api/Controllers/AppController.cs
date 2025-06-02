using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using bepensa_biz.Interfaces;
using bepensa_models.DTO;
using bepensa_models.Enums;
using bepensa_models.General;
using bepensa_models.DataModels;
using bepensa_biz;
using System.Threading.Tasks;
using bepensa_models.App;
using bepensa_models.ApiResponse;
using System.Text.Json;

namespace bepensa_ss_api;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AppController : ControllerBase
{
    private readonly IApp _app;
    private readonly IApi _api;

    private readonly IDireccion _direccion;

    private readonly ILoggerContext _logger;

    public AppController(IApp app, IDireccion direccion, IApi api, ILoggerContext logger)
    {
        _app = app;
        _direccion = direccion;
        _api = api;
        _logger = logger;
    }

    [HttpPost("ConsultaParametro")]
    public async Task<ActionResult<Respuesta<string>>> ConsultaParametro(int pParametro)
    {
        Respuesta<string> resultado = new();

        try
        {
            resultado = await _app.ConsultaParametro(pParametro);

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

    [HttpPost("ConsultaImgPromociones")]
    public async Task<ActionResult<Respuesta<string>>> ConsultaImgPromociones(int pParametro)
    {
        Respuesta<List<ImagenesPromocionesDTO>> resultado = new();

        try
        {
            resultado = await _app.ConsultaImgPromociones(pParametro);

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

    #region Direccion
    [HttpGet("ConsultarColonias/{pCP}")]
    public async Task<ActionResult<Respuesta<List<ColoniaDTO>>>> ConsultarColonias(string pCP)
    {
        Respuesta<List<ColoniaDTO>> resultado = new();

        try
        {
            resultado = await _direccion.ConsultarColonias(pCP);

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

    [HttpGet("ConsultarColonia/{pIdColonia}")]
    public ActionResult<Respuesta<ColoniaDTO>> ConsultarColonias(int pIdColonia)
    {
        Respuesta<ColoniaDTO> resultado = new();

        try
        {
            resultado = _direccion.ConsultarColonia(pIdColonia);

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
    #endregion

    #region WhatsApp
    [AllowAnonymous]
    [HttpPost("/api/webhook/waba_endpoint")]
    public async Task<IActionResult> Receive([FromBody]JsonElement pJson)
    {
        var json = JsonSerializer.Serialize(pJson, new JsonSerializerOptions { WriteIndented = true });

        await _logger.AddJson("WA", json);

        return Ok();
    }
    #endregion

    #region Api
    [AllowAnonymous]
    [HttpPost("Consultar/DisponibilidadPremios/{pToken}")]
    public ActionResult<Respuesta<DisponibilidadMKT>> DisponibilidadPremios([FromBody] string data, Guid pToken)
    {
        Respuesta<DisponibilidadMKT> resultado = new();

        try
        {
            var t = pToken.ToString();
            if (pToken.ToString().ToUpper() == "46398F1D-4999-4A3F-BB3F-685CB58E394F")
            {
                List<string> sku = data.Split(',').ToList();

                resultado = _api.Disponibilidad(sku);

            }
            else
            {
                resultado.Codigo = (int)CodigoDeError.InvalidToken;
                resultado.Data = null;
                resultado.Mensaje = CodigoDeError.InvalidToken.GetDescription();
                resultado.Exitoso = false;
            }

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
    #endregion
}