using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class SubcategoriasLlamadum
{
    public int Id { get; set; }

    public int IdCategoriaLlamada { get; set; }

    public string Nombre { get; set; } = null!;

    public int IdEstatus { get; set; }

    public virtual CategoriasLlamadum IdCategoriaLlamadaNavigation { get; set; } = null!;

    public virtual Estatus IdEstatusNavigation { get; set; } = null!;

    public virtual ICollection<Llamada> Llamada { get; set; } = new List<Llamada>();
}
