namespace bepensa_models.DTO;

public class CategoriaEmpaqueDTO
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public List<CumplimientoEmpaqueDTO> Cumplimiento { get; set; } = [];

    public int Porcentaje { get; set; }
}
