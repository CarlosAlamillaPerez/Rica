﻿using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class Programa
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? PrefijoRms { get; set; }

    public int IdEstatus { get; set; }

    public DateTime? FechaReg { get; set; }

    public int IdOperadorReg { get; set; }

    public int IdCanal { get; set; }

    public int BitValue { get; set; }

    public virtual Canale IdCanalNavigation { get; set; } = null!;

    public virtual Estatus IdEstatusNavigation { get; set; } = null!;

    public virtual Operadore IdOperadorRegNavigation { get; set; } = null!;

    public virtual ICollection<PuntajesDeSubconceptosDeAcumulacion> PuntajesDeSubconceptosDeAcumulacions { get; set; } = new List<PuntajesDeSubconceptosDeAcumulacion>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
