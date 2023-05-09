using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.ConsolidatedBudgetViewModels;
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
    [Route("oficina-tecnica/consolidado-presupuestos")]
    public class ConsolidatedBudgetController : BaseController
    {
        public ConsolidatedBudgetController(IvcDbContext context,
            ILogger<ConsolidatedBudgetController> logger) : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var selected = await _context.ConsolidatedBudgets
                .Select(x => new ConsolidatedBudgetViewModel
            {
                NumberItem = x.NumberItem,
                Description = x.Description,
                ContractualAmount = x.ContractualAmount.ToString("N", CultureInfo.InvariantCulture),
                DeductiveAmount1 = x.DeductiveAmount1.ToString("N", CultureInfo.InvariantCulture),
                DeductiveAmount2 = x.DeductiveAmount2.ToString("N", CultureInfo.InvariantCulture),
                DeductiveAmount3 = x.DeductiveAmount3.ToString("N", CultureInfo.InvariantCulture),
                DeductiveAmount4 = x.DeductiveAmount4.ToString("N", CultureInfo.InvariantCulture),
                Deductives = x.Deductives.ToString("N", CultureInfo.InvariantCulture),
                NetContractual = x.NetContractual.ToString("N", CultureInfo.InvariantCulture),
                AdicionalAmount1 = x.AdicionalAmount1.ToString("N", CultureInfo.InvariantCulture),
                AdicionalAmount2 = x.AdicionalAmount2.ToString("N", CultureInfo.InvariantCulture),
                AdicionalAmount3 = x.AdicionalAmount3.ToString("N", CultureInfo.InvariantCulture),
                AdicionalAmount4 = x.AdicionalAmount4.ToString("N", CultureInfo.InvariantCulture),
                Adicionals = x.Adicionals.ToString("N", CultureInfo.InvariantCulture),
                AccumulatedAmount = x.AccumulatedAmount.ToString("N", CultureInfo.InvariantCulture)
            }).ToListAsync();

            return Ok(selected);
        }

        [HttpPost("generar")]
        public async Task<IActionResult> Generate()
        {
            var consolidateds = await _context.ConsolidatedBudgets.ToListAsync();

            if (consolidateds.Count() != 0)
            {
                _context.ConsolidatedBudgets.RemoveRange(consolidateds);
                await _context.SaveChangesAsync();
            }

            var total = new ConsolidatedBudget();

            //---------------------COMPONENTES PRINCIPALES

            var budgetsMain = await _context.Budgets
                .Include(x => x.ProjectFormula)
                .Include(x => x.BudgetType)
                .Where(x => x.BudgetType.Name == "Contractual" && x.NumberItem.Length < 3 && x.Group == 1)
                .OrderBy(x => x.ProjectFormula.Code)
                .AsNoTracking()
                .ToListAsync();

            var data = new List<ConsolidatedBudget>();

            var f16 = new ConsolidatedBudget();

            var header = new ConsolidatedBudget();
            header.NumberItem = "I.";
            header.Description = "COMPONENTE PRINCIPAL";
            data.Add(header);

            var sumaContractual = 0.0;
            var sumaDeductiveAmount1 = 0.0;
            var sumaDeductiveAmount2 = 0.0;
            var sumaDeductiveAmount3 = 0.0;
            var sumaDeductiveAmount4 = 0.0;
            var sumaDeductives = 0.0;
            var sumaNetContractual = 0.0;
            var sumaAdicionalAmount1 = 0.0;
            var sumaAdicionalAmount2 = 0.0;
            var sumaAdicionalAmount3 = 0.0;
            var sumaAdicionalAmount4 = 0.0;
            var sumaAdicionals = 0.0;
            var sumaAccumulatedAmount = 0.0;

            foreach (var budget in budgetsMain)
            {

                var aux = new ConsolidatedBudget();

                //-------La suma de los deductivos
                var deductiveBudgets = await _context.Budgets
                    .Where(x => x.ProjectFormulaId == budget.ProjectFormulaId
                    && x.BudgetType.Name == "Deductivo" && x.NumberItem.Length < 3 && x.Group == 1)
                    .AsNoTracking()
                    .ToListAsync();

                var deductivesFormula = 0.00;

                foreach (var deductives in deductiveBudgets)
                    deductivesFormula = deductivesFormula + deductives.TotalPrice;

                //-------Los deductivos RRP-19
                var deductiveAmount1Entity = await _context.Budgets
                    .Include(x => x.BudgetTitle)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.ProjectFormulaId == budget.ProjectFormulaId && x.BudgetTitle.Name == "Deductivo N° 01 (RRP-19)"
                    && x.NumberItem.Length < 3 && x.Group == 1);

                var deductiveAmount1Formula = 0.0;

                if (deductiveAmount1Entity != null)
                    deductiveAmount1Formula = deductiveAmount1Entity.TotalPrice;

                //-------Los deductivos Colector de Descarga
                var deductiveAmount2Entity = await _context.Budgets
                    .Include(x => x.BudgetTitle)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.ProjectFormulaId == budget.ProjectFormulaId && x.BudgetTitle.Name == "Deductivo N° 02 (Colector de Descarga)"
                    && x.NumberItem.Length < 3 && x.Group == 1);

                var deductiveAmount2Formula = 0.0;

                if (deductiveAmount2Entity != null)
                    deductiveAmount2Formula = deductiveAmount2Entity.TotalPrice;

                //--------Los deductivos Anclajes Líneas HD
                var deductiveAmount3Entity = await _context.Budgets
                    .Include(x => x.BudgetTitle)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.ProjectFormulaId == budget.ProjectFormulaId && x.BudgetTitle.Name == "Deductivo N° 03 (Anclajes Líneas HD)"
                    && x.NumberItem.Length < 3 && x.Group == 1);

                var deductiveAmount3Formula = 0.0;

                if (deductiveAmount3Entity != null)
                    deductiveAmount3Formula = deductiveAmount3Entity.TotalPrice;

                //--------Los deductivos Dimensionamiento de Cámaras
                var deductiveAmount4Entity = await _context.Budgets
                    .Include(x => x.BudgetTitle)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.ProjectFormulaId == budget.ProjectFormulaId && x.BudgetTitle.Name == "Deductivo N° 04 (Dimensionamiento de Cámaras)"
                    && x.NumberItem.Length < 3 && x.Group == 1);

                var deductiveAmount4Formula = 0.0;

                if (deductiveAmount4Entity != null)
                    deductiveAmount4Formula = deductiveAmount4Entity.TotalPrice;

                //-------La suma de los adicionales
                var adicionalBudgets = await _context.Budgets
                    .Where(x => x.ProjectFormulaId == budget.ProjectFormulaId && x.BudgetType.Name == "Adicional"
                    && x.NumberItem.Length < 3 && x.Group == 1)
                    .AsNoTracking()
                    .ToListAsync();

                var adicionalsFormula = 0.0;

                foreach (var adicionals in adicionalBudgets)
                    adicionalsFormula = adicionalsFormula + adicionals.TotalPrice;

                //--------Los adicionales RRP-19
                var adicionalAmount1Entity = await _context.Budgets
                    .Include(x => x.BudgetTitle)
                    .Include(x => x.BudgetType)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.ProjectFormulaId == budget.ProjectFormulaId && x.BudgetTitle.Name == "Adicional N°01 (RRP-19)"
                    && x.NumberItem.Length < 3 && x.BudgetType.Name == "Adicional" && x.Group == 1);

                var adicionalAmount1Formula = 0.0;

                if (adicionalAmount1Entity != null)
                    adicionalAmount1Formula = adicionalAmount1Entity.TotalPrice;

                //-------Los adicionales Colector de Descarga
                var adicionalAmount2Entity = await _context.Budgets
                    .Include(x => x.BudgetTitle)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.ProjectFormulaId == budget.ProjectFormulaId && x.BudgetTitle.Name == "Adicional N° 02 (Colector de Descarga)"
                    && x.NumberItem.Length < 3 && x.Group == 1);

                var adicionalAmount2Formula = 0.0;

                if (adicionalAmount2Entity != null)
                    adicionalAmount2Formula = adicionalAmount2Entity.TotalPrice;

                //--------Los adicionales Anclajes Líneas HD
                var adicionalAmount3Entity = await _context.Budgets
                    .Include(x => x.BudgetTitle)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.ProjectFormulaId == budget.ProjectFormulaId && x.BudgetTitle.Name == "Adicional N° 03 (Anclajes Líneas HD)"
                    && x.NumberItem.Length < 3 && x.Group == 1);

                var adicionalAmount3Formula = 0.0;

                if (adicionalAmount3Entity != null)
                    adicionalAmount3Formula = adicionalAmount3Entity.TotalPrice;

                //--------Los adicionales Dimensionamiento de Cámaras
                var adicionalAmount4Entity = await _context.Budgets
                    .Include(x => x.BudgetTitle)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.ProjectFormulaId == budget.ProjectFormulaId && x.BudgetTitle.Name == "Adicional N° 04 (Dimensionamiento de Cámaras)"
                    && x.NumberItem.Length < 3 && x.Group == 1);

                var adicionalAmount4Formula = 0.0;

                if (adicionalAmount4Entity != null)
                    adicionalAmount4Formula = adicionalAmount4Entity.TotalPrice;


                var netContractualFormula = budget.TotalPrice + deductivesFormula;

                var accumulatedAmountFormula = budget.TotalPrice + deductivesFormula + adicionalsFormula;

                aux.NumberItem = budget.ProjectFormula.Code;
                aux.Description = budget.Description;
                aux.ContractualAmount = budget.TotalPrice;
                aux.DeductiveAmount1 = deductiveAmount1Formula;
                aux.DeductiveAmount2 = deductiveAmount2Formula;
                aux.DeductiveAmount3 = deductiveAmount3Formula;
                aux.DeductiveAmount4 = deductiveAmount4Formula;
                aux.Deductives = deductivesFormula;
                aux.NetContractual = netContractualFormula;
                aux.AdicionalAmount1 = adicionalAmount1Formula;
                aux.AdicionalAmount2 = adicionalAmount2Formula;
                aux.AdicionalAmount3 = adicionalAmount3Formula;
                aux.AdicionalAmount4 = adicionalAmount4Formula;
                aux.Adicionals = adicionalsFormula;
                aux.AccumulatedAmount = accumulatedAmountFormula;

                sumaContractual += budget.TotalPrice;
                sumaDeductiveAmount1 += deductiveAmount1Formula;
                sumaDeductiveAmount2 += deductiveAmount2Formula;
                sumaDeductiveAmount3 += deductiveAmount3Formula;
                sumaDeductiveAmount4 += deductiveAmount4Formula;
                sumaDeductives += deductivesFormula;
                sumaNetContractual += netContractualFormula;
                sumaAdicionalAmount1 += adicionalAmount1Formula;
                sumaAdicionalAmount2 += adicionalAmount2Formula;
                sumaAdicionalAmount3 += adicionalAmount3Formula;
                sumaAdicionalAmount4 += adicionalAmount4Formula;
                sumaAdicionals += adicionalsFormula;
                sumaAccumulatedAmount += accumulatedAmountFormula;

                data.Add(aux);

            }

            var expensesUtility = _context.ExpensesUtilities.Include(x=>x.BudgetTitle).Where(x => x.Id != null);
            
            var variableExpenses = new ConsolidatedBudget();
            var fixedExpenses = new ConsolidatedBudget();

            variableExpenses.Description = "GASTO GENERAL VARIABLE";
            fixedExpenses.Description = "GASTO GENERAL FIJO";

            var expensesUtilityContractual = await _context.ExpensesUtilities.Include(x => x.BudgetTitle).Where(x => x.BudgetTitle.Name == "Contractual Original").FirstOrDefaultAsync();

            if(expensesUtilityContractual != null)
            {
                variableExpenses.ContractualAmount = expensesUtilityContractual.VariableGeneralExpense;
                fixedExpenses.ContractualAmount = expensesUtilityContractual.FixedGeneralExpense;
            }

            var expensesUtilityDeductive1 = await _context.ExpensesUtilities.Include(x => x.BudgetTitle).Where(x => x.BudgetTitle.Name == "Deductivo N° 01 (RRP-19)").FirstOrDefaultAsync();

            if(expensesUtilityDeductive1 != null)
            {
                variableExpenses.DeductiveAmount1 = expensesUtilityDeductive1.VariableGeneralExpense;
                fixedExpenses.DeductiveAmount1 = expensesUtilityDeductive1.FixedGeneralExpense;
            }

            var expensesUtilityDeductive2 = await _context.ExpensesUtilities.Include(x => x.BudgetTitle).Where(x => x.BudgetTitle.Name == "Deductivo N° 02 (Colector de Descarga)").FirstOrDefaultAsync();

            if (expensesUtilityDeductive2 != null)
            {
                variableExpenses.DeductiveAmount2 = expensesUtilityDeductive2.VariableGeneralExpense;
                fixedExpenses.DeductiveAmount2 = expensesUtilityDeductive2.FixedGeneralExpense;
            }

            var expensesUtilityDeductive3 = await _context.ExpensesUtilities.Include(x => x.BudgetTitle).Where(x => x.BudgetTitle.Name == "Deductivo N° 03 (Anclajes Líneas HD)").FirstOrDefaultAsync();

            if (expensesUtilityDeductive3 != null)
            {
                variableExpenses.DeductiveAmount3 = expensesUtilityDeductive3.VariableGeneralExpense;
                fixedExpenses.DeductiveAmount3 = expensesUtilityDeductive3.FixedGeneralExpense;
            }

            var expensesUtilityDeductive4 = await _context.ExpensesUtilities.Include(x => x.BudgetTitle).Where(x => x.BudgetTitle.Name == "Deductivo N° 04 (Dimensionamiento de Cámaras)").FirstOrDefaultAsync();

            if (expensesUtilityDeductive4 != null)
            {
                variableExpenses.DeductiveAmount4 = expensesUtilityDeductive4.VariableGeneralExpense;
                fixedExpenses.DeductiveAmount4 = expensesUtilityDeductive4.FixedGeneralExpense;
            }

            variableExpenses.Deductives = variableExpenses.DeductiveAmount1 + variableExpenses.DeductiveAmount2 + variableExpenses.DeductiveAmount3 + variableExpenses.DeductiveAmount4;
            variableExpenses.NetContractual = variableExpenses.ContractualAmount + variableExpenses.Deductives;
            fixedExpenses.Deductives = fixedExpenses.DeductiveAmount1 + fixedExpenses.DeductiveAmount2 + fixedExpenses.DeductiveAmount3 + fixedExpenses.DeductiveAmount4;
            fixedExpenses.NetContractual = fixedExpenses.ContractualAmount + fixedExpenses.Deductives;

            var expensesUtilityAdicional1 = await _context.ExpensesUtilities.Include(x => x.BudgetTitle).Where(x => x.BudgetTitle.Name == "Adicional N°01 (RRP-19)").FirstOrDefaultAsync();

            if (expensesUtilityAdicional1 != null)
            {
                variableExpenses.AdicionalAmount1 = expensesUtilityAdicional1.VariableGeneralExpense;
                fixedExpenses.AdicionalAmount1 = expensesUtilityAdicional1.FixedGeneralExpense;
            }

            var expensesUtilityAdicional2 = await _context.ExpensesUtilities.Include(x => x.BudgetTitle).Where(x => x.BudgetTitle.Name == "Adicional N° 02 (Colector de Descarga)").FirstOrDefaultAsync();

            if (expensesUtilityAdicional2 != null)
            {
                variableExpenses.AdicionalAmount2 = expensesUtilityAdicional2.VariableGeneralExpense;
                fixedExpenses.AdicionalAmount2 = expensesUtilityAdicional2.FixedGeneralExpense;
            }

            var expensesUtilityAdicional3 = await _context.ExpensesUtilities.Include(x => x.BudgetTitle).Where(x => x.BudgetTitle.Name == "Adicional N° 03 (Anclajes Líneas HD)").FirstOrDefaultAsync();

            if (expensesUtilityAdicional3 != null)
            {
                variableExpenses.AdicionalAmount3 = expensesUtilityAdicional3.VariableGeneralExpense;
                fixedExpenses.AdicionalAmount3 = expensesUtilityAdicional3.FixedGeneralExpense;
            }

            var expensesUtilityAdicional4 = await _context.ExpensesUtilities.Include(x => x.BudgetTitle).Where(x => x.BudgetTitle.Name == "Adicional N° 04 (Dimensionamiento de Cámaras)").FirstOrDefaultAsync();

            if (expensesUtilityAdicional4 != null)
            {
                variableExpenses.AdicionalAmount4 = expensesUtilityAdicional4.VariableGeneralExpense;
                fixedExpenses.AdicionalAmount4 = expensesUtilityAdicional4.FixedGeneralExpense;
            }

            variableExpenses.Adicionals = variableExpenses.AdicionalAmount1 + variableExpenses.AdicionalAmount2 + variableExpenses.AdicionalAmount3 + variableExpenses.AdicionalAmount4;
            variableExpenses.AccumulatedAmount = variableExpenses.NetContractual + variableExpenses.Adicionals;

            fixedExpenses.Adicionals = fixedExpenses.AdicionalAmount1 + fixedExpenses.AdicionalAmount2 + fixedExpenses.AdicionalAmount3 + fixedExpenses.AdicionalAmount4;
            fixedExpenses.AccumulatedAmount = fixedExpenses.NetContractual + fixedExpenses.Adicionals;


            var directCost = new ConsolidatedBudget();
            directCost.Description = "COSTO DIRECTO";
            directCost.ContractualAmount = sumaContractual;
            directCost.DeductiveAmount1 = sumaDeductiveAmount1;
            directCost.DeductiveAmount2 = sumaDeductiveAmount2;
            directCost.DeductiveAmount3 = sumaDeductiveAmount3;
            directCost.DeductiveAmount4 = sumaDeductiveAmount4;
            directCost.Deductives = sumaDeductives;
            directCost.NetContractual = sumaNetContractual;
            directCost.AdicionalAmount1 = sumaAdicionalAmount1;
            directCost.AdicionalAmount2 = sumaAdicionalAmount2;
            directCost.AdicionalAmount3 = sumaAdicionalAmount3;
            directCost.AdicionalAmount4 = sumaAdicionalAmount4;
            directCost.Adicionals = sumaAdicionals;
            directCost.AccumulatedAmount = sumaAccumulatedAmount;

            var utility = new ConsolidatedBudget();
            utility.Description = "UTILIDAD";
            utility.ContractualAmount = Math.Round(sumaContractual * (expensesUtilityContractual.UtilityPercentage/100), 2);
            utility.DeductiveAmount1 = Math.Round(sumaDeductiveAmount1 * (expensesUtilityDeductive1.UtilityPercentage / 100), 2);
            utility.DeductiveAmount2 = Math.Round(sumaDeductiveAmount2 * (expensesUtilityDeductive2.UtilityPercentage / 100), 2);
            utility.DeductiveAmount3 = Math.Round(sumaDeductiveAmount3 * (expensesUtilityDeductive3.UtilityPercentage / 100), 2);
            utility.DeductiveAmount4 = Math.Round(sumaDeductiveAmount4 * (expensesUtilityDeductive4.UtilityPercentage / 100), 2);
            utility.Deductives = Math.Round(sumaDeductives * (expensesUtilityContractual.UtilityPercentage / 100), 2);
            utility.NetContractual = Math.Round(sumaNetContractual * (expensesUtilityContractual.UtilityPercentage / 100), 2);
            utility.AdicionalAmount1 = Math.Round(sumaAdicionalAmount1 * (expensesUtilityAdicional1.UtilityPercentage / 100), 2);
            utility.AdicionalAmount2 = Math.Round(sumaAdicionalAmount2 * (expensesUtilityAdicional2.UtilityPercentage / 100), 2);
            utility.AdicionalAmount3 = Math.Round(sumaAdicionalAmount3 * (expensesUtilityAdicional3.UtilityPercentage / 100), 2);
            utility.AdicionalAmount4 = Math.Round(sumaAdicionalAmount4 * (expensesUtilityAdicional4.UtilityPercentage / 100), 2);
            utility.Adicionals = Math.Round(sumaAdicionals * (expensesUtilityContractual.UtilityPercentage / 100), 2);
            utility.AccumulatedAmount = Math.Round(sumaAccumulatedAmount * (expensesUtilityContractual.UtilityPercentage / 100), 2);

            var subTotal = new ConsolidatedBudget();
            subTotal.Description = "SUB-TOTAL";
            subTotal.ContractualAmount = Math.Round(directCost.ContractualAmount + utility.ContractualAmount + fixedExpenses.ContractualAmount + variableExpenses.ContractualAmount , 2);
            subTotal.DeductiveAmount1 = Math.Round(directCost.DeductiveAmount1 + utility.DeductiveAmount1 + fixedExpenses.DeductiveAmount1 + variableExpenses.DeductiveAmount1, 2);
            subTotal.DeductiveAmount2 = Math.Round(directCost.DeductiveAmount2 + utility.DeductiveAmount2 + fixedExpenses.DeductiveAmount2 + variableExpenses.DeductiveAmount2, 2);
            subTotal.DeductiveAmount3 = Math.Round(directCost.DeductiveAmount3 + utility.DeductiveAmount3 + fixedExpenses.DeductiveAmount3 + variableExpenses.DeductiveAmount3, 2);
            subTotal.DeductiveAmount4 = Math.Round(directCost.DeductiveAmount4 + utility.DeductiveAmount4 + fixedExpenses.DeductiveAmount4 + variableExpenses.DeductiveAmount4, 2);
            subTotal.Deductives = Math.Round(directCost.Deductives + utility.Deductives + fixedExpenses.Deductives + variableExpenses.Deductives, 2);
            subTotal.NetContractual = Math.Round(directCost.NetContractual + utility.NetContractual + fixedExpenses.NetContractual + variableExpenses.NetContractual, 2);
            subTotal.AdicionalAmount1 = Math.Round(directCost.AdicionalAmount1 + utility.AdicionalAmount1 + fixedExpenses.AdicionalAmount1 + variableExpenses.AdicionalAmount1, 2);
            subTotal.AdicionalAmount2 = Math.Round(directCost.AdicionalAmount2 + utility.AdicionalAmount2 + fixedExpenses.AdicionalAmount2 + variableExpenses.AdicionalAmount2, 2);
            subTotal.AdicionalAmount3 = Math.Round(directCost.AdicionalAmount3 + utility.AdicionalAmount3 + fixedExpenses.AdicionalAmount3 + variableExpenses.AdicionalAmount3, 2);
            subTotal.AdicionalAmount4 = Math.Round(directCost.AdicionalAmount4 + utility.AdicionalAmount4 + fixedExpenses.AdicionalAmount4 + variableExpenses.AdicionalAmount4, 2);
            subTotal.Adicionals = Math.Round(directCost.Adicionals + utility.Adicionals + fixedExpenses.Adicionals + variableExpenses.Adicionals, 2);
            subTotal.AccumulatedAmount = Math.Round(directCost.AccumulatedAmount + utility.AccumulatedAmount + fixedExpenses.AccumulatedAmount + variableExpenses.AccumulatedAmount, 2);

            var igv = new ConsolidatedBudget();
            igv.Description = "IGV";
            igv.ContractualAmount = Math.Round(subTotal.ContractualAmount * 0.18, 2);
            igv.DeductiveAmount1 = Math.Round(subTotal.DeductiveAmount1 * 0.18, 2);
            igv.DeductiveAmount2 = Math.Round(subTotal.DeductiveAmount2 * 0.18, 2);
            igv.DeductiveAmount3 = Math.Round(subTotal.DeductiveAmount3 * 0.18, 2);
            igv.DeductiveAmount4 = Math.Round(subTotal.DeductiveAmount4 * 0.18, 2);
            igv.Deductives = Math.Round(subTotal.Deductives * 0.18, 2);
            igv.NetContractual = Math.Round(subTotal.NetContractual * 0.18, 2);
            igv.AdicionalAmount1 = Math.Round(subTotal.AdicionalAmount1 * 0.18, 2);
            igv.AdicionalAmount2 = Math.Round(subTotal.AdicionalAmount2 * 0.18, 2);
            igv.AdicionalAmount3 = Math.Round(subTotal.AdicionalAmount3 * 0.18, 2);
            igv.AdicionalAmount4 = Math.Round(subTotal.AdicionalAmount4 * 0.18, 2);
            igv.Adicionals = Math.Round(subTotal.Adicionals * 0.18, 2);
            igv.AccumulatedAmount = Math.Round(subTotal.AccumulatedAmount * 0.18, 2);

            var parcial = new ConsolidatedBudget();
            parcial.Description = "PARCIAL";
            parcial.ContractualAmount = Math.Round(subTotal.ContractualAmount + igv.ContractualAmount, 2);
            parcial.DeductiveAmount1 = Math.Round(subTotal.DeductiveAmount1 + igv.DeductiveAmount1, 2);
            parcial.DeductiveAmount2 = Math.Round(subTotal.DeductiveAmount2 + igv.DeductiveAmount2, 2);
            parcial.DeductiveAmount3 = Math.Round(subTotal.DeductiveAmount3 + igv.DeductiveAmount3, 2);
            parcial.DeductiveAmount4 = Math.Round(subTotal.DeductiveAmount4 + igv.DeductiveAmount4, 2);
            parcial.Deductives = Math.Round(subTotal.Deductives + igv.Deductives, 2);
            parcial.NetContractual = Math.Round(subTotal.NetContractual + igv.NetContractual, 2);
            parcial.AdicionalAmount1 = Math.Round(subTotal.AdicionalAmount1 + igv.AdicionalAmount1, 2);
            parcial.AdicionalAmount2 = Math.Round(subTotal.AdicionalAmount2 + igv.AdicionalAmount2, 2);
            parcial.AdicionalAmount3 = Math.Round(subTotal.AdicionalAmount3 + igv.AdicionalAmount3, 2);
            parcial.AdicionalAmount4 = Math.Round(subTotal.AdicionalAmount4 + igv.AdicionalAmount4, 2);
            parcial.Adicionals = Math.Round(subTotal.Adicionals + igv.Adicionals, 2);
            parcial.AccumulatedAmount = Math.Round(subTotal.AccumulatedAmount + igv.AccumulatedAmount, 2);

            total.NumberItem = "00";
            total.Description = "TOTAL";
            total.ContractualAmount += parcial.ContractualAmount;
            total.DeductiveAmount1 += parcial.DeductiveAmount1;
            total.DeductiveAmount2 += parcial.DeductiveAmount2;
            total.DeductiveAmount3 += parcial.DeductiveAmount3;
            total.DeductiveAmount4 += parcial.DeductiveAmount4;
            total.Deductives += parcial.Deductives;
            total.NetContractual += parcial.NetContractual;
            total.AdicionalAmount1 += parcial.AdicionalAmount1;
            total.AdicionalAmount2 += parcial.AdicionalAmount2;
            total.AdicionalAmount3 += parcial.AdicionalAmount3;
            total.AdicionalAmount4 += parcial.AdicionalAmount4;
            total.Adicionals += parcial.Adicionals;
            total.AccumulatedAmount += parcial.AccumulatedAmount;

            data.Add(directCost);
            data.Add(fixedExpenses);
            data.Add(variableExpenses);
            data.Add(utility);
            data.Add(subTotal);
            data.Add(igv);
            data.Add(parcial);

            //--------------------OTROS COMPONENTES

            var oCBudgets = await _context.OCBudgets
                .Include(x => x.ProjectFormula)
                .Include(x => x.BudgetType)
                .Where(x => x.BudgetType.Name == "Contractual" && x.NumberItem.Length < 3 && x.Group == 2)
                .AsNoTracking()
                .ToListAsync();

            header = new ConsolidatedBudget();
            header.NumberItem = "II.";
            header.Description = "OTROS COMPONENTE";
            data.Add(header);

            sumaContractual = 0.0;
            sumaDeductiveAmount1 = 0.0;
            sumaDeductiveAmount2 = 0.0;
            sumaDeductiveAmount3 = 0.0;
            sumaDeductiveAmount4 = 0.0;
            sumaDeductives = 0.0;
            sumaNetContractual = 0.0;
            sumaAdicionalAmount1 = 0.0;
            sumaAdicionalAmount2 = 0.0;
            sumaAdicionalAmount3 = 0.0;
            sumaAdicionalAmount4 = 0.0;
            sumaAdicionals = 0.0;
            sumaAccumulatedAmount = 0.0;

            foreach (var budget in oCBudgets)
            {

                var aux = new ConsolidatedBudget();

                //-------La suma de los deductivos
                var deductiveBudgets = await _context.OCBudgets
                    .Where(x => x.ProjectFormulaId == budget.ProjectFormulaId && x.BudgetType.Name == "Deductivo"
                    && x.NumberItem.Length < 3 && x.Group == 2)
                    .AsNoTracking()
                    .ToListAsync();

                var deductivesFormula = 0.00;

                foreach (var deductives in deductiveBudgets)
                    deductivesFormula = deductivesFormula + deductives.TotalPrice;

                //-------Los deductivos RRP-19
                var deductiveAmount1Entity = await _context.OCBudgets
                    .Include(x => x.BudgetTitle)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.ProjectFormulaId == budget.ProjectFormulaId && x.BudgetTitle.Name == "Deductivo N° 01 (RRP-19)"
                    && x.NumberItem.Length < 3 && x.Group == 2);

                var deductiveAmount1Formula = 0.0;

                if (deductiveAmount1Entity != null)
                    deductiveAmount1Formula = deductiveAmount1Entity.TotalPrice;

                //-------Los deductivos Colector de Descarga
                var deductiveAmount2Entity = await _context.OCBudgets
                    .Include(x => x.BudgetTitle)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.ProjectFormulaId == budget.ProjectFormulaId && x.BudgetTitle.Name == "Deductivo N° 02 (Colector de Descarga)"
                    && x.NumberItem.Length < 3 && x.Group == 2);

                var deductiveAmount2Formula = 0.0;

                if (deductiveAmount2Entity != null)
                    deductiveAmount2Formula = deductiveAmount2Entity.TotalPrice;

                //--------Los deductivos Anclajes Líneas HD
                var deductiveAmount3Entity = await _context.OCBudgets
                    .Include(x => x.BudgetTitle)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.ProjectFormulaId == budget.ProjectFormulaId && x.BudgetTitle.Name == "Deductivo N° 03 (Anclajes Líneas HD)"
                    && x.NumberItem.Length < 3 && x.Group == 2);

                var deductiveAmount3Formula = 0.0;

                if (deductiveAmount3Entity != null)
                    deductiveAmount3Formula = deductiveAmount3Entity.TotalPrice;

                //--------Los deductivos Dimensionamiento de Cámaras
                var deductiveAmount4Entity = await _context.OCBudgets
                    .Include(x => x.BudgetTitle)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.ProjectFormulaId == budget.ProjectFormulaId && x.BudgetTitle.Name == "Deductivo N° 04 (Dimensionamiento de Cámaras)"
                    && x.NumberItem.Length < 3 && x.Group == 2);

                var deductiveAmount4Formula = 0.0;

                if (deductiveAmount3Entity != null)
                    deductiveAmount3Formula = deductiveAmount3Entity.TotalPrice;

                //-------La suma de los adicionales
                var adicionalBudgets = await _context.OCBudgets
                    .Where(x => x.ProjectFormulaId == budget.ProjectFormulaId && x.BudgetType.Name == "Adicional"
                    && x.NumberItem.Length < 3 && x.Group == 2)
                    .AsNoTracking()
                    .ToListAsync();

                var adicionalsFormula = 0.0;

                foreach (var adicionals in adicionalBudgets)
                    adicionalsFormula = adicionalsFormula + adicionals.TotalPrice;

                //--------Los adicionales RRP-19
                var adicionalAmount1Entity = await _context.OCBudgets
                    .Include(x => x.BudgetTitle)
                    .Include(x => x.BudgetType)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.ProjectFormulaId == budget.ProjectFormulaId && x.BudgetTitle.Name == "Adicional N°01 (RRP-19)"
                    && x.NumberItem.Length < 3 && x.BudgetType.Name == "Adicional" && x.Group == 2);

                var adicionalAmount1Formula = 0.0;

                if (adicionalAmount1Entity != null)
                    adicionalAmount1Formula = adicionalAmount1Entity.TotalPrice;

                //-------Los adicionales Colector de Descarga
                var adicionalAmount2Entity = await _context.OCBudgets
                    .Include(x => x.BudgetTitle)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.ProjectFormulaId == budget.ProjectFormulaId && x.BudgetTitle.Name == "Adicional N° 02 (Colector de Descarga)"
                    && x.NumberItem.Length < 3 && x.Group == 2);

                var adicionalAmount2Formula = 0.0;

                if (adicionalAmount2Entity != null)
                    adicionalAmount2Formula = adicionalAmount2Entity.TotalPrice;

                //--------Los adicionales Anclajes Líneas HD
                var adicionalAmount3Entity = await _context.OCBudgets
                    .Include(x => x.BudgetTitle)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.ProjectFormulaId == budget.ProjectFormulaId && x.BudgetTitle.Name == "Adicional N° 03 (Anclajes Líneas HD)"
                    && x.NumberItem.Length < 3 && x.Group == 2);

                var adicionalAmount3Formula = 0.0;

                if (adicionalAmount3Entity != null)
                    adicionalAmount3Formula = adicionalAmount3Entity.TotalPrice;

                //--------Los adicionales Dimensionamiento de Cámaras
                var adicionalAmount4Entity = await _context.OCBudgets
                    .Include(x => x.BudgetTitle)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.ProjectFormulaId == budget.ProjectFormulaId && x.BudgetTitle.Name == "Adicional N° 04 (Dimensionamiento de Cámaras)"
                    && x.NumberItem.Length < 3 && x.Group == 2);

                var adicionalAmount4Formula = 0.0;

                if (adicionalAmount4Entity != null)
                    adicionalAmount4Formula = adicionalAmount3Entity.TotalPrice;


                var netContractualFormula = budget.TotalPrice + deductivesFormula;

                var accumulatedAmountFormula = budget.TotalPrice + deductivesFormula + adicionalsFormula;

                aux.NumberItem = budget.ProjectFormula.Code;
                aux.Description = budget.Description;
                aux.ContractualAmount = budget.TotalPrice;
                aux.DeductiveAmount1 = deductiveAmount1Formula;
                aux.DeductiveAmount2 = deductiveAmount2Formula;
                aux.DeductiveAmount3 = deductiveAmount3Formula;
                aux.DeductiveAmount4 = deductiveAmount4Formula;
                aux.Deductives = deductivesFormula;
                aux.NetContractual = netContractualFormula;
                aux.AdicionalAmount1 = adicionalAmount1Formula;
                aux.AdicionalAmount2 = adicionalAmount2Formula;
                aux.AdicionalAmount3 = adicionalAmount3Formula;
                aux.AdicionalAmount4 = adicionalAmount4Formula;
                aux.Adicionals = adicionalsFormula;
                aux.AccumulatedAmount = accumulatedAmountFormula;

                sumaContractual += budget.TotalPrice;
                sumaDeductiveAmount1 += deductiveAmount1Formula;
                sumaDeductiveAmount2 += deductiveAmount2Formula;
                sumaDeductiveAmount3 += deductiveAmount3Formula;
                sumaDeductiveAmount4 += deductiveAmount4Formula;
                sumaDeductives += deductivesFormula;
                sumaNetContractual += netContractualFormula;
                sumaAdicionalAmount1 += adicionalAmount1Formula;
                sumaAdicionalAmount2 += adicionalAmount2Formula;
                sumaAdicionalAmount3 += adicionalAmount3Formula;
                sumaAdicionalAmount4 += adicionalAmount4Formula;
                sumaAdicionals += adicionalsFormula;
                sumaAccumulatedAmount += accumulatedAmountFormula;


                data.Add(aux);
            }
            f16.Description = "GASTOS GENERALES";

            directCost = new ConsolidatedBudget();
            directCost.Description = "COSTO DIRECTO";
            directCost.ContractualAmount = sumaContractual;
            directCost.DeductiveAmount1 = sumaDeductiveAmount1;
            directCost.DeductiveAmount2 = sumaDeductiveAmount2;
            directCost.DeductiveAmount3 = sumaDeductiveAmount3;
            directCost.DeductiveAmount4 = sumaDeductiveAmount4;
            directCost.Deductives = sumaDeductives;
            directCost.NetContractual = sumaNetContractual;
            directCost.AdicionalAmount1 = sumaAdicionalAmount1;
            directCost.AdicionalAmount2 = sumaAdicionalAmount2;
            directCost.AdicionalAmount3 = sumaAdicionalAmount3;
            directCost.AdicionalAmount4 = sumaAdicionalAmount4;
            directCost.Adicionals = sumaAdicionals;
            directCost.AccumulatedAmount = sumaAccumulatedAmount;

            utility = new ConsolidatedBudget();
            utility.Description = "UTILIDAD";
            utility.ContractualAmount = Math.Round(sumaContractual * 0.0711, 2);
            utility.DeductiveAmount1 = Math.Round(sumaDeductiveAmount1 * 0.0711, 2);
            utility.DeductiveAmount2 = Math.Round(sumaDeductiveAmount2 * 0.0711, 2);
            utility.DeductiveAmount3 = Math.Round(sumaDeductiveAmount3 * 0.0711, 2);
            utility.DeductiveAmount4 = Math.Round(sumaDeductiveAmount4 * 0.0711, 2);
            utility.Deductives = Math.Round(sumaDeductives * 0.0711, 2);
            utility.NetContractual = Math.Round(sumaNetContractual * 0.0711, 2);
            utility.AdicionalAmount1 = Math.Round(sumaAdicionalAmount1 * 0.0711, 2);
            utility.AdicionalAmount2 = Math.Round(sumaAdicionalAmount2 * 0.0711, 2);
            utility.AdicionalAmount3 = Math.Round(sumaAdicionalAmount3 * 0.0711, 2);
            utility.AdicionalAmount4 = Math.Round(sumaAdicionalAmount4 * 0.0711, 2);
            utility.Adicionals = Math.Round(sumaAdicionals * 0.0711, 2);
            utility.AccumulatedAmount = Math.Round(sumaAccumulatedAmount * 0.0711, 2);

            subTotal = new ConsolidatedBudget();
            subTotal.Description = "SUB-TOTAL";
            subTotal.ContractualAmount = Math.Round(directCost.ContractualAmount + utility.ContractualAmount + f16.ContractualAmount, 2);
            subTotal.DeductiveAmount1 = Math.Round(directCost.DeductiveAmount1 + utility.DeductiveAmount1 + f16.DeductiveAmount1, 2);
            subTotal.DeductiveAmount2 = Math.Round(directCost.DeductiveAmount2 + utility.DeductiveAmount2 + f16.DeductiveAmount2, 2);
            subTotal.DeductiveAmount3 = Math.Round(directCost.DeductiveAmount3 + utility.DeductiveAmount3 + f16.DeductiveAmount3, 2);
            subTotal.DeductiveAmount4 = Math.Round(directCost.DeductiveAmount4 + utility.DeductiveAmount4 + f16.DeductiveAmount4, 2);
            subTotal.Deductives = Math.Round(directCost.Deductives + utility.Deductives + f16.Deductives, 2);
            subTotal.NetContractual = Math.Round(directCost.NetContractual + utility.NetContractual + f16.NetContractual, 2);
            subTotal.AdicionalAmount1 = Math.Round(directCost.AdicionalAmount1 + utility.AdicionalAmount1 + f16.AdicionalAmount1, 2);
            subTotal.AdicionalAmount2 = Math.Round(directCost.AdicionalAmount2 + utility.AdicionalAmount2 + f16.AdicionalAmount2, 2);
            subTotal.AdicionalAmount3 = Math.Round(directCost.AdicionalAmount3 + utility.AdicionalAmount3 + f16.AdicionalAmount3, 2);
            subTotal.AdicionalAmount4 = Math.Round(directCost.AdicionalAmount4 + utility.AdicionalAmount4 + f16.AdicionalAmount4, 2);
            subTotal.Adicionals = Math.Round(directCost.Adicionals + utility.Adicionals + f16.Adicionals, 2);
            subTotal.AccumulatedAmount = Math.Round(directCost.AccumulatedAmount + utility.AccumulatedAmount + f16.AccumulatedAmount, 2);

            igv = new ConsolidatedBudget();
            igv.Description = "IGV";
            igv.ContractualAmount = Math.Round(subTotal.ContractualAmount * 0.18, 2);
            igv.DeductiveAmount1 = Math.Round(subTotal.DeductiveAmount1 * 0.18, 2);
            igv.DeductiveAmount2 = Math.Round(subTotal.DeductiveAmount2 * 0.18, 2);
            igv.DeductiveAmount3 = Math.Round(subTotal.DeductiveAmount3 * 0.18, 2);
            igv.DeductiveAmount4 = Math.Round(subTotal.DeductiveAmount4 * 0.18, 2);
            igv.Deductives = Math.Round(subTotal.Deductives * 0.18, 2);
            igv.NetContractual = Math.Round(subTotal.NetContractual * 0.18, 2);
            igv.AdicionalAmount1 = Math.Round(subTotal.AdicionalAmount1 * 0.18, 2);
            igv.AdicionalAmount2 = Math.Round(subTotal.AdicionalAmount2 * 0.18, 2);
            igv.AdicionalAmount3 = Math.Round(subTotal.AdicionalAmount3 * 0.18, 2);
            igv.AdicionalAmount4 = Math.Round(subTotal.AdicionalAmount4 * 0.18, 2);
            igv.Adicionals = Math.Round(subTotal.Adicionals * 0.18, 2);
            igv.AccumulatedAmount = Math.Round(subTotal.AccumulatedAmount * 0.18, 2);

            parcial = new ConsolidatedBudget();
            parcial.Description = "PARCIAL";
            parcial.ContractualAmount = Math.Round(subTotal.ContractualAmount + igv.ContractualAmount, 2);
            parcial.DeductiveAmount1 = Math.Round(subTotal.DeductiveAmount1 + igv.DeductiveAmount1, 2);
            parcial.DeductiveAmount2 = Math.Round(subTotal.DeductiveAmount2 + igv.DeductiveAmount2, 2);
            parcial.DeductiveAmount3 = Math.Round(subTotal.DeductiveAmount3 + igv.DeductiveAmount3, 2);
            parcial.DeductiveAmount4 = Math.Round(subTotal.DeductiveAmount4 + igv.DeductiveAmount4, 2);
            parcial.Deductives = Math.Round(subTotal.Deductives + igv.Deductives, 2);
            parcial.NetContractual = Math.Round(subTotal.NetContractual + igv.NetContractual, 2);
            parcial.AdicionalAmount1 = Math.Round(subTotal.AdicionalAmount1 + igv.AdicionalAmount1, 2);
            parcial.AdicionalAmount2 = Math.Round(subTotal.AdicionalAmount2 + igv.AdicionalAmount2, 2);
            parcial.AdicionalAmount3 = Math.Round(subTotal.AdicionalAmount3 + igv.AdicionalAmount3, 2);
            parcial.AdicionalAmount4 = Math.Round(subTotal.AdicionalAmount4 + igv.AdicionalAmount4, 2);
            parcial.Adicionals = Math.Round(subTotal.Adicionals + igv.Adicionals, 2);
            parcial.AccumulatedAmount = Math.Round(subTotal.AccumulatedAmount + igv.AccumulatedAmount, 2);

            total.ContractualAmount += parcial.ContractualAmount;
            total.DeductiveAmount1 += parcial.DeductiveAmount1;
            total.DeductiveAmount2 += parcial.DeductiveAmount2;
            total.DeductiveAmount3 += parcial.DeductiveAmount3;
            total.DeductiveAmount4 += parcial.DeductiveAmount4;
            total.Deductives += parcial.Deductives;
            total.NetContractual += parcial.NetContractual;
            total.AdicionalAmount1 += parcial.AdicionalAmount1;
            total.AdicionalAmount2 += parcial.AdicionalAmount2;
            total.AdicionalAmount3 += parcial.AdicionalAmount3;
            total.AdicionalAmount4 += parcial.AdicionalAmount4;
            total.Adicionals += parcial.Adicionals;
            total.AccumulatedAmount += parcial.AccumulatedAmount;

            var porcentaje = new ConsolidatedBudget();
            porcentaje.Description = "Porcentaje Contractual Acumulado";
            porcentaje.ContractualAmount = Math.Round(total.AccumulatedAmount / total.ContractualAmount * 100, 2);

            //data.Add(directCost);
            //data.Add(f16);
            //data.Add(variableExpenses);
            //data.Add(fixedExpenses);
            //data.Add(utility);
            data.Add(subTotal);
            data.Add(igv);
            data.Add(parcial);
            data.Add(total);
            data.Add(porcentaje);

            await _context.ConsolidatedBudgets.AddRangeAsync(data);
            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
