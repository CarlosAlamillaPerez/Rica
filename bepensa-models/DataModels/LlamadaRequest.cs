using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace bepensa_models.DataModels;

public class LlamadaRequest
{
    [Display(Name = "Seguimiento de llamada")]
    [Range(1, int.MaxValue, ErrorMessage = "Llamada inválida")]
    [DefaultValue(null)]
    public int? IdPadre { get; set; }

    [Display(Name = "Motivo")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [MaxLength(510, ErrorMessage = "El campo {0} debe contener máximo {1} caracteres")]
    public string? Tema { get; set; }

    [Display(Name = "Usuario")]
    [Range(1, int.MaxValue, ErrorMessage = "Usuario inválido")]
    [DefaultValue(null)]
    public int? IdUsuario { get; set; }

    [Display(Name = "Nombre de quién marca*")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [MaxLength(100, ErrorMessage = "El campo {0} debe contener máximo {1} caracteres")]
    [RegularExpression("^[A-Za-zÁáÉéÍíÓóÚúÑñ\\s]+$", ErrorMessage = "{0} inválido.")]
    public string Nombre { get; set; } = null!;

    [Display(Name = "Teléfono*")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [RegularExpression("(^[0-9]+$)", ErrorMessage = "El campo {0} solo acepta números")]
    [MinLength(10, ErrorMessage = "El campo {0} debe contener 10 caracteres")]
    [MaxLength(10, ErrorMessage = "El campo {0} debe contener 10 caracteres")]
    [DataType(DataType.PhoneNumber)]
    [Phone(ErrorMessage = "Por favor, ingresa un número de teléfono válido.")]
    public string Telefono { get; set; } = null!;

    [Display(Name = "Comentario*")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [MaxLength(400, ErrorMessage = "El {0} debe contener {1} caracteres máximo.")]
    public string Comentario { get; set; } = null!;

    [Display(Name = "Tipo de llamada*")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [Range(1, int.MaxValue, ErrorMessage = "Tipo de llamada inválida")]
    [DefaultValue(0)]
    public int IdTipoLlamada { get; set; }

    [Display(Name = "Subcategoría de llamada*")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [Range(1, int.MaxValue, ErrorMessage = "Subcategoría de llamada inválida")]
    [DefaultValue(0)]
    public int IdSubcategoriaLlamada { get; set; }

    [Display(Name = "Estatus de llamada*")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [Range(1, int.MaxValue, ErrorMessage = "{0} inválido")]
    [DefaultValue(0)]
    public int IdEstatusLlamada { get; set; }

    [Display(Name = "Operador")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [Range(1, int.MaxValue, ErrorMessage = "Operador inválido")]
    [DefaultValue(0)]
    public int IdOperador { get; set; }
}
