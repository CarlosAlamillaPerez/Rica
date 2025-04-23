using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class BitacoraFuerzaVentum
{
    public int Id { get; set; }

    public int IdFdv { get; set; }

    public int IdTipoDeOperacion { get; set; }

    public DateTime FechaReg { get; set; }

    public string? Notas { get; set; }

    public int? IdUsuarioAftd { get; set; }

    public virtual FuerzaVentum IdFdvNavigation { get; set; } = null!;

    public virtual TiposDeOperacion IdTipoDeOperacionNavigation { get; set; } = null!;

    public virtual Usuario? IdUsuarioAftdNavigation { get; set; }
}
