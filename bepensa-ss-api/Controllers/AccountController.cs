using bepensa_biz.Interfaces;
using bepensa_models.DTO;
using bepensa_models.App;
using bepensa_models.Enums;
using bepensa_models.General;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using bepensa_data.models;
using bepensa_models.DataModels;

namespace bepensa_ss_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ISecurity _security;
        private readonly IUsuario _usuario;

        public AccountController(IMapper mapper, ISecurity security, IUsuario usuarios)
        {
            this.mapper = mapper;
            _security = security;
            _usuario = usuarios;
        }

        [HttpPost("Login")]
        public ActionResult<RespuestaAutenticacion> Login(ProviderDTO provider)
        {
            try
            {
                var resultado = _security.ValidaApiKey(provider); //_signInManager.PasswordSignInAsync(login.Email, login.Password, isPersistent: false, lockoutOnFailure: true);

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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("login/usuario")]
        public async Task<ActionResult<Respuesta<UsuarioDTO>>> ValidaAcceso([FromBody]LoginApp credenciales)
        {
            Respuesta<UsuarioDTO> resultado = new ();

            try
            {
                resultado = await _usuario.ValidaAcceso(credenciales);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                resultado.Exitoso = false;
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Data = null;
                resultado.Mensaje = ex.Message;
                return BadRequest(resultado);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("login/RecuperarContrasenia")]
        public async Task<ActionResult<Respuesta<Empty>>> RecuperarContrasenia(RestablecerPassRequest data)
        {
            Respuesta<Empty> resultado = new ();

            try
            {
                var task = await _usuario.RecuperarContrasenia(data);

                if (!task.Exitoso)
                {
                    resultado.Exitoso = false;
                    resultado.Codigo = task.Codigo;
                    resultado.Mensaje = task.Mensaje;
                    return BadRequest(resultado);
                }

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                resultado.Exitoso = false;
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = ex.Message;
                return BadRequest(resultado);
            }
        }
    }
}
