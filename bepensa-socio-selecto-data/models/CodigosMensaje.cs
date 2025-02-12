using System;
using System.Collections.Generic;

namespace bepensa_socio_selecto_data.models;

public partial class CodigosMensaje
{
    public int Id { get; set; }

    public string Mensaje { get; set; } = null!;
}
