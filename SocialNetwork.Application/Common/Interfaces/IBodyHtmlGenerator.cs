using SocialNetwork.Application.Common.Models.Emails;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace SocialNetwork.Application.Common.Interfaces
{
    public interface IBodyHtmlGenerator
    {
        Task<string> GenerateBodyAsync(string html, MailComplieModel model);
    }
}