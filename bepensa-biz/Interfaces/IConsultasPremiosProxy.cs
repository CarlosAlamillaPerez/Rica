using bepensa_models.ApiWa;
using bepensa_models.DTO;
using bepensa_models.General;

namespace bepensa_biz.Interfaces
{
    public interface IConsultasPremiosProxy
    {
        Respuesta<ResponseConsultaPremiosSugeridos> PremiosSugeridos(RequestCliente data);
        Respuesta<List<PremioDTOWa>> PremiosCanjeados(RequestClienteCanje data);
        Respuesta<List<PremioDTOWa>> PremioCanjeadoDetalle(RequestClienteCanjeDetalle data);

    }
}
