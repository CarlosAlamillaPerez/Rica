namespace bepensa_models.ApiResponse;

public class SkuDisponibilidadMKT
{
    public string Sku { get; set; } = null!;

    public int Disponibilidad { get; set; }

    public string? Observaciones { get; set; }
}
