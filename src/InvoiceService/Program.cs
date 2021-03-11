using Serilog;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Pitstop.InvoiceService
{
    class Program
    {
        public static void Main(string[] args)
        {
            WebHost.CreateDefaultBuilder(args)
                .UseSerilog((hostContext, loggerConfiguration) =>
                {
                    loggerConfiguration.ReadFrom.Configuration(hostContext.Configuration);
                })
                .UseStartup<Startup>()
                .Build()
                .Run();
        }
    }
}