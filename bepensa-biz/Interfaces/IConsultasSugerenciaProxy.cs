using bepensa_models.ApiWa;
using bepensa_models.General;

namespace bepensa_biz.Interfaces
{
    public interface IConsultasSugerenciaProxy
    {
        Respuesta<ResponseSugerencia> RegistraSugerencia(RequestClienteSugerencia data);
    }
}
