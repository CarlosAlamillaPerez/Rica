using bepensa_socio_selecto_models.DataModels;
using bepensa_socio_selecto_models.DTO;
using bepensa_socio_selecto_models.General;

namespace bepensa_socio_selecto_biz.Interfaces
{
    public interface IUsuario
    {
        Task<Respuesta<List<UsuarioDTO>>> BuscarUsuario(BuscarRequest pBuscar);
    }
}
