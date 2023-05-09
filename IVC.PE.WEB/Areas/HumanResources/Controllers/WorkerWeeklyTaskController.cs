using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using EFCore.BulkExtensions;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.HumanResources;
using IVC.PE.ENTITIES.UspModels.HumanResources;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectPhaseViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.SewerGroupViewModels;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.PayrollAuthorizationRequestViewModels;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.WorkerDailyTaskViewModels;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.WorkerPayrollViewModels;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.WorkerViewModels;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.WorkerWeeklyTaskViewModels;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Options;
using IVC.PE.WEB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IVC.PE.WEB.Areas.HumanResources.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.HumanResources.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.HUMAN_RESOURCES)]
    [Route("recursos-humanos/tareo-semanal")]
    public class WorkerWeeklyTaskController : BaseController
    {
        private readonly IPayrollQueuedBackground _queuedBackgroundService;
        private readonly IEmailQueuedBackground _emailQueuedBackgroundService;
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public WorkerWeeklyTaskController(IvcDbContext context,
            UserManager<ApplicationUser> userManager,
            ILogger<WorkerWeeklyTaskController> logger,
            IPayrollQueuedBackground queuedBackgroundService,
            IEmailQueuedBackground emailQueuedBackgroundService,
            IOptions<CloudStorageCredentials> storageCredentials)
            : base(context, userManager, logger)
        {
            _queuedBackgroundService = queuedBackgroundService;
            _emailQueuedBackgroundService = emailQueuedBackgroundService;
            _storageCredentials = storageCredentials;
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? weekId = null, Guid? sewerGroupId = null, Guid? workFrontHeadId = null)
        {
            if (weekId == null)
                return Ok(new List<WorkerWeeklyTaskViewModel>());

            var week = await _context.ProjectCalendarWeeks
                .Include(x => x.ProjectCalendar)
                .FirstOrDefaultAsync(x => x.Id == weekId.Value);

            var workerDailyTasks = _context.WorkerDailyTasks
                .Where(x => x.Date.Date >= week.WeekStart.Date && x.Date.Date <= week.WeekEnd.Date &&
                                x.ProjectId == week.ProjectCalendar.ProjectId)
                .AsQueryable();

            if (workerDailyTasks.Count() == 0)
                return Ok(new List<WorkerWeeklyTaskViewModel>());

            if(workFrontHeadId != null)
                workerDailyTasks = workerDailyTasks.Include(x => x.SewerGroup).Where(x => x.SewerGroup.WorkFrontHeadId == workFrontHeadId).AsQueryable();

            if (sewerGroupId != null)
                workerDailyTasks = workerDailyTasks.Where(x => x.SewerGroupId == sewerGroupId).AsQueryable();

            var workerDailyTasksList = workerDailyTasks.OrderBy(x => x.SewerGroupId).ToList();

            var workers = await _context.Workers.ToListAsync();

            var phases = await _context.ProjectPhases.ToListAsync();

            var sewerGroups = await _context.SewerGroups.ToListAsync();

            var taskToDataTable = new List<WorkerWeeklyTaskViewModel>();

            var workersTasked = workerDailyTasks.Select(x => x.WorkerId).Distinct();

            foreach (var worker in workersTasked)
            {
                var dailyTasks = workerDailyTasksList.Where(x => x.WorkerId == worker).OrderBy(x => x.Date).ToList();

                var weeklyTask = new WorkerWeeklyTaskViewModel();

                var workerDb = workers.FirstOrDefault(x => x.Id == worker);
                weeklyTask.WorkerId = workerDb.Id;
                weeklyTask.WorkerFullName = workerDb.FullName;
                weeklyTask.WorkerTypeDocNumber = ConstantHelpers.DocumentType.VALUES[workerDb.DocumentType] + " " + workerDb.Document;

                weeklyTask.WorkerDailyTasks = new WorkerWeeklyDayTaskViewModel[7];
                for (int i = 0; i < 7; i++)
                {
                    weeklyTask.WorkerDailyTasks[i] = new WorkerWeeklyDayTaskViewModel
                    {
                        Hours = string.Empty,
                        Hours60 = string.Empty,
                        Hours100 = string.Empty,
                        Phase = string.Empty
                    };
                }
                var sewerGroupCount = new Dictionary<string, int?>();
                foreach (var task in dailyTasks)
                {
                    var day = (int)task.Date.DayOfWeek;
                    var phase = phases.FirstOrDefault(x => x.Id == task.ProjectPhaseId);
                    var sewerGroup = sewerGroups.FirstOrDefault(x => x.Id == task.SewerGroupId);
                    sewerGroupCount[sewerGroup.Code] = sewerGroupCount.Keys.Contains(sewerGroup.Code) ? sewerGroupCount[sewerGroup.Code].Value + 1 : 1;

                    weeklyTask.WorkerDailyTasks[day] = new WorkerWeeklyDayTaskViewModel
                    {
                        Hours = task.HoursNormal > 0 ? task.HoursNormal.ToString() :
                                        task.HoursHoliday > 0 ? "FE" :
                                        task.HoursMedicalRest > 0 ? "DM" :
                                        task.HoursPaidLeave > 0 ? "PG" :
                                        task.HoursPaternityLeave > 0 ? "PG" :
                                        task.LaborSuspension ? "S" :
                                        task.MedicalLeave ? "SM" :
                                        task.NonAttendance ? "F" :
                                        task.UnPaidLeave ? "PS" : "0.0",
                        Hours60 = task.Hours60Percent > 0 ? task.Hours60Percent.ToString() : string.Empty,
                        Hours100 = task.Hours100Percent > 0 ? task.Hours100Percent.ToString() : string.Empty,
                        Phase = phase?.Code
                    };
                }
                weeklyTask.SewerGroupCode = sewerGroupCount.Count > 1 ? GetSewerGroupIdWithMostAttendance(sewerGroupCount) : sewerGroupCount.Keys.First();
                taskToDataTable.Add(weeklyTask);
            }

            return Ok(taskToDataTable.OrderBy(x => x.SewerGroupCode).ToList());
        }

        [HttpGet("obrero/listar")]
        public async Task<IActionResult> GetAllWorkerTasks(Guid? weekId = null, Guid? workerId = null)
        {
            if (weekId == null || workerId == null)
                return Ok(new List<WorkerDailyTaskViewModel>());

            var week = await _context.ProjectCalendarWeeks
                .Include(x => x.ProjectCalendar)
                .FirstOrDefaultAsync(x => x.Id == weekId.Value);

            var workerDailyTasks = _context.WorkerDailyTasks
                .Where(x => x.Date.Date >= week.WeekStart.Date && x.Date.Date <= week.WeekEnd.Date && x.WorkerId == workerId.Value &&
                            x.ProjectId == week.ProjectCalendar.ProjectId)
                .OrderBy(x => x.Date)
                .AsQueryable();

            var query = await workerDailyTasks
                .Include(x => x.Worker)
                .Include(x => x.ProjectPhase)
                .Include(x => x.SewerGroup)
                .Select(x => new WorkerDailyTaskViewModel
                {
                    Id = x.Id,
                    WorkerId = x.WorkerId,
                    ProjectId = x.ProjectId,
                    ProjectPhaseId = x.ProjectPhaseId,
                    ProjectPhase = x.ProjectPhaseId.HasValue ? new ProjectPhaseViewModel
                    {
                        Code = x.ProjectPhase.Code,
                        Description = x.ProjectPhase.Description
                    } : null,
                    Date = x.Date.ToDateString(),
                    HoursNormal = x.HoursNormal,
                    Hours60Percent = x.Hours60Percent,
                    Hours100Percent = x.Hours100Percent,
                    HoursHoliday = x.HoursHoliday,
                    MedicalLeave = x.MedicalLeave,
                    HoursMedicalRest = x.HoursMedicalRest,
                    HoursPaternityLeave = x.HoursPaternityLeave,
                    HoursPaidLeave = x.HoursPaidLeave,
                    LaborSuspension = x.LaborSuspension,
                    NonAttendance = x.NonAttendance,
                    SewerGroupId = x.SewerGroupId,
                    SewerGroup = new SewerGroupViewModel
                    {
                        Code = x.SewerGroup.Code
                    },
                    UnPaidLeave = x.UnPaidLeave,
                    Worker = new WorkerViewModel
                    {
                        Name = x.Worker.Name,
                        MiddleName = x.Worker.MiddleName,
                        PaternalSurname = x.Worker.PaternalSurname,
                        MaternalSurname = x.Worker.MaternalSurname,
                        DocumentType = x.Worker.DocumentType,
                        Document = x.Worker.Document
                    }
                })
                .ToListAsync();

            return Ok(query);
        }

        #region Authorization Request
        [HttpGet("autorizacion")]
        public async Task<IActionResult> GetProjectPayrollAuth(Guid pid, Guid wid)
        {
            var authReq = await _context.PayrollAuthorizationRequests
                .Where(x => x.WeekId == wid)
                .Select(x => new PayrollAuthorizationRequestViewModel
                {
                    TaskUserAuth1Id = x.TaskUserAuth1Id,
                    WeeklyTaskAuth1 = x.WeeklyTaskAuth1,
                    UserAnswered1 = x.UserAnswered1,
                    TaskUserAuth2Id = x.TaskUserAuth2Id,
                    WeeklyTaskAuth2 = x.WeeklyTaskAuth2,
                    UserAnswered2 = x.UserAnswered2,
                    AlertsSent = true
                }).FirstOrDefaultAsync();

            var users = await _context.Users.ToListAsync();

            if (authReq == null)
            {
                var authDb = await _context.ProjectPayrollResponsibles
                .Where(x => x.ProjectId == pid)
                .Select(x => new PayrollAuthorizationRequestViewModel
                {
                    TaskUserAuth1Id = x.Responsible1Id,
                    WeeklyTaskAuth1 = false,
                    TaskUserAuth2Id = x.Responsible2Id,
                    WeeklyTaskAuth2 = false,
                    AlertsSent = false
                }).FirstOrDefaultAsync();

                if (authDb == null)
                    return Ok(null);

                authDb.Responsible1FullName = users.FirstOrDefault(x => x.Id == authDb.TaskUserAuth1Id).FullName;
                authDb.Responsible2FullName = users.FirstOrDefault(x => x.Id == authDb.TaskUserAuth2Id).FullName;

                return Ok(authDb);
            }

            authReq.Responsible1FullName = users.FirstOrDefault(x => x.Id == authReq.TaskUserAuth1Id).FullName;
            authReq.Responsible2FullName = users.FirstOrDefault(x => x.Id == authReq.TaskUserAuth2Id).FullName;

            return Ok(authReq);
        }

        [HttpPost("solicitar-autorizacion")]
        public async Task<IActionResult> CreateAuthorizationRequest(Guid weekId, Guid projectId)
        {
            var authReq = await _context.PayrollAuthorizationRequests
                .FirstOrDefaultAsync(x => x.WeekId == weekId);

            var week = await _context.ProjectCalendarWeeks
                .Include(x => x.ProjectCalendar.Project)
                .FirstOrDefaultAsync(x => x.Id == weekId);

            if (week == null)
                return BadRequest("No se encuentra la semana seleccionada.");

            var responsibles = await _context.ProjectPayrollResponsibles
                .FirstOrDefaultAsync(x => x.ProjectId == projectId);

            if (responsibles == null)
                return BadRequest("No existen responsables registrados.");

            var responsible1 = await _context.Users.FindAsync(responsibles.Responsible1Id);
            var responsible2 = await _context.Users.FindAsync(responsibles.Responsible2Id);

            if (responsible1 == null || responsible2 == null)
                return BadRequest("Uno o ambos responsables registrados no existen.");

            var linkExcel = ConstantHelpers.SystemUrl.Url + "recursos-humanos/tareo-semanal/exportar-excel?weekId=" + week.Id.ToString();
            var excelResult = await ToUspExcel(weekId);
            var excelAttachment = new Attachment(new MemoryStream(excelResult.Data), excelResult.FileName);
            var linkAuth = $"{ConstantHelpers.SystemUrl.Url}recursos-humanos/tareo-semanal/evaluar/{weekId}";

            if (authReq == null)
            {
                await _context.PayrollAuthorizationRequests.AddAsync(
                    new PayrollAuthorizationRequest
                    {
                        Title = week.ProjectCalendar.Project.Abbreviation + "- Tareo Semanal",
                        Text = "Se solicita aprobación del tareo semanal de la semana #" + week.WeekNumber + " del año " + week.Year + ".",
                        WeekId = weekId,
                        TaskUserAuth1Id = responsibles.Responsible1Id,
                        WeeklyTaskAuth1 = false,
                        TaskUserAuth2Id = responsibles.Responsible2Id,
                        WeeklyTaskAuth2 = false,
                        PayrollUserAuthId = responsibles.Responsible3Id,
                        WeeklyPayrollAuth = false,
                        IsPayrollOk = false
                    });
                await _context.SaveChangesAsync();

                var linkResponsible1 = $"{linkAuth}?userId={responsible1.Id}";
                var linkApprove1 = $"{linkResponsible1}&isApproved=true";
                var linkReject1 = $"{linkResponsible1}&isApproved=false";

                //Actualizar metodo de envio de correos
                _emailQueuedBackgroundService.QueueBackgroundEmailItem(new EmailItem
                {
                    To = new List<MailAddress> { new MailAddress(responsible1.Email, responsible1.RawFullName) },
                    Cc = new List<MailAddress>(),
                    Subject = $"IVC - Solicitud de Aprobación Tareo Semanal - {week.ProjectCalendar.Project.Abbreviation} - Semana {week.WeekNumber}-{week.Year}",
                    Message = $"Hola <strong>{responsible1.RawFullName}</strong>,<br/><br/>Se le solicita revisar y aprobar el tareo de la Semana {week.WeekNumber}-{week.Year} para el proyecto {week.ProjectCalendar.Project.Abbreviation}.<br/>El excel del tareo semanal y demás archivos aparecen adjuntos al correo.<br/><br/><p style='color:green'>Si todo es conforme haga click en el siguiente enlace: <a href='{linkApprove1}'>APROBAR</a>.</p><p style='color:red'>De encontrar alguna disconformidad haga click en el siguiente enlace: <a href='{linkReject1}'>DESAPROBAR</a>.</p><br/>Saludos cordiales.",
                    Attachments = new List<Attachment> { excelAttachment }
                });
                var linkResponsible2 = $"{linkAuth}?userId={responsible2.Id}";
                var linkApprove2 = $"{linkResponsible2}&isApproved=true";
                var linkReject2 = $"{linkResponsible2}&isApproved=false";
                _emailQueuedBackgroundService.QueueBackgroundEmailItem(new EmailItem
                {
                    To = new List<MailAddress> { new MailAddress(responsible2.Email, responsible2.RawFullName) },
                    Cc = new List<MailAddress>(),
                    Subject = $"IVC - Solicitud de Aprobación Tareo Semanal - {week.ProjectCalendar.Project.Abbreviation} - Semana {week.WeekNumber}-{week.Year}",
                    Message = $"Hola <strong>{responsible2.RawFullName}</strong>,<br/><br/>Se le solicita revisar y aprobar el tareo de la Semana {week.WeekNumber}-{week.Year} para el proyecto {week.ProjectCalendar.Project.Abbreviation}.<br/>El excel del tareo semanal y demás archivos aparecen adjuntos al correo.<br/><br/><p style='color:green'>Si todo es conforme haga click en el siguiente enlace: <a href='{linkApprove2}'>APROBAR</a>.</p><p style='color:red'>De encontrar alguna disconformidad haga click en el siguiente enlace: <a href='{linkReject2}'>DESAPROBAR</a>.</p><br/>Saludos cordiales.",
                    Attachments = new List<Attachment> { excelAttachment }
                });

                return Ok("Solicitud registrada. Alertas enviadas.");
            } else
            {
                //Actualizar metodo de envio de correos
                if (!authReq.WeeklyTaskAuth1)
                {
                    var linkResponsible1 = $"{linkAuth}?userId={responsible1.Id}";
                    var linkApprove1 = $"{linkResponsible1}&isApproved=true";
                    var linkReject1 = $"{linkResponsible1}&isApproved=false";
                    _emailQueuedBackgroundService.QueueBackgroundEmailItem(new EmailItem
                    {
                        To = new List<MailAddress> { new MailAddress(responsible1.Email, responsible1.RawFullName) },
                        Cc = new List<MailAddress>(),
                        Subject = $"IVC - Recordatorio de Aprobación Tareo Semanal - {week.ProjectCalendar.Project} - Semana {week.WeekNumber}-{week.Year}",
                        Message = $"Hola <strong>{responsible1.RawFullName}</strong>,<br/><br/>Se le solicita revisar y aprobar el tareo de la Semana {week.WeekNumber}-{week.Year} para el proyecto {week.ProjectCalendar.Project.Abbreviation}.<br/>El excel del tareo semanal y demás archivos aparecen adjuntos al correo.<br/><br/><p style='color:green'>Si todo es conforme haga click en el siguiente enlace: <a href='{linkApprove1}'>APROBAR</a>.</p><p style='color:red'>De encontrar alguna disconformidad haga click en el siguiente enlace: <a href='{linkReject1}'>DESAPROBAR</a>.</p><br/>Saludos cordiales.",
                        Attachments = new List<Attachment> { excelAttachment }
                    });
                }

                if (!authReq.WeeklyTaskAuth2)
                {
                    var linkResponsible2 = $"{linkAuth}?userId={responsible2.Id}";
                    var linkApprove2 = $"{linkResponsible2}&isApproved=true";
                    var linkReject2 = $"{linkResponsible2}&isApproved=false";
                    _emailQueuedBackgroundService.QueueBackgroundEmailItem(new EmailItem
                    {
                        To = new List<MailAddress> { new MailAddress(responsible2.Email, responsible2.RawFullName) },
                        Cc = new List<MailAddress>(),
                        Subject = $"IVC - Recordatorio de Aprobación Tareo Semanal - {week.ProjectCalendar.Project} - Semana {week.WeekNumber}-{week.Year}",
                        Message = $"Hola <strong>{responsible2.RawFullName}</strong>,<br/><br/>Se le solicita revisar y aprobar el tareo de la Semana {week.WeekNumber}-{week.Year} para el proyecto {week.ProjectCalendar.Project.Abbreviation}.<br/>El excel del tareo semanal y demás archivos aparecen adjuntos al correo.<br/><br/><p style='color:green'>Si todo es conforme haga click en el siguiente enlace: <a href='{linkApprove2}'>APROBAR</a>.</p><p style='color:red'>De encontrar alguna disconformidad haga click en el siguiente enlace: <a href='{linkReject2}'>DESAPROBAR</a>.</p><br/>Saludos cordiales.",
                        Attachments = new List<Attachment> { excelAttachment }
                    });
                }

                return Ok("Ya existe una solicitud registrada. Se enviaron recordatorios a los responsables.");
            }
        }

        [HttpGet("evaluar/{weekId}")]
        [AllowAnonymous]
        public async Task<IActionResult> AuthWeeklyTask(Guid weekId, string userId, bool isApproved)
        {
            var authDb = await _context.PayrollAuthorizationRequests.FirstOrDefaultAsync(x => x.WeekId == weekId);

            if (authDb == null)
                return RedirectToAction("Error", "Message", new { message = "No existe petición." });

            var currentWeekHeader = await _context.PayrollMovementHeaders
                .Include(x => x.ProjectCalendarWeek)
                .FirstOrDefaultAsync(x => x.ProjectCalendarWeekId == weekId);
            
            if(currentWeekHeader != null)
                return RedirectToAction("Error", "Message", new { message = "No se puede modificar el estado de la autorización del tareo cuando la planilla ha sido calculada." });

            //if (currentWeekHeader.ProcessStatus == ConstantHelpers.Payroll.ProcessStatus.PROCESSED)

            var week = await _context.ProjectCalendarWeeks
                    .Include(x => x.ProjectCalendar)
                    .Include(x => x.ProjectCalendar.Project)
                    .FirstOrDefaultAsync(x => x.Id == weekId);
            
            if (authDb.TaskUserAuth1Id == userId)
            {
                authDb.WeeklyTaskAuth1 = isApproved;
                authDb.UserAnswered1 = true;
            }
            else if (authDb.TaskUserAuth2Id == userId)
            {
                authDb.WeeklyTaskAuth2 = isApproved;
                authDb.UserAnswered2 = true;
            }
            else if (authDb.PayrollUserAuthId == userId)
            {
                authDb.WeeklyPayrollAuth = isApproved;
            }

            //Metodo que crea la cabecera de la planilla
            if (authDb.WeeklyTaskAuth1 && authDb.WeeklyTaskAuth2 && !authDb.WeeklyPayrollAuth)
            {
                authDb.Title = week.ProjectCalendar.Project.Abbreviation + "-Planilla Semanal";
                authDb.Text = "Se solicita aprobación del tareo semanal de la semana #" + week.WeekNumber + ".";
            }

            if (authDb.WeeklyPayrollAuth)
            {
                week.IsClosed = true;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Message", new
            {
                title = $"Tareo Semanal {(isApproved ? "Aprobado" : "Rechazado")}",
                message = $"El Tareo Semanal correspondiente a la semana {week.WeekNumber}-{week.Year} para el proyecto {week.ProjectCalendar.Project.Abbreviation} ha sido {(isApproved ? "aprobado" : "rechazado")}.",
                icon = isApproved ? Url.Content("~/media/file_approved.png") : Url.Content("~/media/file_rejected.png")
            });
        }

        [HttpGet("mensaje")]
        [AllowAnonymous]
        public async Task<IActionResult> TestMessage()
        {
            return RedirectToAction("Index", "Message", new { title = "Tareo Semanal Aprobado", 
                message = "El Tareo Semanal correspondiente a la semana #5-2022 para el proyecto Consorcio Huarmey ha sido aprobado", icon = Url.Content("~/media/file_approved.png") });
        }

        [HttpPut("actualizar-autorizacion/{id}")]   
        public async Task<IActionResult> UpdatePayrollAuthRequest(Guid id)
        {
            var authDb = await _context.PayrollAuthorizationRequests.FirstOrDefaultAsync(x => x.WeekId == id);

            if (authDb == null)
                return BadRequest("No existe petición.");

            string userId = GetUserId();

            if (authDb.TaskUserAuth1Id == userId)
                authDb.WeeklyTaskAuth1 = true;
            else if (authDb.TaskUserAuth2Id == userId)
                authDb.WeeklyTaskAuth2 = true;
            else if (authDb.PayrollUserAuthId == userId)
                authDb.WeeklyPayrollAuth = true;

            var week = await _context.ProjectCalendarWeeks
                    .Include(x => x.ProjectCalendar)
                    .Include(x => x.ProjectCalendar.Project)
                    .FirstOrDefaultAsync(x => x.Id == id);

            //Metodo que crea la cabecera de la planilla
            if (authDb.WeeklyTaskAuth1 && authDb.WeeklyTaskAuth2 && !authDb.WeeklyPayrollAuth)
            {
                authDb.Title = week.ProjectCalendar.Project.Abbreviation + "-Planilla Semanal";
                authDb.Text = "Se solicita aprobación del tareo semanal de la semana #" + week.WeekNumber + ".";
            }

            if (authDb.WeeklyPayrollAuth)
            {
                week.IsClosed = true;
            }

            await _context.SaveChangesAsync();

            return Ok();
        }
        #endregion

        #region Calculo de Planilla
        [HttpPost("calcular")]
        public async Task<IActionResult> CalculatePayroll(Guid weekId)
        {
            var auths = await _context.PayrollAuthorizationRequests
                .FirstOrDefaultAsync(x => x.WeekId == weekId);

            if (!auths.WeeklyTaskAuth1 || !auths.WeeklyTaskAuth1)
                return BadRequest("No se cuentan con las autorizaciones necesarias.");

            var currentWeekHeader = await _context.PayrollMovementHeaders
                .Include(x => x.ProjectCalendarWeek)
                .FirstOrDefaultAsync(x => x.ProjectCalendarWeekId == weekId);

            if (currentWeekHeader == null)
            {
                var week = await _context.ProjectCalendarWeeks
                    .Include(x => x.ProjectCalendar)
                    .FirstOrDefaultAsync(x => x.Id == weekId);

                currentWeekHeader = new PayrollMovementHeader
                {
                    ProjectId = week.ProjectCalendar.ProjectId,
                    ProjectCalendarWeekId = week.Id,
                    ProcessStatus = 0
                };

                await _context.PayrollMovementHeaders.AddAsync(currentWeekHeader);

                await _context.SaveChangesAsync();
            }
                

            var currentWeek = currentWeekHeader.ProjectCalendarWeek;

            // Verificamos la siguiente semana,
            // para saber si ejecutamos el ajuste de EsSalud
            var nextWeek = await _context.ProjectCalendarWeeks
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.ProjectCalendarId == currentWeek.ProjectCalendarId &&
                                    x.WeekNumber == currentWeek.WeekNumber+1);
            var sameMonth = false;
            if (nextWeek != null)
                sameMonth = (currentWeek.Month == nextWeek.Month);

            // Obtenemos la cantidad de semanas del año
            // Necesario para el calculo de la renta del año
            var numWeeks = await _context.ProjectCalendarWeeks
                .Where(x => x.ProjectCalendarId == currentWeek.ProjectCalendarId)
                .CountAsync();

            // Traemos todas las variables necesarias para el calculo
            var payrollVariables = await _context.PayrollVariables
                .AsNoTracking()
                .ToListAsync();

            // Variables separadas por tipo
            var typeTVariables = payrollVariables.Where(x => x.Code.Contains("T")).OrderBy(x => x.Type).ToList();
            var typeHVariables = payrollVariables.Where(x => x.Code.Contains("H")).OrderBy(x => x.Type).ToList();
            var typeDVariables = payrollVariables.Where(x => x.Code.Contains("D")).OrderBy(x => x.Type).ToList();
            var typeAVariables = payrollVariables.Where(x => x.Code.Contains("A")).OrderBy(x => x.Type).ToList();
            //var typePVariables = payrollVariables.Where(x => x.Code.Contains("P")).OrderBy(x => x.Type).ToList();

            // SqlParameters
            SqlParameter projectParam = new SqlParameter("@ProjectId", currentWeekHeader.ProjectId);
            SqlParameter calendarParam = new SqlParameter("@CalendarId", currentWeek.ProjectCalendarId);
            SqlParameter weekStartParam = new SqlParameter("@WeekStart", currentWeek.WeekStart);
            SqlParameter weekEndParam = new SqlParameter("@WeekEnd", currentWeek.WeekEnd);
            SqlParameter monthParam = new SqlParameter("@Month", currentWeek.Month);
            SqlParameter weekParam = new SqlParameter("@WeekId", weekId);
            SqlParameter ceasedParam = new SqlParameter("@OnlyCeased", false);

            // Traemos el acumulado de horas del tareo diario y la infor de los obreros
            var workerWeekTasks = await _context.Set<UspPayrollWorkerInfoAndDailyTask>().FromSqlRaw("execute HumanResources_uspPayrollWorkerInfoAndDailyTasks @WeekStart, @WeekEnd, @ProjectId"
                , weekStartParam, weekEndParam, projectParam)
                .AsNoTracking()
                .IgnoreQueryFilters()
                .ToListAsync();

            // Traemos los conceptos de la planilla
            var payrollConceptsFormulas = await _context.PayrollConceptFormulas
                .Include(x => x.PayrollConcept)
                .Where(x => x.LaborRegimeId == ConstantHelpers.PayrollConceptFormula.LaborRegime.CIVILCONSTRUCTION)
                .AsNoTracking()
                .ToListAsync();

            // Los conceptos son agrupados por categoría
            var remConcepts = payrollConceptsFormulas.Where(x => x.PayrollConcept.CategoryId == ConstantHelpers.PayrollConcept.Category.INCOMES).ToList();
            var disConcepts = payrollConceptsFormulas.Where(x => x.PayrollConcept.CategoryId == ConstantHelpers.PayrollConcept.Category.DISCOUNTS).ToList();
            var conConcepts = payrollConceptsFormulas.Where(x => x.PayrollConcept.CategoryId == ConstantHelpers.PayrollConcept.Category.CONTRIBUTIONS).ToList();
            var totConcepts = payrollConceptsFormulas.Where(x => x.PayrollConcept.CategoryId == ConstantHelpers.PayrollConcept.Category.TOTALS).ToList();
            var dayConcepts = payrollConceptsFormulas.Where(x => x.PayrollConcept.CategoryId == ConstantHelpers.PayrollConcept.Category.DAILY).ToList();
            var accConcepts = payrollConceptsFormulas.Where(x => x.PayrollConcept.CategoryId == ConstantHelpers.PayrollConcept.Category.CONTROL).ToList();

            // Cargamos todos los conceptos fijos
            var workersFixedConcepts = await _context.WorkerFixedConcepts
                .Include(x => x.PayrollConcept)
                .ToListAsync();


            // Traemos los acumulados de renta del año
            var workersControlConcepts = await _context.Set<UspPayrollWorkerConcept>().FromSqlRaw("execute HumanResources_uspPayrollWorkerFifthRent @CalendarId, @WeekStart, @WeekEnd, @ProjectId"
                    , calendarParam, weekStartParam, weekEndParam, projectParam)
                .AsNoTracking()
                .IgnoreQueryFilters()
                .ToListAsync();

            // Traemos los aportes de essalud vida del mes y sctr
            workersControlConcepts.AddRange(
                await _context.Set<UspPayrollWorkerConcept>().FromSqlRaw("execute HumanResources_uspPayrollWorkerHealthLifeContribution @WeekId"
                    , weekParam)
                .AsNoTracking()
                .IgnoreQueryFilters()
                .ToListAsync()
            );

            // Traemos los acumulados de aportes essalud
            // Solo en el caso de ser la última semana o de haber liquidados
            var hasCeased = (workerWeekTasks.Where(x => x.IsCeased).Count()) > 0;
            if (hasCeased || !sameMonth)
            {
                ceasedParam.Value = sameMonth ? hasCeased : false;
                workersControlConcepts.AddRange(
                    await _context.Set<UspPayrollWorkerConcept>().FromSqlRaw("execute HumanResources_uspPayrollWorkerHealthContribution @WeekId, @OnlyCeased"
                        , weekParam, ceasedParam)
                    .AsNoTracking()
                    .IgnoreQueryFilters()
                    .ToListAsync()
                );
            }

            // Traemos los parametros de la planilla
            var parameters = _context.PayrollParameters.AsNoTracking().FirstOrDefault(x => x.ProjectId == currentWeekHeader.ProjectId);
            

            // Seteamos un contador para conocer cual es el último obrero a procesar
            // para enviar un aviso a la cola de guardado indicando que es el último obrero
            // y actualice el estado de proceso de la semana
            var count = 0;


            foreach (var workerWeekTask in workerWeekTasks)
            {
                count++;

                //Sección de Variables
                // Creamos las variables globales
                var workerVariablesDictionary = LoadGlobalVariablesDictionary(currentWeek.Month, currentWeek.ProcessType, parameters, !sameMonth);
                CreateTypeTVariables(workerWeekTask, typeTVariables, workerVariablesDictionary);
                CreateTypeHVariables(workerWeekTask, typeHVariables, workerVariablesDictionary);
                CreateTypeDVariables(workerWeekTask, typeDVariables, workerVariablesDictionary);
                CreateTypeAVariables(typeAVariables, workerVariablesDictionary, workersControlConcepts.Where(x => x.WorkerId == workerWeekTask.WorkerId).ToList());
                //CreateTypePVariables(typePVariables, workerVariablesDictionary);


                //Cargamos conceptos fijos del obrero
                var workerFixedConcepts = workersFixedConcepts.Where(x => x.WorkerId == workerWeekTask.WorkerId).ToList();
                //Sección de Conceptos
                var workerDetails = new List<PayrollMovementDetail>();
                CalculateTaskConcepts(workerDetails, dayConcepts, workerVariablesDictionary, currentWeekHeader.Id, workerWeekTask.WorkerId);
                CalculateRemunerativeConcepts(workerDetails, remConcepts, workerVariablesDictionary, currentWeekHeader.Id, workerWeekTask.WorkerId, workerFixedConcepts);
                
                
                
                //Calcular Aporte 5ta
                CalculateTaxes(workerVariablesDictionary, currentWeek.WeekNumber, numWeeks);
                CalculateDiscountConcepts(workerDetails, disConcepts, workerVariablesDictionary, currentWeekHeader.Id, workerWeekTask.WorkerId);
                CalculateContributionConcepts(workerDetails, conConcepts, workerVariablesDictionary, currentWeekHeader.Id, workerWeekTask.WorkerId);
                UpdateTotalConcepts(workerDetails, totConcepts, currentWeekHeader.Id, workerWeekTask.WorkerId);
                CalculateControlConcepts(workerDetails, accConcepts, workerVariablesDictionary, currentWeekHeader.Id, workerWeekTask.WorkerId);
                

                _queuedBackgroundService.QueueBackgroundWorkItem(new WorkItem {
                    Details = workerDetails, 
                    IsTheLast = (count == workerWeekTasks.Count),
                    IsTheFirst = (count == 1)
                });
            }            

            return Ok("Planilla sin errores, calculo de la planilla en proceso...");
        }

        

        //[HttpPost("calcular/obrero")]
        //public async Task<IActionResult> CalculateWorkerPayroll(Guid headerId, Guid workerId)
        //{
        //    var workerVariables = await _context.PayrollWorkerVariables
        //        .Include(x => x.PayrollVariable)
        //        .Where(x => x.PayrollMovementHeaderId == headerId && x.WorkerId == workerId)
        //        .ToListAsync();

        //    var workerPrevDetails = await _context.PayrollMovementDetails
        //        .Where(x => x.PayrollMovementHeaderId == headerId && x.WorkerId == workerId)
        //        .ToListAsync();

        //    var workerVariablesDictionary = workerVariables.ToDictionary(x => x.PayrollVariable.Code, y => y.Value);

        //    var header = await _context.PayrollMovementHeaders.Include(x => x.ProjectCalendarWeek).FirstOrDefaultAsync(x => x.Id == headerId);
        //    var parameters = _context.PayrollParameters.FirstOrDefault(x => x.ProjectId == header.ProjectId);

        //    LoadGlobalVariablesDictionary(header.ProjectCalendarWeek.Month, header.ProjectCalendarWeek.ProcessType, parameters)
        //        .ToList()
        //        .ForEach(x => workerVariablesDictionary.Add(x.Key, x.Value));

        //    //Para el Cálculo
        //    var payrollConceptsFormulas = await _context.PayrollConceptFormulas
        //        .Include(x => x.PayrollConcept)
        //        .Where(x => x.LaborRegimeId == ConstantHelpers.PayrollConceptFormula.LaborRegime.CIVILCONSTRUCTION)
        //        .ToListAsync();

        //    var remConcepts = payrollConceptsFormulas.Where(x => x.PayrollConcept.CategoryId == ConstantHelpers.PayrollConcept.Category.INCOMES).ToList();
        //    var disConcepts = payrollConceptsFormulas.Where(x => x.PayrollConcept.CategoryId == ConstantHelpers.PayrollConcept.Category.DISCOUNTS).ToList();
        //    var conConcepts = payrollConceptsFormulas.Where(x => x.PayrollConcept.CategoryId == ConstantHelpers.PayrollConcept.Category.CONTRIBUTIONS).ToList();
        //    var totConcepts = payrollConceptsFormulas.Where(x => x.PayrollConcept.CategoryId == ConstantHelpers.PayrollConcept.Category.TOTALS).ToList();

        //    //Sección de Conceptos
        //    var workerDetails = new List<PayrollMovementDetail>();
        //    CalculateTaskConcepts(workerDetails, dayConcepts, workerVariablesDictionary, currentWeekHeader.Id, workerWeekTask.WorkerId);
        //    CalculateRemunerativeConcepts(workerDetails, remConcepts, workerVariablesDictionary, currentWeekHeader.Id, workerWeekTask.WorkerId);
        //    //Calcular Aporte 5ta
        //    CalculateTaxes(workerVariablesDictionary, currentWeek.WeekNumber, numWeeks);
        //    CalculateDiscountConcepts(workerDetails, disConcepts, workerVariablesDictionary, currentWeekHeader.Id, workerWeekTask.WorkerId);
        //    CalculateContributionConcepts(workerDetails, conConcepts, workerVariablesDictionary, currentWeekHeader.Id, workerWeekTask.WorkerId);
        //    UpdateTotalConcepts(workerDetails, totConcepts, currentWeekHeader.Id, workerWeekTask.WorkerId);
        //    CalculateControlConcepts(workerDetails, accConcepts, workerVariablesDictionary, currentWeekHeader.Id, workerWeekTask.WorkerId);

        //    using (var transaction = _context.Database.BeginTransaction())
        //    {
        //        await _context.BulkDeleteAsync(workerPrevDetails);
        //        await _context.BulkUpdateAsync(workerVariables);
        //        await _context.BulkInsertAsync(workerDetails);

        //        await transaction.CommitAsync();
        //    }

        //    return Ok();
        //}

        private void CalculateTaxes(Dictionary<string, string> workerVariablesDictionary, int currentWeekNumber, int totalWeeks)
        {
            var rentaAfectaPrevia = decimal.Parse(workerVariablesDictionary["A19"]);
            var rentaAfecta = decimal.Parse(workerVariablesDictionary["A20"]);
            var aporteQtaPrevia = decimal.Parse(workerVariablesDictionary["A22"]);
            //var aporteQta = decimal.Parse(workerVariablesDictionary["A23"]);
            var uit = decimal.Parse(workerVariablesDictionary["G01"]);
            //var jornal = decimal.Parse(workerVariablesDictionary["T01"]);
            //var liqSem = bool.Parse(workerVariablesDictionary["T10"]);
            var isCeased = bool.Parse(workerVariablesDictionary["T27"]);


            var totalProyectado = rentaAfectaPrevia + rentaAfecta * (!isCeased ? (totalWeeks - currentWeekNumber + 1) : 1);
            //if (!liqSem)
            //{
            //    var gratJul = 0.0M;
            //    if (projectWeek.Month <= 7)
            //        gratJul = 40.0M * jornal * (210.0M - (7.0M * projectWeek.WeekNumber)) / 210.0M;
            //    var gratDic = 40.0M * jornal;
            //    if (projectWeek.Month > 7)
            //        gratDic = gratDic * (150.0M - (7.0M * (projectWeek.WeekNumber-7))) / 150.0M;
            //    totalProyectado += (gratJul + gratDic);
            //}
            totalProyectado -= (7.0M * uit);
            var totalAporteProyectado = 0.0M;
            var uit5 = 5.0M * uit;
            var uit20 = 20.0M * uit;
            var uit35 = 35.0M * uit;
            var uit45 = 45.0M * uit;
            if (totalProyectado <= 0)
                workerVariablesDictionary["A23"] = "0";
            else if (totalProyectado <= uit5)
                totalAporteProyectado = totalProyectado * 0.08M;
            else
            {
                totalAporteProyectado = uit5 * 0.08M;
                if (totalProyectado <= uit20)
                    totalAporteProyectado += ((totalProyectado - uit5) * 0.14M);
                else
                {
                    totalAporteProyectado += ((uit20 - uit5) * 0.14M);
                    if (totalProyectado <= uit35)
                        totalAporteProyectado += ((totalProyectado - uit20) * 0.17M);
                    else
                    {
                        totalAporteProyectado += ((uit35 - uit20) * 0.17M);
                        if (totalAporteProyectado <= uit45)
                            totalAporteProyectado += ((totalProyectado - uit45) * 0.20M);
                        else
                        {
                            totalAporteProyectado += ((uit45 - uit35) * 0.20M + (totalProyectado - uit45) * 0.30M);
                        }   
                    }
                }
            }
            totalAporteProyectado -= aporteQtaPrevia;
            var aporteQta = totalAporteProyectado / (!isCeased ? (totalWeeks - currentWeekNumber + 1.0M) : 1);
            workerVariablesDictionary["A23"] = aporteQta.ToString("F2", CultureInfo.InvariantCulture);
        }
        #endregion

        #region RemunerativeConcepts
        private void CalculateControlConcepts(List<PayrollMovementDetail> workerDetails, List<PayrollConceptFormula> payrollConceptsFormulas, Dictionary<string, string> workerVariablesDictionary, Guid headerId, Guid workerId)
        {
            foreach (var concept in payrollConceptsFormulas)
            {
                var detail = new PayrollMovementDetail
                {
                    Id = Guid.NewGuid(),
                    PayrollConcept = concept.PayrollConcept,
                    PayrollConceptId = concept.PayrollConcept.Id,
                    PayrollMovementHeaderId = headerId,
                    WorkerId = workerId,
                    Value = EvaluateFormula(concept.Formula, workerVariablesDictionary)
                };

                UpdateAffectedConcepts(detail, concept, workerVariablesDictionary);

                workerDetails.Add(detail);
            }
        }

        private void UpdateTotalConcepts(List<PayrollMovementDetail> workerDetails, List<PayrollConceptFormula> payrollConceptsFormulas, Guid headerId, Guid workerId)
        {
            decimal totalRem = 0.0M, totalCon = 0.0M, totalDis = 0.0M;

            var remConcepts = workerDetails.Where(x => x.PayrollConcept.Code.Contains('R')).ToList();
            var desConcepts = workerDetails.Where(x => x.PayrollConcept.Code.Contains('D')).ToList();
            var conConcepts = workerDetails.Where(x => x.PayrollConcept.Code.Contains('A')).ToList();

            foreach (var totConcept in payrollConceptsFormulas)
            {
                var code = totConcept.PayrollConcept.Code;
                var detail = new PayrollMovementDetail {
                    Id = Guid.NewGuid(),
                    PayrollConcept = totConcept.PayrollConcept,
                    PayrollConceptId = totConcept.PayrollConcept.Id,
                    PayrollMovementHeaderId = headerId,
                    WorkerId = workerId,
                    Value = 0.0M
                };
                decimal value = 0.0M;
                switch (code)
                {
                    case "T01":
                        //Total Remuneración
                        value = remConcepts.Select(x => x.Value).Sum();
                        totalRem = value;
                        break;
                    case "T02":
                        //Total Descuentos
                        value = desConcepts.Select(x => x.Value).Sum();
                        totalDis = value;
                        break;
                    case "T03":
                        //Total Aportes
                        value = conConcepts.Select(x => x.Value).Sum();
                        totalCon = value;
                        break;
                    case "T04":
                        //Total Costo
                        value = totalRem + totalCon;
                        break;
                    case "T05":
                        //Total Neto
                        value = totalRem - totalDis;
                        break;
                }
                detail.Value = value;
                workerDetails.Add(detail);
            }
        }

        private void CalculateContributionConcepts(List<PayrollMovementDetail> workerDetails, List<PayrollConceptFormula> payrollConceptsFormulas, Dictionary<string, string> workerVariablesDictionary, Guid headerId, Guid workerId)
        {
            foreach (var concept in payrollConceptsFormulas)
            {
                var detail = new PayrollMovementDetail
                {
                    Id = Guid.NewGuid(),
                    PayrollConcept = concept.PayrollConcept,
                    PayrollConceptId = concept.PayrollConcept.Id,
                    PayrollMovementHeaderId = headerId,
                    WorkerId = workerId,
                    Value = EvaluateFormula(concept.Formula, workerVariablesDictionary)
                };

                workerDetails.Add(detail);
            }
        }

        private void CalculateDiscountConcepts(List<PayrollMovementDetail> workerDetails, List<PayrollConceptFormula> payrollConceptsFormulas, Dictionary<string, string> workerVariablesDictionary, Guid headerId, Guid workerId)
        {
            foreach (var concept in payrollConceptsFormulas)
            {
                var detail = new PayrollMovementDetail
                {
                    Id = Guid.NewGuid(),
                    PayrollConcept = concept.PayrollConcept,
                    PayrollConceptId = concept.PayrollConcept.Id,
                    PayrollMovementHeaderId = headerId,
                    WorkerId = workerId,
                    Value = EvaluateFormula(concept.Formula, workerVariablesDictionary)
                };

                workerDetails.Add(detail);
            }
        }

        private void CalculateRemunerativeConcepts(List<PayrollMovementDetail> workerDetails, List<PayrollConceptFormula> payrollConceptsFormulas, Dictionary<string, string> workerVariablesDictionary, Guid headerId, Guid workerId, List<WorkerFixedConcept> workerFixedConcepts)
        {
            foreach (var concept in payrollConceptsFormulas)
            {
                var hasFixedValue = workerFixedConcepts.FirstOrDefault(x => x.PayrollConceptId == concept.PayrollConcept.Id);

                var detail = new PayrollMovementDetail
                {
                    Id = Guid.NewGuid(),
                    PayrollConcept = concept.PayrollConcept,
                    PayrollConceptId = concept.PayrollConcept.Id,
                    PayrollMovementHeaderId = headerId,
                    WorkerId = workerId,
                    Value = (hasFixedValue != null ? decimal.Parse(hasFixedValue.FixedValue.ToString("F2", CultureInfo.InvariantCulture)) : EvaluateFormula(concept.Formula, workerVariablesDictionary))
                };

                UpdateAffectedConcepts(detail, concept, workerVariablesDictionary);

                workerDetails.Add(detail);
            }
        }

        private void CalculateTaskConcepts(List<PayrollMovementDetail> workerDetails, List<PayrollConceptFormula> payrollConceptsFormulas, Dictionary<string, string> workerVariablesDictionary, Guid headerId, Guid workerId)
        {
            foreach (var concept in payrollConceptsFormulas)
            {
                var detail = new PayrollMovementDetail
                {
                    Id = Guid.NewGuid(),
                    PayrollConcept = concept.PayrollConcept,
                    PayrollConceptId = concept.PayrollConcept.Id,
                    PayrollMovementHeaderId = headerId,
                    WorkerId = workerId,
                    Value = EvaluateFormula(concept.Formula, workerVariablesDictionary)
                };
                workerDetails.Add(detail);
            }
        }

        private void UpdateAffectedConcepts(PayrollMovementDetail detail, PayrollConceptFormula concept, Dictionary<string, string> workerVariablesDictionary)
        {
            //A08 Afecto EsSalud
            var code = "A08";
            if (concept.IsAffectedToEsSalud)
                workerVariablesDictionary[code] = (decimal.Parse(workerVariablesDictionary[code]) + detail.Value).ToString();

            //A20 Afecto Renta Qta
            code = "A20";
            if (concept.IsAffectedToQta)
                workerVariablesDictionary[code] = (decimal.Parse(workerVariablesDictionary[code]) + detail.Value).ToString();
            //A25 Afecto AFP
            code = "A25";
            if (concept.IsAffectedToAfp)
                workerVariablesDictionary[code] = (decimal.Parse(workerVariablesDictionary[code]) + detail.Value).ToString();
            //A26 Afecto ONP
            code = "A26";
            if (concept.IsAffectedToAfp)
                workerVariablesDictionary[code] = (decimal.Parse(workerVariablesDictionary[code]) + detail.Value).ToString();
            //A27 Afecto Retención Judicial
            code = "A27";
            if (concept.IsAffectedToAfp)
                workerVariablesDictionary[code] = (decimal.Parse(workerVariablesDictionary[code]) + detail.Value).ToString();
        }

        #endregion

        #region CreateVariables
        private Dictionary<string, string> LoadGlobalVariablesDictionary(int month, int processType, PayrollParameter parameters, bool isLastWeek)
        {
            var dictionary = new Dictionary<string, string>
            {
                //UIT
                { "G01", parameters.UIT.ToString() },
                //Factor Gratificación Jul/Dic
                { "G02", month <= 7 ? "210" : "150" },
                //Tipo de Cambio Dolar Soles
                { "G03", parameters.DollarExchangeRate.ToString() },
                //Remuneracion Maxima Asegurable
                { "G04", parameters.MaximumInsurableRemuneration.ToString() },
                //Es última semana
                { "G05", isLastWeek.ToString() },
                //Sueldo Mínimo
                { "G07", parameters.MinimumWage.ToString() },
                //Tasa SCTR
                { "G08", parameters.SCTRRate.ToString() },
                //Aporte EsSalud + Vida
                { "G09", parameters.EsSaludMasVidaCost.ToString() },
                //Tipo de Proceso Semanal
                { "G10", processType.ToString() },
                //Mes al que pertenece la semana
                { "G11", month.ToString() },
                //Cuota Sindical
                { "G12", parameters.UnionFee.ToString() },
                //SCTR Salud Fijo
                { "G13", parameters.SCTRHealthFixed.ToString() },
                //SCTR Pension Fijo
                { "G14", parameters.SCTRPensionFixed.ToString() }
            };

            return dictionary;
        }
        
        //private void CreateTypePVariables(List<PayrollVariable> payrollVariables, Dictionary<string, string> workerVariablesDictionary)
        //{
        //    //No hay provisiones ya el personal obrero se maneja con liquidación semanal
        //    foreach (var variable in payrollVariables)
        //    {
        //        var workerVariable = new PayrollWorkerVariable
        //        {
        //            Id = Guid.NewGuid(),
        //            PayrollMovementHeaderId = headerId,
        //            PayrollVariableId = variable.Id,
        //            PayrollVariable = variable,
        //            WorkerId = workerId,
        //            Value = "0"
        //        };

        //        workerVariables.Add(workerVariable);
        //    }
        //}

        private void CreateTypeAVariables(List<PayrollVariable> payrollVariables, Dictionary<string, string> workerVariablesDictionary, List<UspPayrollWorkerConcept> workerControlConcepts)
        {
            var concept = new UspPayrollWorkerConcept();
            foreach (var variable in payrollVariables)
            {
                string value;
                switch (variable.Code)
                {
                    case "A01":
                        //Acumulado Previo Días Gratificación
                        value = "0";
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "A02":
                        //Acumulado Días Gratificación
                        value = EvaluateFormula(variable.Formula, workerVariablesDictionary).ToString();
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "A03":
                        //Acumulado Previo Días Vacaciones
                        value = "0";
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "A04":
                        //Acumulado Días Vacaciones
                        value = EvaluateFormula(variable.Formula, workerVariablesDictionary).ToString();
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "A05":
                        //Acumulado Previo Días CTS
                        value = "0";
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "A06":
                        //Acumulado Días CTS
                        value = EvaluateFormula(variable.Formula, workerVariablesDictionary).ToString();
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "A07":
                        //Acumulado Previo Afecto EsSalud
                        concept = workerControlConcepts.FirstOrDefault(x => x.Code == "C01");
                        value = (concept != null ? concept.Value.ToString() : "0");
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "A08":
                        //Afecto EsSalud
                        value = "0";
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "A09":
                        //Acumulado Afecto EsSalud
                        value = "0";
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "A10":
                        //Acumulado Previo Aporte EsSalud
                        concept = workerControlConcepts.FirstOrDefault(x => x.Code == "A01");
                        value = (concept != null ? concept.Value.ToString() : "0");
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "A11":
                        //Aporte EsSalud
                        value = "0";
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "A12":
                        //Acumulado Aporte EsSalud
                        value = "0";
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "A13":
                        //Acumulado Previo Aporte EPS
                        concept = workerControlConcepts.FirstOrDefault(x => x.Code == "A02");
                        value = (concept != null ? concept.Value.ToString() : "0");
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "A14":
                        //Aporte EPS
                        value = "0";
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    //case "A15":
                    //    //Acumulado Aporte EPS
                    //    value = "0";
                    //    workerVariablesDictionary.Add(variable.Code, value);
                    //    break;
                    case "A16":
                        //Acumulado Previo Aporte EsSalud + Vida
                        concept = workerControlConcepts.FirstOrDefault(x => x.Code == "A06");
                        value = (concept != null ? concept.Value.ToString() : "0");
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "A17":
                        //Acumulado Previo SCTR Salud Fijo
                        concept = workerControlConcepts.FirstOrDefault(x => x.Code == "A04");
                        value = (concept != null ? concept.Value.ToString() : "0");
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "A18":
                        //Acumulado Previo SCTR Pensión Fijo
                        concept = workerControlConcepts.FirstOrDefault(x => x.Code == "A05");
                        value = (concept != null ? concept.Value.ToString() : "0");
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "A19":
                        //Acumulado Previo Afecto Renta Qta
                        //concept = workerControlConcepts.FirstOrDefault(x => x.Code == "C02");
                        value = "0";
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "A20":
                        //Afecto Renta Qta
                        value = "0";
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    //case "A21":
                    //    //Acumulado Afecto Renta Qta
                    //    value = "0";
                    //    workerVariablesDictionary.Add(variable.Code, value);
                    //    break;
                    case "A22":
                        //Acumulado Previo Aporte Renta Qta
                        concept = workerControlConcepts.FirstOrDefault(x => x.Code == "D07");
                        value = (concept != null ? concept.Value.ToString() : "0");
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "A23":
                        //Aporte Renta Qta
                        value = "0";
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    //case "A24":
                    //    //Acumulado Aporte Renta Qta
                    //    value = "0";
                    //    workerVariablesDictionary.Add(variable.Code, value);
                    //    break;
                    case "A25":
                        //Afecto AFP
                        value = "0";
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "A26":
                        //Afecto ONP
                        value = "0";
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "A27":
                        //Afecto Retención Judicial
                        value = "0";
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "A28":
                        //Acumulado Previo Horas Extras
                        value = "0";
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "A29":
                        //Acumulado Horas Extras
                        value = EvaluateFormula(variable.Formula, workerVariablesDictionary).ToString();
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                }
            }
        }

        private void CreateTypeDVariables(UspPayrollWorkerInfoAndDailyTask workerWeekTask, List<PayrollVariable> payrollVariables, Dictionary<string, string> workerVariablesDictionary)
        {
            foreach (var variable in payrollVariables)
            {
                string value;
                switch (variable.Code)
                {
                    case "D01":
                        //Días Trabajados
                        value = EvaluateFormula(variable.Formula, workerVariablesDictionary).ToString();
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "D02":
                        //Días Gratificación
                        value = EvaluateFormula(variable.Formula, workerVariablesDictionary).ToString();
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "D03":
                        //Días Vacaciones
                        value = EvaluateFormula(variable.Formula, workerVariablesDictionary).ToString();
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "D04":
                        //Días CTS
                        value = EvaluateFormula(variable.Formula, workerVariablesDictionary).ToString();
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "D05":
                        //Días Asistidos
                        value = workerWeekTask.Attendance.ToString();
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "D06":
                        //Días Licencia Médica
                        value = workerWeekTask.MedicalLeave.ToString();
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "D07":
                        //Días Faltas
                        value = workerWeekTask.NonAttendance.ToString();
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "D08":
                        //Días Suspensión Laboral
                        value = workerWeekTask.LaborSuspension.ToString();
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "D09":
                        //Días Sin Goce
                        value = workerWeekTask.UnPaidLeave.ToString();
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                }
            }
        }

        private void CreateTypeHVariables(UspPayrollWorkerInfoAndDailyTask workerWeekTask, List<PayrollVariable> payrollVariables, Dictionary<string, string> workerVariablesDictionary)
        {
            foreach (var variable in payrollVariables)
            {
                string value;
                switch (variable.Code)
                {
                    case "H01":
                        //Horas Normales
                        value = workerWeekTask.HoursNormal.ToString();
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "H02":
                        //Horas Dominical
                        value = EvaluateFormula(variable.Formula, workerVariablesDictionary).ToString();
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "H03":
                        //Horas Descanso Médico
                        value = workerWeekTask.HoursMedicalRest.ToString();
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "H04":
                        //Horas Licencia Paternidad
                        value = workerWeekTask.HoursPaternityLeave.ToString();
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "H05":
                        //Horas Feriado
                        value = workerWeekTask.HoursHoliday.ToString();
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "H06":
                        //Horas 60%
                        value = workerWeekTask.Hours60Percent.ToString();
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "H07":
                        //Horas 100%
                        value = workerWeekTask.Hours100Percent.ToString();
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "H08":
                        //Horas Semana
                        value = EvaluateFormula(variable.Formula, workerVariablesDictionary).ToString();
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "H09":
                        //Horas Semana Efectiva
                        value = EvaluateFormula(variable.Formula, workerVariablesDictionary).ToString();
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "H10":
                        //Horas Con Goce
                        value = workerWeekTask.HoursPaidLeave.ToString();
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                }
            }
        }

        private void CreateTypeTVariables(UspPayrollWorkerInfoAndDailyTask workerWeekTask, List<PayrollVariable> payrollVariables, Dictionary<string, string> workerVariablesDictionary)
        {
            foreach (var variable in payrollVariables)
            {
                string value;
                switch (variable.Code)
                {
                    case "T01":
                        //Jornal
                        value = workerWeekTask.DayWage.ToString();
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "T02":
                        //Nro. Hijos
                        value = workerWeekTask.NumberOfChildren.ToString();
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "T03":
                        //BUC
                        value = workerWeekTask.BUCRate.ToString();
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "T04":
                        //Aporta Cuota Sindical
                        value = workerWeekTask.HasUnionFee.ToString();
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "T05":
                        //Aporta SCTR
                        value = workerWeekTask.HasSctr.ToString();
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "T06":
                        //Tipo Sctr Salud
                        value = workerWeekTask.SctrHealthType.ToString();
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "T07":
                        //Tipo Sctr Pensión
                        value = workerWeekTask.SctrPensionType.ToString();
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "T08":
                        //Retención Judicial Monto Fijo
                        value = workerWeekTask.JudicialRetentionFixedAmmount.ToString();
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "T09":
                        //Retención Judicial Porcentaje
                        value = workerWeekTask.JudicialRetentionPercentRate.ToString();
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "T10":
                        //Tiene Liq. Semanal
                        value = workerWeekTask.HasWeeklysettlement.ToString();
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "T12":
                        //Tipo Trabajador - Regimen Laboral
                        value = workerWeekTask.LaborRegimen.ToString();
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "T13":
                        //Afiliado EPS
                        value = workerWeekTask.HasEPS.ToString();
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "T14":
                        //Aporta EsSalud +Vida
                        value = workerWeekTask.HasEsSaludPlusVida.ToString();
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "T15":
                        //Regimen de Pension
                        value = workerWeekTask.PensionFundCode.ToString();
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "T16":
                        //Tasa Fondo 
                        value = workerWeekTask.FundRate.ToString();
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "T17":
                        //Tasa Comision Flujo
                        value = workerWeekTask.FlowComissionRate.ToString();
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "T18":
                        //Tasa Comision Mixta
                        value = workerWeekTask.MixedComissionRate.ToString();
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "T19":
                        //Tasa Seguro Invalidez
                        value = workerWeekTask.DisabilityInsuranceRate.ToString();
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "T20":
                        //Tasa Jubiliación Anticipada
                        value = EvaluateFormula(variable.Formula, workerVariablesDictionary).ToString();
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "T21":
                        //Tasa Adicional por Regimen Laboral
                        value = EvaluateFormula(variable.Formula, workerVariablesDictionary).ToString();
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "T22":
                        //Tasa EsSalud
                        value = EvaluateFormula(variable.Formula, workerVariablesDictionary).ToString();
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "T23":
                        //Tasa EPS
                        value = EvaluateFormula(variable.Formula, workerVariablesDictionary).ToString();
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "T24":
                        //Remuneración Diaria por Subsidio
                        value = EvaluateFormula(variable.Formula, workerVariablesDictionary).ToString();
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "T25":
                        //Categoría Obrero
                        value = workerWeekTask.Category.ToString();
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                    case "T27":
                        //Es Cesado
                        value = workerWeekTask.IsCeased.ToString();
                        workerVariablesDictionary.Add(variable.Code, value);
                        break;
                }
            }
        }
        #endregion
        
        #region Formula Methods
        private decimal EvaluateFormula(string formula, Dictionary<string, string> workerVariablesDictionary)
        {
            string pattern = @"([H|D|T|G|A|P])\w+";
            MatchCollection matches;

            matches = Regex.Matches(formula, pattern);
            formula = ReplaceFormulaVariables(formula, matches, workerVariablesDictionary);

            return CalculateFormula(formula);
        }

        private string ReplaceFormulaVariables(string formula, MatchCollection matches, Dictionary<string, string> workerVariablesDictionary)
        {
            var newMatches = matches
                .OfType<Match>()
                .Select(m => m.Groups[0].Value)
                .Distinct();

            foreach (var match in newMatches)
            {
                formula = formula.Replace(match, workerVariablesDictionary[match].ToString());
            }
            return formula;
        }

        private decimal CalculateFormula(string formula)
        {
            DataTable dt = new DataTable();

            var result = dt.Compute(formula, "");
            decimal resultFull = decimal.Parse(result.ToString());
            string resultRounded = resultFull.ToString("F2", CultureInfo.InvariantCulture);
            var evaluatedFormula = decimal.Parse(resultRounded);
            return evaluatedFormula;
        }
        #endregion

        #region Excels
        [HttpGet("exportar-excel")]
        public async Task<FileResult> Export(Guid? weekId = null, Guid? sewerGroupId = null, Guid? workFrontHeadId = null)
        {
            var result = await ToUspExcel(weekId, sewerGroupId, workFrontHeadId);
            return File(result.Data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", result.FileName);
        }

        private async Task<(byte[] Data, string FileName)> ToUspExcel(Guid? weekId = null, Guid? sewerGroupId = null, Guid? workFrontHeadId = null)
        {
            try
            {
                var week = await _context.ProjectCalendarWeeks
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == weekId.Value);

                var weekStartParam = new SqlParameter("@WeekStart", week.WeekStart);
                var weekEndParam = new SqlParameter("@WeekEnd", week.WeekEnd);
                var projectParam = new SqlParameter("@ProjectId", GetProjectId());

                var workerDailyTasks = await _context.Set<UspWorkersWeeklyTask>().FromSqlRaw("execute HumanResources_uspWorkersWeeklyTasks2 @WeekStart, @WeekEnd, @ProjectId"
                    , weekStartParam, weekEndParam, projectParam)
                    .AsNoTracking()
                    .IgnoreQueryFilters()
                    .ToListAsync();

                var summary = await _context.Set<UspWorkersWeeklyTaskSummary>().FromSqlRaw("execute HumanResources_uspWorkersWeeklyTasksSummary @WeekStart, @WeekEnd, @ProjectId"
                    , weekStartParam, weekEndParam, projectParam)
                    .AsNoTracking()
                    .IgnoreQueryFilters()
                    .ToListAsync();

                if (workFrontHeadId != null)
                    workerDailyTasks = workerDailyTasks.Where(x => x.WorkFrontHeadId == workFrontHeadId).ToList();

                if (sewerGroupId != null)
                    workerDailyTasks = workerDailyTasks.Where(x => x.SewerGroupId == sewerGroupId).ToList();

                #region ClosedXML
                using (XLWorkbook wb = new XLWorkbook())
                {
                    string filename = "TareoSemanal - " + week.YearWeekNumber + ".xlsx";

                    var wsSummary = wb.Worksheets.Add("Resumen");

                    var wsHome = wb.Worksheets.Add("Casa");
                    var wsCollab = wb.Worksheets.Add("Colaboradores");

                    AddHeaders(wsHome, week.WeekStart);
                    AddHeaders(wsCollab, week.WeekStart);

                    var rowHome = 3;
                    var rowCollab = 3;
                    var prevSg = Guid.Empty;

                    // TODO: Eliminar repetidos
                    var workerDailyTasksList = workerDailyTasks.Distinct().ToList();
                    var newWorkers = workerDailyTasksList.Where(x => x.WorkerIsNew).ToList();
                    var ceaseWorkers = workerDailyTasksList.Where(x => !x.WorkerIsActive).ToList();
                    
                    var summarytHomeWorkers = summary.FirstOrDefault(x => x.IsHome);
                    var homeWorkers = 0;
                    if (summarytHomeWorkers != null)
                        homeWorkers = summarytHomeWorkers.Workers;

                    var summaryCollabWorkers = summary.FirstOrDefault(x => !x.IsHome);
                    var collabWorkers = 0;
                    if (summaryCollabWorkers != null)
                        collabWorkers = summaryCollabWorkers.Workers;

                    var homeSgs = workerDailyTasksList.Where(x => x.IsHome).Select(x => x.SewerGroupId).Distinct().Count();
                    var collabSgs = workerDailyTasksList.Where(x => !x.IsHome).Select(x => x.SewerGroupId).Distinct().Count();

                    SetBackColor(wsHome, homeWorkers + homeSgs + 2);
                    SetBackColor(wsCollab, collabWorkers + collabSgs + 2);

                    foreach (var worker in workerDailyTasksList)
                    {
                        if (!worker.IsHome)
                        {
                            if (worker.SewerGroupId != prevSg)
                            {
                                prevSg = worker.SewerGroupId;
                                wsCollab.Cell($"A{rowCollab}").Value = worker.SewerGroupCode + " - " + worker.ProjectCollaboratorBusinessName + " - " + worker.ProjectCollaborator;
                                wsCollab.Range($"A{rowCollab}:AW{rowCollab}").Style.Fill.SetBackgroundColor(XLColor.Gray);
                                rowCollab++;
                            }
                            AddInfo(worker, wsCollab, rowCollab);
                            rowCollab++;
                        }
                        else
                        {
                            if (worker.SewerGroupId != prevSg)
                            {
                                prevSg = worker.SewerGroupId;
                                wsHome.Cell($"A{rowHome}").Value = worker.SewerGroupCode + " - CASA - " + (string.IsNullOrEmpty(worker.ForemanEmployee) ? worker.ForemanWorker : worker.ForemanEmployee);
                                wsHome.Range($"A{rowHome}:AW{rowHome}").Style.Fill.SetBackgroundColor(XLColor.Gray);
                                rowHome++;
                            }
                            AddInfo(worker, wsHome, rowHome);
                            rowHome++;
                        }

                        wsHome.Columns(2, 56).AdjustToContents();
                        wsCollab.Columns(2, 56).AdjustToContents();

                        //var rngHome = wsHome.Range($"A1:AW{rowHome}");
                        //var rngCollab = wsCollab.Range($"A1:AW{rowCollab}");
                        //IXLCell reference = rngHome.FirstCell();
                        //reference.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        //reference.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                        //var newStyle = reference.Style;
                        //rngHome.Style.Border = newStyle.Border;
                        //rngCollab.Style.Border = newStyle.Border;
                    }

                    AddSummary(wsSummary, summary, newWorkers, ceaseWorkers);
                    using var stream = new MemoryStream();
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return (stream.ToArray(), filename);
                }
                #endregion
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return (null, "");
            }
        }       

        private void AddSummary(IXLWorksheet ws, List<UspWorkersWeeklyTaskSummary> summary, List<UspWorkersWeeklyTask> newWorkers, List<UspWorkersWeeklyTask> ceaseWorkers)
        {
            var backgroundColor = XLColor.FromArgb(22, 54, 92);
            var fontColor = XLColor.White;

            var home = summary.Where(x => x.IsHome).FirstOrDefault() ?? new UspWorkersWeeklyTaskSummary();
            var collab = summary.Where(x => !x.IsHome).FirstOrDefault() ?? new UspWorkersWeeklyTaskSummary();

            #region Obreros
            ws.Cell($"A2").Value = "Total Obreros:";
            ws.Cell($"A2").Style.Fill.SetBackgroundColor(backgroundColor);
            ws.Cell($"A2").Style.Font.SetFontColor(fontColor);
            ws.Cell($"B2").Value = home.Workers + collab.Workers;

            

            ws.Cell($"B3").Value = "Casa";
            ws.Cell($"C3").Value = "Colaboradores";
            ws.Cell($"D3").Value = "Total";
            ws.Range($"A3:D3").Style.Fill.SetBackgroundColor(backgroundColor);
            ws.Range($"A3:D3").Style.Font.SetFontColor(fontColor);

            //Casa y Colaboradores
            ws.Cell($"A4").Value = "OP";
            ws.Cell($"B4").Value = home.Operators;
            ws.Cell($"C4").Value = collab.Operators;
            ws.Cell($"D4").FormulaA1 = "=B4+C4";

            ws.Cell($"A5").Value = "OF";
            ws.Cell($"B5").Value = home.Officials;
            ws.Cell($"C5").Value = collab.Officials;
            ws.Cell($"D5").FormulaA1 = "=B5+C5";

            ws.Cell($"A6").Value = "PE";
            ws.Cell($"B6").Value = home.Pawns;
            ws.Cell($"C6").Value = collab.Pawns;
            ws.Cell($"D6").FormulaA1 = "=B6+C6";

            //Del Sindicato
            ws.Cell($"A7").Value = "OP-S";
            ws.Cell($"B7").Value = home.OperatorsS;
            ws.Cell($"C7").Value = collab.OperatorsS;
            ws.Cell($"D7").FormulaA1 = "=B7+C7";

            ws.Cell($"A8").Value = "OF-S";
            ws.Cell($"B8").Value = home.OfficialsS;
            ws.Cell($"C8").Value = home.OperatorsS;
            ws.Cell($"D8").FormulaA1 = "=B8+C8";

            ws.Cell($"A9").Value = "PE-S";
            ws.Cell($"B9").Value = home.PawnsS;
            ws.Cell($"C9").Value = collab.PawnsS;
            ws.Cell($"D9").FormulaA1 = "=B9+C9";

            //De la Población
            ws.Cell($"A10").Value = "OP-P";
            ws.Cell($"B10").Value = home.OperatorsP;
            ws.Cell($"C10").Value = collab.OperatorsP;
            ws.Cell($"D10").FormulaA1 = "=B10+C10";

            ws.Cell($"A11").Value = "OF-P";
            ws.Cell($"B11").Value = home.OfficialsP;
            ws.Cell($"C11").Value = collab.OfficialsP;
            ws.Cell($"D11").FormulaA1 = "=B11+C11";

            ws.Cell($"A12").Value = "PE-P";
            ws.Cell($"B12").Value = home.PawnsP;
            ws.Cell($"C12").Value = collab.PawnsP;
            ws.Cell($"D12").FormulaA1 = "=B12+C12";

            //Total
            ws.Cell($"B13").FormulaA1 = "=B4+B5+B6+B7+B8+B9+B10+B11+B12";
            ws.Cell($"C13").FormulaA1 = "=C4+C5+C6+C7+C8+C9+C10+C11+C12";
            ws.Cell($"D13").FormulaA1 = "=D4+D5+D6+D7+D8+D9+D10+D11+D12";

            ws.Range($"B13:D13").Style.Fill.SetBackgroundColor(backgroundColor);
            ws.Range($"B13:D13").Style.Font.SetFontColor(fontColor);
            var rngWorkers = ws.Range($"A2:D13");
            #endregion

            #region HH
            ws.Cell($"A15").Value = "Total Horas:";
            ws.Cell($"A15").Style.Fill.SetBackgroundColor(backgroundColor);
            ws.Cell($"A15").Style.Font.SetFontColor(fontColor);
            ws.Cell($"B15").Value = home.WorkersHours + collab.WorkersHours;
            ws.Cell($"B15").Style.NumberFormat.Format = "#,##0.00";

            ws.Cell($"B16").Value = "Casa";
            ws.Cell($"C16").Value = "Colaboradores";
            ws.Cell($"D16").Value = "Total HH";
            ws.Range($"A16:D16").Style.Fill.SetBackgroundColor(backgroundColor);
            ws.Range($"B16:D16").Style.Font.SetFontColor(fontColor);

            //Casa y Colaboradores
            ws.Cell($"A17").Value = "OP";
            ws.Cell($"B17").Value = home.OperatorsHours;
            ws.Cell($"C17").Value = collab.OperatorsHours;
            ws.Cell($"D17").FormulaA1 = "=B17+C17";

            ws.Cell($"A18").Value = "OF";
            ws.Cell($"B18").Value = home.OfficialsHours;
            ws.Cell($"C18").Value = collab.OfficialsHours;
            ws.Cell($"D18").FormulaA1 = "=B18+C18";

            ws.Cell($"A19").Value = "PE";
            ws.Cell($"B19").Value = home.PawnsHours;
            ws.Cell($"C19").Value = collab.PawnsHours;
            ws.Cell($"D19").FormulaA1 = "=B19+C19";

            //Del Sindicato
            ws.Cell($"A20").Value = "OP-S";
            ws.Cell($"B20").Value = home.OperatorsSHours;
            ws.Cell($"C20").Value = collab.OperatorsSHours;
            ws.Cell($"D20").FormulaA1 = "=B20+C20";

            ws.Cell($"A21").Value = "OF-S";
            ws.Cell($"B21").Value = home.OfficialsSHours;
            ws.Cell($"C21").Value = collab.OfficialsSHours;
            ws.Cell($"D21").FormulaA1 = "=B21+C21";

            ws.Cell($"A22").Value = "PE-S";
            ws.Cell($"B22").Value = home.PawnsSHours;
            ws.Cell($"C22").Value = collab.PawnsSHours;
            ws.Cell($"D22").FormulaA1 = "=B22+C22";

            //De la Población
            ws.Cell($"A23").Value = "OP-P";
            ws.Cell($"B23").Value = home.OperatorsPHours;
            ws.Cell($"C23").Value = collab.OperatorsPHours;
            ws.Cell($"D23").FormulaA1 = "=B23+C23";

            ws.Cell($"A24").Value = "OF-P";
            ws.Cell($"B24").Value = home.OfficialsPHours;
            ws.Cell($"C24").Value = collab.OfficialsPHours;
            ws.Cell($"D24").FormulaA1 = "=B24+C24";

            ws.Cell($"A25").Value = "PE-P";
            ws.Cell($"B25").Value = home.PawnsPHours;
            ws.Cell($"C25").Value = collab.PawnsPHours;
            ws.Cell($"D25").FormulaA1 = "=B25+C25";

            //Total
            ws.Cell($"B26").FormulaA1 = "=B17+B18+B19+B20+B21+B22+B23+B24+B25";
            ws.Cell($"C26").FormulaA1 = "=C17+C18+C19+C20+C21+C22+C23+C24+C25";
            ws.Cell($"D26").FormulaA1 = "=D17+D18+D19+D20+D21+D22+D23+D24+D25";

            ws.Range($"B17:D26").Style.NumberFormat.Format = "#,##0.00";

            ws.Range($"B26:D26").Style.Fill.SetBackgroundColor(backgroundColor);
            ws.Range($"B26:D26").Style.Font.SetFontColor(fontColor);
            var rngHours = ws.Range($"A15:D26");
            #endregion

            #region Nuevos
            int row = 1;
            int item = 1;
            ws.Cell($"H{row}").Value = "Ingresos:";
            ws.Cell($"H{row}").Style.Fill.SetBackgroundColor(backgroundColor);
            ws.Cell($"H{row}").Style.Font.SetFontColor(fontColor);
            row++;
            ws.Cell($"H{row}").Value = "Item";
            ws.Cell($"I{row}").Value = "Apellidos y Nombres";
            ws.Cell($"J{row}").Value = "Categoría";
            ws.Cell($"K{row}").Value = "Nro.Doc.";
            ws.Cell($"L{row}").Value = "Ingreso";
            ws.Range($"H{row}:L{row}").Style.Fill.SetBackgroundColor(backgroundColor);
            ws.Range($"H{row}:L{row}").Style.Font.SetFontColor(fontColor);
            var rngNew = ws.Range($"H{row}:L{row + newWorkers.Count}");
            row++;
            foreach (var worker in newWorkers)
            {
                ws.Cell($"H{row}").Value = item;
                ws.Cell($"I{row}").Value = worker.WorkerFullName;
                ws.Cell($"J{row}").Value = worker.WorkerCategoryStr;
                ws.Cell($"K{row}").Style.NumberFormat.Format = "@";
                ws.Cell($"K{row}").Value = worker.WorkerDocument;
                ws.Cell($"L{row}").Style.NumberFormat.Format = "dd/mm/yyyy";
                ws.Cell($"L{row}").Value = worker.WorkerEntryDate;
                row++;
                item++;
            }
            #endregion

            #region Cesados
            row++;
            item = 1;
            ws.Cell($"H{row}").Value = "Cesados:";
            ws.Cell($"H{row}").Style.Fill.SetBackgroundColor(backgroundColor);
            ws.Cell($"H{row}").Style.Font.SetFontColor(fontColor);
            row++;
            ws.Cell($"H{row}").Value = "Item";
            ws.Cell($"I{row}").Value = "Apellidos y Nombres";
            ws.Cell($"J{row}").Value = "Categoría";
            ws.Cell($"K{row}").Value = "Nro.Doc.";
            ws.Cell($"L{row}").Value = "Ingreso";
            ws.Cell($"M{row}").Value = "Cese";
            ws.Range($"H{row}:M{row}").Style.Fill.SetBackgroundColor(backgroundColor);
            ws.Range($"H{row}:M{row}").Style.Font.SetFontColor(fontColor);
            var rngCease = ws.Range($"H{row}:M{row + ceaseWorkers.Count}");
            row++;
            foreach (var worker in ceaseWorkers)
            {
                ws.Cell($"H{row}").Value = item;
                ws.Cell($"I{row}").Value = worker.WorkerFullName;
                ws.Cell($"J{row}").Value = worker.WorkerCategoryStr;
                ws.Cell($"K{row}").Style.NumberFormat.Format = "@";
                ws.Cell($"K{row}").Value = worker.WorkerDocument;
                ws.Range($"L{row}:M{row}").Style.NumberFormat.Format = "dd/mm/yyyy";
                ws.Cell($"L{row}").Value = worker.WorkerEntryDate;
                ws.Cell($"M{row}").Value = worker.WorkerCeaseDate;
                row++;
                item++;
            }
            #endregion

            ws.Columns().AdjustToContents();
            ws.Rows().AdjustToContents();

            //IXLCell reference = rngWorkers.FirstCell();
            //reference.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            //reference.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            //var newStyle = reference.Style;
            //rngWorkers.Style.Border = newStyle.Border;
            //rngHours.Style.Border = newStyle.Border;
            //rngNew.Style.Border = newStyle.Border;
            //rngCease.Style.Border = newStyle.Border;
        }
        
        private void AddInfo(UspWorkersWeeklyTask worker, IXLWorksheet ws, int row)
        {
            ws.Cell($"A{row}").Value = worker.WorkerDocument;
            ws.Cell($"B{row}").Value = worker.WorkerFullName;
            ws.Cell($"C{row}").Value = worker.WorkerCategoryStr;
            ws.Cell($"D{row}").Value = worker.WorkerPosition;
            ws.Cell($"E{row}").Value = worker.WorkerOriginStr;
            ws.Cell($"F{row}").Value = worker.WorkerEntryDate;
            ws.Cell($"G{row}").Value = worker.WorkFrontHead;
            ws.Cell($"H{row}").Value = worker.SewerGroupCode;

            //Lunes
            ws.Cell($"I{row}").Value = worker.MondayHN;
            SetStyleToHnCell(ws, "I", row, worker.MondayHN);
            ws.Cell($"J{row}").Value = worker.MondayH60;
            SetStyleToHeCell(ws, "J", row, worker.MondayH60);
            ws.Cell($"K{row}").Value = worker.MondayH100;
            SetStyleToHeCell(ws, "K", row, worker.MondayH100);
            ws.Cell($"L{row}").Value = worker.MondayPhase;
            //Martes
            ws.Cell($"M{row}").Value = worker.TuesdayHN;
            SetStyleToHnCell(ws, "M", row, worker.TuesdayHN);
            ws.Cell($"N{row}").Value = worker.TuesdayH60;
            SetStyleToHeCell(ws, "N", row, worker.TuesdayH60);
            ws.Cell($"O{row}").Value = worker.TuesdayH100;
            SetStyleToHeCell(ws, "O", row, worker.TuesdayH100);
            ws.Cell($"P{row}").Value = worker.TuesdayPhase;
            //Miercoles
            ws.Cell($"Q{row}").Value = worker.WednesdayHN;
            SetStyleToHnCell(ws, "Q", row, worker.WednesdayHN);
            ws.Cell($"R{row}").Value = worker.WednesdayH60;
            SetStyleToHeCell(ws, "R", row, worker.WednesdayH60);
            ws.Cell($"S{row}").Value = worker.WednesdayH100;
            SetStyleToHeCell(ws, "S", row, worker.WednesdayH100);
            ws.Cell($"T{row}").Value = worker.WednesdayPhase;
            //Jueves
            ws.Cell($"U{row}").Value = worker.ThursdayHN;
            SetStyleToHnCell(ws, "U", row, worker.ThursdayHN);
            ws.Cell($"V{row}").Value = worker.ThursdayH60;
            SetStyleToHeCell(ws, "V", row, worker.ThursdayH60);
            ws.Cell($"W{row}").Value = worker.ThursdayH100;
            SetStyleToHeCell(ws, "W", row, worker.ThursdayH100);
            ws.Cell($"X{row}").Value = worker.ThursdayPhase;
            //Viernes
            ws.Cell($"Y{row}").Value = worker.FridayHN;
            SetStyleToHnCell(ws, "Y", row, worker.FridayHN);
            ws.Cell($"Z{row}").Value = worker.FridayH60;
            SetStyleToHeCell(ws, "Z", row, worker.FridayH60);
            ws.Cell($"AA{row}").Value = worker.FridayH100;
            SetStyleToHeCell(ws, "AA", row, worker.FridayH100);
            ws.Cell($"AB{row}").Value = worker.FridayPhase;
            //Sabado
            ws.Cell($"AC{row}").Value = worker.SaturdayHN;
            SetStyleToHnCell(ws, "AC", row, worker.SaturdayHN);
            ws.Cell($"AD{row}").Value = worker.SaturdayH60;
            SetStyleToHeCell(ws, "AD", row, worker.SaturdayH60);
            ws.Cell($"AE{row}").Value = worker.SaturdayH100;
            SetStyleToHeCell(ws, "AE", row, worker.SaturdayH100);
            ws.Cell($"AF{row}").Value = worker.SaturdayPhase;
            //Domingo
            ws.Cell($"AG{row}").Value = worker.SundayH100;
            SetStyleToHeCell(ws, "AG", row, worker.SundayH100);
            ws.Cell($"AH{row}").Value = worker.SundayPhase;
            //Totales
            ws.Cell($"AI{row}").Value = worker.HN + worker.HD + worker.MR + worker.PL + worker.HML;
            ws.Cell($"AJ{row}").Value = worker.H60;
            ws.Cell($"AK{row}").Value = worker.H100;
            ws.Cell($"AL{row}").FormulaA1 = $"=AI{row}+AJ{row}+AK{row}";
            //Cesados
            ws.Cell($"AM{row}").Value = worker.WorkerCeaseDate;
            //Detalle
            ws.Cell($"AN{row}").Value = worker.HN;
            ws.Cell($"AO{row}").Value = worker.HD;
            ws.Cell($"AP{row}").Value = worker.MR;
            ws.Cell($"AQ{row}").Value = worker.PL;
            ws.Cell($"AR{row}").Value = worker.H60;
            ws.Cell($"AS{row}").Value = worker.H100;
            ws.Cell($"AT{row}").Value = worker.LS;
            ws.Cell($"AU{row}").Value = worker.ML;
            ws.Cell($"AV{row}").Value = worker.NA;
            ws.Cell($"AW{row}").Value = worker.UPL;
        }

        private void SetBackColor(IXLWorksheet ws, int row)
        {
            var hnColor = XLColor.FromArgb(255, 239, 156);
            var backgroundColor = XLColor.FromArgb(235, 241, 222);
            var totalBgColor = XLColor.FromArgb(242, 220, 219);

            ws.Range($"I3:I${row}").Style.Fill.SetBackgroundColor(hnColor);
            ws.Range($"M3:M${row}").Style.Fill.SetBackgroundColor(hnColor);
            ws.Range($"Q3:Q${row}").Style.Fill.SetBackgroundColor(hnColor);
            ws.Range($"U3:U${row}").Style.Fill.SetBackgroundColor(hnColor);
            ws.Range($"Y3:Y${row}").Style.Fill.SetBackgroundColor(hnColor);
            ws.Range($"AC3:AC${row}").Style.Fill.SetBackgroundColor(hnColor);

            ws.Range($"J3:L${row}").Style.Fill.SetBackgroundColor(backgroundColor);
            ws.Range($"R3:T${row}").Style.Fill.SetBackgroundColor(backgroundColor);
            ws.Range($"Z3:AB${row}").Style.Fill.SetBackgroundColor(backgroundColor);
            ws.Range($"AG3:AH${row}").Style.Fill.SetBackgroundColor(backgroundColor);

            ws.Range($"AI3:AL${row}").Style.Fill.SetBackgroundColor(totalBgColor);
            ws.Range($"AN3:AW${row}").Style.Fill.SetBackgroundColor(totalBgColor);

            ws.Column("A").Style.NumberFormat.Format = "@";
            ws.Column($"F").Style.NumberFormat.Format = "dd/mm/yyyy";
            ws.Column($"AM").Style.NumberFormat.Format = "dd/mm/yyyy";
        }

        private void SetStyleToHeCell(IXLWorksheet ws, string col, int row, decimal? he)
        {
            if (he.HasValue)
                if (he.Value > 0)
                {
                    var heColor = XLColor.FromArgb(255, 142, 67);
                    ws.Cell($"{col}{row}").Style.Fill.SetBackgroundColor(heColor);
                }
        }
        private void SetStyleToHeCell(IXLWorksheet ws, string col, int row, string hours)
        {            
            if (!string.IsNullOrEmpty(hours))
            {
                if (hours == "SM")
                {
                    var smColor = XLColor.FromArgb(218, 238, 243);
                    ws.Cell($"{col}{row}").Style.Fill.SetBackgroundColor(smColor);
                } else
                {
                    var heColor = XLColor.FromArgb(255, 142, 67);
                    ws.Cell($"{col}{row}").Style.Fill.SetBackgroundColor(heColor);
                }
            }
                
        }

        private void SetStyleToHnCell(IXLWorksheet ws, string col, int row, string hours)
        {
            switch (hours)
            {
                case "PG":
                    var pgColor = XLColor.FromArgb(230, 184, 183);
                    ws.Cell($"{col}{row}").Style.Fill.SetBackgroundColor(pgColor); 
                    break;
                case "DM":
                    var dmColor = XLColor.Yellow;
                    ws.Cell($"{col}{row}").Style.Fill.SetBackgroundColor(dmColor); 
                    break;
                case "SM":
                    var smColor = XLColor.FromArgb(218, 238, 243);
                    ws.Cell($"{col}{row}").Style.Fill.SetBackgroundColor(smColor); 
                    break;
                case "F":
                    var fColor = XLColor.Red; 
                    ws.Cell($"{col}{row}").Style.Fill.SetBackgroundColor(fColor); 
                    break;
                case "PS":
                    var psColor = XLColor.FromArgb(146, 208, 80); 
                    ws.Cell($"{col}{row}").Style.Fill.SetBackgroundColor(psColor); 
                    break;
                case "0.0": 
                    ws.Cell($"{col}{row}").Style.Fill.SetBackgroundColor(XLColor.Transparent); 
                    break;
                default: break;
            }
        }

        private void AddHeaders(IXLWorksheet ws, DateTime weekStart)
        {
            var backgroundColor = XLColor.FromArgb(22, 54, 92);
            var fontColor = XLColor.White;

            ws.Cell($"A1").Value = "Nro.Doc.";
            ws.Column(1).Style.NumberFormat.Format = "@";
            ws.Range("A1:A2").Merge(false);

            ws.Cell($"B1").Value = "Trabajador";
            ws.Range("B1:B2").Merge(false);

            ws.Cell($"C1").Value = "Categoría";
            ws.Range("C1:C2").Merge(false);

            ws.Cell($"D1").Value = "Cargo";
            ws.Range("D1:D2").Merge(false);

            ws.Cell($"E1").Value = "Procedencia";
            ws.Range("E1:E2").Merge(false);

            ws.Cell($"F1").Value = "Fecha Ingreso";
            ws.Range("F1:F2").Merge(false);

            ws.Cell($"G1").Value = "Jefe de Frente";
            ws.Range("G1:G2").Merge(false);

            ws.Cell($"H1").Value = "Cuadrilla";
            ws.Range("H1:H2").Merge(false);

            ws.Cell($"I1").Value = $"Lunes {weekStart.Day:00}";
            ws.Range("I1:L1").Merge(false);
            ws.Cell($"I2").Value = "HN";
            ws.Cell($"J2").Value = "H60%";
            ws.Cell($"K2").Value = "H100%";
            ws.Cell($"L2").Value = "Fase";
            ws.Column(12).Style.NumberFormat.Format = "@";

            ws.Cell($"M1").Value = $"Martes {weekStart.AddDays(1).Day:00}";
            ws.Range("M1:P1").Merge(false);
            ws.Cell($"M2").Value = "HN";
            ws.Cell($"N2").Value = "H60%";
            ws.Cell($"O2").Value = "H100%";
            ws.Cell($"P2").Value = "Fase";
            ws.Column(16).Style.NumberFormat.Format = "@";

            ws.Cell($"Q1").Value = $"Miercoles {weekStart.AddDays(2).Day:00}";
            ws.Range("Q1:T1").Merge(false);
            ws.Cell($"Q2").Value = "HN";
            ws.Cell($"R2").Value = "H60%";
            ws.Cell($"S2").Value = "H100%";
            ws.Cell($"T2").Value = "Fase";
            ws.Column(20).Style.NumberFormat.Format = "@";

            ws.Cell($"U1").Value = $"Jueves {weekStart.AddDays(3).Day:00}";
            ws.Range("U1:X1").Merge(false);
            ws.Cell($"U2").Value = "HN";
            ws.Cell($"V2").Value = "H60%";
            ws.Cell($"W2").Value = "H100%";
            ws.Cell($"X2").Value = "Fase";
            ws.Column(24).Style.NumberFormat.Format = "@";

            ws.Cell($"Y1").Value = $"Viernes {weekStart.AddDays(4).Day:00}";
            ws.Range("Y1:AB1").Merge(false);
            ws.Cell($"Y2").Value = "HN";
            ws.Cell($"Z2").Value = "H60%";
            ws.Cell($"AA2").Value = "H100%";
            ws.Cell($"AB2").Value = "Fase";
            ws.Column(28).Style.NumberFormat.Format = "@";

            ws.Cell($"AC1").Value = $"Sabado {weekStart.AddDays(5).Day:00}";
            ws.Range("AC1:AF1").Merge(false);
            ws.Cell($"AC2").Value = "HN";
            ws.Cell($"AD2").Value = "H60%";
            ws.Cell($"AE2").Value = "H100%";
            ws.Cell($"AF2").Value = "Fase";
            ws.Column(32).Style.NumberFormat.Format = "@";

            ws.Cell($"AG1").Value = $"Domingo {weekStart.AddDays(6).Day:00}";
            ws.Range("AG1:AH1").Merge(false);
            ws.Cell($"AG2").Value = "H100%";
            ws.Cell($"AH2").Value = "Fase";
            ws.Column(34).Style.NumberFormat.Format = "@";

            ws.Cell($"AI1").Value = "Totales";
            ws.Range($"AI1:AL1").Merge(false);
            ws.Cell($"AI2").Value = "HN";
            ws.Cell($"AJ2").Value = "H60%";
            ws.Cell($"AK2").Value = "H100%";
            ws.Cell($"AL2").Value = "Gral.";

            ws.Cell($"AM1").Value = "Cesados";
            ws.Range($"AM1:AM2").Merge(false);

            ws.Cell($"AN1").Value = "Detalle Semana";
            ws.Range($"AN1:AW1").Merge(false);
            ws.Cell($"AN2").Value = "HN";
            ws.Cell($"AO2").Value = "FE";
            ws.Cell($"AP2").Value = "DM";
            ws.Cell($"AQ2").Value = "PG";
            ws.Cell($"AR2").Value = "H60%";
            ws.Cell($"AS2").Value = "H100%";
            ws.Cell($"AT2").Value = "S";
            ws.Cell($"AU2").Value = "SM";
            ws.Cell($"AV2").Value = "F";
            ws.Cell($"AW2").Value = "PS";

            ws.Range("A1:AW2").Style.Fill.SetBackgroundColor(backgroundColor);
            ws.Range("A1:AW2").Style.Font.SetFontColor(fontColor);
        }
        #endregion

        #region HelperMethods
        private string GetSewerGroupIdWithMostAttendance(Dictionary<string, int?> sewerGroupCount)
        {
            string sewerGroupCode = string.Empty;
            int max = 0;
            foreach (var key in sewerGroupCount.Keys)
            {
                if (max < sewerGroupCount[key].Value)
                {
                    sewerGroupCode = key;
                    max = sewerGroupCount[key].Value;
                }
            }
            return sewerGroupCode;
        }
        private void CreatePayrollHeader(Guid weekId)
        {
            var headerDb = _context.PayrollMovementHeaders.Where(x => x.ProjectCalendarWeekId == weekId).FirstOrDefault();

            if (headerDb != null)
                return;

            var week = _context.ProjectCalendarWeeks.Include(x => x.ProjectCalendar).FirstOrDefault(x => x.Id == weekId);

            var header = new PayrollMovementHeader
            {
                ProjectId = week.ProjectCalendar.ProjectId,
                ProjectCalendarWeekId = week.Id,
            };

            _context.PayrollMovementHeaders.Add(header);
            _context.SaveChanges();
        }
        private async Task SendAuthMail(ProjectCalendarWeek week, string responsibleId)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == responsibleId);
            var linkExcel = ConstantHelpers.SystemUrl.Url + "recursos-humanos/tareo-semanal/exportar-excel?weekId=" + week.Id.ToString();
            //var linkAuth = ConstantHelpers.SystemUrl.Url + "recursos-humanos/tareo-semanal/autorizacion?weekId=" + week.Id.ToString() + "&userId=" + user.Id.ToString();

            var mailMessage = new MailMessage
            {
                From = new MailAddress("sistemaivctest@gmail.com", "Sistema IVC")
            };
            mailMessage.To.Add(new MailAddress(user.Email, user.RawFullName));
            mailMessage.Subject = $"IVC - Solicitud de Aprobación Tareo Semanal - Semana {week.WeekNumber}-{week.Year}";

            mailMessage.Body =
                $"Hola {user.RawFullName},<br/>Favor se le solicita revisar y aprobar el tareo de la Semana {week.WeekNumber}-{week.Year}<br/>Puede descargar el excel del tareo semanal del siguiente <a href='{linkExcel}'>LINK</a>.<br/><br/>Si todo es conforme dirijase a su dashboard y apruebe el tareo semanal en la sección de Gestión RRHH.<br/><br/>Saludos cordiales.";
            mailMessage.IsBodyHtml = true;

            //string someUrl = linkExcel;
            //using (var webClient = new WebClient())
            //{
            //    byte[] imageBytes = webClient.DownloadData(someUrl);
            //    var stream = new MemoryStream(imageBytes);
            //    Attachment excelAttach = new Attachment(stream, $"excel_semana.xlsx");
            //    mailMessage.Attachments.Add(excelAttach);
            //}

            

            using (var client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("sistemaivctest@gmail.com", "IVC.12345");
                client.EnableSsl = true;
                await client.SendMailAsync(mailMessage);
            }
        }
        #endregion

        #region TestMethods
        [HttpPost("solicitar-autorizacion-prueba")]
        public async Task<IActionResult> CreateAuthorizatixonRequestTest(Guid weekId, Guid projectId)
        {
            var authReq = await _context.PayrollAuthorizationRequests
                .FirstOrDefaultAsync(x => x.WeekId == weekId);

            var week = await _context.ProjectCalendarWeeks
                .FirstOrDefaultAsync(x => x.Id == weekId);

            if (authReq == null)
            {
                var users = await _context.Users.ToListAsync();

                var responsibles = await _context.ProjectPayrollResponsibles
                    .FirstOrDefaultAsync(x => x.ProjectId == projectId);

                if (responsibles == null)
                    return BadRequest("No existen responsables registrados.");

                var existResponsible1 = users.Any(x => x.Id == responsibles.Responsible1Id);
                var existResponsible2 = users.Any(x => x.Id == responsibles.Responsible2Id);

                if (!existResponsible1 || !existResponsible2)
                    return BadRequest("Uno o ambos responsables registrados no existen.");

                await _context.PayrollAuthorizationRequests.AddAsync(
                    new PayrollAuthorizationRequest
                    {
                        Title = "Tareo Semanal",
                        Text = "Se solicita aprobación del tareo semanal de la semana #" + week.WeekNumber + " del año " + week.Year + ".",
                        WeekId = weekId,
                        TaskUserAuth1Id = responsibles.Responsible1Id,
                        WeeklyTaskAuth1 = true,
                        TaskUserAuth2Id = responsibles.Responsible2Id,
                        WeeklyTaskAuth2 = true,
                        PayrollUserAuthId = responsibles.Responsible3Id,
                        WeeklyPayrollAuth = false
                    });

                var header = await _context.PayrollMovementHeaders
                    .FirstOrDefaultAsync(x => x.ProjectCalendarWeekId == weekId);

                if (header == null)
                    await _context.PayrollMovementHeaders.AddAsync(
                        new PayrollMovementHeader
                        {
                            ProjectId = projectId,
                            ProjectCalendarWeekId = weekId,
                            ProcessStatus = 0
                        });

                
                await _context.SaveChangesAsync();

                return Ok("Solicitud registrada. Alertas enviadas.");
            }
            else
            {
                return Ok("Ya existe una solicitud registrada. Se enviaron recordatorios a los responsables.");
            }
        }
        #endregion

        #region ImportPayrollTest
        [HttpPost("importar-planilla")]
        public async Task<IActionResult> ImportPayroll(WorkerPayrollImportViewModel model)
        {
            var header = await _context.PayrollMovementHeaders
                .Include(x => x.ProjectCalendarWeek)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ProjectCalendarWeekId == model.WeekId);

            if (model.DeletePreviousInfo)
            {
                _context.PayrollMovementDetails
                    .RemoveRange(
                        await _context.PayrollMovementDetails
                                .Where(x => x.PayrollMovementHeaderId == header.Id)
                                .ToListAsync()
                    );
                await _context.SaveChangesAsync();
            }

            var workersDb = await _context.Workers.ToListAsync();

            var concepts = await _context.PayrollConcepts
                .AsNoTracking()
                .ToListAsync();

            var wages = await _context.WorkerCategories.ToListAsync();

            using (var mem = new MemoryStream())
            {
                await model.File.CopyToAsync(mem);
                using (var workBook = new XLWorkbook(mem))
                {
                    var workSheet = workBook.Worksheets.FirstOrDefault();
                    var counter = 7;

                    while (!workSheet.Cell($"A{counter}").IsEmpty())
                    {
                        var workerConcepts = new List<PayrollMovementDetail>();

                        var document = workSheet.Cell($"A{counter}").GetString();
                        var worker = workersDb.FirstOrDefault(x => x.Document == document);

                        var wage = GetWage(workSheet.Cell($"B{counter}").GetString(), wages);
                        var buc = GetBuc(workSheet.Cell($"B{counter}").GetString());

                        var code = "R01";
                        var concept = concepts.FirstOrDefault(x => x.Code == code);
                        var R01 = decimal.Parse(workSheet.Cell($"D{counter}").GetString());
                        workerConcepts.Add(new PayrollMovementDetail
                        {
                            PayrollConcept = concept,
                            PayrollConceptId = concept.Id,
                            PayrollMovementHeaderId = header.Id,
                            Value = R01,
                            WorkerId = worker.Id
                        });

                        code = "E01";
                        concept = concepts.FirstOrDefault(x => x.Code == code);
                        var E01 = decimal.Round(R01 * 8 / wage,2);
                        workerConcepts.Add(new PayrollMovementDetail
                        {
                            PayrollConcept = concept,
                            PayrollConceptId = concept.Id,
                            PayrollMovementHeaderId = header.Id,
                            Value = E01,
                            WorkerId = worker.Id
                        });

                        code = "E08";
                        concept = concepts.FirstOrDefault(x => x.Code == code);
                        var E08 = decimal.Round(E01 / 8,2);
                        workerConcepts.Add(new PayrollMovementDetail
                        {
                            PayrollConcept = concept,
                            PayrollConceptId = concept.Id,
                            PayrollMovementHeaderId = header.Id,
                            Value = E08,
                            WorkerId = worker.Id
                        });

                        code = "R18";
                        concept = concepts.FirstOrDefault(x => x.Code == code);
                        var R18 = decimal.Parse(workSheet.Cell($"E{counter}").GetString());
                        workerConcepts.Add(new PayrollMovementDetail
                        {
                            PayrollConcept = concept,
                            PayrollConceptId = concept.Id,
                            PayrollMovementHeaderId = header.Id,
                            Value = R18,
                            WorkerId = worker.Id
                        });

                        code = "R12";
                        concept = concepts.FirstOrDefault(x => x.Code == code);
                        var R12 = decimal.Parse(workSheet.Cell($"F{counter}").GetString());
                        workerConcepts.Add(new PayrollMovementDetail
                        {
                            PayrollConcept = concept,
                            PayrollConceptId = concept.Id,
                            PayrollMovementHeaderId = header.Id,
                            Value = R12,
                            WorkerId = worker.Id
                        });

                        code = "E10";
                        concept = concepts.FirstOrDefault(x => x.Code == code);
                        var E10 = decimal.Round(R12 / (0.10M * wage),2);
                        workerConcepts.Add(new PayrollMovementDetail
                        {
                            PayrollConcept = concept,
                            PayrollConceptId = concept.Id,
                            PayrollMovementHeaderId = header.Id,
                            Value = E10,
                            WorkerId = worker.Id
                        });

                        code = "R16";
                        concept = concepts.FirstOrDefault(x => x.Code == code);
                        var R16 = decimal.Parse(workSheet.Cell($"G{counter}").GetString());
                        workerConcepts.Add(new PayrollMovementDetail
                        {
                            PayrollConcept = concept,
                            PayrollConceptId = concept.Id,
                            PayrollMovementHeaderId = header.Id,
                            Value = R16,
                            WorkerId = worker.Id
                        });

                        code = "E13";
                        concept = concepts.FirstOrDefault(x => x.Code == code);
                        var E13 = decimal.Round(R16 / (wage * (1 + buc + 1 / 8)),2);
                        workerConcepts.Add(new PayrollMovementDetail
                        {
                            PayrollConcept = concept,
                            PayrollConceptId = concept.Id,
                            PayrollMovementHeaderId = header.Id,
                            Value = E13,
                            WorkerId = worker.Id
                        });

                        code = "R04";
                        concept = concepts.FirstOrDefault(x => x.Code == code);
                        var R04 = decimal.Parse(workSheet.Cell($"H{counter}").GetString());
                        workerConcepts.Add(new PayrollMovementDetail
                        {
                            PayrollConcept = concept,
                            PayrollConceptId = concept.Id,
                            PayrollMovementHeaderId = header.Id,
                            Value = R04,
                            WorkerId = worker.Id
                        });

                        code = "E05";
                        concept = concepts.FirstOrDefault(x => x.Code == code);
                        var E05 = decimal.Round(R04 / (wage / 8),2);
                        workerConcepts.Add(new PayrollMovementDetail
                        {
                            PayrollConcept = concept,
                            PayrollConceptId = concept.Id,
                            PayrollMovementHeaderId = header.Id,
                            Value = E05,
                            WorkerId = worker.Id
                        });

                        code = "R05";
                        concept = concepts.FirstOrDefault(x => x.Code == code);
                        var R05 = decimal.Parse(workSheet.Cell($"I{counter}").GetString());
                        workerConcepts.Add(new PayrollMovementDetail
                        {
                            PayrollConcept = concept,
                            PayrollConceptId = concept.Id,
                            PayrollMovementHeaderId = header.Id,
                            Value = R05,
                            WorkerId = worker.Id
                        });

                        code = "E03";
                        concept = concepts.FirstOrDefault(x => x.Code == code);
                        var E03 = decimal.Round(R05 * 8 / wage,2);
                        workerConcepts.Add(new PayrollMovementDetail
                        {
                            PayrollConcept = concept,
                            PayrollConceptId = concept.Id,
                            PayrollMovementHeaderId = header.Id,
                            Value = E03,
                            WorkerId = worker.Id
                        });

                        code = "R15";
                        concept = concepts.FirstOrDefault(x => x.Code == code);
                        var R15 = decimal.Parse(workSheet.Cell($"J{counter}").GetString());
                        workerConcepts.Add(new PayrollMovementDetail
                        {
                            PayrollConcept = concept,
                            PayrollConceptId = concept.Id,
                            PayrollMovementHeaderId = header.Id,
                            Value = R15,
                            WorkerId = worker.Id
                        });

                        code = "E19";
                        concept = concepts.FirstOrDefault(x => x.Code == code);
                        var E19 = decimal.Round(R15 * 8 / wage,2);
                        workerConcepts.Add(new PayrollMovementDetail
                        {
                            PayrollConcept = concept,
                            PayrollConceptId = concept.Id,
                            PayrollMovementHeaderId = header.Id,
                            Value = E19,
                            WorkerId = worker.Id
                        });

                        code = "R07";
                        concept = concepts.FirstOrDefault(x => x.Code == code);
                        var R07 = decimal.Parse(workSheet.Cell($"K{counter}").GetString());
                        workerConcepts.Add(new PayrollMovementDetail
                        {
                            PayrollConcept = concept,
                            PayrollConceptId = concept.Id,
                            PayrollMovementHeaderId = header.Id,
                            Value = R07,
                            WorkerId = worker.Id
                        });

                        code = "E06";
                        concept = concepts.FirstOrDefault(x => x.Code == code);
                        var E06 = decimal.Round(R07 / (wage / 8 * 1.60M),2);
                        workerConcepts.Add(new PayrollMovementDetail
                        {
                            PayrollConcept = concept,
                            PayrollConceptId = concept.Id,
                            PayrollMovementHeaderId = header.Id,
                            Value = E06,
                            WorkerId = worker.Id
                        });

                        code = "R08";
                        concept = concepts.FirstOrDefault(x => x.Code == code);
                        var R08 = decimal.Parse(workSheet.Cell($"L{counter}").GetString());
                        workerConcepts.Add(new PayrollMovementDetail
                        {
                            PayrollConcept = concept,
                            PayrollConceptId = concept.Id,
                            PayrollMovementHeaderId = header.Id,
                            Value = R08,
                            WorkerId = worker.Id
                        });

                        code = "E07";
                        concept = concepts.FirstOrDefault(x => x.Code == code);
                        var E07 = decimal.Round(R08 / (wage / 8 * 2),2);
                        workerConcepts.Add(new PayrollMovementDetail
                        {
                            PayrollConcept = concept,
                            PayrollConceptId = concept.Id,
                            PayrollMovementHeaderId = header.Id,
                            Value = E07,
                            WorkerId = worker.Id
                        });

                        code = "R02";
                        concept = concepts.FirstOrDefault(x => x.Code == code);
                        var R02 = decimal.Parse(workSheet.Cell($"M{counter}").GetString());
                        workerConcepts.Add(new PayrollMovementDetail
                        {
                            PayrollConcept = concept,
                            PayrollConceptId = concept.Id,
                            PayrollMovementHeaderId = header.Id,
                            Value = R02,
                            WorkerId = worker.Id
                        });

                        code = "E02";
                        concept = concepts.FirstOrDefault(x => x.Code == code);
                        var E02 = decimal.Round(R02 / (wage / 8),2);
                        workerConcepts.Add(new PayrollMovementDetail
                        {
                            PayrollConcept = concept,
                            PayrollConceptId = concept.Id,
                            PayrollMovementHeaderId = header.Id,
                            Value = E02,
                            WorkerId = worker.Id
                        });

                        code = "R14";
                        concept = concepts.FirstOrDefault(x => x.Code == code);
                        var R14 = decimal.Parse(workSheet.Cell($"N{counter}").GetString());
                        workerConcepts.Add(new PayrollMovementDetail
                        {
                            PayrollConcept = concept,
                            PayrollConceptId = concept.Id,
                            PayrollMovementHeaderId = header.Id,
                            Value = R14,
                            WorkerId = worker.Id
                        });

                        code = "R13";
                        concept = concepts.FirstOrDefault(x => x.Code == code);
                        var R13 = decimal.Parse(workSheet.Cell($"O{counter}").GetString());
                        workerConcepts.Add(new PayrollMovementDetail
                        {
                            PayrollConcept = concept,
                            PayrollConceptId = concept.Id,
                            PayrollMovementHeaderId = header.Id,
                            Value = R13,
                            WorkerId = worker.Id
                        });

                        code = "E11";
                        concept = concepts.FirstOrDefault(x => x.Code == code);
                        var E11 = decimal.Round(R13 / (wage * 0.15M),2);
                        workerConcepts.Add(new PayrollMovementDetail
                        {
                            PayrollConcept = concept,
                            PayrollConceptId = concept.Id,
                            PayrollMovementHeaderId = header.Id,
                            Value = E11,
                            WorkerId = worker.Id
                        });

                        code = "R17";
                        concept = concepts.FirstOrDefault(x => x.Code == code);
                        var R17 = decimal.Parse(workSheet.Cell($"P{counter}").GetString());
                        workerConcepts.Add(new PayrollMovementDetail
                        {
                            PayrollConcept = concept,
                            PayrollConceptId = concept.Id,
                            PayrollMovementHeaderId = header.Id,
                            Value = R17,
                            WorkerId = worker.Id
                        });

                        code = "R09";
                        concept = concepts.FirstOrDefault(x => x.Code == code);
                        var R09 = decimal.Parse(workSheet.Cell($"Q{counter}").GetString());
                        workerConcepts.Add(new PayrollMovementDetail
                        {
                            PayrollConcept = concept,
                            PayrollConceptId = concept.Id,
                            PayrollMovementHeaderId = header.Id,
                            Value = R09,
                            WorkerId = worker.Id
                        });

                        code = "E12";
                        concept = concepts.FirstOrDefault(x => x.Code == code);
                        var E12 = decimal.Round(R09 / 8,2);
                        workerConcepts.Add(new PayrollMovementDetail
                        {
                            PayrollConcept = concept,
                            PayrollConceptId = concept.Id,
                            PayrollMovementHeaderId = header.Id,
                            Value = E12,
                            WorkerId = worker.Id
                        });

                        code = "R03";
                        concept = concepts.FirstOrDefault(x => x.Code == code);
                        var R03 = decimal.Parse(workSheet.Cell($"R{counter}").GetString());
                        workerConcepts.Add(new PayrollMovementDetail
                        {
                            PayrollConcept = concept,
                            PayrollConceptId = concept.Id,
                            PayrollMovementHeaderId = header.Id,
                            Value = R03,
                            WorkerId = worker.Id
                        });

                        code = "R10";
                        concept = concepts.FirstOrDefault(x => x.Code == code);
                        var R10 = decimal.Parse(workSheet.Cell($"S{counter}").GetString());
                        workerConcepts.Add(new PayrollMovementDetail
                        {
                            PayrollConcept = concept,
                            PayrollConceptId = concept.Id,
                            PayrollMovementHeaderId = header.Id,
                            Value = R10,
                            WorkerId = worker.Id
                        });

                        code = "E09";
                        concept = concepts.FirstOrDefault(x => x.Code == code);
                        var E09 = decimal.Round(R10 / (40 * wage / 210),2);
                        workerConcepts.Add(new PayrollMovementDetail
                        {
                            PayrollConcept = concept,
                            PayrollConceptId = concept.Id,
                            PayrollMovementHeaderId = header.Id,
                            Value = E09,
                            WorkerId = worker.Id
                        });

                        code = "E17";
                        concept = concepts.FirstOrDefault(x => x.Code == code);
                        var E17 = decimal.Round(E09 * 8,2);
                        workerConcepts.Add(new PayrollMovementDetail
                        {
                            PayrollConcept = concept,
                            PayrollConceptId = concept.Id,
                            PayrollMovementHeaderId = header.Id,
                            Value = E17,
                            WorkerId = worker.Id
                        });

                        code = "R11";
                        concept = concepts.FirstOrDefault(x => x.Code == code);
                        var R11 = decimal.Parse(workSheet.Cell($"T{counter}").GetString());
                        workerConcepts.Add(new PayrollMovementDetail
                        {
                            PayrollConcept = concept,
                            PayrollConceptId = concept.Id,
                            PayrollMovementHeaderId = header.Id,
                            Value = R11,
                            WorkerId = worker.Id
                        });

                        code = "E18";
                        concept = concepts.FirstOrDefault(x => x.Code == code);
                        var E18 = E01 + E03 + E05 + E19;
                        workerConcepts.Add(new PayrollMovementDetail
                        {
                            PayrollConcept = concept,
                            PayrollConceptId = concept.Id,
                            PayrollMovementHeaderId = header.Id,
                            Value = E18,
                            WorkerId = worker.Id
                        });

                        code = "T01";
                        concept = concepts.FirstOrDefault(x => x.Code == code);
                        var T01 = decimal.Parse(workSheet.Cell($"U{counter}").GetString());
                        workerConcepts.Add(new PayrollMovementDetail
                        {
                            PayrollConcept = concept,
                            PayrollConceptId = concept.Id,
                            PayrollMovementHeaderId = header.Id,
                            Value = T01,
                            WorkerId = worker.Id
                        });

                        code = "D01";
                        concept = concepts.FirstOrDefault(x => x.Code == code);
                        var D01 = decimal.Parse(workSheet.Cell($"V{counter}").GetString());
                        workerConcepts.Add(new PayrollMovementDetail
                        {
                            PayrollConcept = concept,
                            PayrollConceptId = concept.Id,
                            PayrollMovementHeaderId = header.Id,
                            Value = D01,
                            WorkerId = worker.Id
                        });

                        code = "C03";
                        concept = concepts.FirstOrDefault(x => x.Code == code);
                        var C03 = decimal.Round(D01 / 0.13M,2);
                        workerConcepts.Add(new PayrollMovementDetail
                        {
                            PayrollConcept = concept,
                            PayrollConceptId = concept.Id,
                            PayrollMovementHeaderId = header.Id,
                            Value = C03,
                            WorkerId = worker.Id
                        });

                        code = "D02";
                        concept = concepts.FirstOrDefault(x => x.Code == code);
                        var D02 = decimal.Parse(workSheet.Cell($"W{counter}").GetString());
                        workerConcepts.Add(new PayrollMovementDetail
                        {
                            PayrollConcept = concept,
                            PayrollConceptId = concept.Id,
                            PayrollMovementHeaderId = header.Id,
                            Value = D02,
                            WorkerId = worker.Id
                        });                        

                        code = "C04";
                        concept = concepts.FirstOrDefault(x => x.Code == code);
                        var C04 = decimal.Round(D02 / 0.11M,2);
                        workerConcepts.Add(new PayrollMovementDetail
                        {
                            PayrollConcept = concept,
                            PayrollConceptId = concept.Id,
                            PayrollMovementHeaderId = header.Id,
                            Value = C04,
                            WorkerId = worker.Id
                        });

                        code = "D03";
                        concept = concepts.FirstOrDefault(x => x.Code == code);
                        var D03 = decimal.Parse(workSheet.Cell($"X{counter}").GetString());
                        workerConcepts.Add(new PayrollMovementDetail
                        {
                            PayrollConcept = concept,
                            PayrollConceptId = concept.Id,
                            PayrollMovementHeaderId = header.Id,
                            Value = D03,
                            WorkerId = worker.Id
                        });

                        code = "D05";
                        concept = concepts.FirstOrDefault(x => x.Code == code);
                        var D05 = decimal.Parse(workSheet.Cell($"Y{counter}").GetString());
                        workerConcepts.Add(new PayrollMovementDetail
                        {
                            PayrollConcept = concept,
                            PayrollConceptId = concept.Id,
                            PayrollMovementHeaderId = header.Id,
                            Value = D05,
                            WorkerId = worker.Id
                        });

                        code = "D04";
                        concept = concepts.FirstOrDefault(x => x.Code == code);
                        var D04 = decimal.Parse(workSheet.Cell($"Z{counter}").GetString());
                        workerConcepts.Add(new PayrollMovementDetail
                        {
                            PayrollConcept = concept,
                            PayrollConceptId = concept.Id,
                            PayrollMovementHeaderId = header.Id,
                            Value = D04,
                            WorkerId = worker.Id
                        });

                        code = "D09";
                        concept = concepts.FirstOrDefault(x => x.Code == code);
                        var D09 = decimal.Parse(workSheet.Cell($"AP{counter}").GetString());
                        workerConcepts.Add(new PayrollMovementDetail
                        {
                            PayrollConcept = concept,
                            PayrollConceptId = concept.Id,
                            PayrollMovementHeaderId = header.Id,
                            Value = D09,
                            WorkerId = worker.Id
                        });

                        code = "D07";
                        concept = concepts.FirstOrDefault(x => x.Code == code);
                        var D07 = decimal.Parse(workSheet.Cell($"AQ{counter}").GetString());
                        workerConcepts.Add(new PayrollMovementDetail
                        {
                            PayrollConcept = concept,
                            PayrollConceptId = concept.Id,
                            PayrollMovementHeaderId = header.Id,
                            Value = D07,
                            WorkerId = worker.Id
                        });

                        code = "D06";
                        concept = concepts.FirstOrDefault(x => x.Code == code);
                        var D06 = decimal.Parse(workSheet.Cell($"AR{counter}").GetString());
                        workerConcepts.Add(new PayrollMovementDetail
                        {
                            PayrollConcept = concept,
                            PayrollConceptId = concept.Id,
                            PayrollMovementHeaderId = header.Id,
                            Value = D06,
                            WorkerId = worker.Id
                        });

                        code = "D10";
                        concept = concepts.FirstOrDefault(x => x.Code == code);
                        var D10 = decimal.Parse(workSheet.Cell($"AS{counter}").GetString());
                        workerConcepts.Add(new PayrollMovementDetail
                        {
                            PayrollConcept = concept,
                            PayrollConceptId = concept.Id,
                            PayrollMovementHeaderId = header.Id,
                            Value = D10,
                            WorkerId = worker.Id
                        });

                        code = "T02";
                        concept = concepts.FirstOrDefault(x => x.Code == code);
                        var T02 = decimal.Parse(workSheet.Cell($"AT{counter}").GetString());
                        workerConcepts.Add(new PayrollMovementDetail
                        {
                            PayrollConcept = concept,
                            PayrollConceptId = concept.Id,
                            PayrollMovementHeaderId = header.Id,
                            Value = T02,
                            WorkerId = worker.Id
                        });

                        code = "T05";
                        concept = concepts.FirstOrDefault(x => x.Code == code);
                        var T05 = decimal.Parse(workSheet.Cell($"AU{counter}").GetString());
                        workerConcepts.Add(new PayrollMovementDetail
                        {
                            PayrollConcept = concept,
                            PayrollConceptId = concept.Id,
                            PayrollMovementHeaderId = header.Id,
                            Value = T05,
                            WorkerId = worker.Id
                        });

                        code = "A01";
                        concept = concepts.FirstOrDefault(x => x.Code == code);
                        var A01 = decimal.Parse(workSheet.Cell($"AV{counter}").GetString());
                        workerConcepts.Add(new PayrollMovementDetail
                        {
                            PayrollConcept = concept,
                            PayrollConceptId = concept.Id,
                            PayrollMovementHeaderId = header.Id,
                            Value = A01,
                            WorkerId = worker.Id
                        });

                        code = "A04";
                        concept = concepts.FirstOrDefault(x => x.Code == code);
                        var A04 = decimal.Parse(workSheet.Cell($"AW{counter}").GetString());
                        workerConcepts.Add(new PayrollMovementDetail
                        {
                            PayrollConcept = concept,
                            PayrollConceptId = concept.Id,
                            PayrollMovementHeaderId = header.Id,
                            Value = A04,
                            WorkerId = worker.Id
                        });

                        code = "A03";
                        concept = concepts.FirstOrDefault(x => x.Code == code);
                        var A03 = decimal.Parse(workSheet.Cell($"AX{counter}").GetString());
                        workerConcepts.Add(new PayrollMovementDetail
                        {
                            PayrollConcept = concept,
                            PayrollConceptId = concept.Id,
                            PayrollMovementHeaderId = header.Id,
                            Value = A03,
                            WorkerId = worker.Id
                        });

                        code = "A05";
                        concept = concepts.FirstOrDefault(x => x.Code == code);
                        var A05 = decimal.Parse(workSheet.Cell($"AY{counter}").GetString());
                        workerConcepts.Add(new PayrollMovementDetail
                        {
                            PayrollConcept = concept,
                            PayrollConceptId = concept.Id,
                            PayrollMovementHeaderId = header.Id,
                            Value = A05,
                            WorkerId = worker.Id
                        });

                        code = "A06";
                        concept = concepts.FirstOrDefault(x => x.Code == code);
                        var A06 = decimal.Parse(workSheet.Cell($"AZ{counter}").GetString());
                        workerConcepts.Add(new PayrollMovementDetail
                        {
                            PayrollConcept = concept,
                            PayrollConceptId = concept.Id,
                            PayrollMovementHeaderId = header.Id,
                            Value = A06,
                            WorkerId = worker.Id
                        });

                        code = "T03";
                        concept = concepts.FirstOrDefault(x => x.Code == code);
                        var T03 = decimal.Parse(workSheet.Cell($"BA{counter}").GetString());
                        workerConcepts.Add(new PayrollMovementDetail
                        {
                            PayrollConcept = concept,
                            PayrollConceptId = concept.Id,
                            PayrollMovementHeaderId = header.Id,
                            Value = T03,
                            WorkerId = worker.Id
                        });

                        code = "T04";
                        concept = concepts.FirstOrDefault(x => x.Code == code);
                        var T04 = T01 + T03;
                        workerConcepts.Add(new PayrollMovementDetail
                        {
                            PayrollConcept = concept,
                            PayrollConceptId = concept.Id,
                            PayrollMovementHeaderId = header.Id,
                            Value = T04,
                            WorkerId = worker.Id
                        });

                        code = "C01";
                        concept = concepts.FirstOrDefault(x => x.Code == code);
                        var C01 = R01 + R02 + R04 + R05 + R07 + R08 + R12 + R14 + R15 + R18;
                        workerConcepts.Add(new PayrollMovementDetail
                        {
                            PayrollConcept = concept,
                            PayrollConceptId = concept.Id,
                            PayrollMovementHeaderId = header.Id,
                            Value = C01,
                            WorkerId = worker.Id
                        });

                        code = "C02";
                        concept = concepts.FirstOrDefault(x => x.Code == code);
                        var C02 = R01 + R02 + R03 + R04 + R05 + R07 + R08 + R09 + R10 + R11 + R12 + R14 + R15 + R18;
                        workerConcepts.Add(new PayrollMovementDetail
                        {
                            PayrollConcept = concept,
                            PayrollConceptId = concept.Id,
                            PayrollMovementHeaderId = header.Id,
                            Value = C02,
                            WorkerId = worker.Id
                        });

                        _queuedBackgroundService.QueueBackgroundWorkItem(new WorkItem
                        {
                            Details = workerConcepts,
                            IsTheLast = false,
                            IsTheFirst = false
                        });

                        counter++;
                    }
                }
                mem.Close();
            }

            return Ok();
        }

        private decimal GetBuc(string v)
        {
            switch (v)
            {
                case "PEON":
                    return 0.30M;
                case "OFICIAL":
                    return 0.30M;
                case "OPERARIO":
                    return 0.32M;
                default:
                    return 0.0M;
            }
        }

        private decimal GetWage(string v, List<WorkerCategory> wages)
        {
            switch (v)
            {
                case "PEON":
                    return wages.First(x => x.WorkerCategoryId == ConstantHelpers.Worker.Category.PAWN).DayWage;
                case "OFICIAL":
                    return wages.First(x => x.WorkerCategoryId == ConstantHelpers.Worker.Category.OFFICIAL).DayWage; ;
                case "OPERARIO":
                    return wages.First(x => x.WorkerCategoryId == ConstantHelpers.Worker.Category.OPERATOR).DayWage; ;
                default:
                    return 0.0M;
            }
        }
        #endregion

        #region ImportWeeklyTaskExcel
        [HttpPost("importar-tareo-excel")]
        public async Task<IActionResult> ImportWeeklyTaskExcel(WorkerWeeklyTaskImportViewModel model)
        {
            var week = await _context.ProjectCalendarWeeks
                .Include(x => x.ProjectCalendar)
                .FirstOrDefaultAsync(x => x.Id == model.WeekId);

            var dailyTasks = await _context.WorkerDailyTasks
                .Include(x => x.Worker)
                .Where(x => x.Date.Date >= week.WeekStart.Date &&
                    x.Date.Date <= week.WeekEnd.Date &&
                    x.ProjectId == week.ProjectCalendar.ProjectId)
                .ToListAsync();

            var phases = await _context.ProjectPhases
                .Where(x => x.ProjectId == week.ProjectCalendar.ProjectId)
                .ToListAsync();

            using (var mem = new MemoryStream())
            {
                await model.File.CopyToAsync(mem);
                using (var workBook = new XLWorkbook(mem))
                {
                    foreach (var ws in workBook.Worksheets)
                    {
                        if (ws.Name == "Casa" || ws.Name == "Colaboradores")
                        {
                            var counter = 3;

                            while (!ws.Cell($"A{counter}").IsEmpty())
                            {
                                var document = ws.Cell($"A{counter}").GetString();
                                var tasks = dailyTasks.Where(x => x.Worker.Document == document);

                                if (tasks.Count() > 0)
                                {
                                    foreach (var task in tasks)
                                    {
                                        switch (task.Date.DayOfWeek)
                                        {
                                            case DayOfWeek.Sunday:
                                                var h100Sun = ws.Cell($"AG{counter}").GetString();
                                                var ppSun = ws.Cell($"AH{counter}").GetString();
                                                if (decimal.TryParse(h100Sun, out decimal hS100))
                                                    task.Hours100Percent = hS100;
                                                else
                                                {
                                                    switch (h100Sun)
                                                    {
                                                        case "SM":
                                                            task.HoursNormal = 0.0M;
                                                            task.HoursHoliday = 0.0M;
                                                            task.HoursMedicalRest = 0.0M;
                                                            task.HoursPaidLeave = 0.0M;
                                                            task.HoursPaternityLeave = 0.0M;
                                                            task.LaborSuspension = false;
                                                            task.MedicalLeave = true;
                                                            task.NonAttendance = false;
                                                            task.UnPaidLeave = false;
                                                            break;
                                                        default:
                                                            task.HoursNormal = 0.0M;
                                                            task.HoursHoliday = 0.0M;
                                                            task.HoursMedicalRest = 0.0M;
                                                            task.HoursPaidLeave = 0.0M;
                                                            task.HoursPaternityLeave = 0.0M;
                                                            task.LaborSuspension = false;
                                                            task.MedicalLeave = false;
                                                            task.NonAttendance = false;
                                                            task.UnPaidLeave = false;
                                                            break;
                                                    }
                                                }

                                                var ppS = phases.FirstOrDefault(x => x.Code == ppSun);
                                                if (ppS != null)
                                                    task.ProjectPhaseId = ppS.Id;
                                                else
                                                    task.ProjectPhaseId = Guid.Empty;

                                                break;
                                            case DayOfWeek dw when (dw >= DayOfWeek.Monday && dw <= DayOfWeek.Saturday):
                                                var hnStr = string.Empty;
                                                var h60Str = string.Empty;
                                                var h100Str = string.Empty;
                                                var ppStr = string.Empty;
                                                if(dw == DayOfWeek.Monday)
                                                {
                                                    hnStr = ws.Cell($"I{counter}").GetString();
                                                    h60Str = ws.Cell($"J{counter}").GetString();
                                                    h100Str = ws.Cell($"K{counter}").GetString();
                                                    ppStr = ws.Cell($"L{counter}").GetString();
                                                }                                                    
                                                else if(dw ==DayOfWeek.Tuesday)
                                                {
                                                    hnStr = ws.Cell($"M{counter}").GetString();
                                                    h60Str = ws.Cell($"N{counter}").GetString();
                                                    h100Str = ws.Cell($"O{counter}").GetString();
                                                    ppStr = ws.Cell($"P{counter}").GetString();
                                                }
                                                else if (dw == DayOfWeek.Wednesday)
                                                {
                                                    hnStr = ws.Cell($"Q{counter}").GetString();
                                                    h60Str = ws.Cell($"R{counter}").GetString();
                                                    h100Str = ws.Cell($"S{counter}").GetString();
                                                    ppStr = ws.Cell($"T{counter}").GetString();
                                                }
                                                else if (dw == DayOfWeek.Thursday)
                                                {
                                                    hnStr = ws.Cell($"U{counter}").GetString();
                                                    h60Str = ws.Cell($"V{counter}").GetString();
                                                    h100Str = ws.Cell($"W{counter}").GetString();
                                                    ppStr = ws.Cell($"X{counter}").GetString();
                                                }
                                                else if (dw == DayOfWeek.Friday)
                                                {
                                                    hnStr = ws.Cell($"Y{counter}").GetString();
                                                    h60Str = ws.Cell($"Z{counter}").GetString();
                                                    h100Str = ws.Cell($"AA{counter}").GetString();
                                                    ppStr = ws.Cell($"AB{counter}").GetString();
                                                }
                                                else //if (dw == DayOfWeek.Saturday)
                                                {
                                                    hnStr = ws.Cell($"AC{counter}").GetString();
                                                    h60Str = ws.Cell($"AD{counter}").GetString();
                                                    h100Str = ws.Cell($"AE{counter}").GetString();
                                                    ppStr = ws.Cell($"AF{counter}").GetString();
                                                }

                                                if (decimal.TryParse(hnStr, out decimal hn))
                                                {
                                                    task.HoursNormal = hn;
                                                    task.HoursHoliday = 0.0M;
                                                    task.HoursMedicalRest = 0.0M;
                                                    task.HoursPaidLeave = 0.0M;
                                                    task.HoursPaternityLeave = 0.0M;
                                                    task.LaborSuspension = false;
                                                    task.MedicalLeave = false;
                                                    task.NonAttendance = false;
                                                    task.UnPaidLeave = false;
                                                }
                                                else
                                                {
                                                    switch (hnStr)
                                                    {
                                                        case "FE":
                                                            task.HoursNormal = 0.0M;
                                                            task.HoursHoliday = 8.5M;
                                                            task.HoursMedicalRest = 0.0M;
                                                            task.HoursPaidLeave = 0.0M;
                                                            task.HoursPaternityLeave = 0.0M;
                                                            task.LaborSuspension = false;
                                                            task.MedicalLeave = false;
                                                            task.NonAttendance = false;
                                                            task.UnPaidLeave = false;
                                                            break;
                                                        case "DM":
                                                            task.HoursNormal = 0.0M;
                                                            task.HoursHoliday = 0.0M;
                                                            task.HoursMedicalRest = 8.5M;
                                                            task.HoursPaidLeave = 0.0M;
                                                            task.HoursPaternityLeave = 0.0M;
                                                            task.LaborSuspension = false;
                                                            task.MedicalLeave = false;
                                                            task.NonAttendance = false;
                                                            task.UnPaidLeave = false;
                                                            break;
                                                        case "PG":
                                                            task.HoursNormal = 0.0M;
                                                            task.HoursHoliday = 0.0M;
                                                            task.HoursMedicalRest = 0.0M;
                                                            task.HoursPaidLeave = 8.5M;
                                                            task.HoursPaternityLeave = 0.0M;
                                                            task.LaborSuspension = false;
                                                            task.MedicalLeave = false;
                                                            task.NonAttendance = false;
                                                            task.UnPaidLeave = false;
                                                            break;
                                                        case "S":
                                                            task.HoursNormal = 0.0M;
                                                            task.HoursHoliday = 0.0M;
                                                            task.HoursMedicalRest = 0.0M;
                                                            task.HoursPaidLeave = 0.0M;
                                                            task.HoursPaternityLeave = 0.0M;
                                                            task.LaborSuspension = true;
                                                            task.MedicalLeave = false;
                                                            task.NonAttendance = false;
                                                            task.UnPaidLeave = false;
                                                            break;
                                                        case "SM":
                                                            task.HoursNormal = 0.0M;
                                                            task.HoursHoliday = 0.0M;
                                                            task.HoursMedicalRest = 0.0M;
                                                            task.HoursPaidLeave = 0.0M;
                                                            task.HoursPaternityLeave = 0.0M;
                                                            task.LaborSuspension = false;
                                                            task.MedicalLeave = true;
                                                            task.NonAttendance = false;
                                                            task.UnPaidLeave = false;
                                                            break;
                                                        case "F":
                                                            task.HoursNormal = 0.0M;
                                                            task.HoursHoliday = 0.0M;
                                                            task.HoursMedicalRest = 0.0M;
                                                            task.HoursPaidLeave = 0.0M;
                                                            task.HoursPaternityLeave = 0.0M;
                                                            task.LaborSuspension = false;
                                                            task.MedicalLeave = false;
                                                            task.NonAttendance = true;
                                                            task.UnPaidLeave = false;
                                                            break;
                                                        case "PS":
                                                            task.HoursNormal = 0.0M;
                                                            task.HoursHoliday = 0.0M;
                                                            task.HoursMedicalRest = 0.0M;
                                                            task.HoursPaidLeave = 0.0M;
                                                            task.HoursPaternityLeave = 0.0M;
                                                            task.LaborSuspension = false;
                                                            task.MedicalLeave = false;
                                                            task.NonAttendance = false;
                                                            task.UnPaidLeave = true;
                                                            break;
                                                        default:
                                                            task.HoursNormal = 0.0M;
                                                            task.HoursHoliday = 0.0M;
                                                            task.HoursMedicalRest = 0.0M;
                                                            task.HoursPaidLeave = 0.0M;
                                                            task.HoursPaternityLeave = 0.0M;
                                                            task.LaborSuspension = false;
                                                            task.MedicalLeave = false;
                                                            task.NonAttendance = false;
                                                            task.UnPaidLeave = false;
                                                            break;
                                                    }
                                                }

                                                if (decimal.TryParse(h60Str, out decimal h60))
                                                    task.Hours60Percent = h60;
                                                else
                                                    task.Hours60Percent = 0.0M;

                                                if (decimal.TryParse(h100Str, out decimal h100))
                                                    task.Hours100Percent = h100;
                                                else
                                                    task.Hours100Percent = 0.0M;

                                                var pp = phases.FirstOrDefault(x => x.Code == ppStr);
                                                if (pp != null)
                                                    task.ProjectPhaseId = pp.Id;
                                                else
                                                    task.ProjectPhaseId = Guid.Empty;

                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                }
                                counter++;
                            }
                        }
                    }

                    await _context.SaveChangesAsync();
                }
                mem.Close();
            }

            return Ok();
        }
        #endregion

        #region UploadFiles
        [HttpPost("cargar-incidencias")]
        public async Task<IActionResult> UploadWeeklyFiles(WeeklyFilesUploadViewModel model)
        {
            
            if(model.DeletePreviousInfo)
            {

                //if (provider.ProviderFiles.Any())
                //{
                //    if (model.Files?.Any() ?? false)
                //    {
                //        var storage = new CloudStorageService(_storageCredentials);
                //        var toRemoveFiles = provider.ProviderFiles.Where(p => !model.Files.Any(f => f.Id == p.Id)).ToList();
                //        foreach (var file in toRemoveFiles)
                //        {
                //            if (file.FileUrl != null)
                //                await storage.TryDelete($"{ConstantHelpers.Provider.FileType.BLOBS[file.Type]}/{file.FileUrl.AbsolutePath.Split('/').Last()}",
                //                        ConstantHelpers.Storage.Containers.TECHNICAL_LIBRARY);
                //        }
                //        _context.ProviderFiles.RemoveRange(toRemoveFiles);

                //        var files = new List<ProviderFile>();
                //        foreach (var fileModel in model.Files.Where(f => !provider.ProviderFiles.Any(p => p.Id == f.Id)).ToList())
                //        {
                //            var file = new ProviderFile
                //            {
                //                Name = fileModel.Name,
                //                Type = fileModel.Type,
                //                ProviderId = provider.Id
                //            };
                //            if (fileModel.File != null)
                //            {
                //                file.FileUrl = await storage.UploadFile(fileModel.File.OpenReadStream(),
                //                ConstantHelpers.Storage.Containers.TECHNICAL_LIBRARY, System.IO.Path.GetExtension(fileModel.File.FileName),
                //                ConstantHelpers.Provider.FileType.BLOBS[file.Type], file.Name);
                //            }
                //            files.Add(file);
                //        }
                //        await _context.ProviderFiles.AddRangeAsync(files);
                //    }
                //}


            }
            return Ok();
        }
        #endregion
    }
}
