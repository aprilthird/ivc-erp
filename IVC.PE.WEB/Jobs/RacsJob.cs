using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.Security;
using IVC.PE.WEB.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ServiceWorkerCronJobDemo.Jobs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceWorkerCronJobDemo.Jobs
{
    public class RacsJob : CronJobService
    {
        public RacsJob(IScheduleConfig<RacsJob> config, 
            ILogger<RacsJob> logger, 
            IServiceScopeFactory scopeFactory, 
            IEmailSender emailSender)
            : base(config.CronExpression, 
                  config.TimeZoneInfo, 
                  logger, 
                  scopeFactory, 
                  emailSender)
        {

        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJob Racs starts.");
            return base.StartAsync(cancellationToken);
        }

        public override async Task DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{DateTime.Now:hh:mm:ss} CronJob Racs is working.");

            using (var scope = _scopeFactory.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<IvcDbContext>();

                var racsToSend = await _context.RacsReports
                    .Where(x => !x.IsMailSended)
                    .ToListAsync();

                //var racsLifted = await _context.RacsReports
                //    .Where(x => !x.IsLiftMailSended)
                //    .ToListAsync();

                var users = await _context.Users.ToListAsync();

                foreach (var racs in racsToSend)
                {
                    var issuer = users.First(x => x.Id == racs.ApplicationUserId);
                    var lifter = users.First(x => x.Id == racs.LiftResponsibleId);

                    SendAlertMail(racs, issuer, lifter);
                    racs.IsMailSended = true;
                    _logger.LogInformation($"{DateTime.Now:hh:mm:ss} CronJob Racs is sending {racs.Code}.");
                }

                //foreach (var racs in racsLifted)
                //{
                //    var issuer = users.First(x => x.Id == racs.ApplicationUserId);
                //    var lifter = users.First(x => x.Id == racs.LiftResponsibleId);

                //    SendAlertMailLift(racs, issuer, lifter);
                //    racs.IsMailSended = true;
                //    _logger.LogInformation($"{DateTime.Now:hh:mm:ss} CronJob Racs is sending {racs.Code}.");
                //}

                await _context.SaveChangesAsync();
            }

            _logger.LogInformation($"{DateTime.Now:hh:mm:ss} CronJob Racs is idling.");
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJob Racs is stopping.");
            return base.StopAsync(cancellationToken);
        }

        private async void SendAlertMail(RacsReport racs, ApplicationUser issuer, ApplicationUser lifter)
        {
            //Cargar Archivo Adjunto
            MemoryStream ms = new MemoryStream();
            Attachment attachment = null;
            Uri serviceUrl = new Uri($"https://erp-ivc-pdf.azurewebsites.net/api/functionapp?url=https://erp-ivc.azurewebsites.net/seguridad/racs/generar-pdf/{racs.Id}");
            using (var wc = new WebClient())
            {
                byte[] bondBytes = wc.DownloadData(serviceUrl);
                ms = new MemoryStream(bondBytes);
            }
            attachment = new Attachment(ms, $"{racs.Code}.pdf");

            var mailMessage = new MailMessage
            {
                From = new MailAddress("sistemaerp@ivc.pe", "Sistema IVC")
            };

            if (attachment != null)
                mailMessage.Attachments.Add(attachment);

            mailMessage.Subject = $"IVC - Registro de RACS - {racs.Code}";

            if (issuer != null && lifter != null)
            {
                mailMessage.To.Add(new MailAddress(lifter.Email, lifter.RawFullName));
                mailMessage.CC.Add(new MailAddress(issuer.Email, issuer.RawFullName));
                mailMessage.Body =
                    $"Hola, { lifter.FullName }, <br /><br /> " +
                    $"Ha sido registrado como responsable del {racs.Code}.<br />" +
                    $"Favor de coordinar el levantamiento del mismo. <br /><br />" +
                    $"Saludos <br />" +
                    $"Sistema IVC";
                mailMessage.IsBodyHtml = true;

                //Mandar Correo
                using (var client = new SmtpClient("smtp.office365.com", 587))
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("sistemaerp@ivc.pe", "S1st3m4erp");
                    client.EnableSsl = true;
                    await client.SendMailAsync(mailMessage);
                }
            }
        }

        private async void SendAlertMailLift(RacsReport racs, ApplicationUser issuer, ApplicationUser lifter)
        {
            //Cargar Archivo Adjunto
            MemoryStream ms = new MemoryStream();
            Attachment attachment = null;
            Uri serviceUrl = new Uri($"https://erp-ivc-pdf.azurewebsites.net/api/functionapp?url=https://erp-ivc.azurewebsites.net/seguridad/racs/generar-pdf/{racs.Id}");
            using (var wc = new WebClient())
            {
                byte[] bondBytes = wc.DownloadData(serviceUrl);
                ms = new MemoryStream(bondBytes);
            }
            attachment = new Attachment(ms, $"{racs.Code}.pdf");

            var mailMessage = new MailMessage
            {
                From = new MailAddress("sistemaerp@ivc.pe", "Sistema IVC")
            };

            if (attachment != null)
                mailMessage.Attachments.Add(attachment);

            mailMessage.Subject = $"IVC - Registro de RACS - {racs.Code}";

            if (issuer != null && lifter != null)
            {
                mailMessage.To.Add(new MailAddress(lifter.Email, lifter.RawFullName));
                mailMessage.CC.Add(new MailAddress(issuer.Email, issuer.RawFullName));
                mailMessage.Body =
                    $"Hola, { lifter.FullName }, <br /><br /> " +
                    $"Se le informa que el {racs.Code} ha sido levantado.<br />" +
                    $"Le adjuntamos el RACS para su conocimiento. <br /><br />" +
                    $"Saludos <br />" +
                    $"Sistema IVC";
                mailMessage.IsBodyHtml = true;

                //Mandar Correo
                using (var client = new SmtpClient("smtp.office365.com", 587))
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("sistemaerp@ivc.pe", "S1st3m4erp");
                    client.EnableSsl = true;
                    await client.SendMailAsync(mailMessage);
                }
            }
        }
    }
}
