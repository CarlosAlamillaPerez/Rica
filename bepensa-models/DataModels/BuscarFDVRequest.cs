using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace bepensa_models.DataModels;

public class BuscarFDVRequest
{
    [Display(Name = "Fuerza de venta")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [DefaultValue(0)]
    [Range(0, int.MaxValue, ErrorMessage = "Fuerza de venta inválido.")]
    public int IdFDV { get; set; }

    [Display(Name = "Buscar")]
    public string? Buscar { get; set; } = null;
}
