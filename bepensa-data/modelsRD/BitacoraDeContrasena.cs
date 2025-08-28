using System;
using System.Collections.Generic;

namespace bepensa_data.modelsRD;

public partial class BitacoraDeContrasena
{
    public long Id { get; set; }

    public long? IdUsuario { get; set; }

    public int? IdOperador { get; set; }

    public byte[] Password { get; set; } = null!;

    public string Origen { get; set; } = null!;

    public DateTime FechaReg { get; set; }

    public virtual Operadore? IdOperadorNavigation { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
