﻿using bepensa_biz.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bepensa_ss_crm.Areas.Usuario.Controllers
{
    [Area("Usuario")]
    [Authorize]
    public class LlamadasController : Controller
    {
        private readonly IDropDownList _listas;
        private readonly ILlamada _llamada;

        public LlamadasController(ILlamada llamada, IDropDownList listas)
        {
            _llamada = llamada;
            _listas = listas;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CapturaRapida()
        {
            return PartialView("_llamadaModal");
        }

        [HttpGet("llamadas/{id}/subcategorias-llamada")]
        public JsonResult Subcategoria(int id)
        {
            var resultado = _listas.SubcategoriasLlamada(id);

            return Json(resultado);
        }
    }
}
