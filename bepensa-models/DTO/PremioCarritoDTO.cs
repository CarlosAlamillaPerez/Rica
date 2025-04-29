namespace bepensa_models.DTO;

public class PremioCarritoDTO
{
    public int IdPremio { get; set; }

    public string Sku { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string? Imagen { get; set; } = null!;

    public string? TelefonoRecarga { get; set; }

    public int Cantidad { get; set; }

    public int Puntos { get; set; }
}
