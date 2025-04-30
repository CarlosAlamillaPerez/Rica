namespace bepensa_models.ApiResponse;

public class ProcesaCarritoResultado
{
    public long? IdCarrito { get; set; }

    public int? IdPremio { get; set; }

    public string? Premio { get; set; }

    public string? TelefonoRecarga { get; set; }

    public string? Codigo { get; set; }

    public string? Pin { get; set; }

    public string? Folio { get; set; }

    public string? Motivo { get; set; }

    public int? Success { get; set; }

    public DateTime? FechaPromesa { get; set; }

    public static implicit operator ProcesaCarritoResultado(ResponseApiCPD data)
    {
        if (data == null) return new ProcesaCarritoResultado();
        return new ProcesaCarritoResultado
        {
            IdCarrito = data.IdCarrito,
            IdPremio = data.IdPremio,
            TelefonoRecarga = data.TelefonoRecarga,
            Codigo = data.giftCardRender,
            Pin = data.pinRender,
            Folio = data.Folio,
            Motivo = data.Success == 0 ? "Este premio no esta disponible por el momento." : "Canje Exitoso",
            Success = data.Success,
        };
    }
}
