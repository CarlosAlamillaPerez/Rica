using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class PuntajesDeSubconceptosDeAcumulacion
{
    public long Id { get; set; }

    public int IdSubconceptoDeAcumulacion { get; set; }

    public int IdPrograma { get; set; }

    public int Puntos { get; set; }

    public DateTime? FechaReg { get; set; }

    public int IdOperadorReg { get; set; }

    public DateTime? FechaMod { get; set; }

    public int? IdOperadorMod { get; set; }

    public virtual Operadore? IdOperadorModNavigation { get; set; }

    public virtual Operadore IdOperadorRegNavigation { get; set; } = null!;

    public virtual Programa IdProgramaNavigation { get; set; } = null!;

    public virtual SubconceptosDeAcumulacion IdSubconceptoDeAcumulacionNavigation { get; set; } = null!;
}
