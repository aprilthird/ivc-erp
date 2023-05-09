using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Bidding;
using IVC.PE.WEB.Areas.Bidding.ViewModels.BiddingCurrencyTypeViewModels;
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
    [Route("licitaciones/tipo-de-cambio")]
    public class BiddingCurrencyTypeController : BaseController
    {
        public BiddingCurrencyTypeController(IvcDbContext context,
        ILogger<BiddingCurrencyTypeController> logger) : base(context, logger)
        {
        }
        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var query = _context.BiddingCurrencyTypes
              .AsQueryable();


            var data = await query
                .Select(x => new BiddingCurrencyTypeViewModel
                {
                    Id = x.Id,
                    Currency = x.Currency,
                    PublicationDate = x.PublicationDate.Date.ToDateString(),

                })
                .ToListAsync();
            return Ok(data);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.BiddingCurrencyTypes
                .Where(x => x.Id == id)
                .Select(x => new BiddingCurrencyTypeViewModel
                {
                    Id = x.Id,
                    Currency = x.Currency,
                    PublicationDate = x.PublicationDate.Date.ToDateString()


                }).AsNoTracking()
                .FirstOrDefaultAsync();
            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(BiddingCurrencyTypeViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var bidding = new BiddingCurrencyType
            {
                Currency = model.Currency,
                PublicationDate = model.PublicationDate.ToDateTime()

            };
            await _context.BiddingCurrencyTypes.AddAsync(bidding);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, BiddingCurrencyTypeViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var bidding = await _context.BiddingCurrencyTypes.FindAsync(id);


            bidding.Currency = model.Currency;
            bidding.PublicationDate = model.PublicationDate.ToDateTime();
                


            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var bidding = await _context.BiddingCurrencyTypes.FirstOrDefaultAsync(x => x.Id == id);
            if (bidding == null)
                return BadRequest($"Componente de Obra con Id '{id}' no encontrado.");
            _context.BiddingCurrencyTypes.Remove(bidding);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
