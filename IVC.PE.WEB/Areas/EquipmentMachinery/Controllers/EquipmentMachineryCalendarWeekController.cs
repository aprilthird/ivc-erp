using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.EquipmentMachinery;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryCalendarWeekViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static IVC.PE.CORE.Helpers.ConstantHelpers;

namespace IVC.PE.WEB.Areas.EquipmentMachinery.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.EquipmentMachinery.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.EQUIPMENT_MACHINERY)]
    [Route("equipos/calendario-equipos")]
    public class EquipmentMachineryCalendarWeekController : BaseController
    {
        public EquipmentMachineryCalendarWeekController(IvcDbContext context,
ILogger<EquipmentMachineryCalendarWeekController> logger) : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {

            var query = _context.EquipmentMachineryCalendarWeeks
              .AsQueryable();
            var pId = GetProjectId();
            var data = await query
                .Select(x => new EquipmentMachineryCalendarWeekViewModel
                {
                    Id = x.Id,
                    Description = "Semana " + x.WeekNumber + "-" + x.Year,
                    WeekNumber = x.WeekNumber,
                    WeekStart = x.WeekStart.ToDateString(),
                    WeekEnd = x.WeekEnd.ToDateString(),
                    Year = x.Year,
                })
                .OrderByDescending(x=>x.Description)
                .ToListAsync();
            return Ok(data);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.EquipmentMachineryCalendarWeeks

                 .Where(x => x.Id == id)
                 .Select(x => new EquipmentMachineryCalendarWeekViewModel
                 {
                     Id = x.Id,
                     WeekNumber = x.WeekNumber,
                     WeekStart = x.WeekStart.ToDateString(),
                     WeekEnd = x.WeekEnd.ToDateString(),
                     Year = x.Year,
                 }).FirstOrDefaultAsync();

            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(EquipmentMachineryCalendarWeekViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var calendar = new EquipmentMachineryCalendarWeek
            {

                WeekNumber = model.WeekNumber,
                WeekStart = model.WeekStart.ToDateTime(),
                WeekEnd = model.WeekEnd.ToDateTime(),
                Year = model.Year,
            };

            await _context.EquipmentMachineryCalendarWeeks.AddAsync(calendar);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, EquipmentMachineryCalendarWeekViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var calendar = await _context.EquipmentMachineryCalendarWeeks
                .FirstOrDefaultAsync(x => x.Id == id);

                calendar.WeekNumber = model.WeekNumber;
                calendar.WeekStart = model.WeekStart.ToDateTime();
                calendar.WeekEnd = model.WeekEnd.ToDateTime();
                calendar.Year = model.Year;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var equipmentMachineryTypes = await _context.EquipmentMachineryCalendarWeeks
                .FirstOrDefaultAsync(x => x.Id == id);
            if (equipmentMachineryTypes == null)
                return BadRequest($"Calendario con Id '{id}' no encontrado.");
            _context.EquipmentMachineryCalendarWeeks.Remove(equipmentMachineryTypes);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
