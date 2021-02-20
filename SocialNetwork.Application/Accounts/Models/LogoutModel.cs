using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Application.Accounts.Models
{
    public class LogoutModel
    {
        [Required]
        public string Email { get; set; }
    }
}