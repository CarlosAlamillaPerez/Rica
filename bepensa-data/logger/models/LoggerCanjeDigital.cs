using System;
using System.Collections.Generic;

namespace bepensa_data.logger.models;

public partial class LoggerCanjeDigital
{
    public long Id { get; set; }

    public int? IdUsuario { get; set; }

    public int? IdCarrito { get; set; }

    public int? IdPremio { get; set; }

    public string? Folio { get; set; }

    public DateTime FechaRegistro { get; set; }

    public string? Request { get; set; }

    public DateTime? RequestFecha { get; set; }

    public string? Response { get; set; }

    public DateTime? ResponseFecha { get; set; }

    public Guid IdTransaccion { get; set; }
}
