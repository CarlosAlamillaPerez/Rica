using System.ComponentModel.DataAnnotations;

namespace bepensa_models.DataModels;

public class BuscarRequest
{
    [Display(Name = "Buscar")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public string Buscar { get; set; } = null!;
}
