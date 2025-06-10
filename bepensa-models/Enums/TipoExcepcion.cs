using System.ComponentModel.DataAnnotations;

namespace bepensa_models.Enums;

public enum TipoExcepcion
{
    [Display(Name = "Token no encontrado", Description = "Token faltante. La solicitud no puede ser procesada sin el identificador de seguridad.")]
    TokenNoEncontrado,

    [Display(Name = "Usuario no identificado", Description = "El usuario no ha sido identificado.")]
    UsuarioNoIdentificado,

    [Display(Name = "Periodo no identificado", Description = "El periodo no ha sido identificado.")]
    PeriodoNoIdentificado,

    [Display(Name = "Tipo de pregunta no identificado", Description = "El tipo de pregunta no ha sido desarrollado o se ingresó un valor inválido.")]
    TipoPreguntaNoIdentificado,

    [Display(Name = "Error de canje", Description = "Error de canje.")]
    MKTError,
}
