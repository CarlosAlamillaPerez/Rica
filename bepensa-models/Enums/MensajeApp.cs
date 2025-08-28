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
    PassCambiada,

    [Display(Name = "Premio agregado", Description = "Agregaste el premio al carrito correctamente.")]
    PremioAgregado,

    [Display(Name = "Canje exitoso", Description = "¡Tu canje ha sido exitoso!")]
    CanjeExitoso,

    [Display(Name = "Compra de puntos exitosa", Description = "¡Tu compra de puntos y canje se ha realizado con éxito!")]
    CompraPuntosExitosa,

    [Display(Name = "Compra de puntos exitosa", Description = "¡Tu compra de puntos se ha realizado con éxito! Realiza tu depósito y llama al 800 681 8294 para confirmar la recepción.")]
    CompraPuntosExitosaDepostio,

    [Display(Name = "Compra de puntos realizada", Description = "Tu compra de puntos se ha registrado correctamente, pero ocurrió un error inesperado al intentar realizar el canje. Por favor, intenta nuevamente más tarde o contacta a soporte si el problema persiste.")]
    CompraPuntosConErrorEnCanje,

    [Display(
    Name = "Verificación bancaria requerida",
    Description = "Tu compra de puntos ha sido registrada correctamente. Serás redirigido a una página segura donde podrás completar la verificación ingresando el código recibido en la app de tu banco. Este proceso es requerido por la plataforma de pagos para finalizar la transacción.")]
    CompraPuntosRequiereRedireccionAVerificacionBancaria,
}
