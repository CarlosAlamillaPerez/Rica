using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class Canale
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int IdEstatus { get; set; }

    public DateTime FechaReg { get; set; }

    public int IdOperadorReg { get; set; }

    public virtual ICollection<ConceptosDeAcumulacion> ConceptosDeAcumulacions { get; set; } = new List<ConceptosDeAcumulacion>();

    public virtual ICollection<FuerzaVentum> FuerzaVenta { get; set; } = new List<FuerzaVentum>();

    public virtual Operadore IdOperadorRegNavigation { get; set; } = null!;

    public virtual ICollection<ImagenesPromocione> ImagenesPromociones { get; set; } = new List<ImagenesPromocione>();

    public virtual ICollection<PorcentajesIncrementoVentum> PorcentajesIncrementoVenta { get; set; } = new List<PorcentajesIncrementoVentum>();

    public virtual ICollection<PrefijosRm> PrefijosRms { get; set; } = new List<PrefijosRm>();

    public virtual ICollection<Programa> Programas { get; set; } = new List<Programa>();

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
