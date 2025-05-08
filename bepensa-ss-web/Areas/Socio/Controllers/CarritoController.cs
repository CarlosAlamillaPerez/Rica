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

namespace bepensa_ss_web.Areas.Socio.Controllers
{
    [Area("Socio")]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class CarritoController : Controller
    {
        private IAccessSession _sesion { get; set; }
        private readonly ICarrito _carrito;

        public CarritoController(IAccessSession sesion, ICarrito carrito)
        {
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
        public async Task<IActionResult> Store(ProcesarCarritoRequest pCarrito)
        {
            var resultado = await _carrito.ProcesarCarrito(pCarrito, (int)TipoOrigen.Web);

            if (!resultado.Exitoso)
            {
                TempData["msgError"] = resultado.Mensaje;

                return View(pCarrito);
            }

            TempData["model"] = JsonConvert.SerializeObject(resultado.Data);

            return RedirectToAction("Index", "Carrito", new { area = "Socio" });
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
