using System;
using System.Collections.Generic;

namespace bepensa_data.modelsRD;

public partial class CategoriasDeLlamadum
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int IdEstatus { get; set; }

    public virtual Estatus IdEstatusNavigation { get; set; } = null!;

    public virtual ICollection<Llamada> Llamada { get; set; } = new List<Llamada>();

    public virtual ICollection<SubcategoriasDeLlamadum> SubcategoriasDeLlamada { get; set; } = new List<SubcategoriasDeLlamadum>();
}
