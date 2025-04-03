using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class Periodo
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public DateOnly Fecha { get; set; }

    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();

    public virtual ICollection<CuotasDeCompra> CuotasDeCompras { get; set; } = new List<CuotasDeCompra>();

    public virtual ICollection<EstadosDeCuentum> EstadosDeCuenta { get; set; } = new List<EstadosDeCuentum>();

    public virtual ICollection<HistoricoVenta> HistoricoVenta { get; set; } = new List<HistoricoVenta>();

    public virtual ICollection<ImagenesPromocione> ImagenesPromociones { get; set; } = new List<ImagenesPromocione>();

    public virtual ICollection<MetasMensuale> MetasMensuales { get; set; } = new List<MetasMensuale>();

    public virtual ICollection<PorcentajesIncrementoVentum> PorcentajesIncrementoVenta { get; set; } = new List<PorcentajesIncrementoVentum>();
}
