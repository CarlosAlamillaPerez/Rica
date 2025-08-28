using System.ComponentModel.DataAnnotations;

namespace bepensa_models.ApiResponse;

#region Auth Api
public class AuthOpenPay
{
    public string Token { get; set; } = null!;
}
#endregion

#region Reponse Service "Charge"
public class RespuestaOpenPay<T>
{
    public int StatusCode { get; set; }

    public string Status { get; set; } = null!;

    public T? Data { get; set; }
}

public class CargoResponse
{
    public string Id { get; set; } = null!;

    public string Authorization { get; set; } = null!;

    public string Status { get; set; } = null!;

    public decimal Amount { get; set; }

    public string TransactionType { get; set; } = null!;

    public string Method { get; set; } = null!;

    public string? ErrorMessage { get; set; } = null;

    public DateTime CreationDate { get; set; }

    public TarjetaOpenPayResponse Card { get; set; } = null!;

    public PaymentMethod? PaymentMethod { get; set; } = null;
}

public class TarjetaOpenPayResponse
{
    public string BankName { get; set; } = null!;

    public string HolderName { get; set; } = null!;

    public string ExpirationMonth { get; set; } = null!;

    public string ExpirationYear { get; set; } = null!;

    public string CardNumber { get; set; } = null!;

    public string Brand { get; set; } = null!;

    public string Type { get; set; } = null!;
}

public class PaymentMethod
{
    public string Type { get; set; } = null!;

    public string Url { get; set; } = null!;
}
#endregion

#region Requesto for Service "Charge"
public class CargoOpenPayRequest
{
    [Display(Name = "Token")]
    [Required(ErrorMessage = "El campo {0} es requerido.")]
    [MinLength(1, ErrorMessage = "Token inválido.")]
    public string SourceId { get; set; } = null!;

    [Display(Name = "Monto")]
    [Required(ErrorMessage = "El campo {0} es requerido.")]
    public decimal Amount { get; set; }

    [Display(Name = "Dispositivo")]
    [MinLength(1, ErrorMessage = "Dispositivo inválido.")]
    public string? DeviceSessionId { get; set; }

    public CustomerRequest Customer { get; set; } = null!;
}

public class CustomerRequest
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    [Display(Name = "Nombre")]
    [Required(ErrorMessage = "El campo {0} es requerido.")]
    [MinLength(1, ErrorMessage = "{0} inválido.")]
    public string Name { get; set; } = null!;

    [Display(Name = "Apellidos")]
    [Required(ErrorMessage = "El campo {0} es requerido.")]
    [MinLength(1, ErrorMessage = "{0} inválido.")]
    public string LastName { get; set; } = null!;

    [Display(Name = "Celular")]
    public string? PhoneNumber { get; set; } = null!;

    [Display(Name = "Correo electrónico")]
    public string? Email { get; set; } = null!;
}

#endregion