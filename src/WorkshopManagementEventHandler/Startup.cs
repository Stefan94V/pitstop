using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.HealthChecks;
using Microsoft.Extensions.Hosting;
using Pitstop.WorkshopManagementEventHandler.DataAccess;
using Serilog;

namespace Pitstop.WorkshopManagementEventHandler
{
    public class Startup
    {
        private IConfiguration _configuration;

        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // add repo
            services.AddTransient<WorkshopManagementDBContext>((svc) =>
            {
                var sqlConnectionString = _configuration.GetConnectionString("WorkshopManagementCN");
                var dbContextOptions = new DbContextOptionsBuilder<WorkshopManagementDBContext>()
                    .UseSqlServer(sqlConnectionString)
                    .Options;
                var dbContext = new WorkshopManagementDBContext(dbContextOptions);

                DBInitializer.Initialize(dbContext);

                return dbContext;
            });

            // Add framework services.
            services
                .AddControllers()
                .AddDapr();
            // .AddNewtonsoftJson();

            // services.AddHealthChecks(checks =>
            // {
            //     checks.WithDefaultCacheDuration(TimeSpan.FromSeconds(1));
            //     checks.AddSqlCheck("EventStoreCN", _configuration.GetConnectionString("EventStoreCN"));
            //     checks.AddSqlCheck("WorkshopManagementCN", _configuration.GetConnectionString("WorkshopManagementCN"));
            // });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(_configuration)
                // .Enrich.WithMachineName()
                .CreateLogger();
            
            app.UseCloudEvents();
            app.UseRouting();
            

            app.UseEndpoints(endpoints => { endpoints.MapSubscribeHandler(); });
        }
    }
}