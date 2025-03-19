using System.ComponentModel.DataAnnotations;

namespace bepensa_models.DTO;

public class LoginDTO
{
    [Display(Name = "Usuario")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [DataType(DataType.Text)]
    public string Usuario { get; set; } = null!;

    [Display(Name = "Contraseña")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [DataType(DataType.Password)]
    [MaxLength(20, ErrorMessage = "Excede el número de caracteres permitidos")]
    [MinLength(9, ErrorMessage = "Mínimo requiere {1} caracteres")]
    [RegularExpression("^(?=.*\\d)(?=.*[\u0021-\u002f\u003a-\u0040\u005b-\u005f\u00bf\u00a1])(?=.*[A-Z])(?=.*[a-z])\\S{8,20}$", ErrorMessage = "La contraseña debe contener un mínimo de 9 caracteres: de los cuales debe incluir una letra mayúscula, una letra minúscula, un número y un carácter.")]
    public string Password { get; set; } = null!;

    public AccessDTO AccessControl { get; set; } = new AccessDTO();
}
