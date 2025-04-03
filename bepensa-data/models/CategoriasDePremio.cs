using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class CategoriasDePremio
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public string? Imgurl { get; set; }

    public int IdEstatus { get; set; }

    public DateTime Fechareg { get; set; }

    public bool Visible { get; set; }

    public int Ordenar { get; set; }

    public bool Digital { get; set; }

    public string? ClaveCategoria { get; set; }

    public string? Estilos { get; set; }

    public string? FondoColor { get; set; }

    public string? LetraColor { get; set; }

    public virtual Estatus IdEstatusNavigation { get; set; } = null!;

    public virtual ICollection<Premio> Premios { get; set; } = new List<Premio>();
}
