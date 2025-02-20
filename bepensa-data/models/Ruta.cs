using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class Ruta
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int IdEstatus { get; set; }

    public DateTime FechaReg { get; set; }

    public int IdOperadorReg { get; set; }

    public virtual Operadore IdOperadorRegNavigation { get; set; } = null!;

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
