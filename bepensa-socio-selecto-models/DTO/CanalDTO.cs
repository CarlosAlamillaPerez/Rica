using System.ComponentModel.DataAnnotations;

namespace bepensa_socio_selecto_models.DTO
{
    public class CanalDTO
    {
        public int Id { get; set; }

        [Display(Name = "Canal")]
        public string Nombre { get; set; } = null!;
    }
}
