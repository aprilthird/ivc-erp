using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.ConsolidatedEntibadoViewModels;
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
    [Route("oficina-tecnica/consolidado-entibados")]
    public class ConsolidatedEntibadoController : BaseController
    {
        public ConsolidatedEntibadoController(IvcDbContext context,
       ILogger<ConsolidatedEntibadoController> logger) : base(context, logger)
        {

        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var query = _context.ConsolidatedEntibados
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

            var entibados = await query
                .Include(x => x.WorkFront)
                .OrderBy(x => x.OrderNumber)
                .Select(x => new ConsolidatedEntibadoViewModel
                {
                    Id = x.Id,
                    BudgetTitleId = x.BudgetTitleId,
                    ProjectFormulaId = x.ProjectFormulaId,
                    ProjectPhaseId = x.ProjectPhaseId,
                    WorkFront = new WorkFrontViewModel
                    {
                        Code = x.WorkFront.Code
                    },
                    ItemNumber = x.ItemNumber,
                    Description = x.Description,
                    Unit = x.Unit,
                    Metered = x.Metered.ToString("N", CultureInfo.InvariantCulture),
                    Performance = x.Performance.ToString("N", CultureInfo.InvariantCulture),
                    KS60xMinibox = x.KS60xMinibox.ToString("N", CultureInfo.InvariantCulture),
                    KS100xKMC100 = x.KS100xKMC100.ToString("N", CultureInfo.InvariantCulture),
                    RealzaxExtension = x.RealzaxExtension.ToString("N", CultureInfo.InvariantCulture),
                    Corredera = x.Corredera.ToString("N", CultureInfo.InvariantCulture),
                    Paralelo = x.Paralelo.ToString("N", CultureInfo.InvariantCulture)
                }).AsNoTracking()
                .ToListAsync();

            return Ok(entibados);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var query = await _context.ConsolidatedEntibados
                .Select(x => new ConsolidatedEntibadoViewModel
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
                    Performance = x.Performance.ToString("N", CultureInfo.InvariantCulture),
                    KS60xMinibox = x.KS60xMinibox.ToString("N", CultureInfo.InvariantCulture),
                    KS100xKMC100 = x.KS100xKMC100.ToString("N", CultureInfo.InvariantCulture),
                    RealzaxExtension = x.RealzaxExtension.ToString("N", CultureInfo.InvariantCulture),
                    Corredera = x.Corredera.ToString("N", CultureInfo.InvariantCulture),
                    Paralelo = x.Paralelo.ToString("N", CultureInfo.InvariantCulture)
                }).AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return Ok(query);
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, ConsolidatedEntibadoViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entibado = await _context.ConsolidatedEntibados.FirstOrDefaultAsync(x => x.Id == id);

            entibado.ItemNumber = model.ItemNumber;
            entibado.Description = model.Description;
            entibado.Unit = model.Unit;
            if (model.Unit == null)
                entibado.Unit = "";
            entibado.Metered = model.Metered.ToDoubleString();
            entibado.Performance = model.Performance.ToDoubleString();
            entibado.KS60xMinibox = model.KS60xMinibox.ToDoubleString();
            entibado.KS100xKMC100 = model.KS100xKMC100.ToDoubleString();
            entibado.RealzaxExtension = model.RealzaxExtension.ToDoubleString();
            entibado.Corredera = model.Corredera.ToDoubleString();
            entibado.Paralelo = model.Paralelo.ToDoubleString();

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var entibado = await _context.ConsolidatedEntibados.FirstOrDefaultAsync(x => x.Id == id);

            if (entibado == null)
                return BadRequest("No se ha encontrado el acero");

            _context.ConsolidatedEntibados.Remove(entibado);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("afectacion")]
        public async Task<IActionResult> ImportData(LoadConsolidatedSteelViewModel model)
        {
            var consolidados = new List<ConsolidatedEntibado>();

            var pId = GetProjectId();

            var consolidatedEntibados = await _context.ConsolidatedEntibados
               .Include(x => x.ProjectFormula)
               .Where(x => x.ProjectFormula.ProjectId == GetProjectId())
               .ToListAsync();

            var meteredRestatedByStreetchs = await _context.MeteredsRestatedByStreetchs
                .Include(x => x.BudgetTitle)
                .Where(x => x.ProjectId == pId)
                .ToListAsync();

            var entibados = await _context.Entibados
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId)
                .OrderBy(x => x.OrderNumber).ToListAsync();

            var projectFormulas = await _context.ProjectFormulas
               .Where(x => x.ProjectId == pId)
               .ToListAsync();

            var count = _context.ConsolidatedEntibados.Count();

            foreach (var item in entibados)
            {

                var consolidatedEntibado = new ConsolidatedEntibado();

                var existe = consolidatedEntibados.FirstOrDefault(x => x.ItemNumber == item.ItemNumber 
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
                        consolidatedEntibado.Id = Guid.NewGuid();
                        consolidatedEntibado.BudgetTitleId = item.BudgetTitleId;
                        consolidatedEntibado.ProjectFormulaId = item.ProjectFormulaId;
                        consolidatedEntibado.ProjectPhaseId = item.ProjectPhaseId;
                        consolidatedEntibado.WorkFrontId = item.WorkFrontId;
                        consolidatedEntibado.OrderNumber = count;
                        consolidatedEntibado.ItemNumber = item.ItemNumber;
                        consolidatedEntibado.Description = item.Description;
                        consolidatedEntibado.Unit = item.Unit;
                        consolidatedEntibado.KS60xMinibox = item.KS60xMinibox;
                        consolidatedEntibado.KS100xKMC100 = item.KS100xKMC100;
                        consolidatedEntibado.RealzaxExtension = item.RealzaxExtension;
                        consolidatedEntibado.Corredera = item.Corredera;

                        if (formula.Code == "F1" && model.AffectationF1 == 2)
                        {
                            consolidatedEntibado.Metered = budgetCD.Metered;
                            consolidatedEntibado.KS60xMinibox = Math.Round(item.KS60xMinibox * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedEntibado.KS100xKMC100 = Math.Round(item.KS100xKMC100 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedEntibado.RealzaxExtension = Math.Round(item.RealzaxExtension * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedEntibado.Corredera = Math.Round(item.Corredera * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedEntibado.Paralelo = Math.Round(item.Paralelo * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                        }

                        if (formula.Code == "F2" && model.AffectationF2 == 2)
                        {
                            consolidatedEntibado.Metered = budgetCD.Metered;
                            consolidatedEntibado.KS60xMinibox = Math.Round(item.KS60xMinibox * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedEntibado.KS100xKMC100 = Math.Round(item.KS100xKMC100 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedEntibado.RealzaxExtension = Math.Round(item.RealzaxExtension * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedEntibado.Corredera = Math.Round(item.Corredera * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedEntibado.Paralelo = Math.Round(item.Paralelo * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                        }

                        if (formula.Code == "F3" && model.AffectationF3 == 2)
                        {
                            consolidatedEntibado.Metered = budgetCD.Metered;
                            consolidatedEntibado.KS60xMinibox = Math.Round(item.KS60xMinibox * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedEntibado.KS100xKMC100 = Math.Round(item.KS100xKMC100 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedEntibado.RealzaxExtension = Math.Round(item.RealzaxExtension * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedEntibado.Corredera = Math.Round(item.Corredera * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedEntibado.Paralelo = Math.Round(item.Paralelo * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                        }

                        if (formula.Code == "F4" && model.AffectationF4 == 2)
                        {
                            consolidatedEntibado.Metered = budgetCD.Metered;
                            consolidatedEntibado.KS60xMinibox = Math.Round(item.KS60xMinibox * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedEntibado.KS100xKMC100 = Math.Round(item.KS100xKMC100 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedEntibado.RealzaxExtension = Math.Round(item.RealzaxExtension * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedEntibado.Corredera = Math.Round(item.Corredera * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedEntibado.Paralelo = Math.Round(item.Paralelo * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                        }

                        if (formula.Code == "F5" && model.AffectationF5 == 2)
                        {
                            consolidatedEntibado.Metered = budgetCD.Metered;
                            consolidatedEntibado.KS60xMinibox = Math.Round(item.KS60xMinibox * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedEntibado.KS100xKMC100 = Math.Round(item.KS100xKMC100 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedEntibado.RealzaxExtension = Math.Round(item.RealzaxExtension * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedEntibado.Corredera = Math.Round(item.Corredera * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedEntibado.Paralelo = Math.Round(item.Paralelo * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                        }

                        if (formula.Code == "F6" && model.AffectationF6 == 2)
                        {
                            consolidatedEntibado.Metered = budgetCD.Metered;
                            consolidatedEntibado.KS60xMinibox = Math.Round(item.KS60xMinibox * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedEntibado.KS100xKMC100 = Math.Round(item.KS100xKMC100 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedEntibado.RealzaxExtension = Math.Round(item.RealzaxExtension * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedEntibado.Corredera = Math.Round(item.Corredera * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedEntibado.Paralelo = Math.Round(item.Paralelo * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                        }

                        if (formula.Code == "F7" && model.AffectationF7 == 2)
                        {
                            consolidatedEntibado.Metered = budgetCD.Metered;
                            consolidatedEntibado.KS60xMinibox = Math.Round(item.KS60xMinibox * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedEntibado.KS100xKMC100 = Math.Round(item.KS100xKMC100 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedEntibado.RealzaxExtension = Math.Round(item.RealzaxExtension * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedEntibado.Corredera = Math.Round(item.Corredera * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            consolidatedEntibado.Paralelo = Math.Round(item.Paralelo * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                        }

                        consolidados.Add(consolidatedEntibado);
                        count++;
                    }
                    else
                    {
                        existe.Metered = 0.0;
                        existe.KS60xMinibox = Math.Round(item.KS60xMinibox * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                        existe.KS100xKMC100 = Math.Round(item.KS100xKMC100 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                        existe.RealzaxExtension = Math.Round(item.RealzaxExtension * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                        existe.Corredera = Math.Round(item.Corredera * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                        existe.Paralelo = Math.Round(item.Paralelo * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);

                        if (formula.Code == "F1" && model.AffectationF1 == 2)
                        {
                            existe.Metered = budgetCD.Metered;
                            existe.KS60xMinibox = Math.Round(item.KS60xMinibox * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.KS100xKMC100 = Math.Round(item.KS100xKMC100 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.RealzaxExtension = Math.Round(item.RealzaxExtension * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Corredera = Math.Round(item.Corredera * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Paralelo = Math.Round(item.Paralelo * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                        }

                        if (formula.Code == "F2" && model.AffectationF2 == 2)
                        {
                            existe.Metered = budgetCD.Metered;
                            existe.KS60xMinibox = Math.Round(item.KS60xMinibox * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.KS100xKMC100 = Math.Round(item.KS100xKMC100 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.RealzaxExtension = Math.Round(item.RealzaxExtension * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Corredera = Math.Round(item.Corredera * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Paralelo = Math.Round(item.Paralelo * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                        }

                        if (formula.Code == "F3" && model.AffectationF3 == 2)
                        {
                            existe.Metered = budgetCD.Metered;
                            existe.KS60xMinibox = Math.Round(item.KS60xMinibox * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.KS100xKMC100 = Math.Round(item.KS100xKMC100 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.RealzaxExtension = Math.Round(item.RealzaxExtension * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Corredera = Math.Round(item.Corredera * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Paralelo = Math.Round(item.Paralelo * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                        }

                        if (formula.Code == "F4" && model.AffectationF4 == 2)
                        {
                            existe.Metered = budgetCD.Metered;
                            existe.KS60xMinibox = Math.Round(item.KS60xMinibox * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.KS100xKMC100 = Math.Round(item.KS100xKMC100 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.RealzaxExtension = Math.Round(item.RealzaxExtension * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Corredera = Math.Round(item.Corredera * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Paralelo = Math.Round(item.Paralelo * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                        }

                        if (formula.Code == "F5" && model.AffectationF5 == 2)
                        {
                            existe.Metered = budgetCD.Metered;
                            existe.KS60xMinibox = Math.Round(item.KS60xMinibox * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.KS100xKMC100 = Math.Round(item.KS100xKMC100 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.RealzaxExtension = Math.Round(item.RealzaxExtension * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Corredera = Math.Round(item.Corredera * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Paralelo = Math.Round(item.Paralelo * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                        }

                        if (formula.Code == "F6" && model.AffectationF6 == 2)
                        {
                            existe.Metered = budgetCD.Metered;
                            existe.KS60xMinibox = Math.Round(item.KS60xMinibox * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.KS100xKMC100 = Math.Round(item.KS100xKMC100 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.RealzaxExtension = Math.Round(item.RealzaxExtension * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Corredera = Math.Round(item.Corredera * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Paralelo = Math.Round(item.Paralelo * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                        }

                        if (formula.Code == "F7" && model.AffectationF7 == 2)
                        {
                            existe.Metered = budgetCD.Metered;
                            existe.KS60xMinibox = Math.Round(item.KS60xMinibox * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.KS100xKMC100 = Math.Round(item.KS100xKMC100 * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.RealzaxExtension = Math.Round(item.RealzaxExtension * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Corredera = Math.Round(item.Corredera * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                            existe.Paralelo = Math.Round(item.Paralelo * budgetCD.Metered * isDeductivo, 2, MidpointRounding.AwayFromZero);
                        }

                    }
                }
                else
                {
                    if (existe == null)
                    {
                        consolidatedEntibado.Id = Guid.NewGuid();
                        consolidatedEntibado.BudgetTitleId = item.BudgetTitleId;
                        consolidatedEntibado.ProjectFormulaId = item.ProjectFormulaId;
                        consolidatedEntibado.ProjectPhaseId = item.ProjectPhaseId;
                        consolidatedEntibado.WorkFrontId = item.WorkFrontId;
                        consolidatedEntibado.OrderNumber = count;
                        consolidatedEntibado.ItemNumber = item.ItemNumber;
                        consolidatedEntibado.Description = item.Description;
                        consolidatedEntibado.Unit = item.Unit;

                        consolidados.Add(consolidatedEntibado);
                        count++;
                    }
                }
            }
            if (consolidados.Count != 0)
                await _context.ConsolidatedEntibados.AddRangeAsync(consolidados);
            //_context.ConsolidatedEntibados.UpdateRange(consolidadosUpdate);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("metrado-ks60")]
        public async Task<IActionResult> GetKS60Suma(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var query = _context.ConsolidatedEntibados
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId && x.Unit != "");

            var ks60Suma = 0.0;

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
                ks60Suma += item.KS60xMinibox;
            }

            return Ok(ks60Suma.ToString("N2", CultureInfo.InvariantCulture));
        }

        [HttpGet("metrado-ks100")]
        public async Task<IActionResult> GetKS100Suma(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var query = _context.ConsolidatedEntibados
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId && x.Unit != "");

            var ks100Suma = 0.0;

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
                ks100Suma += item.KS100xKMC100;
            }

            return Ok(ks100Suma.ToString("N2", CultureInfo.InvariantCulture));
        }

        [HttpGet("metrado-realza-extension")]
        public async Task<IActionResult> GetRealzaExtensionSuma(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var query = _context.ConsolidatedEntibados
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId && x.Unit != "");

            var realzaExtensionSuma = 0.0;

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
                realzaExtensionSuma += item.RealzaxExtension;
            }

            return Ok(realzaExtensionSuma.ToString("N2", CultureInfo.InvariantCulture));
        }

        [HttpGet("metrado-corredera")]
        public async Task<IActionResult> GetCorrederaSuma(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var query = _context.ConsolidatedEntibados
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId && x.Unit != "");

            var correderaSuma = 0.0;

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
                correderaSuma += item.Corredera;
            }

            return Ok(correderaSuma.ToString("N2", CultureInfo.InvariantCulture));
        }

        [HttpGet("metrado-paralelo")]
        public async Task<IActionResult> GetParaleloSuma(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var query = _context.ConsolidatedEntibados
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId && x.Unit != "");

            var parareloSuma = 0.0;

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
                parareloSuma += item.Paralelo;
            }

            return Ok(parareloSuma.ToString("N2", CultureInfo.InvariantCulture));
        }

        [HttpGet("total")]
        public async Task<IActionResult> GetTotalSuma(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var query = _context.ConsolidatedEntibados
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId && x.Unit != "");

            var totalSuma = 0.0;

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
                totalSuma += item.KS60xMinibox + item.KS100xKMC100 + item.RealzaxExtension + item.Corredera + item.Paralelo;
            }

            return Ok(totalSuma.ToString("N2", CultureInfo.InvariantCulture));
        }

        [HttpDelete("eliminar-filtro")]
        public async Task<IActionResult> DeleteByFilters(Guid? projectFormulaId = null, Guid? budgetTitleId = null)
        {
            var pId = GetProjectId();

            var query = _context.ConsolidatedEntibados
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

            _context.ConsolidatedEntibados.RemoveRange(data);
            await _context.SaveChangesAsync();

            return Ok();
        }


        [HttpGet("f1")]
        public async Task<IActionResult> GetF1()
        {
            var f1 = await _context.ProjectFormulas.FirstOrDefaultAsync(x => x.ProjectId == GetProjectId() && x.Code == "F1");

            var steel = await _context.ConsolidatedEntibados.FirstOrDefaultAsync(x => x.ProjectFormulaId == f1.Id && x.Unit != "");

            var res = 1;

            if (steel != null && steel.Metered != 0)
                res = 2;

            return Ok(res);
        }

        [HttpGet("f2")]
        public async Task<IActionResult> GetF2()
        {
            var f2 = await _context.ProjectFormulas.FirstOrDefaultAsync(x => x.ProjectId == GetProjectId() && x.Code == "F2");

            var steel = await _context.ConsolidatedEntibados.FirstOrDefaultAsync(x => x.ProjectFormulaId == f2.Id && x.Unit != "");

            var res = 1;

            if (steel != null && steel.Metered != 0)
                res = 2;

            return Ok(res);
        }

        [HttpGet("f3")]
        public async Task<IActionResult> GetF3()
        {
            var f3 = await _context.ProjectFormulas.FirstOrDefaultAsync(x => x.ProjectId == GetProjectId() && x.Code == "F3");

            var steel = await _context.ConsolidatedEntibados.FirstOrDefaultAsync(x => x.ProjectFormulaId == f3.Id && x.Unit != "");

            var res = 1;

            if (steel != null && steel.Metered != 0)
                res = 2;

            return Ok(res);
        }

        [HttpGet("f4")]
        public async Task<IActionResult> GetF4()
        {
            var f4 = await _context.ProjectFormulas.FirstOrDefaultAsync(x => x.ProjectId == GetProjectId() && x.Code == "F4");

            var steel = await _context.ConsolidatedEntibados.FirstOrDefaultAsync(x => x.ProjectFormulaId == f4.Id && x.Unit != "");

            var res = 1;

            if (steel != null && steel.Metered != 0)
                res = 2;

            return Ok(res);
        }

        [HttpGet("f5")]
        public async Task<IActionResult> GetF5()
        {
            var f5 = await _context.ProjectFormulas.FirstOrDefaultAsync(x => x.ProjectId == GetProjectId() && x.Code == "F5");

            var steel = await _context.ConsolidatedEntibados.FirstOrDefaultAsync(x => x.ProjectFormulaId == f5.Id && x.Unit != "");

            var res = 1;

            if (steel != null && steel.Metered != 0)
                res = 2;

            return Ok(res);
        }

        [HttpGet("f6")]
        public async Task<IActionResult> GetF6()
        {
            var f6 = await _context.ProjectFormulas.FirstOrDefaultAsync(x => x.ProjectId == GetProjectId() && x.Code == "F6");

            var steel = await _context.ConsolidatedEntibados.FirstOrDefaultAsync(x => x.ProjectFormulaId == f6.Id && x.Unit != "");

            var res = 1;

            if (steel != null && steel.Metered != 0)
                res = 2;

            return Ok(res);
        }

        [HttpGet("f7")]
        public async Task<IActionResult> GetF7()
        {
            var f7 = await _context.ProjectFormulas.FirstOrDefaultAsync(x => x.ProjectId == GetProjectId() && x.Code == "F7");

            var steel = await _context.ConsolidatedEntibados.FirstOrDefaultAsync(x => x.ProjectFormulaId == f7.Id && x.Unit != "");

            var res = 1;

            if (steel != null && steel.Metered != 0)
                res = 2;

            return Ok(res);
        }
    }
}
