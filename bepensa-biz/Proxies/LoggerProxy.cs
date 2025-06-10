using bepensa_biz.Interfaces;
using bepensa_data.logger.models;
using bepensa_data.logger.data;
using bepensa_models.General;
using Microsoft.IdentityModel.Logging;

namespace bepensa_biz.Proxies
{
    public class LoggerProxy : ProxyBase, ILoggerContext
    {
        public LoggerProxy(BepensaLoggerContext context)
        {
            LoggerContext = context;
        }

        public async Task<Empty> AddJson(string pTipo, string? pJson)
        {
            Empty resultado =new();

            try
            {
                LoggerContext.LoggerJsonElements.Add(new LoggerJsonElement
                {
                    Tipo = pTipo,
                    JsonReceived = pJson
                });

                await LoggerContext.SaveChangesAsync();

                return resultado;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
