using bepensa_biz.Interfaces;
using bepensa_models.General;

namespace bepensa_biz.Proxies
{
    public class EnviarCorreoProxy : ProxyBase, IEnviarCorreo
    {
        public void EnviarCorreo(CorreoDTO correo)
        {
            throw new NotImplementedException();
        }

        public void EnviarCorreoCRM(CorreoDTO data)
        {
            throw new NotImplementedException();
        }

        public void Lectura(Guid? Id)
        {
            throw new NotImplementedException();
        }
    }
}
