namespace bepensa_models.DTO;

public class MetaMensualDTO
{
    public long Id { get; set; }

    public DateOnly Fecha { get; set; }

    public decimal Meta { get; set; }

    public decimal ImporteComprado { get; set; }

    public decimal ImportePorComprar { get; set; }

    public decimal CompraPreventa { get; set; }

    public decimal CompraDigital { get; set; }

    public int Porcentaje { get; set; }
}
