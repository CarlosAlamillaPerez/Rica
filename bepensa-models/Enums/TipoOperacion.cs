using System.ComponentModel.DataAnnotations;

namespace bepensa_models.Enums;

public enum TipoOperacion
{
    [Display(Name = "Inicio Sesión", Description = "Inicio Sesión")]
    InicioSesion = 1,

    [Display(Name = "Autenticación fallida", Description = "Autenticación fallida")]
    AutenticacionFallida = 2,

    [Display(Name = "Bloqueo", Description = "Bloqueo de cuenta")]
    BloqueoCuenta = 3,

    [Display(Name = "Recuperación contraseña", Description = "Recuperación de contraseña")]
    RecuperarPassword = 4,

    [Display(Name = "Actualización de datos", Description = "Actualización de datos")]
    ActualizarDatos = 5,

    [Display(Name = "Cambio de contraseña", Description = "Cambio de contraseña")]
    CambioContrasenia = 6,

    [Display(Name = "Agrega premio a carrito", Description = "Agrega premio a carrito")]
    AgregaCarrito = 7,

    [Display(Name = "Quito premio a carrito", Description = "Quito premio a carrito")]
    QuitaCarrito = 8,

    [Display(Name = "Modifico cantidad premio a carrito", Description = "Modifico cantidad premio a carrito")]
    ModificaCarrito = 9,

    [Display(Name = "Registro de llamada", Description = "Registro de llamada")]
    RegistroLlamada = 12
}
