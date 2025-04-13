using Microsoft.EntityFrameworkCore;

namespace bepensa_data.StoredProcedures.Models
{
    [Keyless]
    public class EstadoDeCuentaCTE
    {
        public string Concepto { get; set; } = null!;

        public int Puntos { get; set; }
    }
}
