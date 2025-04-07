using bepensa_data.models;

namespace bepensa_models.DTO;

public class PortafolioPrioritarioDTO
{
    public int Id { get; set; }

    public int IdConceptoDeAcumulacion { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual List<CumplimientoPortafolioDTO> CumplimientoPortafolio { get; set; } = [];

    public int Porcentaje { get; set; }
}
