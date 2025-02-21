using System.ComponentModel.DataAnnotations;

namespace bepensa_models.Enums;

/// <summary>
/// Tomar de referencia el Name del enum que apunta a la tabla de parametros en su columna de Tag.
/// </summary>
public enum TipoParametro
{
    [Display(Name = "FechaInscripcion")]
    FechaInscripcion
}
