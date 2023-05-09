using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Bidding;
using IVC.PE.WEB.Areas.Bidding.ViewModels.BiddingWorkTypeViewModels;
using IVC.PE.WEB.Controllers;
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
    [Route("licitaciones/tipos-de-obra")]

    public class BiddingWorkTypeController : BaseController
    {

        public BiddingWorkTypeController(IvcDbContext context,
        ILogger<BiddingWorkTypeController> logger) : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var query = _context.BiddingWorkTypes
              .AsQueryable();


            var data = await query
                .Select(x => new BiddingWorkTypeViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    PillColor = x.PillColor
                    
                })
                .ToListAsync();
            return Ok(data);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.BiddingWorkTypes
                .Where(x => x.Id == id)
                .Select(x => new BiddingWorkTypeViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    PillColor = x.PillColor

                }).AsNoTracking()
                .FirstOrDefaultAsync();
            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(BiddingWorkTypeViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var bidding = new BiddingWorkType
            {
                Name = model.Name,
                PillColor = model.PillColor
            };
            await _context.BiddingWorkTypes.AddAsync(bidding);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, BiddingWorkTypeViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var bidding = await _context.BiddingWorkTypes.FindAsync(id);


            bidding.Name = model.Name;
            bidding.PillColor = model.PillColor;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var bidding = await _context.BiddingWorkTypes.FirstOrDefaultAsync(x => x.Id == id);
            if (bidding == null)
                return BadRequest($"Tipo de Obra con Id '{id}' no encontrado.");
            _context.BiddingWorkTypes.Remove(bidding);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
