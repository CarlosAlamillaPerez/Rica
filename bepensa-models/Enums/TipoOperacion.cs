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

    [Display(Name = "Modifico cantidad premio a carrito", Description = "Modifico cantidad premio a carrito")]
    ProcesarCarrito = 10,

    [Display(Name = "Fuerza de Venta inicia sesión en usuario", Description = "Fuerza de Venta inicia sesión en usuario")]
    InicioSesionFDV = 11,

    [Display(Name = "Registro de llamada", Description = "Registro de llamada")]
    RegistroLlamada = 12,

    [Display(Name = "Elimino carrito", Description = "Elimino carrito")]
    EliminoCarrito = 13,

    [Display(Name = "Encuesta completada", Description = "Encuesta completada")]
    EncuestaCompletada = 14,

    [Display(Name = "Compra de puntos", Description = "Compra de puntos")]
    CompraPuntos = 15,

    [Display(Name = "Solicitud de compra de puntos", Description = "Solicitud de compra de puntos")]
    SolicitudCompraPuntos = 16,
    
    [Display(Name = "Cancelación de compra de puntos", Description = "Solicitud de compra de puntos")]
    CancelacionCompraPuntos = 17,

    DesbloqueoCuenta = 18
}
