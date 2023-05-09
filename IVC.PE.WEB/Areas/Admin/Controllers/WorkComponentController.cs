using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkComponentViewModels;
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
    [Route("admin/componente-de-obra")]
    public class WorkComponentController : BaseController
    {
                public WorkComponentController(IvcDbContext context,
    ILogger<WorkComponentController> logger)
    : base(context, logger)
        {
        }
        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var projectId = GetProjectId();

            var query = _context.WorkComponents
              .AsQueryable();

            var data = await query
                .Where(x => x.ProjectId == projectId)
                .Select(x => new WorkComponentViewModel
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
            var data = await _context.WorkComponents
                .Include(x => x.Project)
                 .Where(x => x.Id == id)
                 .Select(x => new WorkComponentViewModel
                 {
                     Id = x.Id,
                     Name = x.Name,
                     DocStyle = x.PillColor
                 }).FirstOrDefaultAsync();

            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(WorkComponentViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var workComponents = new WorkComponent
            {
                ProjectId = GetProjectId(),
                Name = model.Name,
                PillColor = model.DocStyle
            };

            await _context.WorkComponents.AddAsync(workComponents);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, WorkComponentViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var workComponents = await _context.WorkComponents
                .FirstOrDefaultAsync(x => x.Id == id);
            workComponents.Name = model.Name;
            workComponents.PillColor = model.DocStyle;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var workComponents = await _context.WorkComponents
                .FirstOrDefaultAsync(x => x.Id == id);
            if (workComponents == null)
                return BadRequest($"Componente con Id '{id}' no encontrado.");
            _context.WorkComponents.Remove(workComponents);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
