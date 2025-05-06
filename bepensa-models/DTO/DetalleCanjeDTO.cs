using Microsoft.VisualBasic;

namespace bepensa_models.DTO;

public class DetalleCanjeDTO
{
    public long Id { get; set; }

    public string Titular { get; set; } = null!;

    public string? Folio { get; set; } = null!;

    public DateOnly FechaCanje { get; set; }

    public DateOnly? FechaPromesa { get; set; }

    public int Puntos { get; set; }

    public string Premio { get; set; } = null!;

    public int IdTipoDeEnvio { get; set; }

    public int IdTipoDePremio { get; set; }

    public int? IdTipoTransaccion { get; set; }

    public string Nombre { get; set; } = null!;

    public string Estatus { get; set; } = null!;

    public string Solicitante { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public int Cantidad { get; set; }

    public string EnviadoCedi { get; set; } = null!;

    public string? Observaciones { get; set; }

    public string? Referencias { get; set; }

    public string? Guia { get; set; }

    public string? Mensajeria { get; set; }

    public DateOnly? FechaDeEntrega { get; set; }

    public string DisponibleFeria { get; set; } = null!;

    public string TipoEnvioCodigo { get; set; } = null!;

    public string OrigenCanje { get; set; } = null!;

    public string? MetodoDeEntrega { get; set; }
    public string? Email { get; set; }

    public int? IdEstatusRedencion { get; set; }

    public string? EstatusRedencion { get; set; }
}
