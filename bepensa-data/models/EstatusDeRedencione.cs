using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class EstatusDeRedencione
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? PrefijoRms { get; set; }

    public virtual ICollection<SeguimientoDeRedencione> SeguimientoDeRedenciones { get; set; } = new List<SeguimientoDeRedencione>();
}
