namespace bepensa_models.DTO
{
    public class EjecucionDTO
    {
        public string Concepto { get; set; } = null!;

        public string Mes { get; set; } = null!;

        public string EstatusEvaluacion { get; set; } = null!;

        public int Resultado { get; set; }
    }
}
