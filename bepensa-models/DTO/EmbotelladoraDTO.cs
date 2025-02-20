using System.ComponentModel.DataAnnotations;

namespace bepensa_models.DTO;

public class EmbotelladoraDTO
{
    public int Id { get; set; }

    [Display(Name = "Embotelladora")]
    public string Nombre { get; set; } = null!;
}
