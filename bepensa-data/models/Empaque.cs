using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class Empaque
{
    public int Id { get; set; }

    public int IdPeriodo { get; set; }

    public int IdSda { get; set; }

    public string Nombre { get; set; } = null!;

    public int IdEstatus { get; set; }

    public DateTime FechaReg { get; set; }

    public int IdOperadorReg { get; set; }

    public DateTime? FechaMod { get; set; }

    public int? IdOperadorMod { get; set; }

    public virtual ICollection<CumplimientosPortafolio> CumplimientosPortafolios { get; set; } = new List<CumplimientosPortafolio>();

    public virtual ICollection<EmpaquesProducto> EmpaquesProductos { get; set; } = new List<EmpaquesProducto>();

    public virtual Estatus IdEstatusNavigation { get; set; } = null!;

    public virtual Operadore? IdOperadorModNavigation { get; set; }

    public virtual Operadore IdOperadorRegNavigation { get; set; } = null!;

    public virtual Periodo IdPeriodoNavigation { get; set; } = null!;

    public virtual SegmentosAcumulacion IdSdaNavigation { get; set; } = null!;
}
