using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.HumanResources;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.PayrollViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.HumanResources.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.HumanResources.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.HUMAN_RESOURCES)]
    [Route("recursos-humanos/calendario-semanal")]
    public class PayrollCalendarController : BaseController
    {
        public PayrollCalendarController(IvcDbContext context)
            : base(context)
        {
        }

        public IActionResult Index() => View();

        [HttpPost("generar")]
        public async Task<IActionResult> Generate([Bind(include: "Year,IsWeekly,FirstDayOfThCalendar,ProjectId")] PayrollCalendarViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var projectCalendar = new ProjectCalendar
            {
                ProjectId = model.ProjectId,
                Year = model.Year,
                IsWeekly = model.IsWeekly,
                FirstDayOfThCalendar = model.FirstDayOfThCalendar.ToDateTime()
            };

            await _context.ProjectCalendars.AddAsync(projectCalendar);

            var projectCalendarWeeks = GenerateWeeks(projectCalendar);

            await _context.ProjectCalendarWeeks.AddRangeAsync(projectCalendarWeeks);

            await _context.SaveChangesAsync();

            return Ok();
        }

        private IEnumerable<ProjectCalendarWeek> GenerateWeeks(ProjectCalendar projectCalendar)
        {
            List<ProjectCalendarWeek> weeks = new List<ProjectCalendarWeek>();
            DateTime startingDayOfTheWeek = projectCalendar.FirstDayOfThCalendar.Value;
            DateTime finalDayOfTheWeek = startingDayOfTheWeek.AddDays(6);

            int weekNumber = 1;
            do
            {
                var newWeek = new ProjectCalendarWeek
                {
                    ProjectCalendarId = projectCalendar.Id,
                    WeekNumber = weekNumber,
                    YearWeekNumber = String.Format("{0}-S{1:D2}", projectCalendar.Year, weekNumber),
                    ProcessType = 0,
                    WeekStart = startingDayOfTheWeek,
                    WeekEnd = finalDayOfTheWeek,
                    Month = GetWeekMonth(startingDayOfTheWeek, finalDayOfTheWeek),
                    Description = String.Format("Semana {0:D2}-{1}", weekNumber, projectCalendar.Year),
                    Year = projectCalendar.Year,
                    IsClosed = false
                };
                weeks.Add(newWeek);
                weekNumber++;
                startingDayOfTheWeek = startingDayOfTheWeek.AddDays(7);
                finalDayOfTheWeek = finalDayOfTheWeek.AddDays(7);

            } while (GetWeekYear(startingDayOfTheWeek, finalDayOfTheWeek) == projectCalendar.Year);

            return weeks;
        }

        private int GetWeekMonth(DateTime startingDayOfTheWeek, DateTime finalDayOfTheWeek)
        {
            if (startingDayOfTheWeek.Month != finalDayOfTheWeek.Month)
            {
                var lastMonthDay = DateTime.DaysInMonth(startingDayOfTheWeek.Year, startingDayOfTheWeek.Month);
                var daysDifToStart = lastMonthDay - startingDayOfTheWeek.Day + 1;
                if (daysDifToStart < 4)
                {
                    return finalDayOfTheWeek.Month;
                }
            }

            return startingDayOfTheWeek.Month;
        }

        private int GetWeekYear(DateTime startingDayOfTheWeek, DateTime finalDayOfTheWeek)
        {
            if (startingDayOfTheWeek.Month != finalDayOfTheWeek.Month)
            {
                var lastMonthDay = DateTime.DaysInMonth(startingDayOfTheWeek.Year, startingDayOfTheWeek.Month);
                var daysDifToStart = lastMonthDay - startingDayOfTheWeek.Day + 1;
                if (daysDifToStart < 4)
                {
                    return finalDayOfTheWeek.Year;
                }
            }

            return startingDayOfTheWeek.Year;
        }

        [HttpGet("listar")]
        public async Task<IActionResult> GetProjectCalendar(Guid? projectId = null, int? year = null)
        {
            if (projectId == null)
            {
                projectId = (await _context.Projects.FirstOrDefaultAsync()).Id;
            }

            var projectCalendar = await _context.ProjectCalendars
                .Where(x => x.ProjectId.Equals(projectId) && x.Year.Equals(year) && x.IsWeekly)
                .FirstOrDefaultAsync();

            if (projectCalendar == null)
            {
                return Ok(new List<PayrollCalendarWeekViewModel>());
            }

            var projectCalendarWeeks = await _context.ProjectCalendarWeeks
            .Where(x => x.ProjectCalendarId.Equals(projectCalendar.Id))
            .Select(x => new PayrollCalendarWeekViewModel
            {
                Description = x.Description,
                Id = x.Id,
                IsClosed = x.IsClosed,
                Month = x.Month,
                ProcessType = x.ProcessType,
                ProjectCalendarId = x.ProjectCalendarId,
                WeekEnd = x.WeekEnd.ToDateString(),
                WeekNumber = x.WeekNumber,
                WeekStart = x.WeekStart.ToDateString(),
                YearWeekNumber = x.YearWeekNumber
            }).ToListAsync();

            return Ok(projectCalendarWeeks);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWeek(Guid id)
        {
            var week = await _context.ProjectCalendarWeeks
                .Include(x => x.ProjectCalendar)
                .Where(x => x.Id.Equals(id))
                .Select(x => new PayrollCalendarWeekViewModel
                {
                    Id = x.Id,
                    Description = x.Description,
                    IsClosed = x.IsClosed,
                    Month = x.Month,
                    ProcessType = x.ProcessType,
                    ProjectCalendarId = x.ProjectCalendarId,
                    WeekEnd = x.WeekEnd.ToDateString(),
                    WeekNumber = x.WeekNumber,
                    WeekStart = x.WeekStart.ToDateString(),
                    YearWeekNumber = x.YearWeekNumber
                }).FirstOrDefaultAsync();

            //var model = await _context.ProjectCalendars
            //    .Where(x => x.Id == week.ProjectCalendarId)
            //    .Select(x => new PayrollCalendarViewModel
            //    {
            //        Id = x.Id,
            //        IsWeekly = x.IsWeekly,
            //        Year = x.Year,
            //        ProjectId = x.ProjectId
            //    }).FirstOrDefaultAsync();

            //model.ProjectCalendarWeek = week;
            return Ok(week);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(PayrollCalendarWeekViewModel model)
        {
            var projectCalendar = await _context.ProjectCalendars
                .Where(x => x.Id == model.ProjectCalendarId)
                .FirstOrDefaultAsync();

            if (projectCalendar == null)
            {
                return BadRequest("No existe calendario");
            }

            var newWeek = new ProjectCalendarWeek
            {
                ProjectCalendarId = projectCalendar.Id,
                WeekNumber = model.WeekNumber,
                YearWeekNumber = String.IsNullOrEmpty(model.YearWeekNumber) ?
                                    model.ProcessType == 0 ?
                                        String.Format("{0}-S{1:D2}", projectCalendar.Year, model.WeekNumber)
                                        : model.ProcessType == 1 ?
                                            String.Format("{0}-S{1:D2}G", projectCalendar.Year, model.WeekNumber)
                                            : String.Format("{0}-S{1:D2}R", projectCalendar.Year, model.WeekNumber)
                                    : model.YearWeekNumber,
                ProcessType = model.ProcessType,
                WeekStart = model.WeekStart.ToDateTime(),
                WeekEnd = model.WeekEnd.ToDateTime(),
                Month = model.Month <= 0 ? GetWeekMonth(model.WeekStart.ToDateTime(), model.WeekEnd.ToDateTime()) : model.Month,
                Description = String.IsNullOrEmpty(model.Description) ? String.Format("Semana {0:D2}-{1}", model.WeekNumber, projectCalendar.Year) : model.Description
            };

            await _context.ProjectCalendarWeeks.AddAsync(newWeek);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, PayrollCalendarWeekViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var week = await _context.ProjectCalendarWeeks.FirstOrDefaultAsync(x => x.Id.Equals(id));
            week.Description = model.Description;
            week.IsClosed = model.IsClosed;
            week.WeekNumber = model.WeekNumber;
            week.YearWeekNumber = model.YearWeekNumber;
            week.ProcessType = model.ProcessType;
            week.Month = model.Month;
            week.WeekStart = model.WeekStart.ToDateTime();
            week.WeekEnd = model.WeekEnd.ToDateTime();

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var week = await _context.ProjectCalendarWeeks.FirstOrDefaultAsync(x => x.Id.Equals(id));

            var header = await _context.PayrollMovementHeaders
                .CountAsync(x => x.ProjectCalendarWeekId == id);
            if (header > 0)
                return BadRequest("La semana tiene información.");

            _context.ProjectCalendarWeeks.Remove(week);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
