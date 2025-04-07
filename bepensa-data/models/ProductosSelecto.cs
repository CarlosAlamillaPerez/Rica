using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class ProductosSelecto
{
    public int Id { get; set; }

    public int IdSubconceptoDeAcumulacion { get; set; }

    public int IdProducto { get; set; }
}
