using bepensa_models.DTO;
using bepensa_models.General;

namespace bepensa_biz.Interfaces
{
    public interface IDireccion
    {
        Task<Respuesta<List<ColoniaDTO>>> ConsultarColonias(string pCP);

        Task<Respuesta<MunicipioDTO>> ConsultarMunicipio(int pIdMunicipio);

        Task<Respuesta<EstadoDTO>> ConsultarEstado(int pIdMunicipio);
    }
}
