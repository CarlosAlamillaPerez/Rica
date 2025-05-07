using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class Tamanio
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int? IdPadre { get; set; }

    public virtual Tamanio? IdPadreNavigation { get; set; }

    public virtual ICollection<Tamanio> InverseIdPadreNavigation { get; set; } = new List<Tamanio>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
