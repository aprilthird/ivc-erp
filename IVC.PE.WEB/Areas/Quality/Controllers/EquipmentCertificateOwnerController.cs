using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Quality;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using IVC.PE.WEB.Areas.Quality.ViewModels.EquipmentCertificateViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Quality.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Quality.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.QUALITY)]
    [Route("calidad/propietario")]
    public class EquipmentCertificateOwnerController : BaseController
    {
        public EquipmentCertificateOwnerController(IvcDbContext context,
        ILogger<EquipmentCertificateOwnerController> logger) : base(context, logger)
        {
        }
        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var projectId = GetProjectId();

            var query = _context.EquipmentOwners
              .AsQueryable();

            var data = await query
                .Where(x => x.ProjectId == projectId)
                .Select(x => new EquipmentCertificateOwnerViewModel
                {
                    Id = x.Id,
                    OwnerName = x.OwnerName,
                    ProjectId = x.ProjectId,
                    Project = new ProjectViewModel
                    {
                        Abbreviation = x.Project.Abbreviation
                    },
                })
                .ToListAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.EquipmentOwners
                 .Include(x => x.Project)
                 .Where(x => x.Id == id)
                 .Select(x => new EquipmentCertificateOwnerViewModel
                 {
                     Id = x.Id,
                     OwnerName = x.OwnerName,
                 }).FirstOrDefaultAsync();

            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(EquipmentCertificateOwnerViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var equipmentCertificateOwner = new EquipmentCertificateOwner
            {
                ProjectId = GetProjectId(),
                OwnerName = model.OwnerName
            };

            await _context.EquipmentOwners.AddAsync(equipmentCertificateOwner);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, EquipmentCertificateOwnerViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var equipmentCertificateOwners = await _context.EquipmentOwners
                .FirstOrDefaultAsync(x => x.Id == id);
            equipmentCertificateOwners.OwnerName = model.OwnerName;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var equipmentCertificateOwners = await _context.EquipmentOwners
                .FirstOrDefaultAsync(x => x.Id == id);
            if (equipmentCertificateOwners == null)
                return BadRequest($"Propietario con Id '{id}' no encontrado.");
            _context.EquipmentOwners.Remove(equipmentCertificateOwners);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
