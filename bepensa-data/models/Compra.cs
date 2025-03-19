using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class Compra
{
    public long Id { get; set; }

    public int IdArchivoDeCarga { get; set; }

    public int IdUsuario { get; set; }

    public int IdPeriodo { get; set; }

    public int IdProducto { get; set; }

    public decimal CajasFisicas { get; set; }

    public decimal CajasUnitarias { get; set; }

    public DateOnly Fecha { get; set; }

    public decimal Importe { get; set; }

    public DateTime FechaReg { get; set; }

    public int IdOperadorReg { get; set; }

    public virtual ArchivosDeCarga IdArchivoDeCargaNavigation { get; set; } = null!;

    public virtual Operadore IdOperadorRegNavigation { get; set; } = null!;

    public virtual Periodo IdPeriodoNavigation { get; set; } = null!;

    public virtual Producto IdProductoNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
