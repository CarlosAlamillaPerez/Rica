using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class JefesVentum
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int? BitCedi { get; set; }
}
