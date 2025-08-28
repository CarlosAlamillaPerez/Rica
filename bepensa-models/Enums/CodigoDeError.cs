using System.ComponentModel.DataAnnotations;

namespace bepensa_models.Enums;

public enum CodigoDeError
{
    [Display(Name = "Existoso", Description = "Existoso")]
    OK = 0,

    [Display(Name = "Usuario sin acceso", Description = "El usuario no existe, está bloqueado o inactivo")]
    UsuarioSinAcceso,

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

    [Display(Name = "Usuario no autorizado", Description = "Usuario no autorizado")]
    UsuarioNoAutorizado,

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

    [Display(Name = "Correo electrónico no registrado", Description = "El usuario no cuenta con un correo electrónico registrado. Se recomienda actualizar la información para continuar.")]
    EmailNoRegistrado,

    [Display(Name = "Correo usado en otra cuenta", Description = "Lo sentimos, la dirección de correo electrónico ingresada ya está en uso por otra cuenta. Por favor, intenta con una dirección de correo electrónico diferente.")]
    CorreoUsado,

    [Display(Name = "Celular usado en otra cuenta", Description = "Lo sentimos, el número de celular ingresado ya está en uso por otra cuenta. Por favor, intenta con un número de celular diferente.")]
    CelularUsado,

    [Display(Name = "Celular no registrado", Description = "El usuario no cuenta con un número de celular registrado. Se recomienda actualizar la información para continuar.")]
    CelularNoRegistrado,

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

    [Display(Name = "Tu cuenta ha sido bloqueada", Description = "Has superado el  número de intentos permitido, por seguridad tu cuenta ha sido bloqueada, comunícate al 01 800 681 8294")]
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
    ErrorDesconocido,

    [Display(Name = "Error de liga de recuperación", Description = "Liga no válida, solicita nuevamente tu enlace para restablecer tu contraseña.")]
    ErrorLigaRecPass,

    [Display(Name = "Error de liga de recuperación", Description = "Expiro el tiempo para cambiar tu contraseña, solicitala nuevamente.")]
    ExpiradaLigaRecPass,

    [Display(Name = "Enlace activado", Description = "El enlace para cambiar la contraseña ha sido visitado y el cambio se completó exitosamente.")]
    LigaPassUtilizada,

    [Display(Name = "Error de link", Description = "El enlace ya no está disponible.")]
    ExpiroLInk,

    [Display(Name = "Encuesta respondida", Description = "Encuesta respondida")]
    EncuestaRespondida,

    [Display(Name = "Solo puedes canjear uno por recarga", Description = "Solo puedes canjear uno por recarga")]
    SoloUnaRecarga,

    [Display(
    Name = "Canje limitado por recarga",
    Description = "Solo puedes canjear un premio por cada recarga. Dirígete al carrito para completar tu canje.")]
    SoloUnaRecargaXCanje,

    [Display(Name = "Ingresa un número telefónico", Description = "Ingresa un número telefónico")]
    IngresaTelefono,

    [Display(Name = "La dirección a donde se enviarán los premios es requerida", Description = "La dirección a donde se enviarán los premios es requerida")]
    DireccionRequerida,

    [Display(Name = "El canje no esta disponible por el momento", Description = "El canje no esta disponible por el momento")]
    CanjeDigitalNoDisponible,

    [Display(Name = "Request invalido", Description = "Request invalido")]
    RequestInvalido,

    [Display(Name = "Agregar solo desde la sección de premios",
             Description = "Este premio solo se puede agregar desde la sección de premios.")]
    SoloDesdeSeccionPremios,

    [Display(Name = "Solo se puede eliminar el premio",
             Description = "Este premio no se puede modificar, solo eliminar.")]
    SoloEliminacion,

    [Display(Name = "No ha seleccionado una tarjeta",
             Description = "Debe seleccionar una tarjeta para poder continuar con este premio.")]
    TarjetaNoSeleccionada,

    [Display(Name = "Pregunta faltante", Description = "Una o más preguntas obligatorias no fueron respondidas.")]
    PreguntaFaltante,

    [Display(Name = "Estado de cuenta no encontrado", Description = "No se encontró un estado de cuenta para el período seleccionado.")]
    EdoCtaNoEncontrado,

    [Display(Name = "Depósito pendiente de liberación", Description = "Tienes un depósito pendiente por liberar. Comunícate al 800 681 8294 para continuar con el proceso.")]
    DepositoPendienteLiberacion,

    [Display(Name = "Redención pendiente en el carrito", Description = "Has realizado una compra de puntos. Dirígete al carrito para completar la redención de tu premio.")]
    RedencionPendienteCarrito,

    [Display(Name = "Acción no permitida", Description = "Este producto proviene de una compra de puntos y no puede ser modificado o eliminado. Para redimirlo, debes procesar el carrito tal como está.")]
    ProductoRedencionNoEditable,

    [Display(Name = "Depósito no encontrado", Description = "No se encontró un depósito pendiente por liberar.")]
    DepositoNoEncontrado,

    [Display(Name = "Pago de puntos no encontrado", Description = "No se encontró tu pago de puntos. Por favor, comunícate con el centro de atención al cliente para reportar el caso.")]
    PagoPuntosNoEncontrado,

    [Display(Name = "Calle no registrada", Description = "El usuario no cuenta con la calle registrada. Se recomienda actualizar la información.")]
    CalleNoRegistrada,

    [Display(Name = "Número exterior no registrado", Description = "El usuario no cuenta con el número exterior registrado. Se recomienda actualizar la información.")]
    NumeroExteriorNoRegistrado,

    [Display(Name = "Código postal no registrado", Description = "El usuario no cuenta con el código postal registrado. Se recomienda actualizar la información.")]
    CodigoPostalNoRegistrado,

    [Display(Name = "Colonia no registrada", Description = "El usuario no cuenta con la colonia registrada. Se recomienda actualizar la información.")]
    ColoniaNoRegistrada,

    [Display(Name = "Ciudad no registrada", Description = "El usuario no cuenta con la ciudad registrada. Se recomienda actualizar la información.")]
    CiudadNoRegistrada,

    [Display(Name = "Entre calle no registrada", Description = "El usuario no cuenta con la referencia 'entre calle' registrada. Se recomienda actualizar la información.")]
    EntreCalleNoRegistrada,

    [Display(Name = "Y calle no registrada", Description = "El usuario no cuenta con la referencia 'y calle' registrada. Se recomienda actualizar la información.")]
    YCalleNoRegistrada,

    [Display(Name = "Referencias no registradas", Description = "El usuario no cuenta con referencias registradas. Se recomienda actualizar la información.")]
    ReferenciasNoRegistradas,

    [Display(Name = "Teléfono no registrado", Description = "El usuario no cuenta con un número telefónico registrado. Se recomienda actualizar la información.")]
    TelefonoNoRegistrado,

    [Display(Name = "Dirección inválida", Description = "La dirección proporcionada es inválida. Se recomienda actualizar la información para continuar.")]
    DireccionInvalida,

    [Display(Name = "Puntos liberados", Description = "Los puntos han sido liberados exitosamente.")]
    PuntosLiberados,

    [Display(Name = "Puntos liberados sin canje", Description = "Los puntos han sido liberados exitosamente, pero el canje no fue procesado. Solo se realizó la compra de puntos.")]
    PuntosLiberadosSinCanje,

    [Display(Name = "Error de liga", Description = "El enlace que intentaste usar ha expirado. Por favor, solicita uno nuevo o repite el proceso correspondiente.")]
    LigaExpirada,

    [Display(Name = "Liga no encontrada", Description = "El enlace que intentaste usar no existe o es incorrecto. Verifica la dirección o solicita uno nuevo.")]
    LigaNoEncontrada,

    #region Errores de servidor
    [Display(Name = "Excepción", Description = "Ha ocurrido un error. Por favor, intenta nuevamente.")]
    Excepcion = 500,

    [Display(Name = "Error de conexión", Description = "Conexión fallida")]
    ConexionFallida,
    #endregion

    #region Errores de validación
    [Display(Name = "Objeto no instanciado", Description = "Objeto no instanciado")]
    ReferenciaNula = 1000,

    [Display(Name = "Propiedad invalida", Description = "Propiedad invalida")]
    PropiedadInvalida,
    #endregion

    #region Errores de negocio

    #region Carrito 3000 .. 3010
    [Display(Name = "Saldo actual insuficiente", Description = "No cuentas con saldo suficiente")]
    SaldoInsuficiente = 3000,
    #endregion

    #endregion

    #region Api OpenPay
    [Display(Name = "Nombre inválido", Description = "El nombre del titular es requerido.")]
    TitularTarjetaRequerido,

    [Display(Name = "Tarjeta inválida", Description = "El número de tarjeta es requerido.")]
    TarjetaRequerido,

    [Display(Name = "Mes inválido", Description = "El mes es requerido.")]
    MesTarjetaRequerido,

    [Display(Name = "Año inválido", Description = "El año es requerido.")]
    AnioTarjetaRequerido,

    [Display(Name = "CVV inválido", Description = "El código de seguridad es requerido.")]
    CVVTarjetaRequerido,


    [Display(Name = "Nombre inválido", Description = "Por favor, ingresa un nombre válido sin números ni símbolos.")]
    TitularTarjetaInvalido,

    [Display(Name = "Número de tarjeta inválido", Description = "El número de tarjeta es inválido o está incompleto")]
    NumeroTarjetaInvalido,

    [Display(Name = "Código de segurida inválido o está incompleto.", Description = "El código de segurida es inválido.")]
    CVVTarjetaInvalido,

    [Display(Name = "Fecha de expiración inválida", Description = "La fecha de expiración es inválida o está incompleta")]
    FechaExpiracionInvalida,


    [Display(Name = "Token no generado", Description = "No se pudo generar el token de la tarjeta")]
    TokenNoGenerado,

    [Display(Name = "Error de red", Description = "Error de comunicación con el servidor de OpenPay")]
    ErrorDeRed,

    [Display(Name = "Fondos insuficientes", Description = "No hay fondos suficientes en la tarjeta")]
    FondosInsuficientes,

    [Display(Name = "Tarjeta expirada", Description = "La tarjeta ha expirado")]
    TarjetaExpirada,

    [Display(Name = "Tarjeta bloqueada", Description = "La tarjeta está bloqueada o restringida")]
    TarjetaBloqueada,

    [Display(Name = "Error desconocido", Description = "Error no especificado en la respuesta de OpenPay")]
    ErrorDesconocidoOP,
    #endregion

    #region Api
    [Display(Name = "Autenticación fallida", Description = "No se pudo autenticar con las credenciales proporcionadas.")]
    AutenticacionFallidaApi,

    [Display(Name = "Sesión caducada", Description = "El token de autenticación ha expirado. Es necesario obtener uno nuevo.")]
    SesionCaducadaApi,

    [Display(Name = "Servidor no disponible", Description = "El servidor remoto no está disponible o no responde.")]
    ServidorNoDisponible,
    #endregion
}