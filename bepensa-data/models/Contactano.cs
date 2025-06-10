using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class Contactano
{
    public int Id { get; set; }

    public int IdUsuario { get; set; }

    public int IdMotivoContactanos { get; set; }

    public string Mensaje { get; set; } = null!;

    public DateTime FechaReg { get; set; }

    public int IdEstatus { get; set; }

    public virtual Estatus IdEstatusNavigation { get; set; } = null!;

    public virtual MotivosContactano IdMotivoContactanosNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
