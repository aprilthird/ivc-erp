using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.WEB.Areas.Bidding.ViewModels.PositionViewModels;
using IVC.PE.WEB.Controllers;
using IVC.PE.ENTITIES.Models.Bidding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Bidding.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Bidding.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.BIDDING)]
    [Route("licitaciones/cargo")]
    public class PositionController : BaseController
    {
        public PositionController(IvcDbContext context,
        ILogger<PositionController> logger) : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var query = _context.Positions
              .AsQueryable();


            var data = await query
                .Select(x => new PositionViewModel
                {
                    Id = x.Id,
                    Name = x.Name

                })
                .ToListAsync();
            return Ok(data);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.Positions
                .Where(x => x.Id == id)
                .Select(x => new PositionViewModel
                {
                    Id = x.Id,
                    Name = x.Name

                }).AsNoTracking()
                .FirstOrDefaultAsync();
            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(PositionViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var bidding = new Position
            {
                Name = model.Name
            };
            await _context.Positions.AddAsync(bidding);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, PositionViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var bidding = await _context.Positions.FindAsync(id);


            bidding.Name = model.Name;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var bidding = await _context.Positions.FirstOrDefaultAsync(x => x.Id == id);
            if (bidding == null)
                return BadRequest($"Cargo con Id '{id}' no encontrado.");
            _context.Positions.Remove(bidding);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
