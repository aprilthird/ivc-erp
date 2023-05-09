using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.UspModels.HumanResources;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.HumanResources.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.HumanResources.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.HUMAN_RESOURCES)]
    [Route("recursos-humanos/correos")]
    public class EmailConfirmationController : BaseController
    {
        private readonly IEmailQueuedBackground _emailQueuedBackgroundService;

        public EmailConfirmationController(IvcDbContext context, 
            ILogger<EmailConfirmationController> logger, 
            IEmailQueuedBackground emailQueuedBackgroundService)
            : base(context, logger)
        {
            _emailQueuedBackgroundService = emailQueuedBackgroundService;
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(string status = null, int? category = null, int? origin = null, int? workgroup = null)
        {
            var projectParam = new SqlParameter("@ProjectId", GetProjectId());

            var query = await _context.Set<UspWorker>().FromSqlRaw("execute HumanResources_uspWorkers @ProjectId"
                , projectParam)
                .IgnoreQueryFilters()
                .ToListAsync();

            if (status != "todos")
                if (status == "activo")
                    query = query.Where(x => x.IsActive).ToList();
                else if (status == "cesado")
                    query = query.Where(x => !x.IsActive).ToList();

            if (category.HasValue)
                query = query.Where(x => x.Category == category.Value).ToList();
            if (origin.HasValue)
                query = query.Where(x => x.Origin == origin.Value).ToList();
            if (workgroup.HasValue)
                query = query.Where(x => x.Workgroup == workgroup.Value).ToList();

            return Ok(query);   
        }

        [HttpPost("enviar-alerta")]
        public async Task<IActionResult> SendEmailConfirmationAlert(Guid workerId)
        {
            var worker = await _context.Workers.FindAsync(workerId);

            if (worker == null)
                return BadRequest($"Trabajador de Id '{workerId}' no encontrado.");

            var linkConfirm = $"{ConstantHelpers.SystemUrl.Url}recursos-humanos/correos/confirmar?key={worker.Id}";

            _emailQueuedBackgroundService.QueueBackgroundEmailItem(new EmailItem
            {
                To = new List<MailAddress> { new MailAddress(worker.Email, worker.RawFullName) },
                Cc = new List<MailAddress>(),
                Subject = $"IVC - Confirmación de Correo",
                Message = $"Hola <strong>{worker.RawFullName}</strong>,<br/><br/>Se le envía este mensaje para solicitarle que confirme su dirección de correo electrónico a través del siguiente enlace: <a href='{linkConfirm}'>CLICK PARA CONFIRMAR</a>.<br/><br/>Saludos cordiales.",
            });

            worker.EmailAlertSentDateTime = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("envio-masivo")]
        public async Task<IActionResult> SendEmailConfirmationAlertToAll()
        {
            var workers = await _context.Workers.Where(x => !x.EmailConfirmed).ToListAsync();
            foreach(var w in workers)
            {
                if (w.Email == null) continue;
                w.EmailAlertSentDateTime = DateTime.UtcNow;
                var linkConfirm = $"{ConstantHelpers.SystemUrl.Url}recursos-humanos/correos/confirmar?key={w.Id}";
                _emailQueuedBackgroundService.QueueBackgroundEmailItem(new EmailItem
                {
                    To = new List<MailAddress> { new MailAddress(w.Email, w.RawFullName) },
                    Cc = new List<MailAddress>(),
                    Subject = $"IVC - Confirmación de Correo",
                    Message = $"Hola <strong>{w.RawFullName}</strong>,<br/><br/>Se le envía este mensaje para solicitarle que confirme su dirección de correo electrónico a través del siguiente enlace: <a href='{linkConfirm}'>CLICK PARA CONFIRMAR</a>.<br/><br/>Saludos cordiales.",
                });
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                var s = ex.StackTrace;
            }
            return Ok();
        }

        [HttpGet("confirmar")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(Guid key)
        {
            var worker = await _context.Workers.FindAsync(key);

            if (worker == null)
                return RedirectToAction("Error", "Message", new { message = "Petición de confirmación no válida." });

            if (worker.EmailConfirmed)
                return RedirectToAction("Error", "Message", new { message = "Su Email ya se encuentra confirmado." });

            worker.EmailConfirmed = true;
            worker.EmailConfirmationDateTime = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            _emailQueuedBackgroundService.QueueBackgroundEmailItem(new EmailItem
            {
                To = new List<MailAddress> { new MailAddress(worker.Email, worker.RawFullName) },
                Cc = new List<MailAddress>(),
                Subject = $"IVC - Correo Confirmado",
                Message = $"Hola <strong>{worker.RawFullName}</strong>,<br/><br/>Se le envía este mensaje para notificarle que su dirección de correo acaba de ser confirmado.<br/><br/>Saludos cordiales.",
            });

            return RedirectToAction("Index", "Message", new
            {
                title = $"Bienvenido, {worker.RawFullName}",
                message = $"Su correo {worker.Email} ha sido confirmado",
                icon = Url.Content("~/media/email_approved.png")
            });
        }
    }
}
