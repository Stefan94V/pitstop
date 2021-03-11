using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pitstop.Infrastructure.Messaging;
using Pitstop.WorkshopManagementEventHandler.DataAccess;
using Polly;
using Serilog;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Pitstop.WorkshopManagementEventHandler
{
    class Program
    {
        public static void Main(string[] args)
        {
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, configHost) =>
                {
                    configHost.SetBasePath(Directory.GetCurrentDirectory());
                    configHost.AddJsonFile("hostsettings.json", optional: true);
                    configHost.AddJsonFile($"appsettings.json", optional: false);
                    configHost.AddEnvironmentVariables();
                    configHost.AddEnvironmentVariables("DOTNET_");
                    configHost.AddCommandLine(args);

                    configHost.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json",
                        optional: false);
                })
                .UseSerilog()
                .UseStartup<Startup>()
                .Build()
                .Run();
        }
    }
}