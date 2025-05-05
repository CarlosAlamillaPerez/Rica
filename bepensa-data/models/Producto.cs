using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class Producto
{
    public int Id { get; set; }

    public string Sku { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Sabor { get; set; } = null!;

    public int IdMarca { get; set; }

    public int? IdCategoriaDeProducto { get; set; }

    public string? Segmento { get; set; }

    public string? Presentacion { get; set; }

    public decimal? Unidades { get; set; }

    public int IdEstatus { get; set; }

    public DateTime FechaReg { get; set; }

    public int IdOperadorReg { get; set; }

    public DateTime? FechaMod { get; set; }

    public int? IdOperadorMod { get; set; }

    public virtual ICollection<DetalleVenta> DetalleVenta { get; set; } = new List<DetalleVenta>();

    public virtual ICollection<EmpaquesProducto> EmpaquesProductos { get; set; } = new List<EmpaquesProducto>();

    public virtual Estatus IdEstatusNavigation { get; set; } = null!;

    public virtual Marca IdMarcaNavigation { get; set; } = null!;

    public virtual Operadore? IdOperadorModNavigation { get; set; }

    public virtual Operadore IdOperadorRegNavigation { get; set; } = null!;
}
