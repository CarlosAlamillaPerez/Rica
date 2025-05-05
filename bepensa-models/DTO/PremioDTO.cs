namespace bepensa_models.DTO;

public class PremioDTO
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

    public int IdTipoDeEnvio { get; set; }

    public bool? AplicaStock { get; set; }

    public int? Stock { get; set; }

    public int? Diaspromesa { get; set; }

    public DateTime? PromoFechaInicio { get; set; }

    public DateTime? PromoFechaFin { get; set; }

    public int? PromoPorcentaje { get; set; }

    public int? PromoPuntos { get; set; }

    public int? IdMetodoDeEntrega { get; set; }

    public string? MetodoDeEntrega { get; set; }
}
