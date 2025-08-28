using bepensa_models.ApiWa;
using bepensa_models.Enums;
using bepensa_models.General;

namespace bepensa_biz.Interfaces
{
    public interface IConsultasCuentaProxy
    {
        Respuesta<ResponseCuentaPrivacidad> ConsultaCliente(RequestCliente data);
        Task<Respuesta<Empty>> RecuperarPassword(RequestRecuperaPass data);
    }
}
