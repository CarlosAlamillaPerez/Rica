namespace bepensa_models.DTO;

public class EstatusProdSelectDTO
{
    public int IdEmpaque { get; set; }

    public string Nombre { get; set; } = null!;

    public bool Cumple { get; set; } = false;
}
