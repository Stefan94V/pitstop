using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pitstop.Infrastructure.Messaging;
using Serilog;

namespace AuditlogService
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            await host.RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            var hostBuilder = Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddScoped<IMessagePublisher, DaprMessagePublisher>();

                    services.AddTransient<AuditlogManagerConfig>((svc) =>
                    {
                        var auditlogConfigSection = hostContext.Configuration.GetSection("Auditlog");
                        string logPath = auditlogConfigSection["path"];
                        return new AuditlogManagerConfig { LogPath = logPath };
                    });

                    services.AddHostedService<AuditLogManager>();
                })
                .UseSerilog((hostContext, loggerConfiguration) =>
                {
                    loggerConfiguration.ReadFrom.Configuration(hostContext.Configuration);
                })
                .UseConsoleLifetime();
                
            return hostBuilder;
        }
    }
}