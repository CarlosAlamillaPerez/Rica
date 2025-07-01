using System;
using System.Collections.Generic;

namespace bepensa_data.logger.models;

public partial class LoggerCortesCuentum
{
    public long Id { get; set; }

    public int IdPeriodo { get; set; }

    public int IdUsuario { get; set; }

    public int IdMovimiento { get; set; }

    public int Saldo { get; set; }

    public DateTime FechaReg { get; set; }

    public Guid IdTransaccionLog { get; set; }
}
