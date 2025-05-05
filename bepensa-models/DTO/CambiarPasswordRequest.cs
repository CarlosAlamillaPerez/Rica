using System.ComponentModel.DataAnnotations;

namespace bepensa_models.DTO
{
    public class CambiarPasswordRequest
    {
        public Guid? Token { get; set; }

        [Display(Name = "Contraseña nueva")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DataType(DataType.Password)]
        [MaxLength(20, ErrorMessage = "Excede los {1} caracteres permitidos")]
        [MinLength(8, ErrorMessage = "Mínimo requiere {1} caracteres")]
        [RegularExpression("^(?=.*\\d)(?=.*[\u0021-\u002f\u003a-\u0040\u005b-\u005f\u00bf\u00a1])(?=.*[A-Z])(?=.*[a-z])\\S{8,20}$", ErrorMessage = "La contraseña debe contener un mínimo de 8 caracteres: de los cuales debe incluir una letra mayúscula, una letra minúscula, un número y un carácter.")]
        public string Password { get; set; } = null!;

        [Display(Name = "Confirma tu contraseña")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DataType(DataType.Password)]
        [MaxLength(20, ErrorMessage = "Excede los {1} caracteres permitidos")]
        [MinLength(8, ErrorMessage = "Mínimo requiere {1} caracteres")]
        [RegularExpression("^(?=.*\\d)(?=.*[\u0021-\u002f\u003a-\u0040\u005b-\u005f\u00bf\u00a1])(?=.*[A-Z])(?=.*[a-z])\\S{8,20}$", ErrorMessage = "La contraseña debe contener un mínimo de 8 caracteres: de los cuales debe incluir una letra mayúscula, una letra minúscula, un número y un carácter.")]

        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmarPassword { get; set; } = null!;
    }
}
