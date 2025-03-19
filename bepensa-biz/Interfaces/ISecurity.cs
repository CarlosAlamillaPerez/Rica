using bepensa_models.DTO;
using bepensa_models.General;

namespace bepensa_biz.Interfaces
{
    public interface ISecurity
    {
        RespuestaAutenticacion GenerarTokenDeUsuario(UsuarioDTO usuario);

        RespuestaAutenticacion GenerarToken(ProviderDTO provider);

        Respuesta<bool> ValidaApiKey(ProviderDTO provider);
    }
}