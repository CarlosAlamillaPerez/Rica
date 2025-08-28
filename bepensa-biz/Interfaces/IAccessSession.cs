using bepensa_models.App;
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
        LoginApp Credenciales { get; set; }

        UsuarioDTO UsuarioActual { get; set; }

        public bool ForzarCambio { get; set; }
        #endregion

        #region CRM
        LoginCRMRequest CredencialesCRM { get; set; }

        OperadorDTO OperadorActual { get; set; }

        FuerzaVentaDTO FuerzaVenta { get; set; }

        List<SeccionDTO> CrmMenuOperador { get; set; }
        #endregion

        void Logout();

        void SetCookie(string key, string value, TimeSpan expiration);
    }
}
