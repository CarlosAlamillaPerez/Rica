using bepensa_models.DataModels;
using System.ComponentModel.DataAnnotations;

namespace bepensa_models.DTO
{
    public class CuentaDTO : CuentaRequest
    {
        [Display(Name = "No. de cliente")]
        public string CUC { get; set; } = null!;

        [Display(Name = "Embotelladora")]
        public string Embotelladora { get; set; } = null!;

        [Display(Name = "CEDI")]
        public string CEDI { get; set; } = null!;

        [Display(Name = "Supervisor")]
        public string Supervisor { get; set; } = null!;


        [Display(Name = "Razón Social")]
        public string RazonSocial { get; set; } = null!;

        public int Saldo { get; set; }

        public int IdEstatus { get; set; }

        public bool Bloqueado { get; set; }
    }
}
