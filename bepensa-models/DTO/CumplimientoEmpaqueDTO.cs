namespace bepensa_models.DTO;

public class CumplimientoEmpaqueDTO
{
    public string? Imagen { get; set; }

    public string Nombre { get; set; } = null!;

    public bool Cumple { get; set; } = false;
}
