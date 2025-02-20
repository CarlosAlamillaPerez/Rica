using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class Zona
{
    public int Id { get; set; }

    public int IdEmbotelladora { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Cedi> Cedis { get; set; } = new List<Cedi>();

    public virtual Embotelladora IdEmbotelladoraNavigation { get; set; } = null!;
}
