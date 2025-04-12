using bepensa_data.StoredProcedures.Models;
using Microsoft.EntityFrameworkCore;

namespace bepensa_data.data
{
    public partial class BepensaContext
    {
        public DbSet<EjecucionCTE> EjecuionTradicional { get; set; }

        public DbSet<PortafolioPrioritarioCTE> PortafolioPrioritario { get; set; }
    }
}
