using bepensa_models.App;
using bepensa_models.DataModels;
using bepensa_models.DTO;
using bepensa_models.General;

namespace bepensa_biz.Interfaces
{
    public interface IUsuarios
    {
        #region Login
        Task<Respuesta<UsuarioDTO>> ValidaAcceso(LoginDTO pCredenciales);
        Task<Respuesta<Empty>> BloquearUsuario(LoginDTO credenciales);
        Respuesta<bool> CambiarContrasenia(CambiarPasswordDTO passwords);
        #endregion

        #region App
        Task<Respuesta<UsuarioDTO>> ValidaAcceso(LoginApp credenciales);
        #endregion

        #region MiCuenta

        Task<Respuesta<List<UsuarioDTO>>> BuscarUsuario(BuscarRequest pBuscar);

        Respuesta<UsuarioDTO> ConsultarUsuario(long idUsuario);

        Respuesta<MiCuentaDTO> MiCuenta(RequestByIdUsuario data);

        Respuesta<CuentaDTO> Cuenta(RequestByIdUsuario data);

        Respuesta<Empty> ActualizarContactos(ActualizarConctactosDTO data);

        Respuesta<Empty> RecuperarContrasenia(EmailRequest info);

        /// <summary>
        /// Cambia la contraseña del usuario mediante un token activo
        /// </summary>
        /// <param name="datos"></param>
        /// <returns></returns>
        Respuesta<Empty> CambiarContraseniaByToken(CambiarPasswordRequest datos);

        Respuesta<Empty> FuerzaDeVentaActivo(FuerzaDeVentaDTO? datos);
        #endregion
    }
}
