using System;
using System.Collections.Generic;

namespace bepensa_data.modelsRD;

public partial class Municipio
{
    public int Id { get; set; }

    public int IdEstado { get; set; }

    public string Municipio1 { get; set; } = null!;

    public DateTime Fechareg { get; set; }
}
