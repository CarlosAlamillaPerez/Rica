using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace bepensa_models.DataModels;

public class UsuarioPeriodoRequest
{
    [Display(Name = "IdUsuario")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [Range(1, int.MaxValue, ErrorMessage = "Usuario inválido.")]
    [DefaultValue(0)]
    public int IdUsuario { get; set; }

    [Display(Name = "IdPeriodo")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [Range(1, int.MaxValue, ErrorMessage = "Periodo inválido.")]
    [DefaultValue(0)]
    public int IdPeriodo { get; set; }
}
