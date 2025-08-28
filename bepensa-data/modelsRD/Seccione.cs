using System;
using System.Collections.Generic;

namespace bepensa_data.modelsRD;

public partial class Seccione
{
    public int Id { get; set; }

    public int? Idpadre { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Vista { get; set; }

    public string? Controlador { get; set; }

    public string? Area { get; set; }

    public int Idestatus { get; set; }

    public string? Icon { get; set; }

    public int? Orden { get; set; }

    public virtual Estatus IdestatusNavigation { get; set; } = null!;

    public virtual Seccione? IdpadreNavigation { get; set; }

    public virtual ICollection<Seccione> InverseIdpadreNavigation { get; set; } = new List<Seccione>();

    public virtual ICollection<Reporte> Reportes { get; set; } = new List<Reporte>();

    public virtual ICollection<SeccionesPorRol> SeccionesPorRols { get; set; } = new List<SeccionesPorRol>();

    public virtual ICollection<TiposDeOperacion> TiposDeOperacions { get; set; } = new List<TiposDeOperacion>();
}
