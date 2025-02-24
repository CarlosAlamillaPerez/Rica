using AutoMapper;
using bepensa_biz.Interfaces;
using bepensa_data.data;

namespace bepensa_biz.Proxies
{
    public class LlamadasProxy : ProxyBase, ILlamada
    {
        private readonly IMapper mapper;

        public LlamadasProxy(BepensaContext context, IMapper mapper)
        {
            DBContext = context;
            this.mapper = mapper;
        }


    }
}
