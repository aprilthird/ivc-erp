using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.ConsolidatedCementViewModels;
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
    [Route("oficina-tecnica/consolidado-cementos")]
    public class ConsolidatedCementController : BaseController
    {
        public ConsolidatedCementController(IvcDbContext context,
           ILogger<ConsolidatedCementController> logger) : base(context, logger)
        {

        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var query = _context.ConsolidatedCements
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
                .Select(x => new ConsolidatedCementViewModel
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
            var query = await _context.ConsolidatedCements
                .Select(x => new ConsolidatedCementViewModel
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
        public async Task<IActionResult> Edit(Guid id, ConsolidatedCementViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cement = await _context.ConsolidatedCements.FirstOrDefaultAsync(x => x.Id == id);

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

        [HttpPost("afectacion")]
        public async Task<IActionResult> Load(LoadConsolidatedSteelViewModel model)
        {
            var consolidados = new List<ConsolidatedCement>();

            var pId = GetProjectId();

            var consolidatedCementos = await _context.ConsolidatedCements
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == GetProjectId())
                .ToListAsync();

            var meteredRestatedByStreetchs = await _context.MeteredsRestatedByStreetchs
               .Include(x => x.BudgetTitle)
               .Where(x => x.ProjectId == pId)
               .ToListAsync();

            var cementos = await _context.Cements
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId)
                .OrderBy(x => x.OrderNumber).ToListAsync();

            var projectFormulas = await _context.ProjectFormulas
                .Where(x => x.ProjectId == pId)
                .ToListAsync();

            var count = _context.ConsolidatedCements.Count();

            foreach (var item in cementos)
            {

                var consolidatedCement = new ConsolidatedCement();

                var existe = consolidatedCementos.FirstOrDefault(x => x.ItemNumber == item.ItemNumber 
                && x.BudgetTitleId == item.BudgetTitleId);

                if (item.Unit != "")
                {
                    var budgetCD = meteredRestatedByStreetchs.FirstOrDefault(x => x.ItemNumber == item.ItemNumber
                     && x.BudgetTitleId == item.BudgetTitleId
                     && x.ProjectFormulaId == item.ProjectFormulaId
                    && x.WorkFrontId == item.WorkFrontId);

                    if (budgetCD == null)
                        continue;
                        //return BadRequest("No existe el presupuesto " + item.ItemNumber);

                    var isDeductivo = 1;

                    if (budgetCD.BudgetTitle.Name.Contains("Deductivo"))
                        isDeductivo = -1;

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
                        consolidatedCement.ContractualMeteredTypeOne = item.ContractualMeteredTypeOne;
                        consolidatedCement.ContractualMeteredTypeFive = item.ContractualMeteredTypeFive;
                        consolidatedCement.ContractualRestatedTypeOne = item.ContractualRestatedTypeOne;
                        consolidatedCement.ContractualRestatedTypeFive = item.ContractualRestatedTypeFive;

                        if (formula.Code == "F1" && model.AffectationF1 == 2)
                        {
                            consolidatedCement.Metered = budgetCD.Metered;
                            consolidatedCement.ContractualMeteredTypeOne = Math.Round(item.ContractualMeteredTypeOne * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedCement.ContractualMeteredTypeFive = Math.Round(item.ContractualMeteredTypeFive * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedCement.ContractualRestatedTypeOne = Math.Round(item.ContractualRestatedTypeOne * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedCement.ContractualRestatedTypeFive = Math.Round(item.ContractualRestatedTypeFive * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                        }

                        if (formula.Code == "F2" && model.AffectationF2 == 2)
                        {
                            consolidatedCement.Metered = budgetCD.Metered;
                            consolidatedCement.ContractualMeteredTypeOne = Math.Round(item.ContractualMeteredTypeOne * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedCement.ContractualMeteredTypeFive = Math.Round(item.ContractualMeteredTypeFive * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedCement.ContractualRestatedTypeOne = Math.Round(item.ContractualRestatedTypeOne * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedCement.ContractualRestatedTypeFive = Math.Round(item.ContractualRestatedTypeFive * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                        }

                        if (formula.Code == "F3" && model.AffectationF3 == 2)
                        {
                            consolidatedCement.Metered = budgetCD.Metered;
                            consolidatedCement.ContractualMeteredTypeOne = Math.Round(item.ContractualMeteredTypeOne * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedCement.ContractualMeteredTypeFive = Math.Round(item.ContractualMeteredTypeFive * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedCement.ContractualRestatedTypeOne = Math.Round(item.ContractualRestatedTypeOne * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedCement.ContractualRestatedTypeFive = Math.Round(item.ContractualRestatedTypeFive * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                        }

                        if (formula.Code == "F4" && model.AffectationF4 == 2)
                        {
                            consolidatedCement.Metered = budgetCD.Metered;
                            consolidatedCement.ContractualMeteredTypeOne = Math.Round(item.ContractualMeteredTypeOne * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedCement.ContractualMeteredTypeFive = Math.Round(item.ContractualMeteredTypeFive * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedCement.ContractualRestatedTypeOne = Math.Round(item.ContractualRestatedTypeOne * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedCement.ContractualRestatedTypeFive = Math.Round(item.ContractualRestatedTypeFive * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                        }

                        if (formula.Code == "F5" && model.AffectationF5 == 2)
                        {
                            consolidatedCement.Metered = budgetCD.Metered;
                            consolidatedCement.ContractualMeteredTypeOne = Math.Round(item.ContractualMeteredTypeOne * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedCement.ContractualMeteredTypeFive = Math.Round(item.ContractualMeteredTypeFive * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedCement.ContractualRestatedTypeOne = Math.Round(item.ContractualRestatedTypeOne * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedCement.ContractualRestatedTypeFive = Math.Round(item.ContractualRestatedTypeFive * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                        }

                        if (formula.Code == "F6" && model.AffectationF6 == 2)
                        {
                            consolidatedCement.Metered = budgetCD.Metered;
                            consolidatedCement.ContractualMeteredTypeOne = Math.Round(item.ContractualMeteredTypeOne * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedCement.ContractualMeteredTypeFive = Math.Round(item.ContractualMeteredTypeFive * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedCement.ContractualRestatedTypeOne = Math.Round(item.ContractualRestatedTypeOne * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedCement.ContractualRestatedTypeFive = Math.Round(item.ContractualRestatedTypeFive * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                        }

                        if (formula.Code == "F7" && model.AffectationF7 == 2)
                        {
                            consolidatedCement.Metered = budgetCD.Metered;
                            consolidatedCement.ContractualMeteredTypeOne = Math.Round(item.ContractualMeteredTypeOne * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedCement.ContractualMeteredTypeFive = Math.Round(item.ContractualMeteredTypeFive * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedCement.ContractualRestatedTypeOne = Math.Round(item.ContractualRestatedTypeOne * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedCement.ContractualRestatedTypeFive = Math.Round(item.ContractualRestatedTypeFive * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                        }

                        consolidados.Add(consolidatedCement);
                        count++;
                    }
                    else
                    {
                        existe.Metered = 0.0;
                        existe.ContractualMeteredTypeOne = item.ContractualMeteredTypeOne;
                        existe.ContractualMeteredTypeFive = item.ContractualMeteredTypeFive;
                        existe.ContractualRestatedTypeOne = item.ContractualRestatedTypeOne;
                        existe.ContractualRestatedTypeFive = item.ContractualRestatedTypeFive;

                        if (formula.Code == "F1" && model.AffectationF1 == 2)
                        {
                            existe.Metered = budgetCD.Metered;
                            existe.ContractualMeteredTypeOne = Math.Round(item.ContractualMeteredTypeOne * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.ContractualMeteredTypeFive = Math.Round(item.ContractualMeteredTypeFive * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.ContractualRestatedTypeOne = Math.Round(item.ContractualRestatedTypeOne * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.ContractualRestatedTypeFive = Math.Round(item.ContractualRestatedTypeFive * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                        }

                        if (formula.Code == "F2" && model.AffectationF2 == 2)
                        {
                            existe.Metered = budgetCD.Metered;
                            existe.ContractualMeteredTypeOne = Math.Round(item.ContractualMeteredTypeOne * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.ContractualMeteredTypeFive = Math.Round(item.ContractualMeteredTypeFive * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.ContractualRestatedTypeOne = Math.Round(item.ContractualRestatedTypeOne * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.ContractualRestatedTypeFive = Math.Round(item.ContractualRestatedTypeFive * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                        }

                        if (formula.Code == "F3" && model.AffectationF3 == 2)
                        {
                            existe.Metered = budgetCD.Metered;
                            existe.ContractualMeteredTypeOne = Math.Round(item.ContractualMeteredTypeOne * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.ContractualMeteredTypeFive = Math.Round(item.ContractualMeteredTypeFive * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.ContractualRestatedTypeOne = Math.Round(item.ContractualRestatedTypeOne * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.ContractualRestatedTypeFive = Math.Round(item.ContractualRestatedTypeFive * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                        }

                        if (formula.Code == "F4" && model.AffectationF4 == 2)
                        {
                            existe.Metered = budgetCD.Metered;
                            existe.ContractualMeteredTypeOne = Math.Round(item.ContractualMeteredTypeOne * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.ContractualMeteredTypeFive = Math.Round(item.ContractualMeteredTypeFive * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.ContractualRestatedTypeOne = Math.Round(item.ContractualRestatedTypeOne * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.ContractualRestatedTypeFive = Math.Round(item.ContractualRestatedTypeFive * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                        }

                        if (formula.Code == "F5" && model.AffectationF5 == 2)
                        {
                            existe.Metered = budgetCD.Metered;
                            existe.ContractualMeteredTypeOne = Math.Round(item.ContractualMeteredTypeOne * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.ContractualMeteredTypeFive = Math.Round(item.ContractualMeteredTypeFive * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.ContractualRestatedTypeOne = Math.Round(item.ContractualRestatedTypeOne * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.ContractualRestatedTypeFive = Math.Round(item.ContractualRestatedTypeFive * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                        }

                        if (formula.Code == "F6" && model.AffectationF6 == 2)
                        {
                            existe.Metered = budgetCD.Metered;
                            existe.ContractualMeteredTypeOne = Math.Round(item.ContractualMeteredTypeOne * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.ContractualMeteredTypeFive = Math.Round(item.ContractualMeteredTypeFive * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.ContractualRestatedTypeOne = Math.Round(item.ContractualRestatedTypeOne * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.ContractualRestatedTypeFive = Math.Round(item.ContractualRestatedTypeFive * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                        }

                        if (formula.Code == "F7" && model.AffectationF7 == 2)
                        {
                            existe.Metered = budgetCD.Metered;
                            existe.ContractualMeteredTypeOne = Math.Round(item.ContractualMeteredTypeOne * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.ContractualMeteredTypeFive = Math.Round(item.ContractualMeteredTypeFive * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.ContractualRestatedTypeOne = Math.Round(item.ContractualRestatedTypeOne * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.ContractualRestatedTypeFive = Math.Round(item.ContractualRestatedTypeFive * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                        }

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
                await _context.ConsolidatedCements.AddRangeAsync(consolidados);
            //_context.ConsolidatedCements.UpdateRange(consolidadosUpdate);
            await _context.SaveChangesAsync();

            return Ok();
        }


        [HttpGet("metrado-contractual-tipo-uno")]
        public async Task<IActionResult> GetContractualMeteredTypeOneSuma(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var query = _context.ConsolidatedCements
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

            var query = _context.ConsolidatedCements
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

            var query = _context.ConsolidatedCements
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

            var query = _context.ConsolidatedCements
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

            var query = _context.ConsolidatedCements
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

            _context.ConsolidatedCements.RemoveRange(data);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("f1")]
        public async Task<IActionResult> GetF1()
        {
            var pId = GetProjectId();

            var f1 = await _context.ProjectFormulas
                .FirstOrDefaultAsync(x => x.ProjectId == pId && x.Code == "F1");

            var steel = await _context.ConsolidatedCements
                .FirstOrDefaultAsync(x => x.ProjectFormulaId == f1.Id && x.Unit != "");

            var res = 1;

            if (steel != null && steel.Metered != 0)
                res = 2;

            return Ok(res);
        }

        [HttpGet("f2")]
        public async Task<IActionResult> GetF2()
        {
            var pId = GetProjectId();

            var f2 = await _context.ProjectFormulas
                .FirstOrDefaultAsync(x => x.ProjectId == pId && x.Code == "F2");

            var steel = await _context.ConsolidatedCements
                .FirstOrDefaultAsync(x => x.ProjectFormulaId == f2.Id && x.Unit != "");

            var res = 1;

            if (steel != null && steel.Metered != 0)
                res = 2;

            return Ok(res);
        }

        [HttpGet("f3")]
        public async Task<IActionResult> GetF3()
        {
            var pId = GetProjectId();

            var f3 = await _context.ProjectFormulas
                .FirstOrDefaultAsync(x => x.ProjectId == pId && x.Code == "F3");

            var steel = await _context.ConsolidatedCements
                .FirstOrDefaultAsync(x => x.ProjectFormulaId == f3.Id && x.Unit != "");

            var res = 1;

            if (steel != null && steel.Metered != 0)
                res = 2;

            return Ok(res);
        }

        [HttpGet("f4")]
        public async Task<IActionResult> GetF4()
        {
            var pId = GetProjectId();

            var f4 = await _context.ProjectFormulas
                .FirstOrDefaultAsync(x => x.ProjectId == pId && x.Code == "F4");

            var steel = await _context.ConsolidatedCements
                .FirstOrDefaultAsync(x => x.ProjectFormulaId == f4.Id && x.Unit != "");

            var res = 1;

            if (steel != null && steel.Metered != 0)
                res = 2;

            return Ok(res);
        }

        [HttpGet("f5")]
        public async Task<IActionResult> GetF5()
        {
            var pId = GetProjectId();

            var f5 = await _context.ProjectFormulas
                .FirstOrDefaultAsync(x => x.ProjectId == pId && x.Code == "F5");

            var steel = await _context.ConsolidatedCements
                .FirstOrDefaultAsync(x => x.ProjectFormulaId == f5.Id && x.Unit != "");

            var res = 1;

            if (steel != null && steel.Metered != 0)
                res = 2;

            return Ok(res);
        }

        [HttpGet("f6")]
        public async Task<IActionResult> GetF6()
        {
            var pId = GetProjectId();

            var f6 = await _context.ProjectFormulas
                .FirstOrDefaultAsync(x => x.ProjectId == pId && x.Code == "F6");

            var steel = await _context.ConsolidatedCements
                .FirstOrDefaultAsync(x => x.ProjectFormulaId == f6.Id && x.Unit != "");

            var res = 1;

            if (steel != null && steel.Metered != 0)
                res = 2;

            return Ok(res);
        }

        [HttpGet("f7")]
        public async Task<IActionResult> GetF7()
        {
            var pId = GetProjectId();

            var f7 = await _context.ProjectFormulas
                .FirstOrDefaultAsync(x => x.ProjectId == pId && x.Code == "F7");

            var steel = await _context.ConsolidatedCements
                .FirstOrDefaultAsync(x => x.ProjectFormulaId == f7.Id && x.Unit != "");

            var res = 1;

            if (steel != null && steel.Metered != 0)
                res = 2;

            return Ok(res);
        }


    }
}
