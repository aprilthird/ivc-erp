using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.WEB.Areas.Logistics.ViewModels.PackageViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.ProviderViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyFamilyViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyGroupViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IVC.PE.WEB.Areas.Logistics.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Logistics.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.LOGISTICS)]
    [Route("logistica/paqueteo")]
    public class PackageController : BaseController
    {
        public PackageController(IvcDbContext context, 
            ILogger<PackageController> logger) 
            : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid supplyFamilyId, Guid? supplyGroupId = null)
        {
            if (supplyFamilyId == Guid.Empty)
                return Ok(new List<PackageViewModel>());

            var pId = GetProjectId();

            #region Entidades
            var query = await _context.SupplyGroups
                .Include(x => x.SupplyFamily)
                .Where(x => x.SupplyFamilyId == supplyFamilyId &&
                (supplyGroupId.HasValue ? x.Id == supplyGroupId : true))
                .AsNoTracking()
                .ToListAsync();

            var budgetInputs = await _context.BudgetInputs
                .Where(x => x.ProjectId == pId &&
                x.SupplyFamilyId == supplyFamilyId &&
                (supplyGroupId.HasValue ? x.SupplyGroupId == supplyGroupId : true))
                .AsNoTracking()
                .ToListAsync();

            var orderItems = await _context.OrderItems
                .Include(x => x.Order)
                .Include(x => x.Supply)
                .Where(x => x.Order.ProjectId == pId &&
                x.Order.Status == ConstantHelpers.Logistics.RequestOrder.Status.APPROVED &&
                x.Supply.SupplyFamilyId == supplyFamilyId &&
                (supplyGroupId.HasValue ? x.Supply.SupplyGroupId == supplyGroupId : true))
                .AsNoTracking()
                .ToListAsync();

            var supplyEntryItems = await _context.SupplyEntryItems
                .Include(x => x.SupplyEntry)
                .Include(x => x.OrderItem.Order)
                .Include(x => x.OrderItem.Supply)
                .Include(x => x.SupplyEntry.Warehouse.WarehouseType)
                .Where(x => x.SupplyEntry.Warehouse.WarehouseType.ProjectId == pId &&
                 x.SupplyEntry.Status == ConstantHelpers.Warehouse.SupplyEntry.Status.CONFIRMED &&
                 x.OrderItem.Supply.SupplyFamilyId == supplyFamilyId &&
                (supplyGroupId.HasValue ? x.OrderItem.Supply.SupplyGroupId == supplyGroupId : true))
                .AsNoTracking()
                .ToListAsync();

            var fieldRequests = await _context.FieldRequestFoldings
                .Include(x => x.ProjectPhase)
                .Include(x => x.GoalBudgetInput.Supply)
                .Include(x => x.FieldRequest)
                .Where(x => x.ProjectPhase.ProjectId == pId 
                && x.GoalBudgetInput.Supply.SupplyFamilyId == supplyFamilyId &&
                (supplyGroupId.HasValue ? x.GoalBudgetInput.Supply.SupplyGroupId == supplyGroupId : true))
                .AsNoTracking().ToListAsync();

            var stocks = await _context.Stocks
                .Where(x => x.ProjectId == pId).ToListAsync();
            #endregion

            var data = new List<PackageViewModel>();

            var primero = query.FirstOrDefault();

            #region Familia
            data.Add(new PackageViewModel
            {
                Id = (Guid)primero.SupplyFamilyId,
                Item = primero.SupplyFamily.Code,
                Insumos = primero.SupplyFamily.Name,
                IsFamily = true,
                IsGroup = false
            });
            #endregion

            #region Inicializaciones
            var grupos = new List<PackageViewModel>();

            var totalBudgetInput = 0.0;
            var totalPurchase = 0.0;
            var totalSupplyEntry = 0.0;
            var totalOutput = 0.0;
            var totalUsed = 0.0;

            var saleSupplySum = 0.0;
            var purchaseSum = 0.0;
            var addedSupplySum = 0.0;
            var outputSupplySum = 0.0;
            var usedSupplySum = 0.0;
            #endregion

            foreach (var item in query)
            {
                saleSupplySum = budgetInputs.Where(x => x.SupplyGroupId == item.Id).Sum(x => x.Parcial);

                purchaseSum = orderItems.Where(x => x.Supply.SupplyGroupId == item.Id && 
                x.Order.Status == ConstantHelpers.Logistics.RequestOrder.Status.APPROVED)
                    .Sum(x => x.Order.Currency == ConstantHelpers.Currency.NUEVOS_SOLES ?
                    x.Parcial : x.Parcial * x.Order.ExchangeRate);

                addedSupplySum = supplyEntryItems.Where(x => x.OrderItem.Supply.SupplyGroupId == item.Id &&
                x.SupplyEntry.Status == ConstantHelpers.Warehouse.SupplyEntry.Status.CONFIRMED)
                    .Sum(x => x.OrderItem.Order.Currency == ConstantHelpers.Currency.NUEVOS_SOLES ?
                    x.Measure * x.OrderItem.UnitPrice : x.Measure * x.OrderItem.UnitPrice * x.OrderItem.Order.ExchangeRate);

                outputSupplySum = fieldRequests.Where(x => x.GoalBudgetInput.Supply.SupplyGroupId == item.Id &&
                x.FieldRequest.Status == ConstantHelpers.Warehouse.FieldRequest.Status.ATTENDED)
                    .Sum(x => x.ValidatedQuantity * stocks.FirstOrDefault(y => y.SupplyId == x.GoalBudgetInput.SupplyId).UnitPrice);

                grupos.Add(new PackageViewModel
                {
                    Id = item.Id,
                    Item = item.Code,
                    Insumos = item.Name,
                    SaleSupply = saleSupplySum.ToString("N2", CultureInfo.InvariantCulture),
                    GeneratedOrderSupply = purchaseSum.ToString("N2", CultureInfo.InvariantCulture),
                    AddedSupply = addedSupplySum.ToString("N2", CultureInfo.InvariantCulture),
                    VirtualOutputSupply = outputSupplySum.ToString("N2", CultureInfo.InvariantCulture),
                    UsedSupply = usedSupplySum.ToString("N2", CultureInfo.InvariantCulture),
                });

                totalBudgetInput += saleSupplySum;
                totalPurchase += purchaseSum;
                totalSupplyEntry += addedSupplySum;
                totalOutput += outputSupplySum;
                totalUsed += usedSupplySum;
            }

            #region Grupo
            data.Add(new PackageViewModel
            {
                Item = "A",
                Insumos = "GRUPOS",
                IsFamily = false,
                IsGroup = true,
                SaleSupply = totalBudgetInput.ToString("N2", CultureInfo.InvariantCulture),
                GeneratedOrderSupply = totalPurchase.ToString("N2", CultureInfo.InvariantCulture),
                AddedSupply = totalSupplyEntry.ToString("N2", CultureInfo.InvariantCulture),
                VirtualOutputSupply = "0.00",
                UsedSupply = "0.00",
                PaidSupply = "0.00",
                ValuedSupply = "0.00"
            });
            #endregion

            data.AddRange(grupos);

            return Ok(data);
        }

        [HttpGet("detalles/ordenes-generadas")]
        public async Task<IActionResult> GetOrder(Guid supplyGroupId)
        {
            if (supplyGroupId == Guid.Empty)
                return Ok(new List<PackageDetailsViewModel>());

            var pId = GetProjectId();
            var query = _context.Orders
                .Include(x => x.Provider)
                .Include(x => x.Project)
                .Include(x => x.OrderItems)
                .ThenInclude(x => x.Supply)
               .Where(x => x.ProjectId == pId &&
               x.OrderItems.FirstOrDefault(x => x.Supply.SupplyGroupId == supplyGroupId) != null &&
               x.Status == ConstantHelpers.Logistics.RequestOrder.Status.APPROVED)
               .ToList();
            
            var data = new List<PackageDetailsViewModel>();
            
            foreach(var aux in query)
            {
                var parcial = _context.OrderItems
                    .Include(x => x.Supply)
                     .Where(x => x.OrderId == aux.Id &&
                     x.Supply.SupplyGroupId == supplyGroupId)
                     .Sum(y => (y.Order.Currency == ConstantHelpers.Currency.NUEVOS_SOLES) ? y.Parcial : (y.Parcial * y.Order.ExchangeRate));
                data.Add(new PackageDetailsViewModel
                {
                    ProviderId = aux.ProviderId,
                    Provider = new ProviderViewModel
                    {
                        BusinessName = aux.Provider.BusinessName
                    },
                    Type = ConstantHelpers.Logistics.RequestOrder.Type.VALUES[aux.Type],
                    Parcial = parcial.ToString("N2"),
                    Number = aux.Project.CostCenter.ToString() + "-"
                    + aux.CorrelativeCode.ToString("D4") + aux.CorrelativeCodeSuffix
                });
            }

            return Ok(data);
        }

        
        [HttpGet("detalles/insumos-ingresados")]
        public async Task<IActionResult> GetAddedSupply(Guid supplyGroupId)
        {
            if (supplyGroupId == Guid.Empty)
                return Ok(new List<PackageDetailsViewModel>());

            var pId = GetProjectId();

            var orderItems = _context.SupplyEntryItems
                .Include(x => x.SupplyEntry)
                .Include(x => x.SupplyEntry.Warehouse.WarehouseType)
                .Include(x => x.OrderItem.Order)
                .Include(x => x.OrderItem.Supply)
                .Include(x => x.OrderItem.Order.Provider)
                .Include(x => x.OrderItem.Order.Project)
                .Where(x => x.SupplyEntry.Warehouse.WarehouseType.ProjectId == pId &&
                 x.SupplyEntry.Status == ConstantHelpers.Warehouse.SupplyEntry.Status.CONFIRMED &&
                 x.OrderItem.Supply.SupplyGroupId == supplyGroupId)
                .Select(x => x.OrderItem);

            var query = _context.Orders
                .Include(x => x.Provider)
                .Include(x => x.Project)
                .Where(x => x.ProjectId == pId &&
            orderItems.Select(y => y.OrderId).Contains(x.Id)).ToList();

            var data = new List<PackageDetailsViewModel>();
            foreach (var aux in query)
            {
                var parcial = _context.OrderItems
                    .Include(x => x.Order)
                    .Include(x => x.Supply)
                    .Where(x => x.OrderId == aux.Id && x.Supply.SupplyGroupId == supplyGroupId)
                    .Sum(x => aux.Currency == ConstantHelpers.Currency.NUEVOS_SOLES ?
                    x.UnitPrice * x.MeasureInAttention : x.UnitPrice * x.MeasureInAttention * x.Order.ExchangeRate);
                data.Add(new PackageDetailsViewModel
                {
                    ProviderId = aux.ProviderId,
                    Provider = new ProviderViewModel
                    {
                        BusinessName = aux.Provider.BusinessName
                    },
                    Type = ConstantHelpers.Logistics.RequestOrder.Type.VALUES[aux.Type],
                    Parcial = parcial.ToString("N2"),
                    Number = aux.Project.CostCenter.ToString() + "-"
                    + aux.CorrelativeCode.ToString("D4") + aux.CorrelativeCodeSuffix
                });
            }
            return Ok(data);
        }

        [HttpGet("detalles/insumos-virtual")]
        public async Task<IActionResult> GetVirtualSupply(Guid supplyGroupId)
        {
            if (supplyGroupId == Guid.Empty)
                return Ok(new List<PackageDetailsViewModel>());

            var pId = GetProjectId();
            var query = _context.BudgetInputs
               .Where(x => x.ProjectId == pId &&
                x.SupplyGroupId == supplyGroupId)
               .AsNoTracking().AsQueryable();

            var data = new List<PackageDetailsViewModel>();

            foreach (var aux in query)
            {
                data.Add(new PackageDetailsViewModel
                {

                });
            }
            return Ok(new List<PackageDetailsViewModel>());
        }

        [HttpGet("detalles/insumos-consumidos")]
        public async Task<IActionResult> GetUsedSupply(Guid supplyGroupId)
        {
            if (supplyGroupId == Guid.Empty)
                return Ok(new List<PackageDetailsViewModel>());

            var pId = GetProjectId();

            var auxFields = _context.FieldRequestFoldings
                .Include(x => x.FieldRequest.WorkFront)
                .Include(x => x.FieldRequest)
                .Include(x => x.GoalBudgetInput)
                .Where(x => x.FieldRequest.WorkFront.ProjectId == pId && 
                x.FieldRequest.Status == ConstantHelpers.Warehouse.FieldRequest.Status.ATTENDED);

            var query = _context.Supplies
                .Include(x => x.SupplyFamily)
                .Include(x => x.SupplyGroup)
                .Where(x => x.SupplyGroupId == supplyGroupId && 
                auxFields.Select(x => x.GoalBudgetInput).Select(x => x.SupplyId).Contains(x.Id))
                .ToList();

            var stocks = _context.Stocks
                .Where(x => x.ProjectId == pId)
                .ToList();

            var data = new List<PackageDetailsViewModel>();

            foreach (var aux in query)
            {
                data.Add(new PackageDetailsViewModel
                {
                    SupplyId = aux.Id,
                    Supply = new SupplyViewModel
                    {
                        Description = aux.Description,
                        CorrelativeCode = aux.CorrelativeCode,
                        SupplyFamily = new SupplyFamilyViewModel
                        {
                            Code = aux.SupplyFamily.Code
                        },
                        SupplyGroup = new SupplyGroupViewModel
                        {
                            Code = aux.SupplyGroup.Code
                        }
                    },
                    Parcial = (stocks.FirstOrDefault(x => x.SupplyId == aux.Id).UnitPrice * 
                    auxFields.Where(x => x.GoalBudgetInput.SupplyId == aux.Id).Sum(x => x.ValidatedQuantity))
                    .ToString("N2")
                });
            }
            return Ok(new List<PackageDetailsViewModel>());
        }

        
        /*
        [HttpGet("ordenes-de-compra")]
        public async Task<IActionResult> Purchase()
        {
            var families = await _context.SupplyGroups
                .Select(x => new PackageGroupViewModel
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name
                }).ToListAsync();
            foreach(var family in families)
            {
                if(family.Code.Contains("17"))
                {
                    family.SaleAmmount = 7908156.59;
                    family.GoalAmmount = 7908156.59;
                }
            }
            return View(families);
        }

        [HttpGet("ordenes-de-compra/{familyId}/ordenes")]
        public async Task<IActionResult> GetOrders(Guid familyId)
        {
            var data = await _context.Orders
                    //.Where(x => x.Request.ProjectId == GetProjectId())
                    //.Where(x => x.Request.SupplyFamilyId == familyId)
                    .Select(x => new PackageOrderViewModel
                    {
                        Code = x.CorrelativeCode.ToString("D4"),
                        ProviderName = x.Provider.BusinessName,
                        Type = x.Type,
                        //Ammount = x.OrderItems.Sum(i => i.UnitPrice * i.Measure)
                    }).ToListAsync();
            return Ok(data);
        }

        [HttpGet("ordenes-de-servicio")]
        public IActionResult Service() => View();
        */
    }
}