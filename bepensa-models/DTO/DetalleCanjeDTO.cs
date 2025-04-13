using Microsoft.VisualBasic;

namespace bepensa_models.DTO;

public class DetalleCanjeDTO
{
    public int Id { get; set; }

    public string? Folio { get; set; } = null!;

    public DateOnly FechaCanje { get; set; }

    public int Puntos { get; set; }

    public string Premio { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Estatus { get; set; } = null!;

    public string Solicitante { get; set; } = null!;

    
    public string Direccion { get; set; } = null!;


    public int? IdTipoCanaje { get; set; }

    public string? TipoCanje { get; set; }

    public int Cantidad { get; set; }

    public string EnviadoCedi { get; set; } = null!;

    public string? Observaciones { get; set; }

    public string? Referencias { get; set; }

    public string DisponibleFeria { get; set; } = null!;

    public string TipoEnvioCodigo { get; set; } = null!;

    public string OrigenCanje { get; set; } = null!;

}
