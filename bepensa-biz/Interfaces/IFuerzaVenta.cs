using bepensa_models.App;
using bepensa_models.DataModels;
using bepensa_models.DTO;
using bepensa_models.Enums;
using bepensa_models.General;

namespace bepensa_biz.Interfaces
{
    public interface IFuerzaVenta
    {
        Task<Respuesta<FuerzaVentaDTO>> ValidaAcceso(LoginApp credenciales, int idCanal, int idOrigen = (int)TipoOrigen.App);

        Task<Respuesta<List<UsuarioDTO>>> ConsultarUsuarios(BuscarFDVRequest pBuscar);

        Task<Respuesta<UsuarioDTO>> ConsultarUsuario(int idUsuario, int idCanal);
    }
}
