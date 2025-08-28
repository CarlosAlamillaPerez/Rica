using Azure;
using bepensa_biz.Extensions;
using bepensa_biz.Interfaces;
using bepensa_biz.Settings;
using bepensa_data.data;
using bepensa_data.models;
using bepensa_models.ApiResponse;
using bepensa_models.Enums;
using bepensa_models.General;
using bepensa_models.Logger;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace bepensa_biz.Proxies
{
    public class OpenPayProxy : ProxyBase, IOpenPay
    {
        private readonly GlobalSettings app;

        private readonly ApiPasarelaSettings configApi;

        private readonly OpenPaySettings config;

        private readonly Serilog.ILogger _logger;

        private readonly Channel<ExternalApiLogger> _logApi;

        private bool Produccion { get; } = false;

        private string UrlBase { get; }


        private readonly HttpClient _http;

        public OpenPayProxy(IOptionsSnapshot<GlobalSettings> app,
            IOptionsSnapshot<ApiPasarelaSettings> configApi, IOptionsSnapshot<OpenPaySettings> config
            , BepensaContext context, Serilog.ILogger logger, Channel<ExternalApiLogger> logApi, IHttpClientFactory factory)
        {
            this.app = app.Value;
            this.configApi = configApi.Value;
            this.config = config.Value;
            _logger = logger;
            _logApi = logApi;
            DBContext = context;

            Produccion = this.app.Produccion;

            UrlBase = Produccion ? this.config.UrlProd : this.config.UrlQA;

            _http = factory.CreateClient();
        }

        public async Task<Respuesta<CargoResponse>> CreditCard(CargoOpenPayRequest data, int idOrigen)
        {
            Respuesta<CargoResponse> resultado = new();

            try
            {
                var valida = Extensiones.ValidateRequest(data);

                if (!valida.Exitoso)
                {
                    resultado.Codigo = valida.Codigo;
                    resultado.Mensaje = valida.Mensaje;
                    resultado.Exitoso = valida.Exitoso;

                    return resultado;
                }

                var auth = await Auth(idOrigen);

                if (!auth.Exitoso || auth.Data == null)
                {
                    resultado.Codigo = auth.Codigo;
                    resultado.Mensaje = auth.Mensaje;
                    resultado.Exitoso = false;

                    return resultado;
                }

                var url = UrlBase + "api/";
                var metodo = "charges";

                var client = new RestClient(url);
                var request = new RestRequest(metodo, Method.Post);

                request.AddHeader("Authorization", $"Bearer {auth.Data.Token}");

                request.AddHeader("Content-Type", "application/json");

                request.AddJsonBody(data);

                var logApi = new ExternalApiLogger
                {
                    ApiName = "OpenPay",
                    Method = metodo,
                    RequestBody = JsonConvert.SerializeObject(request.Parameters),
                    RequestTimestamp = DateTime.Now,
                    IdTransaccionLog = resultado.IdTransaccion
                };

                var response = await client.ExecuteAsync<RespuestaOpenPay<CargoResponse>>(request);

                logApi.ResponseBody = response.Content;
                logApi.ResponseTimestamp = DateTime.Now;
                logApi.StatusCode = (int)response.StatusCode;
                logApi.Resultado = response.IsSuccessStatusCode ? "Success" : "Error";

                await _logApi.Writer.WriteAsync(logApi);

                return GetResponseCharge(response);
            }
            catch (Exception ex)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;

                _logger.Error(ex, "CreditCard(CargoOpenPayRequest) => Cuc::{usuario}", data.Customer.Code);
            }

            return resultado;
        }

        public async Task<Respuesta<CargoResponse>> Charge(string id, int idOrigen)
        {
            Respuesta<CargoResponse> resultado = new();

            try
            {
                var auth = await Auth(idOrigen);

                if (!auth.Exitoso || auth.Data == null)
                {
                    resultado.Codigo = auth.Codigo;
                    resultado.Mensaje = auth.Mensaje;
                    resultado.Exitoso = false;

                    return resultado;
                }

                var url = UrlBase + "api/";
                var metodo = "charges/{id}".Replace("{id}", id);

                var client = new RestClient(url);
                var request = new RestRequest(metodo, Method.Get);

                request.AddHeader("Authorization", $"Bearer {auth.Data.Token}");

                request.AddHeader("Content-Type", "application/json");

                var logApi = new ExternalApiLogger
                {
                    ApiName = "OpenPay",
                    Method = metodo,
                    RequestBody = JsonConvert.SerializeObject(request.Parameters),
                    RequestTimestamp = DateTime.Now,
                    IdTransaccionLog = resultado.IdTransaccion
                };

                var response = await client.ExecuteAsync<RespuestaOpenPay<CargoResponse>>(request);

                logApi.ResponseBody = response.Content;
                logApi.ResponseTimestamp = DateTime.Now;
                logApi.StatusCode = (int)response.StatusCode;
                logApi.Resultado = response.IsSuccessStatusCode ? "Success" : "Error";

                await _logApi.Writer.WriteAsync(logApi);

                return GetResponseCharge(response);
            }
            catch (Exception ex)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;

                _logger.Error(ex, "Charge(string) => IdOpenPay::{usuario}");
            }

            return resultado;
        }

        private async Task<Respuesta<AuthOpenPay>> Auth(int idOrigen)
        {
            Respuesta<AuthOpenPay> resultado = new();

            try
            {
                var url = UrlBase + "api/";
                var metodo = "auth/login";

                var client = new RestClient(url);
                var request = new RestRequest(metodo, Method.Post);

                var data = new
                {
                    UserName = Produccion ? configApi.UsuarioProd : configApi.UsuarioQA,
                    Password = Produccion ? configApi.PasswordProd : configApi.PasswordQA,
                    Origin = EnumExtensions.GetDescriptionFromValue<TipoOrigen>(idOrigen)
                };

                request.AddHeader("Content-Type", "application/json");

                request.AddJsonBody(data);

                var response = await client.ExecuteAsync<RespuestaOpenPay<AuthOpenPay>>(request);

                if (response.IsSuccessful && response.Data != null)
                {
                    resultado.Data = response.Data.Data;
                }
                else
                {
                    resultado.Codigo = (int)CodigoDeError.AutenticacionFallidaApi;
                    resultado.Mensaje = CodigoDeError.AutenticacionFallidaApi.GetDescription();
                    resultado.Exitoso = false;
                }
            }
            catch (Exception ex)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDisplayName();

                _logger.Error(ex, "CreditCard(CargoOpenPayRequest) => Origen::{usuario}", idOrigen);
            }

            return resultado;
        }

        private Respuesta<CargoResponse> GetResponseCharge(RestResponse<RespuestaOpenPay<CargoResponse>> response)
        {
            Respuesta<CargoResponse> resultado = new();

            if (response.Data != null)
            {
                resultado.Data = response.Data.Data;

                if (!(response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.OK)) resultado.Exitoso = false;

                if (response.StatusCode == HttpStatusCode.BadRequest && response.Data.Status.Equals("charge_pending"))
                {
                    resultado.Mensaje = MensajeApp.CompraPuntosRequiereRedireccionAVerificacionBancaria.GetDescription();

                    resultado.Exitoso = true;

                    return resultado;
                }

                var apiResult = response.Data;

                if (apiResult.StatusCode == (int)HttpStatusCode.OK)
                {
                    resultado.Codigo = (int)CodigoDeError.ErrorDeRed;
                    resultado.Exitoso = false;
                    switch (apiResult?.Data?.Status)
                    {
                        case string code when code.Contains("failed"):
                            resultado.Mensaje = "El banco rechazó el pago. Intenta nuevamente o usa otro método.";
                            return resultado;
                        default:
                            return resultado;
                    }
                }

                if (apiResult.StatusCode != 200)
                {
                    resultado.Codigo = (int)CodigoDeError.ErrorDeRed;
                    resultado.Exitoso = false;

                    switch (apiResult.Status)
                    {
                        case string code when code.Contains("3001"):
                            resultado.Mensaje = "La tarjeta fue rechazada.";
                            break;
                        case string code when code.Contains("3002"):
                            resultado.Mensaje = "La tarjeta fue rechazada.";
                            break;
                        case string code when code.Contains("3003"):
                            resultado.Mensaje = "La tarjeta no tiene fondos suficientes.";
                            break;
                        case string code when code.Contains("3004"):
                            resultado.Mensaje = "La tarjeta ha sido identificada como una tarjeta robada.";
                            break;
                        case string code when code.Contains("3005"):
                            resultado.Mensaje = "La tarjeta ha sido rechazada por el sistema antifraudes.";
                            break;
                        case string code when code.Contains("15011"):
                            resultado.Mensaje = "No verificado; transacción denegada.";
                            break;
                        case string code when code.Contains("1003"):
                            resultado.Mensaje = "No pudimos procesar tu pago. Revisa tu tarjeta o intenta con otro método.";
                            break;
                        default:
                            resultado.Mensaje = CodigoDeError.ErrorDeRed.GetDescription();
                            break;
                    }

                    return resultado;
                }
            }
            else
            {
                resultado.Codigo = (int)CodigoDeError.AutenticacionFallidaApi;
                resultado.Mensaje = CodigoDeError.AutenticacionFallidaApi.GetDescription();
                resultado.Exitoso = false;
            }

            return resultado;
        }
    }
}
