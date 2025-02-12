using System;
using System.Collections.Generic;

namespace bepensa_socio_selecto_data.models;

public partial class BitacoraEnvioCorreo
{
    public long Id { get; set; }

    public long? IdUsuario { get; set; }

    public string Email { get; set; } = null!;

    public DateTime FechaEnvio { get; set; }

    public string Codigo { get; set; } = null!;

    public int? MailItemId { get; set; }

    public Guid Token { get; set; }

    public int IdEstatus { get; set; }

    public int? Lecturas { get; set; }

    public DateTime? FechaLectura { get; set; }

    public int? IdOperador { get; set; }

    public virtual Estatus IdEstatusNavigation { get; set; } = null!;

    public virtual Operadore? IdOperadorNavigation { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
