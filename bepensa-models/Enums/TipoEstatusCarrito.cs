using System.ComponentModel.DataAnnotations;

namespace bepensa_models.Enums
{
    public enum TipoEstatusCarrito
    {
        [Display(Name = "En Proceso", Description = "En Proceso")]
        EnProceso = 1,

        [Display(Name = "Cancelado", Description = "Cancelado")]
        Cancelado = 2,

        [Display(Name = "Procesado", Description = "Procesado")]
        Procesado = 3,

        [Display(Name = "No Procesado", Description = "No Procesado")]
        NoProcesado
    }
}
