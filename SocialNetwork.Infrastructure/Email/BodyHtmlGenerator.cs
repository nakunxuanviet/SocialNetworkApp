using RazorLight;
using SocialNetwork.Application.Common.Interfaces;
using SocialNetwork.Application.Common.Models.Emails;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace SocialNetwork.Infrastructure.Email
{
    public class BodyHtmlGenerator : IBodyHtmlGenerator
    {
        public Task<string> GenerateBodyAsync(string html, MailComplieModel model)
        {
            var engine = new RazorLightEngineBuilder()
                .UseEmbeddedResourcesProject(Assembly.GetExecutingAssembly())
                .UseMemoryCachingProvider()
                .Build();

            return engine.CompileRenderStringAsync(Guid.NewGuid().ToString(), html, model);
        }
    }
}