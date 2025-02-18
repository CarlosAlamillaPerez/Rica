using bepensa_socio_selecto_data.data;

namespace bepensa_socio_selecto_biz.Proxies;

public abstract class ProxyBase
{
    protected BepensaContext DBContext { get; set; } = null!;
}
