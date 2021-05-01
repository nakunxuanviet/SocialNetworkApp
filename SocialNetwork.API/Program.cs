using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using SocialNetwork.Domain.Entities.Accounts;
using SocialNetwork.Infrastructure.Persistence;
using System;
using System.Threading.Tasks;

namespace SocialNetwork.API
{
    public class Program
    {
        private static readonly string AppName = typeof(Program).Namespace;

        public async static Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("System", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .CreateLogger();

            try
            {
                Log.Information("Configuring web host ({ApplicationContext})...", AppName);
                var host = CreateHostBuilder(args).Build();

                Log.Information("Applying migrations ({ApplicationContext})...", AppName);

                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;

                    var context = services.GetRequiredService<ApplicationDbContext>();

                    if (context.Database.IsSqlServer())
                    {
                        context.Database.Migrate();
                    }

                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

                    await ApplicationDbContextSeed.SeedDefaultUserAsync(userManager);
                    await ApplicationDbContextSeed.SeedSampleDataAsync(context);
                }

                await host.RunAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!", AppName);
                Log.Fatal(ex, "An error occurred while migrating or seeding the database.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).UseSerilog((hostingContext, loggerConfiguration) =>
                    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration));
    }
}