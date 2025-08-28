using System;
using System.Collections.Generic;

namespace bepensa_data.modelsRD;

public partial class Colonia
{
    public int Id { get; set; }

    public int IdMunicipio { get; set; }

    public string Colonia1 { get; set; } = null!;

    public string? Cp { get; set; }

    public string? Ciudad { get; set; }

    public DateTime Fechareg { get; set; }
}
