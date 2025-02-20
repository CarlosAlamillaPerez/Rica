using bepensa_models.Attributes;
using System.ComponentModel.DataAnnotations;

namespace bepensa_models.Enums
{
    public enum TipoEstatus
    {
        [Display(Name = "Activo", Description = "Activo")]
        [CssClass("badge-success")]
        Activo = 1,

        [Display(Name = "Inactivo", Description = "Inactivo")]
        [CssClass("badge-danger")]
        Inactivo = 2,

        [Display(Name = "Baja", Description = "Baja")]
        [CssClass("badge-danger")]
        Baja = 3,

        [Display(Name = "Bloqueada", Description = "Bloqueada")]
        [CssClass("badge-danger")]
        Bloqueada = 4,

        [Display(Name = "Código Activo", Description = "Código Activo")]
        CodigoActivo = 5,

        [Display(Name = "Preregistro", Description = "Preregistro")]
        [CssClass("badge-danger")]
        Preregistro = 6,
    }
}
