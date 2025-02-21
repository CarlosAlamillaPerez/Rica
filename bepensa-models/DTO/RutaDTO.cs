using System.ComponentModel.DataAnnotations;

namespace bepensa_models.DTO;

public class RutaDTO
{
    public int Id { get; set; }

    [Display(Name = "Ruta")]
    public string Nombre { get; set; } = null!;
}
