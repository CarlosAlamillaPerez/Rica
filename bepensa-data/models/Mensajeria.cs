using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class Mensajeria
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Clave { get; set; }

    public virtual ICollection<Redencione> Redenciones { get; set; } = new List<Redencione>();
}
