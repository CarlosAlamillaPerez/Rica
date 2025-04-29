namespace bepensa_models.ApiResponse;

public class Transaccion
{
    public string? sku { get; set; }

    public int cantidad { get; set; }

    public string? id_cliente { get; set; }

    public string? id_compra { get; set; }

    public string? correo_e { get; set; }

    public string? numero_recarga { get; set; }

    public string? referencia { get; set; }

    public decimal? monto { get; set; }
}
