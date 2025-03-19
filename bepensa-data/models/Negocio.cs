using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class Negocio
{
    public long Id { get; set; }

    public string RazonSocial { get; set; } = null!;

    public string? TelefonoFijo { get; set; }

    public int IdEmbotelladora { get; set; }

    public int IdCedi { get; set; }

    public int IdSupervisor { get; set; }

    public string? Calle { get; set; }

    public string? Numero { get; set; }

    public string? CodigoPostal { get; set; }

    public string? Barrio { get; set; }

    public string? Municipio { get; set; }

    public string? Provincia { get; set; }

    public string? Referencias { get; set; }

    public long Cuc { get; set; }

    public int IdEstatus { get; set; }

    public DateTime FechaReg { get; set; }

    public int? IdOperadorReg { get; set; }

    public int? IdOperadorMod { get; set; }

    public bool Registro { get; set; }

    public virtual ICollection<CuotasDeCompra> CuotasDeCompras { get; set; } = new List<CuotasDeCompra>();

    public virtual ICollection<EstadosDeCuentum> EstadosDeCuenta { get; set; } = new List<EstadosDeCuentum>();

    public virtual ICollection<FuerzasDeVentaNegocio> FuerzasDeVentaNegocios { get; set; } = new List<FuerzasDeVentaNegocio>();

    public virtual Cedi IdCediNavigation { get; set; } = null!;

    public virtual Embotelladora IdEmbotelladoraNavigation { get; set; } = null!;

    public virtual Estatus IdEstatusNavigation { get; set; } = null!;

    public virtual Operadore? IdOperadorModNavigation { get; set; }

    public virtual Operadore? IdOperadorRegNavigation { get; set; }

    public virtual Supervisore IdSupervisorNavigation { get; set; } = null!;
}
