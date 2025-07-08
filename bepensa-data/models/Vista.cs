using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class Vista
{
    public int Id { get; set; }

    public string Codigo { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string? NombreAlternativo { get; set; }

    public bool Activo { get; set; }

    public bool RequiereFechaFin { get; set; }

    public virtual ICollection<SeguimientoVista> SeguimientoVista { get; set; } = new List<SeguimientoVista>();
}
