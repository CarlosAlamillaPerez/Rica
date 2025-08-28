using bepensa_models.DataModels;
using bepensa_models.DTO;
using bepensa_models.General;

namespace bepensa_biz.Interfaces
{
    public interface IReporte
    {
        Respuesta<ReporteDTO> GetDataReporte(int IdReporte);

        Respuesta<List<ReporteDTO>> TableroDescargas();

        Respuesta<List<dynamic>> ReportesDinamico(ReporteRequest data);
    }
}
