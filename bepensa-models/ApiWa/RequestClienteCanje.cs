﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bepensa_models.ApiWa
{
    public class RequestClienteCanje
    {
        [Required(ErrorMessage = "El campo cliente es requerido")]
        public string Cliente { get; set; }
        [Required(ErrorMessage = "El campo mes es requerido")]
        [Range(1, 12, ErrorMessage = "El valor debe estar entre {1} y {2}")]
        public int Mes { get; set; }
        [Required(ErrorMessage = "El campo año es requerido")]
        public int Anio { get; set; }        
        public int tipoCanjeId { get; set; }
    }
}
