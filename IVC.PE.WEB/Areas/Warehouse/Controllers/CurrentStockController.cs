using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.WEB.Areas.Warehouse.ViewModels.StockViewModels;
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

namespace IVC.PE.WEB.Areas.Warehouse.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Warehouse.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.WAREHOUSE)]
    [Route("almacenes/existencias-contables")]
    public class CurrentStockController : BaseController
    {
        public CurrentStockController(IvcDbContext context,
           ILogger<CurrentStockController> logger) : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? supplyFamilyId = null,
            Guid? supplyGroupId = null, Guid? providerId = null)
        {
            var pId = GetProjectId();

            var query = _context.Stocks
                .Include(x => x.Supply)
                .Include(x => x.Supply.SupplyGroup)
                .Include(x => x.Supply.SupplyFamily)
                .Include(x => x.Supply.MeasurementUnit)
                .Where(x => x.ProjectId == pId);

            if (supplyFamilyId.HasValue)
                query = query.Where(x => x.Supply.SupplyFamilyId == supplyFamilyId);

            if (supplyGroupId.HasValue)
                query = query.Where(x => x.Supply.SupplyGroupId == supplyGroupId);

            var supplyEntries = await _context.SupplyEntryItems
                .Include(x => x.OrderItem)
                .Include(x => x.SupplyEntry)
                .Include(x => x.SupplyEntry.Order)
                .Include(x => x.SupplyEntry.Order.Provider)
                .Include(x => x.SupplyEntry.Warehouse.WarehouseType)
                .Where(x => x.SupplyEntry.Status == ConstantHelpers.Warehouse.SupplyEntry.Status.CONFIRMED
                && x.SupplyEntry.Warehouse.WarehouseType.ProjectId == pId)
                .ToListAsync();

            var fieldRequests = await _context.FieldRequestFoldings
                .Include(x => x.FieldRequest)
                .Include(x => x.FieldRequest.BudgetTitle)
                .Where(x => x.FieldRequest.Status == ConstantHelpers.Warehouse.FieldRequest.Status.ATTENDED
                && x.FieldRequest.BudgetTitle.ProjectId == pId)
                .ToListAsync();

            var stocks = new List<CurrentStockViewModel>();

            var today = DateTime.Today;

            foreach(var item in query)
            {
                var entries = supplyEntries
                    .Where(x => x.OrderItem.SupplyId == item.SupplyId
                    //&& providerId.HasValue ? x.SupplyEntry.Order.ProviderId == providerId.Value : true)
                    )
                    .ToList();

                if (providerId.HasValue)
                {
                    entries = entries.Where(x => x.SupplyEntry.Order.ProviderId == providerId).ToList();
                    if(!entries.Any(x => x.OrderItem.SupplyId == item.SupplyId))
                    {
                        continue;
                    }
                }
                
                var requests = fieldRequests
                    .Where(x => x.GoalBudgetInput.SupplyId == item.SupplyId
                    && x.FieldRequest.DeliveryDate.Year == today.Year
                    && x.FieldRequest.DeliveryDate.Month < today.Month)
                    .ToList();

                var ingresos = entries.Sum(x => x.Measure);

                var valorizadas = entries.Where(x => x.SupplyEntry.IsValued == true)
                    .Sum(x => x.Measure);

                var salidas = requests.Sum(x => x.DeliveredQuantity);

                var contable = Math.Round(ingresos - valorizadas - salidas, 2);

                var monto = Math.Round(item.UnitPrice * contable, 2);

                var incomes = Math.Round(item.UnitPrice * ingresos, 2);

                var outcomes = Math.Round(item.UnitPrice * (valorizadas + salidas), 2);

                stocks.Add(new CurrentStockViewModel
                {
                    SupplyId = item.SupplyId,
                    Supply = new Logistics.ViewModels.SupplyViewModels.SupplyViewModel
                    {
                        CorrelativeCode = item.Supply.CorrelativeCode,
                        Description = item.Supply.Description,
                        SupplyFamily = new Logistics.ViewModels.SupplyFamilyViewModels.SupplyFamilyViewModel
                        {
                            Code = item.Supply.SupplyFamily.Code,
                            Name = item.Supply.SupplyFamily.Name
                        },
                        SupplyGroup = new Logistics.ViewModels.SupplyGroupViewModels.SupplyGroupViewModel
                        {
                            Code = item.Supply.SupplyGroup.Code,
                            Name = item.Supply.SupplyGroup.Name
                        },
                        MeasurementUnit = new Logistics.ViewModels.MeasurementUnitViewModels.MeasurementUnitViewModel
                        {
                            Abbreviation = item.Supply.MeasurementUnit.Abbreviation
                        }
                    },
                    Entries = ingresos.ToString("N2", CultureInfo.InvariantCulture),
                    EntriesFloat = ingresos,
                    Requests = (valorizadas + salidas).ToString("N2", CultureInfo.InvariantCulture),
                    RequestsFloat = (valorizadas + salidas),
                    UnitPrice = item.UnitPrice.ToString("N2", CultureInfo.InvariantCulture),
                    Measure = contable.ToString("N2", CultureInfo.InvariantCulture),
                    MeasureFloat = contable,
                    Parcial = monto,
                    ParcialString = "S/ " + monto.ToString("N2", CultureInfo.InvariantCulture),
                    Providers = string.Join("-",
                    entries.Select(x => x.SupplyEntry.Order.Provider.Tradename).Distinct().ToList()),
                    totalEntry = incomes,
                    totalRequest = outcomes
                });
            }

            return Ok(stocks);
        }
    }
}
