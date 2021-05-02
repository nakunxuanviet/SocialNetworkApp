using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Ui.Web;
using SocialNetwork.API.Infrastructure.ExtensionConfigureServices;
using SocialNetwork.API.Infrastructure.Extensions;
using SocialNetwork.API.Infrastructure.Middlewares;
using SocialNetwork.API.Infrastructure.SignalR;
using SocialNetwork.Application;
using SocialNetwork.Infrastructure;
using SocialNetwork.Infrastructure.Persistence;

namespace SocialNetwork.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication()
                .AddCustomController()
                .AddAndConfigureLocalization()
                .AddRedisCache(Configuration)
                .AddCustomCors()
                .AddCustomDbContext(Configuration)
                .AddRepositories()
                .AddServices()
                .AddIdentityServices(Configuration)
                .AddSwaggerDocumentation()
                .AddEmail(Configuration)
                .AddRateLimit()
                .AddCustomLogger();

            services.AddConfigSerilogUI(Configuration);

            services.AddSignalR();

            services.AddHttpContextAccessor();

            services.AddAndConfigureApiVersioning();

            services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwaggerDocumentation(provider);
            }

            app.UseSerilogRequestLogging();

            //app.UseMiddleware<ApiExceptionHandlingMiddleware>();

            //app.UseRewriter(new RewriteOptions().AddRedirectToHttpsPermanent());

            app.UseRequestLocalization();

            app.UseSerilogRequestLogging();

            app.UseHealthChecks("/health");
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            //app.UseCookiePolicy();
            app.UseRouting();
            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            // Default url to view log page is http://<your-app>/serilog-ui. If config RoutePrefix then http://<your-app>/logs
            //app.UseSerilogUi();
            app.UseSerilogUi(option => option.RoutePrefix = "logs");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/chat");
            });
        }
    }
}