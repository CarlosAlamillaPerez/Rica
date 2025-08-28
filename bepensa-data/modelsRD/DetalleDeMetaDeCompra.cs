using System;
using System.Collections.Generic;

namespace bepensa_data.modelsRD;

public partial class DetalleDeMetaDeCompra
{
    public long IdNegocio { get; set; }

    public int IdPeriodo { get; set; }

    public string Nombre { get; set; } = null!;

    public decimal Cuota { get; set; }

    public decimal? Compra { get; set; }

    public int? Avance { get; set; }
}
