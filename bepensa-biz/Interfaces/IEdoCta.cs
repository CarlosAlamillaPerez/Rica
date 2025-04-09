using bepensa_models;
using bepensa_models.DTO;
using bepensa_models.General;

namespace bepensa_biz.Interfaces;

public interface IEdoCta
{
    Respuesta<HeaderEdoCtaDTO> Header(string pCuc, int pYear, int pMes);

    Respuesta<EdoCtaDTO> MisPuntos(string pCuc, int pYear, int pMes);

    Respuesta<List<DetalleCanjeDTO>> DetalleCanje(string pCuc, int pYear, int pMes);
}
