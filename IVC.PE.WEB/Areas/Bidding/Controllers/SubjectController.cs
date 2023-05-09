using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Bidding;
using IVC.PE.WEB.Areas.Bidding.ViewModels.SubjectViewModels;
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
    [Route("licitaciones/materia")]
    public class SubjectController : BaseController
    {
        public SubjectController(IvcDbContext context,
        ILogger<SubjectController> logger) : base(context, logger)
        {
        }
        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var query = _context.Subjects
              .AsQueryable();


            var data = await query
                .Select(x => new SubjectViewModel
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
            var data = await _context.Subjects
                .Where(x => x.Id == id)
                .Select(x => new SubjectViewModel
                {
                    Id = x.Id,
                    Description = x.Description

                }).AsNoTracking()
                .FirstOrDefaultAsync();
            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(SubjectViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var bidding = new Subject
            {
                Description = model.Description,

            };
            await _context.Subjects.AddAsync(bidding);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, SubjectViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var bidding = await _context.Subjects.FindAsync(id);


            bidding.Description = model.Description;


            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var bidding = await _context.Subjects.FirstOrDefaultAsync(x => x.Id == id);
            if (bidding == null)
                return BadRequest($"Centro de estudios con Id '{id}' no encontrado.");
            _context.Subjects.Remove(bidding);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
