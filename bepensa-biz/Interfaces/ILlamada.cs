using bepensa_models.DataModels;
using bepensa_models.Enums;
using bepensa_models.General;

namespace bepensa_biz.Interfaces
{
    public interface ILlamada
    {
        Task<Respuesta<Empty>> RegistrarLlamada(LlamadaRequest pLlamada);
    }
}
