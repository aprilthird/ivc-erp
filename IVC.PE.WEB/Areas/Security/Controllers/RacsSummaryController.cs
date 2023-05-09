using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Security;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using IVC.PE.WEB.Areas.Security.ViewModels.RacsViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IVC.PE.WEB.Areas.Security.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Security.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.SECURITY)]
    [Route("seguridad/conf-racs")]
    public class RacsSummaryController : BaseController
    {
        public RacsSummaryController(IvcDbContext context,
            ILogger<RacsSummaryController> logger)
            : base(context, logger)
        {
        }

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var sums = await _context.RacsSummaries
                .Include(x => x.Project)
                .Select(x => new RacsSummaryViewModel
                {
                    Id = x.Id,
                    ProjectId = x.ProjectId,
                    Project = new ProjectViewModel
                    {
                        Id = x.Project.Id,
                        Abbreviation = x.Project.Abbreviation,
                        CostCenter = x.Project.CostCenter,
                        Name = x.Project.Name
                    },
                    RacsCode = x.RacsCode,
                    RacsCount = x.RacsCount,
                    VersionCode = x.VersionCode
                }).ToListAsync();

            return Ok(sums);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var sums = await _context.RacsSummaries
                .Where(x => x.Id == id)
                .Select(x => new RacsSummaryViewModel
                {
                    Id = x.Id,
                    ProjectId = x.ProjectId,
                    RacsCode = x.RacsCode,
                    RacsCount = x.RacsCount,
                    VersionCode = x.VersionCode
                }).FirstOrDefaultAsync();

            return Ok(sums);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(RacsSummaryViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var sum = new RacsSummary
            {
                ProjectId = model.ProjectId,
                RacsCode = model.RacsCode,
                RacsCount = 0,
                VersionCode = model.VersionCode
            };

            await _context.RacsSummaries.AddAsync(sum);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var sum = await _context.RacsSummaries.FirstOrDefaultAsync(x => x.Id == id);

            if (sum.RacsCount > 0)
                return BadRequest("No se puede eliminar, existen reportes que usan la configuración.");

            _context.RacsSummaries.Remove(sum);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
