using HUG.EmailServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HUG.EmailServices.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailSettings emailSettings, MailRequest mailRequest);
    }
}
