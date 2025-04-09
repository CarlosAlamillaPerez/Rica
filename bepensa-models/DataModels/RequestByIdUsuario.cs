using System.ComponentModel.DataAnnotations;

namespace bepensa_models.DataModels
{
    public class RequestByIdUsuario
{
    [Display(Name = "IdUsuario")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [Range(1, int.MaxValue, ErrorMessage = "Usuario inv�lido.")]
    public int IdUsuario { get; set; }
}
}
