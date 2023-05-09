using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Security;
using IVC.PE.WEB.Areas.Security.ViewModels.TrainingResultStatusViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Security.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Security.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.SECURITY)]
    [Route("seguridad/capacitaciones/variables/estados")]
    public class TrainingResultStatusController : BaseController
    {
        public TrainingResultStatusController(IvcDbContext context,
            ILogger<TrainingResultStatusController> logger) : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.TrainingResultStatus.Select(
                x => new TrainingResultStatusViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Color = x.Color
                }).AsNoTracking().ToListAsync();

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var status = await _context.TrainingResultStatus
                .Where(x => x.Id == id)
                .Select(x => new TrainingResultStatusViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Color = x.Color
                }).AsNoTracking()
                .FirstOrDefaultAsync();
            return Ok(status);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(TrainingResultStatusViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var status = new TrainingResultStatus
            {
                Name = model.Name,
                Color = model.Color
            };
            await _context.TrainingResultStatus.AddAsync(status);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, TrainingResultStatusViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var status = await _context.TrainingResultStatus.FindAsync(id);
            status.Name = model.Name;
            status.Color = model.Color;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var status = await _context.TrainingResultStatus.FindAsync(id);
            if (status is null)
                return BadRequest($"Estado con Id '{id}' no encontrado.");
            _context.TrainingResultStatus.Remove(status);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
