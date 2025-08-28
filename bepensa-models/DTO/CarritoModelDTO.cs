namespace bepensa_models.DTO;

public class CarritoModelDTO
{
    public long Id { get; set; }

    public int IdUsuario { get; set; }

    public int IdPremio { get; set; }

    public int IdTipoDeEnvio { get; set; }

    public int IdTipoDePremio { get; set; }

    public int IdTipoTransaccion { get; set; }

    public string Sku { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string? Imagen { get; set; } = null;

    public int Cantidad { get; set; }

    public int IdEstatusCarrito { get; set; }

    public DateTime FechaReg { get; set; }

    public DateTime? FechaRedencion { get; set; }

    public int Puntos { get; set; }

    public string? TelefonoRecarga { get; set; }

    public int IdOrigen { get; set; }

    public int? IdTarjeta { get; set; }

    public string? NoTarjeta { get; set; } = null;

    public Guid? IdTransaccionLog { get; set; }
}
