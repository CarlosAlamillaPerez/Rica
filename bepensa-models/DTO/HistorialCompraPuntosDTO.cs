namespace bepensa_models.DTO;

public class HistorialCompraPuntosDTO
{
    public int Id { get; set; }

    public int IdTipoPago { get; set; }

    public int PuntosTotales { get; set; }

    public int PuntosFaltantes { get; set; }

    public decimal Monto { get; set; }

    public DateTime FechaReg { get; set; }

    public string? Referencia { get; set; }

    public Guid? IdTransaccionLog { get; set; }

    public int IdEstatusPago { get; set; }
}
