using System;
using System.Collections.Generic;

namespace bepensa_data.modelsRD;

public partial class Supervisore
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Negocio> Negocios { get; set; } = new List<Negocio>();
}
