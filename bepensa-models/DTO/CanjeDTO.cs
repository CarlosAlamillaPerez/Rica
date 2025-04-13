namespace bepensa_models.DTO;

public class CanjeDTO
{
    public int AcumuladoActual { get; set; }

    public int PuntosCanjeados { get; set; }

    public int CanjesRealizados { get; set; }

    public List<DetalleCanjeDTO> Canjes { get; set; } = [];
}
