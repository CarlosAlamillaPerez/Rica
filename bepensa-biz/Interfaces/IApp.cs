using bepensa_models.App;
using bepensa_models.DataModels;
using bepensa_models.DTO;
using bepensa_models.General;

namespace bepensa_biz;

public interface IApp
{
    Task<Respuesta<string>> ConsultaParametro(int pParametro);
}
