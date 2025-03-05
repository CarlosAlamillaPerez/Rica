using bepensa_models.DataModels;
using bepensa_models.DTO;
using bepensa_models.General;

namespace bepensa_biz.Interfaces
{
    public interface IUsuario
    {
        Task<Respuesta<List<UsuarioDTO>>> BuscarUsuario(BuscarRequest pBuscar);

        Task<Respuesta<UsuarioDTO>> BuscarUsuario(int pIdUsuario);
    }
}
