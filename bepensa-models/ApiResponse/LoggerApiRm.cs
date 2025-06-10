namespace bepensa_models.ApiResponse;

public class LoggerApiRm
{
    public int Id { get; set; }

    public string Metodo { get; set; } = null!;

    public string? Descripcion { get; set; }

    public string? Mensaje { get; set; }

    public string? Trace { get; set; }

    public DateTime FechaReg { get; set; }

    public string? Request { get; set; }

    public DateTime? RequestFecha { get; set; }

    public string? Response { get; set; }

    public DateTime? ResponseFecha { get; set; }

    public Guid? IdTransaccion { get; set; }
}
