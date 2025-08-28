using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class HistorialCompraPunto
{
    public int Id { get; set; }

    public int IdUsuario { get; set; }

    public int IdTipoPago { get; set; }

    public int PuntosTotales { get; set; }

    public int PuntosFaltantes { get; set; }

    public decimal Monto { get; set; }

    public string? Referencia { get; set; }

    public string? Nombre { get; set; }

    public string? Email { get; set; }

    public string? Telefono { get; set; }

    public string? TelefonoAlterno { get; set; }

    public string? Calle { get; set; }

    public string? NumeroExterior { get; set; }

    public string? NumeroInterior { get; set; }

    public string? CodigoPostal { get; set; }

    public int? IdColonia { get; set; }

    public string? Ciudad { get; set; }

    public string? CalleInicio { get; set; }

    public string? CalleFin { get; set; }

    public string? Referencias { get; set; }

    public Guid? IdTransaccionLog { get; set; }

    public int IdOrigen { get; set; }

    public int IdEstatusPago { get; set; }

    public DateTime FechaReg { get; set; }

    public int? IdOperadorReg { get; set; }

    public DateTime? FechaMod { get; set; }

    public int? IdOperadorMod { get; set; }

    public string? IdOpenPay { get; set; }

    public virtual Colonia? IdColoniaNavigation { get; set; }

    public virtual EstatusPago IdEstatusPagoNavigation { get; set; } = null!;

    public virtual Operadore? IdOperadorModNavigation { get; set; }

    public virtual Operadore? IdOperadorRegNavigation { get; set; }

    public virtual Origene IdOrigenNavigation { get; set; } = null!;

    public virtual TiposPago IdTipoPagoNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
