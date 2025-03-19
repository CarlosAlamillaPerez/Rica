namespace bepensa_models.DTO;

public class EstatusProdSelectDTO
{
    public int IdProducto { get; set; }

    public string Nombre { get; set; } = null!;

    public bool Comprado { get; set; } = false;
}
