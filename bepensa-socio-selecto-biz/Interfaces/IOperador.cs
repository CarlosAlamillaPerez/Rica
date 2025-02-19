using bepensa_socio_selecto_data.models;
using bepensa_socio_selecto_models.CRM;
using bepensa_socio_selecto_models.DataModels;
using bepensa_socio_selecto_models.DTO;
using bepensa_socio_selecto_models.Enums;
using bepensa_socio_selecto_models.General;

namespace bepensa_socio_selecto_biz.Interfaces
{
    public interface IOperador
    {
        Task<Respuesta<OperadorDTO>> ValidaAcceso(LoginCRMRequest pCredenciales);

        Task<Respuesta<Empty>> BloquearOperador(EmailRequest Email);

        Task<Respuesta<List<SeccionDTO>>> ConsultaMenuOperador(int idrol);
    }
}
