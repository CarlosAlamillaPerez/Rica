using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class Marca
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public DateTime FechaReg { get; set; }

    public int IdOperadorReg { get; set; }

    public DateTime? FechaMod { get; set; }

    public int? IdOperadorMod { get; set; }

    public virtual Operadore? IdOperadorModNavigation { get; set; }

    public virtual Operadore IdOperadorRegNavigation { get; set; } = null!;

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
