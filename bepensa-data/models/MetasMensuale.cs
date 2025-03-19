using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class MetasMensuale
{
    public long Id { get; set; }

    public int IdUsuario { get; set; }

    public int IdPeriodo { get; set; }

    public decimal Meta { get; set; }

    public decimal ImporteComprado { get; set; }

    public decimal CompraPreventa { get; set; }

    public decimal CompraDigital { get; set; }

    public DateTime FechaReg { get; set; }

    public int IdOperadorReg { get; set; }

    public DateTime? FechaMod { get; set; }

    public int? IdOperadorMod { get; set; }

    public virtual Operadore? IdOperadorModNavigation { get; set; }

    public virtual Operadore IdOperadorRegNavigation { get; set; } = null!;

    public virtual Periodo IdPeriodoNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
