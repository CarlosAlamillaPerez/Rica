using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class RolesFdv
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Busqueda { get; set; } = null!;

    public virtual ICollection<FuerzaVentum> FuerzaVenta { get; set; } = new List<FuerzaVentum>();
}
