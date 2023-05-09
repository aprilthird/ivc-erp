using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message, string displayName = null, Dictionary<string, (string, string)> copies = null, IFormFile file = null);
    }
}
