using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Warehouse;
using IVC.PE.WEB.Areas.Logistics.ViewModels.MeasurementUnitViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.OrderViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.ProviderViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyFamilyViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyGroupViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyViewModels;
using IVC.PE.WEB.Areas.Warehouse.ViewModels.StockViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QRCoder;
using Spire.Pdf;
using Spire.Pdf.Graphics;

namespace IVC.PE.WEB.Areas.Warehouse.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Warehouse.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.WAREHOUSE)]
    [Route("almacenes/existencias")]
    public class StockController : BaseController
    {
        public StockController(IvcDbContext context,
           ILogger<StockController> logger) : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? providerId = null, Guid? supplyGroupId = null, Guid? supplyFamilyId = null)
        {
            var entries = await _context.SupplyEntryItems
                .Include(x => x.SupplyEntry)
                .Include(x => x.OrderItem.Supply)
                .Include(x => x.SupplyEntry.Order.Provider)
                .Where(x => x.SupplyEntry.Status == ConstantHelpers.Warehouse.SupplyEntry.Status.CONFIRMED)
                .ToListAsync();

            var dispatches = await _context.FieldRequestFoldings
                .Include(x => x.FieldRequest)
                .Include(x => x.GoalBudgetInput)
                .Where(x => x.FieldRequest.Status == ConstantHelpers.Warehouse.FieldRequest.Status.ATTENDED)
                .ToListAsync();

            var query = _context.Stocks
                .Include(x => x.Supply)
                .Include(x => x.Supply.SupplyFamily)
                .Include(x => x.Supply.SupplyGroup)
                .Include(x => x.Supply.MeasurementUnit)
                .Where(x => x.ProjectId == GetProjectId());

            if (supplyGroupId.HasValue)
                query = query.Where(x => x.Supply.SupplyGroupId == supplyGroupId);

            if (supplyFamilyId.HasValue)
                query = query.Where(x => x.Supply.SupplyFamilyId == supplyFamilyId);

            var stocks = new List<StockViewModel>();
            /*
            var stocks = await query
                .Select(x => new StockViewModel
                {
                    Id = x.Id,
                    SupplyId = x.SupplyId,
                    Supply = new SupplyViewModel
                    {
                        SupplyFamilyId = x.Supply.SupplyFamilyId,
                        SupplyFamily = new SupplyFamilyViewModel
                        {
                            Code = x.Supply.SupplyFamily.Code,
                            Name = x.Supply.SupplyFamily.Name
                        },
                        SupplyGroupId = x.Supply.SupplyGroupId,
                        SupplyGroup = new SupplyGroupViewModel
                        {
                            Code = x.Supply.SupplyGroup.Code,
                            Name = x.Supply.SupplyGroup.Name
                        },
                        CorrelativeCode = x.Supply.CorrelativeCode,
                        Description = x.Supply.Description,
                        MeasurementUnit = new MeasurementUnitViewModel
                        {
                            Abbreviation = x.Supply.MeasurementUnit.Abbreviation
                        }
                    },
                    Measure = x.Measure.ToString("N2", CultureInfo.InvariantCulture),
                    MeasureFloat = x.Measure,
                    MinimumMeasure = x.MinimumMeasure.ToString("N2", CultureInfo.InvariantCulture),
                    UnitPrice = x.UnitPrice.ToString("N2", CultureInfo.InvariantCulture),
                    Parcial = x.Parcial,
                    ParcialString = x.Parcial.ToString("N2", CultureInfo.InvariantCulture),
                }).ToListAsync();

            var data = new List<StockViewModel>();
            */

            foreach(var item in query)
            {
                var prs = entries
                    .Where(x => x.OrderItem.Supply.SupplyGroupId == item.Supply.SupplyGroupId)
                    .ToList();

                if (providerId.HasValue)
                {
                    entries = entries.Where(x => x.SupplyEntry.Order.ProviderId == providerId).ToList();
                    if (!entries.Any(x => x.OrderItem.SupplyId == item.SupplyId))
                    {
                        continue;
                    }
                }
                var providersAux = string.Join(" - ", prs
                    .Select(x => x.OrderItem.Order.Provider.BusinessName)
                    .Distinct());
                
                var income = entries.Where(x => x.OrderItem.SupplyId == item.SupplyId).Sum(x => x.Measure);
                var aux_dispatch = dispatches.Where(x => x.GoalBudgetInput.SupplyId == item.SupplyId).Sum(x => x.DeliveredQuantity);

                var totalEntries = Math.Round(item.UnitPrice * income, 2);
                var totalDispatch = Math.Round(item.UnitPrice * aux_dispatch, 2);

                //var orderItem = _context.OrderItems.FirstOrDefault(x => x.OrderId == ).Select(x => x.UnitPrice);
                var exchangeRate = entries.Select(x => x.SupplyEntry.Order.ExchangeRate);

                stocks.Add(new StockViewModel
                {
                    Id = item.Id,
                    SupplyId = item.SupplyId,
                    Providers = providersAux,
                    Supply = new SupplyViewModel
                    {
                        SupplyFamilyId = item.Supply.SupplyFamilyId,
                        SupplyFamily = new SupplyFamilyViewModel
                        {
                            Code = item.Supply.SupplyFamily.Code,
                            Name = item.Supply.SupplyFamily.Name
                        },
                        SupplyGroupId = item.Supply.SupplyGroupId,
                        SupplyGroup = new SupplyGroupViewModel
                        {
                            Code = item.Supply.SupplyGroup.Code,
                            Name = item.Supply.SupplyGroup.Name
                        },
                        CorrelativeCode = item.Supply.CorrelativeCode,
                        Description = item.Supply.Description,
                        MeasurementUnit = new MeasurementUnitViewModel
                        {
                            Abbreviation = item.Supply.MeasurementUnit.Abbreviation
                        },
                    },
                    Measure = item.Measure.ToString("N2", CultureInfo.InvariantCulture),
                    MeasureFloat = item.Measure,
                    MinimumMeasure = item.MinimumMeasure.ToString("N2", CultureInfo.InvariantCulture),
                    UnitPrice = item.UnitPrice.ToString("N2", CultureInfo.InvariantCulture),
                    Parcial = item.Parcial,
                    ParcialString = item.Parcial.ToString("N2", CultureInfo.InvariantCulture),
                    Income = income.ToString("N2", CultureInfo.InvariantCulture),
                    IncomeFloat = income,
                    Dispatch = aux_dispatch.ToString("N2", CultureInfo.InvariantCulture),
                    DispatchFloat = aux_dispatch,
                    totalEntries = totalEntries,
                    totalDispatch = totalDispatch
                });
                /*
                if (providerId != null)
                {
                    if (prs.FirstOrDefault(x => x.OrderItem.Order.ProviderId == providerId) != null)
                        data.Add(item);
                    else
                        continue;
                }
                else
                    data.Add(item);
                */
            }

            return Ok(stocks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var stock = await _context.Stocks
                .Include(x => x.Supply)
                .Include(x => x.Supply.MeasurementUnit)
                .Where(x => x.Id == id)
                .Select(x => new StockViewModel
                {
                    Id = x.Id,
                    Supply = new SupplyViewModel
                    {
                        Description = x.Supply.Description,
                        MeasurementUnit = new MeasurementUnitViewModel
                        {
                            Abbreviation = x.Supply.MeasurementUnit.Abbreviation
                        }
                    },
                    MinimumMeasure = x.MinimumMeasure.ToString(CultureInfo.InvariantCulture)
                }).FirstOrDefaultAsync();

            return Ok(stock);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(StockViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stock = new Stock
            {
            };

            await _context.Stocks.AddAsync(stock);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, StockViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stock = await _context.Stocks
                .FirstOrDefaultAsync(x => x.Id == id);

            stock.MinimumMeasure = model.MinimumMeasure.ToDoubleString();

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var stock = await _context.Stocks
                .FirstOrDefaultAsync(x => x.Id == id);

            _context.Stocks.Remove(stock);

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("ordenes/listar")]
        public async Task<IActionResult> GetDetail(Guid id)
        {
            if (id == null)
                return BadRequest();

            var items = await _context.OrderItems
                .Include(x => x.Order)
                .Include(x => x.Supply)
                .Include(x => x.Supply.MeasurementUnit)
                .Include(x => x.Order.Provider)
                .Include(x => x.Order.Warehouse.WarehouseType.Project)
                .Where(x => x.SupplyId == id && x.Order.Status == ConstantHelpers.Logistics.RequestOrder.Status.APPROVED)
                .Select(x => new OrderItemViewModel
                {
                    Order = new OrderViewModel
                    {
                        CorrelativeCode = x.Order.CorrelativeCode,
                        CorrelativeCodeStr = x.Order.Warehouse.WarehouseType.Project.CostCenter.ToString() 
                            + "-" + x.Order.CorrelativeCode.ToString("D4") + "-" + x.Order.ReviewDate.Value.Year.ToString(),
                        Provider = new ProviderViewModel
                        {
                            BusinessName = x.Order.Provider.BusinessName
                        },
                        Currency = x.Order.Currency,
                        ExchangeRate = x.Order.ExchangeRate.ToString(CultureInfo.InvariantCulture)
                    },
                    Supply = new SupplyViewModel
                    {
                        Description = x.Supply.Description,
                        MeasurementUnit = new MeasurementUnitViewModel
                        {
                            Abbreviation = x.Supply.MeasurementUnit.Abbreviation
                        }
                    },
                    UnitPrice = x.Order.Currency == 1 ? x.UnitPrice.ToString("N2", CultureInfo.InvariantCulture) 
                    : (x.UnitPrice * x.Order.ExchangeRate).ToString("N2", CultureInfo.InvariantCulture),
                    DolarUnitPrice = x.Order.Currency == 1 ? (x.UnitPrice / x.Order.ExchangeRate).ToString("N2", CultureInfo.InvariantCulture)
                    : x.UnitPrice.ToString("N2", CultureInfo.InvariantCulture),
                    Measure = x.Measure.ToString(CultureInfo.InvariantCulture),
                    Parcial = x.Order.Currency == 1 ? x.Parcial.ToString("N2", CultureInfo.InvariantCulture)
                    : (x.Parcial * x.Order.ExchangeRate).ToString("N2", CultureInfo.InvariantCulture),
                    DolarParcial = x.Order.Currency == 1 ? (x.Parcial / x.Order.ExchangeRate).ToString("N2", CultureInfo.InvariantCulture)
                    : x.Parcial.ToString("N2", CultureInfo.InvariantCulture),
                    MeasureInAttentionString = x.MeasureInAttention.ToString(CultureInfo.InvariantCulture)
                })
                .ToListAsync();

            return Ok(items);
        }
        /*
        //Importar Excel Inicial
        [HttpPost("importar")]
        public async Task<IActionResult> Import(IFormFile file)
        {
            try
            {
                using (var mem = new MemoryStream())
                {
                    await file.CopyToAsync(mem);
                    using (var workBook = new XLWorkbook(mem))
                    {
                        var workSheet = workBook.Worksheets.First();
                        int counter = 2;
                        var stocksInDb = await _context.Stocks.ToListAsync();
                        var stocks = new List<Stock>();
                        while (!workSheet.Cell($"A{counter}").IsEmpty())
                        {
                            var stock = new Stock
                            {
                                Code = workSheet.Cell($"A{counter}").GetString(),
                                Description = workSheet.Cell($"B{counter}").GetString(),
                                Unit = workSheet.Cell($"C{counter}").GetString(),
                                //Quantity = int.TryParse(workSheet.Cell($"D{counter}").GetString(), out int quantity) ? 0 : quantity,
                                Quantity = workSheet.Cell($"D{counter}").TryGetValue<int>(out int quantity) ? quantity : 0,
                                //QuantityMinimum = int.TryParse(workSheet.Cell($"E{counter}").GetString(), out int quantityMinimum) ? 0 : quantityMinimum,
                                QuantityMinimum = workSheet.Cell($"E{counter}").TryGetValue<int>(out int quantityMinimum) ? quantityMinimum : 0,
                                //SalePriceUnit = decimal.TryParse(workSheet.Cell($"F{counter}").GetString(), out decimal salePriceUnit) ? 0.00M : salePriceUnit,
                                SalePriceUnit = workSheet.Cell($"F{counter}").TryGetValue<decimal>(out decimal salePriceUnit) ? salePriceUnit : 0.00M,
                                CurrencyType = 1
                            };

                            if (stocksInDb.FirstOrDefault(x => x.Code == stock.Code) == null &&
                                stocks.FirstOrDefault(x => x.Code == stock.Code) == null)
                            {
                                stocks.Add(stock);
                            }
                            counter++;
                        }
                        await _context.Stocks.AddRangeAsync(stocks);
                        await _context.SaveChangesAsync();
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        */

        [HttpGet("metrado")]
        public async Task<IActionResult> GetMeasure(Guid? providerId = null, Guid? supplyGroupId = null)
        {
            var query = _context.Stocks
              .Include(x => x.Supply)
              .Where(x => x.ProjectId == GetProjectId());

            if (supplyGroupId.HasValue)
                query = query.Where(x => x.Supply.SupplyGroupId == supplyGroupId);

            var entries = await _context.SupplyEntryItems
                .Include(x => x.SupplyEntry)
                .Include(x => x.OrderItem.Supply)
                .Include(x => x.SupplyEntry.Order.Provider)
                .Where(x => x.SupplyEntry.Status == ConstantHelpers.Warehouse.SupplyEntry.Status.CONFIRMED)
                .ToListAsync();

            var result = 0.0;

            foreach (var item in query)
            {
                var prs = entries
                    .Where(x => x.OrderItem.Supply.SupplyGroupId == item.Supply.SupplyGroupId)
                    .ToList();

                if (providerId != null)
                {
                    if (prs.FirstOrDefault(x => x.OrderItem.Order.ProviderId == providerId) != null)
                        result += item.Measure;
                    else
                        continue;
                }
                else
                    result += item.Measure;
            }

            return Ok(result.ToString("N2", CultureInfo.InvariantCulture));
        }
        /*
        [HttpGet("parcial")]
        public async Task<IActionResult> GetParcial(Guid? providerId = null, Guid? supplyGroupId = null)
        {
            var query = _context.Stocks
                .Include(x => x.Supply)
                .Where(x => x.ProjectId == GetProjectId());

            if (supplyGroupId.HasValue)
                query = query.Where(x => x.Supply.SupplyGroupId == supplyGroupId);

            var entries = await _context.SupplyEntryItems
                .Include(x => x.SupplyEntry)
                .Include(x => x.OrderItem.Supply)
                .Include(x => x.SupplyEntry.Order.Provider)
                .Where(x => x.SupplyEntry.Status == ConstantHelpers.Warehouse.SupplyEntry.Status.CONFIRMED)
                .ToListAsync();

            var result = 0.0;

            foreach (var item in query)
            {
                var prs = entries
                    .Where(x => x.OrderItem.Supply.SupplyGroupId == item.Supply.SupplyGroupId)
                    .ToList();

                if (providerId != null)
                {
                    if (prs.FirstOrDefault(x => x.OrderItem.Order.ProviderId == providerId) != null)
                        result += item.Parcial;
                    else
                        continue;
                }
                else
                    result += item.Parcial;
            }

            return Ok(result.ToString("N2", CultureInfo.InvariantCulture));
        }
        */
        [HttpPut("actualizar")]
        public async Task<IActionResult> Update()
        {
            var entryitems = await _context.SupplyEntryItems
                .Include(x => x.OrderItem)
                .Include(x => x.SupplyEntry)
                .Include(x => x.SupplyEntry.Order)
                .Include(x => x.OrderItem.Order)
                .Where(x => x.SupplyEntry.Status == ConstantHelpers.Warehouse.SupplyEntry.Status.CONFIRMED && x.OrderItem.Order.ProviderId == GetProjectId())
                .ToListAsync();

            var stocks = await _context.Stocks
                .Where(x => x.ProjectId == GetProjectId())
                .ToListAsync();

            foreach(var stock in stocks)
            {
                var items = entryitems.Where(x => x.OrderItem.SupplyId == stock.SupplyId).ToList();
                stock.Measure = 0.0;
                var sumaProducto = 0.0;
                var dividendo = 0.0;
                var divisor = 0.0;

                foreach (var item in items)
                {
                    stock.Measure += item.Measure;
                    if (item.SupplyEntry.Order.Currency == ConstantHelpers.Currency.NUEVOS_SOLES)
                    {
                        dividendo += (item.Measure * item.OrderItem.UnitPrice);
                        divisor += item.Measure;
                    }
                    else
                    {
                        dividendo += (item.Measure * item.OrderItem.UnitPrice * item.SupplyEntry.Order.ExchangeRate);
                        divisor += item.Measure;
                    }
                }

                sumaProducto = Math.Round(dividendo / divisor, 2);

                stock.UnitPrice = sumaProducto;
                stock.Parcial = Math.Round(sumaProducto * stock.Measure, 2);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
