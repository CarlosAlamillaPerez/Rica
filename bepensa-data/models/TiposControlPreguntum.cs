using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class TiposControlPreguntum
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<OpcionesPreguntum> OpcionesPregunta { get; set; } = new List<OpcionesPreguntum>();
}
