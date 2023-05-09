using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkStructureViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.Roles.SUPERADMIN)]
    [Area(ConstantHelpers.Areas.ADMIN)]
    [Route("admin/estructura-de-obra")]
    public class WorkStructureController : BaseController
    {
        public WorkStructureController(IvcDbContext context, ILogger<WorkStructureController> logger)
            : base(context, logger)
        {
        }
        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var projectId = GetProjectId();

            var query = _context.WorkStructures
              .AsQueryable();

            var data = await query
                .Where(x => x.ProjectId == projectId)
                .Select(x => new WorkStructureViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    ProjectId = x.ProjectId,
                    ProjectViewModel = new ProjectViewModel
                    {
                        Abbreviation = x.Project.Abbreviation
                    },
                    DocStyle = x.PillColor
                })
                .ToListAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.WorkStructures
                .Include(x => x.Project)
                 .Where(x => x.Id == id)
                 .Select(x => new WorkStructureViewModel
                 {
                     Id = x.Id,
                     Name = x.Name,
                     DocStyle = x.PillColor
                 }).FirstOrDefaultAsync();

            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(WorkStructureViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var workStructures = new WorkStructure
            {
                ProjectId = GetProjectId(),
                Name = model.Name,
                PillColor = model.DocStyle
            };

            await _context.WorkStructures.AddAsync(workStructures);
            await _context.SaveChangesAsync();
            return Ok();
        }
            
        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, WorkStructureViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var workStructures = await _context.WorkStructures
                .FirstOrDefaultAsync(x => x.Id == id);
            workStructures.Name = model.Name;
            workStructures.PillColor = model.DocStyle;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var workStructures = await _context.WorkStructures
                .FirstOrDefaultAsync(x => x.Id == id);
            if (workStructures == null)
                return BadRequest($"Estructura con Id '{id}' no encontrado.");
            _context.WorkStructures.Remove(workStructures);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
