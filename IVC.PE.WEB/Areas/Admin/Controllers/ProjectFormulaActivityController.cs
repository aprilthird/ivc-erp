using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectFormulaViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.MeasurementUnitViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IVC.PE.WEB.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.Roles.SUPERADMIN)]
    [Area(ConstantHelpers.Areas.ADMIN)]
    [Route("admin/actividades-formula")]
    public class ProjectFormulaActivityController : BaseController
    {
        public ProjectFormulaActivityController(IvcDbContext context,
            ILogger<ProjectFormulaActivityController> logger)
            : base(context, logger)
        {
        }

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? formulaId = null)
        {
            if (!formulaId.HasValue)
                return Ok(new List<ProjectFormulaActivityViewModel>());

            var activities = await _context.ProjectFormulaActivities
                .Include(x => x.MeasurementUnit)
                .Where(x => x.ProjectFormulaId == formulaId.Value)
                .Select(x => new ProjectFormulaActivityViewModel
                {
                    Id = x.Id,
                    Description = x.Description,
                    MeasurementUnitId = x.MeasurementUnitId,
                    MeasurementUnit = new MeasurementUnitViewModel
                    {
                        Abbreviation = x.MeasurementUnit.Abbreviation,
                        Name = x.MeasurementUnit.Name
                    }
                }).ToListAsync();

            return Ok(activities);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var activity = await _context.ProjectFormulaActivities
                .Where(x => x.Id == id)
                .Select(x => new ProjectFormulaActivityViewModel
                {
                    Id = x.Id,
                    Description = x.Description,
                    ProjectFormulaId = x.ProjectFormulaId,
                    MeasurementUnitId = x.MeasurementUnitId
                }).FirstOrDefaultAsync();

            return Ok(activity);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(ProjectFormulaActivityViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var activity = new ProjectFormulaActivity
            {
                ProjectFormulaId = model.ProjectFormulaId.Value,
                Description = model.Description,
                MeasurementUnitId = model.MeasurementUnitId.Value
            };

            await _context.ProjectFormulaActivities.AddAsync(activity);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, ProjectFormulaActivityViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var activity = await _context.ProjectFormulaActivities
                .FirstOrDefaultAsync(x => x.Id == id);

            activity.Description = model.Description;
            activity.MeasurementUnitId = model.MeasurementUnitId.Value;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var activity = await _context.ProjectFormulaActivities
                .FirstOrDefaultAsync(x => x.Id == id);

            _context.ProjectFormulaActivities.Remove(activity);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
