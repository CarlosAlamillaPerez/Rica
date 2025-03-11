using bepensa_models.Attributes;
using System.ComponentModel.DataAnnotations;

namespace bepensa_models.Enums;

public enum TipoDeEstatus
{
    [Display(Name = "Activo", Description = "Activo")]
    [CssClass("badge-success")]
    Activo = 1,

    [Display(Name = "Inactivo", Description = "Inactivo")]
    [CssClass("badge-danger")]
    Inactivo,

    [Display(Name = "Baja", Description = "Baja")]
    Baja,

    [Display(Name = "Bloqueada", Description = "Bloqueada")]
    [CssClass("badge-danger")]
    Bloqueada,

    [Display(Name = "Código Activo", Description = "Código Activo")]
    CodigoActivo,

    [Display(Name = "Código Canjeado", Description = "Código Canjeado")]
    CodigoCanjeado,

    [Display(Name = "Código Vencido", Description = "Código Vencido")]
    CodigoVencido

}
