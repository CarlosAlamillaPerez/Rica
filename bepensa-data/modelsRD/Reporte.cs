using System;
using System.Collections.Generic;

namespace bepensa_data.modelsRD;

public partial class Reporte
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int IdSeccion { get; set; }

    public string StoreProcedure { get; set; } = null!;

    public string? ColorTxt { get; set; }

    public string? ColorBg { get; set; }

    public string? Icono { get; set; }

    public int IdEstatus { get; set; }

    public int Orden { get; set; }

    public int StyleTabla { get; set; }

    public virtual Estatus IdEstatusNavigation { get; set; } = null!;

    public virtual Seccione IdSeccionNavigation { get; set; } = null!;
}
