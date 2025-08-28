using System.ComponentModel.DataAnnotations;

namespace bepensa_models.DataModels;

public class TextoRequest
{
    [Display(Name = "Texto")]
    [MaxLength(255)]
    public string? Texto { get; set; } = null;
}
