using System.ComponentModel.DataAnnotations;

namespace bepensa_models.Enums;

public enum TipoMensajeria
{
    [Display(Name = "Correo electrónico")]
    Email = 1,

    [Display(Name = "Mensajería instantanía")]
    Sms = 2
}
