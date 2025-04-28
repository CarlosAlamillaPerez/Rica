using bepensa_biz.Interfaces;
using bepensa_biz.Settings;
using bepensa_models.ApiResponse;
using bepensa_models.Enums;
using bepensa_models.General;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;

namespace bepensa_biz.Proxies
{
    public class ApiProxy : IApi
    {
        private readonly ApiRMSSettings _ajustesRMS;

        public ApiProxy(IOptionsSnapshot<ApiRMSSettings> ajustesRMS)
        {
            _ajustesRMS = ajustesRMS.Value;
        }

        public Respuesta<RastreoRMS> Autenticacion()
        {
            Respuesta<RastreoRMS> resultado = new()
            {
                IdTransaccion = Guid.NewGuid(),
                Data = new()
            };

            var FechaActual = DateTime.Now;

            try
            {
                string UrlBase = _ajustesRMS.BaseUri;
                string Metodo = "rrms_aprobarcred.php";
                string Usuario = _ajustesRMS.Usuario;
                string Password = _ajustesRMS.Password;

                var client = new RestClient(UrlBase);
                var request = new RestRequest(Metodo, Method.Post);

                request.AddParameter("rrms_usuario", Usuario);
                request.AddParameter("rrms_pwd", Password);

                //log.Request = JsonConvert.SerializeObject(request.Parameters, Formatting.None);

                var response = client.Execute(request);
                dynamic responsedinamic = JsonConvert.DeserializeObject<dynamic>(response.Content);
                RastreoRMS respuestaApi = JsonConvert.DeserializeObject<RastreoRMS>(response.Content);

                if (respuestaApi.Success == 0)
                {
                    resultado.Codigo = (int)CodigoDeError.UsuarioNoAutorizado;
                    resultado.Mensaje = "Autenticacion fallida";
                    resultado.Exitoso = false;
                }

                resultado.Data = respuestaApi;

                //log.Response = JsonConvert.SerializeObject(response.Content, Formatting.None);
                //log.ResponseFecha = DateTime.Now;

                //_logger.RegistraLoggerApiRms(log);

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
