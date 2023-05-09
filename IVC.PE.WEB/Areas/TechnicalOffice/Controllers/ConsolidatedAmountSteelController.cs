using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.ConsolidatedAmountSteelViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.ConsolidatedSteelViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.TECHNICAL_OFFICE)]
    [Route("oficina-tecnica/monto-consolidado-aceros")]
    public class ConsolidatedAmountSteelController : BaseController
    {
        public ConsolidatedAmountSteelController(IvcDbContext context,
          ILogger<ConsolidatedAmountSteelController> logger) : base(context, logger)
        {

        }

        public IActionResult Index() => View();


        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var query = _context.ConsolidatedAmountSteels
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId);

            if (projectFormulaId.HasValue)
                query = query.Where(x => x.ProjectFormulaId == projectFormulaId);
            if (budgetTitleId.HasValue)
                query = query.Where(x => x.BudgetTitleId == budgetTitleId);
            if (workFrontId.HasValue)
                query = query.Where(x => x.WorkFrontId == workFrontId);
            if (projectPhaseId.HasValue)
                query = query.Where(x => x.ProjectPhaseId == projectPhaseId);

            var steels = await query
                .Include(x => x.WorkFront)
                .OrderBy(x => x.OrderNumber)
                .Select(x => new ConsolidatedAmountSteelViewModel
                {
                    Id = x.Id,
                    BudgetTitleId = x.BudgetTitleId,
                    ProjectFormulaId = x.ProjectFormulaId,
                    WorkFront = new WorkFrontViewModel
                    {
                        Code = x.WorkFront.Code
                    },
                    ItemNumber = x.ItemNumber,
                    Description = x.Description,
                    Unit = x.Unit,
                    Metered = x.Metered.ToString("N", CultureInfo.InvariantCulture),
                    ContractualMetered = x.ContractualMetered.ToString("N", CultureInfo.InvariantCulture),
                    Rod6mm = x.Rod6mm.ToString("N", CultureInfo.InvariantCulture),
                    Rod8mm = x.Rod8mm.ToString("N", CultureInfo.InvariantCulture),
                    Rod3x8 = x.Rod3x8.ToString("N", CultureInfo.InvariantCulture),
                    Rod1x2 = x.Rod1x2.ToString("N", CultureInfo.InvariantCulture),
                    Rod5x8 = x.Rod5x8.ToString("N", CultureInfo.InvariantCulture),
                    Rod3x4 = x.Rod3x4.ToString("N", CultureInfo.InvariantCulture),
                    Rod1 = x.Rod1.ToString("N", CultureInfo.InvariantCulture),
                    ContractualStaked = x.ContractualStaked.ToString("N", CultureInfo.InvariantCulture)
                }).AsNoTracking()
                .ToListAsync();

            return Ok(steels);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var query = await _context.ConsolidatedAmountSteels
                .Select(x => new ConsolidatedAmountSteelViewModel
                {
                    Id = x.Id,
                    BudgetTitleId = x.BudgetTitleId,
                    ProjectFormulaId = x.ProjectFormulaId,
                    ProjectPhaseId = x.ProjectPhaseId,
                    WorkFrontId = x.WorkFrontId,
                    ItemNumber = x.ItemNumber,
                    Description = x.Description,
                    Unit = x.Unit,
                    Metered = x.Metered.ToString("N", CultureInfo.InvariantCulture),
                    ContractualMetered = x.ContractualMetered.ToString("N", CultureInfo.InvariantCulture),
                    Rod6mm = x.Rod6mm.ToString("N", CultureInfo.InvariantCulture),
                    Rod8mm = x.Rod8mm.ToString("N", CultureInfo.InvariantCulture),
                    Rod3x8 = x.Rod3x8.ToString("N", CultureInfo.InvariantCulture),
                    Rod1x2 = x.Rod1x2.ToString("N", CultureInfo.InvariantCulture),
                    Rod5x8 = x.Rod5x8.ToString("N", CultureInfo.InvariantCulture),
                    Rod3x4 = x.Rod3x4.ToString("N", CultureInfo.InvariantCulture),
                    Rod1 = x.Rod1.ToString("N", CultureInfo.InvariantCulture),
                    ContractualStaked = x.ContractualStaked.ToString("N", CultureInfo.InvariantCulture)
                }).AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return Ok(query);
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, ConsolidatedAmountSteelViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var steel = await _context.ConsolidatedAmountSteels.FirstOrDefaultAsync(x => x.Id == id);

            steel.ItemNumber = model.ItemNumber;
            steel.Description = model.Description;
            steel.Unit = model.Unit;
            steel.Metered = model.Metered.ToDoubleString();
            steel.ContractualMetered = model.ContractualMetered.ToDoubleString();
            steel.Rod6mm = model.Rod6mm.ToDoubleString();
            steel.Rod8mm = model.Rod8mm.ToDoubleString();
            steel.Rod3x8 = model.Rod3x8.ToDoubleString();
            steel.Rod1x2 = model.Rod1x2.ToDoubleString();
            steel.Rod5x8 = model.Rod5x8.ToDoubleString();
            steel.Rod3x4 = model.Rod3x4.ToDoubleString();
            steel.Rod1 = model.Rod1.ToDoubleString();
            steel.ContractualStaked = model.ContractualStaked.ToDoubleString();

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var steel = await _context.ConsolidatedAmountSteels.FirstOrDefaultAsync(x => x.Id == id);

            if (steel == null)
                return BadRequest("No se ha encontrado el acero");

            _context.ConsolidatedAmountSteels.Remove(steel);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("cargar")]
        public async Task<IActionResult> Load()
        {
            var consolidados = new List<ConsolidatedAmountSteel>();

            var steels = await _context.ConsolidatedSteels.Include(x => x.ProjectFormula).Include(x => x.BudgetTitle).OrderBy(x => x.OrderNumber).ToListAsync();

            var projectFormulas = await _context.ProjectFormulas.ToListAsync();

            var count = _context.ConsolidatedAmountSteels.Count();

            var rod6mm = await _context.SteelVariables.AsNoTracking().FirstOrDefaultAsync(x => x.RodDiameterMilimeters == 6);
            var rod8mm = await _context.SteelVariables.AsNoTracking().FirstOrDefaultAsync(x => x.RodDiameterMilimeters == 8);
            var rod3x8 = await _context.SteelVariables.AsNoTracking().FirstOrDefaultAsync(x => x.RodDiameterInch == "3/8");
            var rod1x2 = await _context.SteelVariables.AsNoTracking().FirstOrDefaultAsync(x => x.RodDiameterInch == "1/2");
            var rod5x8 = await _context.SteelVariables.AsNoTracking().FirstOrDefaultAsync(x => x.RodDiameterInch == "5/8");
            var rod3x4 = await _context.SteelVariables.AsNoTracking().FirstOrDefaultAsync(x => x.RodDiameterInch == "3/4");
            var rod1 = await _context.SteelVariables.AsNoTracking().FirstOrDefaultAsync(x => x.RodDiameterInch == "1");

            if (rod6mm == null)
                return BadRequest("No se ha hallado la varrilla 6mm");
            if (rod8mm == null)
                return BadRequest("No se ha hallado la varrilla 8mm");
            if (rod3x8 == null)
                return BadRequest("No se ha hallado la varilla de 3/8\"");
            if (rod1x2 == null)
                return BadRequest("No se ha hallado la varilla de 1/2\"");
            if (rod5x8 == null)
                return BadRequest("No se ha hallado la varilla de 5/8\"");
            if (rod3x4 == null)
                return BadRequest("No se ha hallado la varilla de 3/4\"");
            if (rod1 == null)
                return BadRequest("No se ha hallado la varilla de 1\"");

            foreach (var item in steels)
            {

                var consolidatedSteel = new ConsolidatedAmountSteel();

                var existe = await _context.ConsolidatedAmountSteels.FirstOrDefaultAsync(x => x.ItemNumber == item.ItemNumber && x.BudgetTitleId == item.BudgetTitleId);

                if (item.ContractualMetered != 0)
                {

                    var formula = projectFormulas.FirstOrDefault(x => x.Code == item.ProjectFormula.Code);

                    if (existe == null)
                    {
                        consolidatedSteel.Id = Guid.NewGuid();
                        consolidatedSteel.BudgetTitleId = item.BudgetTitleId;
                        consolidatedSteel.ProjectFormulaId = item.ProjectFormulaId;
                        consolidatedSteel.ProjectPhaseId = item.ProjectPhaseId;
                        consolidatedSteel.WorkFrontId = item.WorkFrontId;
                        consolidatedSteel.OrderNumber = count;
                        consolidatedSteel.ItemNumber = item.ItemNumber;
                        consolidatedSteel.Description = item.Description;
                        consolidatedSteel.Unit = item.Unit;
                        consolidatedSteel.Metered = item.Metered;
                        consolidatedSteel.ContractualMetered = Math.Round(item.ContractualMetered * 2.85, 2, MidpointRounding.AwayFromZero);
                        consolidatedSteel.Rod6mm = Math.Round(item.Rod6mm * rod6mm.PricePerRod, 2, MidpointRounding.AwayFromZero);
                        consolidatedSteel.Rod8mm = Math.Round(item.Rod8mm * rod8mm.PricePerRod, 2, MidpointRounding.AwayFromZero);
                        consolidatedSteel.Rod3x8 = Math.Round(item.Rod3x8 * rod3x8.PricePerRod, 2, MidpointRounding.AwayFromZero);
                        consolidatedSteel.Rod1x2 = Math.Round(item.Rod1x2 * rod1x2.PricePerRod, 2, MidpointRounding.AwayFromZero);
                        consolidatedSteel.Rod5x8 = Math.Round(item.Rod5x8 * rod5x8.PricePerRod, 2, MidpointRounding.AwayFromZero);
                        consolidatedSteel.Rod3x4 = Math.Round(item.Rod3x4 * rod3x4.PricePerRod, 2, MidpointRounding.AwayFromZero);
                        consolidatedSteel.Rod1 = Math.Round(item.Rod1 * rod1.PricePerRod, 2, MidpointRounding.AwayFromZero);
                        consolidatedSteel.ContractualStaked = Math.Round(consolidatedSteel.Rod6mm + consolidatedSteel.Rod3x8 +
                            consolidatedSteel.Rod1x2 + consolidatedSteel.Rod5x8 + consolidatedSteel.Rod3x4 + consolidatedSteel.Rod1, 2, MidpointRounding.AwayFromZero);

                        consolidados.Add(consolidatedSteel);
                        count++;
                    }
                    else
                    {
                        existe.Metered = item.Metered;
                        existe.ContractualMetered = Math.Round(item.ContractualMetered * 2.85, 2, MidpointRounding.AwayFromZero);
                        existe.Rod6mm = Math.Round(item.Rod6mm * rod6mm.PricePerRod, 2, MidpointRounding.AwayFromZero);
                        existe.Rod8mm = Math.Round(item.Rod8mm * rod8mm.PricePerRod, 2, MidpointRounding.AwayFromZero);
                        existe.Rod3x8 = Math.Round(item.Rod3x8 * rod3x8.PricePerRod, 2, MidpointRounding.AwayFromZero);
                        existe.Rod1x2 = Math.Round(item.Rod1x2 * rod1x2.PricePerRod, 2, MidpointRounding.AwayFromZero);
                        existe.Rod5x8 = Math.Round(item.Rod5x8 * rod5x8.PricePerRod, 2, MidpointRounding.AwayFromZero);
                        existe.Rod3x4 = Math.Round(item.Rod3x4 * rod3x4.PricePerRod, 2, MidpointRounding.AwayFromZero);
                        existe.Rod1 = Math.Round(item.Rod1 * rod1.PricePerRod, 2, MidpointRounding.AwayFromZero);
                        existe.ContractualStaked = Math.Round(existe.Rod6mm + existe.Rod3x8 +
                            existe.Rod1x2 + existe.Rod5x8 + existe.Rod3x4 + existe.Rod1, 2, MidpointRounding.AwayFromZero);

                    }
                }
                else
                {
                    if (existe == null)
                    {
                        consolidatedSteel.Id = Guid.NewGuid();
                        consolidatedSteel.BudgetTitleId = item.BudgetTitleId;
                        consolidatedSteel.ProjectFormulaId = item.ProjectFormulaId;
                        consolidatedSteel.ProjectPhaseId = item.ProjectPhaseId;
                        consolidatedSteel.WorkFrontId = item.WorkFrontId;
                        consolidatedSteel.OrderNumber = count;
                        consolidatedSteel.ItemNumber = item.ItemNumber;
                        consolidatedSteel.Description = item.Description;
                        consolidatedSteel.Unit = item.Unit;

                        consolidados.Add(consolidatedSteel);
                        count++;
                    }
                }
            }
            if (consolidados.Count != 0)
                await _context.ConsolidatedAmountSteels.AddRangeAsync(consolidados);
            //_context.ConsolidatedAmountSteels.UpdateRange(consolidadosUpdate);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("metrado-contractual")]
        public async Task<IActionResult> GetContractualMetered(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var query = _context.ConsolidatedAmountSteels
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId && x.ContractualMetered != 0);

            var contractualMeteredSuma = 0.0;

            if (projectFormulaId.HasValue)
                query = query.Where(x => x.ProjectFormulaId == projectFormulaId);
            if (budgetTitleId.HasValue)
                query = query.Where(x => x.BudgetTitleId == budgetTitleId);
            if (workFrontId.HasValue)
                query = query.Where(x => x.WorkFrontId == workFrontId);
            if (projectPhaseId.HasValue)
                query = query.Where(x => x.ProjectPhaseId == projectPhaseId);

            var data = await query.ToListAsync();

            foreach (var item in data)
            {
                contractualMeteredSuma += item.ContractualMetered;
            }

            return Ok(contractualMeteredSuma.ToString("N", CultureInfo.InvariantCulture));
        }

        [HttpGet("rod6mm")]
        public async Task<IActionResult> GetRod6mm(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var query = _context.ConsolidatedAmountSteels
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId && x.ContractualMetered != 0);

            var rod6mmSuma = 0.0;

            if (projectFormulaId.HasValue)
                query = query.Where(x => x.ProjectFormulaId == projectFormulaId);
            if (budgetTitleId.HasValue)
                query = query.Where(x => x.BudgetTitleId == budgetTitleId);
            if (workFrontId.HasValue)
                query = query.Where(x => x.WorkFrontId == workFrontId);
            if (projectPhaseId.HasValue)
                query = query.Where(x => x.ProjectPhaseId == projectPhaseId);

            var data = await query.ToListAsync();

            foreach (var item in data)
            {
                rod6mmSuma += item.Rod6mm;
            }

            return Ok(rod6mmSuma.ToString("N", CultureInfo.InvariantCulture));
        }

        [HttpGet("rod8mm")]
        public async Task<IActionResult> GetRod8mm(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var query = _context.ConsolidatedAmountSteels
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId && x.ContractualMetered != 0);

            var rod8mmSuma = 0.0;

            if (projectFormulaId.HasValue)
                query = query.Where(x => x.ProjectFormulaId == projectFormulaId);
            if (budgetTitleId.HasValue)
                query = query.Where(x => x.BudgetTitleId == budgetTitleId);
            if (workFrontId.HasValue)
                query = query.Where(x => x.WorkFrontId == workFrontId);
            if (projectPhaseId.HasValue)
                query = query.Where(x => x.ProjectPhaseId == projectPhaseId);

            var data = await query.ToListAsync();

            foreach (var item in data)
            {
                rod8mmSuma += item.Rod8mm;
            }

            return Ok(rod8mmSuma.ToString("N", CultureInfo.InvariantCulture));
        }

        [HttpGet("rod3x8")]
        public async Task<IActionResult> GetRod3x8(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var query = _context.ConsolidatedAmountSteels
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId && x.ContractualMetered != 0);

            var rod3x8Suma = 0.0;

            if (projectFormulaId.HasValue)
                query = query.Where(x => x.ProjectFormulaId == projectFormulaId);
            if (budgetTitleId.HasValue)
                query = query.Where(x => x.BudgetTitleId == budgetTitleId);
            if (workFrontId.HasValue)
                query = query.Where(x => x.WorkFrontId == workFrontId);
            if (projectPhaseId.HasValue)
                query = query.Where(x => x.ProjectPhaseId == projectPhaseId);

            var data = await query.ToListAsync();

            foreach (var item in data)
            {
                rod3x8Suma += item.Rod3x8;
            }

            return Ok(rod3x8Suma.ToString("N", CultureInfo.InvariantCulture));
        }

        [HttpGet("rod1x2")]
        public async Task<IActionResult> GetRod1x2(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var query = _context.ConsolidatedAmountSteels
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId && x.ContractualMetered != 0);

            var rod1x2Suma = 0.0;

            if (projectFormulaId.HasValue)
                query = query.Where(x => x.ProjectFormulaId == projectFormulaId);
            if (budgetTitleId.HasValue)
                query = query.Where(x => x.BudgetTitleId == budgetTitleId);
            if (workFrontId.HasValue)
                query = query.Where(x => x.WorkFrontId == workFrontId);
            if (projectPhaseId.HasValue)
                query = query.Where(x => x.ProjectPhaseId == projectPhaseId);

            var data = await query.ToListAsync();

            foreach (var item in data)
            {
                rod1x2Suma += item.Rod1x2;
            }

            return Ok(rod1x2Suma.ToString("N", CultureInfo.InvariantCulture));
        }

        [HttpGet("rod5x8")]
        public async Task<IActionResult> GetRod5x8(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var query = _context.ConsolidatedAmountSteels
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId && x.ContractualMetered != 0);

            var rod5x8Suma = 0.0;

            if (projectFormulaId.HasValue)
                query = query.Where(x => x.ProjectFormulaId == projectFormulaId);
            if (budgetTitleId.HasValue)
                query = query.Where(x => x.BudgetTitleId == budgetTitleId);
            if (workFrontId.HasValue)
                query = query.Where(x => x.WorkFrontId == workFrontId);
            if (projectPhaseId.HasValue)
                query = query.Where(x => x.ProjectPhaseId == projectPhaseId);

            var data = await query.ToListAsync();

            foreach (var item in data)
            {
                rod5x8Suma += item.Rod5x8;
            }

            return Ok(rod5x8Suma.ToString("N", CultureInfo.InvariantCulture));
        }

        [HttpGet("rod3x4")]
        public async Task<IActionResult> GetRod3x4(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var query = _context.ConsolidatedAmountSteels
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId && x.ContractualMetered != 0);

            var rod3x4Suma = 0.0;

            if (projectFormulaId.HasValue)
                query = query.Where(x => x.ProjectFormulaId == projectFormulaId);
            if (budgetTitleId.HasValue)
                query = query.Where(x => x.BudgetTitleId == budgetTitleId);
            if (workFrontId.HasValue)
                query = query.Where(x => x.WorkFrontId == workFrontId);
            if (projectPhaseId.HasValue)
                query = query.Where(x => x.ProjectPhaseId == projectPhaseId);

            var data = await query.ToListAsync();

            foreach (var item in data)
            {
                rod3x4Suma += item.Rod3x4;
            }

            return Ok(rod3x4Suma.ToString("N", CultureInfo.InvariantCulture));
        }

        [HttpGet("rod1")]
        public async Task<IActionResult> GetRod1(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var query = _context.ConsolidatedAmountSteels
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId && x.ContractualMetered != 0);

            var rod1Suma = 0.0;

            if (projectFormulaId.HasValue)
                query = query.Where(x => x.ProjectFormulaId == projectFormulaId);
            if (budgetTitleId.HasValue)
                query = query.Where(x => x.BudgetTitleId == budgetTitleId);
            if (workFrontId.HasValue)
                query = query.Where(x => x.WorkFrontId == workFrontId);
            if (projectPhaseId.HasValue)
                query = query.Where(x => x.ProjectPhaseId == projectPhaseId);

            var data = await query.ToListAsync();

            foreach (var item in data)
            {
                rod1Suma += item.Rod1;
            }

            return Ok(rod1Suma.ToString("N", CultureInfo.InvariantCulture));
        }

        [HttpGet("metrado-replanteado")]
        public async Task<IActionResult> GetcontractualStaked(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var query = _context.ConsolidatedAmountSteels
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId && x.ContractualMetered != 0);

            var contractualStakedSuma = 0.0;

            if (projectFormulaId.HasValue)
                query = query.Where(x => x.ProjectFormulaId == projectFormulaId);
            if (budgetTitleId.HasValue)
                query = query.Where(x => x.BudgetTitleId == budgetTitleId);
            if (workFrontId.HasValue)
                query = query.Where(x => x.WorkFrontId == workFrontId);
            if (projectPhaseId.HasValue)
                query = query.Where(x => x.ProjectPhaseId == projectPhaseId);

            var data = await query.ToListAsync();

            foreach (var item in data)
            {
                contractualStakedSuma += item.ContractualStaked;
            }

            return Ok(contractualStakedSuma.ToString("N", CultureInfo.InvariantCulture));
        }

    

    }
}
