using System.ComponentModel.DataAnnotations;

namespace bepensa_models.Enums;

public enum CodigoDeError
{
    [Display(Name = "Existoso", Description = "Existoso")]
    OK = 0,

    [Display(Name = "Excepción", Description = "Excepción en el proceso")]
    Excepcion,

    [Display(Name = "Error de conexión", Description = "Conexión fallida")]
    ConexionFallida,

    [Display(Name = "Usuario sin acceso", Description = "El usuario no existe, está bloqueado o inactivo")]
    UsuarioSinAcceso,

    [Display(Name = "Objeto no instanciado", Description = "Objeto no instanciado")]
    NullReference,

    [Display(Name = "Objeto no instanciado", Description = "Objeto no instanciado")]
    ReferenciaNula,

    [Display(Name = "Propiedad invalida", Description = "Propiedad invalida")]
    PropiedadInvalida,

    [Display(Name = "Propiedad invalida", Description = "Propiedad invalida")]
    InvalidPropertie,

    [Display(Name = "El correo no existe", Description = "Usuario y/o contraseña inválido(s).")]
    UsuarioInvalido,

    [Display(Name = "Usuario bloqueado o inactivo", Description = "Usuario bloqueado o inactivo")]
    UserNoactiveOrLocked,

    [Display(Name = "No hay datos", Description = "No hay información disponible por el momento.")]
    SinDatos,

    [Display(Name = "Token Invalido", Description = "Token Invalido")]
    InvalidToken,

    [Display(Name = "Ya aplicaste tu cambio", Description = "Ya aplicaste tu cambio")]
    AppliedChange,

    [Display(Name = "Usuario Activo", Description = "Usuario ya se encuentra activo")]
    UsuarioActivo,

    [Display(Name = "Registro no existente", Description = "Registro no existente")]
    NonExistentRecord,

    [Display(Name = "Sesión caducada", Description = "La sesión ha expirado")]
    SesionCaducada,

    [Display(Name = "Password usado", Description = "Tu contraseña deberá ser diferente a las ultimas 3 registradas")]
    PasswordUsado,

    [Display(Name = "Usuario no existe", Description = "No hay ningún usuario con esos datos")]
    NoExisteUsuario,

    [Display(Name = "Empleado no encontrado", Description = "Empleado no encontrado, por favor revisa y vuelve a capturarlos")]
    EmpleadoNoEncontrado,

    [Display(Name = "Empleado no autorizado", Description = "Empleado no autorizado")]
    EmpleadoNoAutorizado,

    [Display(Name = "Usuario registrado", Description = "Este usuario ya se encuentra registrado.")]
    UsuarioRegistrado,

    [Display(Name = "Password usado", Description = "Tu contraseña deberá ser diferente a las ultimas 3 registradas")]
    UsedPassword,

    [Display(Name = "No existe la tarjeta", Description = "No existe la tarjeta")]
    CardNoExist,

    [Display(Name = "Limite Cedi", Description = "Se alcanzó el número máximo de registros por CEDI")]
    LimCEDI,

    [Display(Name = "No se guardo la información", Description = "No se pudo completar la operación de guardado. Intente nuevamente más tarde")]
    SaveInfo,

    [Display(Description = "Error al guardar los cambios. Por favor, inténtelo de nuevo más tarde")]
    UpdateInfo,

    [Display(Name = "Guardado fallido", Description = "Guardado fallido. Verifique que todos los campos obligatorios estén completos")]
    SaveInvalid,

    [Display(Name = "Correo ya registrado", Description = "El correo electrónico que ha ingresado ya está registrado")]
    EmailRegisteredMsgSimple,

    [Display(Name = "Correo ya registrado", Description = "La dirección de correo electrónico ya está registrada en nuestro sistema. Por favor, inicie sesión con esa cuenta o utilice una dirección de correo electrónico diferente")]
    EmailRegistered,

    [Display(Name = "Teléfono usado en otra cuenta", Description = "Lo sentimos, el número de teléfono ingresado ya está en uso por otra cuenta. Por favor, intenta con un número de teléfono diferente.")]
    TelefonoUsado,

    [Display(Name = "Correo electrónico usado en otra cuenta", Description = "Lo sentimos, el correo electrónico ingresado ya está en uso por otra cuenta. Por favor, intenta con un correo electrónico diferente.")]
    EmailUsado,

    [Display(Name = "Error al guardar la información. Intenta más tarde.", Description = "Hubo un problema al intentar guardar la información. Por favor, intenta nuevamente más tarde.")]
    ErrorDeGuardado,

    [Display(Name = "Celular ya registrado", Description = "El celular que ha ingresado ya está registrado.")]
    CelRegisteredMsgSimple,

    [Display(Name = "La contraseña actual no coicide", Description = "La contraseña actual no coicide")]
    ContraseniaActualNoCoincide,

    [Display(Name = "Correo inválido", Description = "El correo electrónico proporcionado no es válido, verifícalo.")]
    EmailInvalido,

    [Display(Name = "Correo usado en otra cuenta", Description = "Lo sentimos, la dirección de correo electrónico ingresada ya está en uso por otra cuenta. Por favor, intenta con una dirección de correo electrónico diferente.")]
    CorreoUsado,

    [Display(Name = "Celular usado en otra cuenta", Description = "Lo sentimos, el número de celular ingresado ya está en uso por otra cuenta. Por favor, intenta con un número de celular diferente.")]
    CelularUsado,

    [Display(Name = "Sin cambios", Description = "Si requieres realizar alguna modificación en los datos, por favor realiza los cambios directamente en el formulario y luego haz clic en el botón \"Actualizar\".")]
    SinCambios,

    [Display(Name = "Periodo no válido", Description = "El periodo ingresado no es válido.")]
    PeriodoInvalido,

    [Display(Name = "Usuario no válido", Description = "El nombre de usuario ingresado no es reconocido por el sistema. Por favor, revisa tu entrada y vuelve a intentarlo.")]
    IdUsuarioNoValido,

    [Display(Name = "Usuario Inactivo", Description = "Tu cuenta está inactiva.")]
    UsuarioInactivo,

    [Display(Name = "Usuario bloqueado", Description = "Tu cuenta está bloqueada")]
    UsuarioBloqueado,

    [Display(Name = "Saldo actual insuficiente", Description = "No cuentas con saldo suficiente")]
    SaldoInsuficiente,

    [Display(Name = "Fallo al agregar premio", Description = "Hubo un problema al agregar los premios al carrito. Inténtalo nuevamente")]
    FalloAgregarPremio,

    [Display(Name = "Premio no encontrado", Description = "El premio que intentaste agregar no se encontró o no se encuentra disponible")]
    PremioNoEncontrado,

    [Display(Name = "Carrito vacío", Description = "El carrito está vacío. Agrega artículos para continuar.")]
    CarritoVacío,

    [Display(Name = "Canje no encontrado", Description = "El canje que intentaste buscar no se encontró")]
    CanjeNoEncontrado,

    [Display(Name = "Periodo no encontrado", Description = "El periodo no fue encontrado.")]
    PeriodoNoEncontrado,

    [Display(Name = "Proceso de Carrito Fallido", Description = "El carrito no pudo ser procesado debido a un error")]
    CarritoFallido,

    [Display(Name = "Proceso de carrito incommpleto", Description = "El proceso de carrito se completó parcialmente; algunos premios fueron procesados correctamente")]
    ProcesoCarritoIncommpleto,

    [Display(Name = "Premios no disponibles", Description = "Algunos premios en tu carrito ya no están disponibles y han sido eliminados. Por favor, revisa tu carrito para ver los artículos restantes")]
    PremiosInactivos,

    [Display(Name = "Fallo al agregar redención", Description = "Hubo un problema al canjear los premios del carrito. Inténtalo nuevamente")]
    FalloAgregarRedencion,

    [Display(Name = "No hay resultados", Description = "No se encontraron resultados para la búsqueda")]
    SinResultados,

    [Display(Name = "Tu cuenta ha sido bloqueada", Description = "Has superado el  número de intentos permitido, por seguridad tu cuenta ha sido bloqueada, comunícate al 01 800  000 00")]
    IntentoDeSesion,

    [Display(Name = "Operador no válido", Description = "El operador no fue encontrado")]
    OperadorInvalido,

    [Display(Name = "Acceso denegado", Description = "No dispones de los privilegios necesarios para continuar")]
    OperadorNoAutorizado,

    [Display(Name = "No se pudo enviar el correo", Description = "No se pudo enviar el correo. Por favor, intenta nuevamente más tarde. Si el problema persiste, considera reportarlo")]
    FalloEnviarCorreo,
    
    [Display(Name = "Código Postal No Encontrado", Description = "El código postal proporcionado no fue encontrado o no existe.")]
    CodigoPostalNoEncontrado,

    [Display(Name = "Período de Inscripción Finalizado", Description = "El período de inscripción ha finalizado y ya no es posible registrarse.")]
    InscripcionFinalizada,

    [Display(Name = "Error desconocido.",
             Description = "Ocurrió un error inesperado. Por favor, intenta de nuevo más tarde.")]
    ErrorDesconocido
}