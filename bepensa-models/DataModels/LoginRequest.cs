using bepensa_models.DTO;
using System.ComponentModel.DataAnnotations;

namespace bepensa_models.DataModels;

public class LoginRequest
{
    [Display(Name = "Usuario")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [RegularExpression(@"(^[0-9]+$)", ErrorMessage = "El campo {0} debe contener solo números.")]
    [MinLength(1, ErrorMessage = "El campo {0} debe contener mínimo de un carácter")]
    [MaxLength(30, ErrorMessage = "El campo {0} debe contener máximo 50 caracteres")]
    public string Usuario { get; set; } = null!;

    [Display(Name = "Contraseña")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [DataType(DataType.Password)]
    [MinLength(1, ErrorMessage = "El campo {0} debe contener mínimo de un carácter")]
    [MaxLength(30, ErrorMessage = "El campo {0} debe contener máximo 50 caracteres")]
    public string Password { get; set; } = null!;

    public AccessDTO AccessControl { get; set; } = new();
}
