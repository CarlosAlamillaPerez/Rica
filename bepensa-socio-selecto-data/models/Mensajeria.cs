using System;
using System.Collections.Generic;

namespace bepensa_socio_selecto_data.models;

public partial class Mensajeria
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Redencione> Redenciones { get; set; } = new List<Redencione>();
}
