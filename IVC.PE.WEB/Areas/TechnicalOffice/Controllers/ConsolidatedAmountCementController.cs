using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.ConsolidatedAmountCementViewModels;
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
    [Route("oficina-tecnica/monto-consolidado-cementos")]
    public class ConsolidatedAmountCementController : BaseController
    {
        public ConsolidatedAmountCementController(IvcDbContext context,
           ILogger<ConsolidatedAmountCementController> logger) : base(context, logger)
        {

        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var query = _context.ConsolidatedAmountCements
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

            var cements = await query
                .Include(x => x.WorkFront)
                .OrderBy(x => x.OrderNumber)
                .Select(x => new ConsolidatedAmountCementViewModel
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
                    ContractualMeteredTypeOne = x.ContractualMeteredTypeOne.ToString("N", CultureInfo.InvariantCulture),
                    ContractualMeteredTypeFive = x.ContractualMeteredTypeFive.ToString("N", CultureInfo.InvariantCulture),
                    ContractualRestatedTypeOne = x.ContractualRestatedTypeOne.ToString("N", CultureInfo.InvariantCulture),
                    ContractualRestatedTypeFive = x.ContractualRestatedTypeFive.ToString("N", CultureInfo.InvariantCulture)
                }).AsNoTracking()
                .ToListAsync();

            return Ok(cements);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var query = await _context.ConsolidatedAmountCements
                .Select(x => new ConsolidatedAmountCementViewModel
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
                    ContractualMeteredTypeOne = x.ContractualMeteredTypeOne.ToString("N", CultureInfo.InvariantCulture),
                    ContractualMeteredTypeFive = x.ContractualMeteredTypeFive.ToString("N", CultureInfo.InvariantCulture),
                    ContractualRestatedTypeOne = x.ContractualRestatedTypeOne.ToString("N", CultureInfo.InvariantCulture),
                    ContractualRestatedTypeFive = x.ContractualRestatedTypeFive.ToString("N", CultureInfo.InvariantCulture)
                }).AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return Ok(query);
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, ConsolidatedAmountCementViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cement = await _context.ConsolidatedAmountCements.FirstOrDefaultAsync(x => x.Id == id);

            cement.ItemNumber = model.ItemNumber;
            cement.Description = model.Description;
            cement.Unit = model.Unit;
            cement.Metered = model.Metered.ToDoubleString();
            cement.ContractualMeteredTypeOne = model.ContractualMeteredTypeOne.ToDoubleString();
            cement.ContractualMeteredTypeFive = model.ContractualMeteredTypeFive.ToDoubleString();
            cement.ContractualRestatedTypeOne = model.ContractualRestatedTypeOne.ToDoubleString();
            cement.ContractualRestatedTypeFive = model.ContractualRestatedTypeFive.ToDoubleString();

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var cement = await _context.Cements.FirstOrDefaultAsync(x => x.Id == id);

            if (cement == null)
                return BadRequest("No se ha encontrado el acero");

            _context.Cements.Remove(cement);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("cargar")]
        public async Task<IActionResult> Load()
        {
            var consolidados = new List<ConsolidatedAmountCement>();

            var steels = await _context.ConsolidatedCements.Include(x => x.ProjectFormula).Include(x => x.BudgetTitle).OrderBy(x => x.OrderNumber).ToListAsync();

            var projectFormulas = await _context.ProjectFormulas.ToListAsync();

            var count = _context.ConsolidatedAmountCements.Count();

            var typeOne = await _context.CementVariables.Include(x=>x.Supply).AsNoTracking().FirstOrDefaultAsync(x => x.Supply.Description == "CEMENTO PORTLAND TIPO I (42.5 KG.)");
            var typeFive = await _context.CementVariables.Include(x => x.Supply).AsNoTracking().FirstOrDefaultAsync(x => x.Supply.Description == "CEMENTO PORTLAND TIPO V (42.5 KG.)");

            if (typeOne == null)
                return BadRequest("No se encontró la variable CEMENTO PORTLAND TIPO I (42.5 KG.)");
            if (typeFive == null)
                return BadRequest("No se encontró la variable CEMENTO PORTLAND TIPO V (42.5 KG.)");

            foreach (var item in steels)
            {

                var consolidatedCement = new ConsolidatedAmountCement();

                var existe = await _context.ConsolidatedAmountCements.FirstOrDefaultAsync(x => x.ItemNumber == item.ItemNumber && x.BudgetTitleId == item.BudgetTitleId);

                if (item.Unit != "")
                {

                    var formula = projectFormulas.FirstOrDefault(x => x.Code == item.ProjectFormula.Code);

                    if (existe == null)
                    {
                        consolidatedCement.Id = Guid.NewGuid();
                        consolidatedCement.BudgetTitleId = item.BudgetTitleId;
                        consolidatedCement.ProjectFormulaId = item.ProjectFormulaId;
                        consolidatedCement.ProjectPhaseId = item.ProjectPhaseId;
                        consolidatedCement.WorkFrontId = item.WorkFrontId;
                        consolidatedCement.OrderNumber = count;
                        consolidatedCement.ItemNumber = item.ItemNumber;
                        consolidatedCement.Description = item.Description;
                        consolidatedCement.Unit = item.Unit;
                        consolidatedCement.Metered = item.Metered;
                        consolidatedCement.ContractualMeteredTypeOne = Math.Round(item.ContractualMeteredTypeOne * typeOne.UnitPrice, 2, MidpointRounding.AwayFromZero);
                        consolidatedCement.ContractualMeteredTypeFive = Math.Round(item.ContractualMeteredTypeFive * typeFive.UnitPrice, 2, MidpointRounding.AwayFromZero);
                        consolidatedCement.ContractualRestatedTypeOne = Math.Round(item.ContractualRestatedTypeOne * typeOne.UnitPrice, 2, MidpointRounding.AwayFromZero);
                        consolidatedCement.ContractualRestatedTypeFive = Math.Round(item.ContractualRestatedTypeFive * typeFive.UnitPrice, 2, MidpointRounding.AwayFromZero);

                        consolidados.Add(consolidatedCement);
                        count++;
                    }
                    else
                    {
                        existe.Metered = item.Metered;
                        existe.ContractualMeteredTypeOne = Math.Round(item.ContractualMeteredTypeOne * typeOne.UnitPrice, 2, MidpointRounding.AwayFromZero);
                        existe.ContractualMeteredTypeFive = Math.Round(item.ContractualMeteredTypeFive * typeFive.UnitPrice, 2, MidpointRounding.AwayFromZero);
                        existe.ContractualRestatedTypeOne = Math.Round(item.ContractualRestatedTypeOne * typeOne.UnitPrice, 2, MidpointRounding.AwayFromZero);
                        existe.ContractualRestatedTypeFive = Math.Round(item.ContractualRestatedTypeFive * typeFive.UnitPrice, 2, MidpointRounding.AwayFromZero);
                    }
                }
                else
                {
                    if (existe == null)
                    {
                        consolidatedCement.Id = Guid.NewGuid();
                        consolidatedCement.BudgetTitleId = item.BudgetTitleId;
                        consolidatedCement.ProjectFormulaId = item.ProjectFormulaId;
                        consolidatedCement.ProjectPhaseId = item.ProjectPhaseId;
                        consolidatedCement.WorkFrontId = item.WorkFrontId;
                        consolidatedCement.OrderNumber = count;
                        consolidatedCement.ItemNumber = item.ItemNumber;
                        consolidatedCement.Description = item.Description;
                        consolidatedCement.Unit = item.Unit;

                        consolidados.Add(consolidatedCement);
                        count++;
                    }
                }
            }
            if (consolidados.Count != 0)
                await _context.ConsolidatedAmountCements.AddRangeAsync(consolidados);
            //_context.ConsolidatedAmountCements.UpdateRange(consolidadosUpdate);
            await _context.SaveChangesAsync();

            return Ok();
        }


        [HttpGet("metrado-contractual-tipo-uno")]
        public async Task<IActionResult> GetContractualMeteredTypeOneSuma(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var query = _context.ConsolidatedAmountCements
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId && x.Unit != "");

            var contractualMeteredTypeOneSuma = 0.0;

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
                contractualMeteredTypeOneSuma += item.ContractualMeteredTypeOne;
            }

            return Ok(contractualMeteredTypeOneSuma.ToString("N0", CultureInfo.InvariantCulture));
        }

        [HttpGet("metrado-contractual-tipo-cinco")]
        public async Task<IActionResult> GetContractualMeteredTypeFiveSuma(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var query = _context.ConsolidatedAmountCements
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId && x.Unit != "");

            var contractualMeteredTypeFiveSuma = 0.0;

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
                contractualMeteredTypeFiveSuma += item.ContractualMeteredTypeFive;
            }

            return Ok(contractualMeteredTypeFiveSuma.ToString("N0", CultureInfo.InvariantCulture));
        }


        [HttpGet("metrado-replanteado-tipo-uno")]
        public async Task<IActionResult> GetContractualRestatedTypeOneSuma(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var query = _context.ConsolidatedAmountCements
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId && x.Unit != "");

            var contractualRestatedTypeOneSuma = 0.0;

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
                contractualRestatedTypeOneSuma += item.ContractualRestatedTypeOne;
            }

            return Ok(contractualRestatedTypeOneSuma.ToString("N0", CultureInfo.InvariantCulture));
        }

        [HttpGet("metrado-replanteado-tipo-cinco")]
        public async Task<IActionResult> GetContractualRestatedTypeFiveSuma(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var query = _context.ConsolidatedAmountCements
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId && x.Unit != "");

            var contractualRestatedTypeFiveSuma = 0.0;

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
                contractualRestatedTypeFiveSuma += item.ContractualRestatedTypeFive;
            }

            return Ok(contractualRestatedTypeFiveSuma.ToString("N0", CultureInfo.InvariantCulture));
        }


        [HttpDelete("eliminar-filtro")]
        public async Task<IActionResult> DeleteByFilters(Guid? projectFormulaId = null, Guid? budgetTitleId = null)
        {
            var pId = GetProjectId();

            var query = _context.ConsolidatedAmountCements
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId);

            if (budgetTitleId.HasValue)
                query = query.Where(x => x.BudgetTitleId == budgetTitleId);
            else
                return BadRequest("No se ha escogido el título de presupuesto");

            if (projectFormulaId.HasValue)
                query = query.Where(x => x.ProjectFormulaId == projectFormulaId);
            else
                return BadRequest("No se ha escogido la fórmula");

            var data = await query.ToListAsync();

            _context.ConsolidatedAmountCements.RemoveRange(data);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
