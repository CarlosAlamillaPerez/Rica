using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class UsuariosPrueba
{
    public int Id { get; set; }

    public int IdUsuario { get; set; }

    public string Password { get; set; } = null!;

    public string? Comentarios { get; set; }

    public string? Cuc { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
