using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class TiposDeMovimiento
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public short Valor { get; set; }

    public virtual ICollection<Saldo> Saldos { get; set; } = new List<Saldo>();
}
