using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Bidding;
using IVC.PE.WEB.Areas.Bidding.ViewModels.ProfessionViewModels;
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
    [Route("licitaciones/profesiones")]
    public class ProfessionController : BaseController
    {
        public ProfessionController(IvcDbContext context,
        ILogger<ProfessionController> logger) : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var query = _context.Professions
              .AsQueryable();


            var data = await query
                .Select(x => new ProfessionViewModel
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
            var data = await _context.Professions
                .Where(x => x.Id == id)
                .Select(x => new ProfessionViewModel
                {
                    Id = x.Id,
                    Name = x.Name

                }).AsNoTracking()
                .FirstOrDefaultAsync();
            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(ProfessionViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var profession = new Profession
            {
                Name = model.Name
            };
            await _context.Professions.AddAsync(profession);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, ProfessionViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var profession = await _context.Professions.FindAsync(id);


            profession.Name = model.Name;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var profession = await _context.Professions.FirstOrDefaultAsync(x => x.Id == id);
            if (profession == null)
                return BadRequest($"Profesion con Id '{id}' no encontrado.");
            _context.Professions.Remove(profession);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}