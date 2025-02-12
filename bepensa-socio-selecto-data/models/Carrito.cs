using System;
using System.Collections.Generic;

namespace bepensa_socio_selecto_data.models;

public partial class Carrito
{
    public long Id { get; set; }

    public long IdUsuario { get; set; }

    public int IdPremio { get; set; }

    public int Cantidad { get; set; }

    public int IdEstatusCarrito { get; set; }

    public DateTime FechaReg { get; set; }

    public DateTime? FechaRedencion { get; set; }

    public int Puntos { get; set; }

    public string? TelefonoRecarga { get; set; }

    public int IdOrigen { get; set; }

    public virtual EstatusDeCarrito IdEstatusCarritoNavigation { get; set; } = null!;

    public virtual Origene IdOrigenNavigation { get; set; } = null!;

    public virtual Premio IdPremioNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
