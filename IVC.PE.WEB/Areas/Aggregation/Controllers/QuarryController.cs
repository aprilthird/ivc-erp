using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Aggregation;
using IVC.PE.WEB.Areas.Aggregation.ViewModels.QuarryViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IVC.PE.WEB.Areas.Aggregation.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Aggregation.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.AGGREGATION)]
    [Route("agregados/canteras")]
    public class QuarryController : BaseController
    {
        public QuarryController(IvcDbContext context,
            ILogger<QuarryController> logger)
            : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.Quarries
                .Select(x => new QuarryViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                })
                .AsNoTracking()
                .ToListAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var project = await _context.Quarries
                .Where(x => x.Id == id)
                .Select(x => new QuarryViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();
            return Ok(project);
        }

        [Authorize(Roles = ConstantHelpers.Permission.Aggregation.FULL_ACCESS)]
        [HttpPost("crear")]
        public async Task<IActionResult> Create(QuarryViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var quarry = new Quarry
            {
                Name = model.Name,
                Project = await _context.Projects.FirstOrDefaultAsync()
            };
            await _context.Quarries.AddAsync(quarry);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.Aggregation.FULL_ACCESS)]
        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, QuarryViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var quarry = await _context.Quarries.FindAsync(id);
            quarry.Name = model.Name;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.Aggregation.FULL_ACCESS)]
        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var quarry = await _context.Quarries.FirstOrDefaultAsync(x => x.Id == id);
            if (quarry == null)
                return BadRequest($"Cantera con Id '{id}' no encontrado.");
            _context.Quarries.Remove(quarry);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}