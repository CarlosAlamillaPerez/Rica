namespace bepensa_models.DTO;

public class EstadoDeCuentaDTO
{
    public int SaldoAnterior { get; set; }

    public int AcumuladoActual { get; set; }

    public int PuntosDisponibles { get; set; }

    public int PuntosCanjeados { get; set; }

    public List<AcumulacionEdoCtaDTO> ConceptosAcumulacion { get; set; } = [];
}
