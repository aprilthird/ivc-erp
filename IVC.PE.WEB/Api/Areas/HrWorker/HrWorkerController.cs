using IVC.PE.BINDINGRESOURCES.Areas.HrWorker;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.HumanResources;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Api.Areas.HrWorker
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/recursos-humanos/obreros")]
    public class HrWorkerController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public HrWorkerController(IvcDbContext context,
            IOptions<CloudStorageCredentials> storageCredentials)
            : base(context)
        {
            _storageCredentials = storageCredentials;
        }

        //-------------------------------------------
        //HrWorker > Asitencia
        //-------------------------------------------
        [HttpGet("asistencia/obreros-habiles")]
        public async Task<IActionResult> GetActiveWorkers(string taskDate, Guid? sewerGroupId = null, Guid? projectId = null)
        {
            if (!projectId.HasValue)
                return BadRequest("Falta seleccionar proyecto.");

            if (!sewerGroupId.HasValue)
                return BadRequest("Falta seleccionar cuadrilla.");

            var taskDateDt = taskDate.ToDateTime();

            var sewerGroup = await _context.SewerGroups
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == sewerGroupId.Value);

            var workers = await _context.WorkerWorkPeriods
                .Include(x => x.Worker)
                .Where(x => x.ProjectId == projectId.Value &&
                            x.EntryDate.Value.Date <= taskDateDt.Date &&
                            (x.CeaseDate.HasValue ? x.CeaseDate.Value.Date >= taskDateDt.Date : true))
                .Select(x => new WorkerAttendanceResourceModel
                {
                    WorkerId = x.WorkerId.Value,
                    WorkerFullName = x.Worker.RawFullName,
                    ProjectPhaseId = Guid.Empty,
                    ProjectPhaseCode = "0 - Sin Asignar",
                    SewerGroupId = sewerGroup.Id,
                    SewerGroupCode = sewerGroup.Code,
                    Category = x.Category,
                    Document = x.Worker.Document,
                    Attended = false,
                    AttendedIcon = "\uf00d"
                }).ToListAsync();

            return Ok(workers);
        }

        [HttpPost("asistencia/registrar")]
        public async Task<IActionResult> CreateAttendance([FromBody] WorkerAttendanceListResourceModel model)
        {
            var taskDateDt = model.TaskDate.ToDateTime();

            var attendanceDate = await _context.WorkerDailyTasks
                .Include(x => x.Worker)
                .Include(x => x.ProjectPhase)
                .Where(x => x.Date.Date == taskDateDt.Date &&
                    x.SewerGroupId == model.SewerGroupId &&
                    x.ProjectId == model.ProjectId)
                .ToListAsync();

            var newAttendance = new List<WorkerDailyTask>();

            foreach (var attendance in model.WorkerAttendances)
            {
                if (attendance.Attended)
                {
                    var exist = attendanceDate.FirstOrDefault(x => x.WorkerId == attendance.WorkerId);
                    if (exist != null)
                    {
                        exist.ProjectPhaseId = attendance.ProjectPhaseId;
                        exist.HoursNormal = taskDateDt.DayOfWeek == DayOfWeek.Sunday ? 0.0M : (taskDateDt.DayOfWeek == DayOfWeek.Saturday ? 5.5M : 8.5M);
                        exist.Hours60Percent = 0.0M;
                        exist.Hours100Percent = 0.0M;
                        exist.HoursHoliday = 0.0M;
                        exist.HoursMedicalRest = 0.0M;
                        exist.HoursPaidLeave = 0.0M;
                        exist.HoursPaternityLeave = 0.0M;
                        exist.LaborSuspension = false;
                        exist.MedicalLeave = false;
                        exist.NonAttendance = false;
                        exist.UnPaidLeave = false;
                        continue;
                    }

                    newAttendance.Add(new WorkerDailyTask
                    {
                        CeasedDate = null,
                        Date = taskDateDt,
                        Hours100Percent = 0.0M,
                        Hours60Percent = 0.0M,
                        HoursHoliday = 0.0M,
                        HoursMedicalRest = 0.0M,
                        HoursNormal = (taskDateDt.DayOfWeek == DayOfWeek.Sunday ? 0.0M : (taskDateDt.DayOfWeek == DayOfWeek.Saturday ? 5.5M : 8.5M)),
                        HoursPaidLeave = 0.0M,
                        HoursPaternityLeave = 0.0M,
                        IsCeased = false,
                        LaborSuspension = false,
                        MedicalLeave = false,
                        NonAttendance = false,
                        ProjectId = model.ProjectId,
                        ProjectPhaseId = attendance.ProjectPhaseId,
                        SewerGroupId = model.SewerGroupId,
                        UnPaidLeave = false,
                        WorkerId = attendance.WorkerId
                    });
                }
            }

            await _context.WorkerDailyTasks.AddRangeAsync(newAttendance);
            await _context.SaveChangesAsync();

            return Ok();
        }

        //-------------------------------------------
        //HrWorker > Tareo Diario
        //-------------------------------------------
        [HttpGet("tareo/listar")]
        public async Task<IActionResult> GetDailyTask(string taskDate, Guid? sewerGroupId = null)
        {
            var taskDateDt = taskDate.ToDateTime();

            var attendanceDate = await _context.WorkerDailyTasks
                .Include(x => x.ProjectPhase)
                .Include(x => x.SewerGroup)
                .Where(x => x.Date.Date == taskDateDt.Date &&
                    (sewerGroupId.HasValue ? x.SewerGroupId == sewerGroupId.Value : true))
                .Select(x => new WorkerDailyTaskResourceModel
                {
                    Id = x.Id,
                    WorkerId = x.WorkerId,
                    Date = x.Date,
                    HoursNormal = x.HoursNormal,
                    Hours60Percent = x.Hours60Percent,
                    Hours100Percent = x.Hours100Percent,
                    ProjectId = x.ProjectId,
                    ProjectPhaseId = (Guid)x.ProjectPhaseId,
                    Phase = x.ProjectPhase.Code + " - " + x.ProjectPhase.Description,
                    SewerGroupId = x.SewerGroupId,
                    SewerGroup = x.SewerGroup.Code
                }).ToListAsync();

            var worker = await _context.WorkerWorkPeriods
                .Include(x => x.Worker)
                .AsNoTracking()
                .Where(x => x.EntryDate.Value.Date <= taskDateDt.Date &&
                            (x.CeaseDate.HasValue ? x.CeaseDate.Value.Date >= taskDateDt.Date : true))
                .ToListAsync();

            foreach (var attendance in attendanceDate)
            {
                var w = worker.FirstOrDefault(x => x.WorkerId == attendance.WorkerId);
                if (w != null)
                {
                    attendance.WorkerDocument = w.Worker.Document;
                    attendance.WorkerFullName = w.Worker.RawFullName;
                    attendance.WorkerCategory = ConstantHelpers.Worker.Category.VALUES[w.Category];
                }
            }

            return Ok(attendanceDate);
        }

        [HttpPut("tareo/editar/{id}")]
        public async Task<IActionResult> EditDailyTask(Guid id, [FromBody] WorkerDailyTaskResourceModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dailyTask = await _context.WorkerDailyTasks
                .FirstOrDefaultAsync(x => x.Id == id);

            dailyTask.HoursNormal = model.HoursNormal;
            dailyTask.Hours60Percent = model.Hours60Percent;
            dailyTask.Hours100Percent = model.Hours100Percent;
            dailyTask.ProjectPhaseId = model.ProjectPhaseId;

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
