using bepensa_data.models;
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

        #region CRM
        LoginCRMRequest CredencialesCRM { get; set; }

        OperadorDTO OperadorActual { get; set; }

        SesionCRM SesionCRM { get; set; }

        List<SeccionDTO> CrmMenuOperador { get; set; }
        #endregion

        void Logout();
    }
}
