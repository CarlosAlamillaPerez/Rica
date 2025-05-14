using Microsoft.EntityFrameworkCore;

namespace bepensa_data.StoredProcedures.Models;

[Keyless]
public class MetaMensualCTE
{
    public long Id { get; set; }

    public int IdPeriodo { get; set; }

    public DateOnly Fecha { get; set; }

    public decimal Meta { get; set; }

    public decimal ImporteComprado { get; set; }

    public decimal CompraPreventa { get; set; }

    public decimal CompraDigital { get; set; }

    public int Porcentaje { get; set; }


    public DateOnly? VentasFechaVenta { get; set; }

    public decimal? VentasImporteComprado { get; set; }
}
