using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class EvaluacionesAcumulacion
{
    public long Id { get; set; }

    public int IdSubcptoAcumulacon { get; set; }

    public int IdPeriodo { get; set; }

    public int IdUsuario { get; set; }

    public int Resultado { get; set; }

    public DateTime FechaReg { get; set; }

    public int IdOperadorReg { get; set; }

    public DateTime? FechaMod { get; set; }

    public int? IdOperadorMod { get; set; }

    public virtual Operadore? IdOperadorModNavigation { get; set; }

    public virtual Operadore IdOperadorRegNavigation { get; set; } = null!;

    public virtual Periodo IdPeriodoNavigation { get; set; } = null!;

    public virtual SubconceptosDeAcumulacion IdSubcptoAcumulaconNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
