using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class HistoricoVenta
{
    public int Id { get; set; }

    public int? IdArchivoDeCarga { get; set; }

    public int IdPeriodo { get; set; }

    public int IdUsuario { get; set; }

    public double Cf { get; set; }

    public double Cu { get; set; }

    public decimal? Importe { get; set; }

    public DateTime FechaReg { get; set; }

    public int IdOperadorReg { get; set; }

    public DateTime? FechaMod { get; set; }

    public int? IdOperadorMod { get; set; }

    public virtual ArchivosDeCarga? IdArchivoDeCargaNavigation { get; set; }

    public virtual Operadore? IdOperadorModNavigation { get; set; }

    public virtual Operadore IdOperadorRegNavigation { get; set; } = null!;

    public virtual Periodo IdPeriodoNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
