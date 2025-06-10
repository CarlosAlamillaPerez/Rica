﻿using bepensa_models.App;
using bepensa_models.DataModels;
using bepensa_models.DTO;
using bepensa_models.Enums;
using bepensa_models.General;

namespace bepensa_biz.Interfaces
{
    public interface IUsuario
    {
        #region Login
        Task<Respuesta<UsuarioDTO>> ValidaAcceso(LoginRequest pCredenciales, int idOrigen = (int)TipoOrigen.Web);
        Task<Respuesta<Empty>> BloquearUsuario(LoginRequest credenciales);
        Respuesta<bool> CambiarContrasenia(CambiarPasswordDTO passwords);
        #endregion

        #region App
        Task<Respuesta<UsuarioDTO>> ValidaAcceso(LoginApp credenciales, int idOrigen = (int)TipoOrigen.App);
        #endregion

        #region MiCuenta

        #region CRM
        Task<Respuesta<List<UsuarioDTO>>> BuscarUsuario(BuscarRequest pBuscar);

        Task<Respuesta<bool>> Actualizar(int pIdUsuario, string pCelular, string pEmail);

        Task<Respuesta<UsuarioDTO>> Actualizar(UsuarioRequest pUsuario, int pIdOperador);

        Task<Respuesta<UsuarioDTO>> BuscarUsuario(int pUsuario);
        #endregion

        //Task<Respuesta<UsuarioDTO>> ConsultarDatos(int idUsuario);

        Respuesta<MiCuentaDTO> MiCuenta(RequestByIdUsuario data);

        Respuesta<CuentaDTO> Cuenta(RequestByIdUsuario data);

        Respuesta<Empty> ActualizarContactos(ActualizarConctactosDTO data);

        Task<Respuesta<Empty>> RecuperarContrasenia(RestablecerPassRequest info);

        /// <summary>
        /// Cambia la contraseña del usuario mediante un token activo
        /// </summary>
        /// <param name="datos"></param>
        /// <returns></returns>
        Respuesta<Empty> CambiarContraseniaByToken(CambiarPasswordRequest datos);

        Task<Respuesta<Empty>> CambiarContraseniaApp(CambiarPasswordRequestApp datos);

        Respuesta<Empty> FuerzaDeVentaActivo(FuerzaDeVentaDTO? datos);
        #endregion
    }
}
