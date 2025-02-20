using System.ComponentModel.DataAnnotations;

namespace bepensa_models.DTO;

public class ProgramaDTO
{
    public int Id { get; set; }

    [Display(Name = "Programa")]
    public string Nombre { get; set; } = null!;

    public CanalDTO Canal { get; set; } = null!;
}
