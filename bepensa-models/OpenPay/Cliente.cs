namespace bepensa_models.OpenPay;

public class Cliente
{
    public string Token { get; set; } = null!;
    public decimal Monto { get; set; }

    public string Name { get; set; }

    public string LastName { get; set; }

    public string PhoneNumber { get; set; }

    public string Email { get; set; }
}
