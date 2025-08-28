using System;
using System.Collections.Generic;

namespace bepensa_data.modelsRD;

public partial class SeguimientoDeRedencione
{
    public long Id { get; set; }

    public long IdRedencion { get; set; }

    public int IdEstatusDeRedencion { get; set; }

    public DateTime Fecha { get; set; }

    public virtual EstatusDeRedencione IdEstatusDeRedencionNavigation { get; set; } = null!;

    public virtual Redencione IdRedencionNavigation { get; set; } = null!;
}
