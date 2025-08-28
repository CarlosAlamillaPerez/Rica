using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace bepensa_models.DataModels
{
    public class PasarelaCarritoRequest : ProcesarCarritoRequest
    {
        [Display(Name = "Token Id")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MinLength(1, ErrorMessage = "{0} inválido.")]
        public string Token_id { get; set; } = null!;

        [Display(Name = "Device Session Id")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MinLength(1, ErrorMessage = "{0} inválido.")]
        public string DeviceSessionId { get; set; } = null!;
    }
}
