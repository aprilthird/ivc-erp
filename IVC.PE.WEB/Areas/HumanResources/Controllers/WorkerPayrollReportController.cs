using ClosedXML.Excel;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.UspModels.HumanResources;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.PayrollViewModels;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.WorkerPayrollViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.HumanResources.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.HumanResources.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.HUMAN_RESOURCES)]
    [Route("recursos-humanos/obreros/reportes")]
    public class WorkerPayrollReportController : BaseController
    {
        public WorkerPayrollReportController(IvcDbContext context)
        : base(context)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("planilla-costos/{id}")]
        public async Task<IActionResult> GetPayrollWorkforceCostReport(Guid? id = null)
        {
            if (!id.HasValue)
                return BadRequest("Semana seleccionada no contiene información.");

            var header = _context.PayrollMovementHeaders.Include(x => x.ProjectCalendarWeek).FirstOrDefault(x => x.ProjectCalendarWeekId == id.Value);

            if (header == null)
                return BadRequest("Semana seleccionada no contiene información.");

            SqlParameter headerParam = new SqlParameter("@HeaderId", header.Id);
            SqlParameter weekStartParam = new SqlParameter("@WeekStart", header.ProjectCalendarWeek.WeekStart);
            SqlParameter weekEndParam = new SqlParameter("@WeekEnd", header.ProjectCalendarWeek.WeekEnd);

            var workforceCosts = await _context.Set<UspPayrollReportWorkforceCost>().FromSqlRaw("execute HumanResources_uspPayrollReportWorkforceCostConditional @WeekStart, @WeekEnd, @HeaderId"
                , weekStartParam, weekEndParam, headerParam)
                .IgnoreQueryFilters()
                .ToListAsync();

            var workersWorkforces = workforceCosts.Select(x => x.WorkerId).Distinct().ToList();

            var collaborators = await _context.ProjectCollaborators.Include(x => x.Provider).ToListAsync();
            var sewerGroups = await _context.SewerGroups.ToListAsync();
            var workers = await _context.Workers.ToListAsync();
            var employees = await _context.Employees.ToListAsync();

            var workforceCostList = new List<WorkerPayrollWorkforceCostViewModel>();

            foreach (var workerWorkforce in workersWorkforces)
            {
                var totalsByWorker = workforceCosts.Where(x => x.WorkerId == workerWorkforce).ToList();

                var sewerGroup = sewerGroups.FirstOrDefault(x => x.Id == totalsByWorker.FirstOrDefault().SewerGroupId);

                var collaborator = collaborators.Where(x => x.Id == sewerGroup.ProjectCollaboratorId).FirstOrDefault();

                var foreman = string.Empty;
                if (collaborator == null)
                {
                    if (sewerGroup.ForemanEmployeeId.HasValue)
                        foreman = employees.Where(x => x.Id == sewerGroup.ForemanEmployeeId.Value).FirstOrDefault().FullName;
                    else if (sewerGroup.ForemanWorkerId.HasValue)
                        foreman = workers.Where(x => x.Id == sewerGroup.ForemanWorkerId.Value).FirstOrDefault().FullName;
                }

                workforceCostList.Add(new WorkerPayrollWorkforceCostViewModel
                {
                    FileName = header.ProjectCalendarWeek.YearWeekNumber,
                    WeekStartDate = header.ProjectCalendarWeek.WeekStart.ToDateString(),
                    Document = totalsByWorker.FirstOrDefault().Document,
                    FullName = totalsByWorker.FirstOrDefault().FullName,
                    CategoryName = totalsByWorker.FirstOrDefault().CategoryName,
                    Position = totalsByWorker.FirstOrDefault().Position,
                    Phase = totalsByWorker.FirstOrDefault().ProjectPhase,
                    Collaborator = collaborator != null ? collaborator.Provider.BusinessName : "CASA",
                    Responsable = collaborator != null ? collaborator.FullName : foreman,
                    SewerGroup = sewerGroup.Code,
                    TotalIncome = (totalsByWorker.FirstOrDefault(x => x.Code == "T01") != null ? totalsByWorker.FirstOrDefault(x => x.Code == "T01").Value : 0.00M),
                    EsSalud = (totalsByWorker.FirstOrDefault(x => x.Code == "A01") != null ? totalsByWorker.FirstOrDefault(x => x.Code == "A01").Value : 0.00M),
                    EarlyRetirementSPP = (totalsByWorker.FirstOrDefault(x => x.Code == "A03") != null ? totalsByWorker.FirstOrDefault(x => x.Code == "A03").Value : 0.00M),
                    SCTRSalud = (totalsByWorker.FirstOrDefault(x => x.Code == "A04") != null ? totalsByWorker.FirstOrDefault(x => x.Code == "A04").Value : 0.00M),
                    SCTRPension = (totalsByWorker.FirstOrDefault(x => x.Code == "A05") != null ? totalsByWorker.FirstOrDefault(x => x.Code == "A05").Value : 0.00M),
                    EsSaludVida = (totalsByWorker.FirstOrDefault(x => x.Code == "A06") != null ? totalsByWorker.FirstOrDefault(x => x.Code == "A06").Value : 0.00M),
                    TotalContribution = (totalsByWorker.FirstOrDefault(x => x.Code == "T03") != null ? totalsByWorker.FirstOrDefault(x => x.Code == "T03").Value : 0.00M),
                    TotalCost = (totalsByWorker.FirstOrDefault(x => x.Code == "T04") != null ? totalsByWorker.FirstOrDefault(x => x.Code == "T04").Value : 0.00M)
                });
            }

            TempData["costosPlanilla"] = JsonConvert.SerializeObject(workforceCostList);

            return Ok();
        }

        [HttpGet("fases-cuadrillas-costos/{id}")]
        public async Task<IActionResult> GetPayrollPhasesSewerGroupsCostReport(Guid? id = null)
        {
            if (!id.HasValue)
                return BadRequest("Semana seleccionada no contiene información.");

            var header = await _context.PayrollMovementHeaders.Include(x => x.ProjectCalendarWeek).FirstOrDefaultAsync(x => x.ProjectCalendarWeekId == id.Value);

            if (header == null)
                return BadRequest("Semana seleccionada no contiene información.");

            SqlParameter headerParam = new SqlParameter("@HeaderId", header.Id);
            SqlParameter weekStartParam = new SqlParameter("@WeekStart", header.ProjectCalendarWeek.WeekStart);
            SqlParameter weekEndParam = new SqlParameter("@WeekEnd", header.ProjectCalendarWeek.WeekEnd);

            var workforceCosts = await _context.Set<UspPayrollReportWorkforceCost>().FromSqlRaw("execute HumanResources_uspPayrollReportWorkforceCostConditional @WeekStart, @WeekEnd, @HeaderId"
                , weekStartParam, weekEndParam, headerParam)
                .IgnoreQueryFilters()
                .ToListAsync();

            var workersWorkforces = workforceCosts.Select(x => x.WorkerId).Distinct().ToList();

            var collaborators = await _context.ProjectCollaborators.Include(x => x.Provider).ToListAsync();
            var sewerGroups = await _context.SewerGroups.ToListAsync();
            var workers = await _context.Workers.ToListAsync();
            var employees = await _context.Employees.ToListAsync();

            var workforceCostList = new List<WorkerPayrollPhaseSewerGroupCostViewModel>();

            foreach (var workerWorkforce in workersWorkforces)
            {
                var totalsByWorker = workforceCosts.Where(x => x.WorkerId == workerWorkforce).ToList();

                var sewerGroup = sewerGroups.FirstOrDefault(x => x.Id == totalsByWorker.FirstOrDefault().SewerGroupId);

                var collaborator = collaborators.Where(x => x.Id == sewerGroup.ProjectCollaboratorId).FirstOrDefault();

                var foreman = string.Empty;
                if (collaborator == null)
                {
                    if (sewerGroup.ForemanEmployeeId.HasValue)
                        foreman = employees.Where(x => x.Id == sewerGroup.ForemanEmployeeId.Value).FirstOrDefault().FullName;
                    else if (sewerGroup.ForemanWorkerId.HasValue)
                        foreman = workers.Where(x => x.Id == sewerGroup.ForemanWorkerId.Value).FirstOrDefault().FullName;
                }

                workforceCostList.Add(new WorkerPayrollPhaseSewerGroupCostViewModel
                {
                    FileName = header.ProjectCalendarWeek.YearWeekNumber,
                    WeekStartDate = header.ProjectCalendarWeek.WeekStart.ToDateString(),
                    PhaseCode = totalsByWorker.FirstOrDefault().ProjectPhase,
                    PhaseDescription = totalsByWorker.FirstOrDefault().ProjectPhaseDescription,
                    Collaborator = collaborator != null ? collaborator.Provider.BusinessName : "CASA",
                    Responsable = collaborator != null ? collaborator.FullName : foreman,
                    SewerGroup = sewerGroup.Code,
                    TotalCost = totalsByWorker.FirstOrDefault(x => x.Code == "T04").Value
                });
            }

            var payrollPhaseSewerGroupCost = new List<WorkerPayrollPhaseSewerGroupCostViewModel>();

            var phases = workforceCostList.Select(x => x.PhaseCode).Distinct();

            foreach (var phase in phases)
            {
                var costPhase = workforceCostList.Where(x => x.PhaseCode == phase).ToList();
                var sewergroups = costPhase.Select(x => x.SewerGroup).Distinct();

                foreach (var sewergroup in sewergroups)
                {
                    var costSg = costPhase.Where(x => x.SewerGroup == sewergroup).ToList();

                    payrollPhaseSewerGroupCost.Add(new WorkerPayrollPhaseSewerGroupCostViewModel
                    {
                        FileName = costSg.FirstOrDefault().FileName,
                        WeekStartDate = costSg.FirstOrDefault().WeekStartDate,
                        PhaseCode = costSg.FirstOrDefault().PhaseCode,
                        PhaseDescription = costSg.FirstOrDefault().PhaseDescription,
                        Collaborator = costSg.FirstOrDefault().Collaborator,
                        Responsable = costSg.FirstOrDefault().Responsable,
                        SewerGroup = costSg.FirstOrDefault().SewerGroup,
                        TotalCost = costSg.Sum(x => x.TotalCost)
                    });
                }
            }

            TempData["costosPllaFaseCuadrilla"] = JsonConvert.SerializeObject(payrollPhaseSewerGroupCost);

            return Ok("costosPllaFaseCuadrilla");
        }

        [HttpGet("fases-horas-costos/{id}")]
        public async Task<IActionResult> GetPayrollPhasesHoursAndCostsReport(Guid? id = null)
        {
            if (!id.HasValue)
                return BadRequest("Semana seleccionada no contiene información.");

            var header = await _context.PayrollMovementHeaders.Include(x => x.ProjectCalendarWeek).FirstOrDefaultAsync(x => x.ProjectCalendarWeekId == id.Value);

            if (header == null)
                return BadRequest("Semana seleccionada no contiene información.");

            SqlParameter weekParam = new SqlParameter("@WeekId", id.Value);

            var workforceHours = await _context.Set<UspPayrollReportPhaseHoursAndCostsByWeek>().FromSqlRaw("execute HumanResources_uspPayrollReportPhaseHoursAndCostsByWeek @WeekId"
                , weekParam)
                .IgnoreQueryFilters()
                .ToListAsync();

            if (workforceHours.Count == 0)
                return BadRequest("Semana seleccionada no contiene información que mostrar.");

            var week = await _context.ProjectCalendarWeeks
                .FirstOrDefaultAsync(x => x.Id == id.Value);

            var collaborators = await _context.ProjectCollaborators.Include(x => x.Provider).ToListAsync();
            var sewerGroups = await _context.SewerGroups.ToListAsync();
            var workers = await _context.Workers.ToListAsync();
            var employees = await _context.Employees.ToListAsync();

            var projectPhases = await _context.ProjectPhases
                .Where(x => x.ProjectId == header.ProjectId)
                .ToListAsync();

            var workforceHoursList = new List<PayrollReportPhaseHoursAndCostsByWeekViewModel>();

            foreach (var phase in projectPhases)
            {
                var workforceHoursByPhase = workforceHours.Where(x => x.PhaseCode == phase.Code).ToList();

                if (workforceHoursByPhase.Count > 0)
                {
                    foreach (var workforceHour in workforceHoursByPhase)
                    {
                        var sewerGroup = sewerGroups.FirstOrDefault(x => x.Id == workforceHour.SewerGroupId);

                        var collaborator = collaborators.Where(x => x.Id == sewerGroup.ProjectCollaboratorId).FirstOrDefault();

                        var foreman = string.Empty;
                        if (collaborator == null)
                        {
                            if (sewerGroup.ForemanEmployeeId.HasValue)
                                foreman = employees.Where(x => x.Id == sewerGroup.ForemanEmployeeId.Value).FirstOrDefault().FullName;
                            else if (sewerGroup.ForemanWorkerId.HasValue)
                                foreman = workers.Where(x => x.Id == sewerGroup.ForemanWorkerId.Value).FirstOrDefault().FullName;
                        }

                        workforceHoursList.Add(new PayrollReportPhaseHoursAndCostsByWeekViewModel
                        {
                            FileName = week.YearWeekNumber,
                            PhaseCode = workforceHour.PhaseCode,
                            Description = workforceHour.Description,
                            SewerGroupCode = workforceHour.SewerGroupCode,
                            Collaborator = collaborator != null ? collaborator.Provider.BusinessName : "CASA",
                            Responsable = collaborator != null ? collaborator.FullName : foreman,
                            TotalHoursOp = workforceHour.TotalHoursOp,
                            TotalHoursOf = workforceHour.TotalHoursOf,
                            TotalHoursPa = workforceHour.TotalHoursPa,
                            TotalHours = workforceHour.TotalHours,
                            TotalCostsOp = workforceHour.TotalCostsOp,
                            TotalCostsOf = workforceHour.TotalCostsOf,
                            TotalCostsPa = workforceHour.TotalCostsPa,
                            TotalCosts = workforceHour.TotalCosts
                        });
                    }
                }
                else
                {
                    workforceHoursList.Add(new PayrollReportPhaseHoursAndCostsByWeekViewModel
                    {
                        FileName = week.YearWeekNumber,
                        PhaseCode = phase.Code,
                        Description = phase.Description,
                        SewerGroupCode = string.Empty,
                        Collaborator = string.Empty,
                        Responsable = string.Empty,
                        TotalHoursOp = 0.00M,
                        TotalHoursOf = 0.00M,
                        TotalHoursPa = 0.00M,
                        TotalHours = 0.00M,
                        TotalCostsOp = 0.00M,
                        TotalCostsOf = 0.00M,
                        TotalCostsPa = 0.00M,
                        TotalCosts = 0.00M
                    });
                }
            }

            TempData["horasCostoPlanilla"] = JsonConvert.SerializeObject(workforceHoursList);

            return Ok("horasCostoPlanilla");
        }

        [HttpGet("fases-obreros/{id}")]
        public async Task<IActionResult> GetPayrollPhasesWorkers(Guid? id = null)
        {
            if (!id.HasValue)
                return BadRequest("Semana seleccionada no contiene información.");

            var header = await _context.PayrollMovementHeaders.Include(x => x.ProjectCalendarWeek).FirstOrDefaultAsync(x => x.ProjectCalendarWeekId == id.Value);

            if (header == null)
                return BadRequest("Semana seleccionada no contiene información.");

            SqlParameter weekParam = new SqlParameter("@WeekId", id.Value);

            var workforceHours = await _context.Set<UspPayrollReportPhaseWorkersByWeek>().FromSqlRaw("execute HumanResources_uspPayrollReportPhaseWorkersByWeek @WeekId"
                , weekParam)
                .IgnoreQueryFilters()
                .ToListAsync();

            if (workforceHours.Count == 0)
                return BadRequest("Semana seleccionada no contiene información que mostrar.");

            var week = await _context.ProjectCalendarWeeks
                .FirstOrDefaultAsync(x => x.Id == id.Value);

            var collaborators = await _context.ProjectCollaborators.Include(x => x.Provider).ToListAsync();
            var sewerGroups = await _context.SewerGroups.ToListAsync();
            var workers = await _context.Workers.ToListAsync();
            var employees = await _context.Employees.ToListAsync();

            var projectPhases = await _context.ProjectPhases
                .Where(x => x.ProjectId == header.ProjectId)
                .ToListAsync();

            var workforceHoursList = new List<PayrollReportPhaseWorkersByWeekViewModel>();

            foreach (var phase in projectPhases)
            {
                var workforceHoursByPhase = workforceHours.Where(x => x.PhaseCode == phase.Code).ToList();

                if (workforceHoursByPhase.Count > 0)
                {
                    foreach (var workforceHour in workforceHoursByPhase)
                    {
                        var sewerGroup = sewerGroups.FirstOrDefault(x => x.Id == workforceHour.SewerGroupId);

                        var collaborator = collaborators.Where(x => x.Id == sewerGroup.ProjectCollaboratorId).FirstOrDefault();

                        var foreman = string.Empty;
                        if (collaborator == null)
                        {
                            if (sewerGroup.ForemanEmployeeId.HasValue)
                                foreman = employees.Where(x => x.Id == sewerGroup.ForemanEmployeeId.Value).FirstOrDefault().FullName;
                            else if (sewerGroup.ForemanWorkerId.HasValue)
                                foreman = workers.Where(x => x.Id == sewerGroup.ForemanWorkerId.Value).FirstOrDefault().FullName;
                        }

                        workforceHoursList.Add(new PayrollReportPhaseWorkersByWeekViewModel
                        {
                            FileName = week.YearWeekNumber,
                            PhaseCode = workforceHour.PhaseCode,
                            Description = workforceHour.Description,
                            SewerGroupCode = workforceHour.SewerGroupCode,
                            Collaborator = collaborator != null ? collaborator.Provider.BusinessName : "CASA",
                            Responsable = collaborator != null ? collaborator.FullName : foreman,
                            TotalWorkersPa = workforceHour.TotalWorkersPa.HasValue ? Math.Round(workforceHour.TotalWorkersPa.Value,0) : 0,
                            TotalWorkersOp = workforceHour.TotalWorkersOp.HasValue ? Math.Round(workforceHour.TotalWorkersOp.Value, 0) : 0,
                            TotalWorkersOf = workforceHour.TotalWorkersOf.HasValue ? Math.Round(workforceHour.TotalWorkersOf.Value, 0) : 0
                        });
                    }
                }
                else
                {
                    workforceHoursList.Add(new PayrollReportPhaseWorkersByWeekViewModel
                    {
                        FileName = week.YearWeekNumber,
                        PhaseCode = phase.Code,
                        Description = phase.Description,
                        SewerGroupCode = string.Empty,
                        Collaborator = string.Empty,
                        Responsable = string.Empty,
                        TotalWorkersPa = 0.00M,
                        TotalWorkersOp = 0.00M,
                        TotalWorkersOf = 0.00M
                    });
                }
            }

            TempData["fasesObrerosPlanilla"] = JsonConvert.SerializeObject(workforceHoursList);

            return Ok("fasesObrerosPlanilla");
        }

        [HttpGet("tareo-horas/{id}")]
        public async Task<IActionResult> GenerateWeekTaskWorkforceHoursConditionalReport(Guid? id = null)
        {
            if (!id.HasValue)
                return BadRequest("Semana seleccionada no contiene información.");

            var week = await _context.ProjectCalendarWeeks
                .Include(x => x.ProjectCalendar)
                .FirstOrDefaultAsync(x => x.Id == id.Value);

            SqlParameter weekStartParam = new SqlParameter("@WeekStart", week.WeekStart);
            SqlParameter weekEndParam = new SqlParameter("@WeekEnd", week.WeekEnd);

            var workforceHours = await _context.Set<UspWeekTaskReportWorkforceHours>().FromSqlRaw("execute HumanResources_uspWeekTaskReportWorkforceHoursConditional @WeekStart, @WeekEnd"
                , weekStartParam, weekEndParam)
                .IgnoreQueryFilters()
                .ToListAsync();

            if (workforceHours.Count == 0)
                return BadRequest("Semana seleccionada no contiene tareo.");

            var collaborators = await _context.ProjectCollaborators.Include(x => x.Provider).ToListAsync();
            var sewerGroups = await _context.SewerGroups.ToListAsync();
            var workers = await _context.Workers.ToListAsync();
            var employees = await _context.Employees.ToListAsync();

            var projectPhases = await _context.ProjectPhases
                .Where(x => x.ProjectId == week.ProjectCalendar.ProjectId)
                .ToListAsync();

            var workforceHoursList = new List<WorkerPayrollWeeklyTaskViewModel>();

            foreach (var phase in projectPhases)
            {
                var workforceHoursByPhase = workforceHours.Where(x => x.PhaseCode == phase.Code).ToList();

                if (workforceHoursByPhase.Count > 0)
                {
                    foreach (var workforceHour in workforceHoursByPhase)
                    {
                        var sewerGroup = sewerGroups.FirstOrDefault(x => x.Id == workforceHour.SewerGroupId);

                        var collaborator = collaborators.Where(x => x.Id == sewerGroup.ProjectCollaboratorId).FirstOrDefault();

                        var foreman = string.Empty;
                        if (collaborator == null)
                        {
                            if (sewerGroup.ForemanEmployeeId.HasValue)
                                foreman = employees.Where(x => x.Id == sewerGroup.ForemanEmployeeId.Value).FirstOrDefault().FullName;
                            else if (sewerGroup.ForemanWorkerId.HasValue)
                                foreman = workers.Where(x => x.Id == sewerGroup.ForemanWorkerId.Value).FirstOrDefault().FullName;
                        }

                        workforceHoursList.Add(new WorkerPayrollWeeklyTaskViewModel
                        {
                            FileName = week.YearWeekNumber,
                            PhaseCode = workforceHour.PhaseCode,
                            Description = workforceHour.Description,
                            SewerGroupCode = workforceHour.SewerGroupCode,
                            Collaborator = collaborator != null ? collaborator.Provider.BusinessName : "CASA",
                            Responsable = collaborator != null ? collaborator.FullName : foreman,
                            TotalHoursOp = workforceHour.TotalHoursOp ?? 0.00M,
                            TotalHoursOf = workforceHour.TotalHoursOf ?? 0.00M,
                            TotalHoursPa = workforceHour.TotalHoursPa ?? 0.00M,
                            TotalHours = workforceHour.TotalHours,
                            TotalWorkersOp = workforceHour.TotalWorkersOp,
                            TotalWorkersOf = workforceHour.TotalWorkersOf,
                            TotalWorkersPa = workforceHour.TotalWorkersPa,
                            TotalWorkers = workforceHour.TotalWorkers
                        });
                    }
                }
                else
                {
                    workforceHoursList.Add(new WorkerPayrollWeeklyTaskViewModel
                    {
                        FileName = week.YearWeekNumber,
                        PhaseCode = phase.Code,
                        Description = phase.Description,
                        SewerGroupCode = string.Empty,
                        Collaborator = string.Empty,
                        Responsable = string.Empty,
                        TotalHoursOp = 0.00M,
                        TotalHoursOf = 0.00M,
                        TotalHoursPa = 0.00M,
                        TotalHours = 0.00M,
                        TotalWorkersOp = 0,
                        TotalWorkersOf = 0,
                        TotalWorkersPa = 0,
                        TotalWorkers = 0
                    });
                }
            }

            TempData["horasPlanilla"] = JsonConvert.SerializeObject(workforceHoursList);

            return Ok("horasPlanilla");
        }


        /// <summary>
        /// Reportes AFP
        /// </summary>
        [HttpGet("afp")]
        public IActionResult WorkerPension() => View();

        [HttpGet("afp/formato")]
        public FileResult GenerateFundDeclarationExcel(int year, int month)
        {
            SqlParameter projectParam = new SqlParameter("@ProjectId", GetProjectId());
            SqlParameter yearParam = new SqlParameter("@YearNum", year);
            SqlParameter monthParam = new SqlParameter("@MonthNum", month);

            var workersFunds = _context.Set<UspWorkerPayrollPension>().FromSqlRaw("execute HumanResources_uspWorkerPayrollPensions @ProjectId, @MonthNum, @YearNum"
                , projectParam, yearParam, monthParam)
                .IgnoreQueryFilters()
                .ToList();

            using (XLWorkbook wb = new XLWorkbook())
            {
                var project = _context.Projects
                    .AsNoTracking()
                    .First(x => x.Id == GetProjectId());

                var filename = "AFP-" + project.CostCenter + ".xlsx";
                var ws = wb.Worksheets.Add("Resumen");
                var itr = 1;
                ws.Column(1).Style.NumberFormat.Format = "@";
                ws.Column(4).Style.NumberFormat.Format = "@";
                foreach (var wf in workersFunds)
                {
                    ws.Cell($"A{itr}").Value = itr;
                    ws.Cell($"B{itr}").Value = wf.PensionFundUniqueIdentificationCode ?? string.Empty;
                    ws.Cell($"C{itr}").Value = 0;
                    ws.Cell($"D{itr}").Value = wf.Document;
                    ws.Cell($"E{itr}").Value = wf.PaternalSurname;
                    ws.Cell($"F{itr}").Value = wf.MaternalSurname;
                    ws.Cell($"G{itr}").Value = wf.Name;
                    ws.Cell($"H{itr}").Value = "S";
                    ws.Cell($"I{itr}").Value = wf.EntryDate.HasValue ? wf.EntryDate.Value.Month == month ? "S" : "N" : "N";
                    ws.Cell($"J{itr}").Value = wf.CeaseDate.HasValue ? wf.CeaseDate.Value.Month == month ? "S" : "N" : "N";
                    ws.Cell($"L{itr}").Value = wf.Total;
                    ws.Cell($"M{itr}").Value = 0.00;
                    ws.Cell($"N{itr}").Value = 0.00;
                    ws.Cell($"O{itr}").Value = 0.00;
                    ws.Cell($"P{itr}").Value = "C";
                    ws.Cell($"Q{itr}").Value = wf.Code;
                    itr++;
                }

                ws.Columns().AdjustToContents();

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
                }
            }
        }

        //[HttpGet("afp/reporte")]
        //public FileResult GenerateFundPensionReport(int year, int month)
        //{

        //}


        /// <summary>
        /// Reportes PLAME
        /// </summary>
        [HttpGet("plame")]
        public IActionResult WorkerPlame() => View();

        [HttpGet("plame/archivos")]
        public async Task<FileResult> GeneratePlameFiles(int year, int month)
        {
            SqlParameter projectParam = new SqlParameter("@ProjectId", GetProjectId());
            SqlParameter yearParam = new SqlParameter("@Year", year);
            SqlParameter monthParam = new SqlParameter("@Month", month);

            var remData = _context.Set<UspWorkersSunatPlame>().FromSqlRaw("execute HumanResources_uspWorkersSunatPlame @ProjectId, @Month, @Year"
                , projectParam, yearParam, monthParam)
                .IgnoreQueryFilters()
                .ToList();

            var jorData = _context.Set<UspWorkersSunatPlameJor>().FromSqlRaw("execute HumanResources_uspWorkersSunatPlameJor @ProjectId, @Month, @Year"
                , projectParam, yearParam, monthParam)
                .IgnoreQueryFilters()
                .ToList();

            var tocData = remData.Select(x => x.Document).Distinct().ToList();

            var project = await _context.Projects.AsNoTracking().FirstAsync(x => x.Id == GetProjectId());

            //Creando archivo zip
            var zipFileMemoryStream = new MemoryStream();
            using (ZipArchive archive = new ZipArchive(zipFileMemoryStream, ZipArchiveMode.Update, leaveOpen: true))
            {
                //Archivo .rem
                var rem = archive.CreateEntry($"0601{year}{month.ToString("00")}{project.RucCompany}.rem");
                using (var streamWriter = new StreamWriter(rem.Open()))
                {
                    foreach (var item in remData)
                    {
                        streamWriter.WriteLine(
                            $"{item.DocumentType}|" +
                            $"{item.Document}|" +
                            $"{item.SunatCode}|" +
                            $"{item.Value.ToString("0.00")}|" +
                            $"{item.Value.ToString("0.00")}|");
                    }
                }

                //Archivo .tra
                var jor = archive.CreateEntry($"0601{year}{month.ToString("00")}{project.RucCompany}.jor");
                using (var streamWriter = new StreamWriter(jor.Open()))
                {
                    foreach (var item in jorData)
                    {
                        streamWriter.WriteLine(
                            $"{item.DocumentType}|" +
                            $"{item.Document}|" +
                            $"{Math.Round(item.Value,0)}|0|0|0|");
                    }
                }

                //Archivo .toc
                var toc = archive.CreateEntry($"0601{year}{month.ToString("00")}{project.RucCompany}.toc");
                using (var streamWriter = new StreamWriter(toc.Open()))
                {
                    foreach (var item in tocData)
                    {
                        streamWriter.WriteLine(
                            $"01|" +
                            $"{item}|0|1||1|");
                    }
                }

                //Archivo .snl
                var snl = archive.CreateEntry($"0601{year}{month.ToString("00")}{project.RucCompany}.snl");
                using (var streamWriter = new StreamWriter(snl.Open()))
                {
                    streamWriter.WriteLine($"");
                    //foreach (var item in query)
                    //{
                    //    streamWriter.WriteLine($"0{item.DocumentType}|" +
                    //        $"{item.Document}||" +
                    //        $"{item.RucCompany}|" +
                    //        $"{item.EstablishmentCode}|");
                    //}
                }
            }

            zipFileMemoryStream.Seek(0, SeekOrigin.Begin);
            return File(zipFileMemoryStream.ToArray(), "application/zip", $"plame{year}{month.ToString("00")}{project.RucCompany}.zip");
        }



        [HttpGet("descargar-excel")]
        public FileResult DownloadFile(string excelName)
        {
            if (TempData[excelName] == null)
            {
                RedirectToAction("Empty");
            }

            DataTable dt = new DataTable();
            string fileName = string.Empty;
            switch (excelName)
            {
                case "costosPlanilla":
                    var payrollCost = JsonConvert.DeserializeObject<List<WorkerPayrollWorkforceCostViewModel>>(TempData[excelName].ToString());
                    fileName = "CostosPlanilla-" + payrollCost.FirstOrDefault().FileName + ".xlsx";
                    dt = GetData(payrollCost);
                    break;
                case "horasPlanilla":
                    var weektaskHours = JsonConvert.DeserializeObject<List<WorkerPayrollWeeklyTaskViewModel>>(TempData[excelName].ToString());
                    fileName = "HorasPlanilla-" + weektaskHours.FirstOrDefault().FileName + ".xlsx";
                    dt = GetData(weektaskHours);
                    break;
                case "costosPllaFaseCuadrilla":
                    var payrollPhaseSewergroupCost = JsonConvert.DeserializeObject<List<WorkerPayrollPhaseSewerGroupCostViewModel>>(TempData[excelName].ToString());
                    fileName = "CostosPllaFaseCuadrilla-" + payrollPhaseSewergroupCost.FirstOrDefault().FileName + ".xlsx";
                    dt = GetData(payrollPhaseSewergroupCost);
                    break;
                case "horasCostoPlanilla":
                    var payrollPhaseCost = JsonConvert.DeserializeObject<List<PayrollReportPhaseHoursAndCostsByWeekViewModel>>(TempData[excelName].ToString());
                    fileName = "CostosPllaFase-" + payrollPhaseCost.FirstOrDefault().FileName + ".xlsx";
                    dt = GetData(payrollPhaseCost);
                    break;
                case "fasesObrerosPlanilla":
                    var payrollPhaseWorkers = JsonConvert.DeserializeObject<List<PayrollReportPhaseWorkersByWeekViewModel>>(TempData[excelName].ToString());
                    fileName = "ObrerosPllaFase-" + payrollPhaseWorkers.FirstOrDefault().FileName + ".xlsx";
                    dt = GetData(payrollPhaseWorkers);
                    break;
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                //Add DataTable in worksheet
                wb.Worksheets.Add(dt);
                wb.Worksheet(dt.TableName).Columns().AdjustToContents();
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        #region Helpers
        private DataTable GetData(List<WorkerPayrollWorkforceCostViewModel> workforceCostList)
        {
            //Creating DataTable
            DataTable dt = new DataTable
            {
                //Setting Table Name
                TableName = "REPORTE MO"
            };
            //Add Columns
            dt.Columns.Add("FECHA", typeof(DateTime));
            dt.Columns.Add("DNI", typeof(string));
            dt.Columns.Add("APELLIDOS Y NOMBRES", typeof(string));
            dt.Columns.Add("CAT", typeof(string));
            dt.Columns.Add("ESPECIALIDAD", typeof(string));
            dt.Columns.Add("FASE", typeof(string));
            dt.Columns.Add("COLABORADOR", typeof(string));
            dt.Columns.Add("RESP.", typeof(string));
            dt.Columns.Add("CUADRILLA", typeof(string));
            dt.Columns.Add("SUELDO BRUTO", typeof(decimal));
            dt.Columns.Add("ESSALUD (AP)", typeof(decimal));
            dt.Columns.Add("AFP JUB ANT (AP)", typeof(decimal));
            dt.Columns.Add("SCTR SALUD (AP)", typeof(decimal));
            dt.Columns.Add("SCTR PENSION (AP)", typeof(decimal));
            dt.Columns.Add("ESSALUD VIDA (AP)", typeof(decimal));
            dt.Columns.Add("TOTAL APORTES", typeof(decimal));
            dt.Columns.Add("TOTAL COSTO", typeof(decimal));
            //Add Rows in DataTable
            foreach (var item in workforceCostList)
            {
                dt.Rows.Add(
                    item.WeekStartDate.ToDateTime(),
                    item.Document,
                    item.FullName,
                    item.CategoryName,
                    item.Position,
                    item.Phase,
                    item.Collaborator,
                    item.Responsable,
                    item.SewerGroup,
                    item.TotalIncome,
                    item.EsSalud,
                    item.EarlyRetirementSPP,
                    item.SCTRSalud,
                    item.SCTRPension,
                    item.EsSaludVida,
                    item.TotalContribution,
                    item.TotalCost
                );
            }
            dt.AcceptChanges();
            return dt;
        }
        private DataTable GetData(List<WorkerPayrollPhaseSewerGroupCostViewModel> payrollPhaseSewergroupCost)
        {
            //Creating DataTable
            DataTable dt = new DataTable
            {
                //Setting Table Name
                TableName = "REPORTE MO"
            };
            //Add Columns
            dt.Columns.Add("FECHA", typeof(DateTime));
            dt.Columns.Add("FASE COD.", typeof(string));
            dt.Columns.Add("FASE DESC.", typeof(string));
            dt.Columns.Add("COLABORADOR", typeof(string));
            dt.Columns.Add("RESP.", typeof(string));
            dt.Columns.Add("CUADRILLA", typeof(string));
            dt.Columns.Add("TOTAL COSTO", typeof(decimal));
            //Add Rows in DataTable
            foreach (var item in payrollPhaseSewergroupCost)
            {
                dt.Rows.Add(
                    item.WeekStartDate.ToDateTime(),
                    item.PhaseCode,
                    item.PhaseDescription,
                    item.Collaborator,
                    item.Responsable,
                    item.SewerGroup,
                    item.TotalCost
                );
            }
            dt.AcceptChanges();
            return dt;
        }
        private DataTable GetData(List<WorkerPayrollWeeklyTaskViewModel> workforceHoursList)
        {
            //Creating DataTable
            DataTable dt = new DataTable
            {
                //Setting Table Name
                TableName = "REPORTE MO"
            };
            //Add Columns
            dt.Columns.Add("FASE SISTEMA", typeof(string));
            dt.Columns.Add("NOMBRE FASE", typeof(string));
            dt.Columns.Add("CUADRILLA", typeof(string));
            dt.Columns.Add("ENCARGADO", typeof(string));
            dt.Columns.Add("ING.RESP.", typeof(string));
            dt.Columns.Add("OP (HH)", typeof(decimal));
            dt.Columns.Add("OF (HH)", typeof(decimal));
            dt.Columns.Add("PE (HH)", typeof(decimal));
            dt.Columns.Add("TOTAL HH", typeof(decimal));
            dt.Columns.Add("OP (CANT)", typeof(int));
            dt.Columns.Add("OF (CANT)", typeof(int));
            dt.Columns.Add("PE (CANT)", typeof(int));
            dt.Columns.Add("TOTAL (CANT)", typeof(int));
            //Add Rows in DataTable
            foreach (var item in workforceHoursList)
            {
                dt.Rows.Add(
                    item.PhaseCode,
                    item.Description,
                    item.SewerGroupCode,
                    item.Collaborator,
                    item.Responsable,
                    item.TotalHoursOp,
                    item.TotalHoursOf,
                    item.TotalHoursPa,
                    item.TotalHours,
                    item.TotalWorkersOp,
                    item.TotalWorkersOf,
                    item.TotalWorkersPa,
                    item.TotalWorkers
                );
            }
            dt.AcceptChanges();
            return dt;
        }
        private DataTable GetData(List<PayrollReportPhaseHoursAndCostsByWeekViewModel> data)
        {
            //Creating DataTable
            DataTable dt = new DataTable
            {
                //Setting Table Name
                TableName = "REPORTE MO"
            };
            //Add Columns
            dt.Columns.Add("FASE SISTEMA", typeof(string));
            dt.Columns.Add("NOMBRE FASE", typeof(string));
            dt.Columns.Add("CUADRILLA", typeof(string));
            dt.Columns.Add("ENCARGADO", typeof(string));
            dt.Columns.Add("ING.RESP.", typeof(string));
            dt.Columns.Add("OP (HH)", typeof(decimal));
            dt.Columns.Add("OF (HH)", typeof(decimal));
            dt.Columns.Add("PE (HH)", typeof(decimal));
            dt.Columns.Add("TOTAL HH", typeof(decimal));
            dt.Columns.Add("OP (COSTO)", typeof(decimal));
            dt.Columns.Add("OF (COSTO)", typeof(decimal));
            dt.Columns.Add("PE (COSTO)", typeof(decimal));
            dt.Columns.Add("TOTAL COSTO", typeof(decimal));
            //Add Rows in DataTable
            foreach (var item in data)
            {
                dt.Rows.Add(
                    item.PhaseCode,
                    item.Description,
                    item.SewerGroupCode,
                    item.Collaborator,
                    item.Responsable,
                    item.TotalHoursOp,
                    item.TotalHoursOf,
                    item.TotalHoursPa,
                    item.TotalHours,
                    item.TotalCostsOp,
                    item.TotalCostsOf,
                    item.TotalCostsPa,
                    item.TotalCosts
                );
            }
            dt.AcceptChanges();
            return dt;
        }
        private DataTable GetData(List<PayrollReportPhaseWorkersByWeekViewModel> data)
        {
            //Creating DataTable
            DataTable dt = new DataTable
            {
                //Setting Table Name
                TableName = "REPORTE MO"
            };
            //Add Columns
            dt.Columns.Add("FASE SISTEMA", typeof(string));
            dt.Columns.Add("NOMBRE FASE", typeof(string));
            dt.Columns.Add("CUADRILLA", typeof(string));
            dt.Columns.Add("ENCARGADO", typeof(string));
            dt.Columns.Add("ING.RESP.", typeof(string));
            dt.Columns.Add("OP", typeof(decimal));
            dt.Columns.Add("OF", typeof(decimal));
            dt.Columns.Add("PE", typeof(decimal));
            dt.Columns.Add("TOTAL", typeof(decimal));
            //Add Rows in DataTable
            foreach (var item in data)
            {
                dt.Rows.Add(
                    item.PhaseCode,
                    item.Description,
                    item.SewerGroupCode,
                    item.Collaborator,
                    item.Responsable,
                    item.TotalWorkersOp,
                    item.TotalWorkersOf,
                    item.TotalWorkersPa,
                    item.TotalWorkers
                );
            }
            dt.AcceptChanges();
            return dt;
        }
        #endregion
    }
}
