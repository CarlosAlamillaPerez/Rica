using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class Municipio
{
    public int Id { get; set; }

    public int IdEstado { get; set; }

    public string Municipio1 { get; set; } = null!;

    public DateTime Fechareg { get; set; }

    public virtual ICollection<Colonia> Colonia { get; set; } = new List<Colonia>();

    public virtual Estado IdEstadoNavigation { get; set; } = null!;
}
