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

        Task<Respuesta<CarritoDTO>> EliminarPremio(RequestByIdPremio pPremio, int idOrigen = (int)TipoOrigen.App);

        Task<Respuesta<CarritoDTO>> ModificarPremio(ActPremioRequest pPremio, int idOrigen = (int)TipoOrigen.App);

        Task<Respuesta<CarritoDTO>> EliminarCarrito(RequestByIdUsuario pPremio, int idOrigen = (int)TipoOrigen.App);

        Respuesta<EvaluacionPagoDTO> EvaluacionPago(int idUsuario, int idOrigen = (int)TipoOrigen.App);

        Task<Respuesta<List<ProcesaCarritoResultado>>> ProcesarCarrito(ProcesarCarritoRequest pPremio, int idOrigen = (int)TipoOrigen.App);

        /// <summary>
        /// Realiza una compra de puntos por deposito. (dinero físico)
        /// </summary>
        /// <param name="pUsuario"></param>
        /// <param name="idOrigen"></param>
        /// <returns></returns>
        Task<Respuesta<Empty>> ProcesarCarritoPorDeposito(ProcesarCarritoRequest pUsuario, int idOrigen = (int)TipoOrigen.App);

        Task<Respuesta<List<ProcesaCarritoResultado>, OpenPayDetails>> ProcesarCarritoConTarjeta(PasarelaCarritoRequest pPuntos, int idOrigen = (int)TipoOrigen.App);

        Respuesta<List<HistorialCompraPuntosDTO>> ConsultarDepositos(int? idUsuario);

        Task<Respuesta<List<ProcesaCarritoResultado>>> LiberarDeposito(RequestByIdUsuario pUsuario, string folio, bool? liberar = null);

        Respuesta<int> ValidarOrigenTranferencia(string idOpenPay);

        Task<Respuesta<List<ProcesaCarritoResultado>>> LiberarTranferencia(string id);

        Respuesta<Empty> ExistePremioFisico(int idUsuario);

        Respuesta<int> ConsultarTotalPremios(int idUsuario);
    }
}
