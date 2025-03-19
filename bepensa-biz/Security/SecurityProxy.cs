using bepensa_biz.Settings;
using bepensa_biz.Interfaces;
using bepensa_data.models;
using bepensa_models.App;
using bepensa_models.DTO;
using bepensa_models.Enums;
using bepensa_models.General;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace bepensa_biz.Security
{
    public class SecurityProxy : ISecurity
    {
        private readonly AppSettings _appSettings;

        public SecurityProxy(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public RespuestaAutenticacion GenerarToken(ProviderDTO provider)
        {
            var claims = new List<Claim>()
            {
                new ("usuario", provider.ApiKey)
            };

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.TokeyKey));

            var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);
            var expiracion = DateTime.UtcNow.AddDays(_appSettings.TokeyExpiration);

            var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiracion, signingCredentials: creds);

            return new RespuestaAutenticacion()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                Expira = expiracion
            };
        }

        public RespuestaAutenticacion GenerarTokenDeUsuario(UsuarioDTO usuario)
        {
            string nombreCompleto = usuario.Nombre + " " + usuario.ApellidoPaterno;

            string iniciales = usuario.Nombre.Substring(0, 1) + usuario.ApellidoPaterno.Substring(0, 1);

            if (!string.IsNullOrEmpty(usuario.ApellidoMaterno))
            {
                nombreCompleto += ' ' + usuario.ApellidoMaterno;
                iniciales += usuario.ApellidoMaterno.Substring(0, 1);
            }

            string sesionId = usuario.Sesion != null ? usuario.Sesion : new Guid().ToString();

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Name, nombreCompleto ),
                new Claim("Iniciales", iniciales ),
                new Claim("Sesion", sesionId),
                new Claim("TipoDeUsuario", usuario.Cuc)
            };

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.TokeyKey));

            var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.UtcNow.AddHours(_appSettings.TokeyExpiration);

            var securityToken = new JwtSecurityToken(issuer: null, 
                                                    audience: null, 
                                                    claims: claims, 
                                                    expires: expiracion, 
                                                    signingCredentials: creds);

            return new RespuestaAutenticacion()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                Expira = expiracion
            };
        }

        public Respuesta<bool> ValidaApiKey(ProviderDTO provider)
        {
            Respuesta<bool> resultado = new Respuesta<bool>();
            try
            {
                var hashKey = new Hash(provider.ApiKey);
                var _apikey = hashKey.Sha512();
                var hashProvider = new Hash(provider.ApiProvider);
                var _apiProvider = hashProvider.Sha512();
                string _key = Convert.ToHexString(_apikey);
                string _provider = Convert.ToHexString(_apiProvider);

                if (_appSettings.ApiKey.Equals(_key) && _appSettings.ApiProvider.Equals(_provider))
                {
                    resultado.Codigo = (int)CodigoDeError.OK;
                    resultado.Data = true;
                    resultado.Exitoso = true;
                    resultado.Mensaje = "Ok.";
                }
                else
                {
                    resultado.Exitoso = false;
                    resultado.Codigo = (int)CodigoDeError.NoExisteUsuario;
                    resultado.Mensaje = "Credenciales no válidas.";
                }
            }
            catch (Exception ex)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = "Error: " + ex.Message;
                resultado.Exitoso = false;
            }
            return resultado;
        }
    }
}
