using System.ComponentModel.DataAnnotations;

namespace bepensa_models.DTO;

public class SupervisorDTO
{
    public int Id { get; set; }

    [Display(Name = "Supervisor")]
    public string Nombre { get; set; } = null!;
}
