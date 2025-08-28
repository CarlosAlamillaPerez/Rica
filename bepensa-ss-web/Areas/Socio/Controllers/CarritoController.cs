using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using bepensa_models.DTO;
using bepensa_biz.Interfaces;
using bepensa_models.DataModels;
using bepensa_data.models;
using Newtonsoft.Json;
using bepensa_models.ApiResponse;
using bepensa_models.Enums;
using bepensa_ss_web.Filters;
using bepensa_models.General;
using bepensa_biz.Settings;
using Microsoft.Extensions.Options;

namespace bepensa_ss_web.Areas.Socio.Controllers
{
    [Area("Socio")]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [ValidaSesionUsuario]
    public class CarritoController : Controller
    {
        private readonly GlobalSettings _app;
        private IAccessSession _sesion { get; set; }
        private readonly ICarrito _carrito;

        public CarritoController(IOptionsSnapshot<GlobalSettings> app, IAccessSession sesion, ICarrito carrito)
        {
            _app = app.Value;
            _sesion = sesion;
            _carrito = carrito;
        }

        [HttpGet("carrito")]
        public IActionResult Index()
        {
            var resultado = _carrito.ConsultarCarrito(new RequestByIdUsuario
            {
                IdUsuario = _sesion.UsuarioActual.Id
            });

            if (TempData["model"] is string jsonCanjes)
            {
                var canjes = JsonConvert.DeserializeObject<List<ProcesaCarritoResultado>>(jsonCanjes);

                ViewBag.Canjes = canjes;
            }

            return View(resultado.Data ?? new CarritoDTO());
        }

        [HttpGet("carrito/proceso-de-canje")]
        public IActionResult Store()
        {
            return View(new ProcesarCarritoRequest());
        }

        [HttpPost("carrito/proceso-de-canje")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Store(ProcesarCarritoRequest pCarrito)
        {
            pCarrito.IdUsuario = _sesion.UsuarioActual.Id;

            var resultado = await _carrito.ProcesarCarrito(pCarrito, (int)TipoOrigen.Web);

            if (!resultado.Exitoso)
            {
                TempData["msgError"] = resultado.Mensaje;

                return View(pCarrito);
            }

            TempData["model"] = JsonConvert.SerializeObject(resultado.Data);

            return RedirectToAction("Index", "Carrito", new { area = "Socio" });
        }

        [HttpPost("carrito/comprar-puntos-por-tarjeta")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcesarCarritoConTarjeta(PasarelaCarritoRequest pPuntos)
        {
            pPuntos.IdUsuario = _sesion.UsuarioActual.Id;

            var resultado = await _carrito.ProcesarCarritoConTarjeta(pPuntos, (int)TipoOrigen.Web);

            if (resultado.Exitoso)
            {
                if (resultado.Data != null && resultado.Data.Count > 0)
                {
                    TempData["model"] = JsonConvert.SerializeObject(resultado.Data);
                }

                if (resultado.Details != null)
                {
                    TempData["msgInfo"] = resultado.Mensaje;

                    TempData["urlRedirect"] = resultado.Details.Url;
                }


                return RedirectToAction("Index", "Carrito", new { area = "Socio" });
            }
            else
            {
                TempData["msgError"] = resultado.Mensaje;

                return RedirectToAction("Store", "Carrito", new { area = "Socio" });
            }
        }

        [HttpPost("carrito/comprar-puntos-por-deposito")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcesarCarritoPorDeposito(ProcesarCarritoRequest pUsuario)
        {
            pUsuario.IdUsuario = _sesion.UsuarioActual.Id;

            var resultado = await _carrito.ProcesarCarritoPorDeposito(pUsuario, (int)TipoOrigen.Web);

            if (resultado.Exitoso)
            {
                TempData["msgSuccess"] = resultado.Mensaje;
            }
            else
            {
                TempData["msgError"] = resultado.Mensaje;
            }

            return RedirectToAction("Index", "Carrito", new { area = "Socio" });
        }

        [AllowAnonymous]
        [HttpGet("transactions")]
        public async Task<IActionResult> Transaction(string? id)
        {
            if (id == null)
            {
                TempData["msgError"] = CodigoDeError.LigaNoEncontrada.GetDescription();
            }
            else
            {
                bool validarCanal = _carrito.ValidarOrigenTranferencia(id).Data == (int)TipoCanal.Tradicional;

                if (!validarCanal)
                {
                    string baseUrl = _app.UrlOnPrimes;
                    string endPoint = "transactions";
                    var url = $"{baseUrl.TrimEnd('/')}/{endPoint}?id={id}";

                    return Redirect(url);
                }

                var resultado = await _carrito.LiberarTranferencia(id);

                if (!resultado.Exitoso)
                {
                    TempData["msgError"] = CodigoDeError.LigaNoEncontrada.GetDescription();
                }
                else
                {
                    if (resultado.Data != null && resultado.Data.Count > 0)
                    {
                        var canjes = resultado.Data;

                        TempData["model"] = JsonConvert.SerializeObject(canjes);

                        ViewBag.Canjes = canjes;
                    }
                    else
                        TempData["msgSuccess"] = resultado.Mensaje;
                }
            }

            var validaSesion = _sesion.UsuarioActual;

            if (validaSesion == null)
                return View();
            else
                return RedirectToAction("Index", "Carrito", new { area = "Socio" });
        }

        [HttpGet("carrito/evaluacion-de-pago")]
        public JsonResult EvaluacionPago()
        {
            var resultado = _carrito.EvaluacionPago(_sesion.UsuarioActual.Id);

            return Json(resultado);
        }

        #region Solicitudes AJAX
        [HttpPost("carrito/modificar-premio")]
        public async Task<JsonResult> ModificarPremio([FromBody] ActPremioRequest pPremio)
        {
            pPremio.IdUsuario = _sesion.UsuarioActual.Id;

            var resultado = await _carrito.ModificarPremio(pPremio, (int)TipoOrigen.Web);

            return Json(resultado);
        }

        [HttpPost("carrito/eliminar-premio")]
        public async Task<JsonResult> EliminarPremio([FromBody] RequestByIdPremio pPremio)
        {
            pPremio.IdUsuario = _sesion.UsuarioActual.Id;

            var resultado = await _carrito.EliminarPremio(pPremio, (int)TipoOrigen.Web);

            return Json(resultado);
        }

        [HttpGet("carrito/eliminar-carrito")]
        public async Task<JsonResult> EliminarCarrito()
        {
            var resultado = await _carrito.EliminarCarrito(new RequestByIdUsuario { IdUsuario = _sesion.UsuarioActual.Id }, (int)TipoOrigen.Web);

            return Json(resultado);
        }

        [HttpGet("carrito/total-de-premios")]
        public JsonResult TotalPremios()
        {
            var resultado = _carrito.ConsultarTotalPremios(_sesion.UsuarioActual.Id);

            return Json(resultado.Data);
        }
        #endregion

        #region Vistas parciales
        /// <summary>
        /// Su usu es únicamente para pintar el carrito
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("carrito/lista-de-premio")]
        public IActionResult ListaPremio([FromBody] CarritoDTO model)
        {
            model ??= new CarritoDTO();

            return PartialView("_listaDePremios", model);
        }
        #endregion
    }
}
