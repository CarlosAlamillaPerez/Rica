using System;
using System.Collections.Generic;

namespace bepensa_data.logger.models;

public partial class LoggerPushNotification
{
    public long Id { get; set; }

    public string? Codigo { get; set; }

    public DateTime FechaRegistro { get; set; }

    public string? Request { get; set; }

    public DateTime? RequestFecha { get; set; }

    public string? Response { get; set; }

    public DateTime? ResponseFecha { get; set; }

    public Guid IdTransaccion { get; set; }
}
