using bepensa_biz.Extensions;
using bepensa_biz.Interfaces;
using bepensa_data.data;
using bepensa_models.Enums;
using bepensa_models.General;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Text;
using bepensa_biz.Settings;
using Microsoft.Extensions.Options;

namespace bepensa_biz.Proxies
{
    public class EmailProxy : ProxyBase, IAppEmail
    {
        private readonly SmsSettings _smsAjustes;

        public EmailProxy(BepensaContext context, IOptionsSnapshot<SmsSettings> smsAjustes)
        {
            DBContext = context;
            _smsAjustes = smsAjustes.Value;
        }

        public async Task<Respuesta<Empty>> RecuperarPassword(TipoMensajeria metodoDeEnvio, TipoUsuario tipoUsuario, int id, Guid token, string url)
        {
            Respuesta<Empty> resultado = new();

            try
            {
                switch (metodoDeEnvio)
                {
                    case TipoMensajeria.Email:
                        var data = new
                        {
                            IdUsuario = id,
                            IdOperador = id,
                            Token = token,
                            Url = url,
                            EnviadoOutPut = false
                        };

                        var parametros = Extensiones.CrearSqlParametrosDelModelo(data);

                        switch (tipoUsuario)
                        {
                            case TipoUsuario.Usuario:
                                var exec = await DBContext.Database.ExecuteSqlRawAsync("EXEC sp_EnvioCorreo_RecuperarPassword @IdUsuario, @Token, @Url, @EnviadoOutPut OUTPUT", parametros);
                                break;
                            default: throw new Exception("Usuario no identificado");
                        }

                        var valida = parametros.FirstOrDefault(p => p.ParameterName == "@EnviadoOutPut");

                        if (valida == null || !(bool)valida.Value)
                        {
                            resultado.Codigo = (int)CodigoDeError.ErrorDesconocido;
                            resultado.Mensaje = CodigoDeError.ErrorDesconocido.GetDescription();
                            resultado.Exitoso = false;
                        }

                        break;
                    case TipoMensajeria.Sms:

                        switch (tipoUsuario)
                        {
                            case TipoUsuario.Usuario:
                                var mensaje = SmsText.RestablecerPass.GetDescription().Replace("@URL", url);

                                var celular = (DBContext.Usuarios.Find(id)?.Celular) ?? throw new Exception("El celular no ha sido identificado.");

                                await SendText(mensaje, [celular]);

                                break;
                        }

                        break;
                    default: throw new Exception("El método de envío es desconocido.");
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

        public void Lectura(Guid? token)
        {
            try
            {
                var query = DBContext.BitacoraEnvioCorreos.FirstOrDefault(x => x.Token == token) 
                            ?? throw new Exception(TipoExcepcion.TokenNoEncontrado.GetDescription());

                if (query.FechaLectura == null)
                {
                    query.FechaLectura = DateTime.Now;
                }

                if (query.Lecturas == null)
                {
                    query.Lecturas = 1;
                }
                else
                {
                    query.Lecturas++;
                }

                DBContext.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Respuesta<Empty>> SendText(string mensaje, List<string> celulares, string? CampaignName = null, bool encode = false, bool longMessage = false)
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
    }
}
