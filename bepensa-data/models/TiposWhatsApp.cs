using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class TiposWhatsApp
{
    public int Id { get; set; }

    public int IdCanal { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Parametros { get; set; }

    public string? Mensaje { get; set; }

    public string? Plantilla { get; set; }

    public string? UrlImagen { get; set; }

    public string? Botones { get; set; }

    public virtual ICollection<BitacoraDeWhatsApp> BitacoraDeWhatsApps { get; set; } = new List<BitacoraDeWhatsApp>();

    public virtual Canale IdCanalNavigation { get; set; } = null!;
}
