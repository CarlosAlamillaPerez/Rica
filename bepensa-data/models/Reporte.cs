using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class Reporte
{
    public int Id { get; set; }

    public int IdCanal { get; set; }

    public int IdDestino { get; set; }

    public int IdSeccion { get; set; }

    public string Nombre { get; set; } = null!;

    public string StoreProcedure { get; set; } = null!;

    public int BitCanal { get; set; }

    public string ColorTxt { get; set; } = null!;

    public string ColorBg { get; set; } = null!;

    public string? Icono { get; set; }

    public int EstiloTabla { get; set; }

    public int Orden { get; set; }

    public int IdEstatus { get; set; }

    public virtual Canale IdCanalNavigation { get; set; } = null!;

    public virtual Origene IdDestinoNavigation { get; set; } = null!;

    public virtual Estatus IdEstatusNavigation { get; set; } = null!;

    public virtual Seccione IdSeccionNavigation { get; set; } = null!;
}
