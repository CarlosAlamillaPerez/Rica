using bepensa_models.DataModels;

namespace bepensa_models.DTO;

public class EncuestaDTO
{
    public int Id { get; set; }

    public string Codigo { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string? Url { get; set; }

    public List<PreguntaEncuestaDTO> Preguntas { get; set; } = [];
}
