using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class RespuestasEncuestum
{
    public long Id { get; set; }

    public long IdBitacoraEncuesta { get; set; }

    public int IdOrigen { get; set; }

    public int IdPregunta { get; set; }

    public int? IdOpcionPregunta { get; set; }

    public string? Texto { get; set; }

    public int? BitRespuesta { get; set; }

    public DateTime FechaReg { get; set; }

    public virtual BitacoraDeEncuestum IdBitacoraEncuestaNavigation { get; set; } = null!;

    public virtual OpcionesPreguntum? IdOpcionPreguntaNavigation { get; set; }

    public virtual Origene IdOrigenNavigation { get; set; } = null!;

    public virtual PreguntasEncuestum IdPreguntaNavigation { get; set; } = null!;
}
