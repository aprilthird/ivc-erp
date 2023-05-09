using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.EquipmentMachinery;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTypeTypeViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.EquipmentMachinery.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.EquipmentMachinery.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.EQUIPMENT_MACHINERY)]
    [Route("equipos/actividades-de-maquinaria")]
    public class EquipmentMachineryTypeTypeActivityController : BaseController
    {
        public EquipmentMachineryTypeTypeActivityController(IvcDbContext context,
    ILogger<EquipmentMachineryTypeTypeActivityController> logger)
    : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? equipmentMachineryId = null)
        {
            if (!equipmentMachineryId.HasValue)
                return Ok(new List<EquipmentMachineryTypeTypeActivityViewModel>());

            var activities = await _context.EquipmentMachineryTypeTypeActivities
                .Where(x => x.EquipmentMachineryTypeTypeId == equipmentMachineryId.Value)
                .Select(x => new EquipmentMachineryTypeTypeActivityViewModel
                {
                    Id = x.Id,
                    Description = x.Description
                }).ToListAsync();

            return Ok(activities);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var activity = await _context.EquipmentMachineryTypeTypeActivities
                .Where(x => x.Id == id)
                .Select(x => new EquipmentMachineryTypeTypeActivityViewModel
                {
                    Id = x.Id,
                    Description = x.Description,
                    EquipmentMachineryTypeTypeId = x.EquipmentMachineryTypeTypeId
                }).FirstOrDefaultAsync();

            return Ok(activity);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(EquipmentMachineryTypeTypeActivityViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var activity = new EquipmentMachineryTypeTypeActivity
            {
                EquipmentMachineryTypeTypeId = model.EquipmentMachineryTypeTypeId.Value,
                Description = model.Description
            };

            await _context.EquipmentMachineryTypeTypeActivities.AddAsync(activity);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, EquipmentMachineryTypeTypeActivityViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var activity = await _context.EquipmentMachineryTypeTypeActivities
                .FirstOrDefaultAsync(x => x.Id == id);

            activity.Description = model.Description;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var activity = await _context.EquipmentMachineryTypeTypeActivities
                .FirstOrDefaultAsync(x => x.Id == id);

            _context.EquipmentMachineryTypeTypeActivities.Remove(activity);
            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
