using SocialNetwork.Application.Common.Models.Emails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Application.Common.Interfaces
{
    public interface ISmtpMailService
    {
        // Send email using Task Factory background
        void SendSmtpMail(EmailRequest mailInfo);

        // Send email using delegate
        void Send(string[] toEmails, string subject, string bodyTemplate, MailComplieModel model);
    }
}