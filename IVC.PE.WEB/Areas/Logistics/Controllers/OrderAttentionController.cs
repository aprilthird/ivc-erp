using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.WEB.Areas.Logistics.ViewModels.OrderViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.ProviderViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyViewModels;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Logistics.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Logistics.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.LOGISTICS)]
    [Route("logistica/atencion-ordenes")]
    public class OrderAttentionController : BaseController
    {

        public OrderAttentionController(IvcDbContext context,
          UserManager<ApplicationUser> userManager,
          IConfiguration configuration) : base(context, userManager, configuration)
        {

        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(int type = 0, Guid? providerId = null, int attentionStatus = 0)
        {
            var result = new List<OrderAttentionViewModel>();

            var query = _context.Orders
                .Include(x => x.Provider)
                .Include(x => x.Warehouse)
                .Include(x => x.Warehouse.WarehouseType.Project)
                .Where(x => x.Status == ConstantHelpers.Logistics.RequestOrder.Status.APPROVED
                && x.ProjectId == GetProjectId());

            if(providerId.HasValue)
                query = query.Where(x=>x.ProviderId == providerId);

            var ordItems = await _context.OrderItems
                .ToListAsync();

            foreach(var item in query.ToList())
            {
                if (type != 0 && item.Type != type)
                {
                    continue;
                }

                if (attentionStatus != 0 && item.AttentionStatus != attentionStatus)
                {
                    continue;
                }

                var aux = ordItems.Where(x => x.OrderId == item.Id).ToList();

                var soles = 0.0;
                var dolares = 0.0;

                var solesInAttention = 0.0;
                var dolaresInAttention = 0.0;

                if (item.Currency == 1)
                {
                    soles = Math.Round(item.Parcial, 2);
                    solesInAttention = Math.Round(aux.Sum(x => x.MeasureInAttention * x.UnitPrice), 2);
                    if (item.ExchangeRate > 0)
                    {
                        dolares = Math.Round(item.Parcial / item.ExchangeRate, 2);
                        dolaresInAttention = Math.Round(solesInAttention / item.ExchangeRate, 2);
                    }
                }
                else
                {
                    soles = Math.Round(item.Parcial * item.ExchangeRate, 2);
                    dolares = Math.Round(item.Parcial, 2);
                    solesInAttention = Math.Round(aux.Sum(x => x.MeasureInAttention * x.UnitPrice) * item.ExchangeRate, 2);
                    dolaresInAttention = Math.Round(aux.Sum(x => x.MeasureInAttention * x.UnitPrice), 2);
                }

                var attention = Math.Round(solesInAttention / soles * 100, 2);

                result.Add(new OrderAttentionViewModel
                {
                    Id = item.Id,
                    Project = item.Warehouse.WarehouseType.Project.Abbreviation,
                    CorrelativeCodeStr = item.Warehouse.WarehouseType.Project.CostCenter + "-" + item.CorrelativeCode.ToString("D4") + item.CorrelativeCodeSuffix + "-" + item.ReviewDate.Value.Year.ToString(),
                    Provider = new ProviderViewModel
                    {
                        RUC = item.Provider.RUC,
                        BusinessName = item.Provider.BusinessName
                    },
                    Date = item.Date.ToDateString(),
                    ExchangeRate = item.ExchangeRate.ToString(CultureInfo.InvariantCulture),
                    Parcial = soles.ToString("N2", CultureInfo.InvariantCulture),
                    DolarParcial = dolares.ToString("N2", CultureInfo.InvariantCulture),
                    ParcialInAttention = solesInAttention.ToString("N2", CultureInfo.InvariantCulture),
                    DolarParcialInAttention = dolaresInAttention.ToString("N2", CultureInfo.InvariantCulture),
                    PercentageAttention = attention.ToString(CultureInfo.InvariantCulture) + "%",
                    Status = item.AttentionStatus,
                    ClosureReason = item.ClosureReason
                });

            }

            return Ok(result);
        }

        [HttpGet("detalles/listar")]
        public async Task<IActionResult> GetDetails(Guid id)
        {
            var result = new List<SupplyEntryInOrderViewModel>();

            var entries = await _context.SupplyEntries
                .Include(x => x.Order)
                .Where(x => x.OrderId == id)
                .ToListAsync();

            foreach(var item in entries)
            {
                var soles = 0.0;
                var dolares = 0.0;

                if (item.Order.Currency == 1)
                {
                    soles = item.Parcial;
                    if (item.Order.ExchangeRate > 0)
                    {
                        dolares = Math.Round(item.Parcial / item.Order.ExchangeRate, 2);
                    }
                }
                else
                {
                    soles = Math.Round(item.Parcial * item.Order.ExchangeRate, 2);
                    dolares = item.Parcial;
                }

                result.Add(new SupplyEntryInOrderViewModel
                {
                    Id = item.Id,
                    DeliveryDate = item.DeliveryDate.ToDateString(),
                    RemissionGuideName = item.RemissionGuide,
                    Parcial = soles.ToString("N2", CultureInfo.InvariantCulture),
                    DolarParcial = dolares.ToString("N2", CultureInfo.InvariantCulture),
                    RemissionGuideUrl = item.RemissionGuideUrl
                });
            }

            return Ok(result);
        }

        [HttpGet("items/listar")]
        public async Task<IActionResult> GetItems(Guid id)
        {
            var ingresos = await _context.SupplyEntryItems
                .Include(x => x.SupplyEntry)
                .Where(x => x.SupplyEntry.Status == ConstantHelpers.Warehouse.SupplyEntry.Status.CONFIRMED)
                .ToListAsync();

            var res = new List<OrderItemViewModel>();

            var items = await _context.OrderItems
                .Include(x => x.Supply)
                .Include(x => x.Supply.SupplyGroup)
                .Include(x => x.Supply.SupplyFamily)
                .Include(x => x.Supply.MeasurementUnit)
                .Where(x => x.OrderId == id)
                .ToListAsync();

            foreach(var item in items)
            {
                res.Add(new OrderItemViewModel
                {
                    Supply = new SupplyViewModel
                    {
                        Description = item.Supply.Description,
                        CorrelativeCode = item.Supply.CorrelativeCode,
                        SupplyGroup = new ViewModels.SupplyGroupViewModels.SupplyGroupViewModel
                        {
                            Code = item.Supply.SupplyGroup.Code
                        },
                        SupplyFamily = new ViewModels.SupplyFamilyViewModels.SupplyFamilyViewModel
                        {
                            Code = item.Supply.SupplyFamily.Code
                        },
                        MeasurementUnit = new ViewModels.MeasurementUnitViewModels.MeasurementUnitViewModel
                        {
                            Abbreviation = item.Supply.MeasurementUnit.Abbreviation
                        }
                    },
                    Measure = item.Measure.ToString(),
                    MeasureInAttention = item.MeasureInAttention,
                    MeasureToAttent = item.Measure - ingresos.Where(x => x.OrderItemId == item.Id).Sum(x => x.Measure),
                    SupplyEntryCode = string.Join("-",
                        ingresos.Where(x => x.OrderItemId == item.Id)
                        .Select(x => x.SupplyEntry.RemissionGuide)
                        .Distinct()
                        .ToList())
                });
            }

            return Ok(res);
        }

        [HttpPut("cerrar-orden/{id}")]
        public async Task<IActionResult> Closure(Guid id, string reason)
        {
            var orden = await _context.Orders.FirstOrDefaultAsync(x => x.Id == id);

            if (orden == null)
                return BadRequest("No se ha encontrado la orden");

            if (orden.AttentionStatus == ConstantHelpers.Logistics.RequestOrder.AttentionStatus.TOTAL)
                return BadRequest("La orden ya se encuentra atentida en su totalidad");

            if (orden.AttentionStatus == ConstantHelpers.Logistics.RequestOrder.AttentionStatus.PENDING)
                return BadRequest("La orden no se ha iniciado");

            orden.AttentionStatus = ConstantHelpers.Logistics.RequestOrder.AttentionStatus.TOTAL;
            orden.ClosureReason = reason;

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
