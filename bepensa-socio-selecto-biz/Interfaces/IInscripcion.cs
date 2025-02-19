using bepensa_socio_selecto_models.DataModels;
using bepensa_socio_selecto_models.DTO;
using bepensa_socio_selecto_models.Enums;
using bepensa_socio_selecto_models.General;

namespace bepensa_socio_selecto_biz.Interfaces
{
    public interface IInscripcion
    {
        Task<Respuesta<InscripcionDTO>> ConsultarUsuario(LoginInscripcionRequest pInscripcion);

        Respuesta<Empty> ValidarEmail(string email);

        Respuesta<Empty> ValidarCelular(string celular);

        Respuesta<Empty> ValidarTelefono(string pTelefono);

        Task<Respuesta<Empty>> Registrar(InscripcionRequest pInscripcion);
    }
}
