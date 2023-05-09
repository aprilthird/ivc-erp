using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectFormulaSewerGroupViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.SewerGroupViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IVC.PE.WEB.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.Roles.SUPERADMIN)]
    [Area(ConstantHelpers.Areas.ADMIN)]
    [Route("admin/cuadrillas-formula")]
    public class ProjectFormulaSewerGroupController : BaseController
    {
        public ProjectFormulaSewerGroupController(IvcDbContext context,
            ILogger<ProjectFormulaSewerGroupController> logger)
            : base(context, logger)
        {
        }

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? formulaId = null)
        {
            if (!formulaId.HasValue)
                return Ok(new List<ProjectFormulaSewerGroupViewModel>());

            var sewergroups = await _context.ProjectFormulaSewerGroups
                .Include(x => x.SewerGroup)
                .Where(x => x.ProjectFormulaId == formulaId.Value)
                .Select(x => new ProjectFormulaSewerGroupViewModel
                {
                    Id = x.Id,
                    ProjectFormulaId = x.ProjectFormulaId,
                    SewerGroupId = x.SewerGroupId,
                    SewerGroup = new SewerGroupViewModel
                    {
                        Code = x.SewerGroup.Code
                    }
                }).ToListAsync();

            return Ok(sewergroups);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var sewergroups = await _context.ProjectFormulaSewerGroups
                .Where(x => x.ProjectFormulaId == id)
                .Select(x => x.SewerGroupId)
                .ToListAsync();

            return Ok(new
            {
                FormulaId = id,
                SewerGroupIds = sewergroups
            });
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(ProjectFormulaSewerGroupViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var sgs = await _context.ProjectFormulaSewerGroups
                .Where(x => x.ProjectFormulaId == model.ProjectFormulaId)
                .ToListAsync();

            _context.ProjectFormulaSewerGroups.RemoveRange(sgs);
            await _context.ProjectFormulaSewerGroups.AddRangeAsync(
                model.SewerGroupIds.Select(x => new ProjectFormulaSewerGroup
                {
                    ProjectFormulaId = model.ProjectFormulaId.Value,
                    SewerGroupId = x
                }).ToList());
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var sewergroup = await _context.ProjectFormulaSewerGroups
                .FirstOrDefaultAsync(x => x.Id == id);

            _context.ProjectFormulaSewerGroups.Remove(sewergroup);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
