using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using bepensa_biz.Interfaces;
using bepensa_models.DTO;
using bepensa_models.Enums;
using bepensa_models.General;
using bepensa_models.DataModels;

namespace bepensa_ss_api;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class EdoCtaController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly ISecurity _security;
    private readonly IEdoCta _edocta;

    public EdoCtaController(IMapper mapper, ISecurity security, IEdoCta edocta)
    {
        this.mapper = mapper;
        _security = security;
        _edocta = edocta;
    }

    [HttpPost("Header")]
    public async Task<ActionResult<Respuesta<HeaderEdoCtaDTO>>>  Header(int pIdUsuario, int pIdPeriodo)
    {
        Respuesta<HeaderEdoCtaDTO> resultado = new();

        try
        {
            resultado = await _edocta.Header(pIdUsuario, pIdPeriodo);

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

    [HttpPost("MisPuntos")]
    public async Task<ActionResult<Respuesta<EdoCtaDTO>>>  MisPuntos(int pIdUsuario, int pIdPeriodo)
    {
        Respuesta<EdoCtaDTO> resultado = new();

        try
        {
            resultado = await _edocta.MisPuntos(pIdUsuario, pIdPeriodo);

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

    [HttpPost("DetalleCanje")]
    public async Task<ActionResult<Respuesta<List<DetalleCanjeDTO>>>>  DetalleCanje(int pIdUsuario, int pIdPeriodo)
    {
        Respuesta<List<DetalleCanjeDTO>> resultado = new();

        try
        {
            resultado = await _edocta.DetalleCanje(pIdUsuario, pIdPeriodo);

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

    [HttpPost("Consultar/EstadoCuenta")]
    public async Task<ActionResult<Respuesta<EstadoDeCuentaDTO>>> ConsultarEstatdoCuenta(UsuarioPeriodoRequest pdUsuario)
    {
        Respuesta<EstadoDeCuentaDTO> resultado = new();

        try
        {
            resultado = await _edocta.ConsultarEstatdoCuenta(pdUsuario);

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

    [HttpPost("Consultar/Canjes")]
    public async Task<ActionResult<Respuesta<CanjeDTO>>> ConsultarCanjes(UsuarioPeriodoRequest pdUsuario)
    {
        Respuesta<CanjeDTO> resultado = new();

        try
        {
            resultado = await _edocta.ConsultarCanjes(pdUsuario);

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
