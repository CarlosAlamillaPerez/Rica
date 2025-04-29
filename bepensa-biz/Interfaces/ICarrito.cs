using bepensa_models.ApiResponse;
using bepensa_models.DataModels;
using bepensa_models.DTO;
using bepensa_models.Enums;
using bepensa_models.General;

namespace bepensa_biz.Interfaces
{
    public interface ICarrito
    {
        Task<Respuesta<Empty>> AgregarPremio(AgregarPremioRequest pPremio, int idOrigen = (int)TipoOrigen.App);

        Respuesta<CarritoDTO> ConsultarCarrito(RequestByIdUsuario pPremio);

        Task<Respuesta<CarritoDTO>> EliminarPremio(RequestByIdPremio pPremio);

        Task<Respuesta<CarritoDTO>> ModificarPremio(ActPremioRequest pPremio);

        Task<Respuesta<List<ProcesaCarritoResultado>>> ProcesarCarrito(ProcesarCarritoRequest pPremio, int idOrigen = (int)TipoOrigen.App);

        Respuesta<Empty> ExistePremioFisico(int idUsuario);
    }
}
