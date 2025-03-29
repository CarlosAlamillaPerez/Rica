using System.ComponentModel.DataAnnotations;

namespace bepensa_models.Enums;

public enum TipoExcepcion
{
    [Display(Name = "Token no encontrado", Description = "Token faltante. La solicitud no puede ser procesada sin el identificador de seguridad.")]
    TokenNoEncontrado
}
