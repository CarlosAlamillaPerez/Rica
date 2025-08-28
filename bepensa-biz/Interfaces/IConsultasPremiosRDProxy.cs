using bepensa_models.ApiWa;
using bepensa_models.DTO;
using bepensa_models.General;

namespace bepensa_biz.Interfaces
{
    public interface IConsultasPremiosRDProxy
    {
        Respuesta<List<PremioRDDTOWa>> PremiosSugeridosRD(RequestCliente data);
        Respuesta<List<PremioRDDTOWa>> PremiosCanjeadosRD(RequestClienteCanje data);
        Respuesta<List<PremioRDDTOWa>> PremioCanjeadoDetalleRD(RequestClienteCanjeDetalle data);
    }
}
