using bepensa_models.App;
using bepensa_models.DataModels;
using bepensa_models.DTO;
using bepensa_models.General;

namespace bepensa_biz.Interfaces
{
    public interface IFuerzaVenta
    {
        Task<Respuesta<FuerzaVentaDTO>> ValidaAcceso(LoginApp credenciales, int idCanal);

        Task<Respuesta<List<UsuarioDTO>>> ConsultarUsuarios(BuscarFDVRequest pBuscar);

        Task<Respuesta<UsuarioDTO>> ConsultarUsuario(int idUsuario, int idCanal);
    }
}
