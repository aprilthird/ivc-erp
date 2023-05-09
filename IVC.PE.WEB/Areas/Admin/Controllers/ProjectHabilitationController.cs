using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IVC.PE.WEB.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.Roles.SUPERADMIN)]
    [Area(ConstantHelpers.Areas.ADMIN)]
    [Route("admin/habilitaciones-proyecto")]
    public class ProjectHabilitationController : BaseController
    {
        public ProjectHabilitationController(IvcDbContext context,
            ILogger<ProjectHabilitationController> logger)
            : base(context, logger)
        {
        }

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? projectId = null)
        {
            if (!projectId.HasValue)
                return Ok(new List<ProjectHabilitationViewModel>());

            var habs = await _context.ProjectHabilitations
                .Where(x => x.ProjectId == projectId)
                .Select(x => new ProjectHabilitationViewModel
                {
                    Id = x.Id,
                    ProjectId = x.ProjectId,
                    LocationCode = x.LocationCode,
                    Description = x.Description
                }).ToListAsync();

            return Ok(habs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var hab = await _context.ProjectHabilitations
                .Where(x => x.Id == id)
                .Select(x => new ProjectHabilitationViewModel
                {
                    Id = x.Id,
                    ProjectId = x.ProjectId,
                    LocationCode = x.LocationCode,
                    Description = x.Description
                }).FirstOrDefaultAsync();

            return Ok(hab);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(ProjectHabilitationViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var hab = new ProjectHabilitation
            {
                ProjectId = model.ProjectId,
                LocationCode = model.LocationCode,
                Description = model.Description
            };

            await _context.ProjectHabilitations.AddAsync(hab);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, ProjectHabilitationViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var hab = await _context.ProjectHabilitations
                .FirstOrDefaultAsync(x => x.Id == id);

            hab.LocationCode = model.LocationCode;
            hab.Description = model.Description;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var hab = await _context.ProjectHabilitations
                .FirstOrDefaultAsync(x => x.Id == id);

            _context.ProjectHabilitations.Remove(hab);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
