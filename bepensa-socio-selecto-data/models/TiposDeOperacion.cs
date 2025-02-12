using System;
using System.Collections.Generic;

namespace bepensa_socio_selecto_data.models;

public partial class TiposDeOperacion
{
    public int Id { get; set; }

    public int IdSeccion { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<BitacoraDeFuerzasDeVentum> BitacoraDeFuerzasDeVenta { get; set; } = new List<BitacoraDeFuerzasDeVentum>();

    public virtual ICollection<BitacoraDeOperadore> BitacoraDeOperadores { get; set; } = new List<BitacoraDeOperadore>();

    public virtual ICollection<BitacoraDeUsuario> BitacoraDeUsuarios { get; set; } = new List<BitacoraDeUsuario>();

    public virtual Seccione IdSeccionNavigation { get; set; } = null!;
}
