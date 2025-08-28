using System;
using System.Collections.Generic;

namespace bepensa_data.modelsRD;

public partial class BitacoraDeUsuario
{
    public int Id { get; set; }

    public long IdUsuario { get; set; }

    public int IdTdo { get; set; }

    public DateTime FechaReg { get; set; }

    public int? IdOperdorReg { get; set; }

    public string? Notas { get; set; }

    public virtual Operadore? IdOperdorRegNavigation { get; set; }

    public virtual TiposDeOperacion IdTdoNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
