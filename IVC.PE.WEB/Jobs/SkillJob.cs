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
using IVC.PE.ENTITIES.Models.Finance;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.UspModels.Biddings;
using IVC.PE.ENTITIES.UspModels.Finances;
using IVC.PE.WEB.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ServiceWorkerCronJobDemo.Jobs
{
    public class SkillJob : CronJobService
    {
        public SkillJob(IScheduleConfig<SkillJob> config, ILogger<SkillJob> logger, IServiceScopeFactory scopeFactory, IEmailSender emailSender)
    : base(config.CronExpression, config.TimeZoneInfo, logger, scopeFactory, emailSender)
        {

        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJob Skill starts.");
            return base.StartAsync(cancellationToken);
        }

        public override async Task DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{DateTime.Now:hh:mm:ss} CronJob 1 is working.");

            using (var scope = _scopeFactory.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<IvcDbContext>();

                var skills = await _context.Set<UspSkills>().FromSqlRaw("execute Biddings_uspSkills")
                .IgnoreQueryFilters()
                .ToListAsync();

                skills = skills.Where(x => !x.IsTheLast).ToList();

                var users = await _context.Users.ToListAsync();

                var skillResponsibles = await _context.BusinessResponsibles.ToListAsync();

                var skill30days = skills.Where(x => x.Validity <= 30 && x.Validity > 15 && !x.Days30).ToList();

                var skill15days = skills.Where(x => x.Validity <= 15 && x.Validity > 0 && !x.Days15).ToList();

                var skill0days = skills.Where(x => x.Validity == 0).ToList();

                foreach (var skill in skill30days)
                {
                    SendAlertMail(skillResponsibles, skill, users);

                    var skillRenovation = await _context.SkillRenovations.FirstOrDefaultAsync(x => x.Id == skill.SkillRenovationId);
                    skillRenovation.Days30 = true;
                    await _context.SaveChangesAsync();
                }

                foreach (var skill in skill15days)
                {
                    SendAlertMail(skillResponsibles, skill, users);

                    var skillRenovation = await _context.SkillRenovations.FirstOrDefaultAsync(x => x.Id == skill.SkillRenovationId);
                    skillRenovation.Days15 = true;
                    await _context.SaveChangesAsync();
                }

                foreach (var skill in skill0days)
                {
                    SendAlertMail(skillResponsibles, skill, users);
                }
            }
        }

        private async void SendAlertMail(List<BusinessResponsible> responsibles, UspSkills skill, List<ApplicationUser> users)
        {
            //Cargar Archivo Adjunto
            MemoryStream ms = new MemoryStream();
            Attachment attachment = null;
            if (skill.FileUrl != null)
            {
                using (var wc = new WebClient())
                {
                    byte[] skillBytes = wc.DownloadData(skill.FileUrl);
                    ms = new MemoryStream(skillBytes);
                }
                attachment = new Attachment(ms, $"{skill.ProfessionalName}_{skill.Profession}_{skill.SkillOrder}.pdf");
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

                mailMessage.Subject = $"IVC - Vencimiento de Control de Habilidad";

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
