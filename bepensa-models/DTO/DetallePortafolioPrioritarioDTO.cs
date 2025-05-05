namespace bepensa_models.DTO
{
    public class DetallePortafolioPrioritarioDTO
    {
        public int IdPeriodo { get; set; }

        public DateOnly Fecha { get; set; }

        public List<PortafolioPrioritarioDTO> PortafolioPrioritario { get; set; } = [];

        public int Porcentaje { get; set; }
    }
}
