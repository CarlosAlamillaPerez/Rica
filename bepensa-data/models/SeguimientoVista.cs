using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class SeguimientoVista
{
    public long Id { get; set; }

    public int IdUsuario { get; set; }

    public int IdVista { get; set; }

    public DateTime FechaReg { get; set; }

    public int? IdFdvaftd { get; set; }

    public Guid? IdTransaccionLog { get; set; }

    public virtual FuerzaVentum? IdFdvaftdNavigation { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

    public virtual Vista IdVistaNavigation { get; set; } = null!;
}
