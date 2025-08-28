using bepensa_models.ApiWa;
using bepensa_models.General;

namespace bepensa_biz.Interfaces
{
    public interface IConsultasCuentaRDProxy
    {
        Respuesta<ResponseCuentaPrivacidad> ConsultaCliente(RequestCliente data);
        Task<Respuesta<Empty>> RecuperarPassword(RequestRecuperaPass data);
    }
}
