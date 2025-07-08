using bepensa_data.data;
using bepensa_data.logger.data;
using bepensa_models.Logger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Channels;

namespace bepensa_biz.Security
{
    public class ExternalApiLogBackgroundService : BackgroundService
    {
        private readonly Channel<ExternalApiLogger> _channel;

        private readonly IServiceProvider _serviceProvider;

        private readonly ILogger<ExternalApiLogBackgroundService> _logger;

        public ExternalApiLogBackgroundService(Channel<ExternalApiLogger> channel,
            IServiceProvider serviceProvider, ILogger<ExternalApiLogBackgroundService>  logger)
        {
            _channel = channel;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await foreach (var log in _channel.Reader.ReadAllAsync(stoppingToken))
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<BepensaLoggerContext>();

                    db.LoggerExternalApis.Add(log);

                    await db.SaveChangesAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al guardar el log de API en la base de datos");
                }
            }
        }
    }
}
