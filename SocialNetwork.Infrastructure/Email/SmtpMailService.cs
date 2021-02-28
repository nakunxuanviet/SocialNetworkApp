using Microsoft.Extensions.Logging;
using SocialNetwork.Application.Common.Interfaces;
using SocialNetwork.Application.Common.Models.Emails;
using System;
using System.Threading.Tasks;

namespace SocialNetwork.Infrastructure.Email
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class SmtpMailService : ISmtpMailService
    {
        private readonly IMailSender _mailSender;
        private readonly ILogger _logger;
        private readonly IBodyHtmlGenerator _bodyGenerator;

        public delegate Task SendMailHandler(string[] toEmails, string subject, string bodyTemplate, MailComplieModel model);

        public SmtpMailService(IMailSender mailSender, IBodyHtmlGenerator bodyGenerator, ILogger<SmtpMailService> logger)
        {
            _mailSender = mailSender;
            _bodyGenerator = bodyGenerator;
            _logger = logger;
        }

        public void SendSmtpMail(EmailRequest mailInfo)
        {
            Task.Factory.StartNew(() =>
            {
                var msg = _mailSender.CreateMailMessage(mailInfo);
                var smtp = _mailSender.CreateSmtpClient();
                smtp.Send(msg);
            });
        }

        public void Send(string[] toEmails, string subject, string bodyTemplate, MailComplieModel model)
        {
            var handler = new SendMailHandler(SendMailBackground);
            Task.Run(() =>
            {
                handler(toEmails, subject, bodyTemplate, model);
            });
        }

        private async Task SendMailBackground(string[] toEmails, string subject, string bodyTemplate, MailComplieModel model)
        {
            try
            {
                var body = await _bodyGenerator.GenerateBodyAsync(bodyTemplate, model);
                _mailSender.Send(toEmails, subject, body);
            }
            catch (Exception)
            {
                _logger.LogError($"Email sending failed with Subject: {subject}");
            }
        }
    }
}