using SocialNetwork.Application.Common.Models.Emails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Application.Common.Interfaces
{
    public interface IMailSender
    {
        SmtpClient CreateSmtpClient();

        MailMessage CreateMailMessage(EmailRequest mailInfo);

        void Send(string[] toEmails, string subject, string body);
    }
}