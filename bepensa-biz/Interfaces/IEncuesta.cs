using bepensa_data.models;
using bepensa_models.General;

namespace bepensa_biz.Interfaces
{
    public interface IEncuesta
    {
        Respuesta<List<Encuesta>> ConsultarEncuestas(int pIdUsuario);
    }
}
