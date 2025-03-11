using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class ConceptosDeAcumulacion
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int IdTipoDeMovimiento { get; set; }

    public virtual ICollection<EstadosDeCuentum> EstadosDeCuenta { get; set; } = new List<EstadosDeCuentum>();

    public virtual ICollection<SubconceptosDeAcumulacion> SubconceptosDeAcumulacions { get; set; } = new List<SubconceptosDeAcumulacion>();
}
