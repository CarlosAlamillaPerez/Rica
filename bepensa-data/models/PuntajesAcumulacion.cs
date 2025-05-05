using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class PuntajesAcumulacion
{
    public long Id { get; set; }

    public int IdSda { get; set; }

    public int Puntos { get; set; }

    public DateTime? FechaReg { get; set; }

    public int IdOperadorReg { get; set; }

    public DateTime? FechaMod { get; set; }

    public int? IdOperadorMod { get; set; }

    public virtual Operadore? IdOperadorModNavigation { get; set; }

    public virtual Operadore IdOperadorRegNavigation { get; set; } = null!;

    public virtual SubconceptosDeAcumulacion IdSdaNavigation { get; set; } = null!;
}
