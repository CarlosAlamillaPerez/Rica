using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class Tarjeta
{
    public int Id { get; set; }

    public int IdPremio { get; set; }

    public int IdUsuario { get; set; }

    public string Folio { get; set; } = null!;

    public string NoTarjeta { get; set; } = null!;

    public int IdEstatus { get; set; }

    public DateTime FechaReg { get; set; }

    public int IdOperadorReg { get; set; }

    public DateTime? FechaMod { get; set; }

    public int? IdOperadorMod { get; set; }

    public virtual Estatus IdEstatusNavigation { get; set; } = null!;

    public virtual Operadore? IdOperadorModNavigation { get; set; }

    public virtual Operadore IdOperadorRegNavigation { get; set; } = null!;

    public virtual Premio IdPremioNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
