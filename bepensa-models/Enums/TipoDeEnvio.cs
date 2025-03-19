using System.ComponentModel.DataAnnotations;

namespace bepensa_models;

public enum TipoDeEnvio
    {
        [Display(Name = "Cambio de contraseña", Description = "Cambio de contraseña")]
        RecuperarContrasenia = 1,

        [Display(Name = "Bienvenida", Description = "Se envía bienvenida por mensajería Default")]
        Bienvenida = 2,

        [Display(Name = "Realizaste Canje", Description = "Realizaste un Canje")]
        RealizasteCanje = 3,

        [Display(Name = "Envío de contraseña", Description = "Se envía contraseña")]
        EnvioDePassword,

        [Display(Name = "Normal", Description = "Se envía por mensajería Default")]
        Normal = 6
    }
