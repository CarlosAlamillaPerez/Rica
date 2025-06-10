using bepensa_biz.Interfaces;
using bepensa_models.Enums;
using bepensa_ss_crm.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bepensa_ss_crm.Areas.Usuario.Controllers
{
    [Area("Usuario")]
    [Authorize]
    [ValidaSesionUsuario]
    public class MecanicasController : Controller
    {
        private IAccessSession _sesion { get; set; }

        public MecanicasController(IAccessSession sesion)
        {
            _sesion = sesion;
        }

        [HttpGet("mecanica/meta-compra")]
        public IActionResult MetaCompra()
        {
            return View();
        }

        [HttpGet("mecanica/ejecucion-de-mercadeo")]
        public IActionResult EjecucionMercadeo()
        {
            return View();
        }

        [HttpGet("mecanica/portafolio")]
        public IActionResult Portafolio()
        {
            return View();
        }

        [HttpGet("mecanica/promociones")]
        public IActionResult Promociones()
        {
            return View();
        }

        [HttpGet("mecanica/foto-de-exito")]
        public IActionResult FotoExito()
        {
            if (_sesion.UsuarioActual.IdCanal != (int)TipoCanal.Tradicional)
            {
                return RedirectToAction("Index", "Socios", new { area = "Usuario" });
            }

            return View();
        }

        [HttpGet("mecanica/actividades-especiales")]
        public IActionResult ActividadesEspeciales()
        {
            if (_sesion.UsuarioActual.IdCanal != (int)TipoCanal.Tradicional)
            {
                return RedirectToAction("Index", "Socios", new { area = "Usuario" });
            }

            return View();
        }

        [HttpGet("mecanica/bonos")]
        public IActionResult Bonos()
        {
            return View();
        }

        [HttpGet("mecanica/compra-por-app")]
        public IActionResult CompraPorApp()
        {
            return View();
        }
    }
}
