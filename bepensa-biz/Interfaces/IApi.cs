using bepensa_biz.Settings;
using bepensa_models.ApiResponse;
using bepensa_models.General;
using Newtonsoft.Json;

namespace bepensa_biz.Interfaces
{
    public interface IApi
    {
        Respuesta<RastreoRMS> Autenticacion();
    }
}
