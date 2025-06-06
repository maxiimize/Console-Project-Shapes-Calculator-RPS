using Autofac;
using Autofac.Extensions.DependencyInjection;
using DataAcessLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedLibrary;
using SharedLibrary.Strategies;
using System;

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
                    builder.RegisterType<AddStrategy>().As<ICalculationStrategy>().InstancePerDependency();
                    builder.RegisterType<SubtractStrategy>().As<ICalculationStrategy>().InstancePerDependency();
                    builder.RegisterType<MultiplyStrategy>().As<ICalculationStrategy>().InstancePerDependency();
                    builder.RegisterType<DivideStrategy>().As<ICalculationStrategy>().InstancePerDependency();
                    builder.RegisterType<SqrtStrategy>().As<ICalculationStrategy>().InstancePerDependency();
                    builder.RegisterType<ModulusStrategy>().As<ICalculationStrategy>().InstancePerDependency();

                    builder.RegisterType<CalculatorMenu>().AsSelf();

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
