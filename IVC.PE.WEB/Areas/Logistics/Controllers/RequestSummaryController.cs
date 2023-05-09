using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Logistics;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.RequestViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IVC.PE.WEB.Areas.Logistics.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Logistics.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.LOGISTICS)]
    [Route("logistica/requerimientos/resumenes")]
    public class RequestSummaryController : BaseController
    {
        public RequestSummaryController(IvcDbContext context) : base(context)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var summaries = await _context.RequestSummaries
                .Where(x => x.ProjectId == GetProjectId())
                .Include(x => x.Project)
                .Select(x => new RequestSummaryViewModel
                {
                    Id = x.Id,
                    ProjectId = x.ProjectId,
                    Project = new ProjectViewModel
                    {
                        Abbreviation = x.Project.Abbreviation
                    },
                    TotalOfRequest = x.TotalOfRequest,
                    CodePrefix = x.CodePrefix
                }).ToListAsync();

            return Ok(summaries);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var summary = await _context.RequestSummaries
                .Where(x => x.Id == id)
                .Select(x => new RequestSummaryViewModel
                {
                    Id = x.Id,
                    ProjectId = x.ProjectId,
                    TotalOfRequest = x.TotalOfRequest,
                    CodePrefix = x.CodePrefix
                }).FirstOrDefaultAsync();

            return Ok(summary);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(RequestSummaryViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var summary = new RequestSummary
            {
                ProjectId = GetProjectId(),
                CodePrefix = model.CodePrefix,
                TotalOfRequest = 0
            };

            await _context.RequestSummaries.AddAsync(summary);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, RequestSummaryViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var summary = await _context.RequestSummaries.FirstOrDefaultAsync(x => x.Id == id);

            summary.CodePrefix = model.CodePrefix;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var summary = await _context.RequestSummaries
                .FirstOrDefaultAsync(x => x.Id == id);

            if (summary.TotalOfRequest > 0)
                return BadRequest("Existen registros con este código.");

            _context.RequestSummaries.Remove(summary);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
