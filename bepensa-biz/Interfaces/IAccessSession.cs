using bepensa_models.CRM;
using bepensa_models.DataModels;
using bepensa_models.DTO;

namespace bepensa_biz.Interfaces
{
    public interface IAccessSession
    {
        #region Inscripcion
        LoginInscripcionRequest CredencialesInscripcion { get; set; }
        #endregion

        #region Web
        LoginRequest Credenciales { get; set; }

        UsuarioDTO UsuarioActual { get; set; }
        #endregion

        #region CRM
        LoginCRMRequest CredencialesCRM { get; set; }

        OperadorDTO OperadorActual { get; set; }

        SesionCRM SesionCRM { get; set; }

        List<SeccionDTO> CrmMenuOperador { get; set; }
        #endregion

        void Logout();

        void SetCookie(string key, string value, TimeSpan expiration);

        string GetCookie(string key);

        void DeleteCookie(string key);

        string GetSesion(string key);

        void SetSesion(string key, string value);

        void RemoveSesion(string key);
    }
}
