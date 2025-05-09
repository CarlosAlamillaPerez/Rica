using bepensa_biz.Interfaces;
using bepensa_biz.Proxies;
using bepensa_biz.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace bepensa_biz.Security;

public static class ServiceConfiguration
{
    #region General
    public static void AppServices(this IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddSingleton<IEncryptor, EncryptorProxy>();

        services.AddSingleton<IAccessSession, SessionProxy>();
        services.AddScoped<IUsuario, UsuariosProxy>();
        services.AddScoped<IEnviarCorreo, EnviarCorreoProxy>();
        services.AddScoped<IBitacoraDeContrasenas, BitacoraDeContrasenasProxy>();
        services.AddScoped<IBitacoraEnvioCorreo, BitacoraEnvioCorreoProxy>();
        services.AddScoped<IAppEmail, EmailProxy>();
        services.AddScoped<IObjetivo, ObjetivosProxy>();
        services.AddScoped<IPremio, PremiosProxy>();
        services.AddScoped<IApp, AppProxy>();
        services.AddScoped<IPeriodo, PeriodosProxy>();
        services.AddScoped<IEdoCta, EdoCtaProxy>();
        services.AddScoped<IDropDownList, DropDownListProxy>();
        services.AddScoped<ICarrito, CarritoProxy>();
        services.AddScoped<IFuerzaVenta, FuerzaVentaProxy>();
        services.AddScoped<IApi, ApiProxy>();
        services.AddScoped<IDireccion, DireccionesProxy>();
    }
    #endregion

    #region CRM
    public static void AppServicesCRM(this IServiceCollection services)
    {   
        services.AddScoped<IOperador, OperadoresProxy>();
        services.AddScoped<ILlamada, LlamadasProxy>();
    }
    #endregion

    /// <summary>
    /// Configura los ajustes de la aplicación relacionandolo con el archivo de configuración "appsettings.json"
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void AppSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<GlobalSettings>(configuration.GetSection("Global"));
        services.Configure<SmsSettings>(configuration.GetSection("Sms"));
        services.Configure<PremiosSettings>(configuration.GetSection("Premios"));

        services.Configure<ApiRMSSettings>(configuration.GetSection("ApiRms"));
        services.Configure<ApiCPDSettings>(configuration.GetSection("ApiCPD"));
    }
}
