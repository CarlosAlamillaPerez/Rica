using System.ComponentModel.DataAnnotations;

namespace bepensa_models.DTO;

public class ZonaDTO
{
    public int Id { get; set; }

    public int IdEmbotelladora { get; set; }

    public string Nombre { get; set; } = null!;

    public EmbotelladoraDTO Embotelladora { get; set; } = null!;
}
