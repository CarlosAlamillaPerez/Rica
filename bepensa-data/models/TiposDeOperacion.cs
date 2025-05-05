﻿using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class TiposDeOperacion
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public DateTime FechaReg { get; set; }

    public int IdOperadorReg { get; set; }

    public virtual ICollection<BitacoraDeOperadore> BitacoraDeOperadores { get; set; } = new List<BitacoraDeOperadore>();

    public virtual ICollection<BitacoraDeUsuario> BitacoraDeUsuarios { get; set; } = new List<BitacoraDeUsuario>();

    public virtual ICollection<BitacoraFuerzaVentum> BitacoraFuerzaVenta { get; set; } = new List<BitacoraFuerzaVentum>();
}
