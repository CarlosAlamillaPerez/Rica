using bepensa_models.DTO;

namespace bepensa_models.Web
{
    public class EdoCtaWeb
    {
        public List<PeriodoDTO> Periodos { get; set; } = null!;

        public EdoCtaDTO EstadoCuenta { get; set; }
    }
}
