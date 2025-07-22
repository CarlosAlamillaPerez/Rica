using bepensa_biz.Interfaces;
using App = bepensa_biz.Settings;
using bepensa_data.models;
using bepensa_models.DataModels;
using bepensa_models.DTO;
using bepensa_models.Enums;
using bepensa_models.General;
using bepensa_ss_op_web.Filters;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace bepensa_ss_op_web.Areas.Socio.Controllers
{
    [Area("Socio")]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [ValidaSesionUsuario]
    public class HomeController : Controller
    {
        private readonly App.GlobalSettings app;
        private IAccessSession _session { get; set; }
        private readonly IUsuario _usuario;
        private readonly IEncuesta _encuesta;
        private readonly IConverter _converter;
        private readonly IBitacoraEnvioCorreo _bitacoraEnvioCorreo;
        private readonly IDireccion _colonia;

        private readonly IApp _app;

        public HomeController(IOptionsSnapshot<App.GlobalSettings> app, IAccessSession session, IUsuario usuario,
            IEncuesta encuesta, IDireccion colonia,
            IConverter converter, IBitacoraEnvioCorreo bitacoraEnvioCorreo,
            IApp _app)
        {
            this.app = app.Value;
            _session = session;
            _usuario = usuario;
            _encuesta = encuesta;
            _converter = converter;
            _colonia = colonia;
            _bitacoraEnvioCorreo = bitacoraEnvioCorreo;
            this._app = _app;
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
            CambiarPasswordDTO model = new CambiarPasswordDTO();

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

        //------------------------------------- DinkToPdf -------------------------------------
        [HttpGet("docs/pdf/estado-de-cuenta/{pIdPeriodo}")]
        public IActionResult DocEstadoCuenta(int pIdPeriodo)
        {
            if (_session.FuerzaVenta == null)
            {
                return RedirectToAction("Index", "Home", new { area = "Socio" });
            }

            var resultado = _bitacoraEnvioCorreo.ConsultarPlantilla("edo-cta-ss", _session.UsuarioActual.Id, pIdPeriodo);

            if (!resultado.Exitoso || resultado.Data == null)
            {
                TempData["msgError"] = resultado.Mensaje;

                return RedirectToAction("Index", "EstadoCuenta", new { area = "Socio" });
            }

            var html = resultado.Data.Html;

            app.RutaLocalImg = app.RutaLocalImg.Replace("\\", "/");

            html = html.Replace("@RUTA", app.RutaLocalImg);

            var pdf = PDF(html);
            //System.IO.File.WriteAllBytes($"wwwroot/docs/pdf/{Guid.NewGuid()}.pdf", pdf); // Para guardar

            //return File(pdf, "application/pdf", "estado-de-cuenta.pdf"); // Sin vista previa, descarga directa.

            return File(pdf, "application/pdf");
        }

        public byte[] PDF(string html)
        {
            var doc = new HtmlToPdfDocument
            {
                GlobalSettings = {
                PaperSize = PaperKind.A4,
                Orientation = Orientation.Portrait
            },
                Objects = {
                    new ObjectSettings {
                        HtmlContent = html,
                        WebSettings = {
                        LoadImages = true,
                        EnableJavascript = true,
                        DefaultEncoding = "utf-8",
                        UserStyleSheet = null,
                        }
                    }
                }
            };

            return _converter.Convert(doc);
        }
        //------------------------------------- DinkToPdf -------------------------------------
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

        #region Alianzas
        [HttpGet("alianzas")]
        public async Task<IActionResult> Alianzas()
        {
            string urlAlianzas = $"https://alianzas.lms-la.com/paso.php?tK=YFU3YNZkQF4NbdW1ZCkoGE8stBRk7rcUwwMK9muOrocHHy9g0iK7mHoVJ55FmWZY&IdC={_session.UsuarioActual.Cuc}&Mn=";


            if (_session.UsuarioActual.FechaNacimiento != null)
            {
                urlAlianzas = urlAlianzas + $"{_session.UsuarioActual.FechaNacimiento.Value.Month}";
            }

            await _app.SeguimientoVistas(new SegVistaRequest
            {
                IdUsuario = _session.UsuarioActual.Id,
                IdVisita = 10,
                IdFDV = _session?.FuerzaVenta?.Id
            }, (int)TipoOrigen.Web);

            return Redirect(urlAlianzas);
        }
        #endregion
    }
}
