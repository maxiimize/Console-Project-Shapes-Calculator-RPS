using System.Threading.Tasks;
using DataAcessLayer;
using DataAcessLayer.Interfaces;
using DataAcessLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedLibrary;
internal class Program
{
    static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(cfg =>
            {
                cfg.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
                services.AddDbContext<AllDbContext>(opts =>
                    opts.UseSqlServer(
                        context.Configuration.GetConnectionString("DefaultConnection")
                    ));

                services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
                
                services.AddTransient<MainMenu>();
                
            })
            .Build();

        using (var scope = host.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AllDbContext>();
            db.Database.Migrate();
        }

        var menu = host.Services.GetRequiredService<MainMenu>();
        await menu.RunAsync();
    }
}