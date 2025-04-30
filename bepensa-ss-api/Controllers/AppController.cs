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

namespace bepensa_ss_api;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AppController : ControllerBase
{
    private readonly IApp _app;

    private readonly IDireccion _direccion;

    public AppController(IApp app, IDireccion direccion)
    {
        _app = app;
        _direccion = direccion;
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
            resultado = _direccion.ConsultarColonias(pIdColonia);

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