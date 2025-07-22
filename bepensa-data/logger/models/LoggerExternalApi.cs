using System;
using System.Collections.Generic;

namespace bepensa_data.logger.models;

public partial class LoggerExternalApi
{
    public long Id { get; set; }

    public string ApiName { get; set; } = null!;

    public string Method { get; set; } = null!;

    public string? RequestBody { get; set; }

    public string? ResponseBody { get; set; }

    public DateTime? RequestTimestamp { get; set; }

    public DateTime? ResponseTimestamp { get; set; }

    public int? StatusCode { get; set; }

    public string? Resultado { get; set; }

    public Guid? IdTransaccionLog { get; set; }
}
