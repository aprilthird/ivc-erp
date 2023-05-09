using ClosedXML.Excel;
using EFCore.BulkExtensions;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.HumanResources;
using IVC.PE.ENTITIES.UspModels.HumanResources;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.PayrollViewModels;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.HumanResources.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.HumanResources.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.HUMAN_RESOURCES)]
    [Route("recursos-humanos/obreros/planilla")]
    public class WorkerPayrollController : BaseController
    {
        private readonly IEmailQueuedBackground _emailQueuedBackgroundService;

        public WorkerPayrollController(IvcDbContext context, IEmailQueuedBackground emailQueuedBackground)
            : base(context)
        {
            _emailQueuedBackgroundService = emailQueuedBackground;
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? weekId)
        {
            if (!weekId.HasValue)
                return Ok(new List<WorkerPayrollViewModel>());

            var status = await _context.PayrollMovementHeaders
                .Include(x => x.ProjectCalendarWeek)
                .FirstOrDefaultAsync(x => x.ProjectCalendarWeekId == weekId.Value);
            if (status != null)
                if (status.ProcessStatus != ConstantHelpers.Payroll.ProcessStatus.PROCESSED)
                    return Ok(new List<WorkerPayrollViewModel>());

            SqlParameter weekParam = new SqlParameter("@WeekId", weekId.Value);
            
            var movementDetails = await _context.Set<UspWorkerPayrollDetail>().FromSqlRaw("execute HumanResources_uspWorkerPayrollDetails @WeekId"
                , weekParam)
                .IgnoreQueryFilters()
                .ToListAsync();

            var workers = await _context.Set<UspWorker>().FromSqlRaw("execute HumanResources_uspWorkersByWeek @WeekId"
                , weekParam)
                .IgnoreQueryFilters()
                .ToListAsync();

            var payrollWorkers = movementDetails.Select(x => x.WorkerId).Distinct().ToList();

            var data = new List<WorkerPayrollViewModel>();

            foreach (var workerId in payrollWorkers)
            {
                var workerDetail = movementDetails.Where(x => x.WorkerId == workerId).ToList();
                var worker = workers.FirstOrDefault(x => x.WorkerId == workerId);
                if (worker == null)
                    continue;
                
                var workerPayroll = new WorkerPayrollViewModel
                {
                    WorkerId = workerId,
                    Worker = worker,
                    Salary = 0.00M,
                    Sunday = 0.00M,
                    SchoolAssignment = 0.00M,
                    Holidays = 0.00M,
                    MedicalRest = 0.00M,
                    PaternalLeave = 0.00M,
                    ExtraHours60 = 0.00M,
                    ExtraHours100 = 0.00M,
                    Mobility = 0.00M,
                    Gratification = 0.00M,
                    ExtraordinaryBonification = 0.00M,
                    Vacations = 0.00M,
                    Cts = 0.00M,
                    BUC = 0.00M,
                    PaidLeave = 0.00M,
                    MedicalLeave = 0.00M,
                    Onp = 0.00M,
                    AfpFund = 0.00M,
                    AfpFlowCommission = 0.00M,
                    AfpMixedCommission = 0.00M,
                    AfpDisabilityInsurance = 0.00M,
                    Conafovicer = 0.00M,
                    FifthCategoryTaxes = 0.00M,
                    JudicialRetention = 0.00M,
                    UnionFee = 0.00M,
                    EsSalud = 0.00M,
                    AfpEarlyRetirement = 0.00M,
                    SctrHealth = 0.00M,
                    SctrPension = 0.00M,
                    EsSaludMasVida = 0.00M,
                    TotalRem = 0.00M,
                    TotalDis = 0.00M,
                    TotalCon = 0.00M,
                    TotalCos = 0.00M,
                    TotalNet = 0.00M
                };

                foreach (var detail in workerDetail)
                {
                    switch (detail.ConceptCode)
                    {
                        case "R01":
                            workerPayroll.Salary = detail.ConceptValue;
                            break;
                        case "R02":
                            workerPayroll.Sunday = detail.ConceptValue;
                            break;
                        case "R03":
                            workerPayroll.SchoolAssignment = detail.ConceptValue;
                            break;
                        case "R04":
                            workerPayroll.Holidays = detail.ConceptValue;
                            break;
                        case "R05":
                            workerPayroll.MedicalRest = detail.ConceptValue;
                            break;
                        case "R06":
                            workerPayroll.PaternalLeave = detail.ConceptValue;
                            break;
                        case "R07":
                            workerPayroll.ExtraHours60 = detail.ConceptValue;
                            break;
                        case "R08":
                            workerPayroll.ExtraHours100 = detail.ConceptValue;
                            break;
                        case "R09":
                            workerPayroll.Mobility = detail.ConceptValue;
                            break;
                        case "R10":
                            workerPayroll.Gratification = detail.ConceptValue;
                            break;
                        case "R11":
                            workerPayroll.ExtraordinaryBonification = detail.ConceptValue;
                            break;
                        case "R12":
                            workerPayroll.Vacations = detail.ConceptValue;
                            break;
                        case "R13":
                            workerPayroll.Cts = detail.ConceptValue;
                            break;
                        case "R14":
                            workerPayroll.BUC = detail.ConceptValue;
                            break;
                        case "R15":
                            workerPayroll.PaidLeave = detail.ConceptValue;
                            break;
                        case "R16":
                            workerPayroll.MedicalLeave = detail.ConceptValue;
                            break;
                        case "D01":
                            workerPayroll.Onp = detail.ConceptValue;
                            break;
                        case "D02":
                            workerPayroll.AfpFund = detail.ConceptValue;
                            break;
                        case "D03":
                            workerPayroll.AfpFlowCommission = detail.ConceptValue;
                            break;
                        case "D04":
                            workerPayroll.AfpMixedCommission = detail.ConceptValue;
                            break;
                        case "D05":
                            workerPayroll.AfpDisabilityInsurance = detail.ConceptValue;
                            break;
                        case "D06":
                            workerPayroll.Conafovicer = detail.ConceptValue;
                            break;
                        case "D07":
                            workerPayroll.FifthCategoryTaxes = detail.ConceptValue;
                            break;
                        case "D08":
                            workerPayroll.JudicialRetention = detail.ConceptValue;
                            break;
                        case "D09":
                            workerPayroll.UnionFee = detail.ConceptValue;
                            break;
                        case "A01":
                            workerPayroll.EsSalud = detail.ConceptValue;
                            break;
                        case "A03":
                            workerPayroll.AfpEarlyRetirement = detail.ConceptValue;
                            break;
                        case "A04":
                            workerPayroll.SctrHealth = detail.ConceptValue;
                            break;
                        case "A05":
                            workerPayroll.SctrPension = detail.ConceptValue;
                            break;
                        case "A06":
                            workerPayroll.EsSaludMasVida = detail.ConceptValue;
                            break;
                        case "T01":
                            workerPayroll.TotalRem = detail.ConceptValue;
                            break;
                        case "T02":
                            workerPayroll.TotalDis = detail.ConceptValue;
                            break;
                        case "T03":
                            workerPayroll.TotalCon = detail.ConceptValue;
                            break;
                        case "T04":
                            workerPayroll.TotalCos = detail.ConceptValue;
                            break;
                        case "T05":
                            workerPayroll.TotalNet = detail.ConceptValue;
                            break;
                        default:
                            break;
                    }
                }

                data.Add(workerPayroll);
            }

            return Ok(data);
        }

        [HttpGet("estado/{id}")]
        public async Task<IActionResult> GetStatus(Guid? id = null)
        {
            if (!id.HasValue)
                return Ok(0);

            var header = await _context.PayrollMovementHeaders
                .Include(x => x.ProjectCalendarWeek)
                .FirstOrDefaultAsync(x => x.ProjectCalendarWeekId == id);

            if (header == null)
                return Ok(0);

            return Ok(header.ProcessStatus);
        }

        [HttpGet("autorizacion/estado/{id}")]
        public async Task<IActionResult> GetAuthStatus(Guid? id = null)
        {
            if (!id.HasValue)
                return BadRequest("No ha seleccionado una semana.");

            var auths = await _context.PayrollAuthorizationRequests
                .FirstOrDefaultAsync(x => x.WeekId == id.Value);

            if (auths == null)
                return Ok(new
                {
                    Requested = false,
                    Answered = false,
                    AuthStatus = false,
                    AuthResponsible = string.Empty
                });

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id.Equals(auths.PayrollUserAuthId));

            return Ok(new
            {
                Requested = auths.PayrollAuthRequested,
                Answered = auths.PayrollAuthAnswered,
                AuthStatus = auths.WeeklyPayrollAuth,
                AuthResponsible = user != null ? user.RawFullName : string.Empty
            });
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

            var responsible = await _context.Users.FindAsync(responsibles.Responsible3Id);

            if (responsible == null)
                return BadRequest("El responsable registrado no existe.");

            var linkExcel = ConstantHelpers.SystemUrl.Url + "recursos-humanos/obreros/planilla/planilla-excel?weekId=" + week.Id.ToString();
            var excelResult = await ToUspExcel(weekId);
            var excelAttachment = new Attachment(new MemoryStream(excelResult.Data), excelResult.FileName);
            var linkAuth = $"{ConstantHelpers.SystemUrl.Url}recursos-humanos/obreros/planilla/evaluar/{weekId}";

            if (!authReq.PayrollAuthRequested)
            {
                authReq.PayrollAuthRequested = true;
                var linkResponsible = $"{linkAuth}?userId={responsible.Id}";
                var linkApprove1 = $"{linkResponsible}&isApproved=true";
                var linkReject1 = $"{linkResponsible}&isApproved=false";

                _emailQueuedBackgroundService.QueueBackgroundEmailItem(new EmailItem
                {
                    To = new List<MailAddress> { new MailAddress(responsible.Email, responsible.RawFullName) },
                    Cc = new List<MailAddress>(),
                    Subject = $"IVC - Solicitud de Aprobación de Planilla - {week.ProjectCalendar.Project.Abbreviation} - Semana {week.WeekNumber}-{week.Year}",
                    Message = $"Hola <strong>{responsible.RawFullName}</strong>,<br/><br/>Se le solicita revisar y aprobar la Planilla de la Semana {week.WeekNumber}-{week.Year} para el proyecto {week.ProjectCalendar.Project.Abbreviation}.<br/>El excel del tareo semanal y demás archivos aparecen adjuntos al correo.<br/><br/><p style='color:green'>Si todo es conforme haga click en el siguiente enlace: <a href='{linkApprove1}'>APROBAR</a>.</p><p style='color:red'>De encontrar alguna disconformidad haga click en el siguiente enlace: <a href='{linkReject1}'>DESAPROBAR</a>.</p><br/>Saludos cordiales.",
                    Attachments = new List<Attachment> { excelAttachment }
                });

                await _context.SaveChangesAsync();
                return Ok("Solicitud registrada. Alertas enviadas.");
            }
            else
            {
                var linkResponsible = $"{linkAuth}?userId={responsible.Id}";
                var linkApprove = $"{linkResponsible}&isApproved=true";
                var linkReject = $"{linkResponsible}&isApproved=false";
                _emailQueuedBackgroundService.QueueBackgroundEmailItem(new EmailItem
                {
                    To = new List<MailAddress> { new MailAddress(responsible.Email, responsible.RawFullName) },
                    Cc = new List<MailAddress>(),
                    Subject = $"IVC - Recordatorio de Aprobación de Planilla- {week.ProjectCalendar.Project} - Semana {week.WeekNumber}-{week.Year}",
                    Message = $"Hola <strong>{responsible.RawFullName}</strong>,<br/><br/>Se le solicita revisar y aprobar la Planilla de la Semana {week.WeekNumber}-{week.Year} para el proyecto {week.ProjectCalendar.Project.Abbreviation}.<br/>El excel del tareo semanal y demás archivos aparecen adjuntos al correo.<br/><br/><p style='color:green'>Si todo es conforme haga click en el siguiente enlace: <a href='{linkApprove}'>APROBAR</a>.</p><p style='color:red'>De encontrar alguna disconformidad haga click en el siguiente enlace: <a href='{linkReject}'>DESAPROBAR</a>.</p><br/>Saludos cordiales.",
                    Attachments = new List<Attachment> { excelAttachment }
                });

                return Ok("Ya existe una solicitud registrada. Se envió un recordatorio al responsable.");
            }
        }

        [HttpGet("evaluar/{weekId}")]
        [AllowAnonymous]
        public async Task<IActionResult> AuthWeeklyPayroll(Guid weekId, bool isApproved)
        {
            var authDb = await _context.PayrollAuthorizationRequests
                .FirstOrDefaultAsync(x => x.WeekId == weekId);

            if (authDb == null) 
                return RedirectToAction("Error", "Message", new { message = "No existe petición." });

            if (!authDb.WeeklyTaskAuth1 || !authDb.WeeklyTaskAuth2) 
                return RedirectToAction("Error", "Message", new { message = "Planilla no autorizada." });

            var currentWeekHeader = await _context.PayrollMovementHeaders
                .Include(x => x.ProjectCalendarWeek)
                .FirstOrDefaultAsync(x => x.ProjectCalendarWeekId == weekId);

            var week = await _context.ProjectCalendarWeeks
                    .Include(x => x.ProjectCalendar)
                    .Include(x => x.ProjectCalendar.Project)
                    .FirstOrDefaultAsync(x => x.Id == weekId);

            if(week.IsClosed)
                return RedirectToAction("Error", "Message", new { message = "No se puede modificar la planilla debido a que ya se encuentra cerrada." });

            authDb.PayrollAuthAnswered = true;
            authDb.WeeklyPayrollAuth = isApproved;
            week.IsClosed = isApproved;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Message", new
            {
                title = $"Planilla Semanal {(isApproved ? "Aprobada" : "Rechazada")}",
                message = $"La planilla correspondiente a la semana {week.WeekNumber}-{week.Year} para el proyecto {week.ProjectCalendar.Project.Abbreviation} ha sido {(isApproved ? "aprobada" : "rechazada")}.",
                icon = isApproved ? Url.Content("~/media/file_approved.png") : Url.Content("~/media/file_rejected.png")
            });
        }


        [HttpPut("autorizacion/autorizar/{id}")]
        public async Task<IActionResult> AuthorizePayroll(Guid? id = null)
        {
            if (!id.HasValue)
                return BadRequest("No ha seleccionado una semana.");

            var auths = await _context.PayrollAuthorizationRequests
                .FirstOrDefaultAsync(x => x.WeekId == id.Value);

            if (auths.WeeklyTaskAuth1 && auths.WeeklyTaskAuth2)
            {
                var usrId = GetUserId();

                if (auths.PayrollUserAuthId.ToUpper().Equals(usrId.ToUpper()))
                {
                    auths.WeeklyPayrollAuth = true;

                    var week = await _context.ProjectCalendarWeeks
                        .FirstOrDefaultAsync(x => x.Id == id.Value);
                    week.IsClosed = true;

                    await _context.SaveChangesAsync();

                    return Ok("Autorización realizada.");
                } else
                {
                    return BadRequest("Usuario no autorizado para realizar esta acción.");
                }

            } else
            {
                return BadRequest("Planilla no autorizada.");
            }
        }

        #region Excel
        [HttpGet("planilla-excel/{id}")]
        public async Task<FileResult> DownloadPayrollExcel(Guid id)
        {
            var result = await ToUspExcel(id);
            return File(result.Data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", result.FileName);
        }

        private async Task<(byte[] Data, string FileName)> ToUspExcel(Guid id)
        {
            var weekParam = new SqlParameter("@WeekId", id);

            var movementDetails = await _context.Set<UspWorkerPayrollDetail>().FromSqlRaw("execute HumanResources_uspWorkerPayrollDetails @WeekId"
                , weekParam)
                .IgnoreQueryFilters()
                .ToListAsync();

            var workers = await _context.Set<UspWorker>().FromSqlRaw("execute HumanResources_uspWorkersByWeek @WeekId"
                , weekParam)
                .IgnoreQueryFilters()
                .ToListAsync();

            var currentWeek = _context.ProjectCalendarWeeks.First(x => x.Id == id);

            var payrollWorkers = movementDetails.Select(x => x.WorkerId).Distinct().ToList();

            var data = new List<WorkerPayrollViewModel>();

            using (XLWorkbook wb = new XLWorkbook())
            {
                string filename = "PlanillaObrerosIVC-" + currentWeek.YearWeekNumber + ".xlsx";
                var ws = wb.Worksheets.Add("Planilla");

                AddHeaders(ws, currentWeek);

                var row = 6;
                foreach (var workerId in payrollWorkers)
                {
                    var workerDetail = movementDetails.Where(x => x.WorkerId == workerId).ToList();
                    var worker = workers.FirstOrDefault(x => x.WorkerId == workerId);
                    if (worker == null)
                        continue;

                    var workerPayroll = new WorkerPayrollViewModel
                    {
                        WorkerId = workerId,
                        Worker = worker,
                        HoursNormal = 0.00M,
                        HoursSunday = 0.00M,
                        HoursMedicalRest = 0.00M,
                        HoursPaternalLeave = 0.00M,
                        HoursHoliday = 0.00M,
                        HoursExtra60 = 0.00M,
                        HoursExtra100 = 0.00M,
                        HoursPaidLeave = 0.00M,
                        DaysWorked = 0.00M,
                        DaysGratification = 0.00M,
                        DaysVacations = 0.00M,
                        DaysCts = 0.00M,
                        DaysAttended = 0.00M,
                        DaysMedicalLeave = 0.00M,
                        DaysUnattended = 0.00M,
                        DaysLaboralSuspension = 0.00M,
                        DaysNoPaidLeave = 0.00M,
                        Salary = 0.00M,
                        Sunday = 0.00M,
                        SchoolAssignment = 0.00M,
                        Holidays = 0.00M,
                        MedicalRest = 0.00M,
                        PaternalLeave = 0.00M,
                        ExtraHours60 = 0.00M,
                        ExtraHours100 = 0.00M,
                        Mobility = 0.00M,
                        Gratification = 0.00M,
                        ExtraordinaryBonification = 0.00M,
                        Vacations = 0.00M,
                        Cts = 0.00M,
                        BUC = 0.00M,
                        PaidLeave = 0.00M,
                        MedicalLeave = 0.00M,
                        CteHe = 0.00M,
                        RegularBonification = 0.00M,
                        Onp = 0.00M,
                        AfpFund = 0.00M,
                        AfpFlowCommission = 0.00M,
                        AfpMixedCommission = 0.00M,
                        AfpDisabilityInsurance = 0.00M,
                        Conafovicer = 0.00M,
                        FifthCategoryTaxes = 0.00M,
                        JudicialRetention = 0.00M,
                        UnionFee = 0.00M,
                        EsSalud = 0.00M,
                        AfpEarlyRetirement = 0.00M,
                        SctrHealth = 0.00M,
                        SctrPension = 0.00M,
                        EsSaludMasVida = 0.00M,
                        TotalRem = 0.00M,
                        TotalDis = 0.00M,
                        TotalCon = 0.00M,
                        TotalCos = 0.00M,
                        TotalNet = 0.00M,
                        TotalEsSaludAffected = 0.00M,
                        TotalFifthRentAffected = 0.00M,
                        TotalOnpAffected = 0.00M,
                        TotalAfpAffected = 0.00M,
                        TotalJudicialAffected = 0.00M
                    };

                    foreach (var detail in workerDetail)
                    {
                        switch (detail.ConceptCode)
                        {
                            case "E01":
                                workerPayroll.HoursNormal = detail.ConceptValue;
                                break;
                            case "E02":
                                workerPayroll.HoursSunday = detail.ConceptValue;
                                break;
                            case "E03":
                                workerPayroll.HoursMedicalRest = detail.ConceptValue;
                                break;
                            case "E04":
                                workerPayroll.HoursPaternalLeave = detail.ConceptValue;
                                break;
                            case "E05":
                                workerPayroll.HoursHoliday = detail.ConceptValue;
                                break;
                            case "E06":
                                workerPayroll.HoursExtra60 = detail.ConceptValue;
                                break;
                            case "E07":
                                workerPayroll.HoursExtra100 = detail.ConceptValue;
                                break;
                            case "E08":
                                workerPayroll.DaysWorked = detail.ConceptValue;
                                break;
                            case "E09":
                                workerPayroll.DaysGratification = detail.ConceptValue;
                                break;
                            case "E10":
                                workerPayroll.DaysVacations = detail.ConceptValue;
                                break;
                            case "E11":
                                workerPayroll.DaysCts = detail.ConceptValue;
                                break;
                            case "E12":
                                workerPayroll.DaysAttended = detail.ConceptValue;
                                break;
                            case "E13":
                                workerPayroll.DaysMedicalLeave = detail.ConceptValue;
                                break;
                            case "E14":
                                workerPayroll.DaysUnattended = detail.ConceptValue;
                                break;
                            case "E15":
                                workerPayroll.DaysLaboralSuspension = detail.ConceptValue;
                                break;
                            case "E16":
                                workerPayroll.DaysNoPaidLeave = detail.ConceptValue;
                                break;
                            case "E19":
                                workerPayroll.HoursPaidLeave = detail.ConceptValue;
                                break;
                            case "R01":
                                workerPayroll.Salary = detail.ConceptValue;
                                break;
                            case "R02":
                                workerPayroll.Sunday = detail.ConceptValue;
                                break;
                            case "R03":
                                workerPayroll.SchoolAssignment = detail.ConceptValue;
                                break;
                            case "R04":
                                workerPayroll.Holidays = detail.ConceptValue;
                                break;
                            case "R05":
                                workerPayroll.MedicalRest = detail.ConceptValue;
                                break;
                            case "R06":
                                workerPayroll.PaternalLeave = detail.ConceptValue;
                                break;
                            case "R07":
                                workerPayroll.ExtraHours60 = detail.ConceptValue;
                                break;
                            case "R08":
                                workerPayroll.ExtraHours100 = detail.ConceptValue;
                                break;
                            case "R09":
                                workerPayroll.Mobility = detail.ConceptValue;
                                break;
                            case "R10":
                                workerPayroll.Gratification = detail.ConceptValue;
                                break;
                            case "R11":
                                workerPayroll.ExtraordinaryBonification = detail.ConceptValue;
                                break;
                            case "R12":
                                workerPayroll.Vacations = detail.ConceptValue;
                                break;
                            case "R13":
                                workerPayroll.Cts = detail.ConceptValue;
                                break;
                            case "R14":
                                workerPayroll.BUC = detail.ConceptValue;
                                break;
                            case "R15":
                                workerPayroll.PaidLeave = detail.ConceptValue;
                                break;
                            case "R16":
                                workerPayroll.MedicalLeave = detail.ConceptValue;
                                break;
                            case "R17":
                                workerPayroll.CteHe = detail.ConceptValue;
                                break;
                            case "R18":
                                workerPayroll.RegularBonification = detail.ConceptValue;
                                break;
                            case "D01":
                                workerPayroll.Onp = detail.ConceptValue;
                                break;
                            case "D02":
                                workerPayroll.AfpFund = detail.ConceptValue;
                                break;
                            case "D03":
                                workerPayroll.AfpFlowCommission = detail.ConceptValue;
                                break;
                            case "D04":
                                workerPayroll.AfpMixedCommission = detail.ConceptValue;
                                break;
                            case "D05":
                                workerPayroll.AfpDisabilityInsurance = detail.ConceptValue;
                                break;
                            case "D06":
                                workerPayroll.Conafovicer = detail.ConceptValue;
                                break;
                            case "D07":
                                workerPayroll.FifthCategoryTaxes = detail.ConceptValue;
                                break;
                            case "D08":
                                workerPayroll.JudicialRetention = detail.ConceptValue;
                                break;
                            case "D09":
                                workerPayroll.UnionFee = detail.ConceptValue;
                                break;
                            case "A01":
                                workerPayroll.EsSalud = detail.ConceptValue;
                                break;
                            case "A03":
                                workerPayroll.AfpEarlyRetirement = detail.ConceptValue;
                                break;
                            case "A04":
                                workerPayroll.SctrHealth = detail.ConceptValue;
                                break;
                            case "A05":
                                workerPayroll.SctrPension = detail.ConceptValue;
                                break;
                            case "A06":
                                workerPayroll.EsSaludMasVida = detail.ConceptValue;
                                break;
                            case "T01":
                                workerPayroll.TotalRem = detail.ConceptValue;
                                break;
                            case "T02":
                                workerPayroll.TotalDis = detail.ConceptValue;
                                break;
                            case "T03":
                                workerPayroll.TotalCon = detail.ConceptValue;
                                break;
                            case "T04":
                                workerPayroll.TotalCos = detail.ConceptValue;
                                break;
                            case "T05":
                                workerPayroll.TotalNet = detail.ConceptValue;
                                break;
                            case "C01":
                                workerPayroll.TotalEsSaludAffected = detail.ConceptValue;
                                break;
                            case "C02":
                                workerPayroll.TotalFifthRentAffected = detail.ConceptValue;
                                break;
                            case "C03":
                                workerPayroll.TotalOnpAffected = detail.ConceptValue;
                                break;
                            case "C04":
                                workerPayroll.TotalAfpAffected = detail.ConceptValue;
                                break;
                            case "C05":
                                workerPayroll.TotalJudicialAffected = detail.ConceptValue;
                                break;
                            default:
                                break;
                        }
                    }

                    AddDetail(ws, currentWeek.WeekNumber, workerPayroll, row);
                    row++;
                }

                using var stream = new MemoryStream();
                wb.SaveAs(stream);
                stream.Position = 0;
                stream.Seek(0, SeekOrigin.Begin);
                return (stream.ToArray(), filename);
            }
        }

        private void AddDetail(IXLWorksheet ws, int weekNumber, WorkerPayrollViewModel d, int row)
        {
            ws.Cell($"B{row}").Value = d.Worker.Document;
            ws.Cell($"C{row}").Value = d.Worker.FullName;
            ws.Cell($"D{row}").Value = d.Worker.EntryDate;
            if (d.Worker.CeaseDate.HasValue)
                ws.Cell($"E{row}").Value = d.Worker.CeaseDate.Value;
            ws.Cell($"F{row}").Value = d.Worker.CategoryDesc;
            ws.Cell($"G{row}").Value = d.Worker.OriginDesc;
            ws.Cell($"H{row}").Value = d.Worker.WorkgroupDesc;
            ws.Cell($"I{row}").Value = d.Worker.WorkPositionName;
            ws.Cell($"J{row}").Value = d.DaysAttended;
            ws.Cell($"K{row}").Value = d.Worker.CategoryWage;
            ws.Cell($"L{row}").Value = weekNumber;
            ws.Cell($"M{row}").Value = d.HoursNormal;
            ws.Cell($"N{row}").Value = d.HoursExtra60;
            ws.Cell($"O{row}").Value = d.HoursExtra100;
            ws.Cell($"P{row}").Value = d.HoursMedicalRest;
            ws.Cell($"Q{row}").Value = d.HoursHoliday;
            ws.Cell($"R{row}").Value = d.HoursPaternalLeave;
            ws.Cell($"S{row}").Value = d.HoursPaidLeave;
            ws.Cell($"T{row}").Value = d.DaysMedicalLeave;
            ws.Cell($"U{row}").Value = d.DaysNoPaidLeave;
            ws.Cell($"V{row}").Value = d.DaysLaboralSuspension;
            ws.Cell($"W{row}").Value = d.DaysUnattended;
            ws.Cell($"X{row}").Value = d.DaysWorked;
            ws.Cell($"Y{row}").Value = d.DaysGratification;
            ws.Cell($"Z{row}").Value = d.DaysCts;
            ws.Cell($"AA{row}").Value = d.DaysVacations;
            ws.Cell($"AB{row}").Value = d.Salary;
            ws.Cell($"AC{row}").Value = d.Sunday;
            ws.Cell($"AD{row}").Value = d.SchoolAssignment;
            ws.Cell($"AE{row}").Value = d.Holidays;
            ws.Cell($"AF{row}").Value = d.MedicalRest;
            ws.Cell($"AG{row}").Value = d.ExtraHours60;
            ws.Cell($"AH{row}").Value = d.ExtraHours100;
            ws.Cell($"AI{row}").Value = d.Mobility;
            ws.Cell($"AJ{row}").Value = d.Gratification;
            ws.Cell($"AK{row}").Value = d.ExtraordinaryBonification;
            ws.Cell($"AL{row}").Value = d.Vacations;
            ws.Cell($"AM{row}").Value = d.Cts;
            ws.Cell($"AN{row}").Value = d.CteHe;
            ws.Cell($"AO{row}").Value = d.BUC;
            ws.Cell($"AP{row}").Value = d.PaternalLeave;
            ws.Cell($"AQ{row}").Value = d.PaidLeave;
            ws.Cell($"AR{row}").Value = d.MedicalLeave;
            ws.Cell($"AS{row}").Value = d.RegularBonification;
            ws.Cell($"AT{row}").Value = d.TotalRem;
            ws.Cell($"AU{row}").Value = d.Onp;
            ws.Cell($"AV{row}").Value = d.Worker.PensionFundCode;
            ws.Cell($"AW{row}").Value = d.AfpFund;
            ws.Cell($"AX{row}").Value = d.AfpFlowCommission;
            ws.Cell($"AY{row}").Value = d.AfpMixedCommission;
            ws.Cell($"AZ{row}").Value = d.AfpDisabilityInsurance;
            ws.Cell($"BA{row}").Value = d.Conafovicer;
            ws.Cell($"BB{row}").Value = d.FifthCategoryTaxes;
            ws.Cell($"BC{row}").Value = d.JudicialRetention;
            ws.Cell($"BD{row}").Value = d.UnionFee;
            ws.Cell($"BE{row}").Value = d.TotalDis;
            ws.Cell($"BF{row}").Value = d.TotalNet;
            ws.Cell($"BG{row}").Value = d.EsSalud;
            ws.Cell($"BH{row}").Value = d.AfpEarlyRetirement;
            ws.Cell($"BI{row}").Value = d.SctrHealth;
            ws.Cell($"BJ{row}").Value = d.SctrPension;
            ws.Cell($"BK{row}").Value = d.EsSaludMasVida;
            ws.Cell($"BL{row}").Value = d.TotalCon;
            ws.Cell($"BM{row}").Value = d.TotalCos;
        }

        private void AddHeaders(IXLWorksheet ws, ProjectCalendarWeek week)
        {
            ws.Cell($"B2").Value = "IVC CONTRATISTAS GENERALES SA";
            ws.Range($"B2:D2").Merge(false);
            ws.Cell($"B3").Value = "RUC. 20100754755";
            ws.Range($"B3:D3").Merge(false);

            ws.Cell($"C4").Value = "Semana " + week.WeekNumber + " del " + week.WeekStart.ToDateString() + " al " + week.WeekEnd.ToDateString();

            ws.Cell($"B5").Value = "Nro.Doc.";
            ws.Column(2).Style.NumberFormat.Format = "@";
            ws.Cell($"C5").Value = "Apellidos y Nombres";
            ws.Cell($"D5").Value = "F/Ingreso";
            ws.Cell($"E5").Value = "F/Cese";
            ws.Cell($"F5").Value = "Categoría";
            ws.Cell($"G5").Value = "Procedencia";
            ws.Cell($"H5").Value = "Destino";
            ws.Cell($"I5").Value = "Cargo";
            ws.Cell($"J5").Value = "Días Asistidos";
            ws.Cell($"K5").Value = "Jornal";
            ws.Cell($"L5").Value = "Semana";
            ws.Cell($"M5").Value = "Horas Normales";
            ws.Cell($"N5").Value = "Horas 60%";
            ws.Cell($"O5").Value = "Horas 100%";
            ws.Cell($"P5").Value = "Horas Desc.Med.";
            ws.Cell($"Q5").Value = "Horas Feriado";
            ws.Cell($"R5").Value = "Horas Lic. Paternidad";
            ws.Cell($"S5").Value = "Horas Lic. Con Goce";
            ws.Cell($"T5").Value = "Días Subsidio Médico";
            ws.Cell($"U5").Value = "Días Sin Goce";
            ws.Cell($"V5").Value = "Días Suspension Lab.";
            ws.Cell($"W5").Value = "Días Inasistencia";
            ws.Cell($"X5").Value = "Días Trabajados";
            ws.Cell($"Y5").Value = "Días Gratificación";
            ws.Cell($"Z5").Value = "Días CTS";
            ws.Cell($"AA5").Value = "Días Vacaciones";
            ws.Cell($"AB5").Value = "Sueldo Básico";
            ws.Cell($"AC5").Value = "Dominical";
            ws.Cell($"AD5").Value = "Asig. Escolar";
            ws.Cell($"AE5").Value = "Feriados";
            ws.Cell($"AF5").Value = "Desc. Médico";
            ws.Cell($"AG5").Value = "H.E. 60%";
            ws.Cell($"AH5").Value = "H.E. 100%";
            ws.Cell($"AI5").Value = "Movilidad";
            ws.Cell($"AJ5").Value = "Gratificación";
            ws.Cell($"AK5").Value = "Bonif.Extr.";
            ws.Cell($"AL5").Value = "Vacaciones";
            ws.Cell($"AM5").Value = "CTS";
            ws.Cell($"AN5").Value = "CTS HE";
            ws.Cell($"AO5").Value = "BUC";
            ws.Cell($"AP5").Value = "Lic. Paternidad";
            ws.Cell($"AQ5").Value = "P. Con Goce";
            ws.Cell($"AR5").Value = "Subs.Médico";
            ws.Cell($"AS5").Value = "Bonif. Regulares";
            ws.Cell($"AT5").Value = "TOTAL REM.";
            ws.Cell($"AU5").Value = "ONP";
            ws.Cell($"AV5").Value = "AFP";
            ws.Cell($"AW5").Value = "AFP Fondo";
            ws.Cell($"AX5").Value = "AFP Com-Flujo";
            ws.Cell($"AY5").Value = "AFP Com-Mixta";
            ws.Cell($"AZ5").Value = "AFP Seg-Inv.";
            ws.Cell($"BA5").Value = "Conafovicer";
            ws.Cell($"BB5").Value = "Qta. Categ.";
            ws.Cell($"BC5").Value = "Ret. Judicial";
            ws.Cell($"BD5").Value = "Cuota Sindical";
            ws.Cell($"BE5").Value = "TOTAL DESC.";
            ws.Cell($"BF5").Value = "TOTAL NETO";
            ws.Cell($"BG5").Value = "EsSalud";
            ws.Cell($"BH5").Value = "AFP Jub.Ant.";
            ws.Cell($"BI5").Value = "SCTR Salud";
            ws.Cell($"BJ5").Value = "SCTR Pensión";
            ws.Cell($"BK5").Value = "EsSalud +Vida";
            ws.Cell($"BL5").Value = "TOTAL APORTES";
            ws.Cell($"BM5").Value = "TOTAL COSTO";

            ws.Range("B5:BM5").Style.Font.Bold = true;

            ws.Range("B5:BM5").Style.Alignment.WrapText = true;
        }
        #endregion

        #region Excel Sample

        [HttpGet("excel")]
        public async Task<IActionResult> ExcelSample()
        {
            string fileName = "PlanillaConceptos.xlsx";


            using (XLWorkbook wb = new XLWorkbook())
            {
                var workSheet = wb.Worksheets.Add("CargaMasiva");

                workSheet.Cell($"B2").Value = "IVC CONTRATISTAS GENERALES SA";
                workSheet.Range($"B2:D2").Merge(false);
                workSheet.Cell($"B3").Value = "RUC. 20100754755";
                workSheet.Range($"B3:D3").Merge(false);

                workSheet.Cell($"B5").Value = "Nro.Doc.";
                workSheet.Cell($"C5").Value = "Días Asistidos";
                workSheet.Cell($"D5").Value = "Jornal";
                workSheet.Cell($"E5").Value = "Semana";
                workSheet.Cell($"F5").Value = "Horas Normales";
                workSheet.Cell($"G5").Value = "Horas 60%";
                workSheet.Cell($"H5").Value = "Horas 100%";
                workSheet.Cell($"I5").Value = "Horas Desc.Med.";
                workSheet.Cell($"J5").Value = "Horas Feriado";
                workSheet.Cell($"K5").Value = "Horas Lic. Paternidad";
                workSheet.Cell($"L5").Value = "Horas Lic. Con Goce";
                workSheet.Cell($"M5").Value = "Días Subsidio Médico";
                workSheet.Cell($"N5").Value = "Días Sin Goce";
                workSheet.Cell($"O5").Value = "Días Suspension Lab.";
                workSheet.Cell($"P5").Value = "Días Inasistencia";
                workSheet.Cell($"Q5").Value = "Días Trabajados";
                workSheet.Cell($"R5").Value = "Días Gratificación";
                workSheet.Cell($"S5").Value = "Días CTS";
                workSheet.Cell($"T5").Value = "Días Vacaciones";
                workSheet.Cell($"U5").Value = "Sueldo Básico";
                workSheet.Cell($"V5").Value = "Dominical";
                workSheet.Cell($"W5").Value = "Asig. Escolar";
                workSheet.Cell($"X5").Value = "Feriados";
                workSheet.Cell($"Y5").Value = "Desc. Médico";
                workSheet.Cell($"Z5").Value = "H.E. 60%";
                workSheet.Cell($"AA5").Value = "H.E. 100%";
                workSheet.Cell($"AB5").Value = "Movilidad";
                workSheet.Cell($"AC5").Value = "Gratificación";
                workSheet.Cell($"AD5").Value = "Bonif.Extr.";
                workSheet.Cell($"AE5").Value = "Vacaciones";
                workSheet.Cell($"AF5").Value = "CTS";
                workSheet.Cell($"AG5").Value = "CTS HE";
                workSheet.Cell($"AH5").Value = "BUC";
                workSheet.Cell($"AI5").Value = "Lic. Paternidad";
                workSheet.Cell($"AJ5").Value = "P. Con Goce";
                workSheet.Cell($"AK5").Value = "Subs.Médico";
                workSheet.Cell($"AL5").Value = "Bonif. Regulares";

                workSheet.Range("U6:AL6").Style.Fill.SetBackgroundColor(XLColor.Green);

                workSheet.Cell($"AM5").Value = "TOTAL REM.";
                workSheet.Cell($"AN5").Value = "ONP";

                workSheet.Cell($"AN6").Style.Fill.SetBackgroundColor(XLColor.Green);

                workSheet.Cell($"AO5").Value = "AFP";

                workSheet.Cell($"AP5").Value = "AFP Fondo";
                workSheet.Cell($"AQ5").Value = "AFP Com-Flujo";
                workSheet.Cell($"AR5").Value = "AFP Com-Mixta";
                workSheet.Cell($"AS5").Value = "AFP Seg-Inv.";
                workSheet.Cell($"AT5").Value = "Conafovicer";
                workSheet.Cell($"AU5").Value = "Qta. Categ.";
                workSheet.Cell($"AV5").Value = "Ret. Judicial";
                workSheet.Cell($"AW5").Value = "Cuota Sindical";

                workSheet.Range("AP6:AW6").Style.Fill.SetBackgroundColor(XLColor.Green);

                workSheet.Cell($"AX5").Value = "TOTAL DESC.";
                workSheet.Cell($"AY5").Value = "TOTAL NETO";

                workSheet.Cell($"AZ5").Value = "EsSalud";
                workSheet.Cell($"BA5").Value = "AFP Jub.Ant.";
                workSheet.Cell($"BB5").Value = "SCTR Salud";
                workSheet.Cell($"BC5").Value = "SCTR Pensión";
                workSheet.Cell($"BD5").Value = "EsSalud +Vida";

                workSheet.Range("AZ6:BD6").Style.Fill.SetBackgroundColor(XLColor.Green);

                workSheet.Cell($"BE5").Value = "TOTAL APORTES";
                workSheet.Cell($"BF5").Value = "TOTAL COSTO";



                workSheet.Range("B5:BM5").Style.Font.Bold = true;

                workSheet.Range("B5:BM5").Style.Alignment.WrapText = true;

                var concepts = await _context.PayrollConcepts.ToListAsync();
                workSheet = wb.Worksheets.Add("Conceptos");
                workSheet.Cell($"A1").Value = "Cod.";
                workSheet.Range("A1:A2").Merge();
                workSheet.Range("A1:A2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"B1").Value = "Desc.";
                workSheet.Range("B1:B2").Merge();
                workSheet.Range("B1:B2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                for (int i = 0; i < concepts.Count; i++)
                {
                    workSheet.Cell($"A${3 + i}").Value = concepts[i].Code;
                    workSheet.Cell($"B${3 + i}").Value = concepts[i].Description;
                }

                workSheet.Columns().AdjustToContents();
                workSheet.Rows().AdjustToContents();

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }
        #endregion

        [HttpPost("importar/actualizaciones")]
        public async Task<IActionResult> ImportConcepts(PayrollConceptImportViewModel model, IFormFile file)
        {
            var status = await _context.PayrollMovementHeaders
                .Include(x => x.ProjectCalendarWeek)
                .FirstOrDefaultAsync(x => x.ProjectCalendarWeekId == model.WeekId);
            if (status != null)
            {
                if (status.ProcessStatus != ConstantHelpers.Payroll.ProcessStatus.PROCESSED)
                    return BadRequest();
            }
            else
                return BadRequest();

            var details = await _context.PayrollMovementDetails
                .Include(x => x.PayrollMovementHeader)
                .Include(x => x.PayrollConcept)
                .Where(x => x.PayrollMovementHeader.ProjectCalendarWeekId == model.WeekId
                && x.PayrollMovementHeader.ProjectId == GetProjectId())
                .ToListAsync();

            var workers = await _context.Workers
                .ToListAsync();

            var data = new List<PayrollMovementDetail>();

            var concepts = await _context.PayrollConcepts.ToListAsync();

            using (var mem = new MemoryStream())
            {
                await file.CopyToAsync(mem);
                using (var workBook = new XLWorkbook(mem))
                {
                    var workSheet = workBook.Worksheets.FirstOrDefault();
                    var counter = 6;

                    while (!workSheet.Cell($"B{counter}").IsEmpty())
                    {
                        var aux = workers.FirstOrDefault(x => x.Document == workSheet.Cell($"B{counter}").GetString());

                        var detailsAux = details.Where(x => x.WorkerId == aux.Id)
                            .Select(x => x.PayrollConcept.Code)
                            .ToList();

                        if (aux == null)
                            return BadRequest("No se ha encontrado el trabajador con DNI " + workSheet.Cell($"B{counter}").GetString());

                        void AddConcept(Guid conceptId, decimal value)
                        {
                            data.Add(new PayrollMovementDetail
                            {
                                Id = Guid.NewGuid(),
                                PayrollConceptId = conceptId,
                                PayrollMovementHeaderId = details.FirstOrDefault().PayrollMovementHeaderId,
                                WorkerId = aux.Id,
                                Value = value
                            });
                        }

                        decimal totalRemuner = 0;
                        decimal totalDsctos = 0;
                        decimal totalAportes = 0;
                        decimal totalNeto = 0; // totalRemuner - totalDsctos
                        decimal totalCosto = 0; // totalRemuner + totalAportes

                        #region RS's

                        var r01Excel = Convert.ToDecimal(workSheet.Cell($"U{counter}").GetString());
                        var r01 = details.FirstOrDefault(x => x.WorkerId == aux.Id && x.PayrollConcept.Code == "R01");

                        if (r01 != null)
                        {
                            r01.Value = r01Excel;
                            totalRemuner += r01.Value;
                        }
                        else
                        {
                            if (r01Excel > 0)
                            {
                                AddConcept(concepts.FirstOrDefault(x => x.Code == "R01").Id, r01Excel);
                                totalRemuner += r01Excel;
                            }
                        }


                        var r02Excel = Convert.ToDecimal(workSheet.Cell($"V{counter}").GetString());
                        var r02 = details.FirstOrDefault(x => x.WorkerId == aux.Id && x.PayrollConcept.Code == "R02");

                        if (r02 != null)
                        {
                            r02.Value = r02Excel;
                            totalRemuner += r02.Value;
                        }
                        else
                        {
                            if (r02Excel > 0)
                            {
                                AddConcept(concepts.FirstOrDefault(x => x.Code == "R02").Id, r02Excel);
                                totalRemuner += r02Excel;
                            }
                        }

                        var r03Excel = Convert.ToDecimal(workSheet.Cell($"W{counter}").GetString());
                        var r03 = details.FirstOrDefault(x => x.WorkerId == aux.Id && x.PayrollConcept.Code == "R03");

                        if (r03 != null)
                        {
                            r03.Value = r03Excel;
                            totalRemuner += r03.Value;
                        }
                        else
                        {
                            if (r03Excel > 0)
                            {
                                AddConcept(concepts.FirstOrDefault(x => x.Code == "R03").Id, r03Excel);
                                totalRemuner += r03Excel;
                            }
                        }


                        var r04Excel = Convert.ToDecimal(workSheet.Cell($"X{counter}").GetString());
                        var r04 = details.FirstOrDefault(x => x.WorkerId == aux.Id && x.PayrollConcept.Code == "R04");

                        if (r04 != null)
                        {
                            r04.Value = r04Excel;
                            totalRemuner += r04.Value;
                        }
                        else
                        {
                            if (r04Excel > 0)
                            {
                                AddConcept(concepts.FirstOrDefault(x => x.Code == "R04").Id, r04Excel);
                                totalRemuner += r04Excel;
                            }
                        }


                        var r05Excel = Convert.ToDecimal(workSheet.Cell($"Y{counter}").GetString());
                        var r05 = details.FirstOrDefault(x => x.WorkerId == aux.Id && x.PayrollConcept.Code == "R05");

                        if (r05 != null)
                        {
                            r05.Value = r05Excel;
                            totalRemuner += r05.Value;
                        }
                        else
                        {
                            if (r05Excel > 0)
                            {
                                AddConcept(concepts.FirstOrDefault(x => x.Code == "R05").Id, r05Excel);
                                totalRemuner += r05Excel;
                            }
                        }


                        var r07Excel = Convert.ToDecimal(workSheet.Cell($"Z{counter}").GetString());
                        var r07 = details.FirstOrDefault(x => x.WorkerId == aux.Id && x.PayrollConcept.Code == "R07");

                        if (r07 != null)
                        {
                            r07.Value = r07Excel;
                            totalRemuner += r07.Value;
                        }
                        else
                        {
                            if (r07Excel > 0)
                            {
                                AddConcept(concepts.FirstOrDefault(x => x.Code == "R07").Id, r07Excel);
                                totalRemuner += r07Excel;
                            }
                        }


                        var r08Excel = Convert.ToDecimal(workSheet.Cell($"AA{counter}").GetString());
                        var r08 = details.FirstOrDefault(x => x.WorkerId == aux.Id && x.PayrollConcept.Code == "R08");

                        if (r08 != null)
                        {
                            r08.Value = r08Excel;
                            totalRemuner += r08.Value;
                        }
                        else
                        {
                            if (r08Excel > 0)
                            {
                                AddConcept(concepts.FirstOrDefault(x => x.Code == "R08").Id, r08Excel);
                                totalRemuner += r08Excel;
                            }
                        }


                        var r09Excel = Convert.ToDecimal(workSheet.Cell($"AB{counter}").GetString());
                        var r09 = details.FirstOrDefault(x => x.WorkerId == aux.Id && x.PayrollConcept.Code == "R09");

                        if (r09 != null)
                        {
                            r09.Value = r09Excel;
                            totalRemuner += r09.Value;
                        }
                        else
                        {
                            if (r09Excel > 0)
                            {
                                AddConcept(concepts.FirstOrDefault(x => x.Code == "R09").Id, r09Excel);
                                totalRemuner += r09Excel;
                            }
                        }


                        var r10Excel = Convert.ToDecimal(workSheet.Cell($"AC{counter}").GetString());
                        var r10 = details.FirstOrDefault(x => x.WorkerId == aux.Id && x.PayrollConcept.Code == "R10");

                        if (r10 != null)
                        {
                            r10.Value = r10Excel;
                            totalRemuner += r10.Value;
                        }
                        else
                        {
                            if (r10Excel > 0)
                            {
                                AddConcept(concepts.FirstOrDefault(x => x.Code == "R10").Id, r10Excel);
                                totalRemuner += r10Excel;
                            }
                        }


                        var r11Excel = Convert.ToDecimal(workSheet.Cell($"AD{counter}").GetString());
                        var r11 = details.FirstOrDefault(x => x.WorkerId == aux.Id && x.PayrollConcept.Code == "R11");

                        if (r11 != null)
                        {
                            r11.Value = r11Excel;
                            totalRemuner += r11.Value;
                        }
                        else
                        {
                            if (r11Excel > 0)
                            {
                                AddConcept(concepts.FirstOrDefault(x => x.Code == "R11").Id, r11Excel);
                                totalRemuner += r11Excel;
                            }
                        }


                        var r12Excel = Convert.ToDecimal(workSheet.Cell($"AE{counter}").GetString());
                        var r12 = details.FirstOrDefault(x => x.WorkerId == aux.Id && x.PayrollConcept.Code == "R12");

                        if (r12 != null)
                        {
                            r12.Value = r12Excel;
                            totalRemuner += r12.Value;
                        }
                        else
                        {
                            if (r12Excel > 0)
                            {
                                AddConcept(concepts.FirstOrDefault(x => x.Code == "R12").Id, r12Excel);
                                totalRemuner += r12Excel;
                            }
                        }


                        var r13Excel = Convert.ToDecimal(workSheet.Cell($"AF{counter}").GetString());
                        var r13 = details.FirstOrDefault(x => x.WorkerId == aux.Id && x.PayrollConcept.Code == "R13");

                        if (r13 != null)
                        {
                            r13.Value = r13Excel;
                            totalRemuner += r13.Value;
                        }
                        else
                        {
                            if (r13Excel > 0)
                            {
                                AddConcept(concepts.FirstOrDefault(x => x.Code == "R13").Id, r13Excel);
                                totalRemuner += r13Excel;
                            }
                        }


                        var r17Excel = Convert.ToDecimal(workSheet.Cell($"AG{counter}").GetString());
                        var r17 = details.FirstOrDefault(x => x.WorkerId == aux.Id && x.PayrollConcept.Code == "R17");

                        if (r17 != null)
                        {
                            r17.Value = r17Excel;
                            totalRemuner += r17.Value;
                        }
                        else
                        {
                            if (r17Excel > 0)
                            {
                                AddConcept(concepts.FirstOrDefault(x => x.Code == "R17").Id, r17Excel);
                                totalRemuner += r17Excel;
                            }
                        }


                        var r14Excel = Convert.ToDecimal(workSheet.Cell($"AH{counter}").GetString());
                        var r14 = details.FirstOrDefault(x => x.WorkerId == aux.Id && x.PayrollConcept.Code == "R14");

                        if (r14 != null)
                        {
                            r14.Value = r14Excel;
                            totalRemuner += r14.Value;
                        }
                        else
                        {
                            if (r14Excel > 0)
                            {
                                AddConcept(concepts.FirstOrDefault(x => x.Code == "R14").Id, r14Excel);
                                totalRemuner += r14Excel;
                            }
                        }


                        var r06Excel = Convert.ToDecimal(workSheet.Cell($"AI{counter}").GetString());
                        var r06 = details.FirstOrDefault(x => x.WorkerId == aux.Id && x.PayrollConcept.Code == "R06");

                        if (r06 != null)
                        {
                            r06.Value = r06Excel;
                            totalRemuner += r06.Value;
                        }
                        else
                        {
                            if (r06Excel > 0)
                            {
                                AddConcept(concepts.FirstOrDefault(x => x.Code == "R06").Id, r06Excel);
                                totalRemuner += r06Excel;
                            }
                        }


                        var r15Excel = Convert.ToDecimal(workSheet.Cell($"AJ{counter}").GetString());
                        var r15 = details.FirstOrDefault(x => x.WorkerId == aux.Id && x.PayrollConcept.Code == "R15");

                        if (r15 != null)
                        {
                            r15.Value = r15Excel;
                            totalRemuner += r15.Value;
                        }
                        else
                        {
                            if (r15Excel > 0)
                            {
                                AddConcept(concepts.FirstOrDefault(x => x.Code == "R15").Id, r15Excel);
                                totalRemuner += r15Excel;
                            }
                        }


                        var r16Excel = Convert.ToDecimal(workSheet.Cell($"AK{counter}").GetString());
                        var r16 = details.FirstOrDefault(x => x.WorkerId == aux.Id && x.PayrollConcept.Code == "R16");

                        if (r16 != null)
                        {
                            r16.Value = r16Excel;
                            totalRemuner += r16.Value;
                        }
                        else
                        {
                            if (r16Excel > 0)
                            {
                                AddConcept(concepts.FirstOrDefault(x => x.Code == "R16").Id, r16Excel);
                                totalRemuner += r16Excel;
                            }
                        }


                        var r18Excel = Convert.ToDecimal(workSheet.Cell($"AL{counter}").GetString());
                        var r18 = details.FirstOrDefault(x => x.WorkerId == aux.Id && x.PayrollConcept.Code == "R18");

                        if (r18 != null)
                        {
                            r18.Value = r18Excel;
                            totalRemuner += r18.Value;
                        }
                        else
                        {
                            if (r18Excel > 0)
                            {
                                AddConcept(concepts.FirstOrDefault(x => x.Code == "R18").Id, r18Excel);
                                totalRemuner += r18Excel;
                            }
                        }

                        #endregion

                        #region D's

                        var d01Excel = Convert.ToDecimal(workSheet.Cell($"AN{counter}").GetString());
                        var d01 = details.FirstOrDefault(x => x.WorkerId == aux.Id && x.PayrollConcept.Code == "D01");

                        if (d01 != null)
                        {
                            d01.Value = d01Excel;
                            totalDsctos += d01.Value;
                        }
                        else
                        {
                            if (d01Excel > 0)
                            {
                                AddConcept(concepts.FirstOrDefault(x => x.Code == "D01").Id, d01Excel);
                                totalDsctos += d01Excel;
                            }
                        }


                        var d02Excel = Convert.ToDecimal(workSheet.Cell($"AP{counter}").GetString());
                        var d02 = details.FirstOrDefault(x => x.WorkerId == aux.Id && x.PayrollConcept.Code == "D02");

                        if (d02 != null)
                        {
                            d02.Value = d02Excel;
                            totalDsctos += d02.Value;
                        }
                        else
                        {
                            if (d02Excel > 0)
                            {
                                AddConcept(concepts.FirstOrDefault(x => x.Code == "D02").Id, d02Excel);
                                totalDsctos += d02Excel;
                            }
                        }


                        var d03Excel = Convert.ToDecimal(workSheet.Cell($"AQ{counter}").GetString());
                        var d03 = details.FirstOrDefault(x => x.WorkerId == aux.Id && x.PayrollConcept.Code == "D03");

                        if (d03 != null)
                        {
                            d03.Value = d03Excel;
                            totalDsctos += d03.Value;
                        }
                        else
                        {
                            if (d03Excel > 0)
                            {
                                AddConcept(concepts.FirstOrDefault(x => x.Code == "D03").Id, d03Excel);
                                totalDsctos += d03Excel;
                            }
                        }


                        var d04Excel = Convert.ToDecimal(workSheet.Cell($"AR{counter}").GetString());
                        var d04 = details.FirstOrDefault(x => x.WorkerId == aux.Id && x.PayrollConcept.Code == "D04");

                        if (d04 != null)
                        {
                            d04.Value = d04Excel;
                            totalDsctos += d04.Value;
                        }
                        else
                        {
                            if (d04Excel > 0)
                            {
                                AddConcept(concepts.FirstOrDefault(x => x.Code == "D04").Id, d04Excel);
                                totalDsctos += d04Excel;
                            }
                        }


                        var d05Excel = Convert.ToDecimal(workSheet.Cell($"AS{counter}").GetString());
                        var d05 = details.FirstOrDefault(x => x.WorkerId == aux.Id && x.PayrollConcept.Code == "D05");

                        if (d05 != null)
                        {
                            d05.Value = d05Excel;
                            totalDsctos += d05.Value;
                        }
                        else
                        {
                            if (d05Excel > 0)
                            {
                                AddConcept(concepts.FirstOrDefault(x => x.Code == "D05").Id, d05Excel);
                                totalDsctos += d05Excel;
                            }
                        }


                        var d06Excel = Convert.ToDecimal(workSheet.Cell($"AT{counter}").GetString());
                        var d06 = details.FirstOrDefault(x => x.WorkerId == aux.Id && x.PayrollConcept.Code == "D06");

                        if (d06 != null)
                        {
                            d06.Value = d06Excel;
                            totalDsctos += d06.Value;
                        }
                        else
                        {
                            if (d06Excel > 0)
                            {
                                AddConcept(concepts.FirstOrDefault(x => x.Code == "D06").Id, d06Excel);
                                totalDsctos += d06Excel;
                            }
                        }


                        var d07Excel = Convert.ToDecimal(workSheet.Cell($"AU{counter}").GetString());
                        var d07 = details.FirstOrDefault(x => x.WorkerId == aux.Id && x.PayrollConcept.Code == "D07");

                        if (d07 != null)
                        {
                            d07.Value = d07Excel;
                            totalDsctos += d07.Value;
                        }
                        else
                        {
                            if (d07Excel > 0)
                            {
                                AddConcept(concepts.FirstOrDefault(x => x.Code == "D07").Id, d07Excel);
                                totalDsctos += d07Excel;
                            }
                        }


                        var d08Excel = Convert.ToDecimal(workSheet.Cell($"AV{counter}").GetString());
                        var d08 = details.FirstOrDefault(x => x.WorkerId == aux.Id && x.PayrollConcept.Code == "D08");

                        if (d08 != null)
                        {
                            d08.Value = d08Excel;
                            totalDsctos += d08.Value;
                        }
                        else
                        {
                            if (d08Excel > 0)
                            {
                                AddConcept(concepts.FirstOrDefault(x => x.Code == "D08").Id, d08Excel);
                                totalDsctos += d08Excel;
                            }
                        }


                        var d09Excel = Convert.ToDecimal(workSheet.Cell($"AW{counter}").GetString());
                        var d09 = details.FirstOrDefault(x => x.WorkerId == aux.Id && x.PayrollConcept.Code == "D09");

                        if (d09 != null)
                        {
                            d09.Value = d09Excel;
                            totalDsctos += d09.Value;
                        }
                        else
                        {
                            if (d09Excel > 0)
                            {
                                AddConcept(concepts.FirstOrDefault(x => x.Code == "D09").Id, d09Excel);
                                totalDsctos += d09Excel;
                            }
                        }

                        #endregion

                        #region A's

                        var a01Excel = Convert.ToDecimal(workSheet.Cell($"AZ{counter}").GetString());
                        var a01 = details.FirstOrDefault(x => x.WorkerId == aux.Id && x.PayrollConcept.Code == "A01");

                        if (a01 != null)
                        {
                            a01.Value = a01Excel;
                            totalAportes += a01.Value;
                        }
                        else
                        {
                            if (a01Excel > 0)
                            {
                                AddConcept(concepts.FirstOrDefault(x => x.Code == "A01").Id, a01Excel);
                                totalAportes += a01Excel;
                            }
                        }


                        var a03Excel = Convert.ToDecimal(workSheet.Cell($"BA{counter}").GetString());
                        var a03 = details.FirstOrDefault(x => x.WorkerId == aux.Id && x.PayrollConcept.Code == "A03");

                        if (a03 != null)
                        {
                            a03.Value = a03Excel;
                            totalAportes += a03.Value;
                        }
                        else
                        {
                            if (a03Excel > 0)
                            {
                                AddConcept(concepts.FirstOrDefault(x => x.Code == "A03").Id, a03Excel);
                                totalAportes += a03Excel;
                            }
                        }


                        var a04Excel = Convert.ToDecimal(workSheet.Cell($"BB{counter}").GetString());
                        var a04 = details.FirstOrDefault(x => x.WorkerId == aux.Id && x.PayrollConcept.Code == "A04");

                        if (a04 != null)
                        {
                            a04.Value = a04Excel;
                            totalAportes += a04.Value;
                        }
                        else
                        {
                            if (a04Excel > 0)
                            {
                                AddConcept(concepts.FirstOrDefault(x => x.Code == "A04").Id, a04Excel);
                                totalAportes += a04Excel;
                            }
                        }


                        var a05Excel = Convert.ToDecimal(workSheet.Cell($"BC{counter}").GetString());
                        var a05 = details.FirstOrDefault(x => x.WorkerId == aux.Id && x.PayrollConcept.Code == "A05");

                        if (a05 != null)
                        {
                            a05.Value = a05Excel;
                            totalAportes += a05.Value;
                        }
                        else
                        {
                            if (a05Excel > 0)
                            {
                                AddConcept(concepts.FirstOrDefault(x => x.Code == "A05").Id, a05Excel);
                                totalAportes += a05Excel;
                            }
                        }


                        var a06Excel = Convert.ToDecimal(workSheet.Cell($"BD{counter}").GetString());
                        var a06 = details.FirstOrDefault(x => x.WorkerId == aux.Id && x.PayrollConcept.Code == "A06");

                        if (a06 != null)
                        {
                            a06.Value = a06Excel;
                            totalAportes += a06.Value;
                        }
                        else
                        {
                            if (a06Excel > 0)
                            {
                                AddConcept(concepts.FirstOrDefault(x => x.Code == "A06").Id, a06Excel);
                                totalAportes += a06Excel;
                            }
                        }

                        #endregion

                        #region T's

                        var t01 = details.FirstOrDefault(x => x.WorkerId == aux.Id && x.PayrollConcept.Code == "T01");

                        if (t01 != null)
                            t01.Value = totalRemuner;
                        else
                        {
                            return BadRequest("No se ha encontrado el Total Remuneración del trabajador " + aux.Document);
                        }


                        var t02 = details.FirstOrDefault(x => x.WorkerId == aux.Id && x.PayrollConcept.Code == "T02");

                        if (t02 != null)
                            t02.Value = totalDsctos;
                        else
                        {
                            return BadRequest("No se ha encontrado el Total Descuentos del trabajador " + aux.Document);
                        }


                        var t03 = details.FirstOrDefault(x => x.WorkerId == aux.Id && x.PayrollConcept.Code == "T03");

                        if (t03 != null)
                            t03.Value = totalAportes;
                        else
                        {
                            return BadRequest("No se ha encontrado el Total Aportes del trabajador " + aux.Document);
                        }


                        var t04 = details.FirstOrDefault(x => x.WorkerId == aux.Id && x.PayrollConcept.Code == "T04");

                        if (t04 != null)
                        {
                            totalCosto = Math.Round(totalRemuner + totalAportes, 2);
                            t04.Value = totalCosto;
                        }
                        else
                        {
                            return BadRequest("No se ha encontrado el Total Costo del trabajador " + aux.Document);
                        }


                        var t05 = details.FirstOrDefault(x => x.WorkerId == aux.Id && x.PayrollConcept.Code == "T05");

                        if (t05 != null)
                        {
                            totalNeto = Math.Round(totalRemuner - totalDsctos, 2);
                            t05.Value = totalNeto;
                        }
                        else
                        {
                            return BadRequest("No se ha encontrado el Total Neto del trabajador " + aux.Document);
                        }

                        #endregion

                        ++counter;
                    }

                    //await _context.PayrollMovementDetails.AddRangeAsync(data);
                    await _context.SaveChangesAsync();

                    using (var transaction = await _context.Database.BeginTransactionAsync())
                    {
                        var bulkConfig = new BulkConfig { PreserveInsertOrder = true, SetOutputIdentity = true };
                        await _context.BulkInsertAsync(data);

                        transaction.Commit();
                    }
                }
                mem.Close();
            }
            return Ok();
        }

        #region TestAuth
        [HttpPut("autorizacion-prueba/{id}")]
        public async Task<IActionResult> UpdateRequest(Guid? id = null)
        {
            if (!id.HasValue)
                return BadRequest("No ha seleccionado una semana.");

            var auths = await _context.PayrollAuthorizationRequests
                .FirstOrDefaultAsync(x => x.WeekId == id.Value);

            var week = await _context.ProjectCalendarWeeks
                .FirstOrDefaultAsync(x => x.Id == id.Value);

            auths.WeeklyPayrollAuth = true;
            week.IsClosed = true;

            await _context.SaveChangesAsync();

            return Ok();
        }
        #endregion
    }
}
