using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.HealthChecks;
using Microsoft.Extensions.Hosting;
using Pitstop.InvoiceService.CommunicationChannels;
using Pitstop.InvoiceService.Repositories;
using Serilog;

namespace Pitstop.InvoiceService
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
            services.AddTransient<IInvoiceRepository>((svc) =>
            {
                var sqlConnectionString = _configuration.GetConnectionString("InvoiceServiceCN");
                return new SqlServerInvoiceRepository(sqlConnectionString);
            });

            services.AddTransient<IEmailCommunicator>((svc) =>
            {
                var mailConfigSection = _configuration.GetSection("Email");
                string mailHost = mailConfigSection["Host"];
                int mailPort = Convert.ToInt32(mailConfigSection["Port"]);
                string mailUserName = mailConfigSection["User"];
                string mailPassword = mailConfigSection["Pwd"];
                return new SMTPEmailCommunicator(mailHost, mailPort, mailUserName, mailPassword);
            });

            // Add framework services.
            services
                .AddMvc((options) => options.EnableEndpointRouting = false)
                .AddDapr()
                .AddNewtonsoftJson();

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

            app.UseMvc();
            app.UseRouting();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseCloudEvents();

            app.UseEndpoints(endpoints => { endpoints.MapSubscribeHandler(); });
        }
    }
}