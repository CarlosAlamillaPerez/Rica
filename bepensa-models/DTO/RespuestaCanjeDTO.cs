namespace bepensa_models.DTO
{
    public class RespuestaCanjeDTO
    {
        public int SaldoUtilizado { get; set; } = 0;

        public int SaldoActual { get; set; } = 0;

        public List<string>? Codigos { get; set; }
    }
}
