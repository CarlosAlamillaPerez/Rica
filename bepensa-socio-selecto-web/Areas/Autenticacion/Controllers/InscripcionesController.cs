using bepensa_socio_selecto_biz.Interfaces;
using bepensa_socio_selecto_models.DataModels;
using Microsoft.AspNetCore.Mvc;

namespace bepensa_ss_inscripcion.Areas.Autenticacion.Controllers
{
    [Area("Autenticacion")]
    public class InscripcionesController : Controller
    {
        private IAccessSession _sesion { get; set; }
        private readonly IInscripcion _inscripcion;

        public InscripcionesController(IAccessSession sesion, IInscripcion inscripcion)
        {
            _sesion = sesion;
            _inscripcion = inscripcion;
        }

        [HttpGet]
        public IActionResult Login()
        {
            _sesion.Logout();

            return View(new LoginInscripcionRequest());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginInscripcionRequest pInscripcion)
        {

            var valida = await _inscripcion.ConsultarUsuario(pInscripcion);

            if (!valida.Exitoso || valida.Data == null)
            {
                ViewData["msgError"] = valida.Mensaje;

                return View();
            }

            pInscripcion.Inscripcion = valida.Data;

            _sesion.CredencialesInscripcion = pInscripcion;

            return RedirectToAction("Inscripcion", "Registros", new { area = "Inscripcion" });
        }
    }
}
