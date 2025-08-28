using bepensa_models.ApiResponse;
using bepensa_models.Enums;
using bepensa_models.General;

namespace bepensa_biz.Interfaces;

public interface IOpenPay
{
    Task<Respuesta<CargoResponse>> CreditCard(CargoOpenPayRequest pTarjeta, int idOrigen = (int)TipoOrigen.App);

    Task<Respuesta<CargoResponse>> Charge(string id, int idOrigen = (int)TipoOrigen.Web);
}
