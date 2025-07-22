using Azure.Core;
using bepensa_biz.Interfaces;
using bepensa_biz.Settings;
using bepensa_models.ApiResponse;
using bepensa_models.Enums;
using bepensa_models.General;
using bepensa_models.Logger;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Channels;

namespace bepensa_biz.Proxies
{
    public class ApiProxy : IApi
    {
        private readonly GlobalSettings _ajustes;

        private readonly ApiRMSSettings _ajustesRMS;

        private readonly ApiCPDSettings _ajustesCDP;

        private readonly Serilog.ILogger _logger;

        private readonly Channel<ExternalApiLogger> _logApi;

        public ApiProxy(IOptionsSnapshot<GlobalSettings> ajustes, IOptionsSnapshot<ApiRMSSettings> ajustesRMS, IOptionsSnapshot<ApiCPDSettings> ajustesCDP
            , Serilog.ILogger logger, Channel<ExternalApiLogger> logApi)
        {
            _ajustes = ajustes.Value;
            _ajustesRMS = ajustesRMS.Value;
            _ajustesCDP = ajustesCDP.Value;
            _logger = logger;
            _logApi = logApi;
        }

        #region RMS
        public async Task<Respuesta<RastreoRMS>> Autenticacion()
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

                var logApi = new ExternalApiLogger
                {
                    ApiName = "RMS",
                    Method = Metodo,
                    RequestBody = JsonConvert.SerializeObject(request.Parameters),
                    RequestTimestamp = DateTime.Now,
                    IdTransaccionLog = resultado.IdTransaccion
                };

                var response = client.Execute(request);
                dynamic responsedinamic = JsonConvert.DeserializeObject<dynamic>(response.Content);
                RastreoRMS respuestaApi = JsonConvert.DeserializeObject<RastreoRMS>(response.Content);

                logApi.ResponseBody = response.Content;
                logApi.ResponseTimestamp = DateTime.Now;
                logApi.StatusCode = (int)response.StatusCode;
                logApi.Resultado = response.IsSuccessStatusCode ? "Success" : "Error";

                await _logApi.Writer.WriteAsync(logApi);

                if (respuestaApi.Success == 0)
                {
                    resultado.Codigo = (int)CodigoDeError.UsuarioNoAutorizado;
                    resultado.Mensaje = "Autenticacion fallida";
                    resultado.Exitoso = false;
                }

                resultado.Data = respuestaApi;

            }
            catch (Exception ex)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;

                _logger.Error(ex, "Autenticacion() => Api::{usuario}", "RMS");
            }

            return resultado;
        }

        public async Task<Respuesta<ResponseRastreoGuia>> ConsultaFolio(RequestEstatusOrden data, string? token)
        {
            Respuesta<ResponseRastreoGuia> resultado = new()
            {
                IdTransaccion = Guid.NewGuid(),
                Data = new()
            };

            var FechaActual = DateTime.Now;

            try
            {

                if (_ajustes.Produccion)
                {
                    if (!string.IsNullOrEmpty(token))
                    {
                        string UrlBase = _ajustesRMS.BaseUri;
                        string Metodo = "rrms_rastreoguia.php";
                        var client = new RestClient(UrlBase);
                        var request = new RestRequest(Metodo, Method.Post);

                        request.AddHeader("Authorization", "Bearer " + token);
                        request.AddParameter("folio", data.Folio);

                        var logApi = new ExternalApiLogger
                        {
                            ApiName = "RMS",
                            Method = "ConsultaFolio",
                            RequestBody = JsonConvert.SerializeObject(request.Parameters),
                            RequestTimestamp = DateTime.Now,
                            IdTransaccionLog = resultado.IdTransaccion
                        };

                        var response = client.Execute(request);
                        dynamic responsedinamic = JsonConvert.DeserializeObject<dynamic>(response.Content);
                        ResponseRastreoGuia respuestaApi = JsonConvert.DeserializeObject<ResponseRastreoGuia>(response.Content, new RastreoItemConverter());

                        logApi.ResponseBody = response.Content;
                        logApi.ResponseTimestamp = DateTime.Now;
                        logApi.StatusCode = (int)response.StatusCode;
                        logApi.Resultado = response.IsSuccessStatusCode ? "Success" : "Error";

                        await _logApi.Writer.WriteAsync(logApi);

                        if (respuestaApi.Success == 0)
                        {
                            resultado.Codigo = (int)CodigoDeError.RequestInvalido;
                            resultado.Mensaje = CodigoDeError.RequestInvalido.GetDescription();
                            resultado.Exitoso = false;
                        }

                        resultado.Data = respuestaApi;
                    }
                    else
                    {
                        resultado.Codigo = (int)CodigoDeError.Excepcion;
                        resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                        resultado.Exitoso = false;
                    }
                }
                else
                {
                    resultado.Data = QAREspuestas();
                }
            }
            catch (Exception ex)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;

                _logger.Error(ex, "ConsultaFolio(RequestEstatusOrden, string?) => ApiRMS::Folio::{usuario}", data.Folio);
            }
            return resultado;
        }

        #region Métodos Privados
        private ResponseRastreoGuia QAREspuestas()
        {
            ResponseRastreoGuia resultado = new();

            try
            {
                string jsontext = string.Empty;
                string noguiagenerica = string.Empty;
                Random random = new();
                int numeroAleatorio = random.Next(1, 3);

                switch (numeroAleatorio)
                {
                    case 1:
                        jsontext = @"{
	                                    ""success"":1
	                                    ,""mensaje"":""Consulta exitosa""
	                                    ,""numero_guia"":""8057353922502700005198""
	                                    ,""folio_estatus_codigo"":""E""
	                                    ,""folio_estatus_descripcion"":""En Proceso""
	                                    ,""tipo_envio"":""NORMAL Se env\u00eda por mensajer\u00eda Default""
	                                    ,""paqueteria"":""Estafeta""
	                                    ,""paqueteria_url"":""https:\/\/www.estafeta.com\/Herramientas\/Rastreo.html""
	                                    ,""rastreo"":[
		                                    {
			                                    ""code"":""01""
			                                    ,""spanishDescription"":""Recibido por estafeta""
			                                    ,""englishDescription"":""Received by Estafeta""
			                                    ,""reasonCode"":""""
			                                    ,""eventDateTime"":""2024-05-21 16:15:00""
			                                    ,""warehouseCode"":""MXE""
			                                    ,""warehouseName"":""M\u00c9XICO ECATEPEC""
		                                    },
		                                    {
			                                    ""code"":""02""
			                                    ,""spanishDescription"":""En tr\u00e1nsito""
			                                    ,""englishDescription"":""In transit""
			                                    ,""reasonCode"":""""
			                                    ,""eventDateTime"":""2024-05-21 18:33:00""
			                                    ,""warehouseCode"":""MXE""
			                                    ,""warehouseName"":""M\u00c9XICO ECATEPEC""
		                                    },
		                                    {""code"":""01"",""spanishDescription"":""Recibido por estafeta"",""englishDescription"":""Received by Estafeta"",""reasonCode"":"""",""eventDateTime"":""2024-05-21 18:34:00"",""warehouseCode"":""MXE"",""warehouseName"":""M\u00c9XICO ECATEPEC""}
		                                    ,{""code"":""02"",""spanishDescription"":""En tr\u00e1nsito"",""englishDescription"":""In transit"",""reasonCode"":"""",""eventDateTime"":""2024-05-21 18:34:00"",""warehouseCode"":""MXE"",""warehouseName"":""M\u00c9XICO ECATEPEC""}
		                                    ,{""code"":""004"",""spanishDescription"":""M\u00c9XICO ECATEPEC En ruta for\u00e1nea hacia HMX - HUB MEX"",""englishDescription"":""M\u00c9XICO ECATEPEC en Foraniana to HMX - HUB MEX"",""reasonCode"":"""",""eventDateTime"":""2024-05-21 18:35:00"",""warehouseCode"":""MXE"",""warehouseName"":""M\u00c9XICO ECATEPEC""}
		                                    ,{""code"":""004"",""spanishDescription"":""HUB MEX En ruta for\u00e1nea hacia TIN - Centro de Int. TIN"",""englishDescription"":""HUB MEX en Foraniana to TIN - Centro de Int. TIN"",""reasonCode"":"""",""eventDateTime"":""2024-05-22 02:58:00"",""warehouseCode"":""HMX"",""warehouseName"":""HUB MEX""}
	                                    ]
	                                    ,""documento_prueba_entrega"":{
		                                    ""tipo_documento"":""SIGNATURE_PROOF_OF_DELIVERY""
		                                    ,""formato_documento"":""PNG""
		                                    ,""documentos"":""""
	                                    }
                                    }";
                        break;
                    case 2:
                        jsontext = @"{
	                                        ""success"":1
	                                        ,""mensaje"":""Consulta exitosa""
	                                        ,""numero_guia"":""274177875571""
	                                        ,""folio_estatus_codigo"":""D""
	                                        ,""folio_estatus_descripcion"":""Entregado""
	                                        ,""tipo_envio"":""NORMAL Se env\u00eda por mensajer\u00eda Default""
	                                        ,""paqueteria"":""FEDEX""
	                                        ,""paqueteria_url"":""https:\/\/www.fedex.com\/es-mx\/tracking.html""
	                                        ,""rastreo"":[
		                                        {
			                                        ""date"":""2024-05-03T11:02:00-06:00""
			                                        ,""eventType"":""IT""
			                                        ,""eventDescription"":""En camino""
			                                        ,""exceptionCode"":""67""
			                                        ,""exceptionDescription"":""Un tercero proveedor de nuestra confianza est\u00e1 en camino con tu paquete.""
			                                        ,""scanLocation"":{
				                                        ""streetLines"":[""""]
				                                        ,""city"":""MEXICO""
				                                        ,""stateOrProvinceCode"":""DF""
				                                        ,""postalCode"":""13400""
				                                        ,""countryCode"":""MX""
				                                        ,""residential"":false
				                                        ,""countryName"":""Mexico""
			                                        }
			                                        ,""locationId"":""CYWA""
			                                        ,""locationType"":""FEDEX_FACILITY""
			                                        ,""derivedStatusCode"":""IT""
			                                        ,""derivedStatus"":""En tr\u00e1nsito""
		                                        },
		                                        {""date"":""2024-05-03T08:52:00-06:00"",""eventType"":""AR"",""eventDescription"":""En oficina local de
                                        FedEx"",""exceptionCode"":"""",""exceptionDescription"":"""",""scanLocation"":{""streetLines"":[""""],""city"":""MEXICO"",""stateOrProvinceCode"":""DF"",""postalCode"":""13400"",""countryCode"":""MX"",""residential"":false,""countryName"":""Mexico""},""locationId"":""CYWA"",""locationType"":""DESTINATION_FEDEX_FACILITY"",""derivedStatusCode"":""IT"",""derivedStatus"":""En
                                        tr\u00e1nsito""}
                                        ,{""date"":""2024-05-03T06:51:00-06:00"",""eventType"":""AR"",""eventDescription"":""En oficina local de
                                        FedEx"",""exceptionCode"":"""",""exceptionDescription"":"""",""scanLocation"":{""streetLines"":[""""],""city"":""MEXICO"",""stateOrProvinceCode"":""DF"",""postalCode"":""09070"",""countryCode"":""MX"",""residential"":false,""countryName"":""Mexico""},""locationId"":""CZAA"",""locationType"":""DESTINATION_FEDEX_FACILITY"",""derivedStatusCode"":""IT"",""derivedStatus"":""En
                                        tr\u00e1nsito""}
                                        ,{""date"":""2024-05-03T00:21:00-06:00"",""eventType"":""DL"",""eventDescription"":""Entregado"",""exceptionCode"":"""",""exceptionDescription"":"""",""scanLocation"":{""streetLines"":[""""],""city"":""ESTADO
                                        DE
                                        MEXICO"",""stateOrProvinceCode"":""EM"",""postalCode"":""56600"",""countryCode"":""MX"",""residential"":false,""countryName"":""Mexico""},""locationId"":""CYWA"",""locationType"":""DELIVERY_LOCATION"",""derivedStatusCode"":""DL"",""derivedStatus"":""Entregado""},{""date"":""2024-05-02T20:41:00-06:00"",""eventType"":""AR"",""eventDescription"":""Lleg\u00f3
                                        al centro de distribuci\u00f3n de
                                        FedEx"",""exceptionCode"":"""",""exceptionDescription"":"""",""scanLocation"":{""streetLines"":[""""],""city"":""CUAUTITLAN
                                        IZCALLI"",""stateOrProvinceCode"":""EM"",""postalCode"":""54769"",""countryCode"":""MX"",""residential"":false,""countryName"":""Mexico""},""locationId"":""CUAH"",""locationType"":""FEDEX_FACILITY"",""derivedStatusCode"":""IT"",""derivedStatus"":""En
                                        tr\u00e1nsito""},{""date"":""2024-05-02T20:07:00-06:00"",""eventType"":""DP"",""eventDescription"":""Dej\u00f3 la oficina FedEx de
                                        origen"",""exceptionCode"":"""",""exceptionDescription"":"""",""scanLocation"":{""streetLines"":[""""],""city"":""CUAUTITLAN
                                        IZCALLI"",""stateOrProvinceCode"":""EM"",""postalCode"":""54769"",""countryCode"":""MX"",""residential"":false,""countryName"":""Mexico""},""locationId"":""NUXA"",""locationType"":""ORIGIN_FEDEX_FACILITY"",""derivedStatusCode"":""IT"",""derivedStatus"":""En
                                        tr\u00e1nsito""},{""date"":""2024-05-02T18:09:00-06:00"",""eventType"":""PU"",""eventDescription"":""Recogido"",""exceptionCode"":"""",""exceptionDescription"":"""",""scanLocation"":{""streetLines"":[""""],""city"":""CUAUTITLAN
                                        IZCALLI"",""stateOrProvinceCode"":""EM"",""postalCode"":""54769"",""countryCode"":""MX"",""residential"":false,""countryName"":""Mexico""},""locationId"":""MUGA"",""locationType"":""PICKUP_LOCATION"",""derivedStatusCode"":""PU"",""derivedStatus"":""Recogido""},{""date"":""2024-05-02T15:40:35-05:00"",""eventType"":""OC"",""eventDescription"":""Informaci\u00f3n
                                        del env\u00edo enviada a
                                        FedEx"",""exceptionCode"":"""",""exceptionDescription"":"""",""scanLocation"":{""streetLines"":[""""],""residential"":false},""locationType"":""CUSTOMER"",""derivedStatusCode"":""IN"",""derivedStatus"":""Etiqueta
                                        creada""}]
	                                        ,""documento_prueba_entrega"":{
		                                        ""tipo_documento"":""SIGNATURE_PROOF_OF_DELIVERY""
		                                        ,""formato_documento"":""PDF""
		                                        ,""documentos"":[
			                                        ""JVBERi0xLjQKJeLjz9MKMTkgMCBvYmogPDwvU3VidHlwZS9Gb3JtL0ZpbHRlci9GbGF0ZURlY29kZS9UeXBlL1hPYmplY3QvTWF0cml4WzEgMCAwIDEgMCAwXS9Gb3JtVHlwZSAxL1Jlc291cmNlczw8L1Byb2NTZXRbL1BERi9UZXh0L0ltYWdlQi9JbWFnZUMvSW1hZ2VJXS9Gb250PDwvSGVsdiA3IDAgUj4+Pj4vQkJveFswIDAgMTY1LjM4IDIzLjk0XS9MZW5ndGggMTA0Pj5zdHJlYW0KeJwrVAhU0A+pUHDydVYoVDAAQkMzUz1jCwUjYz1LE4WiVIVwhTygjFOIgiFEWsFIwVLPUCEkV0HfIzWnTMFCISQNKJ6uoOGSmlik4FxaXJKfm1qkoxmSBRZ2DQFaEajgCrQAAIO9Gr0KZW5kc3RyZWFtCmVuZG9iagoxNSAwIG9iaiA8PC9TdWJ0eXBlL0Zvcm0vRmlsdGVyL0ZsYXRlRGVjb2RlL1R5cGUvWE9iamVjdC9NYXRyaXhbMSAwIDAgMSAwIDBdL0Zvcm1UeXBlIDEvUmVzb3VyY2VzPDwvUHJvY1NldFsvUERGL1RleHQvSW1hZ2VCL0ltYWdlQy9JbWFnZUldL0ZvbnQ8PC9IZWx2IDcgMCBSPj4+Pi9CQm94WzAgMCAxNTQuNiAxNS42NV0vTGVuZ3RoIDk4Pj5zdHJlYW0KeJwrVAhU0A+pUHDydVYoVDAAQkNTEz0zIKlnZqpQlKoQrpAHlHAKUTCEyCoYKZjoWZoqhOQq6Huk5pQpWCiEpAEl0hU0glKLM1NS85JTNUOywCKuIUDTAxVcgWYDAPxOGTUKZW5kc3RyZWFtCmVuZG9iagoxMCAwIG9iaiA8PC9TdWJ0eXBlL0Zvcm0vRmlsdGVyL0ZsYXRlRGVjb2RlL1R5cGUvWE9iamVjdC9NYXRyaXhbMSAwIDAgMSAwIDBdL0Zvcm1UeXBlIDEvUmVzb3VyY2VzPDwvUHJvY1NldFsvUERGL1RleHQvSW1hZ2VCL0ltYWdlQy9JbWFnZUldL0ZvbnQ8PC9IZWx2IDcgMCBSPj4+Pi9CQm94WzAgMCAxNTMuMjkgMTcuODJdL0xlbmd0aCAxMDA+PnN0cmVhbQp4nCtUCFTQD6lQcPJ1VihUMABCQ1NjPSNLBUNzPQsjhaJUhXCFPKCMU4iCIURawUjBTM\/ARCEkV0HfIzWnTMFCISQNKJGuoOGSmpNZllqUmqIZkgUWcQ0BGh+o4Ao0HAAQ5hlkCmVuZHN0cmVhbQplbmRvYmoKMjAgMCBvYmogPDwvU3VidHlwZS9Gb3JtL0ZpbHRlci9GbGF0ZURlY29kZS9UeXBlL1hPYmplY3QvTWF0cml4WzEgMCAwIDEgMCAwXS9Gb3JtVHlwZSAxL1Jlc291cmNlczw8L1Byb2NTZXRbL1BERi9UZXh0L0ltYWdlQi9JbWFnZUMvSW1hZ2VJXS9Gb250PDwvSGVsdiA3IDAgUj4+Pj4vQkJveFswIDAgMTU0LjgxIDE5Ljg1XS9MZW5ndGggMTEwPj5zdHJlYW0KeJwlzcEKgkAUheFX+ZcJMjrR0LTU8UItBpEu1AtYECnoQnr8LsXZ\/d\/iLAxU+qHNiYXa5sPBRY8\/uRhYR27MJq3i\/8yeo6sDOlGdx\/dGRB8GT3Zy1abr6YQs90vqSySXhb5+KmpXg5XEFzCkGy4KZW5kc3RyZWFtCmVuZG9iagoxNCAwIG9iaiA8PC9TdWJ0eXBlL0Zvcm0vRmlsdGVyL0ZsYXRlRGVjb2RlL1R5cGUvWE9iamVjdC9NYXRyaXhbMSAwIDAgMSAwIDBdL0Zvcm1UeXBlIDEvUmVzb3VyY2VzPDwvUHJvY1NldFsvUERGL1RleHQvSW1hZ2VCL0ltYWdlQy9JbWFnZUldL0ZvbnQ8PC9IZWx2IDcgMCBSPj4+Pi9CQm94WzAgMCAxNTIuMzUgMTcuODZdL0xlbmd0aCAxMDQ+PnN0cmVhbQp4nCtUCFTQD6lQcPJ1VihUMABCQ1MjPWNTBUNzPQszhaJUhXCFPKCMU4iCIURawUjBTM\/ATCEkV0HfIzWnTMFCISQNKJGuoOGo5+jj66gQ5uno5xqpGZIFFnUNAVoRqOAKtAAAWj8ZSAplbmRzdHJlYW0KZW5kb2JqCjEyIDAgb2JqIDw8L1N1YnR5cGUvRm9ybS9GaWx0ZXIvRmxhdGVEZWNvZGUvVHlwZS9YT2JqZWN0L01hdHJpeFsxIDAgMCAxIDAgMF0vRm9ybVR5cGUgMS9SZXNvdXJjZXM8PC9Qcm9jU2V0Wy9QREYvVGV4dC9JbWFnZUIvSW1hZ2VDL0ltYWdlSV0vRm9udDw8L0hlbHYgNyAwIFI+Pj4+L0JCb3hbMCAwIDE1NC4yNCAxOS42XS9MZW5ndGggMTA0Pj5zdHJlYW0KeJwlzcEKRVAUheFX+YeUjnMOVxiSMjFQu4zvwFVCMZD79na0hl+rf6cnlouqq9mxOvdJjU9xhck4RgY2hUpwr+LJTJEgK3E7Lic58lOYCLrvnyTCW71bW3oXyvxII1rpabRxA5pYGU0KZW5kc3RyZWFtCmVuZG9iagoxMyAwIG9iaiA8PC9TdWJ0eXBlL0Zvcm0vRmlsdGVyL0ZsYXRlRGVjb2RlL1R5cGUvWE9iamVjdC9NYXRyaXhbMSAwIDAgMSAwIDBdL0Zvcm1UeXBlIDEvUmVzb3VyY2VzPDwvUHJvY1NldFsvUERGL1RleHQvSW1hZ2VCL0ltYWdlQy9JbWFnZUldL0ZvbnQ8PC9IZWx2IDcgMCBSPj4+Pi9CQm94WzAgMCAxNTIuNzQgMTguNzZdL0xlbmd0aCAxMTM+PnN0cmVhbQp4nCXNvQqDMBhG4Vs5o11iI\/VntkS6tCB84CxpWiyaYIfi5fdDecfnhbPSk8tGe7+yctbZsjD1BduYuuIbGIgqrWAPpqAypUUW8luYfzTIS+FN1oWn23iMfkpxnHE+xbRMPp3ksx+caK3HaesP0pceogplbmRzdHJlYW0KZW5kb2JqCjExIDAgb2JqIDw8L1N1YnR5cGUvRm9ybS9GaWx0ZXIvRmxhdGVEZWNvZGUvVHlwZS9YT2JqZWN0L01hdHJpeFsxIDAgMCAxIDAgMF0vRm9ybVR5cGUgMS9SZXNvdXJjZXM8PC9Qcm9jU2V0Wy9QREYvVGV4dC9JbWFnZUIvSW1hZ2VDL0ltYWdlSV0vRm9udDw8L0hlbHYgNyAwIFI+Pj4+L0JCb3hbMCAwIDEyMi4zNCAxOS45OV0vTGVuZ3RoIDEwMj4+c3RyZWFtCnicJc2xCoNAEIThV\/lL05zuqazXKgdpUggLPoEKIQZMIT6+S2S6+WBmZ6S0k\/41sFN5JMZQN0gKKfGbmfi69IbcTESD1NhG+Zw\/Bx22OKwUURtR7bRtVR72\/pfZ\/GEk+\/4FL74YUAplbmRzdHJlYW0KZW5kb2JqCjkgMCBvYmogPDwvU3VidHlwZS9Gb3JtL0ZpbHRlci9GbGF0ZURlY29kZS9UeXBlL1hPYmplY3QvTWF0cml4WzEgMCAwIDEgMCAwXS9Gb3JtVHlwZSAxL1Jlc291cmNlczw8L1Byb2NTZXRbL1BERi9UZXh0L0ltYWdlQi9JbWFnZUMvSW1hZ2VJXS9Gb250PDwvSGVsdiA3IDAgUj4+Pj4vQkJveFswIDAgMTQ0LjY2IDE5Ljg3XS9MZW5ndGggMTAyPj5zdHJlYW0KeJwrVAhU0A+pUHDydVYoVDAAQkMTEz0zMwVDSz0Lc4WiVIVwhTygjFOIgiFEWsFIwVzPwFwhJFdB3yM1p0zBQiEkDSiRrqDhm1ipYKSjYGRgZKIZkgUWcw0BWhCo4Ao0HgAjZxhpCmVuZHN0cmVhbQplbmRvYmoKNiAwIG9iaiA8PC9TdWJ0eXBlL0Zvcm0vRmlsdGVyL0ZsYXRlRGVjb2RlL1R5cGUvWE9iamVjdC9NYXRyaXhbMSAwIDAgMSAwIDBdL0Zvcm1UeXBlIDEvUmVzb3VyY2VzPDwvUHJvY1NldFsvUERGL1RleHQvSW1hZ2VCL0ltYWdlQy9JbWFnZUldL0ZvbnQ8PC9IZWx2IDcgMCBSPj4+Pi9CQm94WzAgMCAxNDUuMDUgMjEuMjFdL0xlbmd0aCAxMDI+PnN0cmVhbQp4nCWNMQqDQBQFrzKlaXb3rxGtN4hCTCF88AQqiAqmkBzfj+F1b2DmoMfrj\/R5cRBs8ixcKIjiovAdGdiNJEX+mEjpyhzd8O24nlToZGAmE3Eh0iVvBt7NQ5f7r9UiPbUlLnHsGNoKZW5kc3RyZWFtCmVuZG9iagoxOCAwIG9iaiA8PC9TdWJ0eXBlL0Zvcm0vRmlsdGVyL0ZsYXRlRGVjb2RlL1R5cGUvWE9iamVjdC9NYXRyaXhbMSAwIDAgMSAwIDBdL0Zvcm1UeXBlIDEvUmVzb3VyY2VzPDwvUHJvY1NldFsvUERGL1RleHQvSW1hZ2VCL0ltYWdlQy9JbWFnZUldL0ZvbnQ8PC9IZWx2IDcgMCBSPj4+Pi9CQm94WzAgMCAxNDEuNzUgMjMuNjZdL0xlbmd0aCAxMDM+PnN0cmVhbQp4nCtUCFTQD6lQcPJ1VihUMABCQxNDPXNTBSNjPTMzhaJUhXCFPKCMU4iCIURawUjBQs\/STCEkV0HfIzWnTMFCISQNKJGuoOGbWKlgZKSjYGRgZKIZkgUWdA0B2hCo4Ao0HwA5sxiZCmVuZHN0cmVhbQplbmRvYmoKMyAwIG9iaiA8PC9GaWx0ZXIvRmxhdGVEZWNvZGUvTGVuZ3RoIDEwPj5zdHJlYW0KeJwr5AIAAO4AfAplbmRzdHJlYW0KZW5kb2JqCjUgMCBvYmogPDwvRmlsdGVyL0ZsYXRlRGVjb2RlL0xlbmd0aCA3Nzc+PnN0cmVhbQp4nK2W33OaQBDH3\/kr9jGdsVfuF4fNk1GapqNjY5hJpm8Ip9IoGMAY\/\/suqJEYTITpoA5zfm8\/u7d3uwe3xhM++S+DX4YJ1\/h+5RoUTHwoCAbCNsFdGN8eQhNscCcodBPDJMyyLAVrwyyko+vdSzI1LkbaD5ehjrLvX9y\/OD7Fbz4HrVOc4bhlBDdVE8bdLFwudXIOQZgg1BHgYGhvoKy2WK4+aJw7t9MbQs+BgfNw0x22wBm0YPDQ2k8+DknWIRbyY2R32Ol2+t0CeuWMRjfDP52rTr+K\/En+zAb5m+hER74+Y3FpuwJRCsNxnb4zYOKrzZV9ptMmKMFOWXRnGibxfB6vw2gKYQoZDiyTOJ58xU+g5+GzTjYoSSBLPP8xV0Wrxbi0V95GwIT8iMeUoErZSkpFz\/aff+C+Fz3CJl4VHvqzOE5zD3\/owHk527wlVe3zknnZKj15XD7hifrnM5xGOiiCHG+aYhmvjdXJc+hryDZL3ZSKx7EudYkVz5vDTy8K5pjOJuS8CjTIa2+74XGp3bgxtn56e\/tz1o99LwvjqClb1i\/9r+zAyxon2WaNsTcR7upF46iRLqXdqN\/lleI\/0I+r0+d090QlrZvs+uQ8bug1THTBpLWZ9zqczk5fXo6BHzhAJXtfT95tKbjX+jHwNpfHN4N8umyrtzeDkU7DAC9XecHZ78lS5zhwyn5Qk8g20O1hu\/j9rl0GOvPCeQoeXqo8LNka++slzOK1xr9bEMWQYkHHFpLovOd6z6j2xnO97bMzHCo6GDgvy0SnKaSYtgU6Cblnr52WM0LRC6tYkIux9r1VqsEr2V57KcIySPTTKsSqRvL5GNUtPMFhC7eJoFgs0RoFfwEYMoVeDJgGKAupsImywaKS4CJuheyUkGGmuCBc7oS8UqgUaYNkgpjWTieqdALX2+IgFSeU74SyUshMgncPaTJi73TWKQ\/5Lua9QVVNlkQKsE1K+D5mu0rIFeEWKGURxXa6drVBXG2riJnvhdSsVGJelMANy0l7vzr0kBjjH5cr1jkKZW5kc3RyZWFtCmVuZG9iago0IDAgb2JqIDw8L0ZpbHRlci9GbGF0ZURlY29kZS9MZW5ndGggNzM+PnN0cmVhbQpIiSrkMrU01TOwMDBUMABCCxMjPQMTMDM5l0vfM9dAwSWfK5CrkMvMVM8IpsrQUs9IwcREz9BcwcLAUs8EqtYQohYgwAC6IRDfCmVuZHN0cmVhbQplbmRvYmoKMTYgMCBvYmogPDwvQ29sb3JTcGFjZS9EZXZpY2VSR0IvSGVpZ2h0IDE2MC9TdWJ0eXBlL0ltYWdlL0ZpbHRlci9EQ1REZWNvZGUvVHlwZS9YT2JqZWN0L1dpZHRoIDU0NC9CaXRzUGVyQ29tcG9uZW50IDgvTGVuZ3RoIDk1NzA+PnN0cmVhbQr\/2P\/uAA5BZG9iZQBkAAAAAAH\/2wDFAAwICAgICAwICAwQCwsLDA8ODQ0OFBIODhMTEhcUEhQUGhsXFBQbHh4nGxQkJycnJyQyNTU1Mjs7Ozs7Ozs7OzsBDQoKDAoMDgwMDhEODgwNERQUDw8RFBARGBEQFBQTFBUVFBMUFRUVFRUVFRoaGhoaGh4eHh4eIyMjIycnJywsLAINCgoMCgwODAwOEQ4ODA0RFBQPDxEUEBEYERAUFBMUFRUUExQVFRUVFRUVGhoaGhoaHh4eHh4jIyMjJycnLCws\/90ABAAi\/8AAEQgAoAIgAwAiAAERAQIRAv\/EAaIAAAICAgMAAwAAAAAAAAAAAAAHBQYBBAIDCAkKCwEAAwACAQUBAAAAAAAAAAAABAUGAAMBAgcICQoLEAABAQQCBgUKCnsAAAAAAAAAAQIDBBEFBhITITVzkRYxQVFTFBUXMkNSYYGh0QcIIjRCVJLh8PEJChgZGiMkJSYnKCkqMzY3ODk6REVGR0hJSlVWV1hZWmJjZGVmZ2hpanFydHV2d3h5eoKDhIWGh4iJipOUlZaXmJmaoqOkpaanqKmqsbKztLW2t7i5usHCw8TFxsfIycrS09TV1tfY2dri4+Tl5ufo6ery8\/T19vf4+foRAAADAgEIBwaHAAAAAAAAAAABAgMRIQQSFTEyUXGxBRQiQVJhkQYjM1OBwQcICQoTFhcYGRokJSYnKCkqNDU2Nzg5OkJDREVGR0hJSlRVVldYWVpiY2RlZmdoaWpyc3R1dnd4eXqCg4SFhoeIiYqSk5SVlpeYmZqhoqOkpaanqKmqsrO0tba3uLm6wsPExcbHyMnK0dLT1NXW19jZ2uHi4+Tl5ufo6erw8fLz9PX29\/j5+v\/aAAwDAAABEQIRAD8A2KYrzSsFGP3DpZMu3jTKTXOxkfqRqbz9qu2Iysl84rDtEMN2cTsjSRmknuIEkgjFs1I1N8BV2wakem9hjXbFTA66rMrCQy+MhbNSNTa52NQ1ItN56Y1KmBlVmVhIZfGQtmpGpvPTGoakam9hja25UwMqsysJDL4yFrXcRqb2GNrbgu4jU5K5LG1tyqGTKqsrCQy+MjDRqPWekKciXzqLW4wxNLqr6EurKzSYs9wt3eROD9qMxMoWRYgkNlEUDnDQ0KNUZEMgBk0DpGAMgYMGAMgYMGAMgYMGAMgYMGAMgYMGAMgYMGDIAYMAYXPMmFMGCpVtrDG0TEu3cOskaRV9AVF5uIVMstqjKrcuZa7cmdxEuRjiWuqLx9yNa62MolYs1M0mZPNwIQgjTZi0akOm9e2q7cNSHTevbVduVQAiqzI8gkOYwiFrXcRab17artyVoiuNKxjppt6qzRrPUX+YWSru7ZrroV1OpKJ4jUtmUaqOTCQLqPYM2jZy0vKNlC2JWWkFzVxqC1lpDPXGpEASdXW9jO1DaR8S3LIS+SWPz1xqCVlj89cakQBlXW9jO1GSPiawFaCXySx+eqbNQySR+Y0uNduRAGVdb2M7UZI+JrllaCYySR+euNduYySx+euNduRAGVdb2M7UcSPia5ZWgl8kkcuWq4125ZaCi3kZCW16s2rLNulEZyy61Wvf20G1GRQ1at3LUZkSDgANSsTMmLAjQkiOOKETIGQHISAAAMGAAAMGAAAMGAAAMGAAAMGAAAMGAAAMGAAAMGAAAMGAMGTBgwf\/0ISst84rDtEMTNZb5RWGaIYdsphNAqQLSAAA2DkAABgwAABjxgAADHjBe9wu3eRGDGYmULPcLt3sRgxls5SCiLrjqugGazRjkZAAcdAAADBgAADBgAADBgAADBgAADBgAADBgAADBgDCmTCmGMC93EXd4465UXj7kY11sYe4iJOMcdcqLx9yMa62NoiuEmgCWUyVAcAAAkdZgUslXd2zXWytlkq7u2a66FFtJiQVqommDKjbjlQMSoABE1weAAAOBgAALpy4cPIABJTElOBy8p45M5Zdar3v7aKUyXWq97+2hjURiY2QoK6mnVXLU6RNAAD4T4AADBgAADBgAADBgAADBgAADBgAADBgAADBgAADBgAADBgDBkwYMH\/\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\/0RUmkWUcRVuc2Cso1LdU7uyLO7oOIZu2bC4\/kQVUXISZkUI1qaOCYVw+RVRXbSKmXulUkdY1qSqnHPWXrxHjuSsrLL+RF7SdBRNHMW562wqK1LdKqp1sopS1dPHJKI6IizvhIt9BPkfuFk2zlKspbU6AuSum9xG8jlGQ61QkGRUqsUTFI\/0respJUsZyTLyy8uHqPHLLatIs01ZLqCKgI5mFsrKcmkzBrULSbrI27eyalalXYi6LInjTI0lNKIhpaplGOVb6XeQEEw8hXiMtK2iLmiupmk4ikIq3v2kaasZTS5lEtWil3cdDI6d2SKjzNKtNVy8sJiVgSEkZy3DrQlxFPGVy5pmmAAJlDrcADvh4VuJeMO2FRFbWSTJ6GqPHxLSssvWUXYnQtolB1o3DDU6WK0BaHtQ6QdIqtPWFsUmuyICJgHkOjStKi2KyWRiWqFSjeMIyOvHQiyOKhmAdcFcOYAAAGDAZpY6u7tmuuiuFjq7u1a67FFtJinq1UTTBlRuJjZJiWXLAAIhweAADCnJEMGQMAhy4YMgAHDhjxlnLLjVx2y3RqoqZaqhTmcsutWL39tKMKiifFDq6MOmFlTRuieCuWkRVK0Sy7dNvWGbsyAbYaZuKklGJFw+lDpXaSuqVSmaMbdNsyuTQ2VIRE6tpKV9Y01GxfHFfGs4YXCDMmW2bBpWc4wLHBwRwEMpulUaTLRSXoikX2lbDLbSIyiEOcmWlZuoslQ62LZTFRG+UY0xQwS3RGmTzdLMMOEiEfszRpGpZqHXHxTLqGetMtJZImUVyhqZYg3LTDxFVVU146lWX9myzNLIcnUig2TyOtGRwV8oJE1GqvnMnVlKihrh00hHvYtlEbXKyiPnNVUOtgJmi1NDjlG8zD1izSySSElAUIDBk7XTlXu6kOkkxxuIdalEkjM5Q5QUNbohlhplVZUt9D0e6hWmlYZVJpmnRRlEvHdqfrKSJPKJ1Eko7iCI74yj1lWq66kEFSUXXzHGIOAiccNeRjLLMkkckSQAMSgCwBkwZMGAAAMGAMGTBgwf\/1IOsd8YnDNEOTFZL4xGGaIceMphNAqQLIAAGUdY5HNhy8eNIywyrSqskRLqm5pmpPes83wpuVcRFpSFwqTHSw7YsUmymUmYgJFMVGwNJET3kY1rXGwBErQ1J71nm+VDTNSW9V6vbK7Ye9qd66mJAV2711MSGmSKrCOi+YyCI0zUlvVe75a2xrtw0Q7VUeO2mLFZLNFRR\/NO2ERd0piQUtZnzCRkUxLeam2J4qU2UZOlEOpDQ1nQFWUwABsshsAXOpkHQ8RBtN0g0yy2jckRp5Ye1Qpi5xzZbsUka2qI9LiNzzljgyeTg9IakaJcuGHbMU5RGWUREV4yq3Otqdi0vRe9tx38Y24iLbnhbEzgORzzM48a74Xh6t0rRLbKsNRblUVFRUtjO3KXXdzQiUbODbdtPLNFWxeWS4rJRfWzOMK3ZJI62URGyUSo57jlDlDJxvA8RlF3Tv5wMmFDBtcC5mllhqfjXFFpCu3zKMIwqWNyZWgmp0LQS3PrjHBuUO58\/fPlVG1nNZ5h0gB1k4iJ1cMInAOx06be3GGWmtil060yyzVUgGot09VFlJToaqjEmYwzcTxZ6vVWo57Cw0Q9h1V4qIqqW9zRkI4WbtiSmKJco6gXbvLsWZG7ITtmqmijecAGUp5mNVuj4Z5OyZy0kpVa0VYo11R7byHcqrdlO5NcsuaGvHObe5sNimXlHDNqpBkZHAVcOEm4yMI6k4JYN4yxYKxNJyVJGjmyLbuILhYeMdMtSus+1Kki3BwxVHsyVPBKDeT54AADrOUOoBY6u7tWuuiuKWOr27ZrroUW0mKerVRNMGVG4mNkmJYAAiQ8rwHN26betWLDKtLnJdOCE1VZlluNVFSe6TdE7O+Zohm90cbnjRFTU2LJbQifGk9wjdIYnQmsQaQxWhNYhho5dLvFMQWl1rqYkGsh02Mwok20lRhWoXmkMVoTWIzpDFaE1iGFanWupiQzaneupiQyQ6bHxA4k20sBBeJBRN3iU1iLdVx227gbFtFZWyyluEpaXWupiQ5Msss3ESRviWICiZccSnwAeK6kVRUgkGkiIje8ZRDVioFzFKjTxJynI2zChikkonGTyMBpUaTeUBzxRKTgGnb94rDC2LK5ciMlIYFIQtucPGUS60yslKdSFHNwbCNN3ZiKLojNib0lAb6Yf1HxeTYoxZuMiIisxoACpJZALzswzJzhyZeNM6qcVVVWagBj4BxGlLIYUDNzNOTDNm0jCZqohyUsqI5M40qAy7dq8WxRFVdgWSg6LcPXCtv2Fmi5S3Dqoah22H6PGlRUlmlncuEds2I3qPiKU0aFXGTq4JKk4vI3smRnXPMhydMMu3aMM6qiXDmgIZQbETgnhe973gMgBgwAABgwAABgwBgyYMGD\/1YOsl8YjDNEOTNZL4xGGaIYeMphNAqQKTdgGFMgp1jqE5Vu+kLhEHWxqqdaEnVu+kLhEHWxqqdaFlSE0mgY0NpZUByAAAxqHFvVRM1mba06RSLr7Q529VXrQnKzO1SkYprpNQuIJpVAbGMsxW80AAaDeQAOTtizWU5bUnaIqi\/pZ0r5iIZYRFlJWVX0CocLWlBQjgzcIAJlv1HcWuVFu98NfJBqO4rNi3adstbc1VYZzxxHEKhNQLeu4eRWZFu98rtzGo8it7bC9srtzKsM54yPSKjMC3ajyK3tMb5XbqGo9iU5emMSnNWETxyS0iogW7UfRO9lnZMqREdQDyBbbYaeI1YJnHKW6FyjeMSpKpQiABcsDZdx1AUvm4eO2W4d\/ZJOTSFDyy7VBiUcOH6Kk5tGiKn3wm6W+7joXMnRDNhkRlyyiJK4dp0wjdscMtJcmiHcJzAwDDSIqSMnB41YpZZymDAstxOYRI9x1x7Uo8pIXbcTHiNR7lUTeHtSkqOIlffCigCmcyQAADeOoYUslXt2zXW1K5mFjq9u2a62KLaTEgrVRNMGVG3HKgYlkAEAiQ7rwE5VS5Hr1wQZNVV3fr1wExDiYZ6muwGqQxKttSmLoZOKKZRSlEq4ZAJoEzHjAAYVQRTh4wZBQA5GDiqI0iouUqERTUHDtumUaSd0lXjVgyrechAU5SbLLtm5mg0WLZpZqj68ruCYiZtFNkxlcYrca6ZdPlZZyjXOyIe254raHWTjRxqONlCnZkZISSpYAADoGwCknR0O4bsFaS7NCMlcNuFi7U2wkpyaQ3xOZJWUc50EsDxSSlMzJL3xpyhfIeHdO2GWmUzEO9M8joCkmYmTtElcJBkpGKkqQRoc6vcJZqhaFmS3vOeORkwBsHQMgBgwYMgYMK0YMGTJxstiCtIcPGDkYMIszJyMH\/9aErLfGIwzRDITNZb4xGFaIZB4yuGmgVIFJACgCnWOoTdW76QuEQdbGqoJSrd9IXCIOtjVU60gsqQmk0DGhtXUByAAUDGoYbylFhWaCk+intrWSTWYz1KrWei2tN0W\/tm8VWRuiVcauiY6kG4yswostV62ByaYVFWeecRwUJUQUUI5MNKys0uDH3D57CLR7elLbKNK0srJqxUW05ZZNULTiUY6tbTlXk2kVJKie0U1RUzNqhxWQ6Fk8rqHKzBwjbKNIwioual0zpFCprNExkTRFPpEuXLFoVmzZS6rSfIk4izFCiUgzIxoN5Dp0jhdcT0B1PXdHOUm+VhhM9VkcoyK0nctvFZRqwZVZTl7RSgU3XNh65adpCqkmpTs0X21DrZMltjglDlKTULxb6F0V1vrfwt9DLlPXW+k24qslTGY5XfSbYMlTOgLvpNsEVSXPHXfGYatuofRXW+k25Uaww0G80oeuUZXdKyVng1KwlamdBVNnvxY3TlY6hmohFsbNhbi3TlDE2BkozOExwSYw3heNJJpUyrqmDai4NYdVaVZzU1RiRkZPIbyhAStD0g3Bo0yw3YTW7nEUZTLOFJjiMp44Mnk4O+gKQcPaOcTeo020ykyXRbgoqFrUkJaXKuVasJJOZeoKtjMStijqUkTNFLaJlpUZkUAHUzOEyFkUjqai2IWCaeK3YSXLNB\/We1Iq2qaIirllPrDXNmkYJuEtKsrZZczpZROtSieUDxiWZmZCJrjGMxsS7aZeWyTOWhXUOx89tizTMOvMG6ExiEkVcQ3kToAAAHUOQLlFjq9u2a62VwsdXd2rXXSie2jxIK1UTTBtRtxyoGJYAAiw8rwKSFDRCQ0TbFasUsZTI8JyOWSzZrJZS0mNbVkTVCkHKUThbUpljRQ08saKVKahNQ2SbQAnUSzFt08sZj1A09JoqFSmBxJNoMkUzFt08oty3IStHxCRLhHiNWV3LF+zll0qxe9OthcQRYtu2NJ2EwFUhEKInYxybEVITKGFUFuEfSNIpCqjMpzGa1pZlHKOAgrQhTQ41Msxq0pSbLt29dstyaRFKi\/insQli8WySZ3R8Tbn7xrKmqmnlE9FkVG2U58BPK6PFLEMRpYIe4nm43173AAABEk8Gkb4QAc3bpW0mcWmZLI5NJkT3S68cRxGZk+EhgJ2KzS4AGEcIxz7QSdEUi9cxE2m5MyLdR8WkS6VtG7LYi\/RZKTdC0okI5taszmoyiCLDZmSFHA468K6koiJoRtEFWpV0FxzjkdLh6j10y3lTQ7UVR0SiURGVeERlGm6vHIwoAqnI4HBW0Sc1lIi4yObZRUYbunbGxdpe2tUy0NCJgrB0sQq3FuyBW7VRkokZBlgpgyS8jXKVKfXjqSkY3X19CbMPGRbTSK8aWxzcw1YV1pQiqiykdza2DCsZYMhbR0eajdXApaGcwSSe+W4TTmJdPVsWGkaXYHchA0OtrfNT4GWTrDVkkw1g1vlS84DADdlfE0NJG8h\/9eErLfGIwrRDITNZLtJRF3WrRDqks3aDxkdYKgQKTKIYAAuHWOoTlW76QuEQdTGqp1pBK1cvpCy0RB1O9UTrSCypCbTQGhtLIcgUDIGNQwalJQ7ETBvHLTFmjTKpY55tgqHJG43lXDJQU9a6vtQjlhqGhVYVWrskkVF47eOWrB6yrLWco8qZolaUYZYR7a7FZ5U\/aixrJQKw0e9RXtlYpru\/jKJYqjiJKpY3s1isLdMoqos0WUjKs2Kql1ZL1r2qgmeGUhtKEb8NT1LQ0kcxTxhGcqUjfZrdWFLq0g9xptiAUwl3LNZsWajeZEODJM4TL+tVPvJo3HPWmVuKk02xFvYh89nZtK1O7uo6wOpKEolERDgiIgJILq3EzQlPPJSh6JSkY124tlrs82SrtjlSiSRmdcOTgKWO2g6Jex6Nqw5V6jK50xpUVRbp3Q7Dh45k1YSsVQ1ar1cShkepb7ajaz1WSJwJlkZZkklzBXFMUGsyccBGB1recAW1cqE0ng2W3MPYqreWiSzijPXTxy1YPEsVy5DxpqitOrhlzZ2uSzmLKtFA6RRysK9s5MznKQREcUxySSo542MlzxWAMtJJVRMxTCBpSoBsgHJhtphbJm4qG07paOdLN29VOtGmBwZEcscmTxutUzSLU7J8qzuZZqtPXjc7JZqpxuZpzcOUfPLCcpoYRJTKIcSh19bA3WqPRi6jU7hqNs2DSsnLyHDxxAOBlAYOQFjq7u1a66UrhY6vT0na66E9tHiQVqdNMGVG3HKgYlkAzIwqEUHryAgAiSMmOHDyGAM3AOBjyGAMyCRgx5DLOWXOrF7066UpjKXS10DEWmjVTOmMai1EluZnKjFBbUsk1MCIq9aRI0hHO3LhVRuSoVOk6ReP20VluZmkKTbfWbqxldW6RizzbpkWxabU41JwEOmo+ICZJJapquoGBVVpVVVmAXVC6gBPDSBzngNqChW3z5GUZsjodsK22ymVNS2UTRVpbYfTTKS4ERHEpxQsnzL4QHFsVJidmZPhMjcM0XQ8Pamrc7u7EiqRoxp2rxtl3JlmclLjYomVcNaOcW+Hbd5U0yxw1iJCmUakoUEbgkYRe0S2j1HApRPhC9VFS6pgkqUo7SNEVWpzI0RNGSmSjJROMhRsWqWyCWk4DAiyObLxthUsVldOAXTpIzKEpY6jJ8uEWKjKVbtrt28e7pzUUsrmIdveRbSL1oXjl4rDaNLdkWOgaSRbJFY2aqNIgi04Gaz8EJqkYgIiNqgpmuKVCLQizMKdbp7bGEa2B2JdG8uEq8JznCFpXd2idaO6kEnR1zLkhzjoJXz22TlJJmo9i7cysKrMpXJgTQow2xKyHKog1FvCYxsN8SnmIqDWJu2ueXmTJV0yyrmb1N1bE7aOg2XSNIqznlHRFSRppn0BqQyNkzI1G+CZnGNzVqTVoaUwOMjeOcOjFmtryyWhbK17qIah2VafrNCdYSSSCYkI3R0p4Fis3LjZcEsf\/\/Q7KXq1SERGPnjEMrSNvFWeWaORKk57tGtkg3ZGZBKYtaJKNcUA2E1MnWQUC1SpTMhGsQJVKlM2EaxDfkYki5hzV5pZDL5TMLSh6t0g4j3DxuGVllhtFVVSQynaSZRDMgQ0tWxtjI1FKIdKlGoZMmANY6RkwoAYMHGxzVNCNoiAiWW23sOw22rK3VZRVJEwqIckZlCRuGEbgtqbqo+WHa0jgJN2U0VllEUq0TVumXLVi3CPGVlPKHirKKkiMpCiXkY9s2VZTdMrvBKFMotUlxHKG1LVwSzdHRzC7qctJLLmdawkQmslGk\/qVFPVVUeu7ue018rU013D6NVbr51vpv5UElF6IISHVfKQXbNHxbWqumrp3u6Bpd8snUK22uwGG5qLGOmkW2ulkuvN\/KyZgavvoVuzbbZXrSr8ih0tIuTkEyMcXylXBeUZVOlGmWWn8C0u6knNBiUVV+AhWXT3SV27esspdkiNTxEs4c2pixW6p2oBtYqW0gOAhrUszGGWWWUkiSOQAaIR0jCoiyNCPomBi7J4+cMvHisqiKqEgB1EZplQDCNwVtN1WikdvGoaEkqtLYqiFYfUHSbhZPXKsrsR6RDhH7FhOUyGpGrTUc0jTL1GZJK6i+0DWMXGknKMbUtXBNtQkQyu6mJKhhIZ+0txkZj7cPXr1pWtKWEn0i0cHe4cvWFmsUxvlo31dZTx13ypC\/h6HpF\/Yq7dKqK1It1WqqxbEcw3GwvEuxy1LVR9VWoNhllXyNWKzuIpPsO0YZRLiyTLB28Wx0CZQ1qaPlCkU\/Vx40+ZWBht0WN2UkuldeVSpVptV0lUbaoFimcakRY0STpdEcXyGFElUaV3qqC1RpXeqo3ZImYEkXMOqrzScOb5TCiyI0onLqpL0TV+Oh3Fi8h1ZWcxi2KBYsg8WNDixlfCuZMyO6kOuJ4qWwVHlCYpOmaK0FQ0zRWY5Uu9igWKZwskSynmCpLtpxCkaZ4vQVDTNF5rlS72KBJM45kUynmMku2nEKRpmitBUNM8XoKl3kmcFimcZIplPMZJdtOIUjTNF6EoaZYrQlLvYpnBYpnHEiWU8xkl204hSWaGik1kptuYGOcubWyyrKLmFrsUzgsUzjYmo1mkzMjOEh0LqTaLJykkZClN0NGNtKqu1OKUJFJlulLvYoFimcdB1FMjMzeOoqlmxOKNJxCkrQ0VoSmNM0XoSl3sUzgsUzjiRTKeOZLtrCQrkDQzLLDKvnW6pzJ926ZYZREuSQ7LFAkFsInSwJyQG3ihbdT1GCRhWUVFRTlKQG4ahG0lAOollOJdkVyKoaIV6qO3KohdJTMWKIDN4iZtzeYKiaLWsTk4obIUhKGik1ioaZorNcqXixQLFDRIplPMb5LtrCQo+maKTWKnc4o2Oc8i3aszyy5WKBJDlNRjNJkZKOAcKqWaqIyNKXGNOjmXrEMyy9y0zzcRAkhmQclMaRERvcAFHHKNTiJ84cGmGWku5xGRME1Jpt0zurMJaSGJHS0ZpXLHLNopkcArzLukWcqyTrUztcwr9p4ivWVWeXMnJIFihqKJiI3mozKcN5xWZvckieUsh0OYZy5VGmGbFdgbCGJGQgiIiIigIgM9Rm9RveP\/9kKZW5kc3RyZWFtCmVuZG9iagoxNyAwIG9iaiA8PC9Db2xvclNwYWNlL0RldmljZVJHQi9IZWlnaHQgMTc1NC9TdWJ0eXBlL0ltYWdlL0ZpbHRlclsvRmxhdGVEZWNvZGUvRENURGVjb2RlXS9UeXBlL1hPYmplY3QvV2lkdGggMTI0MC9CaXRzUGVyQ29tcG9uZW50IDgvTGVuZ3RoIDUzNj4+c3RyZWFtCkiJ7NM5TFRRFMbxc98s4JKJbxxRxsoZxCUxLyCMgzEjI+NamAAuQGEiDgo2BFxQOty1c186ccVOUUQrF1C0kn0YY4EbamHct+aJUGigISH5Xky+U96Te3+3+Zsx8724wtHy4hKJSv8o86k0i9vl8rq8fq\/PWGCkpswJhMJpRiAUzC7IDoYiIxjlcbunejyG3zAikeBIbgwZbbQPmM\/ELivNO6InOuP2mE18onSl6ZrZpGpFKRGHQ\/6OU2k2uyMhcczYcX929oR\/djKwcw4sJ6jBo3U5w5e6e6JnUtLkKcneaT5\/yvTUGTNnzU5Ln5uRGZgXzJofXpQTWbxk6bLlK3Lz8letXrO2oLBoffGGaMnGTaVlmysqt2zdtr1qx87qml279+zdt\/\/AwUOHjxw9dvzEyVOnz5ytPXf+wsVLl+uuXL1Wf\/1Gw83GW7fv3rvf1PzgYcujx61t7R2dXd2xnnjv8xcvX73ue\/P23YePnz5\/+frt+4+fv\/T\/78tmXMbb+v+pa7oslEaxYJT5xCK31SK3zSK33SK3wyK30yK3yyK3m\/1CXPaLcdkvxmW\/GJf9Ylz2i3HZL8ZlvxiX\/WJc9otx2S\/GZb8Yl\/1iXPaLcdkvxmW\/GJf9Ylz2i3HZL8ZlvxiX\/WJc9otx2S\/GZb8Yl\/1iXPaLcdkvxmW\/GJf9Ylz2i3HZL8ZlvxiX\/WJc9otxe34LMABrLfmjCmVuZHN0cmVhbQplbmRvYmoKMzEgMCBvYmogPDwvU3VidHlwZS9Gb3JtL01hdHJpeFsxLjAgMC4wIDAuMCAxLjAgLTMxLjUzMTIgLTY5MC44NTRdL1R5cGUvWE9iamVjdC9Gb3JtVHlwZSAxL1Jlc291cmNlczw8L1Byb2NTZXRbL1BERl0+Pi9CQm94WzMxLjUzMTIgNjkwLjg1NCA1NjguMTkzIDcwMi4xMDRdL0xlbmd0aCA0OT4+c3RyZWFtCjEuMjUgdwowIEcKMzcuMTU2MiA2OTYuNDc5IG0KNTYyLjU2OCA2OTYuNDc5IGwKUwoKZW5kc3RyZWFtCmVuZG9iago0NSAwIG9iaiA8PC9TdWJ0eXBlL0Zvcm0vTWF0cml4WzEuMCAwLjAgMC4wIDEuMCAtMzIuMTExOCAtNjY3LjQ3XS9UeXBlL1hPYmplY3QvRm9ybVR5cGUgMS9SZXNvdXJjZXM8PC9Qcm9jU2V0Wy9QREZdPj4vQkJveFszMi4xMTE4IDY2Ny40NyA1NjguMTkzIDY3OC43Ml0vTGVuZ3RoIDQ5Pj5zdHJlYW0KMS4yNSB3CjAgRwozNy43MzY4IDY3My4wOTUgbQo1NjIuNTY4IDY3My4wOTUgbApTCgplbmRzdHJlYW0KZW5kb2JqCjU5IDAgb2JqIDw8L1N1YnR5cGUvRm9ybS9NYXRyaXhbMS4wIDAuMCAwLjAgMS4wIC0zMi4xMTE4IC01NjMuNTEzXS9UeXBlL1hPYmplY3QvRm9ybVR5cGUgMS9SZXNvdXJjZXM8PC9Qcm9jU2V0Wy9QREZdPj4vQkJveFszMi4xMTE4IDU2My41MTMgNTY5LjkzNSA1NzQuNzYzXS9MZW5ndGggNDg+PnN0cmVhbQoxLjI1IHcKMCBHCjM3LjczNjggNTY5LjEzOCBtCjU2NC4zMSA1NjkuMTM4IGwKUwoKZW5kc3RyZWFtCmVuZG9iago3MyAwIG9iaiA8PC9TdWJ0eXBlL0Zvcm0vTWF0cml4WzEuMCAwLjAgMC4wIDEuMCAtMzEuNjI4NCAtNTQxLjU4MV0vVHlwZS9YT2JqZWN0L0Zvcm1UeXBlIDEvUmVzb3VyY2VzPDwvUHJvY1NldFsvUERGXT4+L0JCb3hbMzEuNjI4NCA1NDEuNTgxIDU2OS40NTEgNTUyLjgzMV0vTGVuZ3RoIDQ5Pj5zdHJlYW0KMS4yNSB3CjAgRwozNy4yNTM0IDU0Ny4yMDYgbQo1NjMuODI2IDU0Ny4yMDYgbApTCgplbmRzdHJlYW0KZW5kb2JqCjgwIDAgb2JqPDwvVHlwZS9DYXRhbG9nL1BhZ2VzIDIgMCBSL1ZlcnNpb24vMS41Pj4KZW5kb2JqCjgxIDAgb2JqPDwvTW9kRGF0ZShEOjIwMjQwNTIyMTYwODM4WikvQ3JlYXRpb25EYXRlKEQ6MjAyNDA1MjIxNjA4MzhaKS9Qcm9kdWNlcihpVGV4dCAyLjEuMiBcKGJ5IGxvd2FnaWUuY29tXCkpPj4KZW5kb2JqCjc5IDAgb2JqIDw8L0ZpbHRlci9GbGF0ZURlY29kZS9UeXBlL09ialN0bS9MZW5ndGggMTc2NS9GaXJzdCA0NTIvTiA1OD4+c3RyZWFtCnicrVfbjts4Ev0Vvk3yYPFOSovBAE7SwTY2STdywVyMfqAl2q2JbPXokk3v1++hSla7gwkwO70PdvFSp6p4WCKLSjLBPPOaSSaNY0oxKz3LmfWWKYx6K5kWTAkF6SELzYzBlNPQZMpoy2zOlBeGOUC8d8wrQJIxw7SUmLSQyYiDdJA+SYznkNBXBWQOZeBkgT+NJiLRaQg4uICEMvAKdlNTWUwCp5xjBiqqgESIcMxMMiEVM7CjlWAJouHUwK+2kMBrBGuA1wjWAq9zwWzC5Vg7cEagDxcG\/q1NEuOwY2AMJrTBJJaijVPMwa9B3GhqCxyogcyZA95KxxCitiDDAWexCCxdWww6+LfeMJ+WmCvmYccW2I+EK7Ar8OsQNygC3ZZ54B0W5RMVifwkCvbjj\/zDuB3u7yL\/iD85\/fPX7XHgL0Ifp8Y\/Y\/MlDnUZ+MWxbKv6uOc\/18f1sa9P\/Z9+SobehUOclL8x+SeGHrycTCBtBHtPhl5iIh6HfqPTGDPTv03\/N4S8DvvI38e+Hbsy9kBcd235IQ4bfv3qNf8Yvw788gCdFyRekri84b9cbX+P5QDELzU4TCbRkoIVc9MzdKhpmJRz0zJkBDUVk3puIsPNyQKTU3xwhKabmwKfwKyQM5nPzYLJkzc4UILWPbExxYUclzSG0fXx2A59+rQS4mXX3r1ov25EBhR+trAZciU3KhPmBrR0oI2R7vt2CENkgr+NVR2+jwLlG0VEa0HCE+0z75LEFH76TJPwk4+btFnThiyJ8are7SKiwLZslOHbLn6JvAxde+Rl3ZXjYdfEr7xCbGWJWPnteNyHbjw0YRx4u2+P8TPvkqGhbqqIr5T\/MbZD7DHURFY4vu\/Cl4jtyPl2bJo48Crs97GbRbVteGya+q6vex4PVehveTxOYte0MMx3XSiHGuHsx7qZzDZxNzz0unp\/O\/BDfRx7fhe74bYd+3CsKAyY3yKZl84EPXUIOfUexs8GJ\/MTfOhCFQ+h+8x3NeLib\/omRXh1wT8QVb9WNUhMa\/iNBkBYE\/u+5g2ptpH3NPOfSSDnBL8YuxYNw8uxS1twj47DFrSf43EbOvRyvhgu27t7Cq7tql3EgusjePWKN+0en2iDvOMZ\/qq4413c1z0WEyt+COUUUNx3MfK7ZuyJq+HfbT+CsLrt+HCLuaUXynGI\/DDiC9B8GqvS1k\/WSuRm0wSOfV\/0Ec8h9OXYTAHleZr8YwwdEKl5G5odeZgHe3xPiq+nxOBr8rY+S7b1lEp8vSx9PSXY+oK\/PLm\/IPAFgS\/OwBcL6pJ0Lknn8kznctG5GG75O3J3RepXpH51pn41Kyyow9gM9V1zz69ocz8R9BNBP51BPy2YX2ny423bIZVjd0CObpueB8IGmg5n2EBuw2IiTDQEfJ4nGiKBI4HjGTguqJp0atKpz3TqRSeChiO5a0m9JfX2TL2dFRZUVX+p0wCRMBJwJOB4BhwXxD1NDhMJ96fhG7pEXnxINUQ6Ca8ZneO4lX5\/9gZZ9Zx\/fGYFbtEif44zNYZ0ILzCafns1T+UkLkwSgghhZQrYX8Q4ofn\/N3bZ95ru1OVWcnodivjirjalrZYlaUUIXc+bCXMvcf1koqWyXF7N97Npyp\/uZy\/+N0sV2SKh79mhs7R6bjnbzbaZ9Km6qBwmfEFqgeVWVz9c\/+Gr6\/ThYuSZ7493j6ErhQKBpmfQic2ru7ike1Cg9Pn5HmKDq5xlJ75Pt0hdMlNq9Hq4WoGqzp\/Gqtan7NqixCcsGLlQx5XJpRilVdVWDkUfUGWxW5n4syqLs5Ypdvpf2bV68Si15ko7AOr1F9YNfY7rCr1NFbpZqXVGPeIVauexKpU\/pzVUvlSb5Gr3m23yFURV2GntuiWeldpHA+5mFm1+oxVuuX\/HqvWFZmcKl2TIS\/n7kKqLf6cVFzlTyOVChRajBOPSHXu\/0mqzuNW6MKsyqpQKxNjsQo++JUrlN0WYYvyu5xJdf6MVK\/+FqnK4g1jDRoCzwansxxvoLm\/sOr1d1JVu6exSoUercabB1Z\/ZhKRTUXjRG56kE2C6kNVUGGoigyvk\/RcyTOJgwxcZ3iX4bmiM3zubKPnkpOKTE1lpXaEtoXJFF5a1prM4BT33maFTMdfnll8RI\/iMFS1GqpTjSKhyZIWMIS1OCSlzykOg7Ld5ZhQAM+1LkVvCqp1xTdhwIo4i8IbnM76cRR2rpZpIZa4sX6Jwtm0e05gF1MUWEdixbtMgd2No+AdBe+IG2ceh2GsRN2uljhSHS\/xdH0Uh5vrdFqJJ268fGDDYpFW51meHqsu7Y6eeMZObPxc3VP0nrjx+V8O41911W\/kNw+1Hu+5ERklEd5\/ASr7hdUKZW5kc3RyZWFtCmVuZG9iago4MiAwIG9iaiA8PC9JbmZvIDgxIDAgUi9GaWx0ZXIvRmxhdGVEZWNvZGUvVHlwZS9YUmVmL1dbMSAyIDJdL0luZGV4WzAgODNdL0lEIFs8MGMxODVlOGI4NWNhNDVhMjA4NGMxZDBkYzc0MDk0MzM+PGI0OTI5MTc2YjgxMTQ3NDU3ZDVlMmMzMDBiNzVmNTYzPl0vUm9vdCA4MCAwIFIvTGVuZ3RoIDIxNy9TaXplIDgzPj5zdHJlYW0KeJwl0DlWQkEQQNGqz+gHPzIoKorgxKCICCKTsAQizI3Yh1twB8QErsDM0NDUnHPcgQHUozu4QdWpF7SIrNeeTMWDvga+iKYXRvBiJN5EWCiE1Z\/YzHs34v9GdG7ElkakbOjMyDSMXt1IfhmSMkI\/rrIlBBHYhQBSsAdpyEBUB3\/uIgs52IcDyEMMDuEIjqEAJ3AKcR3+ukoRzqAEZTiHHbiAS7iCa6hAFXwdfbtKDepwA7fQgATcQRPuoQUP0IakPn+6SgceoQtP0NPJq\/3V+ANW4t4GBZwpbwplbmRzdHJlYW0KZW5kb2JqCnN0YXJ0eHJlZgoxODA4NgolJUVPRgo=""
			                                        ]
		                                        }
                                        }";
                        break;
                    case 3:
                        jsontext = @"{
                                    ""success"":1
                                    ,""mensaje"":""Consulta exitosa""
                                    ,""numero_guia"":""8190962500""
                                    ,""folio_estatus_codigo"":""D""
                                    ,""folio_estatus_descripcion"":""Entregado""
                                    ,""tipo_envio"":""NORMAL Se env\u00eda por mensajer\u00eda Default""
                                    ,""paqueteria"":""DHL""
                                    ,""paqueteria_url"":""https:\/\/www.dhl.com\/mx-es\/home\/rastreo.html""
                                    ,""rastreo"":[
	                                    {
	                                    ""date"":""2024-07-29""
	                                    ,""time"":""17:49:19""
	                                    ,""typeCode"":""PU""
	                                    ,""description"":""Shipment picked up""
	                                    ,""serviceArea"":[
		                                    {""code"":""MEX"",""description"":""Mexico City-MX""}
		                                    ]
	                                    }
	                                    ,{""date"":""2024-07-29"",""time"":""19:04:06"",""typeCode"":""PL"",""description"":""Processed at MEXICO CITY-MEXICO"",""serviceArea"":[{""code"":""MEX"",""description"":""Mexico City-MX""}]}
	                                    ,{""date"":""2024-07-29"",""time"":""19:04:58"",""typeCode"":""DF"",""description"":""Shipment has departed from a DHL facility MEXICO CITY-MEXICO"",""serviceArea"":[{""code"":""MEX"",""description"":""Mexico City-MX""}]}
	                                    ,{""date"":""2024-07-29"",""time"":""19:49:00"",""typeCode"":""AF"",""description"":""Arrived at DHL Sort Facility  MEXICO CITY HUB-MEXICO"",""serviceArea"":[{""code"":""JJC"",""description"":""Mexico City Hub-MX""}]}
	                                    ,{""date"":""2024-07-29"",""time"":""21:10:41"",""typeCode"":""PL"",""description"":""Processed at MEXICO CITY HUB-MEXICO"",""serviceArea"":[{""code"":""JJC"",""description"":""Mexico City Hub-MX""}]}
	                                    ,{""date"":""2024-07-29"",""time"":""21:10:58"",""typeCode"":""DF"",""description"":""Shipment has departed from a DHL facility MEXICO CITY HUB-MEXICO"",""serviceArea"":[{""code"":""JJC"",""description"":""Mexico City Hub-MX""}]}
	                                    ,{""date"":""2024-07-29"",""time"":""22:49:21"",""typeCode"":""AF"",""description"":""Arrived at DHL Sort Facility  HANGARES-MEXICO"",""serviceArea"":[{""code"":""HMX"",""description"":""Hangares-MX""}]}
	                                    ,{""date"":""2024-07-30"",""time"":""00:15:35"",""typeCode"":""PL"",""description"":""Processed at HANGARES-MEXICO"",""serviceArea"":[{""code"":""HMX"",""description"":""Hangares-MX""}]}
	                                    ,{""date"":""2024-07-30"",""time"":""00:38:58"",""typeCode"":""DF"",""description"":""Shipment has departed from a DHL facility HANGARES-MEXICO"",""serviceArea"":[{""code"":""HMX"",""description"":""Hangares-MX""}]}
	                                    ,{""date"":""2024-07-30"",""time"":""11:55:02"",""typeCode"":""AR"",""description"":""Arrived at DHL Delivery Facility  TUXTLA-GUTIERREZ-MEXICO"",""serviceArea"":[{""code"":""TGZ"",""description"":""Tuxtla-Gutierrez-MX""}]}
	                                    ,{""date"":""2024-07-30"",""time"":""13:40:36"",""typeCode"":""OH"",""description"":""Shipment is on hold"",""serviceArea"":[{""code"":""TGZ"",""description"":""Tuxtla-Gutierrez-MX""}]}
	                                    ,{""date"":""2024-08-02"",""time"":""06:29:34"",""typeCode"":""WC"",""description"":""Shipment is out with courier for delivery"",""serviceArea"":[{""code"":""TGZ"",""description"":""Tuxtla-Gutierrez-MX""}]}
	                                    ,{""date"":""2024-08-02"",""time"":""12:07:31"",""typeCode"":""OK"",""description"":""Delivered"",""serviceArea"":[{""code"":""TGZ"",""description"":""Tuxtla-Gutierrez-MX""}],""signedBy"":""""}
                                    ]
                                    ,""documento_prueba_entrega"":{
	                                    ""tipo_documento"":""SIGNATURE_PROOF_OF_DELIVERY""
	                                    ,""formato_documento"":""PDF""
	                                    ,""documentos"":""JVBERi0xLjQKJeLjz9MKNiAwIG9iago8PC9MZW5ndGggNyAwIFIKL0ZpbHRlciAvRmxhdGVEZWNvZGUKPj4Kc3RyZWFtCnjanVbbctpIEH3XV\/RjUrUez\/3ySAzrJRWXHZBrna28KCBAG0COBGG9X789EqAZwHHVlrFxzZzu6T7TfXoopLuEQjrBP7f4O08+pMn17xQcUZDOEgYUfxgITQx1YDgjxklIV8k7qqH3Pv37CFGKOK0jyHa+rTfAKZch8OCLSsKUa4D9PKvgBsHlKq9CLKOUaKEi8G8XnGmrieLtqemiqAE\/GTxXZTkD\/EzzZfEz8sstsTq2OzlZGKKj\/Re4hnqTbfJVvt54r7NinS2bpW0Ns9CYIxmxcVnBZpFDvSieG\/NdsVnALspEOiRQRlZZBFASASyO6VuxXMI6ghlHmLQRbLv6lldgmaNOc4WcXqJQIyXW7SnM1t\/hJUQZSgRjEarcxllbRazhMaSCyaIs62I9h\/4fn2Dwz3OV13VEtBNEitjsYnyKEa7afHa7iDqFCKYjBJkulmRSrvYwLEnO8YsyZaCany2NbpNBmuCVWylAU0u4M7A6LghBLIawTBQeZbW5tLI3WiaLZJZQYp30hzCsI\/8ljPDnXlzHw5uuQy5Y2HRSEMWQGGYI5S0xX8K0sWCMdNF+ua26GuvuG3ZZ3TZBXuVTKNdAedy9gjIiqI2cde0L2QZbhlDjTQ5KcUEmJEfCHF6Iw7Y1+3Z8fEo\/9a5uH9PhYDQa\/OVd+FxPTJE7IpG\/0LSf1xvssU2BAY\/j9hTaEG5UBP9ZTHLoVXnWHnEWHRMoUXjnoc2HZbaeZNArphncZdERnCIclSyEYzj5v69kgNdlnI7g42K+zqevhHMkCxvWp+IN7gZPw5v76JIPKOWIwO5qeTmRMyktMVpEIH\/Tb3EdwMeHqhk3euYtsSN+nJPHsdmwySYrLGaB+frt7j9c\/uEHyg6aqYKSQWSz4RxpER5yPaTQL5PP+IOVf0pgGBUSiOFU+VscUuxCxRqbj33KpKaUMqbRl3OinT+\/IiIwfyhyLKNh\/+u7+uv7\/yMf0mKqKGRH+ZBYEVzpQCwurOyN9vIRDuMzWeBIjwVMEXVTNzH3pidy6ASLANPCNxFOq+M19\/NNVizrA6\/2Eq94I5ZoKoLShJth+uXApr3Epo6s7qtiXpx3r0eY2P9Z99pL3csim8HTw2gwHkP\/\/m4wToc3rwTWFJaKLE8CUoYoy8\/ieYMdfETIiJ34AbGPl\/unS1tbKKwfIwxOLIuTK8Rsly+d5npC30gqMH0oJid1IKyMEN9R\/R+fPegKVco\/qCa\/SpAJ4vaW3TC5e\/LhUcMdp9RI1B3djoXG1+tVETjzVfiML5JRPoufQMoPxgiKMrZuL2LQKQUFI3BeCdfqTSeDCiem9oNRsmavMziTJjwINbPZ4f5hFYgTO4hTZ34VHPi58UOxwZE4yUjTtsopVHF9vkCbnm7x4dqZQeBskfwJazznP1lskCEKZW5kc3RyZWFtCmVuZG9iago3IDAgb2JqCjEwNjAKZW5kb2JqCjQgMCBvYmoKPDwvVGl0bGUgKFByb29mIG9mIERlbGl2ZXJ5KQovQ3JlYXRvciAoVW5rbm93bikKL0F1dGhvciAoREhMIEV4cHJlc3MpCi9Qcm9kdWNlciAoWEVQIDQuMjcuNzA5KQovVHJhcHBlZCAvRmFsc2UKL0NyZWF0aW9uRGF0ZSAoRDoyMDI0MDgwNjE3NTMwNVopCi9Nb2REYXRlIChEOjIwMjQwODA2MTc1MzA1WikKPj4KZW5kb2JqCjEyIDAgb2JqCjw8L1R5cGUvWE9iamVjdAovU3VidHlwZS9JbWFnZQovV2lkdGggNDc0Ci9IZWlnaHQgMTA0Ci9Db2xvclNwYWNlIC9EZXZpY2VSR0IKL0JpdHNQZXJDb21wb25lbnQgOAovU01hc2sgMTQgMCBSCi9GaWx0ZXIgWy9GbGF0ZURlY29kZV0KL0xlbmd0aCAxMyAwIFIKPj4Kc3RyZWFtCnja7d1Pp15XFMfxaSlVQighLqGUK5SSeV9A55nnBWSeeed9AZl33hfQeV9AqRClQoQoFapUuggR7fPc7mefP3utfT58R3HDufues\/Zev7XWb7\/96aO3AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAANDMm6e38CFp\/1J\/fv9p3cUp9PB\/\/\/hxnrcxHkaMOgivHt\/9+fZX+C\/PH1y\/eHjv9ZM7EUaShOLha\/LbN5\/3Pfwf390e\/vDx12x82vjJPC+haHycU7Go28Kz6\/svH10NDMvxScYzjF2EX66+7IsMf\/3wSfzfKmHt928\/S\/LWxaIlOQngCB94OX79+oshUkYcSof\/7t2\/eCza8IdvDGvxY8M3jvfEviBMHYQMH3jdmBznvd3+Uq+f3Bn+K796fLeuFBYL2Pi0cYRO8o51i0IoR4YPvDRxgmr\/xqvLxbH71JXC2sPay0dXeZQxcvFByPCBz0F7YahbTRp+Wot9py8RiIcfnvW3h7UMdcZLdRVMIBfnSccE5PzF\/QhTdaWwxrCWoc7YoaugOnm6dwTk\/MX9SN6PoHXnOZ90i0IoR57unclY\/TyTobgfMeoIWneelvvuHkJUlIvzpGPzFfXW7bIoLRcXao1O1XKfeQ4U60IurpJjZijudze7FmqNzlBnXN5DiHLk6d6ZmFVK4bWGiBNKYe1hLcNYCrn4aKTq3lHOm2aIuLTWnUou3nOYCANJ1b0zvXo8gVzcd8Kv1RqdquW+u4cQ5GJsVIipNURctzU6lUNLdw8hGGZio1pMrSHihFJYe1jL49DS3UMIhpnYKBrXGiLOqXWXc2ghFx9qAppcXKU4XshzsrTWzTATQ8jTvSMa51eTugWWQq3RqRxatvabAsNMXPqVlTbMdL\/Szj2EKFq8OyZxVBubFFx0yCx9v1ItrTuVQwvDTChfZusddb8Sw0yAnf5GtJ8zM6hJkTL35SAZ9hH3KwH5GRUr2r8116\/sttTuVwIOaMrRbh1mQHKfsOZ+JagGLqR7cn+gQtg+C+b6lX12vVRyMcNM1DXh7Ht7R50523uWXL\/CMBOo1Ura0R4\/cIyi4izY3MM17leCqepRnVcDu9oqFveLGpa6XwmmqpM3Ag3cPioW9+eWi92vBCacAxuBRrW0FS3uV6S9SJpHLmaYieomnJdmdgPHKCrOglWEYSbIxfkzu4F1sYrF\/aJysfuVYIQteSPQwDGKisX9orhfCUw48zcCjaqLFS3uzy0Xu18JbHZGZXYlJqDJxfvM1KSSi01AY4lcPDzF68jsyMXTy8XuVwK5OH9mN0oBqFjcL4r7lXA0htsm9DUCDfkGixb3K1LxfiWGmVgoFw9P8ZY0AsUJeU9nufZG6NjjDnsZ1io0hrX4sTzPTC7GErl4eIonswOA4SmezA4AMtgmMFQBcHCS2CY8f3BtkB\/Aye7QyThXIEvlsvvs+n7FNTw5GDj8abt3t0IPHz8236e6ESeVwDdPb+V\/8oPc5Mhld9b7lbq9ETK0Rpup2WdJzYfmMXtkm7DFLFgG5afbGyFDazQLpt2W1F6W5G4g2+JGjdDDlZ9ub4QMXY4smHZbUvOhee4Gsi0u3M5S+cit4o2QYZCt3TDTWWKJq1Wt+dD4Febg5LYoxdtiJiVDo2D3BE0trTuPYWZRD9KIxoWC2MTtIlK8LYJbBrm4e4Imw1A8CybWygwzsco5M4Py0+2NkEEuZsGUwYALhzLMnDIUZ1B+2sXt0lq3escOBlymP1bkXOVOirdFKM6g\/Jz7i0+mdTsYL5GLIy+O96RiQJtvW\/Qmb3HIzFDc75aLk7RGtz+86vOSJc1j\/mxbJBe3SG2X5ncZNu7uonO51mgyRfeSZkiCVFFti+1Ld2kVrNYQ8QRat2jcp8AncQZTRU3SSpo\/obg02a81RDyH1i0a9ynwqZzBDjL9cTIUp7pCdw51Iony0z5EPI3WzeSqY0njH6uPsLlfyZE4eaNg91tatzVaJXqLbnPsg7PEDTl+99tb2jCzdGs0sWKLJcVB7lfKmdAtyX1qDRHPpHXzC1q927wK1buLVVFPxoGFA0pJPCf7Jl5La93\/2lO0a\/6vXKw3O8+2SC7+MAK8eHhvFVWtkOfkZFr3ybEy7\/a5JeUMlmdbJBe\/D8IrTuuXNsws3Rp9Q5\/ewWXkc\/cryR2SVFEPvi3G5xmv6OpdMeWGiCeTi2\/+08Rf\/IDJ4LkllTUkqaIep8YRv+Y7R5E4LsbHGAfXTTt86t6vVMswc6F8EbtwvAzxSrx7Nyb+Fs4p8FOOemnJAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAPr4B2E4sAYKZW5kc3RyZWFtCmVuZG9iagoxMyAwIG9iagoxNzYyCmVuZG9iagoxNCAwIG9iago8PC9UeXBlL1hPYmplY3QKL1N1YnR5cGUvSW1hZ2UKL1dpZHRoIDQ3NAovSGVpZ2h0IDEwNAovQ29sb3JTcGFjZSAvRGV2aWNlR3JheQovQml0c1BlckNvbXBvbmVudCA4Ci9GaWx0ZXIgWy9GbGF0ZURlY29kZV0KL0xlbmd0aCAxNSAwIFIKPj4Kc3RyZWFtCnja7cExAQAAAMKg\/qnnbwagAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACAN1zh2qIKZW5kc3RyZWFtCmVuZG9iagoxNSAwIG9iago3MAplbmRvYmoKMTEgMCBvYmoKPDwvVHlwZS9YT2JqZWN0Ci9TdWJ0eXBlL0ltYWdlCi9XaWR0aCA1MTkKL0hlaWdodCAxNjYKL0NvbG9yU3BhY2UgL0RldmljZVJHQgovQml0c1BlckNvbXBvbmVudCA4Ci9GaWx0ZXIgL0RDVERlY29kZQovTGVuZ3RoIDE2IDAgUgo+PgpzdHJlYW0K\/9j\/4AAQSkZJRgABAQAAAQABAAD\/4QQCaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLwA8P3htbCB2ZXJzaW9uPSIxLjAiIGVuY29kaW5nPSJVVEYtOCI\/Pjx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iPjxyZGY6UkRGIHhtbG5zOnJkZj0iaHR0cDovL3d3dy53My5vcmcvMTk5OS8wMi8yMi1yZGYtc3ludGF4LW5zIyI+PHJkZjpEZXNjcmlwdGlvbiB4bWxuczpzaGlwbWVudD0iaHR0cDovL3d3dy5kaGwuY29tLzIwMTIvZXBvZC1zaWduYXR1cmUvIj48c2hpcG1lbnQ6RGV0YWlscz4mbHQ7TVNHJmd0OyAgJmx0O0hkciBJZD0iR0NBTVgyNDA4MDIxMjA3MzQ0OTEyMDA4NSIgTHN0UHJzZEJ5PSJNWCImZ3Q7ICAgICZsdDtTbmRyIEFwcENkPSJYTUwiLyZndDsgICZsdDsvSGRyJmd0OyAgJmx0O0JkJmd0OyAgICAmbHQ7R0V2dCBUb3VySWQ9IlRHRkktQSIgRXZ0Q2Q9Ik9LIiBFdnREdG09IjIwMjQtMDgtMDJUMTI6MDc6MzEiIFNpZ249IkJsYW5jYSBBaWRhIE1hcnRpbmV6IiZndDsgICAgICAmbHQ7R2VvUG9zJmd0OyAgICAgICAgJmx0O0xhdCBBY2M9IiImZ3Q7MTYuMjk0NjU5OCZsdDsvTGF0Jmd0OyAgICAgICAgJmx0O0xndGQgQWNjPSIiJmd0Oy05Mi40MjQyMTM2Jmx0Oy9MZ3RkJmd0OyAgICAgICAgJmx0O0FsdCBBY2M9IiImZ3Q7ODU2LjUmbHQ7L0FsdCZndDsgICAgICAmbHQ7L0dlb1BvcyZndDsgICAgJmx0Oy9HRXZ0Jmd0OyAgICAmbHQ7U2hwIElkPSIiJmd0OyAgICAgICZsdDtTaHBUciBEc3RTcnZhQ2Q9IlRHWiIgRHN0RmNDZD0iVEdaIi8mZ3Q7ICAgICAgJmx0O1Bjc0luU2hwJmd0OyAgICAgICAgJmx0O1BjcyBQY3NJZD0iSkQwMTQ2MDAwMTE2NTU5OTkzMjQiLyZndDsgICAgICAmbHQ7L1Bjc0luU2hwJmd0OyAgICAmbHQ7L1NocCZndDsgICAgJmx0O1NEb2MgRG9jVHlDZD0iU0lHIiBEb2NJZD0iU0lHTVgwMjAyNDA4MDIxODA3NDA5MjI0MDgwMjAxMjE1NSIvJmd0OyAgJmx0Oy9CZCZndDsmbHQ7L01TRyZndDs8L3NoaXBtZW50OkRldGFpbHM+PC9yZGY6RGVzY3JpcHRpb24+PC9yZGY6UkRGPjwveDp4bXBtZXRhPv\/bAEMACAYGBwYFCAcHBwkJCAoMFA0MCwsMGRITDxQdGh8eHRocHCAkLicgIiwjHBwoNyksMDE0NDQfJzk9ODI8LjM0Mv\/bAEMBCQkJDAsMGA0NGDIhHCEyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMv\/AABEIAKYCBwMBIgACEQEDEQH\/xAAfAAABBQEBAQEBAQAAAAAAAAAAAQIDBAUGBwgJCgv\/xAC1EAACAQMDAgQDBQUEBAAAAX0BAgMABBEFEiExQQYTUWEHInEUMoGRoQgjQrHBFVLR8CQzYnKCCQoWFxgZGiUmJygpKjQ1Njc4OTpDREVGR0hJSlNUVVZXWFlaY2RlZmdoaWpzdHV2d3h5eoOEhYaHiImKkpOUlZaXmJmaoqOkpaanqKmqsrO0tba3uLm6wsPExcbHyMnK0tPU1dbX2Nna4eLj5OXm5+jp6vHy8\/T19vf4+fr\/xAAfAQADAQEBAQEBAQEBAAAAAAAAAQIDBAUGBwgJCgv\/xAC1EQACAQIEBAMEBwUEBAABAncAAQIDEQQFITEGEkFRB2FxEyIygQgUQpGhscEJIzNS8BVictEKFiQ04SXxFxgZGiYnKCkqNTY3ODk6Q0RFRkdISUpTVFVWV1hZWmNkZWZnaGlqc3R1dnd4eXqCg4SFhoeIiYqSk5SVlpeYmZqio6Slpqeoqaqys7S1tre4ubrCw8TFxsfIycrS09TV1tfY2dri4+Tl5ufo6ery8\/T19vf4+fr\/2gAMAwEAAhEDEQA\/APa2U7zwetJtb+6fyqUzEMRgcUnnH0FeC1C+51XZHtb+6fyo2t\/dP5VJ5x9BR5x9BS5afcd2R7W\/un8qNrf3T+VSecfQUecfQUctPuF2R7W\/un8qNrf3T+VSecfQUecfQUctPuF2R7W\/un8qNrf3T+VSecfQUecfQUctPuF2R7W\/un8qNrf3T+VSecfQUCYk4wPzp8tPuK7Iwpz0I\/Cuc8b+K4PBXhmbV54TM4dYoYd23zHPQZ7cAn6Csjx58XNI8Go9pCFvtX28WyN8sZ9ZG7fTr06A5r5v8R+LPEHjbU0k1O6kuXZ8QW0YxGhPACIPwHcnuTXXRwilaT2IlUtoer6N+0OheVdb0RlTGY3smDHOehDkfmD+HNbCftB+FyTv0zWF9MRxH\/2cV4P4n8P3HhbxBcaNdyJJcQJEZCnQF41cgeuN2M+1fQE3wB8INgLc6tGR3S4jOfzQ1rWp4eCTktyYub2LNt8c\/Bc7YkmvbcestsSP\/HSa6nSfHPhfXCq6drtlLI\/3Ymk8uQ\/8AbDfpXnl3+zxorofsetahCx+6ZkSQfoFzXJax8AfElkrPpl7Z6ig6JnyZD+Byv8A49WXssLLRSt\/XmVzTXQ+kth7A+\/FJtb+6fyr5Nj1P4ifDqVFeXVNPhU7VScGS3b2AOUP4V6f4N+P9rePHZ+KrZbOViAL23BMR\/3l6r9QSPYUSwOl4u4Kr3PY9rf3T+VG1v7p\/KlhvI7iFJoXSWJ1DI8bBlYHoQR1FP8AOPoK5HGC3Zd2R7W\/un8qNrf3T+VSecfQUecfQUuWn3Hdke1v7p\/Kja390\/lUnnH0FHnH0FHLT7hdke1v7p\/Kja390\/lUnnH0FHnH0FHLT7hdke1v7p\/Kja390\/lUnnH0FHnH0FHLT7hdke1v7p\/Kja390\/lUnnH0FHnH0FHLT7hdke1v7p\/Kja390\/lUnnH0FHnH0FHLT7hdke1v7p\/Kja390\/lUnnH0FHnH0FHLT7hdke1v7p\/Kja390\/lUnnH0FHnH0FHLT7hdke1v7p\/Kja390\/lUnnH0FHnH0FHLT7hdke1v7p\/Kja390\/lUnnH0FHnH0FHLT7hdke1v7p\/Kja390\/lUnnH0FHnH0FHLT7hdke1v7p\/Kja390\/lUnnH0FHnH0FHLT7hdke1v7p\/Kja390\/lUnnH0FHnH0FHLT7hdke1v7p\/Kja390\/lUnnH0FHnH0FHLT7hdke1v7p\/Kja390\/lUnnH0FHnH0FHLT7hdke1v7p\/Kja390\/lUnnH0FHnH0FHLT7hdke1v7p\/Kja390\/lUnnH0FHnH0FHLT7hdke1v7p\/Kja390\/lUnnH0FHnH0FHLT7hdke1v7p\/Kja390\/lUnnH0FHnH0FHLT7hdke1v7p\/Kja390\/lUnnH0FHnH0FHLT7hdke1v7p\/Kja390\/lUnnH0FHnH0FHLT7hdjYwRIMg0U9ZC7gYoropJW0Ile41o3LEgd\/Wk8p\/T9aVpWDEZ70nmv61g\/Z36lah5T+n60eU\/p+tHmv60ea\/rS\/d+Y\/eDyn9P1o8p\/T9aPNf1o81\/Wj935h7weU\/p+tHlP6frR5r+tHmv60fu\/MPeDyn9P1o8p\/T9aUTOD2\/Ks\/XvENj4a0iXVNSulit4hycAlj2VR3Y+lVGMJbXE20T391b6ZYy3t9PFbWsK7pJZXCqo+teAePfjhdX7Tad4UZ7W05Vr5htmkH+wP4B7\/e6fdrmPFfjPxH8U9fh0+0gl+zGTFpp8J7\/3nPQnHc8AZ6ck+v8Aw8+EOm+FhDqWrLHf6wMMu4ZigP8Asjuf9o\/gB1PbGlSw65p7mXNKeiPJ9I+GGoN4dvvFfiYS2OmW8DTrE5xPdN\/COfuhmIGTzzwOc0nwW8Pf278QraZ03Qaahu2z03AgJ+O4g\/8AATXpn7QOuvaeErLSFfD6hcb3A7xx8kH\/AIEUP4U\/4A6L\/Z\/g661dhiXUp8KfWOPKj\/x4v+lbSrfuXN9RKPvWPJ\/jKCPivrQPX9x\/6Ijr6x8p\/T9a+UPix\/xMPi9qyRnJklgiyBnkRRr29xX1gZHA659x0rnxKjyQ5r7f5FQvd2E8p\/T9aPKf0\/WjzX9aPNf1ri\/d+Zr7wyW1E8TRTRJJG4wyOAQR6EGvIvHXwKtNTjlv\/C6JZXgyzWhOIZfZf7h\/8d6dOtewea\/rS+a5PXk1rSqxpu8WyZRb3PlLwj498Q\/DXVZNLvreZ7NJMXGnz5Voz3KZ+6e\/ofyNfTHh\/XdP8UaRFqek3Cz20nB7Mjd1YdiP\/r9KwvH3w\/03x3p\/78Lb6jEMQXiJyv8Ast\/eX27du+fn3RdY8RfCLxpJFcROu0hbq13fu7mPsyn8yG7cj1FdbjSxWq0ZneUD6z8p\/T9aPKf0\/Ws7Q\/ENn4j0a31XTZvMtp1ypIwVPdSOxB4NaPmv61wNU07O5rdh5T+n60eU\/p+tHmv60ea\/rS\/d+Y\/eDyn9P1o8p\/T9aPNf1o81\/Wj935h7weU\/p+tHlP6frR5r+tHmv60fu\/MPeDyn9P1o8p\/T9aPNf1o81\/Wj935h7weU\/p+tHlP6frR5r+tHmv60fu\/MPeDyn9P1o8p\/T9aPNf1o81\/Wj935h7weU\/p+tHlP6frR5r+tHmv60fu\/MPeDyn9P1o8p\/T9aPNf1o81\/Wj935h7weU\/p+tHlP6frR5r+tHmv60fu\/MPeDyn9P1o8p\/T9aPNf1o81\/Wj935h7weU\/p+tHlP6frR5r+tHmv60fu\/MPeDyn9P1o8p\/T9aPNf1o81\/Wj935h7weU\/p+tHlP6frR5r+tHmv60fu\/MPeDyn9P1o8p\/T9aPNf1o81\/Wj935h7weU\/p+tHlP6frR5r+tHmv60fu\/MPeDyn9P1o8p\/T9aPNf1o81\/Wj935h7weU\/p+tHlP6frR5r+tHmv60fu\/MPeDyn9P1o8p\/T9aPNf1o81\/Wj935h7weU\/p+tHlP6frR5r+tHmv60fu\/MPeDyn9P1o8p\/T9aPNf1o81\/Wj935h7w5EZXBIooR2ZwCaK6KNuX3SJbg0JLE5HNHkt6imszBj8x6+tN3t\/eP51g3TvsVqSeS3qKPJb1FR72\/vH86N7f3j+dK9PsOzJPJb1FHkt6io97f3j+dG9v7x\/Oi9PsFmSeS3qKPJb1FR72\/vH86N7f3j+dF6fYLMi1C6ttJ064v764jgtYELyyP0VR\/noOT0r5X8YeKtY+KXi2Cz0+3ma38zy7CzXrz1du2TjJPQAexJ3fjL8QpPEOqHw3pczNp1pLiVkOftEw4x7qp4HqeeeK9I+E\/w7XwhpK6lqEQ\/tq8TL5H+oQ9Ix7\/3j68dsn0IRhh4e0ktTFtzdkbPw9+G9l4G0rAKT6rOo+1XWPx2J6KD+Z5PYDsxCcjkfjUe9v7x\/OgyFRuL4A5JJ4rjnVjOXNJGii0rI+Y\/jrqp1D4hmxQ7k0+3SHA5BdvnJ+vzKPwr3nT5NJ8B+DtJs9W1C0sUt7ZIj50gXe4ALYHUnJJ49a+a9MuYPE\/xZ\/tK8kCWUt\/JfTNJ0WBCZSD7bFxWb428V3XjHxPdarcM4iZtlvEzZ8qIfdX+p9ya9OVBSioPZGKlZ3NeGZPFHxringbzbe71tWRsH5ohLwf++BX1t5Leor5T+CmntffEyxlAytnFLcN\/3yUH6uK+qSzdmauXGOKai1sXTT3H+S3qKPJb1FR72\/vH86UM5OAWz9a470+xrqP8lvUUeS3qK5PxD8R\/DHhhni1DV4zcr\/y7QZlkz6EL93\/gWK8y1v8AaJkLOmhaLgZ+WW+kJ\/8AHFP\/ALNW8MO56qJDnbqe8+S3qK4r4kfD228baCyr5UWq24LWlweMn\/nm3+yf0OD6g\/P+ofED4geKhOyahqBt4kLyJYIY0jQDJLFBnAGTljXHbrvUbuNC01zcSuEQElmZieAPck1108IoPmT1M5VL6He\/DjxtefD3xNNpmrLLHp0svlXkDg5gkBxvA9R0PqPUgV9TxBJ4Y5YZUkjdQyupyGB5BBHUV8O29vd6pcGOFZLicRs+3OWKopY4+iqT9BXtfwS+IrAxeEdUn+Xn+z5XP4mIn9V\/L0FPE0YyXPbUISa0PefJb1FHkt6imb2x95s\/Wk3t\/eP515l6fY31JPJb1FHkt6io97f3j+dG9v7x\/Olen2CzJPJb1FHkt6io97f3j+dG9v7x\/Oi9PsFmSeS3qKPJb1FR72\/vH86N7f3j+dF6fYLMk8lvUUeS3qKj3t\/eP50b2\/vH86L0+wWZJ5Leoo8lvUVHvb+8fzo3t\/eP50Xp9gsyTyW9RR5LeoqPe394\/nRvb+8fzovT7BZknkt6ijyW9RUe9v7x\/Oje394\/nRen2CzJPJb1FHkt6io97f3j+dG9v7x\/Oi9PsFmSeS3qKPJb1FR72\/vH86N7f3j+dF6fYLMk8lvUUeS3qKj3t\/eP50b2\/vH86L0+wWZJ5Leoo8lvUVHvb+8fzo3t\/eP50Xp9gsyTyW9RR5LeoqPe394\/nRvb+8fzovT7BZknkt6ijyW9RUe9v7x\/Oje394\/nRen2CzJPJb1FHkt6io97f3j+dG9v7x\/Oi9PsFmSeS3qKPJb1FR72\/vH86N7f3j+dF6fYLMk8lvUUeS3qKj3t\/eP50b2\/vH86L0+wWZJ5Leoo8lvUVHvb+8fzo3t\/eP50Xp9gsyTyW9RR5LeoqPe394\/nRvb+8fzovT7BZknkt6ijyW9RUe9v7x\/Oje394\/nRen2CzJVjKOCSKKbGxMgySfxorootcuhEtx5iBJO7rSeSv96on++31pKwc43+Eqz7k3kr\/eo8lf71Q0Uc8P5R2fcm8lf71Hkr\/eqGijnh\/KFn3JhCvrXnHxj8Zf8ACJeFvstlNt1TUt0URB5jj\/jf2PIA9zntXoJYKNxIAHJJ6V8n+LdUuviT8TTFYnfHNOtnZdSBEDjd9D8zn0yfSurCxjOV7aIzqNpHXfAvwENUvz4p1GL\/AEW0fbZqw+\/MOr\/Re3+1\/u19EmJT34rJ0XR7XQNGtNJsk2W9rGI1B6n1J9yck+5q\/WdbEKpLVFRhZE3kr\/eNc38QNQXRfAGuX4fa6WjIhB6O\/wAin\/vphW9Xlvx71H7J4Bis1Ybr28RGX\/YUFyfzC0qHLOoo8oSule581JI8YYI7LuG1sHGR6Vs6xoh0TStKNyCt9fxG7MZ\/5Zwk4jyPVsM302+9bHwy8JDxZ4tijulH9mWY+0XjMcLsHRSf9o8fTJ7VleM9fPifxdqOqjiKWXbAuMbYlG1Bjt8oGffNevf3rHPbQ9T\/AGc9JEt9rmquMeXHHbI2Ou4lmH\/jq\/nXvxiB6sa8f+GeoaP4E+FFtqetXkdr9vmkuQjcvJzsAVRyeFU8dN1cP4z+Nmr680lh4fSTTbJzs8wf8fEo+o+59F5964Z03WqvTTuapqMdz2Hxp8TvDfgxXglnN9qY6WVswJB\/226J+OTz0NeEa78TPGXje8\/s+xaaCKY7Y7HTlbc\/sSPmb37e1a\/gv4JatrpS\/wDELy6bZN83lEf6RKPofufVsn271714f8L6N4Xsvsuj2MVsh++4GXkPqzHk0SnQoP3VdglKe54V4a+AGv6kEn1u6h0qA8mMYlm\/IHaPzyPSvVND+C3gzRQrSWTahOP+Wl828f8AfAwv5g13AOOlMmnitoJJ53WOGNC7uxwFAGSSfQVjLGSlpYpU0jy\/44atb+G\/AcOhaeI7ZtSlCeTCoQCFfmbAHTnaPoTXzXHI8UiyRuyOhDKynBBHQg10nj3xZL4y8WXWpncLYHyrWM\/wRKTt49Tyx9yaufDDwraeL\/GkNhfl\/skUTXEyIcFwpA257AkjOOcenUejBKnT1MXqzvfgN4GuJdQPi29RoreFWjsgRgyMQVZx\/sgEj3JPpXO\/F\/wSfBfimLVNLzFYXzmWHy\/l8iUEEqMdBk5H5DpX0xBbw2ltFb20SRQRKEjjRQoUAcAAdBXOfETw1\/wl3gu\/05VBulXzrX2lXkAemeV\/4Ea44YtOrd6Jmjp+6O+HHiZfGPgqz1KRwbxMwXYGOJVxk47ZGG\/4FXV+Sv8Aer5k+B\/ir+wvFcmiXbFLbUyI13fwTrnb9M5K\/XbX0sRg9\/xrLEwjTn8OhUG2tyXyV\/vUeSv96oaK5+eH8pdn3JvJX+9R5K\/3qhoo54fyhZ9ybyV\/vUeSv96oaKOeH8oWfcm8lf71Hkr\/AHqhoo54fyhZ9ybyV\/vUeSv96oaKOeH8oWfcm8lf71Hkr\/eqGijnh\/KFn3JvJX+9R5K\/3qhoo54fyhZ9ybyV\/vUeSv8AeqGijnh\/KFn3JvJX+9R5K\/3qhoo54fyhZ9ybyV\/vUeSv96oaKOeH8oWfcm8lf71Hkr\/eqGijnh\/KFn3JvJX+9R5K\/wB6oaKOeH8oWfcm8lf71Hkr\/eqGijnh\/KFn3JvJX+9R5K\/3qhoo54fyhZ9ybyV\/vUeSv96oaKOeH8oWfcm8lf71Hkr\/AHqhoo54fyhZ9ybyV\/vUeSv96oaKOeH8oWfcm8lf71Hkr\/eqGijnh\/KFn3JvJX+9R5K\/3qhoo54fyhZ9ycRhWBBoqOL\/AFgorei01ojOW5IfKyc9e9JiH\/Oaif77fWkrF1NdkWkTYh\/zmjEP+c1DRS9p5IfL5k2If85oxDUNA69M0e08kHL5nB\/GXxOvhvwLPBbPtvtRP2WLB5VSP3jf988exYVwX7PnhZZbu98T3afJCDa2mR\/GRl2H0GBn\/ab0rmvjTrsmv\/EA6ZbEyxaeotY0Q53Snl8D1yQv\/Aa+gvC+h2\/hHwhYaYCqrawbppOxfku30zk13zl7KiklqzFLml6HRYh\/zmjEP+c18q6p8Z\/Flz4jkv7G\/NtZLKTBZ+WpQR9g3GWJHU56njHGPpTQtSGs+HtN1TYEN5bRzlAchSyg4z7ZxWFalKkk2kXGSka2If8AOa+bPj94ji1LxTa6JbEGLTIyZWB6yyYJH4KF\/Emvfdd1aHQdCvtVuBmK0haUrnBYgcKPcnA\/Gvi69vJ9S1C4vbqTfPcStLI57sxyT+tbYNczc7bE1NND2eVbbwD+z+hhI\/tXxGF3P32OM\/kI+Pq+e9eIV1\/j3xqfF97ZQ20DW+l6dD5FnCxy2MAFm7AnaOB0wOvWqvw\/0VPEHjzR9OmjWSB5w8qN0ZEBdgfqFI\/GuyPuxcpGT1dkXvC3gTxP4\/nheLzFsYlEP226J8tEUYCr\/ex0wOnfFfQ3gv4W+G\/BsazxxfbtSAy17cLllP8AsL0QfTnnkmutihit4khgjWKJAFREAAUDoABT682pjZS0S0N400ibEP8AnNGIf85qGiuf2nki+XzJsQ\/5zXjfx48aR6bpSeGLCT\/Sr1Q90ynlIc8L9WI\/IH1rrfiH8QrDwNpgbCT6pOp+y2mfw3v6KP1PA7kfO\/h3SNW+KHjspd3btNcEz3d0wz5cYwOB\/wB8qB0GR2ruw9P\/AJeTSSRlN9EZmg+Fr3XbHVb+MeXZabavPNM3TcFJVB6sT+ma9B\/Z4gVvGmpTtnamnMnTjJkQ\/wDstei+PtN07wh8GdT07S4BBAsSRAdS7PIqlie5IJJNcb+zpa5uPEF2RjakEan1yXJ\/kK1lW56MpW0JUbSSPfsQ\/wCc0Yh\/zmoaK8z2nkjfl8z5j+NHhn\/hGPHS6nYkx22pE3MRXjZMCN4H4kN\/wKvoPwTr8PijwbpmrttE08QEwXoJFJV\/p8wP4Vxvxz0caj8PJL1UzJp08cwIHO1jsI+nzA\/hWL+zzqjT6FrGlsSRazpOuewkBBA\/FP1runL2lBTtqjJLlnY9rxD\/AJzRiH\/Oahorh9p5I15fMmxD\/nNGIf8AOahoo9p5IOXzJsQ\/5zRiH\/Oahoo9p5IOXzJsQ\/5zRiH\/ADmoaKPaeSDl8ybEP+c0Yh\/zmoaKPaeSDl8ybEP+c0Yh\/wA5qGij2nkg5fMmxD\/nNGIf85qGij2nkg5fMmxD\/nNGIf8AOahoo9p5IOXzJsQ\/5zRiH\/Oahoo9p5IOXzJsQ\/5zRiH\/ADmoaKPaeSDl8ybEP+c0Yh\/zmoaKPaeSDl8ybEP+c0Yh\/wA5qGij2nkg5fMmxD\/nNGIf85qGij2nkg5fMmxD\/nNGIf8AOahoo9p5IOXzJsQ\/5zRiH\/Oahoo9p5IOXzJsQ\/5zRiH\/ADmoaKPaeSDl8ybEP+c0Yh\/zmoaKPaeSDl8ybEP+c0Yh\/wA5qGij2nkg5fMmxD\/nNGIf85qGij2nkg5fMnXy9w29aKji\/wBYKK3ou6vYzloyQyICQV\/Sk8yP+7+lRsDvbg9aTB9DWLqSuXZEvmR\/3f0o8yP+7+lRYPoaMH0NL2sgsiXzI\/7v6VS1fVbfSNGvdSmX93aQPMw9Qqk4\/SrABHb9K87+N2pNpvw1uYwSrX08dsp9s7z+iEfjWlKUpzURSSSueOfCjTZPFHxUtr28BlEEj6jcN6sDkH\/vtlr6J+IF+tn8PdflXKt9hlRT6Fl2j9TXmP7O+lBNO1nWCuWkmS2Ru67Rub896\/lXS\/HDVF0\/4cXFsSRJfzxwJjrw28n6YTH4111akpV1BGcUlC54R4F8BXvju9u7ezuoLYWqK7vMCcgnGBjv1r620ewh0XRLDS4yZEs7aOBXK4LBVC5\/HGa8e\/Z302SLR9b1Mg7LieOBf+AAsf8A0YPyr0nxf4u07wZocmo6g2XIK29upw07\/wB0eg9T2qMTUnKp7OI4JJXZxX7QGuiy8G2ukxErJqNwC4x1jjwx\/wDHjHXzVWx4n8Tal4s1qbVNTmLyPwkYJ2RJ2RR2A\/U5J5NZCqXYKoJYnAAHJNd1KHJGxlJ3Zs6RYAaTqWtTDENoqww5\/juJMhQPdVDvx3Qetenfs86GJtb1LXpkJjtYhbw5HV35Yj6KuP8Agdch49gXw5pmi+EE4mtIftmoY73UoBwf91AoB9DX0D8MPDp8N+AdNtXj23M6farjjB3vzg+4Xav\/AAGssTV5ad11KhG8jtfMj\/u\/pR5kf939KiwfQ0BTnkHH0ryvaSN7Il8yP+7+lcZ8RPiNp3gbSshUn1WdT9ltSfw3v6KP1PA7kT+OPGun+CNDe8uyHupAVtbYH5pXx+ijjJ7fUgH5L1nWL7X9WuNT1Kdprqdtzsf0AHYAcAV24WnKfvTWhnNpaIbq+r3+u6pPqWpXD3F3O253b9AB2A6ADgCvpr4N+Df+EU8K\/a76ErqepbZZVYcxxj7iexwST7nB6V538GPhv\/a06eJ9XgzYQP8A6JC44mkB++fVVI\/Ej25+iAjE8gjueKrF12v3cBU49WeV\/tBXwi8B2lsmAbi\/QMO5VUY\/z2039n20W28DXl06ndc3zYPqqooH67qxP2jLgra+H7fn55J5CO3AQdP+BGuz+DFuYvhZpThSDK0zn5cf8tXA\/QCplNrDJrqNL3z0LzI\/7v6Ueag6KfyqLB9DRg+hri9rI05Ucv8AE5o3+GmvjZ\/y6k8j3FeUfs5uV1nXQclDbxEjtnccf1r0z4rg\/wDCsNdzx+6T\/wBGLXnH7On\/AB9+IeM\/u4P5vXbSk3h5P+uhnJe+j3\/zI\/7v6UeZH\/d\/SosH0NGD6GuL2sjSyJfMj\/u\/pR5kf939KiwfQ0YPoaPayCyJfMj\/ALv6UeZH\/d\/SosH0NGD6Gj2sgsiXzI\/7v6UeZH\/d\/SosH0NGD6Gj2sgsiXzI\/wC7+lHmR\/3f0qLB9DRg+ho9rILIl8yP+7+lHmR\/3f0qLB9DRg+ho9rILIl8yP8Au\/pR5kf939KiwfQ0YPoaPayCyJfMj\/u\/pR5kf939KiwfQ0YPoaPayCyJfMj\/ALv6UeZH\/d\/SosH0NGD6Gj2sgsiXzI\/7v6UeZH\/d\/SosH0NGD6Gj2sgsiXzI\/wC7+lHmR\/3f0qLB9DRg+ho9rILIl8yP+7+lHmR\/3f0qLB9DRg+ho9rILIl8yP8Au\/pR5kf939KiwfQ0YPoaPayCyJfMj\/u\/pR5kf939KiwfQ0YPoaPayCyJfMj\/ALv6UeZH\/d\/SosH0NGD6Gj2sgsiXzI\/7v6UeZH\/d\/SosH0NGD6Gj2sgsiXzI\/wC7+lHmR\/3f0qLB9DRg+ho9rILIl8yP+7+lHmR\/3f0qLB9DRg+ho9rILIl8yP8Au\/pR5kf939KiwfQ0YPoaPayCyJldWYBRg\/Sio4wRIMiit6TbV2TJDzNhiNvSk8\/\/AGf1pGjYsTjvSeW\/92snKrcq0R3n\/wCz+tHn\/wCz+tN8t\/7tHlv\/AHaXPVC0R4mORhea8V\/aMu2\/sPQrbkCS5kk6\/wB1QP8A2Y17OI3Bzg\/ga8H\/AGjsLJ4bQn5gLkkex8r\/AANdGFlUdRXImlY7T4HRC1+GFm4HM880nTH8ZX\/2WvP\/ANojWDc69pGkqfltrdp2AP8AE7Y5+gT9a9M+D1vInwr0QHHKzN+Bmcj+deE\/Eh5PEHxgv7SNjl7qKyjB\/hICp\/6Fk\/jW9HmlXk3srkysoo918CPY+CPhFp13ft5MEdr9suHxzmT5gMd2+YKB7AV87+MvFWq\/EHxHcag8b+RCjGC3U5W3hXkk+\/cnufwFdf8AGjxsmp6knhbSWxpunsFl2HIklHG33CdPrn0FWtX8IjwD8EriW8hC6zrUsMUzH70K7vMEY\/BDu9z3wK1ppx96W7ZL10R43Xe\/CXQ4NS8WnVb8D+zNFiN9cMRxleVH5jd9FNcFXrmowDwL8Dra0P7vVfE8gllHRlgGCB\/3ztyP+mjVtNu1l1JRznhy2n+I\/wAWEmu03Jd3bXdyDyFiU7iv0wAg+or62807c7eBx1rxH9nzw20Wmaj4ikT57h\/ssBP9xcFj9C2B\/wAAr2vy3\/u15+LqT5+WOyNaaVrsd5\/+z+tHn\/7P603y3\/u0eW\/92uXnqmlonxx498SXfirxjqGoXTOFEhigiY\/6qJSQqj09T7kmpPh\/4Om8a+KIdPUlLSMeddyjqsQIzj3OQB9c9qufEi803xB8RbseHrJNskoh3Q5P2qbOGcDpyxxx1xnqTUHizwPrPgC5tHvL20E8ih0Frc\/vUPuvDDnI3Dj3r2r3jZaM5up9d2kUGn2cFnaQLDbQRrHFGvAVQOAKmE3PQD8a8q+CHinU\/Evhy+ttTnkup9PlVVnlOWZGBwCTyxBVuTzgivUvLf8Au15NX2sJtbnRHlaPnr9oq783xDott\/zztXkx\/vPj\/wBkr174ap9j+G3h+LaBmzST\/vv5v\/Zq8M+Psxk+IcUZJzDYRIf++nb\/ANmr6F8M2bWvhPRoAoAisYEGDnogH9K6azmqEbbkRtzM1\/P\/ANn9aPP\/ANn9ab5b\/wB2gRuDkA5ri56ppaJxfxduFX4Wa7uGAY41z7mVAP1Nedfs45W58RSYyAlup\/Eyf4V0Xx+1b7D4KttMJAlv7kcE8mOMbifwbZ+dR\/s\/aRJb+Dr3UWUg3t2QnuiADP8A30WH4V2xc44ZvqzN2cz13z\/9n9aPP\/2f1pvlv\/do8t\/7tcXPVNLRHef\/ALP60ef\/ALP603y3\/u0eW\/8Ado56oWiO8\/8A2f1o8\/8A2f1pvlv\/AHaPLf8Au0c9ULRHef8A7P60ef8A7P603y3\/ALtHlv8A3aOeqFojvP8A9n9aPP8A9n9ab5b\/AN2jy3\/u0c9ULRHef\/s\/rR5\/+z+tN8t\/7tHlv\/do56oWiO8\/\/Z\/Wjz\/9n9ab5b\/3aPLf+7Rz1QtEd5\/+z+tHn\/7P603y3\/u0eW\/92jnqhaI7z\/8AZ\/Wjz\/8AZ\/Wm+W\/92jy3\/u0c9ULRHef\/ALP60ef\/ALP603y3\/u0eW\/8Ado56oWiO8\/8A2f1o8\/8A2f1pvlv\/AHaPLf8Au0c9ULRHef8A7P60ef8A7P603y3\/ALtHlv8A3aOeqFojvP8A9n9aPP8A9n9ab5b\/AN2jy3\/u0c9ULRHef\/s\/rR5\/+z+tN8t\/7tHlv\/do56oWiO8\/\/Z\/Wjz\/9n9ab5b\/3aPLf+7Rz1QtEd5\/+z+tHn\/7P603y3\/u0eW\/92jnqhaI7z\/8AZ\/Wjz\/8AZ\/Wm+W\/92jy3\/u0c9ULRHef\/ALP60ef\/ALP603y3\/u0eW\/8Ado56oWiO8\/8A2f1o8\/8A2f1pvlv\/AHaPLf8Au0c9ULRHrJvcDGKKaiMrgkUVvScmveIla+gNKwYgY60ea3tSmFixORyaTyW9RWLVW5fuh5re1Hmt7UeS3qKPJb1FK1UPdDzW9q8M\/aOid4vDtxsAVTcIzY7kRkfyNe5+S3qK8y+O+jm8+HTXYALWN1HMSBztOUI+nzg\/hW+G9oqi5tiZ25dDQ+DV6Z\/hZpChgWiM0be2JWI\/QivmfW9We48ZajrNpKUkk1CS6hkU8qTIWUj9K9L+G3jGPQfhP4qV5gk9o+62BPJeZNigD2ZCT+NcZ8OfA1x468RraDdHYW+JLyYfwpnhR\/tNggfie1d1OLhOcpbGUndJI6\/4K+AZNW1RPFGpw50+1cm2WQf6+Yfxe6qe\/wDeHsa2v2iNejeDSNCVsyh2u5AP4Rgqv55b8q9leOw8N6CznyrTTrCAnA4VEUf\/AFvxr458U+IJ\/FHiW+1i4yrXEmUTOdiDhV\/AACsqLnVqub0SHK0Y2Nj4aeEj4v8AGFvaTITYW\/8ApF23bYD93\/gRwPoSe1XviJr03xA+Ii22mKJYI3WwsEXo43Y3fQsSc+mPStGfWLbwJ8MYdH02VX1zxDCtzeyp1ggYfIn1Kk\/QMx7qa6f4D+BHdn8W30QCjdFYK46no0g\/VR\/wL2reUnG8302JSvoex+HdKh8OeHbDR7XHlWkKx5x95urN+LEn8a1PNb2o8lvUUPthjLytGsaglmY4AHqT2ryv3sndnR7qDzn9vyryD4wfFKLS7G48N6LOH1GdTHdyof8Aj2U9VB\/vEcf7P1xWd8TvjGlqJNF8KXSSTFds+oxNuVM\/wxnoTj+IdO3PI8Ls7K91W9W3srae7upCSI4kLux78Dmu3D0Jr3qj+RlOa2RteE9ftvC11JrC263WqxgpZJIuY4WI5lb1IHCj1JJxgZ2vCngrxD8T9bn1K8uZRbNJm61Gcbtzf3VH8RxjgYAGOnAPZeCPgJd3Dx3\/AIsf7PCCGFhG+Xf\/AH2HCj2GTz1Fe8WWm2+nWUdnZwRQW8S7Y4o12qo9gK0rV+XSCuxRjfcx\/CXhTTPBejDTtKjYKzb5ZZDl5WxjcT07dBgfrW95re1Hkt6ijyW9RXmydVu7ubLlR8u\/HYlviVKT\/wA+kX8jX0po0rDQtPHH\/HtH\/wCgivnH492ktv8AERJJB8k9lG6EdwCyn9R\/KvofwvPHqPhPSbyAqY5rOJ155HyDj\/PpXZX5\/Ywt\/WhnC3MzV81vagSsSBkUeS3qK5b4g+LYPBPhifUZGja7kzHaQn+OQ9OP7o6n2GOpFckY1ZOxo3FHg3xq15\/EHxDawtz5kenotoipzukJy+PfJC\/8Br6K8K6Qvhrwppmjx4\/0W3VZCBwZDy5H1Yk\/jXzx8G\/C9z4q8df2xd7nttOf7VLK38cxOUH1zlv+A+9fT\/kvjGRiuvFOSSpx6GcLbsPNb2o81vajyW9RR5LeoritVNPdDzW9qPNb2o8lvUUeS3qKLVQ90PNb2o81vajyW9RR5LeootVD3Q81vajzW9qPJb1FHkt6ii1UPdDzW9qPNb2o8lvUUeS3qKLVQ90PNb2o81vajyW9RR5LeootVD3Q81vajzW9qPJb1FHkt6ii1UPdDzW9qPNb2o8lvUUeS3qKLVQ90PNb2o81vajyW9RR5LeootVD3Q81vajzW9qPJb1FHkt6ii1UPdDzW9qPNb2o8lvUUeS3qKLVQ90PNb2o81vajyW9RR5LeootVD3Q81vajzW9qPJb1FHkt6ii1UPdDzW9qPNb2o8lvUUeS3qKLVQ90PNb2o81vajyW9RR5LeootVD3Q81vajzW9qPJb1FHkt6ii1UPdDzW9qPNb2o8lvUUeS3qKLVQ90PNb2o81vajyW9RR5LeootVD3Q81vajzW9qPJb1FHkt6ii1UPdFSRmcA4ooWMq4JxRW9Lmt7xErX0Gs7Bj8x60m9v7xqQw5Ynd1pPI\/wBr9KwcKly7oZvb+8aN7f3jT\/I\/2v0o8j\/a\/SlyVAuhm9v7xrM8RaUviDw5qOkyOFW7t3iDHkKxHDfgcH8K1vI\/2v0qrc3un2TFbrULWAgZIllVcD15NVGFVO6BuJ8Q3ME9lcz2dwjRyxSFJY242upIII9RzX0N+z1bLH4P1O8XiSW\/MTH1CRoR\/wChmvCPE2qf234o1TUx926upJE46KWO0fliu18D\/FJPBfgfU9Jt7KRtTmnae2uCQY1ZlVTuB9AuR1yeOK9WvCU6fLHc54tJ3Z1fx28eeYF8JWE+4ZEl+yngHqsf8mP\/AAH3FeIfZZfsRuyAIfM8oEkZZsZOB3wMZ9Mj1FNnnlubiS4nkaWaVi8kjnLMxOSSe5JqS2tru\/kS2tYZ7iT+CKJS5\/ACtKcFCKiJu7udF4D0Cw8TeJkh1rVILLT7eLzriS4mEZdFwAikn3A9gD6Cvfb74w+BvD1utnaXj3SwII0gsISVQAYABO1cduDXz1e+A\/Fmm6cNQvPD+oQ2p6u0Jynuw6qPcgVztROlGo029BqTR7ZrP7RGoy5TRNIhtx0827kMjH32rgA\/ia8w8QeM\/EXiiQtrGq3FwmciHdtiX6IML+OM1m6dpt9rF7HZadZzXVy\/CxwoWY+\/Hb36V9A\/Dz4HQ6UYtV8UCO4vlIaOyBDRxH1f++3t0Hv2LQpK6QayOQ+HHwak8Q2i6v4hM9rYPhre3T5ZJh\/eOR8qnt3PXgYJ950Pw5o3hq0Fvo2nwWaEYby1+Zv95jkt+JNbHkn+9+lL5Rxjfx6V51WdWo+yNoqKI97\/AN40b2\/vGn+R\/tfpR5H+1+lYclQq8Rm9v7xoDtnqTT\/I\/wBr9KPI\/wBr9KOSoF4nkPx88OS6n4YtdbhQtNpshEoxz5L4yffDBfwJNUvgT44gm0seEruUR3UDNJZlmwJEJLMo9wST9D7GvaJrOO4hkhmVJIpFKOjrlWUjBBHcV87+M\/gTrVjqkt34WjW8sHYukHmhJYP9n5j8wHY5z7dz30ffp+znoZS0d0e86zrdroOkXOqahP5Vrbpvdu59APUk4AHqa+W9c1jXfi345ijt4jmRvLtLfPyW8Xcsf1Y\/l2FR3Xhn4j3gj0e703xDPDvDJBKJXhDDjIJ+UYz196+gvhn8OYvBGh5uNj6vdANdSgZ2ekan0Hr3PPpiow+rxbvdib52bHhDwzaeD\/DlvpNjkqnzTS4wZZD95j9ccegAHat3e3940\/yP9r9KPI\/2v0rglGrJ3ZsuVDN7f3jRvb+8af5H+1+lHkf7X6VPJUC6Gb2\/vGje3940\/wAj\/a\/SjyP9r9KOSoF0M3t\/eNG9v7xp\/kf7X6UeR\/tfpRyVAuhm9v7xo3t\/eNP8j\/a\/SjyP9r9KOSoF0M3t\/eNG9v7xp\/kf7X6UeR\/tfpRyVAuhm9v7xo3t\/eNP8j\/a\/SjyP9r9KOSoF0M3t\/eNG9v7xp\/kf7X6UeR\/tfpRyVAuhm9v7xo3t\/eNP8j\/AGv0o8j\/AGv0o5KgXQze3940b2\/vGn+R\/tfpR5H+1+lHJUC6Gb2\/vGje3940\/wAj\/a\/SjyP9r9KOSoF0M3t\/eNG9v7xp\/kf7X6UeR\/tfpRyVAuhm9v7xo3t\/eNP8j\/a\/SjyP9r9KOSoF0M3t\/eNG9v7xp\/kf7X6UeR\/tfpRyVAuhm9v7xo3t\/eNP8j\/a\/SjyP9r9KOSoF0M3t\/eNG9v7xp\/kf7X6UeR\/tfpRyVAuhm9v7xo3t\/eNP8j\/AGv0o8j\/AGv0o5KgXQze3940b2\/vGn+R\/tfpR5H+1+lHJUC6Gb2\/vGje3940\/wAj\/a\/SjyP9r9KOSoF0M3t\/eNG9v7xp\/kf7X6UeR\/tfpRyVAuhI2YyDJJopyx7HBzmiuikpJakS3I2J3tyetJk+pqYxoSSW\/Wk8uP8AvfrWDpyuXdEWT6mjJ9TUvlx\/3v1o8uP+9+tL2UguiLJ9TXzH4s+EXiuzm1nWppLe6tYjLdPP5xLunLFiCM5x196+oRGg6OfzqG6sbW+s57S5AkgnjaORCeGVhgj8Qa6KEp0n6kySkfC1Fdj8RfAU\/gLXI7Q3K3NpcqZbaXBDbQcYbtuHt+nSuf0HRbzxFrlppFgga5upAi56KOpY+wAJPsK9VNNXOc6v4ZfDz\/hO9Ru\/tM8ttp9og8yWIDczseFGeOgYk84wPWvpXw14X0nwlpgsNIthDHwZJDy8rf3mbuf0HbFSeFPCeneEfDtvpNicpH80kpwDLIcbmPuePoAB2ra8uP8AvfrXmYmc6krJ6G8EkiLJ9TVSXTNPnlaWWxtpJG5ZnhUk\/U4rQ8uP+9+tHlx\/3v1rmUJrZl3RUt7W3tFZbaCKEMckRoFz+VTZPqal8uP+9+tHlx\/3v1odOb3YXRFk+poyfU1L5cf979aPLj\/vfrS9lILoiyfU0ZPqal8uP+9+tHlx\/wB79aPZSC6Isn1NGT6mpfLj\/vfrR5cf979aPZSC6Isn1NGT6mpfLj\/vfrR5cf8Ae\/Wj2UguiLJ9TRk+pqXy4\/7360eXH\/e\/Wj2UguiLJ9TRk+pqXy4\/7360eXH\/AHv1o9lILoiyfU0ZPqal8uP+9+tHlx\/3v1o9lILoiyfU0ZPqal8uP+9+tHlx\/wB79aPZSC6Isn1NGT6mpfLj\/vfrR5cf979aPZSC6Isn1NGT6mpfLj\/vfrR5cf8Ae\/Wj2UguiLJ9TRk+pqXy4\/7360eXH\/e\/Wj2UguiLJ9TRk+pqXy4\/7360eXH\/AHv1o9lILoiyfU0ZPqal8uP+9+tHlx\/3v1o9lILoiyfU0ZPqal8uP+9+tHlx\/wB79aPZSC6Isn1NGT6mpfLj\/vfrR5cf979aPZSC6Isn1NGT6mpfLj\/vfrR5cf8Ae\/Wj2UguiLJ9TRk+pqXy4\/7360eXH\/e\/Wj2UguiLJ9TRk+pqXy4\/7360eXH\/AHv1o9lILoiyfU0ZPqal8uP+9+tHlx\/3v1o9lILoiyfU0ZPqal8uP+9+tHlx\/wB79aPZSC6Isn1NGT6mpfLj\/vfrR5cf979aPZSC6Isn1NGT6mpfLj\/vfrR5cf8Ae\/Wj2UguiLJ9TRk+pqXy4\/7360eXH\/e\/Wj2UguiLJ9TRk+pqXy4\/7360eXH\/AHv1o9lILoiyfU0ZPqal8uP+9+tHlx\/3v1o9lILoZGSZBk0VIqKrAqcn60VvSTSsyZMhf77fWkoorke5ogooopAFFFFAGB4u8HaR400pbHVI3zGd8M8RCyRN04ODwR1B4PHoKx\/Anwu0nwNJPeRTyXt9NlFnlQDy0\/uqB0z3PfA6UUVuqs1FwvoS4q9zt6KKKwKCiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKAHxf6wUUUV10PhM57n\/9kKZW5kc3RyZWFtCmVuZG9iagoxNiAwIG9iagoxNDcxNwplbmRvYmoKOSAwIG9iago8PC9UeXBlL0ZvbnQKL1N1YnR5cGUvVHlwZTEKL0ZpcnN0Q2hhciAzMgovTGFzdENoYXIgMjUxCi9XaWR0aHMgWyAyNzggMjc4IDM1NSA1NTYgNTU2IDg4OSA2NjcgMjIyIDMzMyAzMzMgMzg5IDU4NCAyNzggMzMzIDI3OAoyNzggNTU2IDU1NiA1NTYgNTU2IDU1NiA1NTYgNTU2IDU1NiA1NTYgNTU2IDI3OCAyNzggNTg0IDU4NCA1ODQKNTU2IDEwMTUgNjY3IDY2NyA3MjIgNzIyIDY2NyA2MTEgNzc4IDcyMiAyNzggNTAwIDY2NyA1NTYgODMzIDcyMgo3NzggNjY3IDc3OCA3MjIgNjY3IDYxMSA3MjIgNjY3IDk0NCA2NjcgNjY3IDYxMSAyNzggMjc4IDI3OCA0NjkKNTU2IDIyMiA1NTYgNTU2IDUwMCA1NTYgNTU2IDI3OCA1NTYgNTU2IDIyMiAyMjIgNTAwIDIyMiA4MzMgNTU2CjU1NiA1NTYgNTU2IDMzMyA1MDAgMjc4IDU1NiA1MDAgNzIyIDUwMCA1MDAgNTAwIDMzNCAyNjAgMzM0IDU4NAowIDAgMCAwIDAgMCAwIDAgMCAwIDAgMCAwIDAgMCAwCjAgMCAwIDAgMCAwIDAgMCAwIDAgMCAwIDAgMCAwIDAKMCAwIDMzMyA1NTYgNTU2IDE2NyA1NTYgNTU2IDU1NiA1NTYgMTkxIDMzMyA1NTYgMzMzIDMzMyA1MDAKNTAwIDAgNTU2IDU1NiA1NTYgMjc4IDAgNTM3IDM1MCAyMjIgMzMzIDMzMyA1NTYgMTAwMCAxMDAwIDAKNjExIDAgMzMzIDMzMyAzMzMgMzMzIDMzMyAzMzMgMzMzIDMzMyAwIDMzMyAzMzMgMCAzMzMgMzMzCjMzMyAxMDAwIDAgMCAwIDAgMCAwIDAgMCAwIDAgMCAwIDAgMAowIDAgMTAwMCAwIDM3MCAwIDAgMCAwIDU1NiA3NzggMTAwMCAzNjUgMCAwIDAKMCAwIDg4OSAwIDAgMCAyNzggMCAwIDIyMiA2MTEgOTQ0IDYxMV0KL0ZvbnREZXNjcmlwdG9yIDE3IDAgUgovVG9Vbmljb2RlIDE4IDAgUgovQmFzZUZvbnQvSGVsdmV0aWNhCj4+CmVuZG9iagoxNyAwIG9iago8PC9UeXBlL0ZvbnREZXNjcmlwdG9yCi9Gb250TmFtZSAvSGVsdmV0aWNhCi9Bc2NlbnQgNzE4Ci9EZXNjZW50IC0yMDcKL0NhcEhlaWdodCA3MTgKL0ZsYWdzIDMyCi9Gb250QkJveCBbLTE2NiAtMjI1IDEwMDAgOTMxXQovSXRhbGljQW5nbGUgMAovU3RlbVYgODgKL1N0ZW1IIDc2Ci9YSGVpZ2h0IDUyMwo+PgplbmRvYmoKMTggMCBvYmoKPDwvTGVuZ3RoIDE5IDAgUgovRmlsdGVyIC9GbGF0ZURlY29kZQo+PgpzdHJlYW0KeNpdk8Fq20AQhu96iu2hkB5cy9bsTAzGEFxCfWgbovYB5NXKEcSSkOWD37776zcp9GDrW6T955th9\/Onl3LxVPfHuCi+5u41XvrrGOJi\/6MasuX+8O3QtZNbvox9KOPkmrarx\/s37hhPbZet1q5uw3Rfzf\/hfN9c3i5TPB+6ps+2W7d8TS8v03hzD3PFL9ny11jHse1O7uHPvkzr8joM7\/Ecu8nl2W7n6tikoOTyszpHt3yqv\/dhkT799+L3bYhuPa9XdAh9HS9DFeJYdaeYbfN857bPz7ssdvV\/7\/yGW45NeKvGbLvGp3meHokDOYBrcg2O5AhuyE3ignsL7C1W5BV4TV6DC3IBFrKAPdmDlaxgIxv4kfwI3pA3iYW1BLWEtQS1hLUEtYS1BLWEtQS1hPmCfGG+IF+YL3N+Ra7AnIlgJsKZCGYinINgDp5z8JiDp4+Hj6ePh4+nj4ePp4+Hj6eDh4Ong4eDp4OHg7JfRb\/KfEW+Ml+Rr8xX5CvzFfnK2Spmq+xd0buyrqKusq6irh7JRzB7V\/Su7F3Ru\/I8KM6Dcg6KORjnYJiD0dPgafQ0eBo9DZ5GT4On0dPgafQ0eBo9DZ5GT4OncT7pgUN+P8047riIH9cnXMcx3az5ts53B7em7eLHhR76Abvm319w5\/8PCmVuZHN0cmVhbQplbmRvYmoKMTkgMCBvYmoKNDk4CmVuZG9iagoxMCAwIG9iago8PC9UeXBlL0ZvbnQKL1N1YnR5cGUvVHlwZTEKL0ZpcnN0Q2hhciAzMgovTGFzdENoYXIgMjUxCi9XaWR0aHMgWyAyNzggMzMzIDQ3NCA1NTYgNTU2IDg4OSA3MjIgMjc4IDMzMyAzMzMgMzg5IDU4NCAyNzggMzMzIDI3OAoyNzggNTU2IDU1NiA1NTYgNTU2IDU1NiA1NTYgNTU2IDU1NiA1NTYgNTU2IDMzMyAzMzMgNTg0IDU4NCA1ODQKNjExIDk3NSA3MjIgNzIyIDcyMiA3MjIgNjY3IDYxMSA3NzggNzIyIDI3OCA1NTYgNzIyIDYxMSA4MzMgNzIyCjc3OCA2NjcgNzc4IDcyMiA2NjcgNjExIDcyMiA2NjcgOTQ0IDY2NyA2NjcgNjExIDMzMyAyNzggMzMzIDU4NAo1NTYgMjc4IDU1NiA2MTEgNTU2IDYxMSA1NTYgMzMzIDYxMSA2MTEgMjc4IDI3OCA1NTYgMjc4IDg4OSA2MTEKNjExIDYxMSA2MTEgMzg5IDU1NiAzMzMgNjExIDU1NiA3NzggNTU2IDU1NiA1MDAgMzg5IDI4MCAzODkgNTg0CjAgMCAwIDAgMCAwIDAgMCAwIDAgMCAwIDAgMCAwIDAKMCAwIDAgMCAwIDAgMCAwIDAgMCAwIDAgMCAwIDAgMAowIDAgMzMzIDU1NiA1NTYgMTY3IDU1NiA1NTYgNTU2IDU1NiAyMzggNTAwIDU1NiAzMzMgMzMzIDYxMQo2MTEgMCA1NTYgNTU2IDU1NiAyNzggMCA1NTYgMzUwIDI3OCA1MDAgNTAwIDU1NiAxMDAwIDEwMDAgMAo2MTEgMCAzMzMgMzMzIDMzMyAzMzMgMzMzIDMzMyAzMzMgMzMzIDAgMzMzIDMzMyAwIDMzMyAzMzMKMzMzIDEwMDAgMCAwIDAgMCAwIDAgMCAwIDAgMCAwIDAgMCAwCjAgMCAxMDAwIDAgMzcwIDAgMCAwIDAgNjExIDc3OCAxMDAwIDM2NSAwIDAgMAowIDAgODg5IDAgMCAwIDI3OCAwIDAgMjc4IDYxMSA5NDQgNjExXQovRm9udERlc2NyaXB0b3IgMjAgMCBSCi9Ub1VuaWNvZGUgMjEgMCBSCi9CYXNlRm9udC9IZWx2ZXRpY2EtQm9sZAo+PgplbmRvYmoKMjAgMCBvYmoKPDwvVHlwZS9Gb250RGVzY3JpcHRvcgovRm9udE5hbWUgL0hlbHZldGljYS1Cb2xkCi9Bc2NlbnQgNzE4Ci9EZXNjZW50IC0yMDcKL0NhcEhlaWdodCA3MTgKL0ZsYWdzIDI2MjE3NgovRm9udEJCb3ggWy0xNzAgLTIyOCAxMDAzIDk2Ml0KL0l0YWxpY0FuZ2xlIDAKL1N0ZW1WIDE0MAovU3RlbUggMTE4Ci9YSGVpZ2h0IDUzMgo+PgplbmRvYmoKMjEgMCBvYmoKPDwvTGVuZ3RoIDIyIDAgUgovRmlsdGVyIC9GbGF0ZURlY29kZQo+PgpzdHJlYW0KeNpdk8GK20AQRO\/6iskhsDk4kiVNzy4Yw+KwxIckyyr5AGmm5QhiSYzlg\/8+UyqzgRwsvWG6q6tE++OH12bzHKZON9XnwrzpZbpGr5vDt3bO8sPxy3EcFpO\/xsk3uph+GEO815hOT8OYbUsTBr\/cT+vTn+\/Nze2y6Pk49lO225n8LV1elngzD+vET1n+IwaNw3gyD78OTTo313n+o2cdF1Nk+70J2ieh5OV7e1aTP4evk9+k0n8XP2+zmnI9b+nBT0Evc+s1tuNJs11R7M3u5WWf6Rj+u6tLtnS9\/93GbFeitCjSK\/Ej+RH8RH4CK1kTV6yvUF9tyVtwSS7BNbkGW7IFC1nAjuzAnFthbsW5FebW1K+hX1OzhmbNmnqt6cl9YktvFt4s\/Vj4sRW5AtOPhR9LHQsd4SzBLGG9oF44VzBX2CvoFWYRZBFmEWQRZhFkEerLqt+RO7Ane3AgBzC\/s+A7C3MJcjnmcsjlmMshl6NPB5+OPh18Ovp08Ono08Gno8\/0wmLcNwArguV9Xzl\/jTFt47rh675h04ZR3\/8E8zSja\/39BV8o1MkKZW5kc3RyZWFtCmVuZG9iagoyMiAwIG9iago0MjUKZW5kb2JqCjEgMCBvYmoKPDwvVHlwZS9DYXRhbG9nCi9QYWdlcyAyIDAgUgovUGFnZUxhYmVscyA8PCAvTnVtcyAgWwogMCA8PC9TL0Q+PgpdID4+Ci9QYWdlTW9kZSAvVXNlTm9uZQovT3BlbkFjdGlvbiBbNSAwIFIgL1hZWiAwIDg0MS45IDBdCi9NZXRhZGF0YSAzIDAgUgoKCj4+CmVuZG9iagoyIDAgb2JqCjw8L1R5cGUvUGFnZXMKL0NvdW50IDEKL0tpZHMgWzUgMCBSIF0KPj4KZW5kb2JqCjUgMCBvYmoKPDwvVHlwZS9QYWdlCi9QYXJlbnQgMiAwIFIKL01lZGlhQm94IFswIDAgNTk1LjI3NiA4NDEuODldCi9Dcm9wQm94IFswIDAgNTk1LjI3NiA4NDEuODldCi9CbGVlZEJveCBbMCAwIDU5NS4yNzYgODQxLjg5XQovVHJpbUJveCBbMCAwIDU5NS4yNzYgODQxLjg5XQovUmVzb3VyY2VzIDggMCBSCi9Db250ZW50cyBbMjMgMCBSIDYgMCBSIF0KCj4+CmVuZG9iago4IDAgb2JqCjw8L1Byb2NTZXQgWy9QREYgL0ltYWdlQyAvVGV4dF0KL0ZvbnQgPDwvRjAgOSAwIFIKL0YxIDEwIDAgUgo+PgovWE9iamVjdCA8PC9JMCAxMSAwIFIKL0kxIDEyIDAgUgo+Pgo+PgplbmRvYmoKMjMgMCBvYmoKPDwvTGVuZ3RoIDI0IDAgUgovRmlsdGVyIC9GbGF0ZURlY29kZQo+PgpzdHJlYW0KeNor5DJQMFDI5TK1NNUzMjcDsnPgbAsTQz0LS6CAAYKZwRWukMcFACXdCs4KZW5kc3RyZWFtCmVuZG9iagoyNCAwIG9iago0NAplbmRvYmoKMyAwIG9iago8PC9UeXBlIC9NZXRhZGF0YSAvU3VidHlwZSAvWE1MIC9MZW5ndGggODkxPj4Kc3RyZWFtCjw\/eHBhY2tldCBiZWdpbj0nJyBpZD0nVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkJz8+Cjx4OnhtcG1ldGEgeG1sbnM6eD0nYWRvYmU6bnM6bWV0YS8nPgo8cmRmOlJERiB4bWxuczpyZGY9Imh0dHA6Ly93d3cudzMub3JnLzE5OTkvMDIvMjItcmRmLXN5bnRheC1ucyMiPgo8cmRmOkRlc2NyaXB0aW9uIHJkZjphYm91dD0iIiB4bWxuczpkYz0iaHR0cDovL3B1cmwub3JnL2RjL2VsZW1lbnRzLzEuMS8iPgo8ZGM6Zm9ybWF0PmFwcGxpY2F0aW9uL3BkZjwvZGM6Zm9ybWF0Pgo8ZGM6dGl0bGU+CiA8cmRmOkFsdD4KICA8cmRmOmxpIHhtbDpsYW5nPSJ4LWRlZmF1bHQiPlByb29mIG9mIERlbGl2ZXJ5PC9yZGY6bGk+CiA8L3JkZjpBbHQ+CjwvZGM6dGl0bGU+CjxkYzpjcmVhdG9yPgogPHJkZjpTZXE+CiAgPHJkZjpsaT5ESEwgRXhwcmVzczwvcmRmOmxpPgogPC9yZGY6U2VxPgo8L2RjOmNyZWF0b3I+CjwvcmRmOkRlc2NyaXB0aW9uPgo8cmRmOkRlc2NyaXB0aW9uIHJkZjphYm91dD0iIiB4bWxuczpwZGY9Imh0dHA6Ly9ucy5hZG9iZS5jb20vcGRmLzEuMy8iPgo8cGRmOlByb2R1Y2VyPlhFUCA0LjI3LjcwOTwvcGRmOlByb2R1Y2VyPgo8L3JkZjpEZXNjcmlwdGlvbj4KPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIj4KPHhtcDpDcmVhdG9yVG9vbD5Vbmtub3duPC94bXA6Q3JlYXRvclRvb2w+Cjx4bXA6Q3JlYXRlRGF0ZT4yMDI0LTA4LTA2VDE3OjUzOjA1WjwveG1wOkNyZWF0ZURhdGU+Cjx4bXA6TW9kaWZ5RGF0ZT4yMDI0LTA4LTA2VDE3OjUzOjA1WjwveG1wOk1vZGlmeURhdGU+CjwvcmRmOkRlc2NyaXB0aW9uPgo8L3JkZjpSREY+CjwveDp4bXBtZXRhPgo8P3hwYWNrZXQgZW5kPSdyJz8+CgplbmRzdHJlYW0KZW5kb2JqCnhyZWYKMCAyNQowMDAwMDAwMDAwIDY1NTM1IGYgCjAwMDAwMjE3OTMgMDAwMDAgbiAKMDAwMDAyMTk1MyAwMDAwMCBuIAowMDAwMDIyNDc2IDAwMDAwIG4gCjAwMDAwMDExNjggMDAwMDAgbiAKMDAwMDAyMjAwOSAwMDAwMCBuIAowMDAwMDAwMDE1IDAwMDAwIG4gCjAwMDAwMDExNDggMDAwMDAgbiAKMDAwMDAyMjIyMCAwMDAwMCBuIAowMDAwMDE4NDk4IDAwMDAwIG4gCjAwMDAwMjAxNzQgMDAwMDAgbiAKMDAwMDAwMzU5MSAwMDAwMCBuIAowMDAwMDAxMzYwIDAwMDAwIG4gCjAwMDAwMDMzMDggMDAwMDAgbiAKMDAwMDAwMzMyOSAwMDAwMCBuIAowMDAwMDAzNTcyIDAwMDAwIG4gCjAwMDAwMTg0NzYgMDAwMDAgbiAKMDAwMDAxOTM4OCAwMDAwMCBuIAowMDAwMDE5NTgxIDAwMDAwIG4gCjAwMDAwMjAxNTQgMDAwMDAgbiAKMDAwMDAyMTA2OSAwMDAwMCBuIAowMDAwMDIxMjczIDAwMDAwIG4gCjAwMDAwMjE3NzMgMDAwMDAgbiAKMDAwMDAyMjMzOCAwMDAwMCBuIAowMDAwMDIyNDU3IDAwMDAwIG4gCnRyYWlsZXIKPDwvU2l6ZSAyNQovSW5mbyA0IDAgUgovUm9vdCAxIDAgUgovSURbPDBFRkM5M0MwRUM2QkVDQkZGOUZGQUYxOEM5RURBOTg5PjwwRUZDOTNDMEVDNkJFQ0JGRjlGRkFGMThDOUVEQTk4OT5dCj4+CnN0YXJ0eHJlZgoyMzQ0NgolJUVPRgo=""
	                                    }
                                    }";
                        break;
                }

                resultado = JsonConvert.DeserializeObject<ResponseRastreoGuia>(jsontext, new RastreoItemConverter());
                resultado.Numero_guia = GeneraGuiaRandom();
            }
            catch (Exception)
            {

                throw;
            }
            return resultado;
        }

        private string GeneraGuiaRandom()
        {
            Random random = new();
            StringBuilder numeroGuia = new(22);

            Random randomlargo = new();
            int numeroAleatorio = randomlargo.Next(1, 23);

            for (int i = 0; i < numeroAleatorio; i++)
            {
                numeroGuia.Append(random.Next(0, 10)); // Genera un dígito aleatorio entre 0 y 9
            }

            string guia = numeroGuia.ToString();
            return guia;
        }
        #endregion
        #endregion

        #region Api Canje de Premios Digitales
        public async Task<Respuesta<List<ResponseApiCPD>>> RedimePremiosDigitales(RequestApiCPD data)
        {
            Respuesta<List<ResponseApiCPD>> resultado = new()
            {
                IdTransaccion = data.IdTransaccion,
                Data = []
            };

            var FechaActual = DateTime.Now;
            string folioslog = string.Empty;
            string patronDespues = @"nip\s*:\s*(\d+)";
            string patronAntes = @"\d+\s*nip";

            //LoggerCanjeDigital log = new()
            //{
            //    IdUsuario = data.IdUsuario,
            //    IdCarrito = data.IdCarrito,
            //    IdPremio = data.IdPremio,
            //    FechaRegistro = FechaActual,
            //    IdTransaccion = data.IdTransaccion,
            //    Request = JsonConvert.SerializeObject(data, Formatting.None),
            //    RequestFecha = FechaActual,
            //};

            try
            {
                string UrlBase = _ajustes.Produccion ? _ajustesCDP.PRODUrl : _ajustesCDP.QAUrl;
                string Metodo = _ajustes.Produccion ? "canjePines.php" : "canjePinesPruebas.php";
                string Usuario = _ajustes.Produccion ? _ajustesCDP.PRODUsuario : _ajustesCDP.QAUsuario;
                string Password = _ajustes.Produccion ? _ajustesCDP.PRODContrasena : _ajustesCDP.QAContrasena;

                var client = new RestClient(UrlBase);
                var request = new RestRequest(Metodo, Method.Post);
                request.AddParameter("acceso[usuario]", Usuario);
                request.AddParameter("acceso[password]", Password);
                request.AddParameter("transaccion[sku]", data.Transaccion.sku);
                request.AddParameter("transaccion[cantidad]", data.Transaccion.cantidad);
                request.AddParameter("transaccion[id_cliente]", data.Transaccion.id_cliente);
                request.AddParameter("transaccion[id_compra]", data.Transaccion.id_compra);
                request.AddParameter("transaccion[correo_e]", data.Transaccion.correo_e);

                if (data.Transaccion.numero_recarga != null)
                {
                    request.AddParameter("transaccion[numero_recarga]", data.Transaccion.numero_recarga);
                }

                //if (data.Transaccion.referencia != null && data.Transaccion.monto != null)
                //{
                //    request.AddParameter("transaccion[referencia]", data.Transaccion.referencia);
                //    request.AddParameter("transaccion[monto]", data.Transaccion.monto);
                //}

                //request.AlwaysMultipartFormData = true;
                //request.AddHeader("Content-Type", "multipart/form-data");

                var logApi = new ExternalApiLogger
                {
                    ApiName = "MKT",
                    Method = Metodo,
                    RequestBody = JsonConvert.SerializeObject(request.Parameters),
                    RequestTimestamp = DateTime.Now,
                    IdTransaccionLog = resultado.IdTransaccion
                };

                var response = client.Execute(request);

                logApi.ResponseBody = response.Content;
                logApi.ResponseTimestamp = DateTime.Now;
                logApi.StatusCode = (int)response.StatusCode;
                logApi.Resultado = response.IsSuccessStatusCode ? "Success" : "Error";

                await _logApi.Writer.WriteAsync(logApi);

                List<ResponseApiCPD> resultApi = [];

                if (response.IsSuccessful)
                {

                    dynamic responsedinamic = JsonConvert.DeserializeObject<dynamic>(response.Content);

                    if (!(responsedinamic.Type == Newtonsoft.Json.Linq.JTokenType.Array))
                    {
                        ResponseApiCPD respuestaApi = JsonConvert.DeserializeObject<ResponseApiCPD>(response.Content);

                        respuestaApi.Idusuario = data.IdUsuario;
                        respuestaApi.IdCarrito = data.IdCarrito;
                        respuestaApi.IdPremio = data.IdPremio;
                        respuestaApi.IdTipoDePremio = data.IdTipoDePremio;
                        respuestaApi.IdTransaccion = data.IdTransaccion;

                        if (data.Transaccion.numero_recarga != null)
                        {
                            respuestaApi.TelefonoRecarga = data.Transaccion.numero_recarga;
                        }

                        if (!string.IsNullOrEmpty(respuestaApi.Giftcard))
                        {
                            Match coincidenciaPin = Regex.Match(respuestaApi.Giftcard, patronDespues);
                            if (coincidenciaPin.Success)
                            {
                                respuestaApi.pinRender = coincidenciaPin.Groups[1].Value;
                            }

                            Match coincidenciaCard = Regex.Match(respuestaApi.Giftcard, patronAntes);
                            if (coincidenciaCard.Success)
                            {
                                respuestaApi.giftCardRender = respuestaApi.Giftcard.Trim().Split(':')[0].Replace("nip", "").Trim();
                            }
                        }
                        resultApi.Add(respuestaApi);
                    }
                    else
                    {
                        List<ResponseApiCPD> respuestaApiArray = JsonConvert.DeserializeObject<List<ResponseApiCPD>>(response.Content);

                        respuestaApiArray.ForEach(x =>
                        {
                            x.Idusuario = data.IdUsuario;
                            x.IdCarrito = data.IdCarrito;
                            x.IdPremio = data.IdPremio;
                            x.IdTipoDePremio = data.IdTipoDePremio;
                            x.IdTransaccion = data.IdTransaccion;

                            if (data.Transaccion.numero_recarga != null)
                            {
                                x.TelefonoRecarga = data.Transaccion.numero_recarga;
                            }

                            if (!string.IsNullOrEmpty(x.Giftcard))
                            {
                                Match coincidenciaPin = Regex.Match(x.Giftcard, patronDespues);
                                if (coincidenciaPin.Success)
                                {
                                    x.pinRender = coincidenciaPin.Groups[1].Value;
                                }

                                Match coincidenciaCard = Regex.Match(x.Giftcard, patronAntes);
                                if (coincidenciaCard.Success)
                                {
                                    x.giftCardRender = x.Giftcard.Trim().Split(':')[0].Replace("nip", "").Trim();
                                }
                                else
                                {
                                    x.giftCardRender = x.Giftcard;
                                }
                            }

                        });
                        resultApi = respuestaApiArray;
                    }


                    foreach (var item in resultApi)
                    {
                        if (item.Success == 1)
                        {
                            folioslog = folioslog + "|" + item.Folio;
                            resultado.Data.Add(item);
                        }
                    }

                    resultado.Data = resultApi;

                    return resultado;
                }

                ResponseApiCPD respuestaApi2 = new()
                {
                    Idusuario = data.IdUsuario,
                    IdCarrito = data.IdCarrito,
                    IdPremio = data.IdPremio,
                    IdTipoDePremio = data.IdTipoDePremio,
                    IdTransaccion = data.IdTransaccion,
                    Success = 0,
                    Mensaje = CodigoDeError.CanjeDigitalNoDisponible.GetDescription()
                };

                if (data.Transaccion.numero_recarga != null)
                {
                    respuestaApi2.TelefonoRecarga = data.Transaccion.numero_recarga;
                }

                resultApi.Add(respuestaApi2);

                resultado.Codigo = (int)CodigoDeError.CanjeDigitalNoDisponible;
                resultado.Mensaje = CodigoDeError.CanjeDigitalNoDisponible.GetDescription();
                resultado.Exitoso = false;

            }
            catch (Exception ex)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;

                _logger.Error(ex, "RedimePremiosDigitales(RequestApiCPD) => ApiMKT::IdCarrito::{usuario}", data.IdCarrito);
            }

            return resultado;
        }

        public async Task<Respuesta<DisponibilidadMKT>> Disponibilidad(List<string> data)
        {
            Respuesta<DisponibilidadMKT> resultado = new();

            try
            {
                string UrlBase = _ajustes.Produccion ? _ajustesCDP.PRODUrl : _ajustesCDP.QAUrl;
                string Metodo = _ajustes.Produccion ? "disponibilidadPinesMultiple.php" : "disponibilidadPinesMultiplePruebas.php";
                string Usuario = _ajustes.Produccion ? _ajustesCDP.PRODUsuario : _ajustesCDP.QAUsuario;
                string Password = _ajustes.Produccion ? _ajustesCDP.PRODContrasena : _ajustesCDP.QAContrasena;

                var client = new RestClient(UrlBase);
                var request = new RestRequest(Metodo, Method.Post);

                request.AddParameter("acceso[usuario]", Usuario);
                request.AddParameter("acceso[password]", Password);
                request.AddParameter("transaccion[sku]", string.Join(",", data));

                var logApi = new ExternalApiLogger
                {
                    ApiName = "MKT",
                    Method = Metodo,
                    RequestBody = JsonConvert.SerializeObject(request.Parameters),
                    RequestTimestamp = DateTime.Now,
                    IdTransaccionLog = resultado.IdTransaccion
                };

                var response = client.Execute(request);

                logApi.ResponseBody = response.Content;
                logApi.ResponseTimestamp = DateTime.Now;
                logApi.StatusCode = (int)response.StatusCode;
                logApi.Resultado = response.IsSuccessStatusCode ? "Success" : "Error";

                await _logApi.Writer.WriteAsync(logApi);

                DisponibilidadMKT resultApi = new();

                if (response.IsSuccessful && response.Content != null)
                {

                    var responsedinamic = JsonConvert.DeserializeObject<dynamic>(response.Content);

                    if (responsedinamic != null)
                    {
                        if (!(responsedinamic.Type == Newtonsoft.Json.Linq.JTokenType.Array))
                        {
                            resultado.Data = JsonConvert.DeserializeObject<DisponibilidadMKT>(response.Content);

                            return resultado;
                        }
                    }

                }

                resultado.Codigo = (int)CodigoDeError.ConexionFallida;
                resultado.Mensaje = CodigoDeError.ConexionFallida.GetDescription();
            }
            catch (Exception ex)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;

                _logger.Error(ex, "Disponibilidad(List<string> => ApiMKT::IdCarrito::{usuario}", string.Join(",", data));
            }
            return resultado;
        }

        public async Task<Respuesta<DisponibilidadMKT>> Disponibilidad(string data)
        {
            return await Disponibilidad([data]);
        }
        #endregion
    }
}
