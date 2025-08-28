using System;
using System.Collections.Generic;

namespace bepensa_data.modelsRD;

public partial class ProductosSelecto
{
    public int Id { get; set; }

    public int IdSubconceptoDeAcumulacion { get; set; }

    public int IdProducto { get; set; }

    public virtual Producto IdProductoNavigation { get; set; } = null!;

    public virtual SubconceptosDeAcumulacion IdSubconceptoDeAcumulacionNavigation { get; set; } = null!;
}
