using bepensa_models.Enums;
using System.ComponentModel.DataAnnotations;

namespace bepensa_models.DTO
{
    public class ActualizarConctactosDTO
    {
        [Display(Name = "IdUsuario")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int IdUsuario { get; set; }

        [Display(Name = "Correo electrónico")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "El correo electrónico proporcionado no es válido, verifícalo.")]
        public string? Email { get; set; }

        [Display(Name = "Celular")]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Solo se permiten números")]
        [MinLength(10, ErrorMessage = "El celular debe contener 10 caracteres")]
        [MaxLength(10, ErrorMessage = "El celular debe contener 10 caracteres")]
        public string? Celular { get; set; }
    }
}
