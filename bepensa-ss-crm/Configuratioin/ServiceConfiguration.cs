﻿using bepensa_biz.Interfaces;
using bepensa_biz.Proxies;
using bepensa_biz.Security;
using bepensa_biz.Settings;
using bepensa_data.data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace bepensa_ss_crm.Configuratioin;

internal static class ServiceConfiguration
{
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
            options.UseSqlServer(connectionString, sqlServerOptionsAction: sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 10,
                    maxRetryDelay: TimeSpan.FromSeconds(3),
                    errorNumbersToAdd: null
                );
            });
            options.UseLazyLoadingProxies();
        });
    }
}
