using System;
using System.Collections.Generic;

namespace bepensa_socio_selecto_data.models;

public partial class Llamada
{
    public int Id { get; set; }

    public int? IdPadre { get; set; }

    public string Tema { get; set; } = null!;

    public int? IdUsuario { get; set; }

    public string Nombre { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Comentario { get; set; } = null!;

    public int IdTdl { get; set; }

    public int IdSdl { get; set; }

    public int IdEdl { get; set; }

    public DateTime FechaReg { get; set; }

    public int IdOperadorReg { get; set; }

    public int? IdOperadorMod { get; set; }

    public virtual EstatusDeLlamadum IdEdlNavigation { get; set; } = null!;

    public virtual Operadore? IdOperadorModNavigation { get; set; }

    public virtual Operadore IdOperadorRegNavigation { get; set; } = null!;

    public virtual Llamada? IdPadreNavigation { get; set; }

    public virtual CategoriasDeLlamadum IdSdlNavigation { get; set; } = null!;

    public virtual TiposDeLlamadum IdTdlNavigation { get; set; } = null!;

    public virtual Usuario? IdUsuarioNavigation { get; set; }

    public virtual ICollection<Llamada> InverseIdPadreNavigation { get; set; } = new List<Llamada>();
}
