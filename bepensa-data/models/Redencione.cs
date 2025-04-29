using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class Redencione
{
    public long Id { get; set; }

    public int? IdUsuario { get; set; }

    public long IdMovimiento { get; set; }

    public int IdPremio { get; set; }

    public int Puntos { get; set; }

    public int Cantidad { get; set; }

    public string Solicitante { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public string TelefonoAlterno { get; set; } = null!;

    public DateOnly? FechaPromesa { get; set; }

    public DateOnly? FechaDeEntrega { get; set; }

    public string? Calle { get; set; }

    public string? NumeroExterior { get; set; }

    public string? CodigoPostal { get; set; }

    public string? Ciudad { get; set; }

    public string? Municipio { get; set; }

    public string? Estado { get; set; }

    public string? Referencias { get; set; }

    public string? Guia { get; set; }

    public string? FolioRms { get; set; }

    public string? PedidoRms { get; set; }

    public int? IdMensajeria { get; set; }

    public string? MetodoDeEntrega { get; set; }

    public int IdOrigen { get; set; }

    public string? Observaciones { get; set; }

    public DateTime FechaReg { get; set; }

    public int? IdOperadorReg { get; set; }

    public string? NumeroInterior { get; set; }

    public int? IdColonia { get; set; }

    public string? CalleInicio { get; set; }

    public string? CalleFin { get; set; }

    public int IdEstatusRedencion { get; set; }

    public virtual ICollection<CodigosRedimido> CodigosRedimidos { get; set; } = new List<CodigosRedimido>();

    public virtual EstatusDeRedencione IdEstatusRedencionNavigation { get; set; } = null!;

    public virtual Mensajeria? IdMensajeriaNavigation { get; set; }

    public virtual Movimiento IdMovimientoNavigation { get; set; } = null!;

    public virtual Operadore? IdOperadorRegNavigation { get; set; }

    public virtual Origene IdOrigenNavigation { get; set; } = null!;

    public virtual Premio IdPremioNavigation { get; set; } = null!;

    public virtual Usuario? IdUsuarioNavigation { get; set; }

    public virtual ICollection<SeguimientoDeRedencione> SeguimientoDeRedenciones { get; set; } = new List<SeguimientoDeRedencione>();
}
