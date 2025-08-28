using bepensa_models.ApiWa;
using bepensa_models.DTO;
using bepensa_models.General;

namespace bepensa_biz.Interfaces
{
    public interface IConsultasAvanceRDProxy
    {
        Respuesta<List<CuotaDeCompraRDDTOWa>> ConsultaAvance(RequestCliente data);
    }
}
