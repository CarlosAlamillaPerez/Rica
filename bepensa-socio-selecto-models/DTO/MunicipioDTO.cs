namespace bepensa_models.DTO;

public class MunicipioDTO
{
    public int Id { get; set; }

    public int IdEstado { get; set; }

    public string Municipio { get; set; } = null!;

    public EstadoDTO Estado { get; set; } = null!;
}
