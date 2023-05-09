using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Bidding;
using IVC.PE.WEB.Areas.Bidding.ViewModels.CollegeViewModels;
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
    [Route("licitaciones/centro-de-estudios")]
    public class CollegeController : BaseController
    {
        public CollegeController(IvcDbContext context,
        ILogger<CollegeController> logger) : base(context, logger)
        {
        }
        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var query = _context.Colleges
              .AsQueryable();


            var data = await query
                .Select(x => new CollegeViewModel
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
            var data = await _context.Colleges
                .Where(x => x.Id == id)
                .Select(x => new CollegeViewModel
                {
                    Id = x.Id,
                    Description = x.Description

                }).AsNoTracking()
                .FirstOrDefaultAsync();
            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(CollegeViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var bidding = new College
            {
                Description = model.Description,

            };
            await _context.Colleges.AddAsync(bidding);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, CollegeViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var bidding = await _context.Colleges.FindAsync(id);


            bidding.Description = model.Description;


            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var bidding = await _context.Colleges.FirstOrDefaultAsync(x => x.Id == id);
            if (bidding == null)
                return BadRequest($"Centro de estudios con Id '{id}' no encontrado.");
            _context.Colleges.Remove(bidding);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
