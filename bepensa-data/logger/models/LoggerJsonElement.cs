using System;
using System.Collections.Generic;

namespace bepensa_data.logger.models;

public partial class LoggerJsonElement
{
    public long Id { get; set; }

    public string Tipo { get; set; } = null!;

    public string? JsonReceived { get; set; }

    public DateTime? FechaReg { get; set; }
}
