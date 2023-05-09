using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.EquipmentMachinery;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTypeSoftViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTypeTransportViewModels;
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
    [Route("equipos/actividades-tipo-de-equipo-transporte")]

    public class EquipmentMachineryTypeTransportActivityController : BaseController
    {
        public EquipmentMachineryTypeTransportActivityController(IvcDbContext context,
ILogger<EquipmentMachineryTypeTransportActivityController> logger)
: base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? equipmentMachineryId = null)
        {
            if (!equipmentMachineryId.HasValue)
                return Ok(new List<EquipmentMachineryTypeTransportActivityViewModel>());

            var activities = await _context.EquipmentMachineryTypeTransportActivities
                .Where(x => x.EquipmentMachineryTypeTransportId == equipmentMachineryId.Value)
                .Select(x => new EquipmentMachineryTypeTransportActivityViewModel
                {
                    Id = x.Id,
                    Description = x.Description
                }).ToListAsync();

            return Ok(activities);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var activity = await _context.EquipmentMachineryTypeTransportActivities
                .Where(x => x.Id == id)
                .Select(x => new EquipmentMachineryTypeTransportActivityViewModel
                {
                    Id = x.Id,
                    Description = x.Description,
                    EquipmentMachineryTypeTransportId = x.EquipmentMachineryTypeTransportId
                }).FirstOrDefaultAsync();

            return Ok(activity);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(EquipmentMachineryTypeTransportActivityViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var activity = new EquipmentMachineryTypeTransportActivity
            {
                EquipmentMachineryTypeTransportId = model.EquipmentMachineryTypeTransportId.Value,
                Description = model.Description
            };

            await _context.EquipmentMachineryTypeTransportActivities.AddAsync(activity);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, EquipmentMachineryTypeTransportActivityViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var activity = await _context.EquipmentMachineryTypeTransportActivities
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

            var activity = await _context.EquipmentMachineryTypeTransportActivities
                .FirstOrDefaultAsync(x => x.Id == id);

            _context.EquipmentMachineryTypeTransportActivities.Remove(activity);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
