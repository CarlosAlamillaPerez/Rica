using System.ComponentModel.DataAnnotations;

namespace bepensa_socio_selecto_models.DataModels;

public class BuscarRequest
{
    [Display(Name = "Buscar")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public string Buscar { get; set; } = null!;
}
