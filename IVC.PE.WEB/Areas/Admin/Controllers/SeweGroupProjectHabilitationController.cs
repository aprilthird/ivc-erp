using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
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
    [Route("admin/habilitaciones-cuadrilla")]
    public class SeweGroupProjectHabilitationController : BaseController
    {
        public SeweGroupProjectHabilitationController(IvcDbContext context,
            ILogger<SeweGroupProjectHabilitationController> logger)
            : base(context, logger)
        {
        }

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? sewerGroupId = null)
        {
            if (sewerGroupId == null)
                return Ok(new List<SewerGroupProjectHabilitationViewModel>());

            var habilitations = await _context.SewerGroupProjectHabilitations
                .Include(x => x.ProjectHabilitation)
                .Where(x => x.SewerGroupId == sewerGroupId.Value)
                .Select(x => new SewerGroupProjectHabilitationViewModel
                {
                    Id = x.Id,
                    SewerGroupId = x.SewerGroupId,
                    ProjectHabilitationId = x.ProjectHabilitationId,
                    ProjectHabilitation = new ProjectHabilitationViewModel
                    {
                        LocationCode = x.ProjectHabilitation.LocationCode,
                        Description = x.ProjectHabilitation.Description
                    }
                }).ToListAsync();

            return Ok(habilitations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var habilitations = await _context.SewerGroupProjectHabilitations
                .Where(x => x.SewerGroupId == id)
                .Select(x => x.ProjectHabilitationId)
                .ToListAsync();

            return Ok(new
            {
                SewerGroupId = id,
                HabilitationIds = habilitations
            });
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(SewerGroupProjectHabilitationViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var habs = await _context.SewerGroupProjectHabilitations
                .Where(x => x.SewerGroupId == model.SewerGroupId)
                .ToListAsync();

            _context.SewerGroupProjectHabilitations.RemoveRange(habs);
            await _context.SewerGroupProjectHabilitations.AddRangeAsync(
                model.ProjectHabilitationIds.Select(x => new SewerGroupProjectHabilitation
                {
                    SewerGroupId = model.SewerGroupId,
                    ProjectHabilitationId = x
                }).ToList());
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var habilitation = await _context.SewerGroupProjectHabilitations
                .FirstOrDefaultAsync(x => x.Id == id);

            _context.SewerGroupProjectHabilitations.Remove(habilitation);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
