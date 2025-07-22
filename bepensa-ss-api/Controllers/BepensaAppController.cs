using bepensa_biz.Interfaces;
using bepensa_biz.Settings;
using bepensa_models.Enums;
using bepensa_models.General;
using bepensa_ss_api.Configuratioin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace bepensa_ss_api.Controllers
{
    [Route("api/bepensa-app")]
    [ApiController]
    public class BepensaAppController : ControllerBase
    {
        private readonly GlobalSettings _config;

        private readonly IApp _app;

        private readonly IFuerzaVenta _objetivo;

        public BepensaAppController(IOptionsSnapshot<GlobalSettings> config, IApp app, IFuerzaVenta objetivo)
        {
            _config = config.Value;
            _app = app;
            _objetivo = objetivo;
        }

        [Authorize(AuthenticationSchemes = ApiKeyAuthenticationHandler.ApiKeySchemeName)]
        [HttpGet("{CUC}")]
        public ActionResult<Respuesta<string>> ValidarUsuario(string CUC)
        {
            Respuesta<string> resultado = new();

            try
            {
                var result = _objetivo.ValidarUsuario(CUC);

                if (!result.Exitoso)
                {
                    resultado.Codigo = result.Codigo;
                    resultado.Mensaje = result.Mensaje;
                    resultado.Exitoso = false;

                    return resultado;
                }

                string url = string.Empty;

                string urlBase = string.Empty;

                if (_config.Produccion)
                    urlBase = "https://socioselecto-bepensa.com";
                else
                    urlBase = "https://qa-web.socioselecto-bepensa.com";

                url = $"{urlBase}/bepensa-app/landing/@CUC/e88bb458-9de0-4a6c-a008-661130c2d3be".Replace("@CUC", CUC);

                //if (result.Data == 2)
                //{
                //    if (_config.Produccion)
                //        urlBase = "https://socioselectoop-bepensa.com";
                //    else
                //        urlBase = "https://qa.socioselectoop-bepensa.com";

                //    url = $"{urlBase}/landing/fdv/@CUC/78a11fa8-8cc3-40ca-aa80-08e863943d4c".Replace("@CUC", CUC);
                //}
                //else
                //{
                //    if (_config.Produccion)
                //        urlBase = "https://socioselecto-bepensa.com";
                //    else
                //        urlBase = "https://qa-web.socioselecto-bepensa.com";

                //    url = $"{urlBase}/landing/fdv/@CUC/e88bb458-9de0-4a6c-a008-661130c2d3be".Replace("@CUC", CUC);
                //}

                resultado.Data = url;
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
