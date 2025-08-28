using System.ComponentModel.DataAnnotations;

namespace bepensa_models.Enums;

public enum TipoPago
{
    [Display(Name = "Depósito", Description = "Pago mediante depósito bancario")]
    Deposito = 1,
    
    [Display(Name = "Tarjeta de Débito o Crédito", Description = "Pago con tarjeta de débito o crédito")]
    Tarjeta = 2
}
