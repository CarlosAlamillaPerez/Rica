using bepensa_biz.Interfaces;
using bepensa_biz.Security;
using bepensa_models.DataModels;
using bepensa_models.DTO;
using bepensa_models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace bepensa_ss_op_web.Controllers
{
    public class SocioController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IObjetivo _objetivo;

        public SocioController(IConfiguration configuration, IObjetivo objetivo)
        {
            this.configuration = configuration;
            _objetivo = objetivo;
        }


        [HttpGet("landing/fdv/{pCuc}/{pToken}")]
        public IActionResult Index(string pCuc, Guid pToken)
        {
            Hash hash = new(pToken.ToString());

            var _token = hash.Sha512();

            var token = Convert.ToHexString(_token);

            if (token == null || !(token == configuration.GetValue<string>("KeySocioSelecto")))
            {
                TempData["msgError"] = CodigoDeError.SesionCaducada.GetDescription();

                return RedirectToAction("Login", "Cuentas", new { area = "Autenticacion" });
            }

            var modelo = new ResumenSocioSelectoDTO();

            var resultado = _objetivo.ResumenSocioSelecto(new LandingFDVRequest { Cuc = pCuc });

            if (resultado.Data != null)
            {
                modelo = resultado.Data;
            }

            return View(modelo);
        }
    }
}
