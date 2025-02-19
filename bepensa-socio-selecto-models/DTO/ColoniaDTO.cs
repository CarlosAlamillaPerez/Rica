namespace bepensa_socio_selecto_models.DTO;

public class ColoniaDTO
{
    public int Id { get; set; }

    public int IdMunicipio { get; set; }

    public string Colonia { get; set; } = null!;

    public string? CP { get; set; } = null!;

    public string? Ciudad { get; set; }

    public MunicipioDTO Municipio { get; set; } = null!;
}
