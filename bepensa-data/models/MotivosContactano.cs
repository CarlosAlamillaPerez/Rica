using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class MotivosContactano
{
    public int Id { get; set; }

    public string Motivo { get; set; } = null!;

    public int IdEstatus { get; set; }

    public virtual ICollection<Contactano> Contactanos { get; set; } = new List<Contactano>();

    public virtual Estatus IdEstatusNavigation { get; set; } = null!;
}
