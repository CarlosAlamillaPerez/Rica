﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using bepensa_models.DTO;
using bepensa_biz.Interfaces;
using bepensa_models.DataModels;
using bepensa_data.models;

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

            return View(resultado.Data ?? new CarritoDTO());
        }

        [HttpPost("carrito/modificar-premio")]
        public async Task<JsonResult> ModificarPremio([FromBody]ActPremioRequest pPremio)
        {
            pPremio.IdUsuario = _sesion.UsuarioActual.Id;

            var resultado = await _carrito.ModificarPremio(pPremio);

            return Json(resultado);
        }

        [HttpPost("carrito/eliminar-premio")]
        public async Task<JsonResult> EliminarPremio([FromBody] RequestByIdPremio pPremio)
        {
            pPremio.IdUsuario = _sesion.UsuarioActual.Id;

            var resultado = await _carrito.EliminarPremio(pPremio);

            return Json(resultado);
        }

        [HttpGet("carrito/proceso-de-canje")]
        public IActionResult Store()
        {
            return View(new ProcesarCarritoRequest());
        }

        [HttpPost("carrito/proceso-de-canje")]
        public async Task<JsonResult> Store(ProcesarCarritoRequest pCarrito)
        {
            var resultado = await _carrito.ProcesarCarrito(pCarrito);

            return Json(resultado);
        }

        

        #region Vistas parciales
        /// <summary>
        /// Su usu es únicamente para pintar el carrito
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("carrito/lista-de-premio")]
        public IActionResult ListaPremio([FromBody]CarritoDTO model)
        {
            model ??= new CarritoDTO();

            return PartialView("_listaDePremios", model);
        }
        #endregion
    }
}
