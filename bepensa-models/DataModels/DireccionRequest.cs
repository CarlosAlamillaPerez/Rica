using System.ComponentModel.DataAnnotations;

namespace bepensa_models.DataModels;

public class DireccionRequest
{
    [Display(Name = "Calle")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [MaxLength(150, ErrorMessage = "El campo {0} debe contener máximo 150 caracteres")]
    public string Calle { get; set; } = null!;

    [Display(Name = "No. Exterior")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [MaxLength(20, ErrorMessage = "El campo {0} debe contener máximo 20 caracteres")]
    public string NumeroExterior { get; set; } = null!;

    [Display(Name = "No. Interior")]
    [MaxLength(20, ErrorMessage = "El campo {0} debe contener máximo 20 caracteres")]
    public string? NumeroInterior { get; set; } = null;

    [Display(Name = "Código Postal")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [RegularExpression("(^[0-9]+$)", ErrorMessage = "El campo {0} solo permite números")]
    [MinLength(5, ErrorMessage = "El {0} debe contener 5 caracteres")]
    [MaxLength(5, ErrorMessage = "El {0} debe contener 5 caracteres")]
    public string CodigoPostal { get; set; } = null!;

    [Display(Name = "Colonia")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [Range(1, int.MaxValue, ErrorMessage = "Colonia inválida.")]
    public int IdColonia { get; set; }

    [Display(Name = "Ciudad")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [MaxLength(150, ErrorMessage = "El {0} debe contener 150 caracteres máximo.")]
    public string Ciudad { get; set; } = null!;

    [Display(Name = "Entre calle")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [MaxLength(150)]
    public string CalleInicio { get; set; } = null!;

    [Display(Name = "Y calle")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [MaxLength(150, ErrorMessage = "El campo {0} debe contener máximo 150 caracteres")]
    public string CalleFin { get; set; } = null!;

    [Display(Name = "Referencias")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [MaxLength(400, ErrorMessage = "El campo {0} debe contener 400 caracteres máximo")]
    public string Referencias { get; set; } = null!;

    [Display(Name = "Teléfono de contaco")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [RegularExpression("(^[0-9]+$)", ErrorMessage = "El campo {0} solo acepta números")]
    [MinLength(10, ErrorMessage = "El campo {0} debe contener 10 caracteres")]
    [MaxLength(10, ErrorMessage = "El campo {0} debe contener 10 caracteres")]
    [DataType(DataType.PhoneNumber)]
    [Phone(ErrorMessage = "Por favor, ingresa un número de teléfono válido.")]
    public string Telefono { get; set; } = null!;
}
