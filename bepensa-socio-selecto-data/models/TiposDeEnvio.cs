using System;
using System.Collections.Generic;

namespace bepensa_socio_selecto_data.models;

public partial class TiposDeEnvio
{
    public int Id { get; set; }

    public string Envio { get; set; } = null!;

    public int IdEstatus { get; set; }

    public virtual Estatus IdEstatusNavigation { get; set; } = null!;

    public virtual ICollection<Premio> Premios { get; set; } = new List<Premio>();
}
