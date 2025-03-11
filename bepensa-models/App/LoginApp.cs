using bepensa_models.DTO;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace bepensa_models.App
{
    public class LoginApp
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

        [Display(Name = "Sesion")]
        [DefaultValue(null)]
        public Guid? Sesion { get; set; }

        [Display(Name = "Token de dispositivo")]
        [DefaultValue(null)]
        public string? TokenDispositivo { get; set; }
    }
}
