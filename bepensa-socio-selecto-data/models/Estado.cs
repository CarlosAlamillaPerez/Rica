using System;
using System.Collections.Generic;

namespace bepensa_socio_selecto_data.models;

public partial class Estado
{
    public long Id { get; set; }

    public string Estado1 { get; set; } = null!;

    public DateTime Fechareg { get; set; }
}
