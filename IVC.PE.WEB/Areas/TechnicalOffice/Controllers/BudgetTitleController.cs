using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BudgetTitleViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IVC.PE.WEB.Areas.TechnicalOffice.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.TECHNICAL_OFFICE)]
    [Route("oficina-tecnica/titulos-de-presupuesto")]
    public class BudgetTitleController : BaseController
    {
        public BudgetTitleController(IvcDbContext context, 
            ILogger<BudgetTitleController> logger) : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var query = _context.BudgetTitles
              .AsNoTracking()
              .AsQueryable();

            query = query.Where(x => x.ProjectId == GetProjectId());

            var data = await query
                .Select(x => new BudgetTitleViewModel
                {
                    Id = x.Id,
                    Abbreviation = x.Abbreviation,
                    Name = x.Name

                }).AsNoTracking()
                .ToListAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.BudgetTitles
                .Where(x => x.Id == id)
                .Select(x => new BudgetTitleViewModel
                {
                    Id = x.Id,
                    Abbreviation = x.Abbreviation,
                    Name = x.Name
                }).AsNoTracking()
                .FirstOrDefaultAsync();
            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(BudgetTitleViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var budgetTitle = new BudgetTitle
            {
                Abbreviation = model.Abbreviation,
                Name = model.Name,
                ProjectId = Guid.Parse(HttpContext.Session.GetString("ProjectId"))
            };
            await _context.BudgetTitles.AddAsync(budgetTitle);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, BudgetTitleViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var budgetTitle = await _context.BudgetTitles.FindAsync(id);
            budgetTitle.Abbreviation = model.Abbreviation;
            budgetTitle.Name = model.Name;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var budgetTitle = await _context.BudgetTitles.FirstOrDefaultAsync(x => x.Id == id);
            if (budgetTitle == null)
                return BadRequest($"Título de Presupuesto con Id '{id}' no encontrado.");
            _context.BudgetTitles.Remove(budgetTitle);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}