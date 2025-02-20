using bepensa_socio_selecto_models.Validators;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace bepensa_socio_selecto_models.DataModels;

public class InscripcionRequest
{


    [Display(Name = "No. de Cliente")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public string Cuc { get; set; } = null!;
    //[Display(Name = "Nombre")]
    //[Required(ErrorMessage = "El campo {0} es obligatorio")]
    //[MaxLength(35, ErrorMessage = "El campo {0} debe contener máximo 35 caracteres")]
    //public string Nombre { get; set; } = null!;

    //[Display(Name = "Apellido paterno")]
    //[Required(ErrorMessage = "El campo {0} es obligatorio")]
    //[MaxLength(50, ErrorMessage = "El campo {0} debe contener máximo 50 caracteres")]
    //public string ApellidoPaterno { get; set; } = null!;

    //[Display(Name = "Apellido materno")]
    //[MaxLength(50, ErrorMessage = "El campo {0} debe contener máximo 50 caracteres")]
    //public string? ApellidoMaterno { get; set; }

    [Display(Name = "Ruta")]
    [RegularExpression("(^[0-9]+$)", ErrorMessage = "El campo {0} solo permite números")]
    [MaxLength(10, ErrorMessage = "El campo {0} debe contener 10 caracteres")]
    public string? TipoRuta { get; set; }

    [Display(Name = "Celular")]
    [RegularExpression("(^[0-9]+$)", ErrorMessage = "El campo {0} solo permite números")]
    [MinLength(10, ErrorMessage = "El campo {0} debe contener 10 caracteres")]
    [MaxLength(10, ErrorMessage = "El campo {0} debe contener 10 caracteres")]
    [DataType(DataType.PhoneNumber)]
    [Phone(ErrorMessage = "Por favor, ingresa un número de celular válido.")]
    public string? Celular { get; set; } = null!;

    [Display(Name = "Correo electrónico")]
    [MaxLength(80, ErrorMessage = "El campo {0} debe contener máximo 80 caracteres")]
    [DataType(DataType.EmailAddress)]
    [EmailAddress(ErrorMessage = "El {0} proporcionado no es válido, verifícalo")]
    public string? Email { get; set; } = null!;

    [Display(Name = "Sexo")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [MaxLength(1, ErrorMessage = "El campo {0} debe contener máximo 1 caracter")]
    public string Sexo { get; set; } = null!;

    [Display(Name = "Fecha de nacimiento")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [FechaNacimientoValida(ErrorMessage = "La persona debe ser mayor de 18 años para registrarse.")]
    public DateOnly? FechaNacimiento { get; set; }

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
    public string? NumeroInterior { get; set; }

    [Display(Name = "Código Postal")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [RegularExpression("(^[0-9]+$)", ErrorMessage = "El campo {0} solo permite números")]
    [MinLength(5, ErrorMessage = "El {0} debe contener 5 caracteres")]
    [MaxLength(5, ErrorMessage = "El {0} debe contener 5 caracteres")]
    public string CodigoPostal { get; set; } = null!;

    [Display(Name = "Colonia")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [Range(1, int.MaxValue, ErrorMessage = "Colonia inválida.")]
    public int? IdColonia { get; set; }

    [Display(Name = "Ciudad")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [MaxLength(150, ErrorMessage = "El {0} debe contener 150 caracteres máximo.")]
    public string? Ciudad { get; set; }

    [Display(Name = "Entre calle")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [MaxLength(150)]
    public string? CalleInicio { get; set; }

    [Display(Name = "Y calle")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [MaxLength(150, ErrorMessage = "El campo {0} debe contener máximo 150 caracteres")]
    public string? CalleFin { get; set; }

    [Display(Name = "Teléfono")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [RegularExpression("(^[0-9]+$)", ErrorMessage = "El campo {0} solo acepta números")]
    [MinLength(10, ErrorMessage = "El campo {0} debe contener 10 caracteres")]
    [MaxLength(10, ErrorMessage = "El campo {0} debe contener 10 caracteres")]
    [DataType(DataType.PhoneNumber)]
    [Phone(ErrorMessage = "Por favor, ingresa un número de teléfono válido.")]
    public string? Telefono { get; set; } = null!;

    [Display(Name = "Referencias")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [MaxLength(400, ErrorMessage = "El campo {0} debe contener 400 caracteres máximo")]
    public string? Referencias { get; set; }
}
