using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SocialNetwork.Application.Common.Interfaces;
using SocialNetwork.Application.Common.Models.Emails;
using SocialNetwork.Domain.SeedWork;
using SocialNetwork.Infrastructure.Email;
using SocialNetwork.Infrastructure.Files;
using SocialNetwork.Infrastructure.Identity;
using SocialNetwork.Infrastructure.Persistence;
using SocialNetwork.Infrastructure.Repository;
using SocialNetwork.Infrastructure.Services;
using System;

namespace SocialNetwork.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("SocialDb"));
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    //options.UseLazyLoadingProxies();
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
                });
            }

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
            services.AddScoped<Func<ApplicationDbContext>>((provider) => () => provider.GetService<ApplicationDbContext>());
            services.AddScoped<DbFactory>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IDomainEventService, DomainEventService>();
            services.AddTransient<ISystemTime, SystemTimeService>();
            services.AddTransient<ICsvFileBuilder, CsvFileBuilder>();
            services.AddSingleton<IUserAccessor, UserAccessor>();
            services.AddTransient<IIdentityService, IdentityService>();

            return services;
        }

        public static IServiceCollection AddCustomLogger(this IServiceCollection services)
        {
            services.AddSingleton<ILogger>(Log.Logger);
            services.AddSingleton<ILoggerManager, LoggerService>();

            return services;
        }

        public static IServiceCollection AddEmail(this IServiceCollection services, IConfiguration configuration)
        {
            // Smtp
            services.AddSingleton<IBodyHtmlGenerator, BodyHtmlGenerator>();
            services.AddSingleton<IMailSender, MailSender>();
            services.AddSingleton<ISmtpMailService, SmtpMailService>();

            // SendGrid
            services.Configure<SendGridEmailSenderOptions>(configuration.GetSection("SendGrid"));
            services.AddScoped<ISendGridEmailService, SendGridEmailService>();

            return services;
        }
    }
}