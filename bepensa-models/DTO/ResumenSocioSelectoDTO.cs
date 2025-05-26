namespace bepensa_models.DTO;

public class ResumenSocioSelectoDTO
{
    public MetaMensualDTO? MetaMensual { get; set; }

    public List<PortafolioPrioritarioDTO>? PortafolioPrioritario { get; set; }

    public ConceptosEdoCtaDTO EstadoCuenta { get; set; } = null!;
}
