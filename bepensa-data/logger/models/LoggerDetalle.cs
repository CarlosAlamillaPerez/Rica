using System;
using System.Collections.Generic;

namespace bepensa_data.logger.models;

public partial class LoggerDetalle
{
    public long Id { get; set; }

    public string Fuente { get; set; } = null!;

    public string? Controlador { get; set; }

    public string? Metodo { get; set; }

    public string? Descripcion { get; set; }

    public string? Mensaje { get; set; }

    public string? Trace { get; set; }

    public string Usuario { get; set; } = null!;

    public DateTime FechaRegistro { get; set; }

    public string? Request { get; set; }

    public DateTime? RequestFecha { get; set; }

    public string? Response { get; set; }

    public DateTime? ResponseFecha { get; set; }

    public Guid? IdTransaccion { get; set; }
}
