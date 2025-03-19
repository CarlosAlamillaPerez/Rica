using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class Carrusel
{
    public int Id { get; set; }

    public string Imagen { get; set; } = null!;

    public string? RutaImagen { get; set; }

    public string? Link { get; set; }

    public int IdVista { get; set; }

    public DateTime VigenciaInicio { get; set; }

    public DateTime VigenciaFin { get; set; }

    public int Orden { get; set; }

    public int IdTipoDeAccion { get; set; }

    public int IdOrigen { get; set; }

    public int? IdEstatus { get; set; }

    public int IdPrograma { get; set; }

    public DateTime FechaReg { get; set; }

    public int IdOperadorReg { get; set; }

    public DateTime? FechaMod { get; set; }

    public int? IdOperadorMod { get; set; }

    public virtual Estatus? IdEstatusNavigation { get; set; }

    public virtual Operadore? IdOperadorModNavigation { get; set; }

    public virtual Operadore IdOperadorRegNavigation { get; set; } = null!;

    public virtual Origene IdOrigenNavigation { get; set; } = null!;

    public virtual Programa IdProgramaNavigation { get; set; } = null!;

    public virtual TiposDeAccione IdTipoDeAccionNavigation { get; set; } = null!;

    public virtual Vista IdVistaNavigation { get; set; } = null!;
}
