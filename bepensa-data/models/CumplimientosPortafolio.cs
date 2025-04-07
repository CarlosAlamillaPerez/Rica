using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class CumplimientosPortafolio
{
    public long Id { get; set; }

    public int IdUsuario { get; set; }

    public int IdEmpaque { get; set; }

    public bool Cumple { get; set; }

    public DateTime FechaReg { get; set; }

    public DateTime? FechaMod { get; set; }

    public virtual Empaque IdEmpaqueNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
