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

    public virtual ICollection<CategoriasDeProducto> CategoriasDeProductos { get; set; } = new List<CategoriasDeProducto>();

    public virtual ICollection<DetallesDeEstadosDeCuentum> DetallesDeEstadosDeCuenta { get; set; } = new List<DetallesDeEstadosDeCuentum>();

    public virtual ConceptosDeAcumulacion IdConceptoDeAcumulacionNavigation { get; set; } = null!;

    public virtual Operadore? IdOperadorRegNavigation { get; set; }

    public virtual ICollection<ProductosSelecto> ProductosSelectos { get; set; } = new List<ProductosSelecto>();

    public virtual ICollection<PuntajesDeSubconceptosDeAcumulacion> PuntajesDeSubconceptosDeAcumulacions { get; set; } = new List<PuntajesDeSubconceptosDeAcumulacion>();
}
