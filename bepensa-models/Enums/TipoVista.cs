using System.ComponentModel.DataAnnotations;

namespace bepensa_models.Enums;

public enum TipoVista
{
    [Display(Name = "VW_INICIO", Description = "Inicio")]
    Inicio = 1,

    [Display(Name = "VW_META_COMPRA", Description = "Meta de compra")]
    MetaDeCompra,
    
    [Display(Name = "VW_PP", Description = " Portafolio Prioritario")]
    PortafolioPrioritario,
    
    [Display(Name = "VW_FOTO_EXITO", Description = "Foto de éxito")]
    FotoDeExito,
    
    [Display(Name = "VW_MERCADEO", Description = "Ejecución de mercadeo")]
    EjecucionDeMercadeo,
    
    [Display(Name = "VW_MECANICA", Description = "Mecánica")]
    Mecanica,
    
    [Display(Name = "VW_PROMOS", Description = "Promociones")]
    Promociones,
    
    [Display(Name = "VW_EDO_CTA", Description = "Estado de cuenta")]
    EstadoDeCuenta,
    
    [Display(Name = "VW_CAT_CANJE", Description = "Catálogo de canje")]
    CatalogoDeCanje,
    
    [Display(Name = "VW_ALIANZAS", Description = "Alianzas")]
    Alianzas,
    [Display(Name = "VW_MI_CUENTA", Description = "Mi Cuenta")]
    MiCuenta,
    
    [Display(Name = "VW_CANJE", Description = "Proceso de canje")]
    ProcesoDeCanje,
    
    [Display(Name = "VW_LOGOUT", Description = "Cerrar sesión")]
    CerrarSesion,

    [Display(Name = "VW_CUMP_ENFRIADOR", Description = "Cumplimiento del enfriador")]
    CumplimientoDelenfriador
}
