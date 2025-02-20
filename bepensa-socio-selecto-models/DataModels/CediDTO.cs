using bepensa_models.DTO;
using System.ComponentModel.DataAnnotations;

namespace bepensa_models.DataModels;

public class CediDTO
{
    public int Id { get; set; }

    [Display(Name = "CEDI")]
    public string Nombre { get; set; } = null!;

    public string CodigoCedi { get; set; } = null!;

    public ZonaDTO Zona { get; set; } = null!;
}
