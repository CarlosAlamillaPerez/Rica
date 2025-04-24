using bepensa_biz.Interfaces;
using bepensa_data.models;
using bepensa_models.DTO;
using bepensa_models.Enums;
using bepensa_models.General;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bepensa_ss_web.Areas.Socio.Controllers
{
    [Area("Socio")]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class HomeController : Controller
    {
        private IAccessSession _session { get; set; }
        private readonly IUsuario _usuario;
        private readonly IObjetivo _objetivo;

        public HomeController(IAccessSession session, IObjetivo objetivo, IUsuario usuario)
        {
            _session = session;
            _objetivo = objetivo;
            _usuario = usuario;
        }

        [HttpGet("home")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("mi-cuenta")]
        public IActionResult MiCuenta()
        {
            return View(_session.UsuarioActual);
        }

        [HttpPost("mi-cuenta/actualizar-contactos")]
        public async Task<JsonResult> ActualizarContactos([FromBody] ActualizarConctactosDTO data)
        {
            var resultado = new Respuesta<bool>();

            if (_session.FuerzaVenta != null)
            {
                resultado.Codigo = (int)CodigoDeError.OperadorNoAutorizado;
                resultado.Mensaje = CodigoDeError.OperadorNoAutorizado.GetDisplayName();
                resultado.Exitoso = false;

                return Json(resultado);
            }

            resultado = await _usuario.Actualizar(data.IdUsuario, data.Celular, data.Email);

            if (resultado.Data)
            {
                UsuarioDTO usuario = _session.UsuarioActual;
                usuario.Email = data.Email;
                usuario.Celular = data.Celular;

                _session.UsuarioActual = usuario;
            }

            return Json(resultado);
        }

        [HttpGet("/cambiar-clave-de-acceso")]
        public IActionResult CambiarPassword()
        {
            CambiarPasswordDTO model = new();

            return View(model);

        }
        [HttpPost("/cambiar-clave-de-acceso")]
        public JsonResult CambiarPassword(CambiarPasswordDTO passwords)
        {
            var resultado = new Respuesta<bool>();

            if (_session.FuerzaVenta != null)
            {
                resultado.Codigo = (int)CodigoDeError.OperadorNoAutorizado;
                resultado.Mensaje = CodigoDeError.OperadorNoAutorizado.GetDisplayName();
                resultado.Exitoso = false;

                return Json(resultado);
            }
            
            passwords.IdUsuario = _session.UsuarioActual.Id;

            resultado = _usuario.CambiarContrasenia(passwords);
        
            return Json(resultado);
        }
    }
}
