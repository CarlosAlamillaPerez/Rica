using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class PortafolioAvance
{
    public int IdUsuario { get; set; }

    public int IdPeriodo { get; set; }

    public string Subconceptodeacumulacion { get; set; } = null!;

    public int? Total { get; set; }

    public int? Totalcumple { get; set; }

    public int? Porcentaje { get; set; }
}
