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
    }
}
