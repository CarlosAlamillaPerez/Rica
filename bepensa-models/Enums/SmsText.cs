using System.ComponentModel.DataAnnotations;

namespace bepensa_models.Enums;

public enum SmsText
{
    [Display(Name = "Bepensa México", Description = "Bepensa. Haz clic en el siguiente enlace para cambiar tu contraseña: @URL")]
    RestablecerPass
}
