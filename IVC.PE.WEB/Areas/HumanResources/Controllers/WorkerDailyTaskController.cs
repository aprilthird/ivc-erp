using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.HumanResources;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.ENTITIES.UspModels.HumanResources;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectCollaboratorViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectPhaseViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.SewerGroupViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.SystemPhaseViewModels;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.WorkerDailyTaskViewModels;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.WorkerViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.ProviderViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace IVC.PE.WEB.Areas.HumanResources.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.HumanResources.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.HUMAN_RESOURCES)]
    [Route("recursos-humanos/tareo-diario")]
    public class WorkerDailyTaskController : BaseController
    {
        public WorkerDailyTaskController(IvcDbContext context,
            UserManager<ApplicationUser> userManager,
            ILogger<WorkerDailyTaskController> logger)
            : base(context, userManager, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetDailyTask(string dateTask, Guid? sewerGroupId = null, Guid? workFrontHeadId = null, Guid? projectId = null)
        {
            SqlParameter projectParam = new SqlParameter("@ProjectId", projectId ?? GetProjectId());
            SqlParameter dateParam = new SqlParameter("@DateTask", !string.IsNullOrEmpty(dateTask) ? dateTask.ToDateTime() : DateTime.Now);
            SqlParameter sewergroupParam = new SqlParameter("@SewerGroupId", SqlDbType.UniqueIdentifier);
            sewergroupParam.Value = (object)sewerGroupId ?? DBNull.Value;
            SqlParameter workfrontheadParam = new SqlParameter("@WorkFrontHeadId", SqlDbType.UniqueIdentifier);
            workfrontheadParam.Value = (object)workFrontHeadId ?? DBNull.Value;

            var data = await _context.Set<UspWorkerDailyTask>().FromSqlRaw("execute HumanResources_uspWorkerDailyTasks @ProjectId, @DateTask, @SewerGroupId, @WorkFrontHeadId"
                , projectParam, dateParam, sewergroupParam, workfrontheadParam)
                .IgnoreQueryFilters()
                .ToListAsync();

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var dailyTask = await _context.WorkerDailyTasks
                                .Include(x => x.Worker)
                                .Where(x => x.Id == id)
                                .Select(x => new WorkerDailyTaskViewModel
                                {
                                    Id = x.Id,
                                    WorkerId = x.WorkerId,
                                    ProjectId = x.ProjectId,
                                    ProjectPhaseId = x.ProjectPhaseId,
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
                                    UnPaidLeave = x.UnPaidLeave,
                                    Worker = new WorkerViewModel
                                    {
                                        Name = x.Worker.Name,
                                        MiddleName = x.Worker.MiddleName,
                                        PaternalSurname = x.Worker.PaternalSurname,
                                        MaternalSurname = x.Worker.MaternalSurname,
                                        DocumentType = x.Worker.DocumentType,
                                        Document = x.Worker.Document
                                    },
                                    IsCeased = x.IsCeased,
                                    CeasedDate = x.CeasedDate.HasValue ? x.CeasedDate.Value.ToDateString() : string.Empty
                                }).FirstOrDefaultAsync();

            if (dailyTask == null)
            {
                return BadRequest("No se encontró el tareo");
            }

            return Ok(dailyTask);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(WorkerDailyTaskCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dbDt = await _context.WorkerDailyTasks
                .FirstOrDefaultAsync(x => x.Date.Date == model.Date.ToDateTime().Date &&
                                        x.WorkerId == model.WorkerId);

            if (dbDt != null)
            {
                dbDt.ProjectPhaseId = Guid.Empty;
                dbDt.SewerGroupId = model.SewerGroupId;
                dbDt.HoursNormal = 0.0M;
                dbDt.Hours60Percent = 0.0M;
                dbDt.Hours100Percent = 0.0M;
                dbDt.HoursHoliday = 0.0M;
                dbDt.MedicalLeave = false;
                dbDt.HoursMedicalRest = 0.0M;
                dbDt.HoursPaternityLeave = 0.0M;
                dbDt.HoursPaidLeave = 0.0M;
                dbDt.UnPaidLeave = false;
                dbDt.LaborSuspension = false;
                dbDt.NonAttendance = false;
            } else
            {
                var dailyTask = new WorkerDailyTask
                {
                    WorkerId = model.WorkerId,
                    ProjectId = model.ProjectId,
                    ProjectPhaseId = Guid.Empty,
                    SewerGroupId = model.SewerGroupId,
                    Date = model.Date.ToDateTime(),
                    HoursNormal = 0.0M,
                    Hours60Percent = 0.0M,
                    Hours100Percent = 0.0M,
                    HoursHoliday = 0.0M,
                    MedicalLeave = false,
                    HoursMedicalRest = 0.0M,
                    HoursPaternityLeave = 0.0M,
                    HoursPaidLeave = 0.0M,
                    UnPaidLeave = false,
                    LaborSuspension = false,
                    NonAttendance = false
                };
                await _context.WorkerDailyTasks.AddAsync(dailyTask);
            }
            
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, WorkerDailyTaskViewModel model, [FromQuery]bool weekly = false)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dailyTask = await _context.WorkerDailyTasks
                                .FirstOrDefaultAsync(x => x.Id == id);

            if (dailyTask == null)
            {
                return BadRequest("No se encontró el tareo");
            }

            dailyTask.ProjectPhaseId = model.ProjectPhaseId;
            dailyTask.SewerGroupId = model.SewerGroupId;
            dailyTask.HoursNormal = model.HoursNormal;
            dailyTask.Hours60Percent = model.Hours60Percent;
            dailyTask.Hours100Percent = model.Hours100Percent;
            
            if(weekly)
            {
                dailyTask.MedicalLeave = model.MedicalLeave;
                dailyTask.HoursMedicalRest = model.HoursMedicalRest;
                dailyTask.HoursPaternityLeave = model.HoursPaternityLeave;
                dailyTask.HoursHoliday = model.HoursHoliday;
                dailyTask.HoursPaidLeave = model.HoursPaidLeave;
                dailyTask.UnPaidLeave = model.UnPaidLeave;
                dailyTask.LaborSuspension = model.LaborSuspension;
                dailyTask.NonAttendance = model.NonAttendance;
            }

            if (dailyTask.IsCeased != model.IsCeased)
            {
                // Obrero activo y hay que cesar
                if (!dailyTask.IsCeased && model.IsCeased)
                {
                    dailyTask.CeasedDate = model.CeasedDate.ToDateTime();
                    dailyTask.IsCeased = model.IsCeased;
                    var worker = await _context.WorkerWorkPeriods
                        .Where(x => x.WorkerId == model.WorkerId && 
                            x.EntryDate.Value.Date <= dailyTask.Date.Date &&
                            (x.CeaseDate.HasValue ? x.CeaseDate.Value.Date >= dailyTask.Date.Date : true))
                        .FirstOrDefaultAsync();
                    worker.CeaseDate = model.CeasedDate.ToDateTime();
                    worker.IsActive = false;
                }

                // Obrero que se cesó por error
                if (dailyTask.IsCeased && !model.IsCeased)
                {
                    dailyTask.CeasedDate = null;
                    dailyTask.IsCeased = model.IsCeased;
                    var worker = await _context.WorkerWorkPeriods
                        .Where(x => x.WorkerId == model.WorkerId &&
                            x.EntryDate.Value.Date <= dailyTask.Date.Date &&
                            (x.CeaseDate.HasValue ? x.CeaseDate.Value.Date >= dailyTask.Date.Date : true))
                        .FirstOrDefaultAsync();
                    worker.CeaseDate = null;
                    worker.IsActive = true;
                }
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var dailyTask = await _context.WorkerDailyTasks
                                .FirstOrDefaultAsync(x => x.Id == id);

            if (dailyTask == null)
            {
                return BadRequest("No se encontró el tareo");
            }

            _context.WorkerDailyTasks.Remove(dailyTask);
            await _context.SaveChangesAsync();

            return Ok();
        }

        



        [HttpPost("importar-dia")]
        public async Task<IActionResult> ImportDay(WorkerDailyTasksImportDayViewModel model)
        {
            Dictionary<int, string> loadErrors = new Dictionary<int, string>();

            using (var mem = new MemoryStream())
            {
                await model.File.CopyToAsync(mem);
                using (var workBook = new XLWorkbook(mem))
                {
                    int row = 3;
                    var projectId = model.ProjectId;
                    var workSheet = workBook.Worksheets.First();
                    var totalRows = workSheet.LastRowUsed().RowNumber();
                    DateTime dailyTaskDate = model.DateTask.ToDateTime();
                    int numDay = (int)dailyTaskDate.DayOfWeek;
                    List<WorkerDailyTask> workerDailyTasks = new List<WorkerDailyTask>();
                    List<WorkerDailyTask> workerDailyTasksToDelete = new List<WorkerDailyTask>();

                    var phases = await _context.ProjectPhases
                        .Where(x => x.ProjectId == model.ProjectId)
                        .ToListAsync();

                    var workers = await _context.WorkerWorkPeriods
                        .Include(x => x.Worker)
                        .Where(x => x.EntryDate.Value <= dailyTaskDate.Date &&
                                    (x.CeaseDate.HasValue ? x.CeaseDate.Value >= dailyTaskDate.Date : true) &&
                                    x.ProjectId == model.ProjectId)
                        .Select(x => x.Worker)
                        .ToListAsync();

                    var workerDTs = _context.WorkerDailyTasks
                            .Include(x => x.SewerGroup)
                            .Where(x => x.ProjectId == projectId && x.Date.Date == dailyTaskDate.Date)
                            .AsQueryable();

                    var activeSewergroups = await _context.SewerGroupPeriods
                        .Include(x => x.SewerGroup)
                        .Where(x => x.DateStart.Date <= dailyTaskDate.Date &&
                                (x.DateEnd.HasValue ? x.DateEnd.Value >= dailyTaskDate.Date : true) &&
                                x.SewerGroup.ProjectId == GetProjectId() &&
                                (model.WorkFrontHeadId != Guid.Empty ? x.WorkFrontHeadId == model.WorkFrontHeadId : true))
                        .ToListAsync();

                    var wDts = new List<WorkerDailyTask>();
                    foreach (var sg in activeSewergroups)
                    {
                        wDts.AddRange(
                            workerDTs.Where(x => x.SewerGroupId == sg.SewerGroupId).ToList()
                            );
                    }

                    while (row <= totalRows)
                    {
                        var nroDocument = workSheet.Cell($"A{row}").Value.ToString();
                        var worker = workers.FirstOrDefault(x => x.Document == nroDocument);
                        if (worker != null)
                        {
                            var existDailyTask = wDts.FirstOrDefault(x => x.WorkerId == worker.Id && x.Date.Date == dailyTaskDate.Date);

                            if (existDailyTask == null)
                            {
                                loadErrors.Add(row, nroDocument + "no existe en la lista de obreros habilitados.");
                                row++;
                                continue;
                            }

                            SetDataVariable(workSheet, numDay, row, existDailyTask, phases);

                            if (existDailyTask.ProjectPhaseId == Guid.Empty)
                                loadErrors.Add(row, nroDocument + " código de Fase incorrecto.");

                            if (existDailyTask.HoursNormal == -1)
                            {
                                existDailyTask.HoursNormal = 0;
                                loadErrors.Add(row, nroDocument + " valor HN incorrecto.");
                                row++;
                                continue;
                            }

                            if (existDailyTask.Hours60Percent == -1)
                            {
                                existDailyTask.Hours60Percent = 0;
                                loadErrors.Add(row, nroDocument + " valor H60% incorrecto.");
                                row++;
                                continue;
                            }

                            if (existDailyTask.Hours100Percent == -1)
                            {
                                existDailyTask.Hours100Percent = 0;
                                loadErrors.Add(row, nroDocument + " valor H100% incorrecto.");
                                row++;
                                continue;
                            }
                        }
                        else
                        {
                            if (int.TryParse(nroDocument, out int numDoc))
                                loadErrors.Add(row, nroDocument + " esta cesado o no existe en la base de datos.");
                        }
                        ++row;
                    }

                    await _context.SaveChangesAsync();
                }
            }

            if (loadErrors.Count != 0)
                TempData["ExcelErrors"] = JsonConvert.SerializeObject(loadErrors);

            return Ok(loadErrors.Count > 0 ? 1 : 0);
        }

        [HttpPost("importar-obreros")]
        public async Task<IActionResult> ImportWorkers(WorkerDailyTasksImportDayViewModel model)
        {
            Dictionary<int, string> loadErrors = new Dictionary<int, string>();

            using (var mem = new MemoryStream())
            {
                await model.File.CopyToAsync(mem);
                using (var workBook = new XLWorkbook(mem))
                {
                    int row = 3;
                    var projectId = model.ProjectId;
                    var workSheet = workBook.Worksheets.First();
                    var totalRows = workSheet.LastRowUsed().RowNumber();
                    var dailyTaskDate = model.DateTask.ToDateTime();
                    int numDay = (int)dailyTaskDate.DayOfWeek;
                    var workerDailyTasks = new List<WorkerDailyTask>();

                    var sewerGroups = await _context.SewerGroupPeriods
                        .Include(x => x.SewerGroup)
                        .Where(x => x.DateStart.Date <= dailyTaskDate.Date &&
                                (x.DateEnd.HasValue ? x.DateEnd.Value >= dailyTaskDate.Date : true) &&
                                x.SewerGroup.ProjectId == GetProjectId() &&
                                (model.WorkFrontHeadId != Guid.Empty ? x.WorkFrontHeadId == model.WorkFrontHeadId : true))
                        .AsNoTracking()
                        .ToListAsync();

                    var workers = await _context.WorkerWorkPeriods
                        .Include(x => x.Worker)
                        .Where(x => x.EntryDate.Value <= dailyTaskDate.Date &&
                                    (x.CeaseDate.HasValue ? x.CeaseDate.Value >= dailyTaskDate.Date : true) &&
                                    x.ProjectId == model.ProjectId)
                        .Select(x => x.Worker)
                        .AsNoTracking()
                        .ToListAsync();

                    var workerDTs = await _context.WorkerDailyTasks
                        .Where(x => x.ProjectId == projectId && x.Date.Date == dailyTaskDate.Date)
                        .ToListAsync();
                    
                    while (row <= totalRows)
                    {
                        var nroDocument = workSheet.Cell($"A{row}").Value.ToString();
                        var worker = workers.FirstOrDefault(x => x.Document == nroDocument);
                        if (worker != null)
                        {   
                            var sewerGroupCode = workSheet.Cell($"B{row}").Value.ToString();
                            var sewerGroup = sewerGroups.FirstOrDefault(x => x.SewerGroup.Code == sewerGroupCode);
                            if (sewerGroup == null)
                            {
                                loadErrors.Add(row, nroDocument + " código de Cuadrilla no pertence al Jefe de Frente.");
                                row++;
                                continue;
                            }

                            var existDailyTask = workerDTs
                                .FirstOrDefault(x => x.WorkerId == worker.Id 
                                        && x.Date.Date == dailyTaskDate.Date);
                            if (existDailyTask != null)
                            {
                                existDailyTask.Date = model.DateTask.ToDateTime();
                                existDailyTask.ProjectId = model.ProjectId;
                                existDailyTask.ProjectPhaseId = Guid.Empty;
                                existDailyTask.SewerGroupId = sewerGroup.SewerGroupId;
                                existDailyTask.HoursNormal = 0.0M;
                                existDailyTask.Hours60Percent = 0.0M;
                                existDailyTask.Hours100Percent = 0.0M;
                                existDailyTask.HoursMedicalRest = 0.0M;
                                existDailyTask.MedicalLeave = false;
                                existDailyTask.HoursPaternityLeave = 0.0M;
                                existDailyTask.HoursHoliday = 0.0M;
                                existDailyTask.HoursPaidLeave = 0.0M;
                                existDailyTask.UnPaidLeave = false;
                                existDailyTask.LaborSuspension = false;
                                existDailyTask.NonAttendance = false;
                            } else
                            {
                                var workerDailyTask = new WorkerDailyTask
                                {
                                    WorkerId = worker.Id,
                                    Date = model.DateTask.ToDateTime(),
                                    ProjectId = model.ProjectId,
                                    ProjectPhaseId = Guid.Empty,
                                    SewerGroupId = sewerGroup.SewerGroupId,
                                    HoursNormal = 0.0M,
                                    Hours60Percent = 0.0M,
                                    Hours100Percent = 0.0M,
                                    HoursMedicalRest = 0.0M,
                                    MedicalLeave = false,
                                    HoursPaternityLeave = 0.0M,
                                    HoursHoliday = 0.0M,
                                    HoursPaidLeave = 0.0M,
                                    UnPaidLeave = false,
                                    LaborSuspension = false,
                                    NonAttendance = false
                                };

                                workerDailyTasks.Add(workerDailyTask);
                            }
                        }
                        else
                        {
                            if (int.TryParse(nroDocument, out int numDoc))
                            {
                                loadErrors.Add(row, nroDocument + " se encuentra cesado o no existe en la base de datos.");
                            }
                        }
                        ++row;
                    }

                    try
                    {
                        if (workerDailyTasks.Count > 0)
                            await _context.WorkerDailyTasks.AddRangeAsync(workerDailyTasks);

                        await _context.SaveChangesAsync();
                    }
                    catch(Exception ex)
                    {
                        _logger.LogError(ex.StackTrace);
                    }
                }
            }

            if (loadErrors.Count != 0)
                TempData["ExcelErrors"] = JsonConvert.SerializeObject(loadErrors);

            return Ok(loadErrors.Count > 0 ? 1 : 0);
        }



        [HttpGet("descargar-excel-errores")]
        public FileResult DownloadFile()
        {
            if (TempData["ExcelErrors"] == null)
            {
                RedirectToAction("Empty");
            }

            var errorDailyTasks = JsonConvert.DeserializeObject<Dictionary<int, string>>(TempData["ExcelErrors"].ToString());

            DataTable dt = GetData(errorDailyTasks);
            //Name of File
            string fileName = "ErroresCarga.xlsx";
            using (XLWorkbook wb = new XLWorkbook())
            {
                //Add DataTable in worksheet
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }            
        }

        private DataTable GetData(Dictionary<int, string> loadErrors)
        {
            //Creating DataTable
            DataTable dt = new DataTable();
            //Setiing Table Name
            dt.TableName = "ErrorsData";
            //Add Columns
            dt.Columns.Add("Línea", typeof(int));
            dt.Columns.Add("Error", typeof(string));
            //Add Rows in DataTable
            foreach (var item in loadErrors)
            {
                dt.Rows.Add(item.Key, item.Value);
            }
            dt.AcceptChanges();
            return dt;
        }

        private void SetDataVariable(IXLWorksheet workSheet, int numDay, int row, WorkerDailyTask workerDailyTask, List<ProjectPhase> phases)
        {
            var hours = workSheet.Cell($"F{row}").Value.ToString();
            var hours60 = workSheet.Cell($"G{row}").Value.ToString();
            var hours100 = workSheet.Cell($"H{row}").Value.ToString();
            var phaseCode = workSheet.Cell($"D{row}").Value.ToString();

            if (!string.IsNullOrEmpty(hours))
            {
                var phase = phases.FirstOrDefault(x => x.Code == phaseCode);
                workerDailyTask.ProjectPhaseId = phase != null ? phase.Id : Guid.Empty;

                if (decimal.TryParse(hours, out decimal dHours))
                {
                    workerDailyTask.HoursNormal = dHours;

                    if (decimal.TryParse(hours60, out decimal dHours60))
                        workerDailyTask.Hours60Percent = dHours60;
                    else if (!String.IsNullOrEmpty(hours60))
                        workerDailyTask.Hours60Percent = -1;

                    if (decimal.TryParse(hours100, out decimal dHours100))
                        workerDailyTask.Hours100Percent = dHours100;
                    else if (!String.IsNullOrEmpty(hours100))
                        workerDailyTask.Hours100Percent = -1;
                }
                else
                {
                    //workerDailyTask.ProjectPhaseId = Guid.Parse("EC46101C-E51F-4B9A-828F-B150CF6A93F1"); //Phase.Code 160220
                    
                    switch (hours)
                    {
                        case ConstantHelpers.WorkerDailyTask.HourConcept.HOURS_PAID_LEAVE: workerDailyTask.HoursPaidLeave = (numDay == 6 ? 5.5M : (numDay == 0 ? 0 : 8.5M)); break;
                        case ConstantHelpers.WorkerDailyTask.HourConcept.HOURS_MEDICAL_REST: workerDailyTask.HoursMedicalRest = (numDay == 6 ? 5.5M : (numDay == 0 ? 0 : 8.5M)); break;
                        case ConstantHelpers.WorkerDailyTask.HourConcept.LABOR_SUSPENSION: workerDailyTask.LaborSuspension = true; break;
                        case ConstantHelpers.WorkerDailyTask.HourConcept.MEDICAL_LEAVE: workerDailyTask.MedicalLeave = true; break;
                        case ConstantHelpers.WorkerDailyTask.HourConcept.UNPAID_LEAVE: workerDailyTask.UnPaidLeave = true; break;
                        case ConstantHelpers.WorkerDailyTask.HourConcept.HOURS_HOLIDAY: workerDailyTask.HoursHoliday = (numDay == 6 ? 5.5M : (numDay == 0 ? 0 : 8.5M)); break;
                        case ConstantHelpers.WorkerDailyTask.HourConcept.NOT_ATTENDANCE: workerDailyTask.NonAttendance = true; break;
                        default: workerDailyTask.HoursNormal = -1; break;
                    }
                }
            } else
            {
                workerDailyTask.NonAttendance = true;
            }
        }
        
        

        [HttpPost("copiar-dia")]
        public async Task<IActionResult> CopyDay(WorkerDailyTaskCopyDayViewModel model)
        {
            if (!model.WorkFrontHeadId.HasValue)
                return BadRequest("Seleccione un jefe de frente.");

            var fromDayDT = model.FromDay.ToDateTime();
            var toDayDT = model.ToDay.ToDateTime();

            var activeSewergroups = await _context.SewerGroupPeriods
                .Include(x => x.SewerGroup)
                .Where(x => x.DateStart.Date <= toDayDT.Date &&
                        (x.DateEnd.HasValue ? x.DateEnd.Value >= toDayDT.Date : true) &&
                        x.SewerGroup.ProjectId == GetProjectId() &&
                        (model.WorkFrontHeadId.HasValue ? x.WorkFrontHeadId == model.WorkFrontHeadId.Value : true))
                .ToListAsync();

            var dbWorkersDailyTask = await _context.WorkerDailyTasks
                .Where(x => (x.Date.Date == toDayDT.Date || x.Date.Date == fromDayDT.Date ) && 
                            x.ProjectId == model.ProjectId)
                .ToListAsync();

            var todayWdts = new List<WorkerDailyTask>();
            var fromWdts = new List<WorkerDailyTask>();
            foreach (var sg in activeSewergroups)
            {
                todayWdts.AddRange(
                    dbWorkersDailyTask.Where(x => x.SewerGroupId == sg.SewerGroupId &&
                                            x.Date.Date == toDayDT.Date).ToList()
                    );
                fromWdts.AddRange(
                    dbWorkersDailyTask.Where(x => x.SewerGroupId == sg.SewerGroupId &&
                                            x.Date.Date == fromDayDT.Date).ToList()
                    );
            }

            var newWorkersDailyTasks = new List<WorkerDailyTask>();

            foreach (var dailyTask in fromWdts)
            {
                var workerDailyTask = todayWdts
                    .FirstOrDefault(x => x.WorkerId == dailyTask.WorkerId);

                if (workerDailyTask != null)
                {
                    workerDailyTask.ProjectPhaseId = Guid.Empty;
                    workerDailyTask.SewerGroupId = dailyTask.SewerGroupId;
                    workerDailyTask.HoursNormal = 0.0M;
                    workerDailyTask.Hours60Percent = 0.0M;
                    workerDailyTask.Hours100Percent = 0.0M;
                    workerDailyTask.HoursMedicalRest = 0.0M;
                    workerDailyTask.MedicalLeave = false;
                    workerDailyTask.HoursPaternityLeave = 0.0M;
                    workerDailyTask.HoursHoliday = 0.0M;
                    workerDailyTask.HoursPaidLeave = 0.0M;
                    workerDailyTask.UnPaidLeave = false;
                    workerDailyTask.LaborSuspension = false;
                    workerDailyTask.NonAttendance = false;
                } else
                {
                    workerDailyTask = new WorkerDailyTask
                    {
                        WorkerId = dailyTask.WorkerId,
                        Date = model.ToDay.ToDateTime(),
                        ProjectId = dailyTask.ProjectId,
                        ProjectPhaseId = Guid.Empty,
                        SewerGroupId = dailyTask.SewerGroupId,
                        HoursNormal = 0.0M,
                        Hours60Percent = 0.0M,
                        Hours100Percent = 0.0M,
                        HoursMedicalRest = 0.0M,
                        MedicalLeave = false,
                        HoursPaternityLeave = 0.0M,
                        HoursHoliday = 0.0M,
                        HoursPaidLeave = 0.0M,
                        UnPaidLeave = false,
                        LaborSuspension = false,
                        NonAttendance = false
                    };
                    newWorkersDailyTasks.Add(workerDailyTask);
                }
            }

            if (newWorkersDailyTasks.Count > 0)
                await _context.WorkerDailyTasks.AddRangeAsync(newWorkersDailyTasks);

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("descargar-obreros-habiles")]
        public FileResult DownloadWorkersExcel (string taskDate, Guid projectId, Guid? workFrontHeadId = null, Guid? sewerGroupId = null)
        {
            SqlParameter dateParam = new SqlParameter("@TaskDate", !string.IsNullOrEmpty(taskDate) ? taskDate.ToDateTime() : DateTime.Now);
            SqlParameter projectParam = new SqlParameter("@ProjectId", projectId);
            SqlParameter sewergroupParam = new SqlParameter("@SewerGroupId", SqlDbType.UniqueIdentifier);
            sewergroupParam.Value = (object)sewerGroupId ?? DBNull.Value;
            SqlParameter workfrontheadParam = new SqlParameter("@WorkFrontHeadId", SqlDbType.UniqueIdentifier);
            workfrontheadParam.Value = (object)workFrontHeadId ?? DBNull.Value;

            var query = _context.Set<UspWorkerDailyTasksByDate>().FromSqlRaw("execute HumanResources_uspWorkerDailyTasksByDate @TaskDate, @ProjectId, @SewerGroupId, @WorkFrontHeadId"
                , dateParam, projectParam, sewergroupParam, workfrontheadParam)
                .IgnoreQueryFilters()
                .ToList();


            //var workerDailyTaks = _context.WorkerDailyTasks
            //    .Include(x => x.SewerGroup)
            //    .Include(x => x.ProjectPhase)
            //    .Include(x => x.Worker)
            //    .Where(x => x.Date.Date == taskDateDt.Date && x.ProjectId == projectId)
            //    .AsQueryable();

            //var activeSewergroups = _context.SewerGroupPeriods
            //    .Include(x => x.SewerGroup)
            //    .Include(x => x.WorkFrontHead)
            //    .Include(x => x.ProjectCollaborator)
            //    .Include(x => x.Provider)
            //    .Include(x => x.ForemanEmployee)
            //    .Include(x => x.ForemanWorker)
            //    .Where(x => x.DateStart.Date <= taskDateDt.Date &&
            //            (x.DateEnd.HasValue ? x.DateEnd.Value >= taskDateDt.Date : true) &&
            //            x.SewerGroup.ProjectId == projectId &&
            //            (workFrontHeadId.HasValue ? x.WorkFrontHeadId == workFrontHeadId.Value : true) &&
            //            (sewerGroupId.HasValue ? x.SewerGroupId == sewerGroupId.Value : true))
            //    .AsQueryable();

            //var wdt = new List<WorkerDailyTask>();
            var fileName = "Todos-";
            if (workFrontHeadId.HasValue || sewerGroupId.HasValue)
            {
                //foreach (var sg in activeSewergroups)
                //{
                //    wdt.AddRange(
                //        workerDailyTaks.Where(x => x.SewerGroupId == sg.SewerGroupId).ToList()
                //        );
                //}
                if (sewerGroupId.HasValue)
                {
                    fileName = query.FirstOrDefault().SewerGroupCode + "-";
                }
                else if (workFrontHeadId.HasValue)
                {
                    fileName = query.FirstOrDefault().WorkFrontHeadCode + "-";
                }
            }
            //else
            //{
            //    wdt = workerDailyTaks.ToList();
            //}
            fileName = fileName + taskDate.Replace("/", "") + ".xlsx";

            //var query = wdt.OrderBy(x => x.SewerGroupId).ToList();

            //foreach (var item in query)
            //{
            //    var period = _context.WorkerWorkPeriods
            //        .Where(x => x.WorkerId == item.WorkerId &&
            //                x.EntryDate.Value.Date <= taskDateDt.Date &&
            //                (x.CeaseDate.HasValue ? x.CeaseDate.Value.Date >= taskDateDt.Date : true))
            //        .FirstOrDefault();
            //    if (period != null)
            //    {
            //        item.Worker.Category = period.Category;
            //    }
            //    else
            //    {
            //        item.Worker.Category = 1;
            //    }
            //}

            var phases = _context.ProjectPhases.Where(x => x.ProjectId == projectId).ToList();
            var dtPhases = new DataTable();
            dtPhases.TableName = "Fases";
            dtPhases.Columns.Add("Código", typeof(string));
            dtPhases.Columns.Add("Descripción", typeof(string));
            foreach (var item in phases)
                dtPhases.Rows.Add(item.Code, item.Description);
            dtPhases.AcceptChanges();

            using (XLWorkbook wb = new XLWorkbook())
            {
                var workSheet = wb.Worksheets.Add("Obreros");

                workSheet.Cell($"A1").Value = "Nro.Doc.";
                workSheet.Column(1).Style.NumberFormat.Format = "@";
                workSheet.Range("A1:A2").Merge();
                workSheet.Range("A1:A2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"B1").Value = "Trabajador";
                workSheet.Range("B1:B2").Merge();
                workSheet.Range("B1:B2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"C1").Value = "Categoría";
                workSheet.Range("C1:C2").Merge();
                workSheet.Range("C1:C2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"D1").Value = "Fase";
                workSheet.Column(4).Style.NumberFormat.Format = "@";
                workSheet.Range("D1:D2").Merge();
                workSheet.Range("D1:D2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"E1").Value = "Cuadrilla";
                workSheet.Range("E1:E2").Merge();
                workSheet.Range("E1:E2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"F1").Value = "HN";
                workSheet.Range("F1:F2").Merge();
                workSheet.Range("F1:F2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"G1").Value = "H60%";
                workSheet.Range("G1:G2").Merge();
                workSheet.Range("G1:G2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"H1").Value = "H100%";
                workSheet.Range("H1:H2").Merge();
                workSheet.Range("H1:H2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Row(1).Style.Font.SetBold(true).Font.SetFontSize(12);

                var row = 3;
                var sgId = Guid.Empty;
                foreach (var item in query)
                {                    
                    if (sgId != item.SewerGroupId)
                    {
                        sgId = item.SewerGroupId;
                        //var sewerGroup = activeSewergroups.First(x => x.SewerGroupId == item.SewerGroupId);

                        //if (sewerGroup.ProjectCollaboratorId.HasValue)
                        //    workSheet.Cell($"A{row}").Value = item.SewerGroup.Code + " - " + sewerGroup.Provider.BusinessName + " - " + sewerGroup.ProjectCollaborator.FullName;
                        //else
                        //{
                        //    var foreman = string.Empty;
                        //    if (sewerGroup.ForemanEmployeeId.HasValue)
                        //        foreman = sewerGroup.ForemanEmployee.FullName;
                        //    else if (item.SewerGroup.ForemanWorkerId.HasValue)
                        //        foreman = sewerGroup.ForemanWorker.FullName;
                        //    workSheet.Cell($"A{row}").Value = item.SewerGroup.Code + " - CASA - " + foreman;
                        //}
                        workSheet.Cell($"A{row}").Value = item.ForemanFullName;
                        workSheet.Range($"A{row}:H{row}").Merge().Style.Fill.SetBackgroundColor(XLColor.Gray);
                        row++;
                    }
                    workSheet.Cell($"A{row}").Value = item.WorkerDocument;
                    workSheet.Cell($"B{row}").Value = item.WorkerFullName;
                    //workSheet.Cell($"C{row}").Value = ConstantHelpers.Worker.Category.SHORT_VALUES[item.Worker.Category];
                    workSheet.Cell($"C{row}").Value = item.WorkerCategoryStr;
                    workSheet.Cell($"D{row}").Value = item.ProjectPhaseCode ?? "0";
                    workSheet.Cell($"E{row}").Value = item.SewerGroupCode;
                    workSheet.Cell($"F{row}").Value = GetHours(item);
                    workSheet.Cell($"G{row}").Value = item.Hours60Percent;
                    workSheet.Cell($"H{row}").Value = item.Hours100Percent;
                    row++;
                }

                workSheet.Columns(1, 8).AdjustToContents();
                workSheet.Rows().Style.Alignment.WrapText = true;
                workSheet.ExpandRows();

                //Add DataTable in worksheet
                //wb.Worksheets.Add(dt);
                wb.Worksheets.Add(dtPhases).Columns(1, 2).AdjustToContents();
                //wb.Worksheets.ElementAt(1).Columns(1, 2).AdjustToContents();

                var dtConcepts = new DataTable();
                dtConcepts.TableName = "Conceptos";
                dtConcepts.Columns.Add("Código", typeof(string));
                dtConcepts.Columns.Add("Descripción", typeof(string));
                foreach (var item in ConstantHelpers.WorkerDailyTask.HourConcept.VALUES)
                    dtConcepts.Rows.Add(item.Key, item.Value);
                dtConcepts.AcceptChanges();
                wb.Worksheets.Add(dtConcepts).Columns(1, 2).AdjustToContents();

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        private object GetHours(UspWorkerDailyTasksByDate item)
        {
            if(item.HoursNormal <= 0)
            {
                if (item.MedicalLeave)
                    return ConstantHelpers.WorkerDailyTask.HourConcept.MEDICAL_LEAVE;
                if (item.UnPaidLeave)
                    return ConstantHelpers.WorkerDailyTask.HourConcept.UNPAID_LEAVE;
                if (item.LaborSuspension)
                    return ConstantHelpers.WorkerDailyTask.HourConcept.LABOR_SUSPENSION;
                if (item.NonAttendance)
                    return ConstantHelpers.WorkerDailyTask.HourConcept.NOT_ATTENDANCE;
                if (item.HoursPaidLeave > 0)
                    return ConstantHelpers.WorkerDailyTask.HourConcept.HOURS_PAID_LEAVE;
                if (item.HoursHoliday > 0)
                    return ConstantHelpers.WorkerDailyTask.HourConcept.HOURS_HOLIDAY;
                if (item.HoursMedicalRest > 0)
                    return ConstantHelpers.WorkerDailyTask.HourConcept.HOURS_MEDICAL_REST;
            }
            return item.HoursNormal;
        }



        //Método que genera el formato excel para la carga de obrero hábiles para el tareo
        [HttpGet("descargar-formato-carga-obreros")]
        public FileResult DownloadExcelForWorkersLoad()
        {
            var todayDt = DateTime.Now;
            var sewerGroups = _context.SewerGroupPeriods
                .Include(x => x.SewerGroup)
                .Include(x => x.WorkFrontHead)  
                .Include(x => x.WorkFrontHead.User)
                .Where(x => x.DateStart.Date <= todayDt.Date &&
                        (x.DateEnd.HasValue ? x.DateEnd.Value >= todayDt.Date : true) &&
                        x.SewerGroup.ProjectId == GetProjectId())
                .ToList();

            var workFrontHeads = _context.WorkFrontHeads.Include(x => x.User).ToList();

            using (XLWorkbook wb = new XLWorkbook())
            {
                var workSheet = wb.Worksheets.Add("Obreros");

                workSheet.Cell($"A1").Value = "Nro.Doc.";
                workSheet.Range("A1:A2").Merge(false);
                workSheet.Range("A1:A2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"B1").Value = "Cuadrilla";
                workSheet.Range("B1:B2").Merge(false);
                workSheet.Range("B1:B2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Columns(1, 2).AdjustToContents();

                var dtSewerGroups = new DataTable();
                dtSewerGroups.TableName = "Cuadrillas";
                dtSewerGroups.Columns.Add("Cod. Cuadrilla.", typeof(string));
                dtSewerGroups.Columns.Add("Jefe de Frente", typeof(string));
                foreach(var sg in sewerGroups)
                {
                    dtSewerGroups.Rows.Add(
                        sg.SewerGroup.Code,
                        sg.WorkFrontHeadId.HasValue ? $"{sg.WorkFrontHead.Code} {sg.WorkFrontHead.User.FullName}" : string.Empty
                        );
                }
                wb.Worksheets.Add(dtSewerGroups).Columns(1, 2).AdjustToContents();

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "FormatoCargaObreros.xlsx");
                }
            }
        }

        //Método que trae la info del colaborador y responsable de la cuadrilla
        //cuando se hace un cambio en los filtros del tareo diario
        [HttpGet("cuadrilla/{id}")]
        public async Task<IActionResult> GetSewerGroupCollab(Guid? id = null)
        {
            if (id == null)
                return Ok(null);

            var sgPeriod = await _context.SewerGroupPeriods
                .Include(x => x.SewerGroup)
                .Where(x => x.SewerGroupId == id)
                .OrderByDescending(x => x.DateStart)
                .FirstOrDefaultAsync();

            var sewerGroupVM = new SewerGroupViewModel
            {
                Id = sgPeriod.SewerGroup.Id,
                Code = sgPeriod.SewerGroup.Code,
                Name = sgPeriod.SewerGroup.Name,
                Type = sgPeriod.SewerGroup.Type,
                WorkFrontHeadId = sgPeriod.WorkFrontHeadId,
                Destination = sgPeriod.Destination,
                WorkStructure = sgPeriod.WorkStructure,
                WorkComponent = sgPeriod.WorkComponent
            };

            if (sgPeriod.ProjectCollaboratorId.HasValue)
            {
                sewerGroupVM.ProjectCollaboratorId = sgPeriod.ProjectCollaboratorId.Value;
                sewerGroupVM.ProjectCollaborator = _context.ProjectCollaborators
                    .Include(x => x.Provider)
                    .Where(x => x.Id == sgPeriod.ProjectCollaboratorId.Value)
                    .Select(x => new ProjectCollaboratorViewModel
                    {
                        Id = x.Id,
                        PaternalSurname = x.PaternalSurname,
                        MaternalSurname = x.MaternalSurname,
                        Name = x.Name,
                        ProjectId = x.ProjectId,
                        ProviderId = x.ProviderId,
                        Provider = new ProviderViewModel
                        {
                            Id = x.ProviderId,
                            BusinessName = x.Provider.BusinessName
                        }
                    }).FirstOrDefault();
                sewerGroupVM.EmployeeWorkerName = sewerGroupVM.ProjectCollaborator.FullName;
            }
            else
            {
                if (sgPeriod.SewerGroup.ForemanEmployeeId.HasValue)
                    sewerGroupVM.EmployeeWorkerName = _context.Employees.FirstOrDefault(x => x.Id == sgPeriod.ForemanEmployeeId.Value).FullName;
                else if (sgPeriod.SewerGroup.ForemanWorkerId.HasValue)
                    sewerGroupVM.EmployeeWorkerName = _context.Workers.FirstOrDefault(x => x.Id == sgPeriod.ForemanWorkerId.Value).FullName;
            }

            return Ok(sewerGroupVM);
        }
    }
};