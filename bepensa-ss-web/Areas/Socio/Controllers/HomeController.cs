using bepensa_biz.Interfaces;
using bepensa_data.models;
using bepensa_models.DataModels;
using bepensa_models.DTO;
using bepensa_models.Enums;
using bepensa_models.General;
using bepensa_ss_web.Filters;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace bepensa_ss_web.Areas.Socio.Controllers
{
    [Area("Socio")]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [ValidaSesionUsuario]
    public class HomeController : Controller
    {
        private IAccessSession _session { get; set; }
        private readonly IUsuario _usuario;
        private readonly IObjetivo _objetivo;
        private readonly IEncuesta _encuesta;
        private readonly IDireccion _colonia;

        public HomeController(IAccessSession session, IObjetivo objetivo, IUsuario usuario, IEncuesta encuesta, IDireccion colonia)
        {
            _session = session;
            _usuario = usuario;
            _encuesta = encuesta;
            _colonia = colonia;
        }

        [HttpGet("home")]
        public IActionResult Index()
        {

            if (TempData["clvKitBienvenida"] != null && TempData["clvKitBienvenida"] is string json)
            {
                var bitacora = JsonConvert.DeserializeObject<BitacoraEncuestaDTO>(json);

                ViewBag.Encuesta = bitacora;
            }
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


        [HttpPost("home/guardar-encuesta")]
        public JsonResult ResponderEncuesta([FromBody] EncuestaRequest data)
        {
            var encuesta = _encuesta.ConsultarEncuestas(_session.UsuarioActual.Id).Data?.Where(x => x.Encuesta.Codigo.Equals("clvKitBienvenida")).FirstOrDefault();

            data.IdBitacoraEncuesta = encuesta.Id;
            data.IdUsuario = _session.UsuarioActual.Id;

            var resultado = _encuesta.ResponderEncuesta(data, (int)TipoOrigen.Web);

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

        #region Dirección
        /// <summary>
        /// Consulta las colonias con base al código postal
        /// </summary>
        /// <param name="CP"></param>
        /// <returns>Lista de colonias</returns>
        [HttpGet("consulta/colonias/{CP}")]
        public async Task<JsonResult> ConsultarColonia(string CP)
        {
            var resultado = await _colonia.ConsultarColonias(CP);

            return Json(resultado);
        }

        [HttpGet("consulta/municipio/{idColonia}")]
        public async Task<JsonResult> ConsultarMunicipio(int idColonia)
        {
            var resultado = await _colonia.ConsultarMunicipio(idColonia);

            return Json(resultado);
        }

        [HttpGet("consulta/estado/{idMunicipio}")]
        public async Task<JsonResult> ConsultarEstado(int idMunicipio)
        {
            var resultado = await _colonia.ConsultarEstado(idMunicipio);

            return Json(resultado);
        }
        #endregion
    }
}
