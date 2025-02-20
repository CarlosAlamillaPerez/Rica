using bepensa_data.models;
using bepensa_models.CRM;
using bepensa_models.DataModels;
using bepensa_models.DTO;
using bepensa_models.Enums;
using bepensa_models.General;

namespace bepensa_biz.Interfaces
{
    public interface IOperador
    {
        Task<Respuesta<OperadorDTO>> ValidaAcceso(LoginCRMRequest pCredenciales);

        Task<Respuesta<Empty>> BloquearOperador(EmailRequest Email);

        Task<Respuesta<List<SeccionDTO>>> ConsultaMenuOperador(int idrol);
    }
}
