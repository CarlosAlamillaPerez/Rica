using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class Premio
{
    public int Id { get; set; }

    public int IdCategoriaDePremio { get; set; }

    public string Sku { get; set; } = null!;

    public string? Marca { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public decimal? Precio { get; set; }

    public int Puntos { get; set; }

    public string? Imagen { get; set; }

    public bool Visible { get; set; }

    public int IdEstatus { get; set; }

    public int IdOperador { get; set; }

    public DateTime FechaReg { get; set; }

    public int IdOperadorM { get; set; }

    public int IdTipoDeEnvio { get; set; }

    public bool? AplicaStock { get; set; }

    public int? Stock { get; set; }

    public int? Diaspromesa { get; set; }

    public DateTime? PromoFechaInicio { get; set; }

    public DateTime? PromoFechaFin { get; set; }

    public int? PromoPorcentaje { get; set; }

    public int? PromoPuntos { get; set; }

    public int? IdMetodoDeEntrega { get; set; }

    public virtual ICollection<Carrito> Carritos { get; set; } = new List<Carrito>();

    public virtual CategoriasDePremio IdCategoriaDePremioNavigation { get; set; } = null!;

    public virtual Estatus IdEstatusNavigation { get; set; } = null!;

    public virtual MetodosDeEntrega? IdMetodoDeEntregaNavigation { get; set; }

    public virtual Operadore IdOperadorMNavigation { get; set; } = null!;

    public virtual Operadore IdOperadorNavigation { get; set; } = null!;

    public virtual TiposDeEnvio IdTipoDeEnvioNavigation { get; set; } = null!;

    public virtual ICollection<Redencione> Redenciones { get; set; } = new List<Redencione>();
}
