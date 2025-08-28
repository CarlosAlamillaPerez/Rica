using System;
using System.Collections.Generic;

namespace bepensa_data.modelsRD;

public partial class Zona
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Cedi> Cedis { get; set; } = new List<Cedi>();
}
