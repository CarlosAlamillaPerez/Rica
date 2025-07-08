using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class CatalogoPushNotificacione
{
    public int Id { get; set; }

    public int IdCanal { get; set; }

    public string Codigo { get; set; } = null!;

    public string TituloEn { get; set; } = null!;

    public string TituloEs { get; set; } = null!;

    public string? TextoEn { get; set; }

    public string? TextoEs { get; set; }

    public string? UrlLink { get; set; }

    public string? ImgIos { get; set; }

    public string? ImgAndroid { get; set; }

    public string? Icon { get; set; }

    public Guid IdTransaccion { get; set; }

    public string? Seccion { get; set; }

    public virtual ICollection<BitacoraPushNotificacione> BitacoraPushNotificaciones { get; set; } = new List<BitacoraPushNotificacione>();

    public virtual Canale IdCanalNavigation { get; set; } = null!;
}
