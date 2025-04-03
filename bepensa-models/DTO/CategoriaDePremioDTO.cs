using System.Text.Json.Serialization;

namespace bepensa_models.DTO;

public class CategoriaDePremioDTO
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public string? Imgurl { get; set; }

    public bool Digital { get; set; }

    [JsonIgnore]
    public string? Estilos { get; set; }

    public string? FondoColor { get; set; }

    public string? LetraColor { get; set; }
}
