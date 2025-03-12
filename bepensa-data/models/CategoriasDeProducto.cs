using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class CategoriasDeProducto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int? IdSubconceptoDeAcumulacion { get; set; }

    public int? IdEstatus { get; set; }

    public DateTime? FechaReg { get; set; }

    public virtual ICollection<CuotasDeCompra> CuotasDeCompras { get; set; } = new List<CuotasDeCompra>();

    public virtual Estatus? IdEstatusNavigation { get; set; }

    public virtual SubconceptosDeAcumulacion? IdSubconceptoDeAcumulacionNavigation { get; set; }

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
