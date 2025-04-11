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
public class SocioController : ControllerBase
{
    private readonly IObjetivo _objetivo;

    private readonly IUsuario _usuario;

    public SocioController(IObjetivo objetivo, IUsuario usuario)
    {
        _objetivo = objetivo;
        _usuario = usuario;
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost("Consultar/ConsultarDatos")]
    public async Task<ActionResult<Respuesta<UsuarioDTO>>> ConsultarDatos(int pIdUsuario)
    {
        Respuesta<UsuarioDTO> resultado = new();

        try
        {
            resultado = await _usuario.BuscarUsuario(pIdUsuario);

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
    [HttpPost("Consultar/MetaMensual")]
    public ActionResult<Respuesta<MetaMensualDTO>> ConsultarMeteMensual(UsuarioPeriodoRequest pUsuario)
    {
        Respuesta<MetaMensualDTO> resultado = new();

        try
        {
            resultado = _objetivo.ConsultarMetaMensual(pUsuario);

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
    [HttpPost("Consultar/PortafolioPrioritarioActual")]
    public ActionResult<Respuesta<List<PortafolioPrioritarioDTO>>> ConsultarPortafolioPrioritario(RequestByIdUsuario pUsuario)
    {
        Respuesta<List<PortafolioPrioritarioDTO>> resultado = new();

        try
        {
            resultado = _objetivo.ConsultarPortafolioPrioritario(pUsuario);

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
    [HttpPost("Consultar/PortafolioPrioritario")]
    public ActionResult<Respuesta<List<PortafolioPrioritarioDTO>>> ConsultarPortafolioPrioritario(UsuarioPeriodoRequest pUsuario)
    {
        Respuesta<List<PortafolioPrioritarioDTO>> resultado = new();

        try
        {
            resultado = _objetivo.ConsultarPortafolioPrioritario(pUsuario);

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
    [HttpPost("Consultar/PortafoliosPrioritarios")]
    public ActionResult<Respuesta<List<DetallePortafolioPrioritarioDTO>>> ConsultarPortafoliosPrioritarios(RequestByIdUsuario pUsuario)
    {
        Respuesta<List<DetallePortafolioPrioritarioDTO>> resultado = new();

        try
        {
            resultado = _objetivo.ConsultarPortafoliosPrioritarios(pUsuario);

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
    [HttpPost("Consultar/MetasMensuales")]
    public ActionResult<Respuesta<List<MetaCompraDTO>>> ConsultarMetasMensuales(RequestByIdUsuario pUsuario)
    {
        Respuesta<List<MetaCompraDTO>> resultado = new();

        try
        {
            resultado = _objetivo.ConsultarMetasMensuales(pUsuario);

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
    [HttpPost("Actualizar/ActualizarPassword")]
    public async Task<ActionResult<Respuesta<UsuarioDTO>>> ActualizarPassword(CambiarPasswordRequestApp data)
    {
        Respuesta<Empty> resultado = new();

        try
        {
            resultado = await _usuario.CambiarContraseniaApp(data);

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
    [HttpPost("Actualizar/ActualizarDatos")]
    public async Task<ActionResult<Respuesta<UsuarioDTO>>> ActualizarDatos(int pIdUsuario, string pCelular, string pEmail)
    {
        Respuesta<bool> resultado = new();

        try
        {
            resultado = await _usuario.Actualizar(pIdUsuario, pCelular, pEmail);

            return Ok(resultado);
        }
        catch (Exception)
        {
            resultado.Exitoso = false;
            resultado.Codigo = (int)CodigoDeError.Excepcion;
            resultado.Data = false;
            resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();

            return BadRequest(resultado);
        }
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost("Consultar/ConsultarEjecucionTradicional")]
    public ActionResult<Respuesta<List<EjecucionDTO>>> ConsultarEjecucionTradicional(RequestByIdUsuario pUsuario)
    {
        Respuesta<List<EjecucionDTO>> resultado = new();

        try
        {
            resultado = _objetivo.ConsultarEjecucionTradicional(pUsuario);

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
