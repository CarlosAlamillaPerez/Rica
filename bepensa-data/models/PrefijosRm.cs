using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class PrefijosRm
{
    public int Id { get; set; }

    public int IdCanal { get; set; }

    public int IdZona { get; set; }

    public string Prefijo { get; set; } = null!;

    public DateTime FechaReg { get; set; }

    public int IdOperadorReg { get; set; }

    public DateTime? FechaMod { get; set; }

    public int? IdOperadorMod { get; set; }

    public virtual Canale IdCanalNavigation { get; set; } = null!;

    public virtual Operadore? IdOperadorModNavigation { get; set; }

    public virtual Operadore IdOperadorRegNavigation { get; set; } = null!;

    public virtual Zona IdZonaNavigation { get; set; } = null!;
}
