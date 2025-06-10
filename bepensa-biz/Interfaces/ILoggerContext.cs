using bepensa_models.General;

namespace bepensa_biz.Interfaces
{
    public interface ILoggerContext
    {
        Task<Empty> AddJson(string pTipo, string pJson);
    }
}
