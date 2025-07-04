namespace bepensa_models.DTO;

public class PeriodosEmpaquesDTO
{
    public int IdPeriodo { get; set; }

    public DateOnly Fecha { get; set; }

    public List<CategoriaEmpaqueDTO> Categoria { get; set; } = [];

    public int Porcentaje { get; set; }
}
