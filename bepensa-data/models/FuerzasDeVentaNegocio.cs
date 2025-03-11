using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class FuerzasDeVentaNegocio
{
    public long Id { get; set; }

    public long IdNegocio { get; set; }

    public long IdFuerzaDeVenta { get; set; }

    public DateTime FechaReg { get; set; }

    public int IdOperadorReg { get; set; }

    public virtual FuerzasDeVentum IdFuerzaDeVentaNavigation { get; set; } = null!;

    public virtual Negocio IdNegocioNavigation { get; set; } = null!;

    public virtual Operadore IdOperadorRegNavigation { get; set; } = null!;
}
