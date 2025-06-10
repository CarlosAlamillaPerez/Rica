using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace bepensa_models.DataModels;

public class RequestByIdUsuario
{
    [Display(Name = "IdUsuario")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [Range(1, int.MaxValue, ErrorMessage = "Usuario inválido.")]
    [DefaultValue(0)]
    public int IdUsuario { get; set; }

    [JsonIgnore]
    [Display(Name = "Operador")]
    [Range(1, int.MaxValue, ErrorMessage = "Operador inválido.")]
    [DefaultValue(null)]
    public int? IdOperador { get; set; } = null;
}
