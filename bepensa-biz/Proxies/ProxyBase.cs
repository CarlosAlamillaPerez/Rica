using bepensa_data.data;

namespace bepensa_biz.Proxies;

public abstract class ProxyBase
{
    protected BepensaContext DBContext { get; set; } = null!;
}
