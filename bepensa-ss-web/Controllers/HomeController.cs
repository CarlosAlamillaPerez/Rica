using bepensa_ss_web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace bepensa_ss_web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("docs/aviso-privacidad")]
        public IActionResult Privacy()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "docs", "aviso-privacidad", "AVISO-DE-PRIVACIDAD-SIMPLIFICADO-BEPENSA-2025.pdf");

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);

            return File(fileBytes, "application/pdf");
        }

        [HttpGet("docs/terminos-condiciones")]
        public IActionResult TyC()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "docs", "terminos-condiciones", "TyC- SOCIO-SELECTO-2025.pdf");

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);

            return File(fileBytes, "application/pdf");
        }

        [HttpGet("docs/politica-garantia")]
        public IActionResult Policy()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "docs", "politica-garantia", "politica-de-garantias-de-premios.pdf");

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);

            return File(fileBytes, "application/pdf");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
