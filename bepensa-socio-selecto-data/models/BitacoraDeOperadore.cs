using System;
using System.Collections.Generic;

namespace bepensa_socio_selecto_data.models;

public partial class BitacoraDeOperadore
{
    public int Id { get; set; }

    public int IdOperador { get; set; }

    public int IdTipoDeOperacion { get; set; }

    public DateTime FechaReg { get; set; }

    public int? IdOperadorReg { get; set; }

    public string? Notas { get; set; }

    public virtual Operadore IdOperadorNavigation { get; set; } = null!;

    public virtual Operadore? IdOperadorRegNavigation { get; set; }

    public virtual TiposDeOperacion IdTipoDeOperacionNavigation { get; set; } = null!;
}
