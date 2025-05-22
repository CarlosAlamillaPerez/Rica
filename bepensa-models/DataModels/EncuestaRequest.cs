namespace bepensa_models.DataModels;

public class EncuestaRequest
{
    public int IdBitacoraEncuesta { get; set; }

    public List<PreguntaRequest> Preguntas { get; set; } = [];
}

public class PreguntaRequest
{
    public int IdPregunta { get; set; }

    public List<RespuestaRequest> Opciones { get; set; } = [];
}

public class RespuestaRequest
{
    public int IdOpcion { get; set; }

    public string? Texto { get; set; } = null;
}