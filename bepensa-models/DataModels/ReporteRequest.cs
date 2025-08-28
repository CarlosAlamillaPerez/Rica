using System.ComponentModel.DataAnnotations;

namespace bepensa_models.DataModels;

public class ReporteRequest
{
    [Required(ErrorMessage = "Ingrese una Fecha Inicial")]
    [DataType(DataType.Date)]
    public DateOnly? FechaInicial { get; set; } = null;

    [Required(ErrorMessage = "Ingrese una Fecha Final")]
    [DataType(DataType.Date)]
    public DateOnly? FechaFinal { get; set; } = null;

    public int IdReporte { get; set; }

    public int IdPerfil { get; set; }

    public string? NombreReporte { get; set; }
}
