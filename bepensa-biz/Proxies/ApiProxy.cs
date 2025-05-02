using bepensa_biz.Interfaces;
using bepensa_biz.Settings;
using bepensa_models.ApiResponse;
using bepensa_models.Enums;
using bepensa_models.General;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using System.Text.RegularExpressions;

namespace bepensa_biz.Proxies
{
    public class ApiProxy : IApi
    {
        private readonly GlobalSettings _ajustes;

        private readonly ApiRMSSettings _ajustesRMS;

        private readonly ApiCPDSettings _ajustesCDP;

        public ApiProxy(IOptionsSnapshot<GlobalSettings> ajustes, IOptionsSnapshot<ApiRMSSettings> ajustesRMS, IOptionsSnapshot<ApiCPDSettings> ajustesCDP)
        {
            _ajustes = ajustes.Value;
            _ajustesRMS = ajustesRMS.Value;
            _ajustesCDP = ajustesCDP.Value;
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

        #region Api Canje de Premios Digitales
        public Respuesta<List<ResponseApiCPD>> RedimePremiosDigitales(RequestApiCPD data)
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

                var response = client.Execute(request);

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
                            //log.Folio = folioslog.Substring(1, folioslog.Length - 1);
                        }
                    }

                    resultado.Data = resultApi;
                    //log.ResponseFecha = DateTime.Now;
                    //log.Response = JsonConvert.SerializeObject(response.Content, Formatting.None);

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

                //log.ResponseFecha = DateTime.Now;
                //log.Response = JsonConvert.SerializeObject(response.Content, Formatting.None);

                resultado.Codigo = (int)CodigoDeError.CanjeDigitalNoDisponible;
                resultado.Mensaje = CodigoDeError.CanjeDigitalNoDisponible.GetDescription();
                resultado.Exitoso = false;

            }
            catch (Exception)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;
            }

            //LogContext.LoggerCanjeDigitals.Add(log);
            //LogContext.SaveChanges();
            return resultado;
        }

        public Respuesta<DisponibilidadMKT> Disponibilidad(List<string> data)
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


                var response = client.Execute(request);

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
            catch (Exception)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;
            }
            return resultado;
        }

        public Respuesta<DisponibilidadMKT> Disponibilidad(string data)
        {
            return Disponibilidad([data]);
        }
        #endregion
    }
}
