using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SocialNetwork.API.Infrastructure.ExtensionConfigureServices;
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
                //.AddRedisCache(Configuration)
                .AddCustomCors()
                .AddCustomDbContext(Configuration)
                .AddRepositories()
                .AddServices()
                .AddIdentityServices(Configuration)
                .AddSwaggerDocumentation()
                .AddEmail(Configuration)
                .AddRateLimit()
                .AddCustomLogger();

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

            //app.UseRewriter(new RewriteOptions().AddRedirectToHttpsPermanent());

            app.UseRequestLocalization();

            app.UseHealthChecks("/health");
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            //app.UseCookiePolicy();
            app.UseRouting();
            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/chat");
            });
        }
    }
}