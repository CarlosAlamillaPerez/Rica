using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class CategoriasDeProducto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int? IdSubconceptoDeAcumulacion { get; set; }

    public int? IdEstatus { get; set; }

    public DateTime? FechaReg { get; set; }

    public int IdOperadorReg { get; set; }

    public DateTime? FechaMod { get; set; }

    public int? IdOperadorMod { get; set; }

    public virtual Estatus? IdEstatusNavigation { get; set; }

    public virtual Operadore? IdOperadorModNavigation { get; set; }

    public virtual Operadore IdOperadorRegNavigation { get; set; } = null!;

    public virtual SubconceptosDeAcumulacion? IdSubconceptoDeAcumulacionNavigation { get; set; }
}
