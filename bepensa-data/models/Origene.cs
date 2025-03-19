using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class Origene
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Carrito> Carritos { get; set; } = new List<Carrito>();

    public virtual ICollection<Redencione> Redenciones { get; set; } = new List<Redencione>();
}
