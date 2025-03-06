using bepensa_models.DTO;
using bepensa_models.Validators;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace bepensa_models.DataModels
{
    public class UsuarioRequest
    {
        [Display(Name = "Id de usuario")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "País inválido")]
        [DefaultValue(0)]
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [MaxLength(45, ErrorMessage = "El campo {0} debe contener máximo {1} caracteres")]
        [RegularExpression("^[A-Za-zÁáÉéÍíÓóÚúÑñ\\s]+$", ErrorMessage = "{0} inválido.")]
        public string Nombre { get; set; } = null!;

        [Display(Name = "Apellido paterno")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe contener máximo 50 caracteres")]
        [RegularExpression("^[A-Za-zÁáÉéÍíÓóÚúÑñ\\s]+$", ErrorMessage = "{0} inválido.")]
        public string ApellidoPaterno { get; set; } = null!;

        [Display(Name = "Apellido materno")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe contener máximo 50 caracteres")]
        [RegularExpression("^[A-Za-zÁáÉéÍíÓóÚúÑñ\\s]+$", ErrorMessage = "{0} inválido.")]
        public string? ApellidoMaterno { get; set; }

        [Display(Name = "Fecha de nacimiento")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [FechaNacimientoValida(ErrorMessage = "La persona debe ser mayor de 18 años para registrarse.")]
        public DateOnly? FechaNacimiento { get; set; }

        [Display(Name = "Sexo")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [MaxLength(1, ErrorMessage = "El campo {0} debe contener máximo 1 caracter")]
        public string? Sexo { get; set; }

        [Display(Name = "Celular")]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "El campo {0} solo permite números")]
        [MinLength(10, ErrorMessage = "El campo {0} debe contener 10 caracteres")]
        [MaxLength(10, ErrorMessage = "El campo {0} debe contener 10 caracteres")]
        [DataType(DataType.PhoneNumber)]
        [Phone(ErrorMessage = "Por favor, ingresa un número de celular válido.")]
        public string? Celular { get; set; }

        [Display(Name = "Correo electrónico")]
        [MaxLength(80, ErrorMessage = "El campo {0} debe contener máximo 80 caracteres")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "El {0} proporcionado no es válido, verifícalo")]
        public string? Email { get; set; }

        [Display(Name = "Calle")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [MaxLength(150, ErrorMessage = "El campo {0} debe contener máximo 150 caracteres")]
        public string? Calle { get; set; }

        [Display(Name = "No. Exterior")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [MaxLength(20, ErrorMessage = "El campo {0} debe contener máximo 20 caracteres")]
        public string? NumeroExterior { get; set; }

        [Display(Name = "No. Interior")]
        [MaxLength(20, ErrorMessage = "El campo {0} debe contener máximo 20 caracteres")]
        public string? NumeroInterior { get; set; }

        [Display(Name = "Código Postal")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "El campo {0} solo permite números")]
        [MinLength(5, ErrorMessage = "El {0} debe contener 5 caracteres")]
        [MaxLength(5, ErrorMessage = "El {0} debe contener 5 caracteres")]
        public string? CodigoPostal { get; set; } = null!;

        [Display(Name = "Colonia")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "Colonia inválida.")]
        public int? IdColonia { get; set; }

        [Display(Name = "Ciudad")]
        [MaxLength(150, ErrorMessage = "El {0} debe contener 150 caracteres máximo.")]
        public string? Ciudad { get; set; }

        [Display(Name = "Entre calle")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [MaxLength(150)]
        public string? CalleInicio { get; set; }

        [Display(Name = "Y calle")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [MaxLength(150, ErrorMessage = "El campo {0} debe contener máximo 150 caracteres")]
        public string? CalleFin { get; set; }

        [Display(Name = "Referencias")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [MaxLength(400, ErrorMessage = "El campo {0} debe contener 400 caracteres máximo")]
        public string? Referencias { get; set; }

        [Display(Name = "Teléfono de negocio")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "El campo {0} solo acepta números")]
        [MinLength(10, ErrorMessage = "El campo {0} debe contener 10 caracteres")]
        [MaxLength(10, ErrorMessage = "El campo {0} debe contener 10 caracteres")]
        [DataType(DataType.PhoneNumber)]
        [Phone(ErrorMessage = "Por favor, ingresa un número de teléfono válido.")]
        public string? Telefono { get; set; }

        public static implicit operator UsuarioDTO(UsuarioRequest request)
        {
            return new UsuarioDTO
            {
                Id = request.Id,
                Nombre = request.Nombre,
                ApellidoPaterno = request.ApellidoPaterno,
                ApellidoMaterno = request.ApellidoMaterno,
                FechaNacimiento = request.FechaNacimiento,
                Sexo = request.Sexo,
                Celular = request.Celular,
                Email = request.Email,
                Calle = request.Calle,
                NumeroExterior = request.NumeroExterior,
                NumeroInterior = request.NumeroInterior,
                CodigoPostal = request.CodigoPostal,
                IdColonia = request.IdColonia,
                Ciudad = request.Ciudad,
                CalleInicio = request.CalleInicio,
                CalleFin = request.CalleFin,
                Referencias = request.Referencias,
                Telefono = request.Telefono
            };
        }
    }
}
