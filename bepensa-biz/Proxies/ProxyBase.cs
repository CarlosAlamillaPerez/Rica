using bepensa_data.data;
using bepensa_data.logger.data;

namespace bepensa_biz.Proxies;

public abstract class ProxyBase
{
    protected BepensaContext DBContext { get; set; } = null!;

    protected BepensaLoggerContext LoggerContext { get; set; } = null!;
    protected BepensaRD_Context DBContextRD { get; set; } = null!;
}
