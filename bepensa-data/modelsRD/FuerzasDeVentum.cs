using System;
using System.Collections.Generic;

namespace bepensa_data.modelsRD;

public partial class FuerzasDeVentum
{
    public long Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Usuario { get; set; } = null!;

    public string? Email { get; set; }

    public byte[]? Password { get; set; }

    public string? Sesion { get; set; }

    public bool Bloqueado { get; set; }

    public int IdEstatus { get; set; }

    public DateTime FechaReg { get; set; }

    public int IdOperadorReg { get; set; }

    public DateTime? FechaMod { get; set; }

    public int? IdOperadorMod { get; set; }

    public string? TokenMovil { get; set; }

    public virtual ICollection<BitacoraDeFuerzasDeVentum> BitacoraDeFuerzasDeVenta { get; set; } = new List<BitacoraDeFuerzasDeVentum>();

    public virtual ICollection<FuerzasDeVentaNegocio> FuerzasDeVentaNegocios { get; set; } = new List<FuerzasDeVentaNegocio>();

    public virtual Estatus IdEstatusNavigation { get; set; } = null!;

    public virtual Operadore? IdOperadorModNavigation { get; set; }

    public virtual Operadore IdOperadorRegNavigation { get; set; } = null!;
}
