using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Bidding;
using IVC.PE.WEB.Areas.Bidding.ViewModels.BiddingWorkComponentViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static IVC.PE.CORE.Helpers.ConstantHelpers;

namespace IVC.PE.WEB.Areas.Bidding.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Bidding.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.BIDDING)]
    [Route("licitaciones/componentes-de-obra")]
    public class BiddingWorkComponentController : BaseController
    {
        public BiddingWorkComponentController(IvcDbContext context,
ILogger<BiddingWorkComponentController> logger) : base(context, logger)
        {
        }
        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var query = _context.BiddingWorkComponents
              .AsQueryable();


            var data = await query
                .Select(x => new BiddingWorkComponentViewModel
                {
                    Id = x.Id,
                    Description = x.Description

                })
                .ToListAsync();
            return Ok(data);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.BiddingWorkComponents
                .Where(x => x.Id == id)
                .Select(x => new BiddingWorkComponentViewModel
                {
                    Id = x.Id,
                    Description = x.Description

                }).AsNoTracking()
                .FirstOrDefaultAsync();
            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(BiddingWorkComponentViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var bidding = new BiddingWorkComponent
            {
                Description = model.Description,
                
            };
            await _context.BiddingWorkComponents.AddAsync(bidding);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, BiddingWorkComponentViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var bidding = await _context.BiddingWorkComponents.FindAsync(id);


            bidding.Description = model.Description;
            

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var bidding = await _context.BiddingWorkComponents.FirstOrDefaultAsync(x => x.Id == id);
            if (bidding == null)
                return BadRequest($"Componente de Obra con Id '{id}' no encontrado.");
            _context.BiddingWorkComponents.Remove(bidding);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
