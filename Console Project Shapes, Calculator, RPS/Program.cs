using System;
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
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(cfg =>
                {
                    cfg.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((ctx, services) =>
                {
                    services.AddDbContext<AllDbContext>(opts =>
                        opts.UseSqlServer(ctx.Configuration.GetConnectionString("DefaultConnection"))
                    );

                    services.AddTransient<MainMenu>();
                })
                .Build();

            using (var scope = host.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AllDbContext>();
                db.Database.Migrate();
            }

            var menu = host.Services.GetRequiredService<MainMenu>();
            menu.Run();
        }
    }
}
