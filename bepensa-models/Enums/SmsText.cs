using System.ComponentModel.DataAnnotations;

namespace bepensa_models.Enums;

public enum SmsText
{
    [Display(Name = "Bepensa México", Description = "Hola, te compartimos tu contraseña de Socio Selecto: @PASSWORD")]
    RestablecerPass
}
