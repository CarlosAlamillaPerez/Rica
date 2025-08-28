using System.ComponentModel.DataAnnotations;

namespace bepensa_models.ApiWa
{
    public class RequestCliente
    {
        [Required(ErrorMessage = "El campo cliente es requerido")]
        public string Cliente { get; set; }
    }
}
