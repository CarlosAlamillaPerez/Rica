using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class Estado
{
    public int Id { get; set; }

    public string Estado1 { get; set; } = null!;

    public DateTime Fechareg { get; set; }

    public virtual ICollection<Municipio> Municipios { get; set; } = new List<Municipio>();
}
