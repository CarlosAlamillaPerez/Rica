namespace bepensa_models.DTO
{
    public class ReporteDTO
    {
        public int Id { get; set; }

        public int IdCanal { get; set; }

        public int IdDestino { get; set; }

        public string Nombre { get; set; } = null!;

        public int IdSeccion { get; set; }

        public string StoreProcedure { get; set; } = null!;

        public string ColorTxt { get; set; } = null!;

        public string ColorBg { get; set; } = null!;

        public string? Icono { get; set; }

        public int EstiloTabla { get; set; }

        public int Orden { get; set; }

        public int IdEstatus { get; set; }
    }
}
