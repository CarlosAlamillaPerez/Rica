﻿using System;
using System.Collections.Generic;

namespace bepensa_socio_selecto_data.models;

public partial class Canale
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int IdEstatus { get; set; }

    public DateTime FechaReg { get; set; }

    public int IdOperadorReg { get; set; }

    public virtual Operadore IdOperadorRegNavigation { get; set; } = null!;

    public virtual ICollection<Programa> Programas { get; set; } = new List<Programa>();
}
