using System;
using System.Collections.Generic;

namespace bepensa_data.modelsRD;

public partial class SeccionesPorRol
{
    public int Id { get; set; }

    public int Idrol { get; set; }

    public int Idseccion { get; set; }

    public virtual Role IdrolNavigation { get; set; } = null!;

    public virtual Seccione IdseccionNavigation { get; set; } = null!;
}
