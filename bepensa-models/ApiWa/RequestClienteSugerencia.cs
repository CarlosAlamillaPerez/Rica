using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bepensa_models.ApiWa
{
    public class RequestClienteSugerencia
    {
        [Required(ErrorMessage = "El campo cliente es requerido")]
        public string Cliente { get; set; }

        [Required(ErrorMessage = "El campo comentarios es requerido")]
        public string Comentarios { get; set; }
    }
}
