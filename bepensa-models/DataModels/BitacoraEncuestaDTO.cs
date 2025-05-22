using bepensa_models.DTO;

namespace bepensa_models.DataModels;

public class BitacoraEncuestaDTO
{
    public long Id { get; set; }

    public string Url { get; set; } = null!;

    public int NoIngresos { get; set; }

    public DateTime? FechaIngreso { get; set; }

    public int NoContestaciones { get; set; }

    public DateTime? FechaInicioRespuesta { get; set; }

    public DateTime? FechaFinRespuesta { get; set; }

    public DateTime? FechaRespuestaEsperada { get; set; }

    public bool Contestada { get; set; }

    public EncuestaDTO Encuesta { get; set; } = new();
}
