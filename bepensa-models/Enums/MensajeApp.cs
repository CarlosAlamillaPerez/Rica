using System.ComponentModel.DataAnnotations;

namespace bepensa_models.Enums;

public enum MensajeApp
{
    [Display(Name = "Enlace enviado al correo",
             Description = "El enlace para restablecer la contraseña ha sido enviado correctamente al correo electrónico.")]
    EnlaceCorreoEnviado,

    [Display(Name = "Enlace enviado al celular",
             Description = "El enlace para restablecer la contraseña ha sido enviado correctamente al celular.")]
    EnlaceSMSenviado,

    [Display(Name = "Contraseña cambiada con éxito", Description = "La contraseña se ha cambiado correctamente.")]
    PassCambiada
}
