namespace bepensa_models.DTO;

public class PeriodoDTO
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public DateOnly Fecha { get; set; }
}
