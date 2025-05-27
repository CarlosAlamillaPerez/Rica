using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class RespuestaEsperadum
{
    public int Id { get; set; }

    public int IdPregunta { get; set; }

    public int? IdOpcionPregunta { get; set; }

    public string? Texto { get; set; }

    public int? BitRespuesta { get; set; }

    public int? Valor { get; set; }

    public int? Orden { get; set; }

    public DateTime FechaReg { get; set; }

    public int IdOperadorReg { get; set; }

    public DateTime? FechaMod { get; set; }

    public int? IdOperadorMod { get; set; }

    public virtual OpcionesPreguntum? IdOpcionPreguntaNavigation { get; set; }

    public virtual Operadore? IdOperadorModNavigation { get; set; }

    public virtual Operadore IdOperadorRegNavigation { get; set; } = null!;

    public virtual PreguntasEncuestum IdPreguntaNavigation { get; set; } = null!;
}
