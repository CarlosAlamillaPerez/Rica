namespace bepensa_models.DTO;

public class MetaCompraDTO
{
    public long Id { get; set; }

    public int IdPeriodo { get; set; }

    public DateOnly Fecha { get; set; }

    public decimal Meta { get; set; }

    public decimal ImporteComprado { get; set; }

    public decimal CompraPreventa { get; set; }

    public decimal CompraDigital { get; set; }

    public int Porcentaje { get; set; }

    public List<ResumenVentaDTO>? Ventas { get; set; }
}
