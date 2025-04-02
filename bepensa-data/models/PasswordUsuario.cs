using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class PasswordUsuario
{
    public int Id { get; set; }

    public string Cuc { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTime FechaReg { get; set; }
}
