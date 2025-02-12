using System;
using System.Collections.Generic;

namespace bepensa_socio_selecto_data.models;

public partial class MetodosDeEntrega
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public bool Digital { get; set; }

    public virtual ICollection<Premio> Premios { get; set; } = new List<Premio>();
}
