using bepensa_models.DTO;
using bepensa_models.General;

namespace bepensa_biz.Interfaces
{
    public interface IPeriodo
    {
        Respuesta<List<PeriodoDTO>> ConsultarPeriodos(int omitir = 0);
    }
}
