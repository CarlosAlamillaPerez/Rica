using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class Colonia
{
    public int Id { get; set; }

    public int IdMunicipio { get; set; }

    public string Colonia1 { get; set; } = null!;

    public string? Cp { get; set; }

    public string? Ciudad { get; set; }

    public DateTime Fechareg { get; set; }

    public virtual Municipio IdMunicipioNavigation { get; set; } = null!;

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
