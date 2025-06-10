using System.ComponentModel.DataAnnotations;

namespace bepensa_models.ApiResponse;

public class RequestEstatusOrden
{
    [Required(ErrorMessage = "Requiere un folio")]
    public string? Folio { get; set; } = null;
}
