using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class Encuesta
{
    public int Id { get; set; }

    public int IdCanal { get; set; }

    public string Codigo { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string? Url { get; set; }

    public Guid Token { get; set; }

    public string Vista { get; set; } = null!;

    public string Tema { get; set; } = null!;

    public DateOnly FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }

    public bool Persistente { get; set; }

    public string MensajeEnvio { get; set; } = null!;

    public int IdEstatus { get; set; }

    public DateTime FechaReg { get; set; }

    public int IdOperadorReg { get; set; }

    public DateTime? FechaMod { get; set; }

    public int? IdOperadorMod { get; set; }

    public virtual ICollection<BitacoraDeEncuestum> BitacoraDeEncuesta { get; set; } = new List<BitacoraDeEncuestum>();

    public virtual Canale IdCanalNavigation { get; set; } = null!;

    public virtual Estatus IdEstatusNavigation { get; set; } = null!;

    public virtual Operadore? IdOperadorModNavigation { get; set; }

    public virtual Operadore IdOperadorRegNavigation { get; set; } = null!;

    public virtual ICollection<PreguntasEncuestum> PreguntasEncuesta { get; set; } = new List<PreguntasEncuestum>();
}
