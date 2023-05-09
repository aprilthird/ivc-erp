using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BudgetTypeViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.TECHNICAL_OFFICE)]
    [Route("oficina-tecnica/tipo-presupuestos")]
    public class BudgetTypeController : BaseController
    {
        public BudgetTypeController(IvcDbContext context,
            ILogger<BudgetTypeController> logger) : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var types = await _context.BudgetTypes
                .Select(x => new BudgetTypeViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).AsNoTracking()
                .ToListAsync();

            return Ok(types);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var types = await _context.BudgetTypes
                .Select(x => new BudgetTypeViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return Ok(types);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(BudgetTypeViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var type = new BudgetType
            {
                Name = model.Name
            };

            await _context.BudgetTypes.AddAsync(type);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, BudgetTypeViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var type = await _context.BudgetTypes
                .FirstOrDefaultAsync(x => x.Id == id);

            type.Name = model.Name;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id, BudgetTypeViewModel model)
        {
            var type = await _context.BudgetTypes
                .FirstOrDefaultAsync(x => x.Id == id);

            if (type == null)
                return BadRequest("No se ha encontrado el tipo de presuesto");

            _context.BudgetTypes.Remove(type);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
