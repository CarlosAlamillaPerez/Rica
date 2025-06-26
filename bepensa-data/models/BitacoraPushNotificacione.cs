using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class BitacoraPushNotificacione
{
    public long Id { get; set; }

    public int IdCatPushNotificacion { get; set; }

    public int IdPeriodo { get; set; }

    public int IdUsuario { get; set; }

    public string TituloEn { get; set; } = null!;

    public string TituloEs { get; set; } = null!;

    public string? TextoEn { get; set; }

    public string? TextoEs { get; set; }

    public string? UrlLink { get; set; }

    public string? ImgIos { get; set; }

    public string? ImgAndroid { get; set; }

    public string? Icon { get; set; }

    public string? Seccion { get; set; }

    public DateTime? FechaEnvio { get; set; }

    public string? Codigo { get; set; }

    public Guid Token { get; set; }

    public int IdEstatusPushNotificacion { get; set; }

    public int Lecturas { get; set; }

    public DateTime? FechaLectura { get; set; }

    public int IdOrigen { get; set; }

    public DateTime FechaReg { get; set; }

    public int? IdOperadorReg { get; set; }

    public DateTime? FechaMod { get; set; }

    public int? IdOperadorMod { get; set; }

    public virtual CatalogoPushNotificacione IdCatPushNotificacionNavigation { get; set; } = null!;

    public virtual EstatusDePushNotificacione IdEstatusPushNotificacionNavigation { get; set; } = null!;

    public virtual Operadore? IdOperadorModNavigation { get; set; }

    public virtual Operadore? IdOperadorRegNavigation { get; set; }

    public virtual Origene IdOrigenNavigation { get; set; } = null!;

    public virtual Periodo IdPeriodoNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
