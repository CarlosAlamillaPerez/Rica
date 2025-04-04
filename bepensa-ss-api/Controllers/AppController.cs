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
    private readonly IMapper mapper;
    private readonly ISecurity _security;
    private readonly IApp _app;

    public AppController(IMapper mapper, ISecurity security, IApp app)
    {
        this.mapper = mapper;
        _security = security;
        _app = app;
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

}
