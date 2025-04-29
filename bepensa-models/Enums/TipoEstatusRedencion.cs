using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bepensa_models.Enums
{
    public enum TipoEstatusRedencion
    {
        [Display(Name = "Solicitado", Description = "Solicitado")]
        Solicitado = 1,

        [Display(Name = "En Proceso", Description = "En Proceso")]
        EnProceso = 2,

        [Display(Name = "En garantia", Description = "En garantia")]
        EnGarantia = 3,

        [Display(Name = "Incidencia", Description = "Incidencia")]
        Incidencia = 4,

        [Display(Name = "Entregado", Description = "Entregado")]
        Entregado = 5,

        [Display(Name = "Cancelado", Description = "Cancelado")]
        Cancelado = 6,

        [Display(Name = "Confirmado", Description = "Confirmado")]
        Confirmado,
    }
}
