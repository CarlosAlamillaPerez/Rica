using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class TiposDeAccione
{
    public int Id { get; set; }

    public string Accion { get; set; } = null!;

    public int IdEstatus { get; set; }
}
