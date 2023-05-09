using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string message,string displayName = null,Dictionary<string, (string, string)> copies = null, IFormFile file = null)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress("sistemaivctest@gmail.com", "Sistema IVC")
            };

            mailMessage.To.Add(new MailAddress( email, displayName ));
            mailMessage.Subject = subject;

            if(copies != null)
            foreach (var key in copies.Keys )
            {
                mailMessage.CC.Add(new MailAddress(copies[key].Item2, copies[key].Item1));
            }
            //mailMessage.CC.Add(new MailAddress("recepcion@jicamarca.pe", "recepcion"));
            mailMessage.Body =message;
            mailMessage.IsBodyHtml = true;
            if (file != null && file?.Length < 1024 * 1024 * 50)
                mailMessage.Attachments.Add(new Attachment(file.OpenReadStream(), file.FileName));
            using (var client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("sistemaivctest@gmail.com", "IVC.12345");
                client.EnableSsl = true;
                await client.SendMailAsync(mailMessage);
            }
        }
    }
}
