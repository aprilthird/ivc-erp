using ClosedXML.Excel;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Production;
using IVC.PE.ENTITIES.UspModels.Production;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectFormulaViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.SewerGroupViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.UserViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontHeadViewModels;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.ProjectCalendarWeekViewModels;
using IVC.PE.WEB.Areas.Production.ViewModels.WeeklyAdvanceViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Production.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Production.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.PRODUCTION)]
    [Route("produccion/avance-semanal")]
    public class WeeklyAdvanceController : BaseController
    {

        public WeeklyAdvanceController(IvcDbContext context,
            ILogger<WeeklyAdvanceController> logger): base(context, logger)
        {

        }
        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? projectCalendarWeekId = null, Guid? workFrontHeadId = null, Guid? sewerGroupId = null, Guid? projectFormulaId = null)
        {
            var query = _context.WeeklyAdvances
                .Include(x => x.WorkFrontHead)
                .Include(x => x.WorkFrontHead.User)
                .Include(x => x.SewerGroup)
                .Where(x => x.Id != null);

            if (projectCalendarWeekId.HasValue)
                query = query.Where(x => x.ProjectCalendarWeekId == projectCalendarWeekId);
            if (workFrontHeadId.HasValue)
                query = query.Where(x => x.WorkFrontHeadId == workFrontHeadId);
            if (sewerGroupId.HasValue)
                query = query.Where(x => x.SewerGroupId == sewerGroupId);
            if (projectFormulaId.HasValue)
                query = query.Where(x => x.ProjectFormulaId == projectFormulaId);

            var data = await query
                .Select(x => new WeeklyAdvanceViewModel
                {
                    Id = x.Id,
                    ProjectFormulaId = x.ProjectFormulaId,
                    ProjectFormula = new ProjectFormulaViewModel
                    {
                        Code = x.ProjectFormula.Code,
                        Name = x.ProjectFormula.Name
                    },
                    ProjectCalendarWeekId = x.ProjectCalendarWeekId,
                    ProjectCalendarWeek = new ProjectCalendarWeekViewModel
                    {
                        WeekStart = x.ProjectCalendarWeek.WeekStart.ToLocalDateFormat(),
                        WeekEnd = x.ProjectCalendarWeek.WeekEnd.ToLocalDateFormat()
                    },
                    WorkFrontHeadId = x.WorkFrontHeadId,
                    WorkFrontHead = new WorkFrontHeadViewModel
                    {
                        User = new UserViewModel
                        {
                            AuxFullName = x.WorkFrontHead.User.FullName
                        }
                    },
                    SewerGroupId = x.SewerGroupId,
                    SewerGroup = new SewerGroupViewModel
                    {
                        Code = x.SewerGroup.Code
                    },
                    TotalNetBudget = x.TotalNetBudget.ToString("N", CultureInfo.InvariantCulture),
                    AccumulatedBudget = x.AccumulatedBudget.ToString("N", CultureInfo.InvariantCulture),
                    PercentageAdvance = x.PercentageAdvance.ToString(),
                    WorkersNumberOP = x.WorkersNumberOP,
                    WorkersNumberOF = x.WorkersNumberOF,
                    WorkersNumberPE = x.WorkersNumberPE,
                    WorkerNumberTotal = x.WorkerNumberTotal,
                    SaleMO = x.SaleMO.ToString("N", CultureInfo.InvariantCulture),
                    SaleEQ = x.SaleEQ.ToString("N", CultureInfo.InvariantCulture),
                    SaleSubcontract = x.SaleSubcontract.ToString("N", CultureInfo.InvariantCulture),
                    SaleMaterials = x.SaleMaterials.ToString("N", CultureInfo.InvariantCulture),
                    SaleTotal = x.SaleTotal.ToString("N", CultureInfo.InvariantCulture),
                    GoalMO = x.GoalMO.ToString("N", CultureInfo.InvariantCulture),
                    GoalEQ = x.GoalEQ.ToString("N", CultureInfo.InvariantCulture),
                    GoalSubcontract = x.GoalSubcontract.ToString("N", CultureInfo.InvariantCulture),
                    GoalMaterials = x.GoalMaterials.ToString("N", CultureInfo.InvariantCulture),
                    GoalTotal = x.GoalTotal.ToString("N", CultureInfo.InvariantCulture),
                    CostEQ = x.CostEQ.ToString("N", CultureInfo.InvariantCulture),
                    CostMO = x.CostMO.ToString("N", CultureInfo.InvariantCulture),
                    CostSubcontract = x.CostSubcontract.ToString("N", CultureInfo.InvariantCulture),
                    CostMaterials = x.CostMaterials.ToString("N", CultureInfo.InvariantCulture),
                    CostTotal = x.CostTotal.ToString("N", CultureInfo.InvariantCulture),
                    MarginActual = x.MarginActual.ToString("N", CultureInfo.InvariantCulture),
                    MarginAccumulated = x.MarginAccumulated.ToString("N", CultureInfo.InvariantCulture)
                }).AsNoTracking()
                .ToListAsync();

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {

            var data = await _context.WeeklyAdvances
                .Select(x => new WeeklyAdvanceViewModel
                {
                    Id = x.Id,
                    ProjectCalendarWeekId = x.ProjectCalendarWeekId,
                    ProjectCalendarWeek = new ProjectCalendarWeekViewModel
                    {
                        WeekStart = x.ProjectCalendarWeek.WeekStart.ToLocalDateFormat(),
                        WeekEnd = x.ProjectCalendarWeek.WeekEnd.ToLocalDateFormat()
                    },
                    WorkFrontHeadId = x.WorkFrontHeadId,
                    WorkFrontHead = new WorkFrontHeadViewModel
                    {
                        User = new UserViewModel
                        {
                            AuxFullName = x.WorkFrontHead.User.FullName
                        }
                    },
                    SewerGroupId = x.SewerGroupId,
                    SewerGroup = new SewerGroupViewModel
                    {
                        Code = x.SewerGroup.Code
                    },
                    TotalNetBudget = x.TotalNetBudget.ToString("N", CultureInfo.InvariantCulture),
                    AccumulatedBudget = x.AccumulatedBudget.ToString("N", CultureInfo.InvariantCulture),
                    PercentageAdvance = x.PercentageAdvance.ToString(),
                    WorkersNumberOP = x.WorkersNumberOP,
                    WorkersNumberOF = x.WorkersNumberOF,
                    WorkersNumberPE = x.WorkersNumberPE,
                    WorkerNumberTotal = x.WorkerNumberTotal,
                    SaleMO = x.SaleMO.ToString("N", CultureInfo.InvariantCulture),
                    SaleEQ = x.SaleEQ.ToString("N", CultureInfo.InvariantCulture),
                    SaleSubcontract = x.SaleSubcontract.ToString("N", CultureInfo.InvariantCulture),
                    SaleMaterials = x.SaleMaterials.ToString("N", CultureInfo.InvariantCulture),
                    SaleTotal = x.SaleTotal.ToString("N", CultureInfo.InvariantCulture),
                    GoalMO = x.GoalMO.ToString("N", CultureInfo.InvariantCulture),
                    GoalEQ = x.GoalEQ.ToString("N", CultureInfo.InvariantCulture),
                    GoalSubcontract = x.GoalSubcontract.ToString("N", CultureInfo.InvariantCulture),
                    GoalMaterials = x.GoalMaterials.ToString("N", CultureInfo.InvariantCulture),
                    GoalTotal = x.GoalTotal.ToString("N", CultureInfo.InvariantCulture),
                    CostEQ = x.CostEQ.ToString("N", CultureInfo.InvariantCulture),
                    CostMO = x.CostMO.ToString("N", CultureInfo.InvariantCulture),
                    CostSubcontract = x.CostSubcontract.ToString("N", CultureInfo.InvariantCulture),
                    CostMaterials = x.CostMaterials.ToString("N", CultureInfo.InvariantCulture),
                    CostTotal = x.CostTotal.ToString("N", CultureInfo.InvariantCulture),
                    MarginActual = x.MarginActual.ToString("N", CultureInfo.InvariantCulture),
                    MarginAccumulated = x.MarginAccumulated.ToString("N", CultureInfo.InvariantCulture)
                }).AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return Ok(data);
        }

        [HttpPost("importar-datos")]
        public async Task<IActionResult> ImportData(IFormFile file, WeeklyAdvanceViewModel model)
        {
            var week = await _context.ProjectCalendarWeeks
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == model.ProjectCalendarWeekId);

            var header = await _context.PayrollMovementHeaders
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ProjectCalendarWeekId == week.Id);

            SqlParameter weekStartParam = new SqlParameter("@WeekStart", week.WeekStart);
            SqlParameter weekEndParam = new SqlParameter("@WeekEnd", week.WeekEnd);
            SqlParameter sewergroupParam = new SqlParameter("@SewerGroupId", model.SewerGroupId);
            SqlParameter projectParam = new SqlParameter("@ProjectId", GetProjectId());
            SqlParameter headerParam = new SqlParameter("@HeaderId", header.Id);

            var workers = _context.Set<UspWeeklyAdvanceTotalWorker>().FromSqlRaw("execute Production_uspWeeklyAdvanceTotalWorkers @WeekStart, @WeekEnd, @SewerGroupId, @ProjectId"
                , weekStartParam, weekEndParam, sewergroupParam, projectParam)
                .AsNoTracking()
                .FirstOrDefault();

            var workersCosts = _context.Set<UspWeeklyAdvanceTotalWorkerCost>().FromSqlRaw("execute Production_uspWeeklyAdvanceTotalWorkerCosts @WeekStart, @WeekEnd, @SewerGroupId, @ProjectId, @HeaderId"
                , weekStartParam, weekEndParam, sewergroupParam, projectParam, headerParam)
                 .AsNoTracking()
                 .AsQueryable();

            var weeklyAdvance = new WeeklyAdvance();
            weeklyAdvance.Id = Guid.NewGuid();

            var foldingBudgets = new List<FoldingBudgetWeeklyAdvance>();

            using (var mem = new MemoryStream())
            {
                await file.CopyToAsync(mem);
                using (var workBook =  new XLWorkbook(mem))
                {
                    var workSheet = workBook.Worksheets.FirstOrDefault();
                    var counter = 11;

                    var saleMO = 0.0;
                    var saleEQ = 0.0;
                    var saleSubContract = 0.0;
                    var saleMaterials = 0.0;

                    var goalMO = 0.0;
                    var goalEQ = 0.0;
                    var goalSubContract = 0.0;
                    var goalMaterials = 0.0;



                    while (!workSheet.Cell($"B{counter}").IsEmpty())
                    {
                        var avanceActualStr = workSheet.Cell($"H{counter}").GetString();
                        var avanceActual = 0.0;
                        if (!string.IsNullOrEmpty(avanceActualStr))
                        {
                            if (Double.TryParse(avanceActualStr, out double avanceActualDouble))
                                avanceActual = avanceActualDouble;
                        }
                        

                        if (avanceActual != 0.0)
                        {
                            var numberItem = workSheet.Cell($"B{counter}").GetString();
                            var budget = await _context.Budgets.AsNoTracking().FirstOrDefaultAsync(x => x.NumberItem == numberItem);

                            var folding = new FoldingBudgetWeeklyAdvance();

                            folding.Id = Guid.NewGuid();
                            folding.NumberItem = budget.NumberItem;
                            folding.WeeklyAdvanceId = weeklyAdvance.Id;
                            folding.Description = budget.Description;
                            folding.Unit = budget.Unit;
                            folding.ContractualMO = budget.ContractualMO;
                            folding.ContractualEQ = budget.ContractualEQ;
                            folding.ContractualSubcontract = budget.ContractualSubcontract;
                            folding.ContractualMaterials = budget.ContractualMaterials;
                            folding.ActualAdvance = avanceActual;

                            //-----------------------------

                            var saleMOSuma = Math.Round(avanceActual * budget.ContractualMO, 2);
                            saleMO += saleMOSuma;

                            var saleEQSuma = Math.Round(avanceActual * budget.ContractualEQ, 2);
                            saleEQ += saleEQSuma;

                            var saleSubContractSuma = Math.Round(avanceActual * budget.ContractualSubcontract, 2);
                            saleSubContract += saleSubContractSuma;

                            var saleMaterialsSuma = Math.Round(avanceActual * budget.ContractualMaterials, 2);
                            saleMaterials += saleMaterialsSuma;

                            //-----------------------------

                            var goalMOSuma = Math.Round(avanceActual * budget.CollaboratorMO, 2);
                            goalMO += goalMOSuma;

                            var goalEQSuma = Math.Round(avanceActual * budget.CollaboratorEQ, 2);
                            goalEQ += goalEQSuma;

                            var goalSubContractSuma = Math.Round(avanceActual * budget.ContractualSubcontract, 2);
                            goalSubContract += goalSubContractSuma;

                            var goalMaterialsSuma = Math.Round(avanceActual * budget.ContractualMaterials, 2);
                            goalMaterials += goalMaterialsSuma;

                            //-----------------------------

                            var costMOSuma = Math.Round(avanceActual);

                            foldingBudgets.Add(folding);
                        }

                        counter++;
                    }

                    weeklyAdvance.ProjectFormulaId = model.ProjectFormulaId;
                    weeklyAdvance.ProjectCalendarWeekId = model.ProjectCalendarWeekId;
                    weeklyAdvance.WorkFrontHeadId = model.WorkFrontHeadId;
                    weeklyAdvance.SewerGroupId = model.SewerGroupId;

                    if (workers != null)
                    {
                        weeklyAdvance.WorkerNumberTotal = workers.Totals;
                        weeklyAdvance.WorkersNumberPE = workers.Pawns;
                        weeklyAdvance.WorkersNumberOF = workers.Officials;
                        weeklyAdvance.WorkersNumberOP = workers.Operators;
                    }

                    weeklyAdvance.SaleMO = saleMO;
                    weeklyAdvance.SaleEQ = saleEQ;
                    weeklyAdvance.SaleSubcontract = saleSubContract;
                    weeklyAdvance.SaleMaterials = saleMaterials;
                    weeklyAdvance.SaleTotal = saleMO + saleEQ + saleSubContract + saleMaterials;

                    weeklyAdvance.CostMO = workersCosts.Select(x => x.TotalCost).Sum();


                    weeklyAdvance.GoalMO = goalMO;
                    weeklyAdvance.GoalEQ = goalEQ;
                    weeklyAdvance.GoalSubcontract = goalSubContract;
                    weeklyAdvance.GoalMaterials = goalMaterials;
                    weeklyAdvance.GoalTotal = goalMO +goalEQ + goalSubContract + goalSubContract;

                }
                mem.Close();
            }

            await _context.WeeklyAdvances.AddAsync(weeklyAdvance);
            await _context.FoldingBudgetWeeklyAdvances.AddRangeAsync(foldingBudgets);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("excel-carga-masiva")]
        public FileResult ExportExcelMassiveLoad()
        {
            string fileName = "AvanceSemanalCarga.xlsx";
            using (XLWorkbook wb = new XLWorkbook())
            {
                var workSheet = wb.Worksheets.Add("CargaMasiva");

                workSheet.Cell($"B2").Value = "Item";
                workSheet.Cell($"C2").Value = "Descripción";
                workSheet.Cell($"D2").Value = "Und.";
                workSheet.Cell($"E2").Value = "Metrado Total";
                workSheet.Cell($"F2").Value = "Avance Acumulado Anterior";
                workSheet.Cell($"G2").Value = "Avance Actual";
                workSheet.Cell($"H2").Value = "Avance Acumulado";

                workSheet.Cell($"B3").Value = "Info Aquí";
                workSheet.Cell($"B3").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Column(1).Width = 1;
                workSheet.Column(2).Width = 15;
                workSheet.Column(3).Width = 20;
                workSheet.Column(4).Width = 10;
                workSheet.Column(5).Width = 15;
                workSheet.Column(6).Width = 30;
                workSheet.Column(7).Width = 15;
                workSheet.Column(8).Width = 18;

                workSheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                workSheet.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                workSheet.Range("B2:H9").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                workSheet.Range("B2:H9").Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);


                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

    }
}