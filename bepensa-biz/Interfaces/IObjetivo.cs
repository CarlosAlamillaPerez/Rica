using bepensa_models.DataModels;
using bepensa_models.DTO;
using bepensa_models.General;

namespace bepensa_biz.Interfaces
{
    public interface IObjetivo
    {
        /// <summary>
        /// Trae la meta mensual de un periodo seleccionado.
        /// </summary>
        /// <param name="pUsuario"></param>
        /// <returns></returns>
        Respuesta<MetaMensualDTO> ConsultarMetaMensual(UsuarioPeriodoRequest pUsuario);

        /// <summary>
        /// Trae la meta mensual del actual-
        /// </summary>
        /// <param name="pUsuario"></param>
        /// <returns></returns>
        Respuesta<MetaMensualDTO> ConsultarMetaMensual(RequestByIdUsuario pUsuario);

        Respuesta<List<PortafolioPrioritarioDTO>> ConsultarPortafolioPrioritario(UsuarioPeriodoRequest pUsuario);

        Respuesta<List<PortafolioPrioritarioDTO>> ConsultarPortafolioPrioritario(RequestByIdUsuario pUsuario);

        Respuesta<List<MetaCompraDTO>> ConsultarMetasMensuales(RequestByIdUsuario pUsuario);

        Respuesta<List<DetallePortafolioPrioritarioDTO>> ConsultarPortafoliosPrioritarios(RequestByIdUsuario pUsuario);

        Respuesta<List<EjecucionDTO>> ConsultarEjecucionTradicional(RequestByIdUsuario pUsuario);
    }
}
