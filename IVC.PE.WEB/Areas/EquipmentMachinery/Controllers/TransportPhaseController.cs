using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.EquipmentMachinery;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectPhaseViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.TransportPhaseViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IVC.PE.WEB.Areas.EquipmentTransport.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.EquipmentMachinery.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.EQUIPMENT_MACHINERY)]
    [Route("equipos/fase-transporte")]
    public class TransportPhaseController : BaseController
    {

        public TransportPhaseController(IvcDbContext context,
        ILogger<TransportPhaseController> logger) : base(context, logger)
        {

        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {

            var query = _context.TransportPhases
              .AsQueryable();

            var data = await query
                .Include(x => x.ProjectPhase)
                .Where(x => x.ProjectId == GetProjectId())
                .Select(x => new TransportPhaseViewModel
                {
                    Id = x.Id,
                    ProjectPhaseId = x.ProjectPhaseId,
                    ProjectPhase = new ProjectPhaseViewModel
                    {
                        Code = x.ProjectPhase.Code,
                        Description = x.ProjectPhase.Description
                    },
                    ProjectId = x.ProjectId

                })
                .ToListAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.TransportPhases

                 .Where(x => x.Id == id)
                 .Select(x => new TransportPhaseViewModel
                 {
                     Id = x.Id,
                     ProjectPhaseId = x.ProjectPhaseId,

                 }).FirstOrDefaultAsync();

            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(TransportPhaseViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var equipmentTransportType = new TransportPhase
            {

                ProjectPhaseId = model.ProjectPhaseId,
                ProjectId = GetProjectId()

            };

            await _context.TransportPhases.AddAsync(equipmentTransportType);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, TransportPhaseViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var equipmentTransportTypes = await _context.TransportPhases
                .FirstOrDefaultAsync(x => x.Id == id);
            equipmentTransportTypes.ProjectPhaseId = model.ProjectPhaseId;


            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var equipmentTransportTypes = await _context.TransportPhases
                .FirstOrDefaultAsync(x => x.Id == id);
            if (equipmentTransportTypes == null)
                return BadRequest($"Tipo de certificado con Id '{id}' no encontrado.");
            _context.TransportPhases.Remove(equipmentTransportTypes);
            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}
