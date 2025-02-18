using System;
using System.Collections.Generic;

namespace bepensa_socio_selecto_data.models;

public partial class Seccione
{
    public int Id { get; set; }

    public int? IdPadre { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Vista { get; set; }

    public string? Controlador { get; set; }

    public string? Area { get; set; }

    public int IdEstatus { get; set; }

    public string? Icon { get; set; }

    public int? Orden { get; set; }

    public virtual Estatus IdEstatusNavigation { get; set; } = null!;

    public virtual Seccione? IdPadreNavigation { get; set; }

    public virtual ICollection<Seccione> InverseIdPadreNavigation { get; set; } = new List<Seccione>();

    public virtual ICollection<Reporte> Reportes { get; set; } = new List<Reporte>();

    public virtual ICollection<SeccionesPorRol> SeccionesPorRols { get; set; } = new List<SeccionesPorRol>();
}
