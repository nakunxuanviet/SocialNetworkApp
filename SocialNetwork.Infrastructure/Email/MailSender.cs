using Microsoft.Extensions.Configuration;
using NaKun.Arc.Domain.Exceptions;
using SocialNetwork.Application.Common.Interfaces;
using SocialNetwork.Application.Common.Models.Emails;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;
using System.Text;

namespace SocialNetwork.Infrastructure.Email
{
    public class MailSender : IMailSender
    {
        private readonly IConfiguration _configuration;
        private readonly SmtpClient _smtpClient;
        private readonly SmtpSettingOptions _settings;

        public delegate void SendMailHandler(string[] toEmails, string subject, string body, object attachs = null);

        public MailSender(IConfiguration configuration)
        {
            _configuration = configuration;
            _settings = _configuration.GetSection("SmtpSettings").Get<SmtpSettingOptions>();
            _smtpClient = new SmtpClient
            {
                Host = _settings.Host,
                EnableSsl = _settings.EnableSsl,
                Port = Convert.ToInt32(_settings.Port),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(_settings.Account, _settings.Password),
                Timeout = _settings.Timeout
            };
        }

        public SmtpClient CreateSmtpClient() => _smtpClient;

        public MailMessage CreateMailMessage(EmailRequest mailInfo)
        {
            var msg = new MailMessage
            {
                IsBodyHtml = true,
                Body = mailInfo.HtmlBody,
                BodyEncoding = Encoding.UTF8,
                SubjectEncoding = Encoding.UTF8
            };
            // Kiem tra ToMail
            if (mailInfo.Recipients != null && mailInfo.Recipients.Count > 0)
            {
                foreach (var toMail in mailInfo.Recipients)
                {
                    msg.To.Add(toMail);
                }
            }
            else
            {
                throw new DomainException("Did not find the mail to send !");
            }

            // Kiem tra cc mail
            if (mailInfo.CCs != null && mailInfo.CCs.Count > 0)
            {
                foreach (var ccMail in mailInfo.CCs)
                {
                    msg.CC.Add(ccMail);
                }
            }

            // Kiem tra bcc
            if (mailInfo.BCCs != null && mailInfo.BCCs.Count > 0)
            {
                foreach (var bccMail in mailInfo.BCCs)
                {
                    msg.Bcc.Add(bccMail);
                }
            }

            if (mailInfo.Attachments != null && mailInfo.Attachments.Any())
            {
                foreach (var file in mailInfo.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            var fileBytes = ms.ToArray();
                            Attachment att = new Attachment(new MemoryStream(fileBytes), file.FileName);
                            msg.Attachments.Add(att);
                        }
                    }
                }
            }

            msg.From = new MailAddress(_settings.Account, _settings.DisplayName);
            msg.Subject = mailInfo.Subject;

            return msg;
        }

        public void Send(string[] toEmails, string subject, string body)
        {
            Send(toEmails, subject, body, null);
        }

        private void Send(string[] toEmails, string subject, string body, object attachs = null)
        {
            var fromAddress = new MailAddress(_settings.Account, _settings.DisplayName);

            MailMessage message = new MailMessage()
            {
                From = fromAddress,
                Subject = subject,
                //Body       = body,
                IsBodyHtml = true
            };

            // Create the HTML view
            var htmlView = AlternateView.CreateAlternateViewFromString(body, Encoding.UTF8, MediaTypeNames.Text.Html);
            var imageLinked = EmailLogoLinkedResource();
            htmlView.LinkedResources.Add(imageLinked);
            message.AlternateViews.Add(htmlView);

            foreach (var toEmail in toEmails)
            {
                message.To.Add(toEmail);
            }

            _smtpClient.Send(message);
        }

        private LinkedResource EmailLogoLinkedResource()
        {
            var assembly = Assembly.GetAssembly(typeof(MailSender));
            var imageStream = assembly.GetManifestResourceStream("EVN.Common.Resources.Images");

            var resource = new LinkedResource(imageStream);
            resource.ContentId = "logo_embedded";
            resource.ContentType.MediaType = MediaTypeNames.Image.Jpeg;
            resource.TransferEncoding = TransferEncoding.Base64;
            resource.ContentType.Name = resource.ContentId;
            resource.ContentLink = new Uri("cid:" + resource.ContentId);

            return resource;
        }
    }
}