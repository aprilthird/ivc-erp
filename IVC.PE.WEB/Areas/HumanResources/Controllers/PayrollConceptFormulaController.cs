using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.HumanResources;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.PayrollViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IVC.PE.WEB.Areas.HumanResources.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.HumanResources.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.HUMAN_RESOURCES)]
    [Route("recursos-humanos/conceptos/formulas")]
    public class PayrollConceptFormulaController : BaseController
    {
        public PayrollConceptFormulaController(IvcDbContext context,
                    UserManager<ApplicationUser> userManager,
                    ILogger<PayrollConceptFormulaController> logger)
                    : base(context, userManager, logger)
        {
        }

        [HttpGet("listar")]
        public async Task<IActionResult> GetAllFormulas(Guid? conceptId = null)
        {
            if (!conceptId.HasValue)
                return Ok(new List<PayrollConceptFormulaViewModel>());

            var payrollConceptFormulas = await _context.PayrollConceptFormulas
                .Where(x => x.PayrollConceptId == conceptId.Value)
                .Select(x => new PayrollConceptFormulaViewModel
                {
                    Id = x.Id,
                    Active = x.Active,
                    Formula = x.Formula,
                    LaborRegimeId = x.LaborRegimeId,
                    PayrollConceptId = x.PayrollConceptId,
                    IsAffectedToAfp = x.IsAffectedToAfp,
                    IsComputableToVacac = x.IsComputableToVacac,
                    IsComputableToGrati = x.IsComputableToGrati,
                    IsComputableToCTS = x.IsComputableToCTS,
                    IsAffectedToEsSalud = x.IsAffectedToEsSalud,
                    IsAffectedToOnp = x.IsAffectedToOnp,
                    IsAffectedToQta = x.IsAffectedToQta,
                    IsAffectedToRetJud = x.IsAffectedToRetJud
                }).ToListAsync();

            return Ok(payrollConceptFormulas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var formula = await _context.PayrollConceptFormulas
                .Where(x => x.Id == id)
                .Select(x => new PayrollConceptFormulaViewModel
                {
                    Id = x.Id,
                    LaborRegimeId = x.LaborRegimeId,
                    PayrollConceptId = x.PayrollConceptId,
                    PayrollVariableId = x.PayrollVariableId.Value,
                    Active = x.Active,
                    Formula = x.Formula,
                    IsAffectedToAfp = x.IsAffectedToAfp,
                    IsAffectedToEsSalud = x.IsAffectedToEsSalud,
                    IsAffectedToOnp = x.IsAffectedToOnp,
                    IsAffectedToQta = x.IsAffectedToQta,
                    IsAffectedToRetJud = x.IsAffectedToRetJud,
                    IsComputableToCTS = x.IsComputableToCTS,
                    IsComputableToGrati = x.IsComputableToGrati,
                    IsComputableToVacac = x.IsComputableToVacac
                }).FirstOrDefaultAsync();

            return Ok(formula);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(PayrollConceptFormulaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var checkRegime = await _context.PayrollConceptFormulas
                .FirstOrDefaultAsync(x => x.PayrollConceptId == model.PayrollConceptId &&
                            x.LaborRegimeId == model.LaborRegimeId);
            if (checkRegime != null)
                return BadRequest("Ya existe una formula para el regimen laboral solicitado.");

            var formula = new PayrollConceptFormula
            {
                LaborRegimeId = model.LaborRegimeId,
                PayrollConceptId = model.PayrollConceptId,
                PayrollVariableId = model.PayrollVariableId ?? null,
                Active = model.Active,
                Formula = model.Formula,
                IsAffectedToAfp = model.IsAffectedToAfp,
                IsAffectedToEsSalud = model.IsAffectedToEsSalud,
                IsAffectedToOnp = model.IsAffectedToOnp,
                IsAffectedToQta = model.IsAffectedToQta,
                IsAffectedToRetJud = model.IsAffectedToRetJud,
                IsComputableToCTS = model.IsComputableToCTS,
                IsComputableToGrati = model.IsComputableToGrati,
                IsComputableToVacac = model.IsComputableToVacac
            };

            await _context.PayrollConceptFormulas.AddAsync(formula);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, PayrollConceptFormulaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var formula = await _context.PayrollConceptFormulas
                .FirstOrDefaultAsync(x => x.Id == id);

            formula.PayrollVariableId = model.PayrollVariableId;
            formula.Active = model.Active;
            formula.Formula = model.Formula;
            formula.IsAffectedToAfp = model.IsAffectedToAfp;
            formula.IsAffectedToEsSalud = model.IsAffectedToEsSalud;
            formula.IsAffectedToOnp = model.IsAffectedToOnp;
            formula.IsAffectedToQta = model.IsAffectedToQta;
            formula.IsAffectedToRetJud = model.IsAffectedToRetJud;
            formula.IsComputableToCTS = model.IsComputableToCTS;
            formula.IsComputableToGrati = model.IsComputableToGrati;
            formula.IsComputableToVacac = model.IsComputableToVacac;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var formula = await _context.PayrollConceptFormulas
                .FirstOrDefaultAsync(x => x.Id == id);

            _context.PayrollConceptFormulas.Remove(formula);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}