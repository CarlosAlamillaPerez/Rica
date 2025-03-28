using bepensa_models.Enums;
using System.ComponentModel.DataAnnotations;

namespace bepensa_models.DataModels;

public class RestablecerPassRequest
{
    public TipoMensajeria TipoMensajeria { get; set; }

    [Display(Name = "No Cliente")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [MinLength(1, ErrorMessage = "El campo {0} debe contener máximo 80 caracteres")]
    [MaxLength(80, ErrorMessage = "El campo {0} debe contener mínimo {1} caracteres")]
    public string Cuc { get; set; } = null!;

    [Display(Name = "Contacto")]
    [Required(ErrorMessage = "Es requerido ingresar el medio de envío.")]
    [MaxLength(80, ErrorMessage = "El campo {0} debe contener máximo 80 caracteres")]
    public string Contacto { get; set; } = null!;
}
