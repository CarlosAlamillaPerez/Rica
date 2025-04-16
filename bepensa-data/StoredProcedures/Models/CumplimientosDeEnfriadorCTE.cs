using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bepensa_data.StoredProcedures.Models
{
    [Keyless]
    public class CumplimientosDeEnfriadorCTE
    {
        public int IdUsuario { get; set; }

        public DateOnly Fecha { get; set; }

        public int IdPeriodo { get; set; }

        public string NombrePeriodo { get; set; } = null!;

        public int IdSDA { get; set; }

        public string NombreSDA { get; set; } = null!;

        public int Resultado { get; set; }
    }
}
