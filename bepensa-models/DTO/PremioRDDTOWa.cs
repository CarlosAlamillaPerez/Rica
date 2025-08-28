using bepensa_data.modelsRD;

namespace bepensa_models.DTO
{
    public class PremioRDDTOWa
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

        public static implicit operator PremioRDDTOWa(Premio data)
        {
            if (data == null) return new PremioRDDTOWa();
            return new PremioRDDTOWa
            {
                Id = data.Id,
                IdCategoriaDePremio = data.IdCategoriaDePremio,
                Sku = data.Sku,
                Marca = data.Marca,
                Nombre = data.Nombre,
                Descripcion = data.Descripcion,
                Precio = data.Precio,
                Puntos = data.Puntos,
                Imagen = data.Imagen,
                Visible = data.Visible,
                IdEstatus = data.IdEstatus,
                IdTipoDeEnvio = data.IdTipoDeEnvio,
                AplicaStock = data.AplicaStock,
                Stock = data.Stock,
                Diaspromesa = data.Diaspromesa,
                PromoFechaInicio = data.PromoFechaInicio,
                PromoFechaFin = data.PromoFechaFin,
                PromoPorcentaje = data.PromoPorcentaje,
                PromoPuntos = data.PromoPuntos,
                IdMetodoDeEntrega = data.IdMetodoDeEntrega,
                MetodoDeEntrega = data.IdMetodoDeEntregaNavigation.Nombre
            };
        }
    }
}
