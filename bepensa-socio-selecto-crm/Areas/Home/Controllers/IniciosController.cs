﻿using bepensa_socio_selecto_biz.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace bepensa_socio_selecto_crm.Areas.Home.Controllers
{
    [Area("Home")]
    public class IniciosController : Controller
    {
        private readonly IAccessSession _sesion;

        public IniciosController(IAccessSession sesion)
        {
            _sesion = sesion;
        }

        [HttpGet("home")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
