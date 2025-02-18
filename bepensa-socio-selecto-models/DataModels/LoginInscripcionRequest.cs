using bepensa_socio_selecto_models.DTO;
using System.ComponentModel.DataAnnotations;

namespace bepensa_socio_selecto_models.DataModels;

public class LoginInscripcionRequest
{
    [Display(Name = "Usuario")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [RegularExpression(@"(^[0-9]+$)", ErrorMessage = "El campo {0} debe contener solo números.")]
    [MinLength(1, ErrorMessage = "El campo {0} debe contener mínimo de un carácter")]
    [MaxLength(30, ErrorMessage = "El campo {0} debe contener máximo 50 caracteres")]
    public string Cuc { get; set; } = null!;

    [Display(Name = "Contraseña")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [DataType(DataType.Password)]
    [Compare("Cuc", ErrorMessage = "Usuario y/o Contraseña incorrectos")]
    [MinLength(1, ErrorMessage = "El campo {0} debe contener mínimo de un carácter")]
    [MaxLength(30, ErrorMessage = "El campo {0} debe contener máximo 50 caracteres")]
    public string Password { get; set; } = null!;

    public InscripcionDTO? Inscripcion { get; set; }
}
