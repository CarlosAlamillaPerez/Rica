using System;
using System.Collections.Generic;

namespace bepensa_socio_selecto_data.models;

public partial class Cedi
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int? CodigoCedi { get; set; }

    public int RegistrosMax { get; set; }

    public int? IdZona { get; set; }

    public virtual Zona? IdZonaNavigation { get; set; }

    public virtual ICollection<Negocio> Negocios { get; set; } = new List<Negocio>();
}
