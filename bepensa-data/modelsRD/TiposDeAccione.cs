using System;
using System.Collections.Generic;

namespace bepensa_data.modelsRD;

public partial class TiposDeAccione
{
    public int Id { get; set; }

    public string Accion { get; set; } = null!;

    public int IdEstatus { get; set; }
}
