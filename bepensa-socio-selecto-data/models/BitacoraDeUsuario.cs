using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class BitacoraDeUsuario
{
    public int Id { get; set; }

    public int IdUsuario { get; set; }

    public int IdTipoDeOperacion { get; set; }

    public DateTime FechaReg { get; set; }

    public int? IdOperdorReg { get; set; }

    public string? Notas { get; set; }

    public virtual Operadore? IdOperdorRegNavigation { get; set; }

    public virtual TiposDeOperacion IdTipoDeOperacionNavigation { get; set; } = null!;
}
