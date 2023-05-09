using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.WEB.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using IVC.PE.WEB.Controllers;
using IVC.PE.ENTITIES.UspModels.DocumentaryControl;
using IVC.PE.ENTITIES.Models.General;

namespace ServiceWorkerCronJobDemo.Jobs
{
    public class PermissionCronjob : CronJobService
    {
        public PermissionCronjob(IScheduleConfig<PermissionCronjob> config, ILogger<PermissionCronjob> logger, IServiceScopeFactory scopeFactory, IEmailSender emailSender)
    : base(config.CronExpression, config.TimeZoneInfo, logger, scopeFactory, emailSender)
        {

        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJob PermissionCronjob starts.");
            return base.StartAsync(cancellationToken);


        }

        public override async Task DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{DateTime.Now:hh:mm:ss} CronJob 1 is working.");

            using (var scope = _scopeFactory.CreateScope())
            {

                var _context = scope.ServiceProvider.GetRequiredService<IvcDbContext>();

                var equips = await _context.Set<UspPermissions>().FromSqlRaw("execute DocumentaryControl_uspPermissions")
                .IgnoreQueryFilters()
                .ToListAsync();

                equips = equips.ToList();

                var users = await _context.Users.ToListAsync();

                var equipsResponsibles = await _context.PermissionRenovationApplicationUsers.ToListAsync();

                var equips30days = equips.Where(x => x.Validity <= 30 && x.Validity > 15 && !x.Days30).ToList();

                var equips15days = equips.Where(x => x.Validity <= 15 && x.Validity > 0 && !x.Days15).ToList();

                var equips0days = equips.Where(x => x.Validity == 0).ToList();

                foreach (var equip in equips30days)
                {
                    var responsibles = equipsResponsibles.FirstOrDefault(x => x.PermissionRenovationId== equip.BondRenovationId).UserId;

                    SendAlertMail(responsibles.Split(','), equip, users);

                    var renewal = await _context.PermissionRenovations.FirstOrDefaultAsync(x => x.Id == equip.BondRenovationId);
                    renewal.Days30 = true;
                    await _context.SaveChangesAsync();
                }

                foreach (var equip in equips15days)
                {
                    var responsibles = equipsResponsibles.FirstOrDefault(x => x.PermissionRenovationId == equip.BondRenovationId).UserId;

                    SendAlertMail(responsibles.Split(','), equip, users);
                    var renewal = await _context.PermissionRenovations.FirstOrDefaultAsync(x => x.Id == equip.BondRenovationId);
                    renewal.Days15 = true;
                    await _context.SaveChangesAsync();
                }

                foreach (var equip in equips0days)
                {
                    var responsibles = equipsResponsibles.FirstOrDefault(x => x.PermissionRenovationId == equip.BondRenovationId).UserId;

                    SendAlertMail(responsibles.Split(','), equip, users);
                }
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJob Permission is stopping.");
            return base.StopAsync(cancellationToken);
        }

        private async void SendAlertMail(string[] responsibles, UspPermissions equip, List<ApplicationUser> users)
        {
            //Cargar Archivo Adjunto
            MemoryStream ms = new MemoryStream();
            Attachment attachment = null;
            if (equip.FileUrl != null)
            {
                using (var wc = new WebClient())
                {
                    byte[] bondBytes = wc.DownloadData(equip.FileUrl);
                    ms = new MemoryStream(bondBytes);
                }
                attachment = new Attachment(ms, $"permiso_-{equip.AuthorizationNumber}-{equip.Order}.pdf");
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

                mailMessage.Subject = $"IVC - Vencimiento de Autorización/Permiso";

                var user = users.FirstOrDefault(x => x.Id.Equals(responsible));
                if (user != null)
                {
                    mailMessage.To.Add(new MailAddress(user.Email, user.RawFullName));
                    mailMessage.Body =
                        $"Hola, { user.FullName }, <br /><br /> " +
                        $"Favor de coordinar el vencimiento del Permiso y/o Autorización { equip.AuthorizingEntity }  -  { equip.AuthorizationNumber} - { equip.AuthorizationType} -{ equip.RenovationType} -   que tiene como fecha de vencimiento  { equip.EndDateString }. <br />" +
                        $"{equip.Length}:<br />" +
                        $"{equip.PrincipalWay}:<br />" +
                        $"{equip.From}:<br />" +
                        $"{equip.To}:<br /><br />" +
                        $"Saludos <br />" +
                        $"Sistema IVC <br />" +
                        $"Control de Permisos";
                    mailMessage.IsBodyHtml = true;

                    //Mandar Correo
                    using (var client = new SmtpClient("smtp.office365.com", 587))
                    {
                        client.UseDefaultCredentials = false;
                        client.Credentials = new NetworkCredential("sistemaerp@ivc.pe", "S1st3m4erp");
                        client.EnableSsl = true;
                        await client.SendMailAsync(mailMessage);
                        Thread.Sleep(15000);
                    }
                }
                // mail.Body("Descarga la carta fianza <a href={bond.FileUrl}>AQUÍ</a>");
            }
        }


    }
}
