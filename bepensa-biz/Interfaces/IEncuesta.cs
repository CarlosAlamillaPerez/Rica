using bepensa_models.DataModels;
using bepensa_models.DTO;
using bepensa_models.General;

namespace bepensa_biz.Interfaces
{
    public interface IEncuesta
    {
        Respuesta<List<BitacoraEncuestaDTO>> ConsultarEncuestas(int pIdUsuario);

        Respuesta<Empty> ResponderEncuesta(EncuestaRequest pEncuesta);
    }
}
