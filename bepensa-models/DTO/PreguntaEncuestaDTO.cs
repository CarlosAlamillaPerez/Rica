namespace bepensa_models.DTO;

public class PreguntaEncuestaDTO
{
    public int Id { get; set; }

    public int IdTipoPregunta { get; set; }

    public string TipoPregunta { get; set; } = null!;

    public int NumeroPregunta { get; set; }

    public string Texto { get; set; } = null!;

    public bool Obligatoria { get; set; }

    public string? MensajeObligatoria { get; set; }

    public int? LimiteRespuestas { get; set; }

    public string? MensajeLimite { get; set; }

    public int RespuestasRequeridas { get; set; }

    public string? MsjRspRequeridas { get; set; }

    public string? Codigo { get; set; }

    public List<OpcionPreguntaDTO> Opciones { get; set; } = [];
}
