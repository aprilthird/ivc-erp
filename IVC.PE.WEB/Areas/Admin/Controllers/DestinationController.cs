using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.WEB.Areas.Admin.ViewModels.DestinationViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
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
    [Route("admin/grupo-de-destino")]
    public class DestinationController : BaseController
    {
        public DestinationController(IvcDbContext context,
    ILogger<DestinationController> logger)
    : base(context, logger)
        {
        }
        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var projectId = GetProjectId();

            var query = _context.Destinations
              .AsQueryable();

            var data = await query
                .Where(x => x.ProjectId == projectId)
                .Select(x => new DestinationViewModel
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
            var data = await _context.Destinations
                .Include(x=>x.Project)
                 .Where(x => x.Id == id)
                 .Select(x => new DestinationViewModel
                 {
                     Id = x.Id,
                     Name = x.Name,
                     DocStyle = x.PillColor
                 }).FirstOrDefaultAsync();

            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(DestinationViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var destinations = new Destination
            {
                ProjectId = GetProjectId(),
                Name = model.Name,
                PillColor = model.DocStyle
            };

            await _context.Destinations.AddAsync(destinations);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, DestinationViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var destinations = await _context.Destinations
                .FirstOrDefaultAsync(x => x.Id == id);
            destinations.Name = model.Name;
            destinations.PillColor = model.DocStyle;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var destinations = await _context.Destinations
                .FirstOrDefaultAsync(x => x.Id == id);
            if (destinations == null)
                return BadRequest($"Grupo de destino con Id '{id}' no encontrado.");
            _context.Destinations.Remove(destinations);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
