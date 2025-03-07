using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class BitacoraDeOperadore
{
    public int Id { get; set; }

    public int IdOperador { get; set; }

    public int IdTipoDeOperacion { get; set; }

    public DateTime FechaReg { get; set; }

    public int? IdOperadorAftd { get; set; }

    public string? Notas { get; set; }

    public int? IdUsuarioAftd { get; set; }

    public virtual Operadore? IdOperadorAftdNavigation { get; set; }

    public virtual Operadore IdOperadorNavigation { get; set; } = null!;

    public virtual TiposDeOperacion IdTipoDeOperacionNavigation { get; set; } = null!;

    public virtual Usuario? IdUsuarioAftdNavigation { get; set; }
}
