using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class BitacoraDeWhatsApp
{
    public long Id { get; set; }

    public int IdTipoWhatsApp { get; set; }

    public int IdPeriodo { get; set; }

    public int IdUsuario { get; set; }

    public string Celular { get; set; } = null!;

    public string? Parametros { get; set; }

    public string Mensaje { get; set; } = null!;

    public int IdEstatus { get; set; }

    public string? EstatusApi { get; set; }

    public string? JsonRequest { get; set; }

    public string? JsonResponse { get; set; }

    public DateTime FechaReg { get; set; }

    public int IdOperadorReg { get; set; }

    public DateTime? FechaMod { get; set; }

    public int? IdOperadorMod { get; set; }

    public virtual Estatus IdEstatusNavigation { get; set; } = null!;

    public virtual Operadore? IdOperadorModNavigation { get; set; }

    public virtual Operadore IdOperadorRegNavigation { get; set; } = null!;

    public virtual Periodo IdPeriodoNavigation { get; set; } = null!;

    public virtual TiposWhatsApp IdTipoWhatsAppNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
