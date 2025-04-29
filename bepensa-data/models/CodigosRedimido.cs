using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class CodigosRedimido
{
    public int Id { get; set; }

    public int IdUsuario { get; set; }

    public long? IdCarrito { get; set; }

    public long? IdRedencion { get; set; }

    public DateTime FechaReg { get; set; }

    public string? TelefonoRecarga { get; set; }

    public string? Codigo { get; set; }

    public string? Pin { get; set; }

    public string? Folio { get; set; }

    public string? Motivo { get; set; }

    public Guid? IdTransaccionLog { get; set; }

    public virtual Carrito? IdCarritoNavigation { get; set; }

    public virtual Redencione? IdRedencionNavigation { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
