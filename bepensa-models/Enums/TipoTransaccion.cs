using System.ComponentModel.DataAnnotations;

namespace bepensa_models.Enums;

public enum TipoTransaccion
{
    [Display(Name = "Recarga", Description = "Recarga")]
    Recarga = 1,

    [Display(Name = "Codigo", Description = "Codigo")]
    Codigo,

    [Display(Name = "Envio", Description = "Envio")]
    Envio,

    [Display(Name = "Pago Servicio", Description = "Pago Servicio")]
    PagoServicio
}
