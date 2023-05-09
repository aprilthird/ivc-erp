using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Logistics;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.ConsolidatedBudgetInputViewModels;
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
    [Route("oficina-tecnica/consolidado-insumos")]
    public class ConsolidatedBudgetInputController : BaseController
    {
        public ConsolidatedBudgetInputController(IvcDbContext context,
            ILogger<ConsolidatedBudgetInputController> logger) : base(context, logger)
        {

        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? supplyGroupId = null, Guid? supplyFamilyId = null)
        {
            var query = _context.ConsolidatedBudgetInputs.Where(x => x.Description != null);

            if (supplyFamilyId.HasValue)
                query = query.Where(x => x.SupplyFamilyId == supplyFamilyId.Value || x.SupplyFamilyId == null);
            if (supplyGroupId.HasValue)
                query = query.Where(x => x.SupplyGroupId == supplyGroupId.Value || x.SupplyGroupId == null);

            var selected = await query
                .Select(x => new ConsolidatedBudgetInputViewModel
                {
                    NumberItem = x.NumberItem,
                    Description = x.Description,
                    ContractualAmount = x.ContractualAmount.ToString("N", CultureInfo.InvariantCulture),
                    DeductiveAmount1 = x.DeductiveAmount1.ToString("N", CultureInfo.InvariantCulture),
                    DeductiveAmount2 = x.DeductiveAmount2.ToString("N", CultureInfo.InvariantCulture),
                    DeductiveAmount3 = x.DeductiveAmount3.ToString("N", CultureInfo.InvariantCulture),
                    Deductives = x.Deductives.ToString("N", CultureInfo.InvariantCulture),
                    NetContractual = x.NetContractual.ToString("N", CultureInfo.InvariantCulture),
                    AdicionalAmount1 = x.AdicionalAmount1.ToString("N", CultureInfo.InvariantCulture),
                    AdicionalAmount2 = x.AdicionalAmount2.ToString("N", CultureInfo.InvariantCulture),
                    AdicionalAmount3 = x.AdicionalAmount3.ToString("N", CultureInfo.InvariantCulture),
                    Adicionals = x.Adicionals.ToString("N", CultureInfo.InvariantCulture),
                    AccumulatedAmount = x.AccumulatedAmount.ToString("N", CultureInfo.InvariantCulture)
                }).ToListAsync();

            return Ok(selected);
        }

        [HttpPost("generar")]
        public async Task<IActionResult> Generate()
        {
            var consolidateds = await _context.ConsolidatedBudgetInputs.ToListAsync();

            var budgetInputs = await _context.BudgetInputs.AsNoTracking().ToListAsync();

            if (consolidateds.Count() != 0)
            {
                _context.ConsolidatedBudgetInputs.RemoveRange(consolidateds);
                await _context.SaveChangesAsync();
            }

            var supplyFamilies = await _context.SupplyFamilies.AsNoTracking().ToListAsync();

            var supplyGroups = await _context.SupplyGroups.AsNoTracking().ToListAsync();

            var listFamily = new List<SupplyFamily>();
            var listGroup = new List<SupplyGroup>();

            var count = 0;

            foreach (var family in supplyFamilies)
            {
                foreach(var item in budgetInputs)
                {
                    if(family.Id == item.SupplyFamilyId && count == 0)
                    {
                        listFamily.Add(family);
                        count++;
                    }
                }
                count = 0;
            }

            foreach(var group in supplyGroups)
            {
                foreach(var item in budgetInputs)
                {
                    if(group.Id == item.SupplyGroupId && count == 0)
                    {
                        listGroup.Add(group);
                        count++;
                    }
                }
                count = 0;
            }

            var total = new ConsolidatedBudgetInput();

            //---------------------COMPONENTES PRINCIPALES

            var budgetsMain = await _context.BudgetInputs
                .Include(x => x.ProjectFormula)
                .Include(x => x.BudgetType)
                .Where(x => x.BudgetType.Name == "Contractual" && x.Group == 1)
                .OrderBy(x => x.SupplyFamilyId)
                .AsNoTracking()
                .ToListAsync();

            var data = new List<ConsolidatedBudgetInput>();

            var header = new ConsolidatedBudgetInput();
            header.NumberItem = "I.";
            header.Description = "INSUMOS VENTA";
            data.Add(header);

            var sumaContractual = 0.0;
            var sumaDeductiveAmount1 = 0.0;
            var sumaDeductiveAmount2 = 0.0;
            var sumaDeductiveAmount3 = 0.0;
            var sumaDeductives = 0.0;
            var sumaNetContractual = 0.0;
            var sumaAdicionalAmount1 = 0.0;
            var sumaAdicionalAmount2 = 0.0;
            var sumaAdicionalAmount3 = 0.0;
            var sumaAdicionals = 0.0;
            var sumaAccumulatedAmount = 0.0;

            foreach (var family in listFamily)
            {
                header = new ConsolidatedBudgetInput();
                header.NumberItem = family.Code;
                header.Description = family.Name;
                header.SupplyFamilyId = family.Id;

                data.Add(header);

                foreach (var group in listGroup)
                {
                    var contractualFormula = 0.0;
                    var deductivesFormula = 0.0;
                    var deductiveAmount1Formula = 0.0;
                    var deductiveAmount2Formula = 0.0;
                    var deductiveAmount3Formula = 0.0;
                    var adicionalsFormula = 0.00;
                    var adicionalAmount1Formula = 0.0;
                    var adicionalAmount2Formula = 0.0;
                    var adicionalAmount3Formula = 0.0;

                    var netContractualFormula = 0.0;

                    var accumulatedAmountFormula = 0.0;

                    var aux = new ConsolidatedBudgetInput();

                    //-------La suma de los deductivos
                    var contractualBudgets = await _context.BudgetInputs
                        .Where(x => x.BudgetType.Name == "Contractual" && x.Group == 1
                        && x.SupplyFamilyId == family.Id && x.SupplyGroupId == group.Id)
                        .AsNoTracking()
                        .ToListAsync();

                    foreach (var item in contractualBudgets)
                        contractualFormula += item.Parcial;

                    //-------La suma de los deductivos
                    var deductiveBudgets = await _context.BudgetInputs
                        .Where(x => x.BudgetType.Name == "Deductivo" && x.Group == 1
                        && x.SupplyFamilyId == family.Id && x.SupplyGroupId == group.Id)
                        .AsNoTracking()
                        .ToListAsync();

                    foreach (var item in deductiveBudgets)
                        deductivesFormula += item.Parcial;

                    //-------Los deductivos RRP-19
                    var deductiveAmount1Entity = await _context.BudgetInputs
                        .Include(x => x.BudgetTitle)
                        .Where(x => x.BudgetTitle.Name == "Deductivo N° 01 (RRP-19)" && x.Group == 1
                        && x.SupplyFamilyId == family.Id && x.SupplyGroupId == group.Id)
                        .AsNoTracking()
                        .ToListAsync();

                    foreach (var item in deductiveAmount1Entity)
                        deductiveAmount1Formula += item.Parcial;

                    //-------Los deductivos Colector de Descarga
                    var deductiveAmount2Entity = await _context.BudgetInputs
                        .Include(x => x.BudgetTitle)
                        .Where(x => x.BudgetTitle.Name == "Deductivo N° 02 (Colector de Descarga)" && x.Group == 1
                        && x.SupplyFamilyId == family.Id && x.SupplyGroupId == group.Id)
                        .AsNoTracking()
                        .ToListAsync();

                    foreach (var item in deductiveAmount2Entity)
                        deductiveAmount2Formula += item.Parcial;

                    //--------Los deductivos Anclajes Líneas HD
                    var deductiveAmount3Entity = await _context.BudgetInputs
                        .Include(x => x.BudgetTitle)
                        .Where(x => x.BudgetTitle.Name == "Deductivo N° 03 (Anclajes Líneas HD)" && x.Group == 1
                        && x.SupplyFamilyId == family.Id && x.SupplyGroupId == group.Id)
                        .AsNoTracking()
                        .ToListAsync();

                    foreach (var item in deductiveAmount3Entity)
                        deductiveAmount3Formula += item.Parcial;

                    //-------La suma de los adicionales
                    var adicionalBudgets = await _context.BudgetInputs
                        .Where(x => x.BudgetType.Name == "Adicional" && x.Group == 1
                        && x.SupplyFamilyId == family.Id && x.SupplyGroupId == group.Id)
                        .AsNoTracking()
                        .ToListAsync();

                    foreach (var item in adicionalBudgets)
                        adicionalsFormula += item.Parcial;

                    //--------Los adicionales RRP-19
                    var adicionalAmount1Entity = await _context.BudgetInputs
                        .Include(x => x.BudgetTitle)
                        .Include(x => x.BudgetType)
                        .Where(x => x.BudgetTitle.Name == "Adicional N°01 (RRP-19)" && x.BudgetType.Name == "Adicional" && x.Group == 1
                        && x.SupplyFamilyId == family.Id && x.SupplyGroupId == group.Id)
                        .AsNoTracking()
                        .ToListAsync();

                    foreach (var item in adicionalAmount1Entity)
                        adicionalAmount1Formula += item.Parcial;

                    //-------Los adicionales Colector de Descarga
                    var adicionalAmount2Entity = await _context.BudgetInputs
                        .Include(x => x.BudgetTitle)
                        .Where(x => x.BudgetTitle.Name == "Adicional N° 02 (Colector de Descarga)" && x.Group == 1
                        && x.SupplyFamilyId == family.Id && x.SupplyGroupId == group.Id)
                        .AsNoTracking()
                        .ToListAsync();

                    foreach (var item in adicionalAmount2Entity)
                        adicionalAmount2Formula += item.Parcial;

                    //--------Los adicionales Anclajes Líneas HD
                    var adicionalAmount3Entity = await _context.BudgetInputs
                        .Include(x => x.BudgetTitle)
                        .Where(x => x.BudgetTitle.Name == "Adicional N° 03 (Anclajes Líneas HD)" && x.Group == 1
                        && x.SupplyFamilyId == family.Id && x.SupplyGroupId == group.Id)
                        .AsNoTracking()
                        .ToListAsync();

                    foreach (var item in adicionalAmount3Entity)
                        adicionalAmount3Formula += item.Parcial;


                    netContractualFormula = contractualFormula + deductivesFormula;

                    accumulatedAmountFormula = contractualFormula + deductivesFormula + adicionalsFormula;

                    aux.NumberItem = group.Code;
                    aux.Description = group.Name;
                    aux.ContractualAmount = contractualFormula;
                    aux.DeductiveAmount1 = deductiveAmount1Formula;
                    aux.DeductiveAmount2 = deductiveAmount2Formula;
                    aux.DeductiveAmount3 = deductiveAmount3Formula;
                    aux.Deductives = deductivesFormula;
                    aux.NetContractual = netContractualFormula;
                    aux.AdicionalAmount1 = adicionalAmount1Formula;
                    aux.AdicionalAmount2 = adicionalAmount2Formula;
                    aux.AdicionalAmount3 = adicionalAmount3Formula;
                    aux.Adicionals = adicionalsFormula;
                    aux.AccumulatedAmount = accumulatedAmountFormula;
                    aux.SupplyFamilyId = family.Id;
                    aux.SupplyGroupId = group.Id;

                    sumaContractual += contractualFormula;
                    sumaDeductiveAmount1 += deductiveAmount1Formula;
                    sumaDeductiveAmount2 += deductiveAmount2Formula;
                    sumaDeductiveAmount3 += deductiveAmount3Formula;
                    sumaDeductives += deductivesFormula;
                    sumaNetContractual += netContractualFormula;
                    sumaAdicionalAmount1 += adicionalAmount1Formula;
                    sumaAdicionalAmount2 += adicionalAmount2Formula;
                    sumaAdicionalAmount3 += adicionalAmount3Formula;
                    sumaAdicionals += adicionalsFormula;
                    sumaAccumulatedAmount += accumulatedAmountFormula;

                    if(aux.ContractualAmount != 0.0)
                        data.Add(aux);
                }

            }

            await _context.ConsolidatedBudgetInputs.AddRangeAsync(data);
            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
