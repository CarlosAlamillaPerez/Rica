using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class Supervisore
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
