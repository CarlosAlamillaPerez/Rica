using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class CuotasDeCompra
{
    public long Id { get; set; }

    public long IdNegocio { get; set; }

    public int IdPeriodo { get; set; }

    public int IdCategoriaDeProducto { get; set; }

    public decimal Cuota { get; set; }

    public DateTime? FechaReg { get; set; }

    public int IdOperadorReg { get; set; }

    public virtual Negocio IdNegocioNavigation { get; set; } = null!;

    public virtual Operadore IdOperadorRegNavigation { get; set; } = null!;

    public virtual Periodo IdPeriodoNavigation { get; set; } = null!;
}
