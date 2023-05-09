using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Services
{
    public interface IRacsQueuedBackground
    {
        void QueueBackgroundRacsItem(RacsReport racsItem);
    }

    public sealed class RacsQueuedBackground : BackgroundService, IRacsQueuedBackground
    {
        private static readonly ConcurrentQueue<RacsReport> _racsItems = new ConcurrentQueue<RacsReport>();
        private static readonly SemaphoreSlim _signal = new SemaphoreSlim(0);


        private readonly ILogger _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public RacsQueuedBackground(ILoggerFactory loggerFactory,
            IServiceScopeFactory scopeFactory)
        {
            _logger = loggerFactory.CreateLogger<PayrollQueuedBackground>();
            _scopeFactory = scopeFactory;
        }

        /// <summary>
		/// Transient method via IQueuedBackgroundService
		/// </summary>
        public void QueueBackgroundRacsItem(RacsReport racsItem)
        {
            if (racsItem == null)
            {
                throw new ArgumentNullException(nameof(racsItem));
            }

            _racsItems.Enqueue(racsItem);
            _signal.Release();
            _logger.LogInformation("Queued Hosted Service is starting. Id:" + racsItem.Id);
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
                    var _racsItem = _racsItems.TryDequeue(out var racsItem) ? racsItem : null;
                    _logger.LogInformation("Getting racsItem.");
                    if (racsItem != null)
                    {
                        _logger.LogInformation("Trying to create scope.");

                        using (var scope = _scopeFactory.CreateScope())
                        {
                            var _context = scope.ServiceProvider.GetRequiredService<IvcDbContext>();
                            _logger.LogInformation("Getting receivers...");
                            var issuer = await _context.Users.FirstOrDefaultAsync(x => x.Id == _racsItem.ApplicationUserId);
                            var lifter = await _context.Users.FirstOrDefaultAsync(x => x.Id == _racsItem.LiftResponsibleId);

                            //Cargar Archivo Adjunto
                            _logger.LogInformation("Getting attachments...");
                            MemoryStream ms = new MemoryStream();
                            Attachment attachment = null;
                            Uri serviceUrl = new Uri($"https://erp-ivc-pdf.azurewebsites.net/api/functionapp?url=https://erp-ivc.azurewebsites.net/seguridad/racs/generar-pdf/{_racsItem.Id}");
                            _logger.LogInformation("Trying WebClient...");
                            using (var wc = new WebClient())
                            {
                                byte[] bondBytes = wc.DownloadData(serviceUrl);
                                ms = new MemoryStream(bondBytes);
                            }
                            attachment = new Attachment(ms, $"{_racsItem.Code}.pdf");

                            _logger.LogInformation("Creating Mail Message...");
                            var mailMessage = new MailMessage
                            {
                                From = new MailAddress("sistemaerp@ivc.pe", "Sistema IVC")
                            };

                            _logger.LogInformation("Attaching Attachment...");
                            if (attachment != null)
                                mailMessage.Attachments.Add(attachment);

                            mailMessage.Subject = $"IVC - Registro de RACS - {_racsItem.Code}";

                            if (issuer != null && lifter != null)
                            {
                                //mailMessage.To.Add(new MailAddress("rd.lazaroc@gmail.com", "Ricardo Lazaro"));
                                mailMessage.To.Add(new MailAddress(lifter.Email, lifter.RawFullName));
                                mailMessage.CC.Add(new MailAddress(issuer.Email, issuer.RawFullName));

                                if (_racsItem.Status == ConstantHelpers.RacsReports.IN_PROCESS)
                                {
                                    mailMessage.Body =
                                        $"Hola, { lifter.FullName }, <br /><br /> " +
                                        $"Se le informa que el {_racsItem.Code} ha sido creado.<br />" +
                                        $"Le adjuntamos el RACS para su conocimiento. <br /><br />" +
                                        $"Saludos <br />" +
                                        $"Sistema IVC";
                                } else
                                {
                                    mailMessage.Body =
                                        $"Hola, { lifter.FullName }, <br /><br /> " +
                                        $"Se le informa que el {_racsItem.Code} ha sido levantado.<br />" +
                                        $"Le adjuntamos el RACS para su conocimiento. <br /><br />" +
                                        $"Saludos <br />" +
                                        $"Sistema IVC";
                                }

                                mailMessage.IsBodyHtml = true;

                                //Mandar Correo
                                _logger.LogInformation("Sending mail...");
                                using (var client = new SmtpClient("smtp.office365.com", 587))
                                {
                                    client.UseDefaultCredentials = false;
                                    client.Credentials = new NetworkCredential("sistemaerp@ivc.pe", "S1st3m4erp");
                                    client.EnableSsl = true;
                                    await client.SendMailAsync(mailMessage);
                                }
                                _logger.LogInformation("Mail Sended...");
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
                       "Error occurred executing RacsQueued.");
                }
            }

            _logger.LogInformation("Queued Hosted Service is stopping.");
        }
    }
}
