using System.ComponentModel.DataAnnotations;

namespace bepensa_models.Enums
{
    public enum TipoConceptoAcumulacion
    {
        [Display(Name = "Mecanica de Acumulación", Description = "Mecanica de Acumulación")]
        MecanicaDeAcumulacion = 1,

        [Display(Name = "Ejecución de Mercadeo", Description = "Ejecución de Mercadeo")]
        EjecucionDeMercadeo = 2,

        [Display(Name = "Portafolio Prioritario", Description = "Portafolio Prioritario")]
        PortafolioPrioritario = 3,

        [Display(Name = "Portafolio Imperdonable", Description = "Portafolio Imperdonable")]
        PortafolioImperdonable = 4,
        [Display(Name = "Promociones", Description = "Promociones")]
        Promociones = 5,
        [Display(Name = "Bonos", Description = "Bonos")]
        Bonos = 6,
        [Display(Name = "Actividades Especiales", Description = "Actividades Especiales")]
        ActividadesEspeciales = 7,
        [Display(Name = "Ajustes", Description = "Ajustes")]
        Ajustes = 8,
        [Display(Name = "Redenciones", Description = "Redenciones")]
        Redenciones = 9,
        [Display(Name = "Cancelación de Redenciones", Description = "Cancelación de Redenciones")]
        CancelacionDeRedenciones = 10,
    }
}
