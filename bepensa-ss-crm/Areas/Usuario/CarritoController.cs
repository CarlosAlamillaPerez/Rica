using bepensa_biz.Interfaces;
using bepensa_models.ApiResponse;
using bepensa_models.DataModels;
using bepensa_models.DTO;
using bepensa_models.Enums;
using bepensa_ss_crm.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace bepensa_ss_crm.Areas.Usuario
{
    [Area("Usuario")]
    [Authorize]
    [ValidaSesionUsuario]
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
            pCarrito.IdOperador = _sesion.OperadorActual.Id;

            var resultado = await _carrito.ProcesarCarrito(pCarrito, (int)TipoOrigen.CallCenter);

            if (!resultado.Exitoso)
            {
                TempData["msgError"] = resultado.Mensaje;

                return View(pCarrito);
            }

            TempData["model"] = JsonConvert.SerializeObject(resultado.Data);

            return RedirectToAction("Index", "Carrito", new { area = "Usuario" });
        }

        #region Solicitudes AJAX
        [HttpPost("carrito/modificar-premio")]
        public async Task<JsonResult> ModificarPremio([FromBody] ActPremioRequest pPremio)
        {
            pPremio.IdUsuario = _sesion.UsuarioActual.Id;
            pPremio.IdOperador = _sesion.OperadorActual.Id;

            var resultado = await _carrito.ModificarPremio(pPremio, (int)TipoOrigen.CallCenter);

            return Json(resultado);
        }

        [HttpPost("carrito/eliminar-premio")]
        public async Task<JsonResult> EliminarPremio([FromBody] RequestByIdPremio pPremio)
        {
            pPremio.IdUsuario = _sesion.UsuarioActual.Id;
            pPremio.IdOperador = _sesion.OperadorActual.Id;

            var resultado = await _carrito.EliminarPremio(pPremio, (int)TipoOrigen.CallCenter);

            return Json(resultado);
        }

        [HttpGet("carrito/eliminar-carrito")]
        public async Task<JsonResult> EliminarCarrito()
        {
            var resultado = await _carrito.EliminarCarrito(new RequestByIdUsuario { IdUsuario = _sesion.UsuarioActual.Id, IdOperador = _sesion.OperadorActual.Id }, (int)TipoOrigen.CallCenter);

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
