using bepensa_models.ApiWa;
using bepensa_models.General;

namespace bepensa_biz.Interfaces
{
    public interface IConsultasClienteProxy
    {
        Respuesta<ResponsePuntos> ConsultaPuntos(RequestClientePeriodo data);
        Respuesta<ResponseCliente> ConsultaCliente(RequestCliente data);
    }
}
