using bepensa_models.Enums;
using bepensa_models.General;

namespace bepensa_biz.Interfaces
{
    public interface IAppEmail
    {
        #region Envío de correo
        /// <summary>
        /// Realiza una solicitud para la recuperación de contraseña mediante un vínculo.
        /// </summary>
        /// <param name="metodoDeEnvio"></param>
        /// <param name="tipoUsuario"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Respuesta<Empty>> RecuperarPassword(TipoMensajeria metodoDeEnvio, TipoUsuario tipoUsuario, int id);

        Task<Respuesta<Empty>> ComprobanteDeCanje(TipoMensajeria metodoDeEnvio, TipoUsuario tipoUsuario, int id, long idRedencion, int? idHelp);
        #endregion

        #region Métodos de consulta
        /// <summary>
        /// El método realiza una búsqueda del mensaje enviado mediante un token único, una vez identificado este método realizar un incremento en el campo de lectura.
        /// </summary>
        /// <param name="token"></param>
        void Lectura(Guid? token);

        /// <summary>
        /// El método realiza una búsqueda de la url corta para identificar la url original.
        /// </summary>
        /// <param name="clave"></param>
        /// <returns></returns>
        Respuesta<string> ObtenerUrlOriginal(string clave);
        #endregion
    }
}
