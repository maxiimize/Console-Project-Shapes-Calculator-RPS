using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using SharedLibrary;
using DataAcessLayer;

namespace ConsoleProject.MainApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var hostBuilder = Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory()) 
                .ConfigureAppConfiguration(cfg =>
                {
                    cfg.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((ctx, services) =>
                {
                    services.AddDbContext<AllDbContext>(opts =>
                        opts.UseSqlServer(ctx.Configuration.GetConnectionString("DefaultConnection"))
                    );

                    services.AddTransient<ShapesMenu>();
                    services.AddTransient<CalculatorMenu>();
                    services.AddTransient<RpsMenu>();
                    services.AddTransient<MainMenu>();
                })
                .ConfigureContainer<ContainerBuilder>(builder =>
                {
                   
                });

            var host = hostBuilder.Build();

            using (var scope = host.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AllDbContext>();
                db.Database.Migrate();
            }

            using (var scope = host.Services.CreateScope())
            {
                var menu = scope.ServiceProvider.GetRequiredService<MainMenu>();
                menu.Run();
            }
        }
    }
}
