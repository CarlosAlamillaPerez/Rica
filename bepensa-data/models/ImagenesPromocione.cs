using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class ImagenesPromocione
{
    public int Id { get; set; }

    public int IdCanal { get; set; }

    public int IdPeriodo { get; set; }

    public string Nombre { get; set; } = null!;

    public string Url { get; set; } = null!;

    public int IdEstatus { get; set; }

    public DateTime FechaReg { get; set; }

    public int IdOperadorReg { get; set; }

    public int IdOperadorMod { get; set; }

    public virtual Canale IdCanalNavigation { get; set; } = null!;

    public virtual Estatus IdEstatusNavigation { get; set; } = null!;

    public virtual Operadore IdOperadorModNavigation { get; set; } = null!;

    public virtual Operadore IdOperadorRegNavigation { get; set; } = null!;

    public virtual Periodo IdPeriodoNavigation { get; set; } = null!;
}
