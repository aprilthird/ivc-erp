using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.UspModels.Quality;
using IVC.PE.WEB.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using IVC.PE.WEB.Controllers;

namespace ServiceWorkerCronJobDemo.Jobs
{
    public class PatternCalibrationJob : CronJobService
    {
        public PatternCalibrationJob(IScheduleConfig<PatternCalibrationJob> config, ILogger<PatternCalibrationJob> logger, IServiceScopeFactory scopeFactory, IEmailSender emailSender)
    : base(config.CronExpression, config.TimeZoneInfo, logger, scopeFactory, emailSender)
        {

        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJob PatternCalibration starts.");
            return base.StartAsync(cancellationToken);
        }

        public override async Task DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{DateTime.Now:hh:mm:ss} PatternCalibrationJob 1 is working.");

            using (var scope = _scopeFactory.CreateScope())
            {

                var _context = scope.ServiceProvider.GetRequiredService<IvcDbContext>();

                var pats = await _context.Set<UspPatternCalibrations>().FromSqlRaw("execute Quality_uspPatternCalibrations")
                .IgnoreQueryFilters()
                .ToListAsync();

                pats = pats.ToList();

                var users = await _context.Users.ToListAsync();

                var patsResponsibles = await _context.PatternCalibrationRenewalApplicationUsers.ToListAsync();

                var pats30days = pats.Where(x => x.Validity <= 30 && x.Validity > 15 && !x.Days30).ToList();

                var pats15days = pats.Where(x => x.Validity <= 15 && x.Validity > 0 && !x.Days15).ToList();

                var pats0days = pats.Where(x => x.Validity == 0).ToList();

                foreach (var pat in pats30days)
                {
                    var responsibles = patsResponsibles.FirstOrDefault(x => x.PatternCalibrationRenewalId == pat.RenewalId).UserId;

                    SendAlertMail(responsibles.Split(','), pat, users);

                    var renewal = await _context.PatternCalibrationRenewals.FirstOrDefaultAsync(x => x.Id == pat.RenewalId);
                    renewal.Days30 = true;
                    await _context.SaveChangesAsync();
                }

                foreach (var pat in pats15days)
                {
                    var responsibles = patsResponsibles.FirstOrDefault(x => x.PatternCalibrationRenewalId == pat.RenewalId).UserId;

                    SendAlertMail(responsibles.Split(','), pat, users);
                    var renewal = await _context.PatternCalibrationRenewals.FirstOrDefaultAsync(x => x.Id == pat.RenewalId);
                    renewal.Days15 = true;
                    await _context.SaveChangesAsync();
                }

                foreach (var pat in pats0days)
                {
                    var responsibles = patsResponsibles.FirstOrDefault(x => x.PatternCalibrationRenewalId == pat.RenewalId).UserId;

                    SendAlertMail(responsibles.Split(','), pat, users);
                }
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJob PatternCalibration is stopping.");
            return base.StopAsync(cancellationToken);
        }

        private async void SendAlertMail(string[] responsibles, UspPatternCalibrations pat, List<ApplicationUser> users)
        {
            //Cargar Archivo Adjunto
            MemoryStream ms = new MemoryStream();
            Attachment attachment = null;
            if (pat.FileUrl != null)
            {    using (var wc = new WebClient())
                {
                    byte[] bondBytes = wc.DownloadData(pat.FileUrl);
                    ms = new MemoryStream(bondBytes);
                }
            attachment = new Attachment(ms, $"{pat.ProjectAbbreviation}_patron_calibración_{pat.ReferenceNumber}-{pat.RenewalOrder}.pdf");
            }
            //Buscar en users para obtener los correos.
            foreach (var responsible in responsibles)
            {
                var mailMessage = new MailMessage
                {
                    From = new MailAddress("sistemaerp@ivc.pe", "Sistema IVC")
                };

                if (attachment != null)
                    mailMessage.Attachments.Add(attachment);

                mailMessage.Subject = $"IVC - Vencimiento de Patrón de Calibración";

                var user = users.FirstOrDefault(x => x.Id.Equals(responsible));
                if (user != null)
                {
                    mailMessage.To.Add(new MailAddress( user.Email , user.RawFullName));
                    mailMessage.Body =
                        $"Hola, { user.FullName }, <br /><br /> " +
                        $"Favor de coordinar la renovación del Patrón de Calibración  { pat.ProjectAbbreviation }  -  { pat.ReferenceNumber}  -   que tiene como fecha de vencimiento  { pat.EndDateStr }. <br /><br />" +
                        $"Saludos <br />" +
                        $"Sistema IVC <br />" +
                        $"Control de Patrón de Calibración";
                    mailMessage.IsBodyHtml = true;

                    //Mandar Correo
                    using (var client = new SmtpClient("smtp.office365.com", 587))
                    {
                        client.UseDefaultCredentials = false;
                        client.Credentials = new NetworkCredential("sistemaerp@ivc.pe", "S1st3m4erp");
                        client.EnableSsl = true;
                        
                        await client.SendMailAsync(mailMessage);

                        await Task.Delay(15000);
                    }
                }
                // mail.Body("Descarga la carta fianza <a href={bond.FileUrl}>AQUÍ</a>");
            }
        }
    }
}
