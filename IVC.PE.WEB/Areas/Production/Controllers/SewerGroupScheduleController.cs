using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Production;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.SewerGroupViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.UserViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontHeadViewModels;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.ProjectCalendarWeekViewModels;
using IVC.PE.WEB.Areas.Production.ViewModels.SewerGroupScheduleViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IVC.PE.WEB.Areas.Production.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Production.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.PRODUCTION)]
    [Route("produccion/programaciones")]
    public class SewerGroupScheduleController : BaseController
    {
        public SewerGroupScheduleController(IvcDbContext context,
            ILogger<SewerGroupScheduleController> logger)
            : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? weekId, Guid? workFrontHeadId = null, Guid? sewerGroupId = null)
        {
            var schedules = await _context.SewerGroupSchedules
                .Include(x => x.ProjectCalendarWeek)
                .Include(x => x.SewerGroup)
                .Include(x => x.WorkFrontHead.User)
                .Select(x => new SewerGroupScheduleViewModel
                {
                    Id = x.Id,
                    ProjectCalendarWeekId = x.ProjectCalendarWeekId,
                    ProjectCalendarWeek = new ProjectCalendarWeekViewModel
                    {
                        YearWeekNumber = x.ProjectCalendarWeek.YearWeekNumber
                    },
                    SewerGroupId = x.SewerGroupId,
                    SewerGroup = new SewerGroupViewModel
                    {
                        Code = x.SewerGroup.Code
                    },
                    WorkFrontHeadId = x.WorkFrontHeadId,
                    WorkFrontHead = new WorkFrontHeadViewModel
                    {
                        User = new UserViewModel
                        {
                            PaternalSurname = x.WorkFrontHead.User.PaternalSurname,
                            MaternalSurname = x.WorkFrontHead.User.MaternalSurname,
                            Name = x.WorkFrontHead.User.Name
                        }
                    },
                    IsIssued = x.isIssued
                }).ToListAsync();

            var activities = await _context.SewerGroupScheduleActivities
                .Include(x => x.SewerManifold)
                .ToListAsync();

            foreach (var schedule in schedules)
            {
                schedule.Description = string.Join(", ",
                    activities.Where(x => x.SewerGroupDailyScheduleId == schedule.Id)
                        .Select(x => x.SewerManifold.Name)
                        .ToList());
            }

            if (weekId.HasValue)
                schedules = schedules.Where(x => x.ProjectCalendarWeekId == weekId.Value).ToList();

            if (workFrontHeadId.HasValue)
                schedules = schedules.Where(x => x.WorkFrontHeadId == workFrontHeadId.Value).ToList();

            if (sewerGroupId.HasValue)
                schedules = schedules.Where(x => x.SewerGroupId == sewerGroupId.Value).ToList();

            return Ok(schedules);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var schedule = await _context.SewerGroupSchedules
                .Include(x => x.SewerGroup)
                .Where(x => x.Id == id)
                .Select(x => new SewerGroupScheduleViewModel
                {
                    Id = x.Id,
                    ProjectCalendarWeekId = x.ProjectCalendarWeekId,
                    SewerGroupId = x.SewerGroupId,
                    WorkFrontHeadId = x.WorkFrontHeadId,
                    IsIssued = x.isIssued
                }).FirstOrDefaultAsync();

            return Ok(schedule);
        }

        [HttpGet("formula-check/{id}")]
        public async Task<IActionResult> GetFormulaCheck(Guid id)
        {
            var schedule = await _context.SewerGroupSchedules
               .Include(x => x.SewerGroup)
               .Where(x => x.Id == id)
               .Select(x => new SewerGroupScheduleViewModel
               {
                   Id = x.Id,
                   ProjectCalendarWeekId = x.ProjectCalendarWeekId,
                   SewerGroupId = x.SewerGroupId,
                   WorkFrontHeadId = x.WorkFrontHeadId,
                   IsIssued = x.isIssued
               }).FirstOrDefaultAsync();

            var formula = await _context.ProjectFormulaSewerGroups
                .Include(x => x.ProjectFormula)
                .Where(x => x.SewerGroupId == schedule.SewerGroupId)
                .ToListAsync();

            var fs = $"F{string.Join("/", formula.Select(x => x.ProjectFormula.Code.Last()))}";

            return Ok(new
            {
                schedule,
                fs
            });
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(SewerGroupScheduleViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var schedule = new SewerGroupSchedule
            {
                ProjectCalendarWeekId = model.ProjectCalendarWeekId,
                WorkFrontHeadId = model.WorkFrontHeadId,
                SewerGroupId = model.SewerGroupId,
                isIssued = false
            };

            await _context.SewerGroupSchedules.AddAsync(schedule);
            await _context.SaveChangesAsync();

            return Ok(schedule.Id);
        }

        [HttpPut("emitir/{id}")]
        public async Task<IActionResult> Issued(Guid id, bool issued)
        {
            var schedule = await _context.SewerGroupSchedules
                .FirstOrDefaultAsync(x => x.Id == id);

            schedule.isIssued = issued;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var schedule = await _context.SewerGroupSchedules
                .FirstOrDefaultAsync(x => x.Id == id);

            _context.SewerGroupSchedules.Remove(schedule);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
