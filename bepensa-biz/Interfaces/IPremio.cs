using bepensa_models.DTO;
using bepensa_models.General;

namespace bepensa_biz.Interfaces
{
    public interface IPremio
    {
        /// <summary>
        /// Obtiene todas las categorías disponibles y activas de los premios.
        /// </summary>
        /// <returns>Lista de categorías de premios.</returns>
        Respuesta<List<CategoriaDePremioDTO>> ConsultarCategorias();

        /// <summary>
        /// Consulta los premios dependiendo del Id de la categoría.
        /// </summary>
        /// <param name="data"></param>
        /// <returns>Lista de premios simplificada.</returns>
        Task<Respuesta<List<PremioDTO>>> ConsultarPremios(int pIdCategoriaDePremio, int? idUsuario);

        Task<Respuesta<PremioDTO>> ConsultarPremioById(int pId, int? idUsuario);

        Respuesta<List<PremioDTO>> ConsultarPremiosByPuntos(int pPuntos);
    }
}
