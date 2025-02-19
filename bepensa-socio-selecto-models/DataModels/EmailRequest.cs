using System.ComponentModel.DataAnnotations;

namespace bepensa_socio_selecto_models.DataModels;

public class EmailRequest
{
    [Display(Name = "Correo electrónico")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [DataType(DataType.EmailAddress)]
    [EmailAddress(ErrorMessage = "El {0} proporcionado no es válido, verifícalo.")]
    [MaxLength(80, ErrorMessage = "El campo {0} debe contener máximo 50 caracteres")]
    public string Email { get; set; } = null!;
}
