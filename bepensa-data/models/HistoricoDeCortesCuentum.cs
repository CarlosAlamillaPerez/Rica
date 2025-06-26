using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class HistoricoDeCortesCuentum
{
    public int Id { get; set; }

    public int IdPeriodo { get; set; }

    public int IdUsuario { get; set; }

    public DateOnly FechaCorte { get; set; }

    public Guid? IdTransaccionLog { get; set; }

    public virtual Periodo IdPeriodoNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
