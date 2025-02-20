using System.ComponentModel.DataAnnotations;

namespace bepensa_models.DTO
{
    public class CanalDTO
    {
        public int Id { get; set; }

        [Display(Name = "Canal")]
        public string Nombre { get; set; } = null!;
    }
}
