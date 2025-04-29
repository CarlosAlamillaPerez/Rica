namespace bepensa_models.ApiResponse;

public class DisponibilidadMKT
{
    public int Success { get; set; }

    public string Mensaje { get; set; } = null!;

    public List<SkuDisponibilidadMKT>? Resultado { get; set; }
}
