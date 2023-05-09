using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.DocumentaryControl;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using IVC.PE.WEB.Areas.DocumentaryControl.ViewModels.WorkbookViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.DocumentaryControl.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.DocumentaryControl.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.DOCUMENTARY_CONTROL)]
    [Route("control-documentario/tipo-libros-de-obra")]
    public class WorkbookTypeController : BaseController
    {

        public WorkbookTypeController(IvcDbContext context,
        ILogger<WorkbookTypeController> logger) : base(context, logger)
        {
        }
        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var projectId = GetProjectId();

            var query = _context.WorkbookTypes
              .AsQueryable();

            var data = await query
                .Where(x => x.ProjectId == projectId)
                .Select(x => new WorkbookTypeViewModel
                {
                    Id = x.Id,
                    Description = x.Description,
                    ProjectId = x.ProjectId,
                    Project = new ProjectViewModel
                    {
                        Abbreviation = x.Project.Abbreviation
                    },
                    Color = x.PillColor
                })
                .ToListAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.WorkbookTypes
                 .Include(x => x.Project)
                 .Where(x => x.Id == id)
                 .Select(x => new WorkbookTypeViewModel
                 {
                     Id = x.Id,
                     Description = x.Description,
                     Color = x.PillColor
                 }).FirstOrDefaultAsync();

            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(WorkbookTypeViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var wbType = new WorkbookType
            {
                ProjectId = GetProjectId(),
                Description = model.Description,
                PillColor = model.Color
            };

            await _context.WorkbookTypes.AddAsync(wbType);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, WorkbookTypeViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var wbType = await _context.WorkbookTypes
                .FirstOrDefaultAsync(x => x.Id == id);
            wbType.Description = model.Description;
            wbType.PillColor = model.Color;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var wbType = await _context.WorkbookTypes
                .FirstOrDefaultAsync(x => x.Id == id);
            if (wbType == null)
                return BadRequest($"Tipo de libro con Id '{id}' no encontrado.");
            _context.WorkbookTypes.Remove(wbType);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
