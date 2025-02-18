using System.ComponentModel.DataAnnotations;

namespace bepensa_socio_selecto_models.Enums;

public enum CodigoDeError
{
    [Display(Name = "Exitoso", Description = "Exitoso")]
    OK = 0,

    [Display(Name = "Excepción", Description = "Se ha producido un error inesperado. Por favor, intenta de nuevo.")]
    Excepcion,

    [Display(Name = "Error de conexión", Description = "Conexión fallida")]
    ConexionFallida,

    [Display(Name = "Objeto no instanciado", Description = "Objeto no instanciado")]
    ReferenciaNula,

    [Display(Name = "Propiedad invalida", Description = "Propiedad invalida")]
    PropiedadInvalida,

    [Display(Name = "Usuario Inválido", Description = "Usuario y/o contraseña inválido(s)")]
    UsuarioInvalido,

    [Display(Name = "Usuario inactivo", Description = "Cuenta bloqueada o inactiva. Para resolverlo, contacta con nuestro soporte.")]
    UsuarioInactivo,

    [Display(Name = "Usuario bloqueado", Description = "Tu cuenta ha sido bloqueada tras varios intentos incorrectos. Contacta con soporte para asistencia.")]
    UsuarioBloqueado,

    [Display(Name = "Cuenta bloqueada", Description = "Cuenta bloqueada por seguridad. Para resolverlo, contacta con nuestro soporte.")]
    CuentaBloqueada,

    [Display(Name = "No hay datos", Description = "No hay información disponible en este momento.")]
    SinDatos,

    [Display(Name = "Token Invalido", Description = "Token Invalido")]
    TokenInvalido,

    [Display(Name = "Usuario activo", Description = "El usuario ya se encuentra activo. Puede iniciar sesión o recuperar su contraseña.")]
    UsuarioActivo,

    [Display(Name = "Registro no existente", Description = "Registro no existente")]
    NoExiste,

    [Display(Name = "Sesión caducada", Description = "La sesión ha expirado")]
    SesionCaducada,

    [Display(Name = "Password usado", Description = "Tu contraseña deberá ser diferente a las ultimas 3 registradas")]
    PasswordUsado,

    [Display(Name = "Usuario no existe", Description = "Usuario no existe")]
    NoExisteUsuario,

    [Display(Name = "Empleado no encontrado", Description = "Empleado no encontrado, por favor revisa y vuelve a capturarlos")]
    EmpleadoNoEncontrado,

    [Display(Name = "Empleado no autorizado", Description = "Empleado no autorizado")]
    EmpleadoNoAutorizado,

    [Display(Name = "Usuario registrado", Description = "Este empleado ya fue registrado al programa")]
    UsuarioRegistrado,

    [Display(Name = "Error al registrar", Description = "Hubo un error al intentar registrar el usuario")]
    ErrorRegistroUsuario,

    [Display(Name = "Usuario no encontrado", Description = "Lo sentimos, no pudimos encontrar ninguna cuenta asociada con los datos proporcionados. Verifica los detalles e intenta nuevamente. Si necesitas ayuda, no dudes en contactarnos.")]
    UsuarioNoEncontrado,

    [Display(Name = "Correo inválido", Description = "El correo electrónico proporcionado no es válido, verifícalo")]
    EmailInvalido,

    [Display(Name = "Ya aplicaste tu cambio", Description = "Ya aplicaste tu cambio")]
    CambioAplicado,

    [Display(Name = "Tu cuenta ha sido bloqueada", Description = "Has superado el  número de intentos permitido, por seguridad tu cuenta ha sido bloqueada, comunícate al 01 800  000 00")]
    IntentoDeSesion,

    [Display(Name = "Operador no válido", Description = "El operador no fue encontrado")]
    OperadorInvalido,

    [Display(Name = "Acceso denegado", Description = "No dispones de los privilegios necesarios para continuar")]
    OperadorNoAutorizado,

    [Display(Name = "Clientes no encontrado", Description = "El número de cliente que ingresaste es incorrecto")]
    ClienteNoEncontrado,

    [Display(Name = "Edición No Permitida", Description = "No puedes editar esta visita porque no fue registrada por ti.")]
    EdicionNoPermitidaVisita,

    [Display(Name = "Opción inválida", Description = "Opción inválida")]
    OpcionInvalida,

    [Display(Name = "Estatus no encontrado", Description = "Estatus no encontrado, por favor revisa y vuelve a capturarlo")]
    EstatusNoEncontrado,

    [Display(Name = "Imagen requerida", Description = "Se requiere al menos una imagen para continuar")]
    ImagenRequerida,

    [Display(Name = "Imagen no guardada. Verifica que la imagen sea válida y vuelve a intentarlo.", Description = "Imágenes no guardadas. Imágenes no guardadas. Verifica que todas las imágenes sean válidas y vuelve a intentarlo.")]
    ImagenNoGuardada,

    [Display(Name = "Número de imágenes inválido", Description = "Se permiten entre 1 y 6 imágenes.")]
    CantidadImagenesInvalida,

    [Display(Name = "Período no encontrado", Description = "No se encontró el período solicitado. Por favor, verifica la información y vuelve a intentarlo.")]
    PeriodoNoEncontrado,

    [Display(Name = "No se pudo realizar la actualización", Description = "Ocurrió un error al intentar actualizar.")]
    ActualizacionFallida,

    [Display(Name = "Celular usado en otra cuenta", Description = "Lo sentimos, el número de celular ingresado ya está en uso por otra cuenta. Por favor, intenta con un número de celular diferente.")]
    CelularUsado,

    [Display(Name = "Teléfono usado en otra cuenta", Description = "Lo sentimos, el número de teléfono ingresado ya está en uso por otra cuenta. Por favor, intenta con un número de teléfono diferente.")]
    TelefonoUsado,

    [Display(Name = "Correo electrónico usado en otra cuenta", Description = "Lo sentimos, el correo electrónico ingresado ya está en uso por otra cuenta. Por favor, intenta con un correo electrónico diferente.")]
    EmailUsado,

    [Display(Name = "Error al guardar la información. Intenta más tarde.", Description = "Hubo un problema al intentar guardar la información. Por favor, intenta nuevamente más tarde.")]
    ErrorDeGuardado,

    [Display(Name = "Error desconocido.",
             Description = "Ocurrió un error inesperado. Por favor, intenta de nuevo más tarde.")]
    ErrorDesconocido,

    [Display(Name = "Límite de imágenes superado", Description = "El número de imágenes permitidas ha sido superado.")]
    LimiteImgSuperado,

    [Display(Name = "El cliente no tiene celular registrado", Description = "Notamos que el cliente al que deseas enviar la encuesta no tiene un número de celular registrado en su cuenta.")]
    CllienteNoCelular,

    [Display(Name = "El cliente no tiene celular registrado", Description = "Notamos que el cliente al que deseas enviar la encuesta no tiene un correo electrónico registrado en su cuenta.")]
    CllienteNoEmail,

    [Display(Name = "El medio seleccionado no es válido", Description = "El medio de contacto seleccionado para el envío de la encuesta no es válido.")]
    MedioNoValidoEncuesta,

    [Display(Name = "Límite de responsables superado", Description = "El número de responsables permitidos ha sido superado.")]
    LimiteRepresentanteSuperado,

    [Display(Name = "Correo electrónico o número de celular requerido", Description = "Por favor, proporciona un correo electrónico o un número de celular válido.")]
    ContactoRequerido,

    [Display(Name = "Debe elegir un solo medio de contacto", Description = "Por favor, proporciona solo un medio de contacto: ya sea un correo electrónico o un número de celular. No puedes proporcionar ambos.")]
    EleccionUnicaContacto,

    [Display(Name = "Código activo", Description = "Tienes un código activo. Por favor, espera {minutos} minutos antes de solicitar otro.")]
    CodigoActivo,

    [Display(Name = "Código inválido", Description = "El código ingresado no es válido. Por favor, verifica el código e inténtalo de nuevo.")]
    CodigoInvalido,

    [Display(Name = "Código expirado", Description = "El código ingresado ha expirado. Por favor, solicita uno nuevo.")]
    CodigoExpirado,

    [Display(Name = "Registro incompleto", Description = "El registro del usuario no está completo. Es necesario que completes el proceso de registro para continuar.")]
    RegistroIncompleto,

    [Display(Name = "Producto no encontrado", Description = "Producto no encontrado, por favor revisa y vuelve a capturarlo.")]
    ProductoNoEncontrado,

    [Display(Name = "No hay promociones especiales", Description = "No se encontraron promociones especiales disponibles en este momento.")]
    NoHayPromocionesEspeciales,

    [Display(Name = "No hay promociones", Description = "No hay promociones activas en este momento.")]
    NoHayPromociones,

    [Display(Name = "Promocion no encontrada", Description = "Promocion no encontrada, por favor revisa y vuelve a capturarla.")]
    PromocionNoEncontrada,

    [Display(Name = "Carrito vacío", Description = "No tienes productos en tu carrito.")]
    CarritoVacio,

    [Display(Name = "Pedido no encontrado", Description = "El pedido que estás buscando no existe o ha sido eliminado.")]
    PedidoNoEncontrado,

    [Display(Name = "Pedido ya realizado", Description = "Este pedido ya ha sido completado y no puede ser modificado.")]
    PedidoYaRealizado,

    [Display(Name = "Encuesta no disponible", Description = "Encuesta no disponible")]
    EncuestaNoDisponible,

    [Display(Name = "Envio no registrado", Description = "No se ha enviado una encuesta")]
    EnvioNoRegistrado,

    [Display(Name = "Encuesta respondida", Description = "Ya has respondido la encuesta")]
    EncuestaRespondida,

    [Display(Name = "Usuario no registrado", Description = "Usuario no registrado")]
    UsuarioNoRegistrado,

    [Display(Name = "*Campos obligatorios", Description = "*Campo obligatorio")]
    CamposObligatorios,

    [Display(Name = "Cliente no autorizado", Description = "El número de cliente no está autorizado")]
    ClienteNoAutorizado,

    [Display(Name = "Cliente registrado", Description = "Tu número de cliente ya está registrado")]
    ClienteRegistrado,

    [Display(Name = "Error coincidencia password ", Description = "Las contraseñas no coinciden")]
    ErrorPasswordDistinta,

    [Display(Name = "Faltan respuestas", Description = "Por favor, asegúrese de completar todas las respuestas obligatorias.")]
    RespuestasFaltantes,

    [Display(Name = "Evaluación de preciador", Description = "La evaluación ya ha sido completada y en 48 hrs. recibirás comentarios")]
    EvalEnviada,

    [Display(Name = "Información incompleta", Description = "Parece que algunos datos no están completos")]
    DatosIncompletos,

    [Display(Name = "Sin Evaluación Realizada", Description = "No se encontró ninguna evaluación realizada en este mes.")]
    SinEvaluacion,

    [Display(Name = "Código Postal No Encontrado", Description = "El código postal proporcionado no fue encontrado o no existe.")]
    CodigoPostalNoEncontrado,

    [Display(Name = "Período de Inscripción Finalizado", Description = "El período de inscripción ha finalizado y ya no es posible registrarse.")]
    InscripcionFinalizada
}
