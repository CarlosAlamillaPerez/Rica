using System;
using System.Collections.Generic;

namespace bepensa_data.modelsRD;

public partial class DetallesDePortafolio
{
    public long IdNegocio { get; set; }

    public int IdPeriodo { get; set; }

    public int IdSubconceptoDePortafolio { get; set; }

    public string NombrePortafolio { get; set; } = null!;

    public int IdProducto { get; set; }

    public string Sku { get; set; } = null!;

    public string NombreProducto { get; set; } = null!;

    public int Comprado { get; set; }

    public decimal? Importe { get; set; }
}
