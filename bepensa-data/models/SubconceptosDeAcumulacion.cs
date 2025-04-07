using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class SubconceptosDeAcumulacion
{
    public int Id { get; set; }

    public int IdConceptoDeAcumulacion { get; set; }

    public string Nombre { get; set; } = null!;

    public DateTime FechaReg { get; set; }

    public int? IdOperadorReg { get; set; }

    public DateTime? FechaMod { get; set; }

    public int? IdOperadorMod { get; set; }

    public string? FondoColor { get; set; }

    public string? LetraColor { get; set; }

    public virtual ICollection<CategoriasDeProducto> CategoriasDeProductos { get; set; } = new List<CategoriasDeProducto>();

    public virtual ConceptosDeAcumulacion IdConceptoDeAcumulacionNavigation { get; set; } = null!;

    public virtual Operadore? IdOperadorModNavigation { get; set; }

    public virtual Operadore? IdOperadorRegNavigation { get; set; }

    public virtual ICollection<PuntajesDeSubconceptosDeAcumulacion> PuntajesDeSubconceptosDeAcumulacions { get; set; } = new List<PuntajesDeSubconceptosDeAcumulacion>();

    public virtual ICollection<SegmentosAcumulacion> SegmentosAcumulacions { get; set; } = new List<SegmentosAcumulacion>();
}
