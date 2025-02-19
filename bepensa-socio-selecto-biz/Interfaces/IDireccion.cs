using bepensa_socio_selecto_models.DTO;
using bepensa_socio_selecto_models.General;

namespace bepensa_socio_selecto_biz.Interfaces
{
    public interface IDireccion
    {
        Task<Respuesta<List<ColoniaDTO>>> ConsultarColonias(string pCP);

        Task<Respuesta<MunicipioDTO>> ConsultarMunicipio(int pIdMunicipio);

        Task<Respuesta<EstadoDTO>> ConsultarEstado(int pIdMunicipio);
    }
}
