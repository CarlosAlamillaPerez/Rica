namespace bepensa_models.DTO;

public class FuerzaVentaDTO
{
    public int Id { get; set; }

    public int IdCanal { get; set; }

    public int IdRolFdv { get; set; }

    public string Usuario { get; set; } = null!;

    public string? SesionId { get; set; }

    public int? IdBusqueda { get; set; }

    public int IdEstatus { get; set; }
}
