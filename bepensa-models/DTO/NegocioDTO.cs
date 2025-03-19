using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bepensa_models.DTO
{
    public class NegocioDTO
    {
        public long Id { get; set; }

        public string Cuc { get; set; } = null!;

        [Display(Name = "Razón Social")]
        public string RazonSocial { get; set; } = null!;

        [Display(Name = "Teléfono")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "El campo {0} solo permite números")]
        [MinLength(10, ErrorMessage = "El teléfono debe contener 10 caracteres")]
        [MaxLength(10, ErrorMessage = "El teléfono debe contener 10 caracteres")]
        public string? TelefonoFijo { get; set; }

        public int IdEmbotelladora { get; set; }

        public int IdCedi { get; set; }

        public int IdSupervisor { get; set; }
        
        [Display(Name = "Calle")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string? Calle { get; set; }

        [Display(Name = "Número")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string? Numero { get; set; }

        [Display(Name = "Código Postal")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [MinLength(5, ErrorMessage = "El código postal debe contener 5 dígitos.")]
        [MaxLength(5, ErrorMessage = "El código postal debe contener 5 dígitos.")]
        [RegularExpression(@"[0-9]+", ErrorMessage = "Ingresa solo números, por favor.")]
        public string? CodigoPostal { get; set; }

        [Display(Name = "Barrio")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string? Barrio { get; set; }

        [Display(Name = "Municipio")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string? Municipio { get; set; }

        [Display(Name = "Provincia")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string? Provincia { get; set; }

        [Display(Name = "Referencias")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string? Referencias { get; set; }

        public int IdEstatus { get; set; }

        public DateTime FechaReg { get; set; }

        public int? IdOperadorReg { get; set; }

        public int? IdOperadorMod { get; set; }

        public bool Registro { get; set; }

        public virtual CediDTO IdCediNavigation { get; set; } = null!;

        public virtual EmbotelladoraDTO IdEmbotelladoraNavigation { get; set; } = null!;

        public virtual EstatusDTO? IdEstatusNavigation { get; set; } = null!;

        public virtual OperadorDTO? IdOperadorModNavigation { get; set; } = null!;

        public virtual OperadorDTO? IdOperadorRegNavigation { get; set; } = null!;

        public virtual SupervisorDTO IdSupervisorNavigation { get; set; } = null!;
    }
}
