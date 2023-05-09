using ClosedXML.Excel;
using EFCore.BulkExtensions;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.ConsolidatedSteelViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.SteelViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.TECHNICAL_OFFICE)]
    [Route("oficina-tecnica/consolidado-aceros")]
    public class ConsolidatedSteelController : BaseController
    {
        public ConsolidatedSteelController(IvcDbContext context, 
            ILogger<ConsolidatedSteelController> logger): base(context, logger)
        {

        }

        public IActionResult Index() => View();
        
        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var query = _context.ConsolidatedSteels
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
                .Select(x => new ConsolidatedSteelViewModel
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
                    Rod6mm = x.Rod6mm.ToString("N0", CultureInfo.InvariantCulture),
                    Rod8mm = x.Rod8mm.ToString("N0", CultureInfo.InvariantCulture),
                    Rod3x8 = x.Rod3x8.ToString("N0", CultureInfo.InvariantCulture),
                    Rod1x2 = x.Rod1x2.ToString("N0", CultureInfo.InvariantCulture),
                    Rod5x8 = x.Rod5x8.ToString("N0", CultureInfo.InvariantCulture),
                    Rod3x4 = x.Rod3x4.ToString("N0", CultureInfo.InvariantCulture),
                    Rod1 = x.Rod1.ToString("N0", CultureInfo.InvariantCulture),
                    ContractualStaked = x.ContractualStaked.ToString("N", CultureInfo.InvariantCulture)
                }).AsNoTracking()
                .ToListAsync();

            return Ok(steels);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var query = await _context.ConsolidatedSteels
                .Select(x => new ConsolidatedSteelViewModel
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
                    Rod6mm = x.Rod6mm.ToString("N0", CultureInfo.InvariantCulture),
                    Rod8mm = x.Rod8mm.ToString("N0", CultureInfo.InvariantCulture),
                    Rod3x8 = x.Rod3x8.ToString("N0", CultureInfo.InvariantCulture),
                    Rod1x2 = x.Rod1x2.ToString("N0", CultureInfo.InvariantCulture),
                    Rod5x8 = x.Rod5x8.ToString("N0", CultureInfo.InvariantCulture),
                    Rod3x4 = x.Rod3x4.ToString("N0", CultureInfo.InvariantCulture),
                    Rod1 = x.Rod1.ToString("N0", CultureInfo.InvariantCulture),
                    ContractualStaked = x.ContractualStaked.ToString("N", CultureInfo.InvariantCulture)
                }).AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return Ok(query);
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, ConsolidatedSteelViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var steel = await _context.ConsolidatedSteels.FirstOrDefaultAsync(x => x.Id == id);

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
            var steel = await _context.Steels.FirstOrDefaultAsync(x => x.Id == id);

            if (steel == null)
                return BadRequest("No se ha encontrado el acero");

            _context.Steels.Remove(steel);
            await _context.SaveChangesAsync();

            return Ok();
        }

       [HttpPost("afectacion")]
       public async Task<IActionResult> Load(LoadConsolidatedSteelViewModel model)
        {
            var consolidados = new List<ConsolidatedSteel>();

            var pId = GetProjectId();

            var consolidatedSteels = await _context.ConsolidatedSteels
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == GetProjectId())
                .ToListAsync();

            var meteredRestatedByStreetchs = await _context.MeteredsRestatedByStreetchs
                .Include(x => x.BudgetTitle)
                .Where(x => x.ProjectId == pId)
                .ToListAsync();

            var steels = await _context.Steels
                .Include(x => x.ProjectFormula)
                .Where(x=>x.ProjectFormula.ProjectId == pId)
                .OrderBy(x => x.OrderNumber).ToListAsync();

            var projectFormulas = await _context.ProjectFormulas
                .Where(x => x.ProjectId == pId)
                .ToListAsync();

            var count = _context.ConsolidatedSteels.Count();

            foreach (var item in steels)
            {

                var consolidatedSteel = new ConsolidatedSteel();

                var existe = consolidatedSteels.FirstOrDefault(x => x.ItemNumber == item.ItemNumber 
                && x.BudgetTitleId == item.BudgetTitleId);

                if (item.ContractualMetered != 0)
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
                        consolidatedSteel.Id = Guid.NewGuid();
                        consolidatedSteel.BudgetTitleId = item.BudgetTitleId;
                        consolidatedSteel.ProjectFormulaId = item.ProjectFormulaId;
                        consolidatedSteel.ProjectPhaseId = item.ProjectPhaseId;
                        consolidatedSteel.WorkFrontId = item.WorkFrontId;
                        consolidatedSteel.OrderNumber = count;
                        consolidatedSteel.ItemNumber = item.ItemNumber;
                        consolidatedSteel.Description = item.Description;
                        consolidatedSteel.Unit = item.Unit;
                        consolidatedSteel.ContractualMetered = item.ContractualMetered;
                        consolidatedSteel.Rod6mm = item.Rod6mm;
                        consolidatedSteel.Rod8mm = item.Rod8mm;
                        consolidatedSteel.Rod3x8 = item.Rod3x8;
                        consolidatedSteel.Rod1x2 = item.Rod1x2;
                        consolidatedSteel.Rod5x8 = item.Rod5x8;
                        consolidatedSteel.Rod3x4 = item.Rod3x4;
                        consolidatedSteel.Rod1 = item.Rod1;
                        consolidatedSteel.ContractualStaked = item.ContractualStaked;

                        if (formula.Code == "F1" && model.AffectationF1 == 2)
                        {
                            consolidatedSteel.Metered = budgetCD.Metered;
                            consolidatedSteel.ContractualMetered = Math.Round(item.ContractualMetered * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedSteel.Rod6mm = Math.Round(item.Rod6mm * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedSteel.Rod8mm = Math.Round(item.Rod8mm * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedSteel.Rod3x8 = Math.Round(item.Rod3x8 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedSteel.Rod1x2 = Math.Round(item.Rod1x2 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedSteel.Rod5x8 = Math.Round(item.Rod5x8 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedSteel.Rod3x4 = Math.Round(item.Rod3x4 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedSteel.Rod1 = Math.Round(item.Rod1 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedSteel.ContractualStaked = Math.Round(item.ContractualStaked * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                        }

                        if (formula.Code == "F2" && model.AffectationF2 == 2)
                        {
                            consolidatedSteel.Metered = budgetCD.Metered;
                            consolidatedSteel.ContractualMetered = Math.Round(item.ContractualMetered * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedSteel.Rod6mm = Math.Round(item.Rod6mm * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedSteel.Rod8mm = Math.Round(item.Rod8mm * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedSteel.Rod3x8 = Math.Round(item.Rod3x8 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedSteel.Rod1x2 = Math.Round(item.Rod1x2 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedSteel.Rod5x8 = Math.Round(item.Rod5x8 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedSteel.Rod3x4 = Math.Round(item.Rod3x4 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedSteel.Rod1 = Math.Round(item.Rod1 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedSteel.ContractualStaked = Math.Round(item.ContractualStaked * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                        }

                        if (formula.Code == "F3" && model.AffectationF3 == 2)
                        {
                            consolidatedSteel.Metered = budgetCD.Metered;
                            consolidatedSteel.ContractualMetered = Math.Round(item.ContractualMetered * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedSteel.Rod6mm = Math.Round(item.Rod6mm * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedSteel.Rod8mm = Math.Round(item.Rod8mm * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedSteel.Rod3x8 = Math.Round(item.Rod3x8 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedSteel.Rod1x2 = Math.Round(item.Rod1x2 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedSteel.Rod5x8 = Math.Round(item.Rod5x8 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedSteel.Rod3x4 = Math.Round(item.Rod3x4 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedSteel.Rod1 = Math.Round(item.Rod1 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedSteel.ContractualStaked = Math.Round(item.ContractualStaked * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                        }

                        if (formula.Code == "F4" && model.AffectationF4 == 2)
                        {
                            consolidatedSteel.Metered = budgetCD.Metered;
                            consolidatedSteel.ContractualMetered = Math.Round(item.ContractualMetered * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedSteel.Rod6mm = Math.Round(item.Rod6mm * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedSteel.Rod8mm = Math.Round(item.Rod8mm * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedSteel.Rod3x8 = Math.Round(item.Rod3x8 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedSteel.Rod1x2 = Math.Round(item.Rod1x2 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedSteel.Rod5x8 = Math.Round(item.Rod5x8 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedSteel.Rod3x4 = Math.Round(item.Rod3x4 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedSteel.Rod1 = Math.Round(item.Rod1 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedSteel.ContractualStaked = Math.Round(item.ContractualStaked * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                        }

                        if (formula.Code == "F5" && model.AffectationF5 == 2)
                        {
                            consolidatedSteel.Metered = budgetCD.Metered;
                            consolidatedSteel.ContractualMetered = Math.Round(item.ContractualMetered * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedSteel.Rod6mm = Math.Round(item.Rod6mm * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedSteel.Rod8mm = Math.Round(item.Rod8mm * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedSteel.Rod3x8 = Math.Round(item.Rod3x8 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedSteel.Rod1x2 = Math.Round(item.Rod1x2 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedSteel.Rod5x8 = Math.Round(item.Rod5x8 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedSteel.Rod3x4 = Math.Round(item.Rod3x4 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedSteel.Rod1 = Math.Round(item.Rod1 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedSteel.ContractualStaked = Math.Round(item.ContractualStaked * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                        }

                        if (formula.Code == "F6" && model.AffectationF6 == 2)
                        {
                            consolidatedSteel.Metered = budgetCD.Metered;
                            consolidatedSteel.ContractualMetered = Math.Round(item.ContractualMetered * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedSteel.Rod6mm = Math.Round(item.Rod6mm * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedSteel.Rod8mm = Math.Round(item.Rod8mm * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedSteel.Rod3x8 = Math.Round(item.Rod3x8 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedSteel.Rod1x2 = Math.Round(item.Rod1x2 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedSteel.Rod5x8 = Math.Round(item.Rod5x8 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedSteel.Rod3x4 = Math.Round(item.Rod3x4 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedSteel.Rod1 = Math.Round(item.Rod1 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedSteel.ContractualStaked = Math.Round(item.ContractualStaked * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                        }

                        if (formula.Code == "F7" && model.AffectationF7 == 2)
                        {
                            consolidatedSteel.Metered = budgetCD.Metered;
                            consolidatedSteel.ContractualMetered = Math.Round(item.ContractualMetered * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedSteel.Rod6mm = Math.Round(item.Rod6mm * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedSteel.Rod8mm = Math.Round(item.Rod8mm * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedSteel.Rod3x8 = Math.Round(item.Rod3x8 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedSteel.Rod1x2 = Math.Round(item.Rod1x2 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedSteel.Rod5x8 = Math.Round(item.Rod5x8 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedSteel.Rod3x4 = Math.Round(item.Rod3x4 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedSteel.Rod1 = Math.Round(item.Rod1 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedSteel.ContractualStaked = Math.Round(item.ContractualStaked * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                        }

                        consolidados.Add(consolidatedSteel);
                        count++;
                    }
                    else
                    {
                        existe.Metered = 0.0;
                        existe.ContractualMetered = item.ContractualMetered;
                        existe.Rod6mm = item.Rod6mm;
                        existe.Rod8mm = item.Rod8mm;
                        existe.Rod3x8 = item.Rod3x8;
                        existe.Rod1x2 = item.Rod1x2;
                        existe.Rod5x8 = item.Rod5x8;
                        existe.Rod3x4 = item.Rod3x4;
                        existe.Rod1 = item.Rod1;
                        existe.ContractualStaked = item.ContractualStaked;

                        if (formula.Code == "F1" && model.AffectationF1 == 2)
                        {
                            existe.Metered = budgetCD.Metered;
                            existe.ContractualMetered = Math.Round(item.ContractualMetered * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Rod6mm = Math.Round(item.Rod6mm * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Rod8mm = Math.Round(item.Rod8mm * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Rod3x8 = Math.Round(item.Rod3x8 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Rod1x2 = Math.Round(item.Rod1x2 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Rod5x8 = Math.Round(item.Rod5x8 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Rod3x4 = Math.Round(item.Rod3x4 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Rod1 = Math.Round(item.Rod1 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.ContractualStaked = Math.Round(item.ContractualStaked * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                        }

                        if (formula.Code == "F2" && model.AffectationF2 == 2)
                        {
                            existe.Metered = budgetCD.Metered;
                            existe.ContractualMetered = Math.Round(item.ContractualMetered * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Rod6mm = Math.Round(item.Rod6mm * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Rod8mm = Math.Round(item.Rod8mm * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Rod3x8 = Math.Round(item.Rod3x8 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Rod1x2 = Math.Round(item.Rod1x2 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Rod5x8 = Math.Round(item.Rod5x8 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Rod3x4 = Math.Round(item.Rod3x4 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Rod1 = Math.Round(item.Rod1 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.ContractualStaked = Math.Round(item.ContractualStaked * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                        }

                        if (formula.Code == "F3" && model.AffectationF3 == 2)
                        {
                            existe.Metered = budgetCD.Metered;
                            existe.ContractualMetered = Math.Round(item.ContractualMetered * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Rod6mm = Math.Round(item.Rod6mm * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Rod8mm = Math.Round(item.Rod8mm * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Rod3x8 = Math.Round(item.Rod3x8 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Rod1x2 = Math.Round(item.Rod1x2 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Rod5x8 = Math.Round(item.Rod5x8 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Rod3x4 = Math.Round(item.Rod3x4 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Rod1 = Math.Round(item.Rod1 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.ContractualStaked = Math.Round(item.ContractualStaked * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                        }

                        if (formula.Code == "F4" && model.AffectationF4 == 2)
                        {
                            existe.Metered = budgetCD.Metered;
                            existe.ContractualMetered = Math.Round(item.ContractualMetered * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Rod6mm = Math.Round(item.Rod6mm * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Rod8mm = Math.Round(item.Rod8mm * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Rod3x8 = Math.Round(item.Rod3x8 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Rod1x2 = Math.Round(item.Rod1x2 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Rod5x8 = Math.Round(item.Rod5x8 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Rod3x4 = Math.Round(item.Rod3x4 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Rod1 = Math.Round(item.Rod1 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.ContractualStaked = Math.Round(item.ContractualStaked * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                        }

                        if (formula.Code == "F5" && model.AffectationF5 == 2)
                        {
                            existe.Metered = budgetCD.Metered;
                            existe.ContractualMetered = Math.Round(item.ContractualMetered * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Rod6mm = Math.Round(item.Rod6mm * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Rod8mm = Math.Round(item.Rod8mm * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Rod3x8 = Math.Round(item.Rod3x8 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Rod1x2 = Math.Round(item.Rod1x2 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Rod5x8 = Math.Round(item.Rod5x8 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Rod3x4 = Math.Round(item.Rod3x4 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Rod1 = Math.Round(item.Rod1 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.ContractualStaked = Math.Round(item.ContractualStaked * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                        }

                        if (formula.Code == "F6" && model.AffectationF6 == 2)
                        {
                            existe.Metered = budgetCD.Metered;
                            existe.ContractualMetered = Math.Round(item.ContractualMetered * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Rod6mm = Math.Round(item.Rod6mm * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Rod8mm = Math.Round(item.Rod8mm * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Rod3x8 = Math.Round(item.Rod3x8 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Rod1x2 = Math.Round(item.Rod1x2 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Rod5x8 = Math.Round(item.Rod5x8 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Rod3x4 = Math.Round(item.Rod3x4 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Rod1 = Math.Round(item.Rod1 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.ContractualStaked = Math.Round(item.ContractualStaked * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                        }

                        if (formula.Code == "F7" && model.AffectationF7 == 2)
                        {
                            existe.Metered = budgetCD.Metered;
                            existe.ContractualMetered = Math.Round(item.ContractualMetered * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Rod6mm = Math.Round(item.Rod6mm * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Rod8mm = Math.Round(item.Rod8mm * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Rod3x8 = Math.Round(item.Rod3x8 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Rod1x2 = Math.Round(item.Rod1x2 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Rod5x8 = Math.Round(item.Rod5x8 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Rod3x4 = Math.Round(item.Rod3x4 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Rod1 = Math.Round(item.Rod1 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.ContractualStaked = Math.Round(item.ContractualStaked * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                        }

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
                await _context.ConsolidatedSteels.AddRangeAsync(consolidados);
            //_context.ConsolidatedSteels.UpdateRange(consolidadosUpdate);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("metrado-contractual")]
        public async Task<IActionResult> GetContractualMetered(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var query = _context.ConsolidatedSteels
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

            var query = _context.ConsolidatedSteels
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

            return Ok(rod6mmSuma.ToString("N0", CultureInfo.InvariantCulture));
        }

        [HttpGet("rod8mm")]
        public async Task<IActionResult> GetRod8mm(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var query = _context.ConsolidatedSteels
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

            return Ok(rod8mmSuma.ToString("N0", CultureInfo.InvariantCulture));
        }

        [HttpGet("rod3x8")]
        public async Task<IActionResult> GetRod3x8(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var query = _context.ConsolidatedSteels
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

            return Ok(rod3x8Suma.ToString("N0", CultureInfo.InvariantCulture));
        }

        [HttpGet("rod1x2")]
        public async Task<IActionResult> GetRod1x2(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var query = _context.ConsolidatedSteels
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

            return Ok(rod1x2Suma.ToString("N0", CultureInfo.InvariantCulture));
        }

        [HttpGet("rod5x8")]
        public async Task<IActionResult> GetRod5x8(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var query = _context.ConsolidatedSteels
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

            return Ok(rod5x8Suma.ToString("N0", CultureInfo.InvariantCulture));
        }

        [HttpGet("rod3x4")]
        public async Task<IActionResult> GetRod3x4(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var query = _context.ConsolidatedSteels
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

            return Ok(rod3x4Suma.ToString("N0", CultureInfo.InvariantCulture));
        }

        [HttpGet("rod1")]
        public async Task<IActionResult> GetRod1(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var query = _context.ConsolidatedSteels
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

            return Ok(rod1Suma.ToString("N0", CultureInfo.InvariantCulture));
        }

        [HttpGet("metrado-replanteado")]
        public async Task<IActionResult> GetcontractualStaked(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var query = _context.ConsolidatedSteels
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

        [HttpDelete("eliminar-filtro")]
        public async Task<IActionResult> DeleteByFilters(Guid? projectFormulaId = null, Guid? budgetTitleId = null)
        {
            var pId = GetProjectId();

            var query = _context.ConsolidatedSteels
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

            _context.ConsolidatedSteels.RemoveRange(data);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("f1")]
        public async Task<IActionResult> GetF1()
        {
            var f1 = await _context.ProjectFormulas.FirstOrDefaultAsync(x => x.ProjectId == GetProjectId() && x.Code == "F1");

            var steel = await _context.ConsolidatedSteels.FirstOrDefaultAsync(x => x.ProjectFormulaId == f1.Id && x.Unit != "");

            var res = 1;

            if (steel != null && steel.Metered != 0)
                res = 2;

            return Ok(res);
        }

        [HttpGet("f2")]
        public async Task<IActionResult> GetF2()
        {
            var f2 = await _context.ProjectFormulas.FirstOrDefaultAsync(x => x.ProjectId == GetProjectId() && x.Code == "F2");

            var steel = await _context.ConsolidatedSteels.FirstOrDefaultAsync(x => x.ProjectFormulaId == f2.Id && x.Unit != "");

            var res = 1;

            if (steel != null && steel.Metered != 0)
                res = 2;

            return Ok(res);
        }

        [HttpGet("f3")]
        public async Task<IActionResult> GetF3()
        {
            var f3 = await _context.ProjectFormulas.FirstOrDefaultAsync(x => x.ProjectId == GetProjectId() && x.Code == "F3");

            var steel = await _context.ConsolidatedSteels.FirstOrDefaultAsync(x => x.ProjectFormulaId == f3.Id && x.Unit != "");

            var res = 1;

            if (steel != null && steel.Metered != 0)
                res = 2;

            return Ok(res);
        }

        [HttpGet("f4")]
        public async Task<IActionResult> GetF4()
        {
            var f4 = await _context.ProjectFormulas.FirstOrDefaultAsync(x => x.ProjectId == GetProjectId() && x.Code == "F4");

            var steel = await _context.ConsolidatedSteels.FirstOrDefaultAsync(x => x.ProjectFormulaId == f4.Id && x.Unit != "");

            var res = 1;

            if (steel != null && steel.Metered != 0)
                res = 2;

            return Ok(res);
        }

        [HttpGet("f5")]
        public async Task<IActionResult> GetF5()
        {
            var f5 = await _context.ProjectFormulas.FirstOrDefaultAsync(x => x.ProjectId == GetProjectId() && x.Code == "F5");

            var steel = await _context.ConsolidatedSteels.FirstOrDefaultAsync(x => x.ProjectFormulaId == f5.Id && x.Unit != "");

            var res = 1;

            if (steel != null && steel.Metered != 0)
                res = 2;

            return Ok(res);
        }

        [HttpGet("f6")]
        public async Task<IActionResult> GetF6()
        {
            var f6 = await _context.ProjectFormulas.FirstOrDefaultAsync(x => x.ProjectId == GetProjectId() && x.Code == "F6");

            var steel = await _context.ConsolidatedSteels.FirstOrDefaultAsync(x => x.ProjectFormulaId == f6.Id && x.Unit != "");

            var res = 1;

            if (steel != null && steel.Metered != 0)
                res = 2;

            return Ok(res);
        }

        [HttpGet("f7")]
        public async Task<IActionResult> GetF7()
        {
            var f7 = await _context.ProjectFormulas.FirstOrDefaultAsync(x => x.ProjectId == GetProjectId() && x.Code == "F7");

            var steel = await _context.ConsolidatedSteels.FirstOrDefaultAsync(x => x.ProjectFormulaId == f7.Id && x.Unit != "");

            var res = 1;

            if (steel != null && steel.Metered != 0)
                res = 2;

            return Ok(res);
        }
         
    }
}
       