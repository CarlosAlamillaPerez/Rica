using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class PreguntasEncuestum
{
    public int Id { get; set; }

    public int? IdPadre { get; set; }

    public int IdEncuesta { get; set; }

    public int IdTipoPregunta { get; set; }

    public int NumeroPregunta { get; set; }

    public string Texto { get; set; } = null!;

    public string? TextoAlternativo { get; set; }

    public int Valor { get; set; }

    public bool Obligatoria { get; set; }

    public string? MensajeObligatoria { get; set; }

    public int? LimiteRespuestas { get; set; }

    public string? MensajeLimite { get; set; }

    public int RespuestasRequeridas { get; set; }

    public string? MsjRspRequeridas { get; set; }

    public string? Codigo { get; set; }

    public int IdEstatus { get; set; }

    public DateTime FechaReg { get; set; }

    public int IdOperadorReg { get; set; }

    public DateTime? FechaMod { get; set; }

    public int? IdOperadorMod { get; set; }

    public virtual Encuesta IdEncuestaNavigation { get; set; } = null!;

    public virtual Estatus IdEstatusNavigation { get; set; } = null!;

    public virtual Operadore? IdOperadorModNavigation { get; set; }

    public virtual Operadore IdOperadorRegNavigation { get; set; } = null!;

    public virtual PreguntasEncuestum? IdPadreNavigation { get; set; }

    public virtual TiposPreguntum IdTipoPreguntaNavigation { get; set; } = null!;

    public virtual ICollection<PreguntasEncuestum> InverseIdPadreNavigation { get; set; } = new List<PreguntasEncuestum>();

    public virtual ICollection<OpcionesPreguntum> OpcionesPreguntumIdPreguntaNavigations { get; set; } = new List<OpcionesPreguntum>();

    public virtual ICollection<OpcionesPreguntum> OpcionesPreguntumIdSkipPreguntaEncuestaNavigations { get; set; } = new List<OpcionesPreguntum>();

    public virtual ICollection<RespuestaEsperadum> RespuestaEsperada { get; set; } = new List<RespuestaEsperadum>();

    public virtual ICollection<RespuestasEncuestum> RespuestasEncuesta { get; set; } = new List<RespuestasEncuestum>();
}
