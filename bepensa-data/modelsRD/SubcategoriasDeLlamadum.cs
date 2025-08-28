using System;
using System.Collections.Generic;

namespace bepensa_data.modelsRD;

public partial class SubcategoriasDeLlamadum
{
    public int Id { get; set; }

    public int IdCdl { get; set; }

    public string Nombre { get; set; } = null!;

    public int IdEstatus { get; set; }

    public virtual CategoriasDeLlamadum IdCdlNavigation { get; set; } = null!;

    public virtual Estatus IdEstatusNavigation { get; set; } = null!;
}
