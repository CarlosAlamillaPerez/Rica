using System.ComponentModel.DataAnnotations;

namespace bepensa_models.DataModels;

public class CelularRequest
{
    [Display(Name = "Celular")]
    [MaxLength(10, ErrorMessage = "No puede exceder de {1} caracteres el número de celular")]
    [MinLength(10, ErrorMessage = "El minimo de caracteres es de {1} en el número de celular")]
    [RegularExpression("^\\d+$", ErrorMessage = "El campo {0} solo admite números")]
    public string Celular { get; set; } = null!;
}
