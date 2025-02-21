using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class EstatusDeLlamadum
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Llamada> Llamada { get; set; } = new List<Llamada>();
}
