using System.ComponentModel.DataAnnotations;

namespace bepensa_models.Enums;

public enum TipoPremio
{
    [Display(Name = "Fisico", Description = "Fisico")]
    Fisico = 1,

    [Display(Name = "Digital", Description = "Digital")]
    Digital
}
