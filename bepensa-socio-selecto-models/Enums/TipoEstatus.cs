using System.ComponentModel.DataAnnotations;

namespace bepensa_socio_selecto_models.Enums
{
    public enum TipoEstatus
    {
        [Display(Name = "Activo", Description = "Activo")]
        Activo = 1,

        [Display(Name = "Inactivo", Description = "Inactivo")]
        Inactivo = 2,

        [Display(Name = "Baja", Description = "Baja")]
        Baja = 3,

        [Display(Name = "Bloqueada", Description = "Bloqueada")]
        Bloqueada = 4,

        [Display(Name = "Código Activo", Description = "Código Activo")]
        CodigoActivo = 5,

        [Display(Name = "Preregistro", Description = "Preregistro")]
        Preregistro = 6,
    }
}
