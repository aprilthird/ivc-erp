    using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using IVC.PE.BINDINGRESOURCES.Areas.Security;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.Security;
using IVC.PE.ENTITIES.UspModels.Security;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Options;
using IVC.PE.WEB.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IVC.PE.WEB.Api.Areas.Security
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/seguridad/racs")]
    public class RacsReportController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        private readonly IRacsQueuedBackground _queuedBackgroundService;
        private readonly IEmailQueuedBackground _emailQueuedBackgroundService;

        public RacsReportController(IvcDbContext context,
            ILogger<RacsReportController> logger,
            IOptions<CloudStorageCredentials> storageCredentials,
            IRacsQueuedBackground queuedBackgroundService,
            IEmailQueuedBackground emailQueuedBackgroundService)
            : base(context, logger)
        {
            _storageCredentials = storageCredentials;
            _queuedBackgroundService = queuedBackgroundService;
            _emailQueuedBackgroundService = emailQueuedBackgroundService;
        }

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? projectId, string reportDate)
        {
            if (projectId == Guid.Empty)
                return BadRequest("Debe seleccionar un proyecto.");
            if (string.IsNullOrEmpty(reportDate))
                return BadRequest("Debe seleccionar una fecha.");

            var reporteDateDt = reportDate.ToDateTime();

            SqlParameter projectParam = new SqlParameter("@ProjectId", projectId.Value);
            var racs = await _context.Set<UspRacsAll>().FromSqlRaw("execute Security_uspRacsAll @ProjectId"
                , projectParam)
                .IgnoreQueryFilters()
                .ToListAsync();

            var query = racs
                .Where(x => x.ReportDate.Date == reporteDateDt.Date)
                .Select(x => new RacsListResourceModel
                {
                    Id = x.Id,
                    Code = x.Code,
                    ReportDate = x.ReportDate.ToDateString(),
                    SewerGroupCode = x.SewerGroupCode,
                    Status = x.Status,
                    StatusStr = x.Status == 0 ? "\uf00d" : "\uf00c",
                    ReportUser = x.ReportUser
                }).ToList();

            return Ok(query);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid? id)
        {
            if (id == Guid.Empty)
                return BadRequest("No existe el RACS solicitado.");

            var query = await _context.RacsReports
                .Include(x => x.SewerGroup)
                .Where(x => x.Id == id.Value)
                .Select(x => new RacsResourceModel
                {
                    Id = x.Id,
                    Code = x.Code,
                    ProjectId = x.ProjectId,
                    ReportDate = x.ReportDate.ToDateString(),
                    ApplicationUserId = x.ApplicationUserId,
                    LiftResponsibleId = x.LiftResponsibleId,
                    WorkFrontId = x.WorkFrontId,
                    SewerGroupId = x.SewerGroupId,
                    IdentifiesSC = x.IdentifiesSC,
                    DescriptionIdentifiesSC = !string.IsNullOrEmpty(x.DescriptionIdentifiesSC) ? x.DescriptionIdentifiesSC : string.Empty,
                    SCQ01 = x.SCQ01,
                    SCQ02 = x.SCQ02,
                    SCQ03 = x.SCQ03,
                    SCQ04 = x.SCQ04,
                    SCQ05 = x.SCQ05,
                    SCQ06 = x.SCQ06,
                    SCQ07 = x.SCQ07,
                    SCQ08 = x.SCQ08,
                    SCQ09 = x.SCQ09,
                    SCQ10 = x.SCQ10,
                    SCQ11 = x.SCQ11,
                    SCQ12 = x.SCQ12,
                    SCQ13 = x.SCQ13,
                    SCQ14 = x.SCQ14,
                    SCQ15 = x.SCQ15,
                    SCQ16 = x.SCQ16,
                    SCQ17 = x.SCQ17,
                    SCQ18 = x.SCQ18,
                    SCQ19 = x.SCQ19,
                    SCQ20 = x.SCQ20,
                    SCQ21 = x.SCQ21,
                    SCQ22 = x.SCQ22,
                    SCQ23 = x.SCQ23,
                    SCQ24 = x.SCQ24,
                    SCQ25 = x.SCQ25,
                    SCQ26 = x.SCQ26,
                    SCQ27 = x.SCQ27,
                    SCQ28 = x.SCQ28,
                    SCQ29 = x.SCQ29,
                    SpecifyConditions = !string.IsNullOrEmpty(x.SpecifyConditions) ? x.SpecifyConditions : string.Empty,
                    IdentifiesSA = x.IdentifiesSA,
                    DescriptionIdentifiesSA = !string.IsNullOrEmpty(x.DescriptionIdentifiesSA) ? x.DescriptionIdentifiesSA : string.Empty,
                    SAQ01 = x.SAQ01,
                    SAQ02 = x.SAQ02,
                    SAQ03 = x.SAQ03,
                    SAQ04 = x.SAQ04,
                    SAQ05 = x.SAQ05,
                    SAQ06 = x.SAQ06,
                    SAQ07 = x.SAQ07,
                    SAQ08 = x.SAQ08,
                    SAQ09 = x.SAQ09,
                    SAQ10 = x.SAQ10,
                    SAQ11 = x.SAQ11,
                    SAQ12 = x.SAQ12,
                    SAQ13 = x.SAQ13,
                    SAQ14 = x.SAQ14,
                    SAQ15 = x.SAQ15,
                    SAQ16 = x.SAQ16,
                    SAQ17 = x.SAQ17,
                    SAQ18 = x.SAQ18,
                    SAQ19 = x.SAQ19,
                    SAQ20 = x.SAQ20,
                    SAQ21 = x.SAQ21,
                    SAQ22 = x.SAQ22,
                    SAQ23 = x.SAQ23,
                    SAQ24 = x.SAQ24,
                    SAQ25 = x.SAQ25,
                    SAQ26 = x.SAQ26,
                    SAQ27 = x.SAQ27,
                    SAQ28 = x.SAQ28,
                    SAQ29 = x.SAQ29,
                    SAQ30 = x.SAQ30,
                    SAQ31 = x.SAQ31,
                    SAQ32 = x.SAQ32,
                    SAQ33 = x.SAQ33,
                    SAQ34 = x.SAQ34,
                    SAQ35 = x.SAQ35,
                    SAQ36 = x.SAQ36,
                    SpecifyActs = !string.IsNullOrEmpty(x.SpecifyActs) ? x.SpecifyActs : string.Empty,
                    ICQ01 = x.ICQ01,
                    ICQ02 = x.ICQ02,
                    ICQ03 = x.ICQ03,
                    ICQ04 = x.ICQ04,
                    ICQ05 = x.ICQ05,
                    SpecifyAppliedCorrections = !string.IsNullOrEmpty(x.SpecifyAppliedCorrections) ? x.SpecifyAppliedCorrections : string.Empty,
                    SpecifyAnotherAlternative = !string.IsNullOrEmpty(x.SpecifyAnotherAlternative) ? x.SpecifyAnotherAlternative : string.Empty,
                    SignatureUrl = x.SignatureUrl,
                    //LocationImageUrl = x.LocationImageUrl,
                    ObservationImageUrl = x.ObservationImageUrl,
                    LiftingObservations = !string.IsNullOrEmpty(x.LiftingObservations) ? x.LiftingObservations : string.Empty,
                    LiftingImageUrl = x.LiftingImageUrl,
                    Status = x.Status
                }).FirstOrDefaultAsync();

            return Ok(query);
        }

        [HttpGet("levantar-observacion/{id}")]
        public async Task<IActionResult> GetRacsToLift(Guid? id)
        {
            if (id == Guid.Empty)
                return BadRequest("No existe el RACS solicitado.");

            var query = await _context.RacsReports
                .Where(x => x.Id == id.Value)
                .Select(x => new RacsToLift
                {
                    Id = x.Id,
                    Code = x.Code,
                    ObservationImageUrl = x.ObservationImageUrl,
                    LiftingObservations = x.LiftingObservations,
                    LiftingImageUrl = x.LiftingImageUrl,
                    Status = x.Status
                }).FirstOrDefaultAsync();

            return Ok(query);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create([FromBody] RacsResourceModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var summary = await _context.RacsSummaries.FirstOrDefaultAsync(x => x.ProjectId == model.ProjectId);
            summary.RacsCount++;

            var racs = new RacsReport
            {
                Code = $"RACS-{summary.RacsCode}-{summary.RacsCount:0000}",
                ProjectId = model.ProjectId,
                ReportDate = model.ReportDate.ToDateTime(),
                ApplicationUserId = model.ApplicationUserId,
                LiftResponsibleId = model.LiftResponsibleId,
                WorkFrontId = model.WorkFrontId,
                SewerGroupId = model.SewerGroupId,
                IdentifiesSC = model.IdentifiesSC,
                DescriptionIdentifiesSC = model.DescriptionIdentifiesSC,
                SCQ01 = model.SCQ01,
                SCQ02 = model.SCQ02,
                SCQ03 = model.SCQ03,
                SCQ04 = model.SCQ04,
                SCQ05 = model.SCQ05,
                SCQ06 = model.SCQ06,
                SCQ07 = model.SCQ07,
                SCQ08 = model.SCQ08,
                SCQ09 = model.SCQ09,
                SCQ10 = model.SCQ10,
                SCQ11 = model.SCQ11,
                SCQ12 = model.SCQ12,
                SCQ13 = model.SCQ13,
                SCQ14 = model.SCQ14,
                SCQ15 = model.SCQ15,
                SCQ16 = model.SCQ16,
                SCQ17 = model.SCQ17,
                SCQ18 = model.SCQ18,
                SCQ19 = model.SCQ19,
                SCQ20 = model.SCQ20,
                SCQ21 = model.SCQ21,
                SCQ22 = model.SCQ22,
                SCQ23 = model.SCQ23,
                SCQ24 = model.SCQ24,
                SCQ25 = model.SCQ25,
                SCQ26 = model.SCQ26,
                SCQ27 = model.SCQ27,
                SCQ28 = model.SCQ28,
                SCQ29 = model.SCQ29,
                SpecifyConditions = model.SpecifyConditions,
                IdentifiesSA = model.IdentifiesSA,
                DescriptionIdentifiesSA = model.DescriptionIdentifiesSA,
                SAQ01 = model.SAQ01,
                SAQ02 = model.SAQ02,
                SAQ03 = model.SAQ03,
                SAQ04 = model.SAQ04,
                SAQ05 = model.SAQ05,
                SAQ06 = model.SAQ06,
                SAQ07 = model.SAQ07,
                SAQ08 = model.SAQ08,
                SAQ09 = model.SAQ09,
                SAQ10 = model.SAQ10,
                SAQ11 = model.SAQ11,
                SAQ12 = model.SAQ12,
                SAQ13 = model.SAQ13,
                SAQ14 = model.SAQ14,
                SAQ15 = model.SAQ15,
                SAQ16 = model.SAQ16,
                SAQ17 = model.SAQ17,
                SAQ18 = model.SAQ18,
                SAQ19 = model.SAQ19,
                SAQ20 = model.SAQ20,
                SAQ21 = model.SAQ21,
                SAQ22 = model.SAQ22,
                SAQ23 = model.SAQ23,
                SAQ24 = model.SAQ24,
                SAQ25 = model.SAQ25,
                SAQ26 = model.SAQ26,
                SAQ27 = model.SAQ27,
                SAQ28 = model.SAQ28,
                SAQ29 = model.SAQ29,
                SAQ30 = model.SAQ30,
                SAQ31 = model.SAQ31,
                SAQ32 = model.SAQ32,
                SAQ33 = model.SAQ33,
                SAQ34 = model.SAQ34,
                SAQ35 = model.SAQ35,
                SAQ36 = model.SAQ36,
                SpecifyActs = model.SpecifyActs,
                ICQ01 = model.ICQ01,
                ICQ02 = model.ICQ02,
                ICQ03 = model.ICQ03,
                ICQ04 = model.ICQ04,
                ICQ05 = model.ICQ05,
                SpecifyAppliedCorrections = model.SpecifyAppliedCorrections,
                SpecifyAnotherAlternative = model.SpecifyAnotherAlternative,
                Status = 0,
                IsMailSended = false,
                IsLiftMailSended = false
            };

            if(model.SignatureArray != null && model.SignatureArray.Length > 0)
            {
                var ms = new MemoryStream(model.SignatureArray);
                var storage = new CloudStorageService(_storageCredentials);
                racs.SignatureUrl = await storage.UploadFile(ms,
                    ConstantHelpers.Storage.Containers.SECURITY,
                    ".png",
                    ConstantHelpers.Storage.Blobs.RACS,
                    $"{racs.Code}-signature");
            }

            if (model.ObservationImageArray != null && model.ObservationImageArray.Length > 0)
            {
                var ms = new MemoryStream(model.ObservationImageArray);
                var storage = new CloudStorageService(_storageCredentials);
                racs.ObservationImageUrl = await storage.UploadFile(ms,
                    ConstantHelpers.Storage.Containers.SECURITY,
                    ".jpeg",
                    ConstantHelpers.Storage.Blobs.RACS,
                    $"{racs.Code}-observation");
            }

            await _context.RacsReports.AddAsync(racs);
            await _context.SaveChangesAsync();
            
            if (racs != null)
            {
                var issuer = await _context.Users.FirstOrDefaultAsync(x => x.Id == racs.ApplicationUserId);
                var lifter = await _context.Users.FirstOrDefaultAsync(x => x.Id == racs.LiftResponsibleId);
                var project = await _context.Projects.FindAsync(model.ProjectId);
                racs.Project = project;
                await SendMailAlert(racs, issuer, lifter);
            }

            //_queuedBackgroundService.QueueBackgroundRacsItem(racs);

            return Ok();
        }

        [HttpPut("levantar-observacion")]
        public async Task<IActionResult> LiftRacs([FromBody] RacsToLift model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var racs = await _context.RacsReports
                .Include(x => x.Project)
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            racs.LiftingObservations = model.LiftingObservations;
            racs.Status = ConstantHelpers.RacsReports.LIFTED;
            if (model.LiftingImageArray != null && model.LiftingImageArray.Length > 0)
            {
                var ms = new MemoryStream(model.LiftingImageArray);
                var storage = new CloudStorageService(_storageCredentials);
                racs.LiftingImageUrl = await storage.UploadFile(ms,
                    ConstantHelpers.Storage.Containers.SECURITY,
                    ".jpeg",
                    ConstantHelpers.Storage.Blobs.RACS,
                    $"{racs.Code}-lift");
            }

            await _context.SaveChangesAsync();

            if (racs != null)
            {
                var issuer = await _context.Users.FirstOrDefaultAsync(x => x.Id == racs.ApplicationUserId);
                var lifter = await _context.Users.FirstOrDefaultAsync(x => x.Id == racs.LiftResponsibleId);
                await SendMailLiftAlert(racs, issuer, lifter);
            }

            //_queuedBackgroundService.QueueBackgroundRacsItem(racs);

            return Ok();
        }

        //Metodo creado con fines de prueba
        //[AllowAnonymous]
        //[HttpGet("test-mail/{id}")]        
        //public async Task<IActionResult> GetTestMail(Guid id)
        //{
        //    var racs = await _context.RacsReports
        //        .AsNoTracking()
        //        .FirstOrDefaultAsync(x => x.Id == id);

        //    var issuer = await _context.Users.FirstOrDefaultAsync(x => x.Id == racs.ApplicationUserId);
        //    var lifter = await _context.Users.FirstOrDefaultAsync(x => x.Id == racs.LiftResponsibleId);
        //    #pragma warning disable CS4014 // Dado que no se esperaba esta llamada, la ejecución del método actual continuará antes de que se complete la llamada
        //    Task.Run(
        //        () => SendMailAlert(racs, issuer, lifter)
        //    );
        //    #pragma warning restore CS4014 // Dado que no se esperaba esta llamada, la ejecución del método actual continuará antes de que se complete la llamada
        //    return Ok();
        //}

        [AllowAnonymous]
        [HttpGet("probar")]
        public async Task<IActionResult> Test()
        {
            try
            {
                var rac = await _context.RacsReports.Include(x => x.Project).FirstOrDefaultAsync(x => x.Id == new Guid("F40C11F0-2142-4C2A-7B93-08D9ED7616CC"));
                var issuer = new ApplicationUser { Email = "jeffreyrm96@gmail.com", Name = "Luis", MiddleName = "Jeffrey", PaternalSurname = "Rojas", MaternalSurname = "Montes" };
                await SendMailLiftAlert(rac, issuer, issuer);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }


        private async Task SendMailAlert(RacsReport racs, ApplicationUser issuer, ApplicationUser lifter)
        {
            var serviceUrl = new Uri($"{ConstantHelpers.SystemUrl.PdfGeneratorUrl}?url={ConstantHelpers.SystemUrl.Url}seguridad/racs/generar-pdf/{racs?.Id}");
            using var wc = new WebClient();
            var bondBytes = await wc.DownloadDataTaskAsync(serviceUrl);
            
            _emailQueuedBackgroundService.QueueBackgroundEmailItem(new EmailItem
            {
                To = new List<MailAddress> { new MailAddress(lifter?.Email, lifter?.RawFullName) },
                Cc = new List<MailAddress>() { new MailAddress(issuer?.Email, issuer?.RawFullName) },
                Subject = $"IVC - Registro de RACS - {racs?.Code}",
                Message = $"Hola, { lifter?.FullName }, <br /><br /> " +
                $"Se le informa que en el centro de costo <strong>{ racs?.Project?.Abbreviation }</strong> el <strong>{racs?.Code}</strong> ha sido <strong>creado</strong>.<br />" +
                $"Le adjuntamos el RACS para su conocimiento. <br /><br />" +
                $"Saludos <br />" +
                $"<strong>Sistema IVC <br/>" +
                $"Control de SST</strong>",
                Attachments = new List<Attachment> { new Attachment(new MemoryStream(bondBytes), $"{racs?.Code}.pdf") }
            });
        }

        //private async void SendMailAlert(RacsReport racs, ApplicationUser issuer, ApplicationUser lifter)
        //{
        //    //Cargar Archivo Adjunto
        //    MemoryStream ms = new MemoryStream();
        //    Attachment attachment = null;
        //    Uri serviceUrl = new Uri($"https://erp-ivc-pdf.azurewebsites.net/api/functionapp?url=https://erp-ivc.azurewebsites.net/seguridad/racs/generar-pdf/{racs.Id}");
        //    using (var wc = new WebClient())
        //    {
        //        byte[] bondBytes = wc.DownloadData(serviceUrl);
        //        ms = new MemoryStream(bondBytes);
        //    }
        //    attachment = new Attachment(ms, $"{racs.Code}.pdf");

        //    var mailMessage = new MailMessage
        //    {
        //        From = new MailAddress("sistemaerp@ivc.pe", "Sistema IVC")
        //    };

        //    //var mailMessage = new MailMessage
        //    //{
        //    //    From = new MailAddress("sistemaivctest@gmail.com", "Sistema IVC")
        //    //};

        //    if (attachment != null)
        //        mailMessage.Attachments.Add(attachment);

        //    mailMessage.Subject = $"IVC - Registro de RACS - {racs.Code}";

        //    if (issuer != null && lifter != null)
        //    {
        //        //mailMessage.To.Add(new MailAddress("rd.lazaroc@gmail.com", "Ricardo Lazaro"));
        //        mailMessage.To.Add(new MailAddress(lifter.Email, lifter.RawFullName));
        //        mailMessage.CC.Add(new MailAddress(issuer.Email, issuer.RawFullName));
        //        mailMessage.Body =
        //            $"Hola, { lifter.FullName }, <br /><br /> " +
        //            $"Se le informa que el {racs.Code} ha sido levantado.<br />" +
        //            $"Le adjuntamos el RACS para su conocimiento. <br /><br />" +
        //            $"Saludos <br />" +
        //            $"Sistema IVC";
        //        mailMessage.IsBodyHtml = true;

        //        ////Mandar Correo
        //        //using (var client = new SmtpClient("smtp.gmail.com", 587))
        //        //{
        //        //    client.UseDefaultCredentials = false;
        //        //    client.Credentials = new NetworkCredential("sistemaivctest@gmail.com", "IVC.12345");
        //        //    client.EnableSsl = true;
        //        //    await client.SendMailAsync(mailMessage);
        //        //}

        //        //Mandar Correo
        //        using (var client = new SmtpClient("smtp.office365.com", 587))
        //        {
        //            client.UseDefaultCredentials = false;
        //            client.Credentials = new NetworkCredential("sistemaerp@ivc.pe", "S1st3m4erp");
        //            client.EnableSsl = true;
        //            await client.SendMailAsync(mailMessage);
        //        }
        //    }
        //}

        private async Task SendMailLiftAlert(RacsReport racs, ApplicationUser issuer, ApplicationUser lifter)
        {
            var serviceUrl = new Uri($"{ConstantHelpers.SystemUrl.PdfGeneratorUrl}?url={ConstantHelpers.SystemUrl.Url}seguridad/racs/generar-pdf/{racs?.Id}");
            using var wc = new WebClient();
            var bondBytes = await wc.DownloadDataTaskAsync(serviceUrl);

            _emailQueuedBackgroundService.QueueBackgroundEmailItem(new EmailItem
            {
                To = new List<MailAddress> { new MailAddress(lifter?.Email, lifter?.RawFullName) },
                Cc = new List<MailAddress>() { new MailAddress(issuer?.Email, issuer?.RawFullName) },
                Subject = $"IVC - Registro de RACS - {racs?.Code}",
                Message = $"Hola, { lifter?.FullName }, <br /><br /> " +
                $"Se le informa que en el centro de costo <strong>{ racs?.Project?.Abbreviation }</strong> el <strong>{racs?.Code}</strong> ha sido <strong>levantado</strong>.<br />" +
                $"Le adjuntamos el RACS para su conocimiento. <br /><br />" +
                $"Saludos <br />" +
                $"<strong>Sistema IVC <br/>" +
                $"Control de SST</strong>",
                Attachments = new List<Attachment> { new Attachment(new MemoryStream(bondBytes), $"{racs?.Code}.pdf") }
            });
        }

        //private async void SendMailLiftAlert(RacsReport racs, ApplicationUser issuer, ApplicationUser lifter)
        //{
        //    //Cargar Archivo Adjunto
        //    MemoryStream ms = new MemoryStream();
        //    Attachment attachment = null;
        //    Uri serviceUrl = new Uri($"https://erp-ivc-pdf.azurewebsites.net/api/functionapp?url=https://erp-ivc.azurewebsites.net/seguridad/racs/generar-pdf/{racs.Id}");
        //    using (var wc = new WebClient())
        //    {
        //        byte[] bondBytes = wc.DownloadData(serviceUrl);
        //        ms = new MemoryStream(bondBytes);
        //    }
        //    attachment = new Attachment(ms, $"{racs.Code}.pdf");

        //    var mailMessage = new MailMessage
        //    {
        //        From = new MailAddress("sistemaerp@ivc.pe", "Sistema IVC")
        //    };

        //    if (attachment != null)
        //        mailMessage.Attachments.Add(attachment);

        //    mailMessage.Subject = $"IVC - Registro de RACS - {racs.Code}";

        //    if (issuer != null && lifter != null)
        //    {
        //        mailMessage.To.Add(new MailAddress(lifter.Email, lifter.RawFullName));
        //        mailMessage.CC.Add(new MailAddress(issuer.Email, issuer.RawFullName));
        //        mailMessage.Body =
        //            $"Hola, { lifter.FullName }, <br /><br /> " +
        //            $"Se le informa que el {racs.Code} ha sido levantado.<br />" +
        //            $"Le adjuntamos el RACS para su conocimiento. <br /><br />" +
        //            $"Saludos <br />" +
        //            $"Sistema IVC";
        //        mailMessage.IsBodyHtml = true;

        //        //Mandar Correo
        //        using (var client = new SmtpClient("smtp.office365.com", 587))
        //        {
        //            client.UseDefaultCredentials = false;
        //            client.Credentials = new NetworkCredential("sistemaerp@ivc.pe", "S1st3m4erp");
        //            client.EnableSsl = true;
        //            await client.SendMailAsync(mailMessage);
        //        }
        //    }
        //}
    }
}
