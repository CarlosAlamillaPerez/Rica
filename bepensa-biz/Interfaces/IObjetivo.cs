using bepensa_models.DataModels;
using bepensa_models.DTO;
using bepensa_models.General;

namespace bepensa_biz.Interfaces
{
    public interface IObjetivo
    {
        Respuesta<MetaMensualDTO> ConsultarMeteMensual(UsuarioPeriodoRequest pUsuario);

        Respuesta<List<PortafolioPrioritarioDTO>> ConsultarPortafolioPrioritario(UsuarioPeriodoRequest pUsuario);
    }
}
