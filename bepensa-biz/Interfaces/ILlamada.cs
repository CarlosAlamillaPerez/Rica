using bepensa_models.DataModels;
using bepensa_models.DTO;
using bepensa_models.Enums;
using bepensa_models.General;

namespace bepensa_biz.Interfaces
{
    public interface ILlamada
    {
        Task<Respuesta<Empty>> RegistrarLlamada(LlamadaRequest pLlamada);

        Respuesta<List<LlamadaDTO>> ConsultarLlamadas(int idUsuario);

        Respuesta<LlamadaDTO> ConsultarLlamada(int idLlamada);
    }
}
