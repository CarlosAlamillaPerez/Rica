using bepensa_models.Enums;
using bepensa_models.General;

namespace bepensa_biz.Interfaces
{
    public interface IAppEmail
    {
        Task<Respuesta<Empty>> RecuperarPassword(TipoMensajeria metodoDeEnvio, TipoUsuario tipoUsuario, int id);

        void Lectura(Guid? token);

        Respuesta<string> ObtenerUrlOriginal(string clave);
    }
}
