using Microsoft.EntityFrameworkCore;

namespace bepensa_data.StoredProcedures.Models;

[Keyless]
public class FotoExitoCTE
{
    public int IdPeriodo { get; set; }

    public DateOnly Fecha { get; set; }

    public int IdSda { get; set; }
    public string SubconceptoAcumulacion { get; set; } = null!;

    public string? FondoColor { get; set; }

    public string? LetraColor { get; set; }

    public string SegAcumulacion { get; set; } = null!;

    public string Empaques { get; set; } = null!;

    public string? Imagen { get; set; }

    public bool Cumple { get; set; }
}
