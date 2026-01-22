using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SpaServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SistemaFrontEnd.Services.Spa
{
    public static class SpaServiceExtensions
    {
        public static IServiceCollection AddSpaService(this IServiceCollection services,
            SpaServiceConfiguration spaServiceConfiguration, IHostEnvironment environment)
        {
            services.AddSingleton(spaServiceConfiguration);
            Console.WriteLine($"[SPA] Configuring SPA service");

            services.AddSpaStaticFiles(options =>
            {
                options.RootPath = environment.IsDevelopment() ? spaServiceConfiguration.DevelopmentSpaFilePath : spaServiceConfiguration.ProductionSpaFilePath;
            });

            return services;
        }

        public static IApplicationBuilder UseSpaService(this WebApplication app)
        {
            Console.WriteLine("[SPA] Initializing middleware");

            var spaServiceConfiguration = app.Services.GetRequiredService<SpaServiceConfiguration>();

            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseRouting();

            app.UseSpa(async spa =>
            {
                spa.Options.SourcePath = app.Environment.IsDevelopment() ? spaServiceConfiguration.DevelopmentSpaFilePath : spaServiceConfiguration.ProductionSpaFilePath;

                if (app.Environment.IsDevelopment())
                {
                    await spa.UseDevServer(spaServiceConfiguration, app);
                }
                else
                {
                    Console.WriteLine("[SPA] Running in Production mode - Serving static files");
                }
            });

            return app;
        }
    }
}