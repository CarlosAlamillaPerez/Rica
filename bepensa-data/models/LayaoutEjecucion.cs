using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class LayaoutEjecucion
{
    public string Cuc { get; set; } = null!;

    public string Mes { get; set; } = null!;

    public short Anio { get; set; }

    public string Indicador { get; set; } = null!;

    public bool Cumple { get; set; }
}
