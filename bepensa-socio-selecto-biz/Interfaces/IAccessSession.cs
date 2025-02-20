using bepensa_socio_selecto_data.models;
using bepensa_models.CRM;
using bepensa_models.DataModels;
using bepensa_models.DTO;

namespace bepensa_socio_selecto_biz.Interfaces
{
    public interface IAccessSession
    {
        #region Inscripcion
        LoginInscripcionRequest CredencialesInscripcion { get; set; }
        #endregion

        #region CRM
        LoginCRMRequest CredencialesCRM { get; set; }

        OperadorDTO OperadorActual { get; set; }

        List<SeccionDTO> CrmMenuOperador { get; set; }
        #endregion

        void Logout();
    }
}
