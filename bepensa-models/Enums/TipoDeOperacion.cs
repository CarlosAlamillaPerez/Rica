using System.ComponentModel.DataAnnotations;

namespace bepensa_models.Enums;

public enum TipoDeOperacion
    {
        [Display(Name = "Inicio Sesión", Description = "Inicio Sesión")]
        InicioSesion = 1,

        [Display(Name = "Autenticación fallida", Description = "Autenticación fallida")]
        AuthenticationFailed,

        [Display(Name = "Bloqueo", Description = "Bloqueo de cuenta")]
        BloqueoCuenta,

        [Display(Name = "Recuperación contraseña", Description = "Recuperación de contraseña")]
        RecuperarPassword,

        [Display(Name = "Actualización de datos", Description = "Actualización de datos")]
        UpdateData,

        [Display(Name = "Cambio de contraseña", Description = "Cambio de contraseña")]
        CambioContrasenia,

        [Display(Name = "Agrega premio a carrito", Description = "Agrega premio a carrito")]
        AgregaCarrito,

        [Display(Name = "Quito premio a carrito", Description = "Quito premio a carrito")]
        QuitaCarrito,

        [Display(Name = "Modifico cantidad premio a carrito", Description = "Modifico cantidad premio a carrito")]
        ModificaCarrito,

        [Display(Name = "Proceso carrito", Description = "Realizo redención")]
        ProcesarCarrito,

        [Display(Name = "Fuerza de Venta inicia sesión en usuario", Description = "Fuerza de Venta inicia sesión en usuario")]
        IniciaSesionFDV,

        [Display(Name = "Actualización de usuario", Description = "Actualización de usuario")]
        ActualizarUsuarioCRM
    }
