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
using IVC.PE.ENTITIES.Models.Finance;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.UspModels.Finances;
using IVC.PE.WEB.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ServiceWorkerCronJobDemo.Jobs
{
    public class MyCronJob1 : CronJobService
    {

        //public class FunClass
        //{
        //    private DbContextOptions<IvcDbContext> _dbContextOptions;

        //    public FunClass(DbContextOptions<IvcDbContext> dbContextOptions)
        //    {
        //        _dbContextOptions = dbContextOptions;
        //    }

        //    public async Task<List<UspBonds>> GetBonds()
        //    {
        //        using var _context = new IvcDbContext(_dbContextOptions);
        //        var bonds = await _context.Set<UspBonds>().FromSqlRaw("execute Finances_uspBonds")
        //        .Where(x => !x.IsTheLast)
        //        .IgnoreQueryFilters()
        //        .ToListAsync();

        //        return bonds;
        //    }

        //    public async Task<List<BondRenovationApplicationUser>> GetResponsibles()
        //    {
        //        using var _context = new IvcDbContext(_dbContextOptions);
        //        var bondResponsibles = await _context.BondRenovationApplicationUsers.ToListAsync();

        //        return bondResponsibles;
        //    }

        //    public async Task<List<ApplicationUser>> GetUsers()
        //    {
        //        using var _context = new IvcDbContext(_dbContextOptions);
        //        var users = await _context.Users.ToListAsync();
        //        return users;
        //    }
        //}

        public MyCronJob1(IScheduleConfig<MyCronJob1> config, ILogger<MyCronJob1> logger, IServiceScopeFactory scopeFactory, IEmailSender emailSender)
            : base(config.CronExpression, config.TimeZoneInfo, logger, scopeFactory, emailSender)
        {

        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJob 1 starts.");
            return base.StartAsync(cancellationToken);
        }

        public override async Task DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{DateTime.Now:hh:mm:ss} CronJob 1 is working.");

            using (var scope = _scopeFactory.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<IvcDbContext>();

                var bonds = await _context.Set<UspBonds>().FromSqlRaw("execute Finances_uspBonds")
                .IgnoreQueryFilters()
                .ToListAsync();

                bonds = bonds.Where(x => !x.IsTheLast).ToList();

                var users = await _context.Users.ToListAsync();

                var bondResponsibles = await _context.BondRenovationApplicationUsers.ToListAsync();

                var bond30days = bonds.Where(x => x.Validity <= 30 && x.Validity > 15 && !x.Days30).ToList();

                var bond15days = bonds.Where(x => x.Validity <= 15 && x.Validity > 0 && !x.Days15).ToList();

                var bond0days = bonds.Where(x => x.Validity == 0).ToList();

                foreach (var bond in bond30days)
                {
                    var responsibles = bondResponsibles.FirstOrDefault(x => x.BondRenovationId == bond.BondRenovationId).UserId;

                    SendAlertMail(responsibles.Split(','), bond, users);

                    var bondRenovation = await _context.BondRenovations.FirstOrDefaultAsync(x => x.Id == bond.BondRenovationId);
                    bondRenovation.Days30 = true;
                    await _context.SaveChangesAsync();
                }

                foreach (var bond in bond15days)
                {
                    var responsibles = bondResponsibles.FirstOrDefault(x => x.BondRenovationId == bond.BondRenovationId).UserId;

                    SendAlertMail(responsibles.Split(','), bond, users);

                    var bondRenovation = await _context.BondRenovations.FirstOrDefaultAsync(x => x.Id == bond.BondRenovationId);
                    bondRenovation.Days15 = true;
                    await _context.SaveChangesAsync();
                }

                foreach (var bond in bond0days)
                {
                    var responsibles = bondResponsibles.FirstOrDefault(x => x.BondRenovationId == bond.BondRenovationId).UserId;

                    SendAlertMail(responsibles.Split(','), bond, users);
                }
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJob 1 is stopping.");
            return base.StopAsync(cancellationToken);
        }

        private async void SendAlertMail(string[] responsibles, UspBonds bond, List<ApplicationUser> users)
        {
            //Cargar Archivo Adjunto
            MemoryStream ms = new MemoryStream();
            Attachment attachment = null;
            if (bond.FileUrl != null)
            {
                using (var wc = new WebClient())
                {
                    byte[] bondBytes = wc.DownloadData(bond.FileUrl);
                    ms = new MemoryStream(bondBytes);
                }
                attachment = new Attachment(ms, $"{bond.ProjectAbbreviation}_carta_fianza_{bond.BondNumber}-{bond.BondOrder}.pdf");
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

                mailMessage.Subject = $"IVC - Vencimiento de Fianza";

                var user = users.FirstOrDefault(x => x.Id.Equals(responsible));
                if (user != null)
                {
                    mailMessage.To.Add(new MailAddress(user.Email, user.RawFullName));
                    mailMessage.Body =
                        $"Hola, { user.FullName }, <br /><br /> " +
                        $"Favor de coordinar la renovación de Fianza del centro de costo { bond.ProjectAbbreviation }, { bond.BondType }, N° { bond.BondNumber } de { bond.PenAmmountFormatted }  que tiene como fecha de vencimiento  { bond.EndDateString }. <br /><br />" +
                        $"El responsable de renovar esta fianza es {bond.BondGuarantor}<br />" +
                        $"<br />" +
                        $"Saludos <br />" +
                        $"Sistema IVC <br />" +
                        $"Control de Fianzas";
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
