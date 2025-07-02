using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace bepensa_models.DataModels;

public class SegVistaRequest
{
    [Display(Name = "IdUsuario")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [Range(1, int.MaxValue, ErrorMessage = "Usuario inválido.")]
    [DefaultValue(0)]
    public int IdUsuario { get; set; }

    [Display(Name = "IdVisita")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [Range(1, int.MaxValue, ErrorMessage = "Visita inválida.")]
    [DefaultValue(0)]
    public int IdVisita { get; set; }

    [DefaultValue(null)]
    public int? IdFDV { get; set; } = null;
}