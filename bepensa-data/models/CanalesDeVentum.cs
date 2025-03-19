using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class CanalesDeVentum
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<SubcanalesDeVentum> SubcanalesDeVenta { get; set; } = new List<SubcanalesDeVentum>();
}
