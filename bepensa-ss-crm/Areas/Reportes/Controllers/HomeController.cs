using bepensa_biz.Interfaces;
using bepensa_biz.Settings;
using bepensa_models.DataModels;
using bepensa_models.DTO;
using bepensa_models.General;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace bepensa_ss_crm.Areas.Reportes.Controllers
{
    [Area("Reportes")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly GlobalSettings _app;
        private IAccessSession _sesion { get; set; }

        private readonly IReporte _reporte;

        private readonly IExportacion _exportacion;

        private readonly string ExcelContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        public HomeController(IWebHostEnvironment webHostEnvironment, IOptionsSnapshot<GlobalSettings> appAjustes, IAccessSession sesion, IReporte reporte, IExportacion exportacion)
        {
            _webHostEnvironment = webHostEnvironment;
            _app = appAjustes.Value;
            _sesion = sesion;
            _reporte = reporte;
            _exportacion = exportacion;
        }

        [HttpGet("reportes")]
        public IActionResult Index()
        {
            var resultado = _reporte.TableroDescargas();

            var model = resultado.Data ?? new List<ReporteDTO>();

            return View(model);
        }

        [HttpPost("reportes/mostrar-reporte")]
        [HttpPost("reportes/mostrar-reporte/{page}")]
        public IActionResult CargaReporte(ReporteRequest data, int page = 1)
        {
            var resultado = _reporte.ReportesDinamico(data);

            ReporteGeneral<List<dynamic>> model = new()
            {
                DatosReportes = data,
                Data = resultado.Data
            };

            return PartialView("_reporte", model);
        }

        [HttpPost("reportes/descargar-reporte")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Descargar([FromBody]ReporteRequest data)
        {
            string rootPath = _webHostEnvironment.WebRootPath;

            string path = Path.Combine(rootPath, "img/reportes", "logoreporte.jpg");

            FileInfo img = new FileInfo(path);

            var model = _reporte.ReportesDinamico(data).Data;

            var fechaInicio = data.FechaInicial!.Value.ToDateTime(TimeOnly.MinValue);

            var fechaFin = data.FechaFinal!.Value.ToDateTime(TimeOnly.MinValue);

            byte[] file = await _exportacion.GeneraExportacionDinamicaAsync(data.IdReporte, img, model, fechaInicio, fechaFin);

            return File(file, ExcelContentType, data.NombreReporte + ".xlsx");
        }
    }
}
