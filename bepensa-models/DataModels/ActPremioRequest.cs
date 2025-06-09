using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace bepensa_models.DataModels;

public class ActPremioRequest
{
    [Display(Name = "IdUsuario")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [Range(1, int.MaxValue, ErrorMessage = "Usuario inválido.")]
    public int IdUsuario { get; set; }

    [Display(Name = "IdPremio")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [Range(1, int.MaxValue, ErrorMessage = "Premio inválido.")]
    public int IdPremio { get; set; }

    [Display(Name = "Aumentar Cantidad")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public bool AumentarCantidad { get; set; } = true;

    [JsonIgnore]
    [Display(Name = "Operador")]
    [Range(1, int.MaxValue, ErrorMessage = "Operador inválido.")]
    [DefaultValue(null)]
    public int? IdOperador { get; set; } = null;
}
