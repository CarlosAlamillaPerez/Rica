namespace bepensa_models.DTO;

public class EvaluacionPagoDTO
{
    public int PuntosDisponibles { get; set; }

    public int PuntosCarrito { get; set; }

    public int PuntosFaltantes { get; set; }

    public decimal Deposito { get; set; }

    public decimal Tarjeta { get; set; }
}
