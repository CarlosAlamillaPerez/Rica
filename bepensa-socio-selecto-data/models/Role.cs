using System;
using System.Collections.Generic;

namespace bepensa_socio_selecto_data.models;

public partial class Role
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Operadore> Operadores { get; set; } = new List<Operadore>();

    public virtual ICollection<SeccionesPorRol> SeccionesPorRols { get; set; } = new List<SeccionesPorRol>();
}
