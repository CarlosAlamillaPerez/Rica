using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class TiposPreguntum
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<PreguntasEncuestum> PreguntasEncuesta { get; set; } = new List<PreguntasEncuestum>();
}
