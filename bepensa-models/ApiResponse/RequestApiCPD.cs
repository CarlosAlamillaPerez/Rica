namespace bepensa_models.ApiResponse;

public class RequestApiCPD
{
    public int IdUsuario { get; set; }
    public long IdCarrito { get; set; }
    public int IdPremio { get; set; }
    public Guid IdTransaccion { get; set; }
    public Transaccion? Transaccion { get; set; }
}
