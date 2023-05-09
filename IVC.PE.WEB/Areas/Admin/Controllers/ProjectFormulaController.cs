using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectFormulaViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IVC.PE.WEB.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.Roles.SUPERADMIN)]
    [Area(ConstantHelpers.Areas.ADMIN)]
    [Route("admin/formulas")]
    public class ProjectFormulaController : BaseController
    {
        public ProjectFormulaController(IvcDbContext context,
            ILogger<ProjectFormulaController> logger)
            : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var formulas = await _context.ProjectFormulas
                .Where(x => x.ProjectId == GetProjectId())
                .Select(x => new ProjectFormulaViewModel
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    Group = x.Group
                }).ToListAsync();

            return Ok(formulas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var formula = await _context.ProjectFormulas
                .Where(x => x.Id == id)
                .Select(x => new ProjectFormulaViewModel
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    Group = x.Group
                }).FirstOrDefaultAsync();

            return Ok(formula);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(ProjectFormulaViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var formula = new ProjectFormula
            {
                ProjectId = GetProjectId(),
                Code = model.Code,
                Name = model.Name,
                Group = model.Group
            };

            await _context.ProjectFormulas.AddAsync(formula);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, ProjectFormulaViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var formula = await _context.ProjectFormulas
                .FirstOrDefaultAsync(x => x.Id == id);

            formula.Code = model.Code;
            formula.Name = model.Name;
            formula.Group = model.Group;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var formula = await _context.ProjectFormulas
                .FirstOrDefaultAsync(x => x.Id == id);

            _context.ProjectFormulas.Remove(formula);
            await _context.SaveChangesAsync();
            
            return Ok();
        }
    }
}
