using bepensa_models.Enums;
using System.ComponentModel.DataAnnotations;

namespace bepensa_models.DTO
{
    public class ActualizarConctactosDTO
    {
        [Display(Name = "IdUsuario")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public long IdUsuario { get; set; }

        [Display(Name = "Correo electrónico")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "El correo electrónico proporcionado no es válido, verifícalo.")]
        public string Email { get; set; } = null!;

        [Display(Name = "Celular")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Solo se permiten números")]
        [MinLength(10, ErrorMessage = "El celular debe contener 10 caracteres")]
        [MaxLength(10, ErrorMessage = "El celular debe contener 10 caracteres")]
        public string Celular { get; set; } = null!;
    }
}
