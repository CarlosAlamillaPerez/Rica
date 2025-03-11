using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class BitacoraDeFuerzasDeVentum
{
    public long Id { get; set; }

    public long IdFuerzaDeVenta { get; set; }

    public int IdTipoDeOperacion { get; set; }

    public DateTime FechaReg { get; set; }

    public int? IdOperadorReg { get; set; }

    public string? Notas { get; set; }

    public virtual FuerzasDeVentum IdFuerzaDeVentaNavigation { get; set; } = null!;

    public virtual Operadore? IdOperadorRegNavigation { get; set; }

    public virtual TiposDeOperacion IdTipoDeOperacionNavigation { get; set; } = null!;
}
