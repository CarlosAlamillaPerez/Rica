using System.ComponentModel.DataAnnotations;

namespace bepensa_models.ApiWa
{
    public class RequestRecuperaPass
    {
        [Required(ErrorMessage = "El campo cliente es requerido")]
        public string Cliente { get; set; }

        [Required(ErrorMessage = "Últimos 4 digitos de teléfono es reguerido")]
        [StringLength(4)]
        [RegularExpression(@"^[0-9]{4}$", ErrorMessage = "El campo debe contener solo 4 dígitos numéricos.")]
        public string telUltimo4digitos { get; set; }
    }
}
