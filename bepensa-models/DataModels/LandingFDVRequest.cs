using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace bepensa_models.DataModels;

public class LandingFDVRequest
{
    [Display(Name = "No Cliente")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [MinLength(1, ErrorMessage = "El campo {0} es obligatorio.")]
    [RegularExpression("(^[0-9]+$)", ErrorMessage = "El campo {0} solo permite números")]
    [DefaultValue(0)]
    public string Cuc { get; set; } = null!;
}
