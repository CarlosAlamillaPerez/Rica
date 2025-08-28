using System.ComponentModel.DataAnnotations;

namespace bepensa_models.Enums;

public enum TipoEstatusPago
{
    [Display(Name = "En Proceso", Description = "Pago en proceso de validación o transferencia")]
    EnProceso = 1,
    
    [Display(Name = "Confirmado", Description = "Pago recibido y confirmado")]
    Confirmado = 2,

    [Display(Name = "Fallido", Description = "Pago recibido y confirmado")]
    Fallido = 3,

    [Display(Name = "Cancelado", Description = "Pago cancelado por el usuario o sistema")]
    Cancelado = 4
}
