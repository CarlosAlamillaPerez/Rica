using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bepensa_models.DataModels
{
    public class AgregarPremioRequest
    {
        [Display(Name = "IdUsuario")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Usuario inválido.")]
        public int IdUsuario { get; set; }

        [Display(Name = "IdPremio")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Premio inválido.")]
        public int IdPremio { get; set; }

        [Display(Name = "Cantidad")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Cantidad inválida.")]
        public int Cantidad { get; set; } = 1;

        //[Display(Name = "Telefono")]
        //[RegularExpression("(^[0-9]+$)", ErrorMessage = "El campo {0} solo permite números")]
        //[MinLength(10, ErrorMessage = "El campo {0} debe contener 10 caracteres")]
        //[MaxLength(10, ErrorMessage = "El campo {0} debe contener 10 caracteres")]
        //public string? TelefonoRecarga { get; set; } = null;
    }
}
