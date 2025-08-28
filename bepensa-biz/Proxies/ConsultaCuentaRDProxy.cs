using bepensa_biz.Extensions;
using bepensa_biz.Interfaces;
using bepensa_biz.Settings;
using bepensa_data.data;
using bepensa_data.modelsRD;
using bepensa_models.ApiWa;
using bepensa_models.Enums;
using bepensa_models.General;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text;

namespace bepensa_biz.Proxies
{
    public class ConsultaCuentaRDProxy : ProxyBase, IConsultasCuentaRDProxy
    {
        private readonly IConfiguration _configuration;
        private readonly GlobalSettings _ajustes;
        private readonly SmsSettings _smsAjustes;
        public ConsultaCuentaRDProxy(BepensaRD_Context Context, IConfiguration Configuracion, IOptionsSnapshot<GlobalSettings> ajustes, IOptionsSnapshot<SmsSettings> smsAjustes)
        {
            DBContextRD = Context;
            _configuration = Configuracion;
            _ajustes = ajustes.Value;
            _smsAjustes = smsAjustes.Value;
        }

        public Respuesta<ResponseCuentaPrivacidad> ConsultaCliente(RequestCliente data)
        {
            Respuesta<ResponseCuentaPrivacidad> resultado = new();
            resultado.Data = new();

            try
            {
                var valida = Extensions.Extensiones.ValidateRequest(data);

                if (!valida.Exitoso)
                {
                    resultado.Codigo = valida.Codigo;
                    resultado.Mensaje = valida.Mensaje;
                    resultado.Exitoso = valida.Exitoso;
                    goto final;
                }


                if (!DBContextRD.Usuarios.Any(x => x.Cuc == data.Cliente))
                {
                    resultado.Codigo = (int)CodigoDeError.NoExisteUsuario;
                    resultado.Mensaje = CodigoDeError.NoExisteUsuario.GetDescription();
                    resultado.Exitoso = false;
                    goto final;
                }

                Usuario usuario = DBContextRD.Usuarios.FirstOrDefault(x => x.Cuc == data.Cliente);//variable se guarda lo de usuario                                                                                                            

                resultado.Data.urlAvisoPrivacidad = Convert.ToString(_configuration["SettingsUrlPrivacidad:siteUrlPrivacidadRD"]);
                resultado.Data.urlPlaystore = "";
                resultado.Data.urlAppleStore = "";                

            }
            catch (Exception)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;
                //
            }
        final:
            return resultado;
        }

        public async Task<Respuesta<Empty>> RecuperarPassword(RequestRecuperaPass data)
        {
            Respuesta<Empty> resultado = new();
            resultado.Data = new();
            Guid token = Guid.NewGuid();
            string url = (_ajustes.Url + "restablecer-contrasena/{token}").Replace("{token}", token.ToString()).ToLower();

            try
            {
                var valida = Extensions.Extensiones.ValidateRequest(data);

                if (!valida.Exitoso)
                {
                    resultado.Codigo = valida.Codigo;
                    resultado.Mensaje = valida.Mensaje;
                    resultado.Exitoso = valida.Exitoso;
                    goto final;
                }


                if (!DBContextRD.Usuarios.Any(x => x.Cuc == data.Cliente))
                {
                    resultado.Codigo = (int)CodigoDeError.NoExisteUsuario;
                    resultado.Mensaje = CodigoDeError.NoExisteUsuario.GetDescription();
                    resultado.Exitoso = false;
                    goto final;
                }

                Usuario usuario = DBContextRD.Usuarios.FirstOrDefault(x => x.Cuc == data.Cliente);
                //&& x.Celular.Substring(Math.Max(0, x.Celular.Length - 4)) == data.telUltimo4digitos);//variable se guarda lo de usuario
                if (usuario.Celular.Substring(usuario.Celular.Length - 4) != data.telUltimo4digitos)
                {
                    resultado.Codigo = (int)CodigoDeError.NoExisteUsuario;
                    resultado.Mensaje = CodigoDeError.NoExisteUsuario.GetDescription();
                    resultado.Exitoso = false;
                    goto final;
                }
                var celular = usuario.Celular;

                usuario.BitacoraEnvioCorreos.Add(new BitacoraEnvioCorreo
                {
                    Email = usuario.Celular,
                    FechaEnvio = DateTime.Now,
                    Codigo = "SMS-RestablecerPass",
                    Token = token,
                    IdEstatus = (int)TipoEstatus.CodigoActivo
                });
                DBContextRD.SaveChanges();
                url = GetShortUrl(url);

                var mensaje = SmsText.RestablecerPass.GetDescription().Replace("@URL", url);

                await SendText(mensaje, [celular]);

            }
            catch (Exception)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;
                //
            }
        final:
            return resultado;
        }

        #region Métodos Privados
        private async Task<Respuesta<Empty>> SendText(string mensaje, List<string> celulares, string? CampaignName = null, bool encode = false, bool longMessage = false)
        {
            Respuesta<Empty> resultado = new() { IdTransaccion = Guid.NewGuid() };

            try
            {
                SmsBasic data = new()
                {
                    Text = mensaje,
                    Campaign_name = CampaignName ?? _smsAjustes.CampaignName,
                    Recipients = [],
                    Route = _smsAjustes.Route,
                    Country = _smsAjustes.Country,
                    Encode = encode,
                    Long_message = longMessage
                };

                celulares.ForEach(x =>
                {
                    data.Recipients.Add(new TelefonoSms
                    {
                        Cellphone = x
                    });
                });

                using var client = new HttpClient();

                string url = _smsAjustes.Url;

                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    Formatting = Formatting.Indented
                };

                string jsonData = JsonConvert.SerializeObject(data, settings);

                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImp0aSI6ImZmZTdjOTlmMTg0YWNhNDM1MDY4NzVkNWU5MDE2OWQyYWFjNTg3MmU0MmU1YWI0ZTNiYmUzZjhmMGI5NjIwOTZkOGIzYzVmNTRjYzZmZTBkIn0.eyJhdWQiOiI3IiwianRpIjoiZmZlN2M5OWYxODRhY2E0MzUwNjg3NWQ1ZTkwMTY5ZDJhYWM1ODcyZTQyZTVhYjRlM2JiZTNmOGYwYjk2MjA5NmQ4YjNjNWY1NGNjNmZlMGQiLCJpYXQiOjE3Mzg5NTE3NDcsIm5iZiI6MTczODk1MTc0NywiZXhwIjoxNzcwNDg3NzQ3LCJzdWIiOiI0Njc3Iiwic2NvcGVzIjpbXX0.e61WQqP3CoDhI-uARdKSOEsknH-a9BE-NSlempVotIIKyJ2AGRsnmFUmcdjA8k1COekp3oA3_1M9OuPI9tSzc6-yWvDdOYq3Od-fuWIZtk39wqSZXvU25ijGb9Ybwemw2EHN6sPoAoqTEGGZHzgaW8cBEA7XagrKRrpo1_XcSA29yociZo758yrC7BUkQih241UjrYE0mhY_vpO29Xxt4QJmsZU1H7fUtT4r6vJlQ-sgEA3J8wZmKelbA8xwCIqcCze553oeQTflr3x_zgVEmFyPFxN-gfw0TSFYRWmza4JFnYhrdj6xOIfj1pcHrMqTodTIkCV8hFMJQQCTE8IdmlS3oc7byuEvIl6LEe_LoPJ0RXnJV7-alDZtdUdNqd01cSFaQ3ixP9X16yUb-SMmC-PaN6dkvQVYZzxVxjyu1DJmhZQDOr72w2OiIMZGfKweKlsY2HPeKCGgsPDcAYu6vD3UKIu-A9tSGfwhQbCwci1tAxskI4xz1fHZyi7ct3Awg8QPSodPJLyL033GnPFEC4wAzEDM9khqt5z1sGMzYiQe0AR8qiHXkAmgWdcT62YraLsmA74B4RmEiqI9nZJLjB37VGrs6bZKbPj0w3NDlH0cR6Zmjh_74f-_CXq7cybjnYl3u_QC8bW6iFeJxMou4CZWYu1GL3bMNX21HmsVGOA");

                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                HttpResponseMessage response = await client.PostAsync(url, content);

                string responseData = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    resultado.Codigo = (int)CodigoDeError.ErrorDesconocido;
                    resultado.Mensaje = CodigoDeError.ErrorDesconocido.GetDisplayName();
                    resultado.Exitoso = false;
                }
            }
            catch (Exception)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;
            }

            return resultado;
        }

        private string GetShortUrl(string longUrl)
        {
            try
            {
                string clave;

                do
                {
                    clave = URLGenerator.GenerateShortUrl();
                }
                while (DBContextRD.UrlShorteners.Any(x => x.Clave.Equals(clave) && x.IdEstatus == (int)TipoEstatus.Activo));

                var shortUrl = _ajustes.Url + clave;

                DBContextRD.UrlShorteners.Add(new UrlShortener
                {
                    OriginalUrl = longUrl,
                    ShortUrl = shortUrl,
                    Clave = clave,
                    IdEstatus = (int)TipoEstatus.Activo
                });

                DBContextRD.SaveChanges();

                return shortUrl;
            }
            catch (Exception)
            {
                return longUrl;
            }
        }
        #endregion
    }
}
