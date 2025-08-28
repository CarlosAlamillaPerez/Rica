using System;
using System.Collections.Generic;

namespace bepensa_data.modelsRD;

public partial class ImagenesPromocione
{
    public int Id { get; set; }

    public int IdPrograma { get; set; }

    public int IdPeriodo { get; set; }

    public string Nombre { get; set; } = null!;

    public string Url { get; set; } = null!;

    public string Tipo { get; set; } = null!;

    public int IdEstatus { get; set; }

    public DateTime FechaReg { get; set; }

    public int IdOperadorReg { get; set; }

    public DateTime? FechaMod { get; set; }

    public int? IdOperadorMod { get; set; }

    public virtual Estatus IdEstatusNavigation { get; set; } = null!;

    public virtual Operadore? IdOperadorModNavigation { get; set; }

    public virtual Operadore IdOperadorRegNavigation { get; set; } = null!;

    public virtual Periodo IdPeriodoNavigation { get; set; } = null!;

    public virtual Programa IdProgramaNavigation { get; set; } = null!;
}
