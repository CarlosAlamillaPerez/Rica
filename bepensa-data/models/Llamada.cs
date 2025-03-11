using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class Llamada
{
    public int Id { get; set; }

    public int? IdPadre { get; set; }

    public string? Tema { get; set; }

    public int? IdUsuario { get; set; }

    public string Nombre { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public string Comentario { get; set; } = null!;

    public int IdTipoLlamada { get; set; }

    public int IdSubcategoriaLlamada { get; set; }

    public int IdEstatusLlamada { get; set; }

    public DateTime FechaReg { get; set; }

    public int IdOperadorReg { get; set; }

    public int? IdOperadorMod { get; set; }

    public DateTime? FechaMod { get; set; }

    public virtual EstatusDeLlamadum IdEstatusLlamadaNavigation { get; set; } = null!;

    public virtual Operadore? IdOperadorModNavigation { get; set; }

    public virtual Operadore IdOperadorRegNavigation { get; set; } = null!;

    public virtual Llamada? IdPadreNavigation { get; set; }

    public virtual SubcategoriasLlamadum IdSubcategoriaLlamadaNavigation { get; set; } = null!;

    public virtual TiposLlamadum IdTipoLlamadaNavigation { get; set; } = null!;

    public virtual Usuario? IdUsuarioNavigation { get; set; }

    public virtual ICollection<Llamada> InverseIdPadreNavigation { get; set; } = new List<Llamada>();
}
