using bepensa_biz.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bepensa_ss_crm.Areas.Usuario.Controllers
{
    [Area("Usuario")]
    [Authorize]
    public class LlamadasController : Controller
    {
        private readonly ILlamada _llamada;

        public LlamadasController(ILlamada llamada)
        {
            _llamada = llamada;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CapturaRapida()
        {
            return PartialView("_llamadaModal");
        }
    }
}
