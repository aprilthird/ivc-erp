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
using IVC.PE.ENTITIES.Models.Bidding;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.UspModels.Biddings;
using IVC.PE.WEB.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ServiceWorkerCronJobDemo.Jobs
{
    public class LegalDocumentationCronJob : CronJobService
    {
        public LegalDocumentationCronJob(IScheduleConfig<LegalDocumentationCronJob> config, ILogger<LegalDocumentationCronJob> logger, IServiceScopeFactory scopeFactory, IEmailSender emailSender)
        : base(config.CronExpression, config.TimeZoneInfo, logger, scopeFactory, emailSender)
        {

        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Cronjob Legal Documentation starts.");
            return base.StartAsync(cancellationToken);
        }

        public override async Task DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{DateTime.Now:hh:mm:ss} Cronjob Legal Documentation is working.");

            using (var scope = _scopeFactory.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<IvcDbContext>();

                var legaldocs = await _context.Set<UspLegalDocumentations>().FromSqlRaw("execute Biddings_uspLegalDocumentations")
                .IgnoreQueryFilters()
                .ToListAsync();

                legaldocs = legaldocs.Where(x => !x.IsTheLast).ToList();

                var users = await _context.Users.ToListAsync();

                var legalDocsResponsibles = await _context.BusinessResponsibles.Where(x=>x.SendEmail).ToListAsync();

                var legalDocs5days = legaldocs.Where(x => x.Validity <= 5 && x.Validity > 0 && !x.Days5).ToList();

                var legalDocs0days = legaldocs.Where(x => x.Validity == 0).ToList();

                foreach (var legalDoc in legalDocs5days)
                {
                    SendAlertMail(legalDocsResponsibles, legalDoc, users);

                    var legalDocRenovation = await _context.LegalDocumentationRenovations.FirstOrDefaultAsync(x => x.Id == legalDoc.LegalDocumentationRenovationId);
                    legalDocRenovation.Days5 = true;
                    await _context.SaveChangesAsync();
                }

                foreach (var legalDoc in legalDocs0days)
                {
                    SendAlertMail(legalDocsResponsibles, legalDoc, users);
                }
            }
        }

        private async void SendAlertMail(List<BusinessResponsible> responsibles, UspLegalDocumentations legalDoc, List<ApplicationUser> users)
        {
            //Cargar Archivo Adjunto
            MemoryStream ms = new MemoryStream();
            Attachment attachment = null;
            if (legalDoc.FileUrl != null)
            {    using (var wc = new WebClient())
                {
                    byte[] legalDocBytes = wc.DownloadData(legalDoc.FileUrl);
                    ms = new MemoryStream(legalDocBytes);
                }
                attachment = new Attachment(ms, $"{legalDoc.BusinessName}_licitacion_{legalDoc.LegalDocumentationOrder}.pdf");
            }
            //Buscar en users para obtener los correos.
            foreach (var responsible in responsibles)
            {
                var mailMessage = new MailMessage
                {
                    From = new MailAddress("sistemaivctest@gmail.com", "Sistema IVC")
                };

                if (attachment != null)
                    mailMessage.Attachments.Add(attachment);

                mailMessage.Subject = $"IVC - Vencimiento de Documentacion Legal";

                var user = users.FirstOrDefault(x => x.Id.Equals(responsible.UserId));
                if (user != null)
                {
                    mailMessage.To.Add(new MailAddress(user.Email, user.RawFullName));
                    mailMessage.Body =
                        $"Hola, { user.FullName }, <br /><br /> " +
                        $"Favor de revisar el Documento adjunto que está proximo a vencer <br /><br />" +
                        $"Saludos <br />" +
                        $"Sistema IVC <br />" +
                        $"Control de Licitación";
                    mailMessage.IsBodyHtml = true;

                    //Mandar Correo
                    using (var client = new SmtpClient("smtp.gmail.com", 587))
                    {
                        client.UseDefaultCredentials = false;
                        client.Credentials = new NetworkCredential("sistemaivctest@gmail.com", "IVC.12345");
                        client.EnableSsl = true;
                        await client.SendMailAsync(mailMessage);
                    }
                }
                // mail.Body("Descarga la carta fianza <a href={bond.FileUrl}>AQUÍ</a>");
            }
        }
    }
}
