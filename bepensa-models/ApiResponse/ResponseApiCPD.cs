using bepensa_data.models;

namespace bepensa_models.ApiResponse;

public class ResponseApiCPD
{
    public long? IdCarrito { get; set; }
    public int? Idusuario { get; set; }
    public int? IdPremio { get; set; }
    public Guid IdTransaccion { get; set; }
    public int? success { get; set; }
    public string? mensaje { get; set; }
    public string? giftcard { get; set; }
    public string? folio { get; set; }
    public string? TelefonoRecarga { get; set; }
    public string? giftCardRender { get; set; }
    public string? pinRender { get; set; }

    public static implicit operator CodigosRedimido(ResponseApiCPD data)
    {
        if (data == null) return new CodigosRedimido();
        return new CodigosRedimido
        {
            IdUsuario = (int)data.Idusuario,
            Codigo = data.giftCardRender,
            TelefonoRecarga = data.TelefonoRecarga,
            IdCarrito = data.IdCarrito,
            FechaReg = DateTime.Now,
            Pin = data.pinRender,
            IdTransaccionLog = data.IdTransaccion,
            Folio = data.folio,
            Motivo = data.mensaje
        };
    }

}
