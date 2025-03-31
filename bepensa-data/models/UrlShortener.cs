using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class UrlShortener
{
    public long Id { get; set; }

    public string OriginalUrl { get; set; } = null!;

    public string ShortUrl { get; set; } = null!;

    public string Clave { get; set; } = null!;

    public DateTime FechaReg { get; set; }

    public int Clicks { get; set; }

    public int IdEstatus { get; set; }

    public virtual Estatus IdEstatusNavigation { get; set; } = null!;
}
