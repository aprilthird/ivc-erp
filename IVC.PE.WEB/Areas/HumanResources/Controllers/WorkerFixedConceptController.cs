using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.HumanResources;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.WorkerViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.HumanResources.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.HumanResources.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.HUMAN_RESOURCES)]
    [Route("recursos-humanos/obreros/conceptos-fijos")]
    public class WorkerFixedConceptController : BaseController
    {
        public WorkerFixedConceptController(IvcDbContext context)
            : base(context)
        {
        }

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? workerId = null)
        {
            if (!workerId.HasValue)
                return Ok(new List<WorkerFixedConceptViewModel>());

            var data = await _context.WorkerFixedConcepts
                .Include(x => x.PayrollConcept)
                .Where(x => x.WorkerId == workerId.Value)
                .Select(x => new WorkerFixedConceptViewModel
                {
                    Id = x.Id,
                    WorkerId = x.WorkerId,
                    PayrollConceptId = x.PayrollConceptId,
                    PayrollConcept = new ViewModels.PayrollViewModels.PayrollConceptViewModel
                    {
                        CategoryId = x.PayrollConcept.CategoryId,
                        Code = x.PayrollConcept.Code,
                        Description = x.PayrollConcept.Description,
                        ShortDescription = x.PayrollConcept.ShortDescription
                    },
                    FixedValue = x.FixedValue.ToString()
                })
                .ToListAsync();

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.WorkerFixedConcepts
                .Include(x => x.PayrollConcept)
                .Where(x => x.Id == id)
                .Select(x => new WorkerFixedConceptViewModel
                {
                    Id = x.Id,
                    WorkerId = x.WorkerId,
                    PayrollConceptId = x.PayrollConceptId,
                    FixedValue = x.FixedValue.ToString()
                })
                .FirstOrDefaultAsync();

            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(WorkerFixedConceptViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (double.TryParse(model.FixedValue, out double value))
            {
                await _context.WorkerFixedConcepts
                    .AddAsync(new WorkerFixedConcept
                    {
                        PayrollConceptId = model.PayrollConceptId,
                        WorkerId = model.WorkerId,
                        FixedValue = value
                    });
                await _context.SaveChangesAsync();
            }

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, WorkerFixedConceptViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var data = await _context.WorkerFixedConcepts
                .FirstOrDefaultAsync(x => x.Id == id);

            if (double.TryParse(model.FixedValue, out double value))
            {
                data.FixedValue = value;
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var data = await _context.WorkerFixedConcepts
                .FirstOrDefaultAsync(x => x.Id == id);

            _context.WorkerFixedConcepts.Remove(data);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
