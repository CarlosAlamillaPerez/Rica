using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class CodigosMensaje
{
    public int Id { get; set; }

    public string Mensaje { get; set; } = null!;
}
