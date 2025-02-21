using bepensa_models.DTO;
using System.ComponentModel.DataAnnotations;

namespace bepensa_models.CRM;

public class LoginCRMRequest
{
    [Display(Name = "Correo electrónico")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [DataType(DataType.EmailAddress)]
    [EmailAddress(ErrorMessage = "El {0} proporcionado no es válido, verifícalo")]
    [MaxLength(180, ErrorMessage = "El campo {0} excede el número de caracteres permitidos")]
    [MinLength(1, ErrorMessage = "El campo {0} requiere {1} caracter(es) mínimo")]
    public string Email { get; set; } = null!;

    [Display(Name = "Contraseña")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [DataType(DataType.Password)]
    [MaxLength(20, ErrorMessage = "El campo {0} excede el número de caracteres permitidos")]
    [MinLength(8, ErrorMessage = "El campo {0} requiere {1} caracteres mínimo")]
    public string Password { get; set; } = null!;

    public AccessDTO ControlAcceso { get; set; } =  new();
}
