using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bepensa_models.ApiWa
{
    public class RequestClienteCanjeDetalle
    {
        [Required(ErrorMessage = "El campo cliente es requerido")]
        public string Cliente { get; set; }

        [Required(ErrorMessage = "El campo folio es requerido")]
        public string folio { get; set; }
    }
}
