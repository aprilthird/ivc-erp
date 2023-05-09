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
    [Route("calidad/entidad-certificadora")]
    public class EquipmentCertifyingEntityController : BaseController
    {
        public EquipmentCertifyingEntityController(IvcDbContext context,
        ILogger<EquipmentCertifyingEntityController> logger) : base(context, logger)
        {
        }
        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var projectId = GetProjectId();

            var query = _context.EquipmentCertifyingEntities
              .AsQueryable();

            var data = await query
                .Where(x => x.ProjectId == projectId)
                .Select(x => new EquipmentCertifyingEntityViewModel
                {
                    Id = x.Id,
                    CertifyingEntityName = x.CertifyingEntityName,
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
            var data = await _context.EquipmentCertifyingEntities
                 .Include(x => x.Project)
                 .Where(x => x.Id == id)
                 .Select(x => new EquipmentCertifyingEntityViewModel
                 {
                     Id = x.Id,
                     CertifyingEntityName = x.CertifyingEntityName,
                 }).FirstOrDefaultAsync();

            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(EquipmentCertifyingEntityViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var equipmentCertifyingEntities = new EquipmentCertifyingEntity
            {
                ProjectId = GetProjectId(),
                CertifyingEntityName = model.CertifyingEntityName,
            };

            await _context.EquipmentCertifyingEntities.AddAsync(equipmentCertifyingEntities);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, EquipmentCertifyingEntityViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var equipmentCertifyingEntities = await _context.EquipmentCertifyingEntities
                .FirstOrDefaultAsync(x => x.Id == id);
            equipmentCertifyingEntities.CertifyingEntityName = model.CertifyingEntityName;
            

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var equipmentCertifyingEntities = await _context.EquipmentCertifyingEntities
                .FirstOrDefaultAsync(x => x.Id == id);
            if (equipmentCertifyingEntities == null)
                return BadRequest($"Entidad con Id '{id}' no encontrado.");
            _context.EquipmentCertifyingEntities.Remove(equipmentCertifyingEntities);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
