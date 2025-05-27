using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class BitacoraDeEncuestum
{
    public long Id { get; set; }

    public int IdEncuesta { get; set; }

    public int? IdUsuario { get; set; }

    public int? IdOperador { get; set; }

    public Guid Token { get; set; }

    public string Url { get; set; } = null!;

    public int NoIngresos { get; set; }

    public DateTime? FechaIngreso { get; set; }

    public int NoContestaciones { get; set; }

    public DateTime? FechaInicioRespuesta { get; set; }

    public DateTime? FechaFinRespuesta { get; set; }

    public DateTime? FechaRespuestaEsperada { get; set; }

    public bool Contestada { get; set; }

    public int IdEstatus { get; set; }

    public DateTime FechaReg { get; set; }

    public int IdOperadorReg { get; set; }

    public DateTime? FechaMod { get; set; }

    public int? IdOperadorMod { get; set; }

    public virtual Encuesta IdEncuestaNavigation { get; set; } = null!;

    public virtual Estatus IdEstatusNavigation { get; set; } = null!;

    public virtual Operadore? IdOperadorModNavigation { get; set; }

    public virtual Operadore? IdOperadorNavigation { get; set; }

    public virtual Operadore IdOperadorRegNavigation { get; set; } = null!;

    public virtual Usuario? IdUsuarioNavigation { get; set; }

    public virtual ICollection<RespuestasEncuestum> RespuestasEncuesta { get; set; } = new List<RespuestasEncuestum>();
}
