using System.ComponentModel.DataAnnotations;

namespace bepensa_models.DataModels
{
    public class RequestByIdUsuario
{
    [Display(Name = "IdUsuario")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public int IdUsuario { get; set; }
}
}
