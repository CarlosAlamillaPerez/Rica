using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class DetalleVenta
{
    public long Id { get; set; }

    public int IdVenta { get; set; }

    public int IdProducto { get; set; }

    public int IdSuborigenVenta { get; set; }

    public double Cf { get; set; }

    public double Cu { get; set; }

    public decimal Importe { get; set; }

    public int IdEstatus { get; set; }

    public DateTime FechaReg { get; set; }

    public int IdOperadorReg { get; set; }

    public DateTime? FechaMod { get; set; }

    public int? IdOperadorMod { get; set; }

    public virtual Estatus IdEstatusNavigation { get; set; } = null!;

    public virtual Operadore? IdOperadorModNavigation { get; set; }

    public virtual Operadore IdOperadorRegNavigation { get; set; } = null!;

    public virtual Producto IdProductoNavigation { get; set; } = null!;

    public virtual SuborigenesVentum IdSuborigenVentaNavigation { get; set; } = null!;

    public virtual Venta IdVentaNavigation { get; set; } = null!;
}
