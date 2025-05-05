using bepensa_data.models;
using Microsoft.EntityFrameworkCore;

namespace bepensa_data.StoredProcedures.Models;

[Keyless]
public class CanjeCTE
{
    public long Id { get; set; }

    public int IdUsuario { get; set; }

    public string Titular { get; set; } = null!;

    public DateOnly FechaCanje { get; set; }

    public int IdPremio { get; set; }

    public string Premio { get; set; } = null!;

    public int IdTipoDePremio { get; set; }

    public int Puntos { get; set; }

    public int Cantidad { get; set; }

    public string? Folio { get; set; }

    public DateOnly? FechaPromesa { get; set; }

    public string Estatus { get; set; } = null!;

    public string? MetodoDeEntrega { get; set; }

    public string? Email { get; set; }

    public string? Telefono { get; set; }

    public string? TelefonoAlterno { get; set; }

    public string Direccion { get; set; } = null!;

    public string? Referencias { get; set; }

    public string? Guia { get; set; }

    public int? IdMensajeria { get; set; }

    public string? Mensajeria { get; set; }

    public DateOnly? FechaDeEntrega { get; set; }

    public string Solicitante { get; set; } = null!;

    public string? PedidoRMS { get; set; }

    public int? IdOrigen { get; set; }

    public string? Observaciones { get; set; }

    public int? IdEstatusRedencion { get; set; }

    public string? EstatusRedencion { get; set; }
}
