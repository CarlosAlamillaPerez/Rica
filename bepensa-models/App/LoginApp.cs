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
        public string Password { get; set; } = null!;

        [Display(Name = "Sesion")]
        [DefaultValue(null)]
        public Guid? Sesion { get; set; }

        [Display(Name = "Token de dispositivo")]
        [DefaultValue(null)]
        public string? TokenDispositivo { get; set; }
    }
}
