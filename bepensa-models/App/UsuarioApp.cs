using bepensa_models.DTO;
using bepensa_models.Validators;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace bepensa_models.App
{
    public class UsuarioApp
    {
        public int? Id { get; set; }

        public string? Cuc { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [MaxLength(35, ErrorMessage = "El campo debe contener máximo 35 caracteres")]
        public string Nombre { get; set; } = null!;

        [Display(Name = "Apellido Paterno")]
        [MaxLength(50, ErrorMessage = "El campo debe contener máximo 50 caracteres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string ApellidoPaterno { get; set; } = null!;

        [Display(Name = "Apellido Materno")]
        [MaxLength(50, ErrorMessage = "El campo debe contener máximo 50 caracteres")]
        public string? ApellidoMaterno { get; set; }

        [Display(Name = "Fecha de nacimiento")]
        [FechaNacimientoValida]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public DateTime? FechaNacimiento { get; set; }

        [Display(Name = "Celular")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Solo se permiten números")]
        [MinLength(10, ErrorMessage = "El celular debe contener 10 caracteres")]
        [MaxLength(10, ErrorMessage = "El celular debe contener 10 caracteres")]
        public string? Celular { get; set; }

        public bool CambiarPass { get; set; }

        [Display(Name = "Correo electrónico")]
        [MaxLength(80, ErrorMessage = "El campo debe contener máximo 80 caracteres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DataType(DataType.EmailAddress, ErrorMessage = "El correo electrónico proporcionado no es válido, verifícalo")]
        public string Email { get; set; } = null!;

        public string? Sesion { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [MaxLength(1, ErrorMessage = "El campo debe contener máximo 1 caracter")]
        public string Sexo { get; set; } = null!;

        public long? IdNegocio { get; set; }

        public int Saldo { get; set; }

        public DateTime? UltimaActualizacion { get; set; }

        [Display(Name = "Fuerza de Venta")]
        [DefaultValue(false)]
        public bool FuerzaDeVenta { get; set; } = false;
    }
}
