using bepensa_models.Validators;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace bepensa_models.DataModels;

public class CuentaRequest
{
    public long Id { get; set; }

    [Display(Name = "Nombre")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [MaxLength(35, ErrorMessage = "El campo {0} debe contener máximo 35 caracteres")]
    public string Nombre { get; set; } = null!;

    [Display(Name = "Apellido Paterno")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [MaxLength(50, ErrorMessage = "El campo {0} debe contener máximo 50 caracteres")]
    public string ApellidoPaterno { get; set; } = null!;

    [Display(Name = "Apellido Materno")]
    [MaxLength(50, ErrorMessage = "El campo {0} debe contener máximo 50 caracteres")]
    public string? ApellidoMaterno { get; set; }

    [Display(Name = "Celular")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [RegularExpression("(^[0-9]+$)", ErrorMessage = "El campo {0} solo permite números")]
    [MinLength(10, ErrorMessage = "El campo {0} debe contener 10 caracteres")]
    [MaxLength(10, ErrorMessage = "El campo {0} debe contener 10 caracteres")]
    public string Celular { get; set; } = null!;

    [Display(Name = "Correo electrónico")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [MaxLength(80, ErrorMessage = "El campo {0} debe contener máximo 80 caracteres")]
    [DataType(DataType.EmailAddress)]
    [EmailAddress(ErrorMessage = "El {0} proporcionado no es válido, verifícalo")]
    public string Email { get; set; } = null!;

    [Display(Name = "Sexo")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [MaxLength(1, ErrorMessage = "El campo {0} debe contener máximo 1 caracter")]
    public string Sexo { get; set; } = null!;

    [Display(Name = "Fecha de nacimiento")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [FechaNacimientoValida]
    public DateOnly? FechaNacimiento { get; set; }

    [Display(Name = "Calle")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [MaxLength(150, ErrorMessage = "El campo {0} debe contener máximo 150 caracteres")]
    public string Calle { get; set; } = null!;

    [Display(Name = "Número")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [MaxLength(10, ErrorMessage = "El campo {0} debe contener máximo 10 caracteres")]
    public string Numero { get; set; } = null!;

    [Display(Name = "Código Postal")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [MinLength(5, ErrorMessage = "El {0} debe contener 5 caracteres")]
    [MaxLength(5, ErrorMessage = "El {0} debe contener 5 caracteres")]
    public string CodigoPostal { get; set; } = null!;

    [Display(Name = "Barrio")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [MaxLength(150)]
    public string Barrio { get; set; } = null!;

    [Display(Name = "Municipio")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [MaxLength(150, ErrorMessage = "El campo {0} debe contener máximo 150 caracteres")]
    public string Municipio { get; set; } = null!;

    [Display(Name = "Provincia")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [MaxLength(150, ErrorMessage = "El campo {0} debe contener máximo 150 caracteres")]
    public string Provincia { get; set; } = null!;

    [Display(Name = "Referencias")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [MaxLength(400, ErrorMessage = "El campo {0} debe contener máximo 150 caracteres")]
    public string Referencias { get; set; } = null!;

    [Display(Name = "Teléfono")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [RegularExpression("(^[0-9]+$)", ErrorMessage = "El campo {0} solo acepta números")]
    [MinLength(10, ErrorMessage = "El campo {0} debe contener 10 caracteres")]
    [MaxLength(10, ErrorMessage = "El campo {0} debe contener 10 caracteres")]
    public string TelefonoFijo { get; set; } = null!;

    //[JsonIgnore]
    //public byte[] Password { get; set; } = null!;

    [Display(Name = "IdOperador")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public int IdOperador { get; set; }

    [JsonIgnore]
    public bool ExitosoOutput { get; set; }

    [JsonIgnore]
    [MaxLength(3000)]
    public string? ErrorOutput { get; set; }
}
