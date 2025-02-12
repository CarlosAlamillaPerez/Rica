using System;
using System.Collections.Generic;

namespace bepensa_socio_selecto_data.models;

public partial class EstadosDeCuentum
{
    public long Id { get; set; }

    public long IdNegocio { get; set; }

    public int IdPeriodo { get; set; }

    public int IdConceptoDeAcumulacion { get; set; }

    public int Puntos { get; set; }

    public DateTime FechaReg { get; set; }

    public int IdOperadorReg { get; set; }

    public virtual ICollection<DetallesDeEstadosDeCuentum> DetallesDeEstadosDeCuenta { get; set; } = new List<DetallesDeEstadosDeCuentum>();

    public virtual ConceptosDeAcumulacion IdConceptoDeAcumulacionNavigation { get; set; } = null!;

    public virtual Negocio IdNegocioNavigation { get; set; } = null!;

    public virtual Operadore IdOperadorRegNavigation { get; set; } = null!;

    public virtual Periodo IdPeriodoNavigation { get; set; } = null!;
}
