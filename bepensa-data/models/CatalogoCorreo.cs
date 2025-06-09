using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class CatalogoCorreo
{
    public int Id { get; set; }

    public string Codigo { get; set; } = null!;

    public string Titulo { get; set; } = null!;

    public string Html { get; set; } = null!;

    public string? Cco { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public DateTime FechaUltimaModificacion { get; set; }

    public int IdEstatus { get; set; }

    public bool? ReEnviable { get; set; }

    public string? Comentarios { get; set; }

    public virtual Estatus IdEstatusNavigation { get; set; } = null!;
}
