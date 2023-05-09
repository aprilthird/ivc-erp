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
    [Route("admin/capataces")]
    public class ForemanController : BaseController
    {
        public ForemanController(IvcDbContext context,
            ILogger<ForemanController> logger)
            : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.Projects
                .Select(x => new ProjectViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .AsNoTracking()
                .ToListAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var project = await _context.Projects
                .Where(x => x.Id == id)
                .Select(x => new ProjectViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).FirstOrDefaultAsync();
            return Ok(project);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(ProjectViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var project = new Project
            {
                Name = model.Name
            };
            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar")]
        public async Task<IActionResult> Edit(Guid id, ProjectViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var project = await _context.Projects.FindAsync(id);
            project.Name = model.Name;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var bank = await _context.Projects.FirstOrDefaultAsync(x => x.Id == id);
            if (bank == null)
                return BadRequest($"Proyecto con Id '{id}' no encontrado.");
            _context.Projects.Remove(bank);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}