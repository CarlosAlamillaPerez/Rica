using bepensa_biz.Interfaces;
using bepensa_biz.Security;
using bepensa_models.DataModels;
using bepensa_models.DTO;
using bepensa_models.Enums;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace bepensa_ss_free.Controllers
{
    public class SocioController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IObjetivo _objetivo;

        public SocioController (IConfiguration configuration, IObjetivo objetivo)
        {
            this.configuration = configuration;

            _objetivo = objetivo;
        }

        public IActionResult Index()
        {
            return Content("Sitio no encontrado");
        }

        [HttpGet("landing/fdv/{pCuc}")]
        public IActionResult V1(string pCuc)
        {
            var resultado = _objetivo.ResumenSocioSelecto(new LandingFDVRequest { Cuc = pCuc });

            var model = resultado.Data ?? new ResumenSocioSelectoDTO();

            return View("Index", model);
        }

        [HttpGet("landing/fdv/{pCuc}/{pToken}")]
        public IActionResult V2(string pCuc, string pToken)
        {
            string expectedToken = configuration.GetValue<string>("KeySocioSelecto");

            var hashedTokenBytes = new Hash(pToken.ToString()).Sha512();

            var hashedToken = Convert.ToHexString(hashedTokenBytes);

            if (!string.Equals(hashedToken, expectedToken, StringComparison.Ordinal))
            {
                return BadRequest();
            }

            var resultado = _objetivo.ResumenSocioSelecto(new LandingFDVRequest { Cuc = pCuc });

            var model = resultado.Data ?? new ResumenSocioSelectoDTO();

            return View("Index", model);
        }

        [HttpGet("landing/fdv/{pCuc}/1d4d7f36-4767-4744-897e-d4e3ed8bc907")]
        public IActionResult V3(string pCuc)
        {
            var resultado = _objetivo.ResumenSocioSelecto(new LandingFDVRequest { Cuc = pCuc });

            var model = resultado.Data ?? new ResumenSocioSelectoDTO();

            return View("Index", model);
        }
    }
}
