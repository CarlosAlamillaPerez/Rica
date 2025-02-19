namespace bepensa_socio_selecto_models.DTO;

public class SeccionDTO
{
    public int Id { get; set; }

    public int? IdPadre { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Vista { get; set; }

    public string? Controlador { get; set; }

    public string? Area { get; set; }

    public int IdEstatus { get; set; }

    public string? Icon { get; set; }

    public int? Orden { get; set; }
}
