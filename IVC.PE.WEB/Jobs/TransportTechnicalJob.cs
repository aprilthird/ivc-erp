using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.UspModels.EquipmentMachinery;
using IVC.PE.WEB.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
    public class TransportTechnicalJob : CronJobService
    {
        public TransportTechnicalJob(IScheduleConfig<TransportTechnicalJob> config, ILogger<TransportTechnicalJob> logger, IServiceScopeFactory scopeFactory, IEmailSender emailSender)
         : base(config.CronExpression, config.TimeZoneInfo, logger, scopeFactory, emailSender)
        {

        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJob TransportTech starts.");
            return base.StartAsync(cancellationToken);
        }

        public override async Task DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{DateTime.Now:hh:mm:ss} CronJob 1 is working.");

            using (var scope = _scopeFactory.CreateScope())
            {

                var _context = scope.ServiceProvider.GetRequiredService<IvcDbContext>();

                var equips = await _context.Set<UspTransportTechFolding>().FromSqlRaw("execute EquipmentMachinery_uspTransportTechFolding")
                .IgnoreQueryFilters()
                .ToListAsync();

                equips = equips.ToList();

                var users = await _context.Users.ToListAsync();

                var equipsResponsibles = await _context.EquipmentMachineryTransportTechnicalRevisionFoldingApplications.ToListAsync();

                var equips30days = equips.Where(x => x.Validity <= 30 && x.Validity > 15 && !x.Days30).ToList();

                var equips15days = equips.Where(x => x.Validity <= 15 && x.Validity > 0 && !x.Days15).ToList();

                var equips0days = equips.Where(x => x.Validity == 0).ToList();

                foreach (var equip in equips30days)
                {
                    var responsibles = equipsResponsibles.FirstOrDefault(x => x.EquipmentMachineryTransportTechnicalRevisionFoldingId == equip.FoldingId).UserId;

                    SendAlertMail(responsibles.Split(','), equip, users);

                    var renewal = await _context.EquipmentMachineryTransportTechnicalRevisions.FirstOrDefaultAsync(x => x.Id == equip.FoldingId);
                    renewal.Days30 = true;
                    await _context.SaveChangesAsync();
                }

                foreach (var equip in equips15days)
                {
                    var responsibles = equipsResponsibles.FirstOrDefault(x => x.EquipmentMachineryTransportTechnicalRevisionFoldingId == equip.FoldingId).UserId;

                    SendAlertMail(responsibles.Split(','), equip, users);

                    var renewal = await _context.EquipmentMachineryTransportTechnicalRevisions.FirstOrDefaultAsync(x => x.Id == equip.FoldingId);
                    renewal.Days15 = true;
                    await _context.SaveChangesAsync();
                }

                foreach (var equip in equips0days)
                {
                    var responsibles = equipsResponsibles.FirstOrDefault(x => x.EquipmentMachineryTransportTechnicalRevisionFoldingId == equip.FoldingId).UserId;

                    SendAlertMail(responsibles.Split(','), equip, users);
                }
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJob For35 is stopping.");
            return base.StopAsync(cancellationToken);
        }

        private async void SendAlertMail(string[] responsibles, UspTransportTechFolding equip, List<ApplicationUser> users)
        {
            //Cargar Archivo Adjunto
            MemoryStream ms = new MemoryStream();
            Attachment attachment = null;
            if (equip.TechnicalRevisionFileUrl != null)
            {
                using (var wc = new WebClient())
                {
                    byte[] bondBytes = wc.DownloadData(equip.TechnicalRevisionFileUrl);
                    ms = new MemoryStream(bondBytes);
                }
                attachment = new Attachment(ms, $"Revision_Tecnica_{equip.TechnicalOrder}_{equip.Model}_{equip.Brand}_{equip.EquipmentPlate}.pdf");
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

                mailMessage.Subject = $"IVC - Vencimiento de Revisión Técnica - Equipos Transporte";

                var user = users.FirstOrDefault(x => x.Id.Equals(responsible));
                if (user != null)
                {
                    mailMessage.To.Add(new MailAddress(user.Email, user.RawFullName));
                    mailMessage.Body =
                        $"Hola, { user.FullName }, <br /><br /> " +
                        $"Favor de coordinar la renovación de la Revisión Técnica del equipo {equip.Model} - {equip.Brand} - {equip.EquipmentPlate} - que le pertenece a {equip.Tradename} y tiene como fecha de vencimiento  { equip.EndDateTechnicalRevisionString } en el centro de costo {equip.Center}. <br /><br />" +
                        $"Saludos <br />" +
                        $"Sistema IVC <br />" +
                        $"Control de Equipo";
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
