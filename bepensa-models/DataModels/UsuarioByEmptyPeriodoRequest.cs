using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace bepensa_models.DataModels;

public class UsuarioByEmptyPeriodoRequest
{
    [Display(Name = "IdUsuario")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [Range(1, int.MaxValue, ErrorMessage = "Usuario inválido.")]
    [DefaultValue(0)]
    public int IdUsuario { get; set; }

    [Display(Name = "IdPeriodo")]
    [DefaultValue(null)]
    public int? IdPeriodo { get; set; } = null;
}
