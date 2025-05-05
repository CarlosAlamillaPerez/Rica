namespace bepensa_models.DTO
{
    public class CarritoDTO
    {
        public int Total { get; set; }

        public List<PremioCarritoDTO> Carrito { get; set; } = [];
    }
}
