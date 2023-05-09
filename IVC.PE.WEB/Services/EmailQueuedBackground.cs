using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.General;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Services
{
    public class EmailItem
    {
        public List<MailAddress> To { get; set; } = new List<MailAddress>();
        public List<MailAddress> Cc { get; set; } = new List<MailAddress>();
        public string Subject { get; set; }
        public string Message { get; set; }
        public List<Attachment> Attachments { get; set; } = new List<Attachment>();
        public Func<Task> OnMailSucceeded { get; set; }
        public Func<Task> OnMailError { get; set; }
    }

    public interface IEmailQueuedBackground
    {
        void QueueBackgroundEmailItem(EmailItem emailItem);
    }

    public sealed class EmailQueuedBackground : BackgroundService, IEmailQueuedBackground
    {
        private static readonly ConcurrentQueue<EmailItem> _emailItems = new ConcurrentQueue<EmailItem>();
        private static readonly SemaphoreSlim _signal = new SemaphoreSlim(0);

        private readonly ILogger _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public EmailQueuedBackground(ILoggerFactory loggerFactory,
            IServiceScopeFactory scopeFactory)
        {
            _logger = loggerFactory.CreateLogger<EmailQueuedBackground>();
            _scopeFactory = scopeFactory;
        }

        /// <summary>
		/// Transient method via IQueuedBackgroundService
		/// </summary>
        public void QueueBackgroundEmailItem(EmailItem emailItem)
        {
            if (emailItem == null)
            {
                throw new ArgumentNullException(nameof(emailItem));
            }

            _emailItems.Enqueue(emailItem);
            _signal.Release();
            _logger.LogInformation("Queued Hosted Service is starting. Id:" + emailItem.To.First());
        }

        /// <summary>
		/// Long running task via BackgroundService
		/// </summary>
        protected async override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Queued Hosted Service is starting.");

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await _signal.WaitAsync(cancellationToken);
                    var _racsItem = _emailItems.TryDequeue(out var emailItem) ? emailItem : null;
                    _logger.LogInformation("Getting racsItem.");
                    if (emailItem != null)
                    {
                        _logger.LogInformation("Trying to create scope.");
                        using (var scope = _scopeFactory.CreateScope())
                        {
                            var _context = scope.ServiceProvider.GetRequiredService<IvcDbContext>();
                        }

                        _logger.LogInformation("Creating Mail Message...");
                        var mailMessage = new MailMessage
                        {
                            From = new MailAddress("sistemaerp@ivc.pe", "Sistema IVC")
                        };

                        mailMessage.Subject = emailItem.Subject;

                        foreach (var to in emailItem.To)
                            mailMessage.To.Add(to);
                        foreach (var cc in emailItem.Cc)
                            mailMessage.CC.Add(cc);

                        mailMessage.Body = emailItem.Message;

                        mailMessage.IsBodyHtml = true;

                        if (emailItem.Attachments.Any())
                        {
                            foreach (var atc in emailItem.Attachments)
                                mailMessage.Attachments.Add(atc);
                        }

                        //Mandar Correo
                        _logger.LogInformation("Sending mail...");
                        using (var client = new SmtpClient("smtp.office365.com", 587))
                        {
                            client.UseDefaultCredentials = false;
                            client.Credentials = new NetworkCredential("sistemaerp@ivc.pe", "S1st3m4erp");
                            client.EnableSsl = true;
                            client.DeliveryFormat = SmtpDeliveryFormat.International;
                            try
                            {
                                await client.SendMailAsync(mailMessage);
                                _logger.LogInformation("Mail Sended...");
                                emailItem.OnMailSucceeded?.Invoke();
                            }
                            catch
                            {
                                emailItem.OnMailError?.Invoke();
                            }
                        }
                    }
                }
                catch (TaskCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,
                       "Error occurred executing Email Background Service.");
                }
            }

            _logger.LogInformation("Queued Hosted Service is stopping.");
        }
    }
}
