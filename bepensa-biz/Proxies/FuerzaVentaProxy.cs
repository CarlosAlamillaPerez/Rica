using bepensa_biz.Interfaces;
using bepensa_data.data;

namespace bepensa_biz.Proxies
{
    public class FuerzaVentaProxy : ProxyBase, IFuerzaVenta
    {
        public FuerzaVentaProxy(BepensaContext context)
        {
            DBContext = context;
        }

        
    }
}
