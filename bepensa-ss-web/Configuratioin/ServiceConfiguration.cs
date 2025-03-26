﻿using bepensa_biz.Interfaces;
using bepensa_biz.Proxies;
using bepensa_biz.Security;
using bepensa_biz.Settings;
using bepensa_data.data;
using Microsoft.EntityFrameworkCore;

namespace bepensa_ss_web.Configuratioin;

internal static class ServiceConfiguration
{
    internal static void AppServices(this IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddSingleton<IEncryptor, EncryptorProxy>();

        services.AddScoped<IAccessSession, SessionProxy>();
        services.AddScoped<IUsuario, UsuariosProxy>();
        services.AddScoped<IEnviarCorreo, EnviarCorreoProxy>();
        services.AddScoped<IBitacoraDeContrasenas, BitacoraDeContrasenasProxy>();
        services.AddScoped<IBitacoraEnvioCorreo, BitacoraEnvioCorreoProxy>();
    }

    /// <summary>
    /// Configura los ajustes de la aplicación relacionandolo con el archivo de configuración "appsettings.json"
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    internal static void AppSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<GlobalSettings>(configuration.GetSection("Global"));
    }

    /// <summary>
    /// Configura el contexto de la base de datos para la aplicación.
    /// </summary>
    /// <param name="services">Colección de servicios</param>
    /// <param name="configuration">Configuración de la aplicación</param>
    internal static void AppDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DBContext");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("La cadena de conexión 'DBContext' no está configurada");
        }

        services.AddDbContext<BepensaContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });
    }
}
