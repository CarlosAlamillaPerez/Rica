using Microsoft.AspNetCore.Authentication.Cookies;
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
    }
}
