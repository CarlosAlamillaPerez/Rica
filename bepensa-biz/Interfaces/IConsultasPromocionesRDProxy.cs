using bepensa_models.ApiWa;
using bepensa_models.DTO;
using bepensa_models.General;

namespace bepensa_biz.Interfaces
{
    public interface IConsultasPromocionesRDProxy
    {
        Respuesta<List<PromocionesRDDTOWa>> ConsultaPromociones(RequestCliente data);
    }
}
