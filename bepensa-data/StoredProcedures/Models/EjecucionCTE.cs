using Microsoft.EntityFrameworkCore;

namespace bepensa_data.StoredProcedures.Models;

[Keyless]
public class EjecucionCTE
{
    public string Concepto { get; set; } = null!;

    public string Mes { get; set; } = null!;

    public string EstatusEvaluacion { get; set; } = null!;

    public int Resultado { get; set; }

    public int Puntos { get; set; }
}
