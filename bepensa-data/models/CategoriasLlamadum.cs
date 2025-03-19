using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class CategoriasLlamadum
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int IdEstatus { get; set; }

    public virtual Estatus IdEstatusNavigation { get; set; } = null!;

    public virtual ICollection<SubcategoriasLlamadum> SubcategoriasLlamada { get; set; } = new List<SubcategoriasLlamadum>();
}
