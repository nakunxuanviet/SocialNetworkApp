using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Application.Common.Models.Emails
{
    public class EmailRequest
    {
        public EmailRequest()
        {
            Recipients = new List<string>();
            CCs = new List<string>();
            BCCs = new List<string>();
        }

        public EmailRequest(string subject, IList<string> recipients, string htmlBody)
        {
            Subject = subject;
            Recipients = recipients;
            HtmlBody = htmlBody;
        }

        /// <summary>
        /// Địa chỉ gửi mail
        /// </summary>
        public string FromAddress { get; set; }

        /// <summary>
        /// Tiêu đề mail
        /// </summary>
        [Required]
        public string Subject { get; set; }

        /// <summary>
        /// Danh sách người nhận mail
        /// </summary>
        [Required]
        public IList<string> Recipients { get; set; }

        public IList<string> CCs { get; set; }

        public IList<string> BCCs { get; set; }

        [Required]
        public string HtmlBody { get; set; }

        /// <summary>
        /// Danh sách file đính kèm
        /// </summary>
        public List<IFormFile> Attachments { get; set; }
    }
}