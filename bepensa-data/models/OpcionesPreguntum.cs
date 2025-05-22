using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class OpcionesPreguntum
{
    public int Id { get; set; }

    public int IdPregunta { get; set; }

    public int IdTipoControl { get; set; }

    public string? Texto { get; set; }

    public int Valor { get; set; }

    public int? IdSkipPreguntaEncuesta { get; set; }

    public virtual PreguntasEncuestum IdPreguntaNavigation { get; set; } = null!;

    public virtual PreguntasEncuestum? IdSkipPreguntaEncuestaNavigation { get; set; }

    public virtual TiposControlPreguntum IdTipoControlNavigation { get; set; } = null!;

    public virtual ICollection<RespuestasEncuestum> RespuestasEncuesta { get; set; } = new List<RespuestasEncuestum>();
}
