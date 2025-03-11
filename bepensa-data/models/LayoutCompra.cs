using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class LayoutCompra
{
    public long Cuc { get; set; }

    public string Sku { get; set; } = null!;

    public decimal Unidad { get; set; }

    public DateOnly Fecha { get; set; }

    public decimal Importe { get; set; }
}
