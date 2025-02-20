using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class Embotelladora
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Negocio> Negocios { get; set; } = new List<Negocio>();

    public virtual ICollection<Zona> Zonas { get; set; } = new List<Zona>();
}
