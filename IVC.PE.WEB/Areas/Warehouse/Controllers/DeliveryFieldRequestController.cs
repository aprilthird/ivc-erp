using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectFormulaViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectPhaseViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.MeasurementUnitViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyFamilyViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyGroupViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BudgetTitleViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.GoalBudgetInputViewModels;
using IVC.PE.WEB.Areas.Warehouse.ViewModels.FieldRequestViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Warehouse.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Warehouse.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.WAREHOUSE)]
    [Route("almacenes/entregas")]
    public class DeliveryFieldRequestController : BaseController
    {
        public DeliveryFieldRequestController(IvcDbContext context,
            UserManager<ApplicationUser> userManager,
            ILogger<DeliveryFieldRequestController> logger) : base(context, userManager, logger)
        {
        }

        public IActionResult Index() => View();


        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(int year = 0, int month = 0, Guid? budgetTitleId = null, Guid? projectFormulaId = null, 
            Guid? workFrontId = null,Guid? sewerGroupId = null,Guid? supplyFamilyId = null, Guid? supplyGroupId = null,
            Guid? weekId = null)
        {
            var query = _context.FieldRequestFoldings
                .Include(x => x.FieldRequest)
                .Include(x => x.GoalBudgetInput)
                .Include(x => x.GoalBudgetInput.Supply)
                .Include(x => x.GoalBudgetInput.MeasurementUnit)
                .Include(x => x.GoalBudgetInput.Supply.SupplyFamily)
                .Include(x => x.GoalBudgetInput.Supply.SupplyGroup)
                .Where(x => x.FieldRequest.Status == ConstantHelpers.Warehouse.FieldRequest.Status.ATTENDED
                && x.GoalBudgetInput.ProjectFormula.ProjectId == GetProjectId())
                .AsEnumerable();

            if (supplyFamilyId.HasValue)
                query = query.Where(x => x.GoalBudgetInput.Supply.SupplyFamilyId == supplyFamilyId.Value);
            if (supplyGroupId.HasValue)
                query = query.Where(x => x.GoalBudgetInput.Supply.SupplyGroupId == supplyGroupId.Value);
            if (projectFormulaId.HasValue)
                query = query.Where(x => x.GoalBudgetInput.ProjectFormulaId == projectFormulaId.Value);
            if (budgetTitleId.HasValue)
                query = query.Where(x => x.GoalBudgetInput.BudgetTitleId == budgetTitleId.Value);
            if (workFrontId.HasValue)
                query = query.Where(x => x.GoalBudgetInput.WorkFrontId == workFrontId.Value);
            if (sewerGroupId.HasValue)
                query = query.Where(x => x.FieldRequest.SewerGroupId == sewerGroupId.Value);

            if (year > 0)
                query = query.Where(x => x.FieldRequest.DeliveryDate.Year == year);

            if (month > 0)
                query = query.Where(x => x.FieldRequest.DeliveryDate.Month == month);

            var weekCalendar = await _context.ProjectCalendarWeeks
                .ToListAsync();


            if (weekId.HasValue)
            {
                var aux = weekCalendar.FirstOrDefault(x => x.Id == weekId.Value);
                /*
                query = query.Where(x => x.FieldRequest.DeliveryDate.Year == aux.Year
                && (x.FieldRequest.DeliveryDate.Month == aux.WeekStart.Month || x.FieldRequest.DeliveryDate.Month == aux.WeekStart.Month)
                && (x.FieldRequest.DeliveryDate));*/

                query = query.Where(x => x.FieldRequest.DeliveryDate >= aux.WeekStart
                && x.FieldRequest.DeliveryDate <= aux.WeekEnd);
            }

            var result = new List<DeliveryFieldRequestViewModel>();

            var stocks = await _context.Stocks
                .Where(x => x.ProjectId == GetProjectId())
                .ToListAsync();

            var data = query.GroupBy(x => x.GoalBudgetInput.SupplyId);

            foreach (var folding in data)
            {
                var supply = folding.FirstOrDefault();

                var stock = stocks.FirstOrDefault(x => x.SupplyId == supply.GoalBudgetInput.SupplyId);

                var meta = query
                    .Where(x => x.GoalBudgetInput.SupplyId == supply.GoalBudgetInput.SupplyId)
                    .GroupBy(x => x.GoalBudgetInputId);

                var delivered = 0.0;
                var techo = 0.0;
                var toReq = 0.0;
                var cost = 0.0;

                foreach(var item in folding)
                {
                    delivered += item.DeliveredQuantity;
                }

                foreach(var aux in meta)
                {
                    techo += aux.FirstOrDefault().GoalBudgetInput.WarehouseCurrentMetered;
                }

                cost = Math.Round(delivered * stock.UnitPrice, 2);
                toReq = Math.Round(techo - delivered, 2);

                result.Add(new DeliveryFieldRequestViewModel
                {
                    Id = supply.Id,
                    GoalBudgetInput = new GoalBudgetInputViewModel
                    {
                        SupplyId = supply.GoalBudgetInput.SupplyId,
                        Supply = new SupplyViewModel
                        {
                            Description = supply.GoalBudgetInput.Supply.Description,
                            SupplyFamily = new SupplyFamilyViewModel
                            {
                                Code = supply.GoalBudgetInput.Supply.SupplyFamily.Code,
                                Name = supply.GoalBudgetInput.Supply.SupplyFamily.Name
                            },
                            SupplyGroup = new SupplyGroupViewModel
                            {
                                Code = supply.GoalBudgetInput.Supply.SupplyGroup.Code,
                                Name = supply.GoalBudgetInput.Supply.SupplyGroup.Name
                            },
                            CorrelativeCode = supply.GoalBudgetInput.Supply.CorrelativeCode
                        },
                        MeasurementUnit = new MeasurementUnitViewModel
                        {
                            Abbreviation = supply.GoalBudgetInput.MeasurementUnit.Abbreviation
                        },
                        WarehouseCurrentMetered= supply.GoalBudgetInput.WarehouseCurrentMetered.ToString("N2", CultureInfo.InvariantCulture)
                    },
                    DeliveredQuantity = delivered.ToString("N2", CultureInfo.InvariantCulture),
                    Techo = techo.ToString("N2", CultureInfo.InvariantCulture),
                    Cost = Math.Round(cost, 2),
                    CostString = "S/ " + cost.ToString("N2", CultureInfo.InvariantCulture),
                    MeasureToRequest = toReq.ToString("N2", CultureInfo.InvariantCulture)
                });
            }


            return Ok(result);
        }

        [HttpGet("detalles/listar")]
        public async Task<IActionResult> GetDetail(Guid? supplyId = null)
        {
            var query = await _context.FieldRequestFoldings
                .Include(x => x.FieldRequest)
                .Include(x => x.ProjectPhase)
                .Include(x => x.GoalBudgetInput)
                .Include(x => x.GoalBudgetInput.MeasurementUnit)
                .Where(x => x.GoalBudgetInput.SupplyId == supplyId
                && x.FieldRequest.Status == ConstantHelpers.Warehouse.FieldRequest.Status.ATTENDED)
                .ToListAsync();

            var folding = new List<DeliveryFieldRequestFoldingViewModel>();

            var stocks = await _context.Stocks
                .Where(x => x.ProjectId == GetProjectId())
                .ToListAsync();

            foreach (var item in query)
            {
                var stock = stocks.FirstOrDefault(x => x.SupplyId == item.GoalBudgetInput.SupplyId);

                folding.Add(new DeliveryFieldRequestFoldingViewModel
                {
                    FieldRequest = new FieldRequestViewModel
                    {
                        DeliveryDate = item.FieldRequest.DeliveryDate.ToDateString(),
                        DocumentNumber = item.FieldRequest.DocumentNumber.ToString("D4")
                    },
                    ProjectPhase = new ProjectPhaseViewModel
                    {
                        Code = item.ProjectPhase.Code
                    },
                    GoalBudgetInput = new GoalBudgetInputViewModel
                    {
                        MeasurementUnit = new MeasurementUnitViewModel
                        {
                            Abbreviation = item.GoalBudgetInput.MeasurementUnit.Abbreviation
                        }
                    },
                    DeliveredQuantity = item.DeliveredQuantity.ToString("N2", CultureInfo.InvariantCulture),
                    Parcial = "S/ " + Math.Round(item.DeliveredQuantity * stock.UnitPrice, 2).ToString("N2", CultureInfo.InvariantCulture)
                });
            }

            return Ok(folding);
        }

    }
}
