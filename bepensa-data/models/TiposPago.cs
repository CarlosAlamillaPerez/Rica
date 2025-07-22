using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class TiposPago
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<HistorialCompraPunto> HistorialCompraPuntos { get; set; } = new List<HistorialCompraPunto>();
}
