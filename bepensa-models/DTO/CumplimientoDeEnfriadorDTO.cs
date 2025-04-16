using bepensa_data.StoredProcedures.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bepensa_models.DTO
{
    public class CumplimientoDeEnfriadorDTO
    {
        public int IdPeriodo { get; set; }

        public DateOnly Fecha { get; set; }

        public string? NombrePeriodo { get; set; } = string.Empty;

        public List<CumplimientosDeEnfriadorCTE>? Cumplimientos { get; set; }
    }
}
