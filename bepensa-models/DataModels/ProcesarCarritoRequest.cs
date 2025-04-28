using System.ComponentModel.DataAnnotations;

namespace bepensa_models.DataModels;

public class ProcesarCarritoRequest
{
    [Display(Name = "IdUsuario")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [Range(1, int.MaxValue, ErrorMessage = "Usuario inválido.")]
    public int IdUsuario { get; set; }

    [Display(Name = "Nombre")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [MaxLength(255, ErrorMessage = "El campo {0} debe contener máximo {1} caracteres")]
    [RegularExpression("^[A-Za-zÁáÉéÍíÓóÚúÑñ\\s]+$", ErrorMessage = "{0} inválido.")]
    public string Nombre { get; set; } = null!;

    [Display(Name = "Correo electrónico")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [MaxLength(80, ErrorMessage = "El campo {0} debe contener máximo 80 caracteres")]
    [DataType(DataType.EmailAddress)]
    [EmailAddress(ErrorMessage = "El {0} proporcionado no es válido, verifícalo")]
    public string Email { get; set; } = null!;

    [Display(Name = "Teléfono de contaco")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [RegularExpression("(^[0-9]+$)", ErrorMessage = "El campo {0} solo acepta números")]
    [MinLength(10, ErrorMessage = "El campo {0} debe contener 10 caracteres")]
    [MaxLength(10, ErrorMessage = "El campo {0} debe contener 10 caracteres")]
    [DataType(DataType.PhoneNumber)]
    [Phone(ErrorMessage = "Por favor, ingresa un número de teléfono válido.")]
    public string Telefono { get; set; } = null!;

    public DireccionRequest? Direccion { get; set; }
}
