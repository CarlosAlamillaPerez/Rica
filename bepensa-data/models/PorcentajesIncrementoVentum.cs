using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class PorcentajesIncrementoVentum
{
    public int Id { get; set; }

    public int IdCanal { get; set; }

    public int IdZona { get; set; }

    public int IdPeriodo { get; set; }

    public double Porcentaje { get; set; }

    public virtual Canale IdCanalNavigation { get; set; } = null!;

    public virtual Periodo IdPeriodoNavigation { get; set; } = null!;

    public virtual Zona IdZonaNavigation { get; set; } = null!;
}
