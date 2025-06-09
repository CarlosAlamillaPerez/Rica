namespace bepensa_models.DTO;

public class OpcionPreguntaDTO
{
    public int Id { get; set; }

    public int IdTipoControl { get; set; }

    public string TipoControl { get; set; } = null!;

    public string? Texto { get; set; }

    public int Valor { get; set; }

    public int? IdSkipPreguntaEncuesta { get; set; }
}
