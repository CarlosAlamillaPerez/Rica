using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class FuerzaVentum
{
    public int Id { get; set; }

    public int IdCanal { get; set; }

    public int IdRolFdv { get; set; }

    public string Usuario { get; set; } = null!;

    public byte[]? Password { get; set; }

    public string? SesionId { get; set; }

    public int? IdBusqueda { get; set; }

    public int IdEstatus { get; set; }

    public DateTime FechaReg { get; set; }

    public int IdOperadorReg { get; set; }

    public DateTime? FechaMod { get; set; }

    public int? IdOperadorMod { get; set; }

    public virtual ICollection<BitacoraFuerzaVentum> BitacoraFuerzaVenta { get; set; } = new List<BitacoraFuerzaVentum>();

    public virtual Canale IdCanalNavigation { get; set; } = null!;

    public virtual Estatus IdEstatusNavigation { get; set; } = null!;

    public virtual Operadore? IdOperadorModNavigation { get; set; }

    public virtual Operadore IdOperadorRegNavigation { get; set; } = null!;

    public virtual RolesFdv IdRolFdvNavigation { get; set; } = null!;

    public virtual ICollection<JefesVentum> JefesVenta { get; set; } = new List<JefesVentum>();
}
