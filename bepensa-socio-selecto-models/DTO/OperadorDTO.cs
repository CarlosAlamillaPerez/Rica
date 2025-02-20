using bepensa_socio_selecto_data.models;

namespace bepensa_models.DTO;

public class OperadorDTO
{
    public int Id { get; set; }

    public int IdRol { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Celular { get; set; }

    public string? SessionId { get; set; }

    public int IdEstatus { get; set; }

    public DateTime FechaReg { get; set; }

    public RolDTO Rol { get; set; } = null!;
}
