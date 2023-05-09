using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using ClosedXML.Excel;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.Logistics;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.BankViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.LogisticResponsibleViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.OrderViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.ProviderViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.RequestViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyFamilyViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyGroupViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.GoalBudgetInputViewModels;
using IVC.PE.WEB.Areas.Warehouse.ViewModels.WarehouseViewModels;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Options;
using IVC.PE.WEB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IVC.PE.WEB.Areas.Logistics.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Logistics.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.LOGISTICS)]
    [Route("logistica/ordenes")]
    public class OrderController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public OrderController(IvcDbContext context,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            IOptions<CloudStorageCredentials> storageCredentials,
            ILogger<OrderController> logger)
            : base(context, userManager, configuration, logger)
        {
            _storageCredentials = storageCredentials;
        }

        [HttpGet("compra/listado")]
        public IActionResult PurchaseList() => View();

        [HttpGet("compra/listado/listar")]
        public async Task<IActionResult> GetAllListado(Guid? providerId = null,
            Guid? supplyFamilyId = null, Guid? supplyGroupId = null)
        {
            var search = Request.Query[ConstantHelpers.Datatable.ServerSide.SentParameters.SEARCH_VALUE].ToString();
            var currentNumber = Convert.ToInt32(Request.Query[ConstantHelpers.Datatable.ServerSide.SentParameters.PAGING_FIRST_RECORD]);
            var recordsPerPage = Convert.ToInt32(Request.Query[ConstantHelpers.Datatable.ServerSide.SentParameters.RECORDS_PER_DRAW]);
            var orderColumn = Convert.ToInt32(Request.Query[ConstantHelpers.Datatable.ServerSide.SentParameters.ORDER_COLUMN]);
            var orderDirection = Convert.ToString(Request.Query[ConstantHelpers.Datatable.ServerSide.SentParameters.ORDER_DIRECTION]);

            var query = _context.Orders
                .Include(x => x.Provider)
                .Include(x => x.Warehouse)
                .Include(x => x.OrderItems)
                .ThenInclude(x => x.Supply)
                .Include(x => x.Requests)
                .ThenInclude(x => x.Request)
                .ThenInclude(x => x.BudgetTitle)
                .Include(x => x.Project)
                .Where(x => x.Type == ConstantHelpers.Logistics.RequestOrder.Type.PURCHASE
                && x.Status != ConstantHelpers.Logistics.RequestOrder.Status.PRE_ISSUED && x.Status != ConstantHelpers.Logistics.RequestOrder.Status.OBSERVED
                && x.ProjectId == GetProjectId())
                .AsNoTracking()
                .AsQueryable();

            var usuarios = await _context.Users
                .Include(x => x.WorkAreaEntity)
                .ToListAsync();

            var userRoles = await _context.UserRoles
                .Include(x => x.Role)
                .ToListAsync();

            var totalRecords = query.Count();

            if (providerId.HasValue)
                query = query.Where(x => x.ProviderId == providerId);

            if (supplyFamilyId.HasValue)
                query = query.Where(x => x.OrderItems
                .Where(y => y.Supply.SupplyFamilyId == supplyFamilyId)
                .Any() == true);

            if (supplyGroupId.HasValue)
                query = query.Where(x => x.OrderItems
                .Where(y => y.Supply.SupplyGroupId == supplyGroupId)
                .Any() == true);

            if (orderColumn == 3)
            {
                if (orderDirection == "asc")
                    query = query.OrderBy(x => x.Status);
                else 
                    query = query.OrderByDescending(x => x.Status);
            }
            else if (orderColumn == 4)
            {
                if (orderDirection == "asc")
                    query = query.OrderBy(x => x.CorrelativeCode);
                else
                    query = query.OrderByDescending(x => x.CorrelativeCode);
            }
            else if (orderColumn == 7)
            {
                if (orderDirection == "asc")
                    query = query.OrderBy(x => x.Date);
                else
                    query = query.OrderByDescending(x => x.Date);
            }
            else if (orderColumn == 13)
            {
                if (orderDirection == "asc")
                    query = query.OrderBy(x => x.PriceFileUrl);
                else
                    query = query.OrderByDescending(x => x.PriceFileUrl);
            }
            else if (orderColumn == 14)
            {
                if (orderDirection == "asc")
                    query = query.OrderBy(x => x.SupportFileUrl);
                else
                    query = query.OrderByDescending(x => x.SupportFileUrl);
            }

            var aux = query
                .ToList();

            if (!string.IsNullOrEmpty(search))
            {
                if (!search.Any(y => Char.IsLetter(y)))
                    aux = aux.Where(x => search == x.CorrelativeCode.ToString()).ToList();
                else if(!search.Any(y=> Char.IsNumber(y)))
                    aux = aux.Where(x => search == x.CorrelativeCodeSuffix).ToList();
                else
                    aux = aux.Where(x => new String(search.Where(Char.IsNumber).ToArray()) == x.CorrelativeCode.ToString()
                    && new String(search.Where(Char.IsLetter).ToArray()) == x.CorrelativeCodeSuffix).ToList();
            }

            var count = aux.Count();

            aux = aux
                 .Skip(currentNumber)
                 .Take(recordsPerPage)
                 .ToList();

            var data = aux
                .Select(x => new OrderViewModel
                {
                    Id = x.Id,
                    CorrelativeCode = x.CorrelativeCode,
                    CorrelativeCodeStr = x.Project.CostCenter.ToString() + "-"
                    + x.CorrelativeCode.ToString("D4") + x.CorrelativeCodeSuffix + "-"
                    + x.ReviewDate.Value.Year.ToString(),
                    Provider = new ProviderViewModel
                    {
                        RUC = x.Provider.RUC,
                        BusinessName = x.Provider.BusinessName
                    },
                    RequestsName = string.Join(" / ", x.Requests
                    .Select(y => y.Request.CorrelativePrefix + "-"
                    + y.Request.CorrelativeCode.ToString("D4"))),
                    CostCenter = x.Project.CostCenter.ToString(),
                    Abbreviation = x.Project.Abbreviation.ToString(),
                    BudgetTitle = x.Requests.FirstOrDefault().Request.BudgetTitle.Name,
                    QuotationNumber = x.QuotationNumber,
                    Date = x.Date.ToDateString(),
                    DeliveryTime = x.DeliveryTime,
                    Warehouse = new WarehouseViewModel
                    {
                        Address = (x.Warehouse != null ? x.Warehouse.Address : x.ManualWarehouse)
                    },
                    Observations = x.Observations,
                    PriceFileUrl = x.PriceFileUrl,
                    PdfFileUrl = x.PdfFileUrl,
                    SupportFileUrl = x.SupportFileUrl,
                    Status = x.Status,
                    AttentionStatus = x.AttentionStatus,
                    PaymentMethod = x.PaymentMethod,
                    CurrencyStr = ConstantHelpers.Biddings.CURRENCY[x.Currency],
                    Parcial = (x.Currency == ConstantHelpers.Currency.NUEVOS_SOLES) ? x.Parcial.ToString("N2", CultureInfo.InvariantCulture)
                    : ((x.ExchangeRate > 0) ? Math.Round(x.Parcial * x.ExchangeRate, 2).ToString("N2", CultureInfo.InvariantCulture)
                    : 0.ToString("N2", CultureInfo.InvariantCulture)),
                    DolarParcial =(x.Currency == ConstantHelpers.Currency.NUEVOS_SOLES) ? 0.ToString("N2", CultureInfo.InvariantCulture)
                    : (ConstantHelpers.Currency.SIGN_VALUES[x.Currency] + " " + x.Parcial.ToString("N2", CultureInfo.InvariantCulture)),
                    IssuedUserId = x.IssuedUserId
                }).ToList();

            foreach (var order in data)
                if (usuarios.FirstOrDefault(x => x.Id == order.IssuedUserId).WorkAreaEntity.Name == "Logística"
                        && userRoles.FirstOrDefault(x => x.Role.Name == "Oficina Técnica" && x.UserId == order.IssuedUserId) != null)
                    order.CorrelativeCodeStr += "-OB";

            return Ok(new
            {
                draw = ConstantHelpers.Datatable.ServerSide.SentParameters.DRAW_COUNTER,
                recordsTotal = totalRecords,
                recordsFiltered = count,
                data
            });
        }


        [HttpGet("compra")]
        public IActionResult Purchase() => View();

        [HttpGet("compra/listar")]
        public async Task<IActionResult> GetAll()
        {
            var search = Request.Query[ConstantHelpers.Datatable.ServerSide.SentParameters.SEARCH_VALUE].ToString();
            var currentNumber = Convert.ToInt32(Request.Query[ConstantHelpers.Datatable.ServerSide.SentParameters.PAGING_FIRST_RECORD]);
            var recordsPerPage = Convert.ToInt32(Request.Query[ConstantHelpers.Datatable.ServerSide.SentParameters.RECORDS_PER_DRAW]);

            var pId = GetProjectId();

            var query = _context.Orders
                .Include(x => x.Provider)
                .Include(x => x.Warehouse)
                .Include(x => x.OrderItems)
                .ThenInclude(x => x.Supply)
                .Include(x => x.Requests)
                .ThenInclude(x => x.Request)
                .ThenInclude(x => x.BudgetTitle)
                .Include(x => x.Project)
                .Where(x => x.Type == ConstantHelpers.Logistics.RequestOrder.Type.PURCHASE
                && (x.Status == ConstantHelpers.Logistics.RequestOrder.Status.PRE_ISSUED || x.Status == ConstantHelpers.Logistics.RequestOrder.Status.OBSERVED)
                && x.ProjectId == pId)
                .AsNoTracking().AsQueryable();

            var usuarios = await _context.Users
                .Include(x => x.WorkAreaEntity)
                .ToListAsync();

            var userRoles = await _context.UserRoles
                .Include(x => x.Role)
                .ToListAsync();

            var totalRecords = query.Count();

            var aux = query
                .ToList();

            if (!string.IsNullOrEmpty(search))
            {
                if (!search.Any(y => Char.IsLetter(y)))
                    aux = aux.Where(x => search == x.CorrelativeCode.ToString()).ToList();
                else if (!search.Any(y => Char.IsNumber(y)))
                    aux = aux.Where(x => search == x.CorrelativeCodeSuffix).ToList();
                else
                    aux = aux.Where(x => new String(search.Where(Char.IsNumber).ToArray()) == x.CorrelativeCode.ToString()
                    && new String(search.Where(Char.IsLetter).ToArray()) == x.CorrelativeCodeSuffix).ToList();
            }

            var count = aux.Count();

            aux = aux
                 .Skip(currentNumber)
                 .Take(recordsPerPage)
                 .ToList();
            
            var data = aux
                .Select(x => new OrderViewModel
                {
                    Id = x.Id,
                    CorrelativeCode = x.CorrelativeCode,
                    CorrelativeCodeStr = x.Project.CostCenter.ToString() + "-"
                    + x.CorrelativeCode.ToString("D4") + x.CorrelativeCodeSuffix,
                    Provider = new ProviderViewModel
                    {
                        RUC = x.Provider.RUC,
                        BusinessName = x.Provider.BusinessName
                    },
                    RequestsName = string.Join(" / ", x.Requests
                    .Select(y => y.Request.CorrelativePrefix + "-"
                    + y.Request.CorrelativeCode.ToString("D4"))),
                    CostCenter = x.Project.CostCenter.ToString(),
                    Abbreviation = x.Project.Abbreviation.ToString(),
                    BudgetTitle = x.Requests.FirstOrDefault().Request.BudgetTitle.Name,
                    QuotationNumber = x.QuotationNumber,
                    Date = x.Date.ToDateString(),
                    DeliveryTime = x.DeliveryTime,
                    Warehouse = new WarehouseViewModel
                    {
                        Address = (x.Warehouse != null ? x.Warehouse.Address : x.ManualWarehouse)
                    },
                    Observations = x.Observations,
                    PriceFileUrl = x.PriceFileUrl,
                    PdfFileUrl = x.PdfFileUrl,
                    SupportFileUrl = x.SupportFileUrl,
                    Status = x.Status,
                    AttentionStatus = x.AttentionStatus,
                    PaymentMethod = x.PaymentMethod,
                    CurrencyStr = ConstantHelpers.Biddings.CURRENCY[x.Currency],
                    Parcial = (x.Currency == ConstantHelpers.Currency.NUEVOS_SOLES) ? x.Parcial.ToString("N2", CultureInfo.InvariantCulture)
                    : ((x.ExchangeRate > 0) ? Math.Round(x.Parcial * x.ExchangeRate, 2).ToString("N2", CultureInfo.InvariantCulture)
                    : 0.ToString("N2", CultureInfo.InvariantCulture)),
                    DolarParcial = (x.Currency == ConstantHelpers.Currency.NUEVOS_SOLES) ? 0.ToString("N2", CultureInfo.InvariantCulture)
                    : (ConstantHelpers.Currency.SIGN_VALUES[x.Currency] + " " + x.Parcial.ToString("N2", CultureInfo.InvariantCulture)),
                    IssuedUserId = x.IssuedUserId
                }).ToList();

            foreach (var order in data)
                if (usuarios.FirstOrDefault(x => x.Id == order.IssuedUserId).WorkAreaEntity.Name == "Logística"
                        && userRoles.FirstOrDefault(x => x.Role.Name == "Oficina Técnica" && x.UserId == order.IssuedUserId) != null)
                    order.CorrelativeCodeStr += "-OB";

            return Ok(new
            {
                draw = ConstantHelpers.Datatable.ServerSide.SentParameters.DRAW_COUNTER,
                recordsTotal = totalRecords,
                recordsFiltered = count,
                data
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            /*
            var requests = await _context.RequestsInOrders
                .Include(x => x.Request)
                .Include(x => x.Request.SupplyFamily)
                .Where(x => x.OrderId == id)
                .Select(x => x.Request)
                .ToListAsync();
            */
            var data = await _context.Orders
                .Select(x => new OrderViewModel
                {
                    Id = x.Id,
                    CorrelativeCode = x.CorrelativeCode,
                    ProviderId = x.ProviderId,
                    RequestIds = x.Requests.Select(i => i.RequestId).ToList(),
                    RequestsName = string.Join(" / ", x.Requests.Select(i => i.Request.CorrelativePrefix + "-" + i.Request.CorrelativeCode.ToString("D4")).ToList()),
                    Provider = new ProviderViewModel
                    {
                        RUC = x.Provider.RUC,
                        BusinessName = x.Provider.BusinessName
                    },
                    QuotationNumber = x.QuotationNumber,
                    Date = x.Date.ToDateString(),
                    DeliveryTime = x.DeliveryTime,
                    Currency = x.Currency,
                    PaymentMethod = x.PaymentMethod,
                    WarehouseId = x.WarehouseId,
                    BillTo = x.BillTo,
                    Warranty = x.Warranty,
                    Observations = x.Observations,
                    Status = x.Status,
                    AttentionStatus = x.AttentionStatus,
                    PriceFileUrl = x.PriceFileUrl,
                    SupportFileUrl = x.SupportFileUrl,
                    PdfFileUrl = x.PdfFileUrl,
                    QualityCertificate = x.QualityCertificate,
                    Blueprint = x.Blueprint,
                    SecurityDocument = x.SecurityDocument,
                    CalibrationCertificate = x.CalibrationCertificate,
                    CatalogAndStorageCriteria = x.CatalogAndStorageCriteria,
                    Other = x.Other,
                    OtherDescription = x.OtherDescription,
                    Conditions = x.Conditions,
                    ExchangeRate = x.ExchangeRate.ToString(),
                    CorrelativeCodeSuffix = x.CorrelativeCodeSuffix,
                    CorrelativeCodeManual = !string.IsNullOrEmpty(x.CorrelativeCodeSuffix),
                    ManualWarehouse = x.ManualWarehouse,
                    WarehouseCheckbox = !string.IsNullOrEmpty(x.ManualWarehouse)
                }).AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (data == null)
                return BadRequest();
            /*
            foreach (var item in requests)
            {
                var requestTypeStr = "";

                var groups = "";

                if (item.RequestType == 1)
                    requestTypeStr = "Compra";
                else
                    requestTypeStr = "Servicio";

                var folding = _context.RequestItems.Include(x => x.GoalBudgetInput)
              .Include(x => x.GoalBudgetInput.Supply)
              .Include(x => x.GoalBudgetInput.Supply.SupplyGroup)
              .AsEnumerable().Where(x => x.RequestId == item.Id)
                .GroupBy(x => x.GoalBudgetInput.Supply.SupplyGroupId).Where(x => x.Count() > 0);

                var userString = "";
                var reqUsers = await _context.RequestUsers.Where(x => x.RequestId == item.Id).ToListAsync();
                foreach (var reqUser in reqUsers)
                {
                    var last = reqUsers.Last();
                    var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == reqUser.UserId);
                    if (reqUser == last)
                        userString = userString + user.FullName;
                    else
                        userString = userString + user.FullName + " - ";
                }

                foreach (var itemF in folding)
                {
                    groups += itemF.FirstOrDefault().GoalBudgetInput.Supply.SupplyGroup.Name + " ";
                }


                data.RequestList += (item.CorrelativePrefix + "-" + item.CorrelativeCode.ToString("D4") + " | Tipo: " + requestTypeStr + " | F. Entrega: " + item.DeliveryDate.Value.ToDateString()
                + " | Familia: " + item.SupplyFamily.Name + " | Grupos: " + groups + " | Solicitantes: " + userString);
            }
            */

            return Ok(data);
        }

        [HttpGet("compra/detalles/listar")]
        public async Task<IActionResult> GetDetail(Guid id)
        {
            var orderItems = await _context.OrderItems
                  .Where(x => x.OrderId == id)
                  .Include(x => x.Order)
                  .Include(x => x.Supply)
                  .Include(x => x.Supply.SupplyFamily)
                  .Include(x => x.Supply.SupplyGroup)
                  .Include(x => x.Supply.MeasurementUnit)
                  .ToListAsync();

            var order = await _context.Orders
                .Include(x => x.Requests)
                .FirstOrDefaultAsync(x => x.Id == id);

            var data = new List<OrderItemViewModel>();

            if (order == null)
                return Ok(data);

            var reqItems = await _context.RequestItems
                .Include(x => x.Supply)
                .Include(x => x.Request)
                .Where(x => order.Requests.Select(y => y.RequestId).Contains(x.RequestId))
                .ToListAsync();

            var observation = "";

            foreach (var item in orderItems)
            {
                var aux = reqItems.Where(x => x.SupplyId == item.SupplyId
                && x.Observations == item.Observations).ToList();
                var list = new List<string>();
                var measure = 0.0;
                var measureInAttention = 0.0;

                foreach (var itemReq in aux)
                {
                    measure += itemReq.Measure;
                    measureInAttention += itemReq.MeasureInAttention;
                    observation = itemReq.Observations;
                }

                foreach (var req in aux.GroupBy(x => x.RequestId))
                {
                    list.Add(req.FirstOrDefault().Request.CorrelativePrefix
                        + "-" + req.FirstOrDefault().Request.CorrelativeCode.ToString("D4"));
                }

                data.Add(new OrderItemViewModel
                {
                    Id = item.Id,
                    Supply = new SupplyViewModel
                    {
                        Description = item.Supply.Description,
                        SupplyFamily = new SupplyFamilyViewModel
                        {
                            Code = item.Supply.SupplyFamily.Code
                        },
                        SupplyGroupId = item.Supply.SupplyGroupId,
                        SupplyGroup = new SupplyGroupViewModel
                        {
                            Code = item.Supply.SupplyGroup.Code
                        },
                        CorrelativeCode = item.Supply.CorrelativeCode,
                        MeasurementUnit = new ViewModels.MeasurementUnitViewModels.MeasurementUnitViewModel
                        {
                            Abbreviation = item.Supply.MeasurementUnit.Abbreviation
                        }
                    },
                    RequestMeasure = measure,
                    RequestMeasureInAttention = measureInAttention,
                    RequestObservation = item.Observations,
                    RequestCode = string.Join(" - ", list),
                    Order = new OrderViewModel
                    {
                        Status = item.Order.Status
                    },
                    Measure = Math.Round(item.Measure, 2).ToString(CultureInfo.InvariantCulture),
                    //UnitPrice = Math.Round(item.UnitPrice, 2).ToString(),
                    UnitPrice = Math.Round(item.UnitPrice, 6).ToString(CultureInfo.InvariantCulture),
                    Parcial = item.Parcial.ToString("N2", CultureInfo.InvariantCulture),
                    Glosa = item.Glosa,
                    UnitPriceDiscount = Math.Round(item.DiscountedUnitPrice, 6).ToString(CultureInfo.InvariantCulture),
                    Discount = new OrderItemDiscountViewModel
                    {
                        FinancialDiscount = item.FinancialDiscount.ToString(CultureInfo.InvariantCulture),
                        ItemDiscount = item.FinancialDiscount.ToString(CultureInfo.InvariantCulture),
                        AdditionalDiscount = item.AdditionalDiscount.ToString(CultureInfo.InvariantCulture),
                        IGV = item.IGV.ToString(CultureInfo.InvariantCulture),
                        ISC = item.ISC.ToString(CultureInfo.InvariantCulture)
                    }
                });
            }

            /*
            if (orderItems.Count != 0 && (orderItems.FirstOrDefault().Order.Status == ConstantHelpers.Logistics.RequestOrder.Status.PRE_ISSUED 
                || orderItems.FirstOrDefault().Order.Status == ConstantHelpers.Logistics.RequestOrder.Status.CANCELED))
            {
                foreach (var item in data)
                {
                    item.RequestItem.MeasureInAttention = orderItems.FirstOrDefault(x => x.Id == item.Id).RequestItem.MeasureInAttention;
                }
            }
            */
            return Ok(data);
        }

        [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.FULL_ACCESS)]
        [DisableRequestSizeLimit]
        [HttpPost("compra/crear")]
        public async Task<IActionResult> Create(OrderViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (model.RequestIds.Count == 0)
                return BadRequest("No se ha seleccionado los requerimientos");

            if ((model.Currency == ConstantHelpers.Currency.AMERICAN_DOLLARS || model.Currency == ConstantHelpers.Currency.EUROS) && model.ExchangeRate.ToDoubleString() <= 0)
                return BadRequest("El tipo de cambio no puede ser 0");

            var order = new Order
            {
                //CorrelativeCode = count,
                Type = ConstantHelpers.Logistics.RequestOrder.Type.PURCHASE,
                ProviderId = model.ProviderId,
                QuotationNumber = model.QuotationNumber,
                Date = string.IsNullOrEmpty(model.Date) ? DateTime.Today : model.Date.ToDateTime(),
                Currency = model.Currency,
                PaymentMethod = model.PaymentMethod,
                DeliveryTime = model.DeliveryTime,
                WarehouseId = model.WarehouseId,
                BillTo = model.BillTo,
                Warranty = model.Warranty,
                Observations = model.Observations,
                QualityCertificate = model.QualityCertificate,
                Blueprint = model.Blueprint,
                SecurityDocument = model.SecurityDocument,
                CalibrationCertificate = model.CalibrationCertificate,
                CatalogAndStorageCriteria = model.CatalogAndStorageCriteria,
                Other = model.Other,
                OtherDescription = model.OtherDescription,
                Conditions = model.Conditions,
                IssuedUserId = GetUserId(),
                ExchangeRate = model.ExchangeRate.ToDoubleString(),
                ProjectId = GetProjectId()
            };

            var orders = await _context.Orders
                .Where(x => x.Type == ConstantHelpers.Logistics.RequestOrder.Type.PURCHASE
                && x.ProjectId == GetProjectId()
                && x.Date.Year == DateTime.Now.Year).ToListAsync();

            if (model.CorrelativeCodeManual == true)
            {
                if (orders.FirstOrDefault(x => x.CorrelativeCode == model.CorrelativeCode
                 && order.CorrelativeCodeSuffix == model.CorrelativeCodeSuffix) == null)
                {
                    order.CorrelativeCode = model.CorrelativeCode.Value;
                    order.CorrelativeCodeSuffix = model.CorrelativeCodeSuffix;
                }
                else
                    BadRequest("La combinación de correlativo y sufijo ya están en uso en este año");
            }
            else
            {
                if (orders.Count() > 0)
                {
                    var count = orders.OrderBy(x => x.CorrelativeCode).Last();
                    order.CorrelativeCode = count.CorrelativeCode + 1;
                }
                else
                {
                    order.CorrelativeCode = 1;
                }
            }

            if (model.PriceFile != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                order.PriceFileUrl = await storage.UploadFile(model.PriceFile.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.LOGISTICS, System.IO.Path.GetExtension(model.PriceFile.FileName), 
                    ConstantHelpers.Storage.Blobs.ORDER_PRICE, order.CorrelativeCode.ToString());
            }
            if (model.SupportFile != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                order.SupportFileUrl = await storage.UploadFile(model.SupportFile.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.LOGISTICS, System.IO.Path.GetExtension(model.SupportFile.FileName),
                    ConstantHelpers.Storage.Blobs.ORDER_SUPPORT, order.CorrelativeCode.ToString());
            }

            if(model.WarehouseCheckbox == true)
            {
                order.WarehouseId = null;
                order.ManualWarehouse = model.ManualWarehouse;
            }
            else
            {
                order.ManualWarehouse = null;
                order.WarehouseId = model.WarehouseId;
            }

            var reqs = new List<RequestsInOrder>();

            var orderItems = new List<OrderItem>();

            var total = 0.0;

            var reqItems = await _context.RequestItems
                .Include(x => x.Supply)
                .Where(x => model.RequestIds.Contains(x.RequestId))
                .ToListAsync();

            var approveAuths = await _context.LogisticResponsibles
                .Where(x => x.ProjectId == GetProjectId() 
                && x.UserType == ConstantHelpers.Logistics.RequestOrder.UserTypes.OkOrder)
                .ToListAsync();

            var supplies = await _context.Supplies.ToListAsync();

            var requests = await _context.Requests.Where(x => model.RequestIds.Contains(x.Id)).ToListAsync();

            foreach (var stringItem in model.Items)
            {
                var stringSplit = stringItem.Split("|");

                if (stringSplit[1] == "undefined" || stringSplit[2] == "undefined")
                    return BadRequest("Quite lo filtros de la tabla de insumos para guardar");

                if (string.IsNullOrEmpty(stringSplit[1]))
                    return BadRequest("No se ha ingresado todas las cantidades a atender");

                if (string.IsNullOrEmpty(stringSplit[2]))
                    return BadRequest("No se ha ingresado todos los precios unitarios");

                var guidSplit = stringSplit[0].Split("_");

                Guid supplyId = Guid.Parse(guidSplit[0]);
                if (stringSplit[4] == null) stringSplit[4] = "";
                var measure = 0.0;
                var measureAttention = 0.0;
                var toAttend = 0.0;

                var aux = reqItems.Where(x => x.SupplyId == supplyId && x.Observations == stringSplit[4]).ToList();

                var cant = stringSplit[1].ToDoubleString();

                var saldo = cant;

                foreach (var item in aux)
                {
                    measure += item.Measure;
                    measureAttention += item.MeasureInAttention;
                    toAttend = item.Measure - item.MeasureInAttention;
                    item.Supply.Status = ConstantHelpers.Supply.Status.IN_ORDER;

                    if (toAttend == 0)
                        continue;
                    else if (saldo > toAttend)
                    {
                        if (aux.Last() == item)
                            return BadRequest("Se está superando el límite atendido");
                        saldo -= toAttend;
                        item.MeasureInAttention += toAttend;
                    }
                    else
                    {
                        item.MeasureInAttention += saldo;
                        saldo = 0;
                    }
                }

                var price = double.Parse(stringSplit[2], CultureInfo.InvariantCulture);

                //if (Double.TryParse(stringSplit[2], out double result))
                //  price = result;

                var glosa = stringSplit[3].ToString();

                if (cant > (measure - measureAttention))
                    return BadRequest("Se ha superado el límite de metrado para atender");

                total += Math.Round(cant * price, 2);

                if (cant > 0)
                    orderItems.Add(new OrderItem
                    {
                        SupplyId = supplyId,
                        Order = order,
                        Measure = cant,
                        UnitPrice = price,
                        DiscountedUnitPrice = price,
                        Parcial = Math.Round(cant * price, 2),
                        Glosa = glosa,
                        Observations = stringSplit[4].ToString()
                    });
            }

            order.Parcial = Math.Round(total, 2);
            
            foreach (var id in model.RequestIds)
            {
                var req = await _context.Requests.FirstOrDefaultAsync(x => x.Id == id);
                req.OrderStatus = ConstantHelpers.Logistics.RequestOrder.Status.ORDER_C;
                var reqInOrder = new RequestsInOrder
                {
                    Order = order,
                    RequestId = id
                };
                reqs.Add(reqInOrder);

                var completo = true;
                var pendiente = false;

                foreach (var item in reqItems)
                {
                    if (item.Measure != item.MeasureInAttention)
                    {
                        completo = false;
                        if (item.MeasureInAttention == 0)
                            pendiente = true;
                    }
                }

                if (completo == true)
                    req.AttentionStatus = ConstantHelpers.Logistics.RequestOrder.AttentionStatus.TOTAL;
                else
                {
                    if (pendiente == false)
                        req.AttentionStatus = ConstantHelpers.Logistics.RequestOrder.AttentionStatus.PARTIAL;
                    else
                        req.AttentionStatus = ConstantHelpers.Logistics.RequestOrder.AttentionStatus.PENDING;
                }
            }

            if (model.Liquidation)
            {
                order.ReviewDate = DateTime.Now;
                order.ApproveDate = DateTime.Now;
                order.Status = ConstantHelpers.Logistics.RequestOrder.Status.APPROVED;
                foreach (var item in orderItems)
                {
                    var saldo = item.Measure;
                    //foreach (var reqItem in reqItems.Where(x => x.SupplyId == item.SupplyId))
                    //    if (saldo > reqItem.Measure)
                    //    {
                    //        saldo -= reqItem.Measure;
                    //        reqItem.GoalBudgetInput.CurrentMetered -= reqItem.Measure;
                    //    }
                    //    else
                    //    {
                    //        reqItem.GoalBudgetInput.CurrentMetered -= saldo;
                    //        saldo = 0;
                    //    }
                    supplies.FirstOrDefault(x => x.Id == item.SupplyId).Status = ConstantHelpers.Supply.Status.IN_ORDER;
                }
                foreach (var req in reqs)
                {
                    var reqItem = reqItems.Where(x => x.RequestId == req.RequestId);
                    requests.FirstOrDefault(x => x.Id == req.RequestId).AttentionStatus = ConstantHelpers.Logistics.RequestOrder.AttentionStatus.TOTAL;

                    foreach (var item in reqItem)
                        if (item.Measure != item.MeasureInAttention)
                            requests.FirstOrDefault(x => x.Id == req.RequestId).AttentionStatus = ConstantHelpers.Logistics.RequestOrder.AttentionStatus.PARTIAL;
                }
                foreach (var authItem in approveAuths)
                {
                    var type = "";
                    if (order.Type == ConstantHelpers.Logistics.RequestOrder.Type.PURCHASE) type = "Compra";
                    else type = "Servicio";
                    var projectName = _context.Projects.FirstOrDefault(x => x.Id == GetProjectId());

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress("sistemaerp@ivc.pe", "Sistema IVC"),
                        Subject = $"Aviso de aprobación de Orden de {type} {projectName.CostCenter.ToString()}-{order.CorrelativeCode.ToString("D4")}{(order.CorrelativeCodeSuffix != null ? order.CorrelativeCodeSuffix: "" )}-{order.ReviewDate.Value.Year.ToString()}"
                    };

                    var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == authItem.UserId);

                    mailMessage.To.Add(new MailAddress(user.Email, user.RawFullName));

                    mailMessage.Body =
                       $"Hola, {user.RawFullName}<br /><br /> " +
                       $"Se le informa que la orden de {type} {projectName.CostCenter.ToString()}-{order.CorrelativeCode.ToString("D4")}{(order.CorrelativeCodeSuffix != null ? order.CorrelativeCodeSuffix : "")}-{order.ReviewDate.Value.Year.ToString()} para el proyecto {projectName.Abbreviation} ha sido aprobada mediante liquidación.<br />" +
                       $"<br />" +
                       $"Saludos Cordiales<br /><br />Sistema IVC <br />" +
                       $"Control de Logística";
                    mailMessage.IsBodyHtml = true;
                    //Mandar Correo
                    using (var client = new SmtpClient("smtp.office365.com", 587))
                    {
                        client.UseDefaultCredentials = false;
                        client.Credentials = new NetworkCredential("sistemaerp@ivc.pe", "S1st3m4erp");
                        client.EnableSsl = true;
                        await client.SendMailAsync(mailMessage);
                    }
                }
            }
            await _context.OrderItems.AddRangeAsync(orderItems);
            await _context.RequestsInOrders.AddRangeAsync(reqs);
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.FULL_ACCESS)]
        [DisableRequestSizeLimit]
        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, OrderViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _context.SupplyEntries.FirstOrDefaultAsync(x => x.OrderId == id) != null)
                return BadRequest("No se puede editar una orden que está en uso en Almacén");

            var order = await _context.Orders
                .Include(x => x.Requests)
                .FirstOrDefaultAsync(x => x.Id == id);

            var orderItems = await _context.OrderItems
                .Where(x => x.OrderId == id)
                .ToListAsync();

            var requestItems = _context.RequestItems
                .Include(x => x.Request)
                .Include(x => x.Supply)
                .Where(x => order.Requests.Select(y => y.RequestId).Contains(x.RequestId))
                .ToList();

            var orderAuth = await _context.OrderAuthorizations
                .Where(x => x.OrderId == id)
                .ToListAsync();

            if (order.Status == ConstantHelpers.Logistics.RequestOrder.Status.APPROVED
               || order.Status == ConstantHelpers.Logistics.RequestOrder.Status.APPROVED_PARTIALLY
               || order.Status == ConstantHelpers.Logistics.RequestOrder.Status.CANCELED
               || order.Status == ConstantHelpers.Logistics.RequestOrder.Status.ANSWER_PENDING)
                return BadRequest("No se puede editar una orden aprobada, aprobada parcialmente, pendiente de respuesta o cancelada");

            //order.CorrelativeCode = model.CorrelativeCode;
            order.ProviderId = model.ProviderId;
            order.QuotationNumber = model.QuotationNumber;
            order.Currency = model.Currency;
            order.PaymentMethod = model.PaymentMethod;
            order.DeliveryTime = model.DeliveryTime;
            order.WarehouseId = model.WarehouseId;
            order.BillTo = model.BillTo;
            order.Warranty = model.Warranty;
            order.Observations = model.Observations;
            order.QualityCertificate = model.QualityCertificate;
            order.Blueprint = model.Blueprint;
            order.SecurityDocument = model.SecurityDocument;
            order.CalibrationCertificate = model.CalibrationCertificate;
            order.CatalogAndStorageCriteria = model.CatalogAndStorageCriteria;
            order.Other = model.Other;
            order.OtherDescription = model.OtherDescription;
            order.Conditions = model.Conditions;
            order.ExchangeRate = model.ExchangeRate.ToDoubleString();
            order.Date = model.Date.ToDateTime();

            var orders = await _context.Orders
                .Where(x => x.Type == ConstantHelpers.Logistics.RequestOrder.Type.PURCHASE
                && x.ProjectId == GetProjectId()
                && x.Date.Year == DateTime.Now.Year).ToListAsync();

            if (model.CorrelativeCodeManual == true)
            {
                if (orders.FirstOrDefault(x => x.CorrelativeCode == model.CorrelativeCode
                 && order.CorrelativeCodeSuffix == model.CorrelativeCodeSuffix) == null)
                {
                    order.CorrelativeCode = model.CorrelativeCode.Value;
                    order.CorrelativeCodeSuffix = model.CorrelativeCodeSuffix;
                }
                else
                    BadRequest("La combinación de correlativo y sufijo ya están en uso en este año");
            }
            else
            {
                order.CorrelativeCodeSuffix = null;
            }

            if (model.PriceFile != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (order.PriceFileUrl != null)
                    await storage.TryDelete(order.PriceFileUrl, ConstantHelpers.Storage.Containers.LOGISTICS);
                order.PriceFileUrl = await storage.UploadFile(model.PriceFile.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.LOGISTICS, System.IO.Path.GetExtension(model.PriceFile.FileName), ConstantHelpers.Storage.Blobs.ORDER_PRICE, order.CorrelativeCode.ToString());
            }
            if (model.SupportFile != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (order.SupportFileUrl != null)
                    await storage.TryDelete(order.SupportFileUrl, ConstantHelpers.Storage.Containers.LOGISTICS);
                order.SupportFileUrl = await storage.UploadFile(model.SupportFile.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.LOGISTICS, System.IO.Path.GetExtension(model.SupportFile.FileName), ConstantHelpers.Storage.Blobs.ORDER_SUPPORT, order.CorrelativeCode.ToString());
            }
            if(model.WarehouseCheckbox == true)
            {
                order.WarehouseId = null;
                order.ManualWarehouse = model.ManualWarehouse;
            }
            else
            {
                order.WarehouseId = model.WarehouseId;
                order.ManualWarehouse = null;
            }

            var total = 0.0;

            foreach (var item in orderItems)
            {
                var saldo = item.Measure;
                foreach (var reqItem in requestItems.Where(x => x.SupplyId == item.SupplyId && x.Observations == item.Observations))
                    if (saldo > reqItem.Measure)
                    {
                        saldo -= reqItem.MeasureInAttention;
                        reqItem.MeasureInAttention = 0;
                    }
                    else
                    {
                        reqItem.MeasureInAttention -= saldo;
                        saldo = 0;
                    }
            }

            foreach (var stringItem in model.Items)
            {
                var stringSplit = stringItem.Split("|");
                Guid ordId = Guid.Parse(stringSplit[0]);

                var cant = stringSplit[1].ToDoubleString();
                var price = double.Parse(stringSplit[2], CultureInfo.InvariantCulture);
                var glosa = stringSplit[3].ToString();
                var obs = stringSplit[4] == null ? "" : stringSplit[4].ToString();

                var orderItem = orderItems.FirstOrDefault(x => x.Id == ordId);
                var limite = 0.0;

                var reqAux = requestItems.Where(x => x.SupplyId == orderItem.SupplyId && x.Observations == orderItem.Observations).ToList();

                foreach (var item in reqAux)
                {
                    limite += item.Measure;
                    limite -= item.MeasureInAttention;
                }

                if (order.Status != ConstantHelpers.Logistics.RequestOrder.Status.CANCELED
                    && order.Status != ConstantHelpers.Logistics.RequestOrder.Status.PRE_ISSUED
                    && order.Status != ConstantHelpers.Logistics.RequestOrder.Status.OBSERVED)
                    limite += orderItem.Measure;

                if (cant > limite)
                    return BadRequest("Se ha superado el límite de metrado para atender");

                orderItem.Measure = cant;
                orderItem.UnitPrice = price;
                orderItem.Parcial = Math.Round(cant * price, 2);
                orderItem.Glosa = glosa;

                total += Math.Round(cant * price, 2);

                if (cant == 0)
                    _context.Remove(orderItem);
            }

            foreach (var item in orderItems)
            {
                var saldo = item.Measure;
                foreach (var reqItem in requestItems.Where(x => x.SupplyId == item.SupplyId 
                && x.Observations == item.Observations))
                {
                    var toAttend = reqItem.Measure - reqItem.MeasureInAttention;
                    if (saldo > toAttend)
                    {
                        saldo -= toAttend;
                        reqItem.MeasureInAttention += toAttend;
                    }
                    else
                    {
                        reqItem.MeasureInAttention += saldo;
                        saldo = 0;
                    }
                }
            }

            order.Parcial = Math.Round(total, 2);
            /*
            foreach (var req in order.Requests)
            {

                req.Request.AttentionStatus = ConstantHelpers.Logistics.RequestOrder.AttentionStatus.TOTAL;

                foreach (var item in requestItems.Where(x => x.RequestId == req.RequestId))
                    if (item.Measure != item.MeasureInAttention)
                        req.Request.AttentionStatus = ConstantHelpers.Logistics.RequestOrder.AttentionStatus.PARTIAL;
            }
            */
            foreach (var req in order.Requests)
            {
                var completo = true;
                var pendiente = false;

                foreach (var item in requestItems.Where(x => x.RequestId == req.RequestId))
                    if (item.Measure != item.MeasureInAttention)
                    {
                        completo = false;
                        if (item.MeasureInAttention == 0)
                            pendiente = true;
                    }

                if (completo == true)
                    req.Request.AttentionStatus = ConstantHelpers.Logistics.RequestOrder.AttentionStatus.TOTAL;
                else
                {
                    if (pendiente == false)
                        req.Request.AttentionStatus = ConstantHelpers.Logistics.RequestOrder.AttentionStatus.PARTIAL;
                    else
                        req.Request.AttentionStatus = ConstantHelpers.Logistics.RequestOrder.AttentionStatus.PENDING;
                }
            }

            if (order.Status == ConstantHelpers.Logistics.RequestOrder.Status.OBSERVED)
                order.Status = ConstantHelpers.Logistics.RequestOrder.Status.PRE_ISSUED;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("emitir/{id}")]
        public async Task<IActionResult> UpdateStatusIssued(Guid id)
        {
            var order = await _context.Orders
                .Include(x => x.Provider)
                .Include(x => x.Requests)
                .Include(x => x.Project)
                .FirstOrDefaultAsync(x => x.Id == id);

            var pId = GetProjectId();

            var logRes = await _context.LogisticResponsibles
              .Where(x => x.ProjectId == pId)
              .ToListAsync();

            if (order.Status == ConstantHelpers.Logistics.RequestOrder.Status.OBSERVED)
            {
                return BadRequest("No se puede emitir una orden observada");
            }

            order.Status = ConstantHelpers.Logistics.RequestOrder.Status.ANSWER_PENDING;
            order.ReviewDate = DateTime.Today;

            var logOrds = logRes
           .Where(x => x.UserType >= ConstantHelpers.Logistics.RequestOrder.UserTypes.AuthOrder)
           .ToList();

            var logExiste = await _context.OrderAuthorizations.Where(x => x.OrderId == id).ToListAsync();

            _context.RemoveRange(logExiste);

            //var linkkk = "https://localhost:44307";
            //var linkAuth = $"{linkkk}/logistica/ordenes/evaluar/{id}";
            var linkAuth = $"{ConstantHelpers.SystemUrl.Url}logistica/ordenes/evaluar/{id}";

            var orAuths = new List<OrderAuthorization>();

            Attachment attachment = GetFile(id);
            var type = ConstantHelpers.Logistics.RequestOrder.Type.VALUES[order.Type];
            var code = order.Project.CostCenter + "-" + order.CorrelativeCode.ToString("D4") + "-" + order.ReviewDate.Value.Year.ToString();

            //mailMessage.To.Add(new MailAddress("arian.cc@hotmail.com", "Arian"));
            /* -- Authorizations -- */

            var log = logOrds.Where(x => x.UserType == ConstantHelpers.Logistics.RequestOrder.UserTypes.AuthOrder).ToList();
            foreach (var auth in log)
            {
                orAuths.Add(new OrderAuthorization
                {
                    OrderId = id,
                    UserId = auth.UserId,
                    UserType = auth.UserType
                });

                var mail = new MailMessage
                {
                    From = new MailAddress("sistemaerp@ivc.pe", "Sistema IVC"),
                    Subject = $"Aviso de emisión de Orden de {type} {order.Project.CostCenter.ToString()}-{order.CorrelativeCode.ToString("D4")}{order.CorrelativeCodeSuffix}-{order.ReviewDate.Value.Year.ToString()}"
                };

                if (attachment != null)
                    mail.Attachments.Add(attachment);

                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == auth.UserId);
                var linkResponsible1 = $"{linkAuth}?userId={user.Id}";
                var linkApprove1 = $"{linkResponsible1}&isApproved=true";
                var linkReject1 = $"{linkResponsible1}&isApproved=false";

                mail.To.Add(new MailAddress(user.Email, user.RawFullName));

                mail.Body =
                   $"Hola, {user.RawFullName}<br /><br /> " +
                   $"Se le solicita revisar y aprobar la orden de {type} {order.Project.CostCenter.ToString()}-{order.CorrelativeCode.ToString("D4")}{order.CorrelativeCodeSuffix}-{order.ReviewDate.Value.Year.ToString()} para el proyecto {order.Project.Abbreviation}.<br />" +
                   $"Proveedor {order.Provider.Tradename} <br />" +
                   $"Monto {ConstantHelpers.Currency.SIGN_VALUES[order.Currency]} {order.Parcial.ToString("N2", CultureInfo.InvariantCulture)} + IGV<br />El excel de la orden de {type} se encuentra adjunto al correo.<br /><br />" +
                   $"Si todo es conforme haga click en el siguiente enlace: <a href='{linkApprove1}'><span style='color:green'>APROBAR.</span></a><br />" +
                   $"De encontrar alguna disconformidad haga click en el siguiente enlace: <a href='{linkReject1}'><span style='color:red'>DESAPROBAR.</span></a><br />" +
                   $"Saludos Cordiales<br /><br />Sistema IVC <br />" +
                   $"Control de Logística";
                mail.IsBodyHtml = true;
                //Mandar Correo
                using (var client = new SmtpClient("smtp.office365.com", 587))
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("sistemaerp@ivc.pe", "S1st3m4erp");
                    client.EnableSsl = true;
                    await client.SendMailAsync(mail);
                }
            }

            /* -- Reviews -- */
            var mailMessage = new MailMessage
            {
                From = new MailAddress("sistemaerp@ivc.pe", "Sistema IVC"),
                Subject = $"{order.Requests.FirstOrDefault().Request.CorrelativePrefix} - Aviso de emisión de Orden de {type} {order.Project.CostCenter.ToString()}-{order.CorrelativeCode.ToString("D4")}{order.CorrelativeCodeSuffix}-{order.ReviewDate.Value.Year.ToString()}"
            };

            mailMessage.Attachments.Add(attachment);

            foreach (var auth in logRes.Where(x => x.UserType == ConstantHelpers.Logistics.RequestOrder.UserTypes.ReviewOrder))
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == auth.UserId);

                mailMessage.To.Add(new MailAddress(user.Email, user.RawFullName));
            }
            //mailMessage.To.Add(new MailAddress("henrrypaul_22@hotmail.com", "Henrry"));

            mailMessage.Body =
                $"Hola, <br /><br /> " +
                $"Se ha emitido una  Orden de {type} {order.Project.CostCenter.ToString()}-{order.CorrelativeCode.ToString("D4")}{order.CorrelativeCodeSuffix}-{order.ReviewDate.Value.Year.ToString()} que se encuentra pendiente de aprobación para el proyecto {order.Project.Abbreviation}.<br />" +
                $"<br />" +
                $"Orden {code}<br />" +
                $"Proveedor {order.Provider.Tradename} <br />" +
                $"Monto {ConstantHelpers.Currency.SIGN_VALUES[order.Currency]} {order.Parcial.ToString("N2", CultureInfo.InvariantCulture)} + IGV<br />" +
                $"<br />" +
                $"El excel de la orden de {type} se adjuntan al correo.<br />" +
                $"Saludos <br />" +
                $"Sistema IVC <br />" +
                $"Control de Logística";
            mailMessage.IsBodyHtml = true;

            //Mandar Correo
            using (var client = new SmtpClient("smtp.office365.com", 587))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("sistemaerp@ivc.pe", "S1st3m4erp");
                client.EnableSsl = true;
                await client.SendMailAsync(mailMessage);
            }

            await _context.OrderAuthorizations.AddRangeAsync(orAuths);
            await _context.SaveChangesAsync();
            return Ok();
        }
        /*
        [HttpGet("correos")]
        public async Task<IActionResult> SendMails()
        {

            var pId = GetProjectId();

            var orders = await _context.Orders
                .Include(x => x.Provider)
                .Include(x => x.Requests)
                .Include(x => x.Warehouse.WarehouseType.Project)
                .Where(x => x.Warehouse.WarehouseType.ProjectId == pId
                && x.Status == ConstantHelpers.Logistics.RequestOrder.Status.APPROVED)
                .Take(5)
                .ToListAsync();

            var logRes = await _context.LogisticResponsibles
              .Where(x => x.ProjectId == pId)
              .ToListAsync();

            foreach (var order in orders)
            {
                Attachment attachment = GetFile(order.Id);

                var type = ConstantHelpers.Logistics.RequestOrder.Type.VALUES[order.Type];
                var code = order.Warehouse.WarehouseType.Project.CostCenter + "-" + order.CorrelativeCode.ToString("D4") + "-" + order.ReviewDate.Value.Year.ToString();

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("sistemaerp@ivc.pe", "Sistema IVC"),
                    Subject = $"{order.Requests.FirstOrDefault().Request.CorrelativePrefix} - Aviso de emisión de Orden de {type} {code}"
                };

                mailMessage.Attachments.Add(attachment);
                mailMessage.To.Add(new MailAddress("pargumedo@jicamarca.pe", "ARGUMEDO ESTAY, FRANCISCO"));

                mailMessage.Body =
                    $"Hola, <br /><br /> " +
                    $"Se ha emitido una  Orden de {type} en el centro de costo {order.Warehouse.WarehouseType.Project.Abbreviation}.<br />" +
                    $"<br />" +
                    $"Orden {code}<br />" +
                    $"Proveedor {order.Provider.Tradename} <br />" +
                    $"Monto {ConstantHelpers.Currency.SIGN_VALUES[order.Currency]} {order.Parcial.ToString("N2", CultureInfo.InvariantCulture)} + IGV<br />" +
                    $"<br />" +
                    $"Saludos <br />" +
                    $"Sistema IVC <br />" +
                    $"Control de Logística";
                mailMessage.IsBodyHtml = true;

                //Mandar Correo
                using (var client = new SmtpClient("smtp.office365.com", 587))
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("sistemaerp@ivc.pe", "S1st3m4erp");
                    client.EnableSsl = true;
                    await client.SendMailAsync(mailMessage);
                }
            }

            return Ok();
        }
    */

        [HttpPut("cancelar/{id}")]
        public async Task<IActionResult> UpdateStatusCanceled(Guid id)
        {
            //            var order = await _context.Orders
            //                   .Include(x => x.Requests)
            //                   .FirstOrDefaultAsync(x => x.Id == id);

            //            var pId = GetProjectId();

            //            var items = await _context.OrderItems
            //                .Where(x => x.OrderId == id)
            //                .ToListAsync();

            //            var reqItems = await _context.RequestItems
            //               .Include(x => x.Request)
            //               .Include(x => x.GoalBudgetInput)
            //               .Where(x => order.Requests.Select(x => x.RequestId).Contains(x.RequestId))
            //               .ToListAsync();

            //            var logRes = await _context.LogisticResponsibles
            //                .Where(x => x.ProjectId == pId)
            //                .ToListAsync();

            //            order.Status = ConstantHelpers.Logistics.RequestOrder.Status.CANCELED;
            //            order.AttentionStatus = ConstantHelpers.Logistics.RequestOrder.AttentionStatus.PENDING;
            //            order.ApproveDate = DateTime.Today;

            //            var logExiste = await _context.OrderAuthorizations.Where(x => x.OrderId == id
            //            && x.UserType == ConstantHelpers.Logistics.RequestOrder.UserTypes.AuthOrder).ToListAsync();

            //            _context.RemoveRange(logExiste);
            //*/          
            //            var logAuths = logRes
            //           .Where(x => x.UserType == ConstantHelpers.Logistics.RequestOrder.UserTypes.AuthOrder)
            //           .ToList();

            //            string userId = GetUserId();

            //            if (logAuths.Select(x => x.UserId).Contains(userId) == false)
            //                return BadRequest("Usted no tiene los permisos para anular esta orden de compra");

            //            foreach (var item in items)
            //            {
            //                var saldo = item.Measure;
            //                foreach (var reqItem in reqItems.Where(x => x.GoalBudgetInput.SupplyId == item.SupplyId))
            //                    if (saldo > reqItem.Measure)
            //                    {
            //                        saldo -= reqItem.MeasureInAttention;
            //                        reqItem.MeasureInAttention = 0;
            //                    }
            //                    else
            //                    {
            //                        reqItem.MeasureInAttention -= saldo;
            //                        saldo = 0;
            //                    }
            //            }

            //            foreach (var req in order.Requests)
            //            {
            //                req.Request.AttentionStatus = ConstantHelpers.Logistics.RequestOrder.AttentionStatus.TOTAL;

            //                foreach (var item in reqItems.Where(x => x.RequestId == req.RequestId))
            //                    if (item.Measure != item.MeasureInAttention)
            //                        req.Request.AttentionStatus = ConstantHelpers.Logistics.RequestOrder.AttentionStatus.PARTIAL;
            //            }

            //            await _context.SaveChangesAsync();
            /*Solo para emergencias*/
            return Ok();
        }


        [HttpPut("aprobar/{id}")]
        public async Task<IActionResult> UpdateStatusApproved(Guid id)
        {
            /*if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var order = await _context.RequestsInOrders
                   .Include(x => x.Order)
                   .Include(x => x.Order.Requests)
                   .Include(x => x.Request)
                   .Where(x => x.OrderId == id)
                   .ToListAsync();

            var items = await _context.OrderItems
                .Include(x => x.Supply)
                .Where(x => x.OrderId == id)
                .ToListAsync();

            var requestItems = await _context.RequestItems
                .Include(x => x.Request)
                .Include(x => x.GoalBudgetInput)
                .Where(x => order.FirstOrDefault().Order.Requests.Select(y => y.RequestId).Contains(x.RequestId))
                .ToListAsync();

            var logAuths = await _context.OrderAuthorizations.Where(x => x.OrderId == id
            && x.UserType == ConstantHelpers.Logistics.RequestOrder.UserTypes.AuthOrder).ToListAsync();

            string userId = GetUserId();

            if (logAuths.Select(x => x.UserId).Contains(userId) == false)
                return BadRequest("Usted no tiene los permisos para aprobar esta orden de compra");

            var user = logAuths.FirstOrDefault(x => x.UserId == userId);
            user.IsApproved = true;
            user.ApprovedDate = DateTime.Today;

            if (logAuths.Where(x => x.IsApproved == false).Count() > 0)
                order.FirstOrDefault().Order.Status = ConstantHelpers.Logistics.RequestOrder.Status.APPROVED_PARTIALLY;
            else
                order.FirstOrDefault().Order.Status = ConstantHelpers.Logistics.RequestOrder.Status.APPROVED;

            order.FirstOrDefault().Order.ApproveDate = DateTime.Today;

            if (order.FirstOrDefault().Order.Status == ConstantHelpers.Logistics.RequestOrder.Status.APPROVED)
            {
                foreach (var item in items)
                {
                    var saldo = item.Measure;
                    foreach (var reqItem in requestItems.Where(x => x.GoalBudgetInput.SupplyId == item.SupplyId))
                        if (saldo > reqItem.Measure)
                        {
                            saldo -= reqItem.Measure;
                            reqItem.GoalBudgetInput.CurrentMetered -= reqItem.Measure;
                        }
                        else
                        {
                            reqItem.GoalBudgetInput.CurrentMetered -= saldo;
                            saldo = 0;
                        }
                    item.Supply.Status = ConstantHelpers.Supply.Status.IN_ORDER;
                }
                foreach (var req in order)
                {
                    var reqItems = requestItems.Where(x => x.RequestId == req.RequestId);
                    req.Request.AttentionStatus = ConstantHelpers.Logistics.RequestOrder.AttentionStatus.TOTAL;

                    foreach (var item in reqItems)
                        if (item.Measure != item.MeasureInAttention)
                            req.Request.AttentionStatus = ConstantHelpers.Logistics.RequestOrder.AttentionStatus.PARTIAL;
                }
            }

            //await _context.OrderAuthorizations.AddRangeAsync(orAuths);
            await _context.SaveChangesAsync();*/
            /*Solo para emergencias*/
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.FULL_ACCESS)]
        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var order = await _context.Orders
                .Include(x => x.Requests)
                .FirstOrDefaultAsync(x => x.Id == id);

            var requests = await _context.RequestsInOrders.Where(x => x.OrderId == id)
                .ToListAsync();

            var ordItems = await _context.OrderItems
                .Include(x => x.Supply)
                .Where(x => x.OrderId == id)
                .ToListAsync();

            var requestItems = _context.RequestItems
                .Include(x => x.Request)
                .Include(x => x.Supply)
                .Where(x => order.Requests.Select(y => y.RequestId).Contains(x.RequestId))
                .ToList();

            var items = ordItems
                .Where(x => x.OrderId == id)
                .ToList();

            var orderAuths = _context.OrderAuthorizations
                .Where(x => x.OrderId == id)
                .ToList();

            foreach (var item in items)
            {
                if (ordItems.FirstOrDefault(x => x.SupplyId == item.SupplyId
                 && x.OrderId != item.OrderId) == null)
                    item.Supply.Status = ConstantHelpers.Supply.Status.NO_ORDER;

                var saldo = item.Measure;
                foreach (var reqItem in requestItems.Where(x => x.SupplyId == item.SupplyId && x.Observations == item.Observations))
                    if (reqItem.MeasureInAttention == 0)
                        continue;
                    else if (saldo > reqItem.MeasureInAttention)
                    {
                        saldo -= reqItem.MeasureInAttention;
                        reqItem.MeasureInAttention = 0;
                    }
                    else
                    {
                        reqItem.MeasureInAttention -= saldo;
                        saldo = 0;
                    }
            }

            foreach (var req in order.Requests)
            {
                var completo = true;
                var pendiente = false;

                foreach (var item in requestItems.Where(x => x.RequestId == req.RequestId))
                    if (item.Measure != item.MeasureInAttention)
                    {
                        completo = false;
                        if (item.MeasureInAttention == 0)
                            pendiente = true;
                    }

                if (completo == true)
                    req.Request.AttentionStatus = ConstantHelpers.Logistics.RequestOrder.AttentionStatus.TOTAL;
                else
                {
                    if (pendiente == false)
                        req.Request.AttentionStatus = ConstantHelpers.Logistics.RequestOrder.AttentionStatus.PARTIAL;
                    else
                        req.Request.AttentionStatus = ConstantHelpers.Logistics.RequestOrder.AttentionStatus.PENDING;
                }

                var aux3 = _context.RequestsInOrders.Where(x => x.RequestId == req.RequestId
                && x.OrderId != id).ToList();

                if (aux3.Count() == 0)
                    req.Request.OrderStatus = ConstantHelpers.Logistics.RequestOrder.Status.APPROVED;
            }

            var storage = new CloudStorageService(_storageCredentials);

            if (order.PriceFileUrl != null)
                await storage.TryDelete(order.PriceFileUrl, ConstantHelpers.Storage.Containers.LOGISTICS);

            if (order.SupportFileUrl != null)
                await storage.TryDelete(order.SupportFileUrl, ConstantHelpers.Storage.Containers.LOGISTICS);
            /*
            foreach (var reqId in requests)
            {
                var req = await _context.Requests.FirstOrDefaultAsync(x => x.Id == reqId.RequestId);
                if (_context.RequestsInOrders.Where(x => x.RequestId == req.Id).Count() == 1)
                    req.OrderStatus = ConstantHelpers.Logistics.RequestOrder.Status.APPROVED;
            }
            */
            _context.OrderAuthorizations.RemoveRange(orderAuths);
            _context.OrderItems.RemoveRange(items);
            _context.RequestsInOrders.RemoveRange(requests);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("detalles/{id}")]
        public async Task<IActionResult> GetItem(Guid id)
        {
            var item = await _context.OrderItems
                .Select(x=> new OrderItemDiscountViewModel
                {
                    Id = x.Id,
                    FinancialDiscount = x.FinancialDiscount.ToString("N2", CultureInfo.InvariantCulture),
                    ItemDiscount = x.ItemDiscount.ToString("N2", CultureInfo.InvariantCulture),
                    AdditionalDiscount = x.AdditionalDiscount.ToString("N2", CultureInfo.InvariantCulture),
                    IGV = x.IGV.ToString("N2", CultureInfo.InvariantCulture),
                    ISC = x.ISC.ToString("N2", CultureInfo.InvariantCulture)
                })
                .FirstOrDefaultAsync(x => x.Id == id);

            return Ok(item);
        }

        [HttpPut("descuentos/{id}")]
        public async Task<IActionResult> Discounts(Guid id, OrderItemDiscountViewModel model)
        {
            var item = await _context.OrderItems.FirstOrDefaultAsync(x => x.Id == id);

            if (item == null)
                return BadRequest("No se ha encontrado el item");

            item.FinancialDiscount = model.FinancialDiscount.ToDoubleString();
            item.ItemDiscount = model.ItemDiscount.ToDoubleString();
            item.AdditionalDiscount = model.AdditionalDiscount.ToDoubleString();
            item.IGV = model.IGV.ToDoubleString();
            item.ISC = model.ISC.ToDoubleString();

            item.DiscountedUnitPrice = (100 - item.FinancialDiscount)/100 *
                             (100 - item.ItemDiscount)/100 *
                             (100 - item.AdditionalDiscount)/100 *
                             (100 - item.IGV)/100 *
                             (100 - item.ISC)/100 * item.UnitPrice;

            item.Parcial = item.DiscountedUnitPrice * item.Measure;

            await _context.SaveChangesAsync();
            return Ok();
        }

        #region SERVICIO

        [HttpGet("servicio/listado")]
        public IActionResult ServiceList() => View();

        [HttpGet("servicio/listado/listar")]
        public async Task<IActionResult> GetAllServiceListado(Guid? providerId = null,
            Guid? supplyFamilyId = null, Guid? supplyGroupId = null)
        {
            var search = Request.Query[ConstantHelpers.Datatable.ServerSide.SentParameters.SEARCH_VALUE].ToString();
            var currentNumber = Convert.ToInt32(Request.Query[ConstantHelpers.Datatable.ServerSide.SentParameters.PAGING_FIRST_RECORD]);
            var recordsPerPage = Convert.ToInt32(Request.Query[ConstantHelpers.Datatable.ServerSide.SentParameters.RECORDS_PER_DRAW]);

            var query = _context.Orders
                .Include(x => x.Provider)
                .Include(x => x.Warehouse)
                .Include(x => x.OrderItems)
                .ThenInclude(x => x.Supply)
                .Include(x => x.Requests)
                .ThenInclude(x => x.Request)
                .ThenInclude(x => x.BudgetTitle)
                .Include(x => x.Project)
                .Where(x => x.Type == ConstantHelpers.Logistics.RequestOrder.Type.SERVICE
                && x.Status != ConstantHelpers.Logistics.RequestOrder.Status.PRE_ISSUED && x.Status != ConstantHelpers.Logistics.RequestOrder.Status.OBSERVED
                && x.ProjectId == GetProjectId())
                .AsNoTracking()
                .AsQueryable();

            var usuarios = await _context.Users
                .Include(x => x.WorkAreaEntity)
                .ToListAsync();

            var userRoles = await _context.UserRoles
                .Include(x => x.Role)
                .ToListAsync();

            var totalRecords = query.Count();

            if (providerId.HasValue)
                query = query.Where(x => x.ProviderId == providerId);

            if (supplyFamilyId.HasValue)
                query = query.Where(x => x.OrderItems
                .Where(y => y.Supply.SupplyFamilyId == supplyFamilyId)
                .Any() == true);

            if (supplyGroupId.HasValue)
                query = query.Where(x => x.OrderItems
                .Where(y => y.Supply.SupplyGroupId == supplyGroupId)
                .Any() == true);

            var aux = query
                .ToList();

            if (!string.IsNullOrEmpty(search))
            {
                if (!search.Any(y => Char.IsLetter(y)))
                    aux = aux.Where(x => search == x.CorrelativeCode.ToString()).ToList();
                else if (!search.Any(y => Char.IsNumber(y)))
                    aux = aux.Where(x => search == x.CorrelativeCodeSuffix).ToList();
                else
                    aux = aux.Where(x => new String(search.Where(Char.IsNumber).ToArray()) == x.CorrelativeCode.ToString()
                    && new String(search.Where(Char.IsLetter).ToArray()) == x.CorrelativeCodeSuffix).ToList();
            }

            var count = aux.Count();

            aux = aux
                 .Skip(currentNumber)
                 .Take(recordsPerPage)
                 .ToList();

            var data = aux
                .Select(x => new OrderViewModel
                {
                    Id = x.Id,
                    CorrelativeCode = x.CorrelativeCode,
                    CorrelativeCodeStr = x.Project.CostCenter.ToString() + "-"
                    + x.CorrelativeCode.ToString("D4") + x.CorrelativeCodeSuffix + "-"
                    + x.ReviewDate.Value.Year.ToString(),
                    Provider = new ProviderViewModel
                    {
                        RUC = x.Provider.RUC,
                        BusinessName = x.Provider.BusinessName
                    },
                    RequestsName = string.Join(" / ", x.Requests
                    .Select(y => y.Request.CorrelativePrefix + "-"
                    + y.Request.CorrelativeCode.ToString("D4"))),
                    CostCenter = x.Project.CostCenter.ToString(),
                    Abbreviation = x.Project.Abbreviation.ToString(),
                    BudgetTitle = x.Requests.FirstOrDefault().Request.BudgetTitle.Name,
                    //CostCenter = requests.FirstOrDefault().Request.Project.CostCenter.ToString(),
                    //Abbreviation = requests.FirstOrDefault().Request.Project.Abbreviation,
                    //BudgetTitle = requests.FirstOrDefault().Request.BudgetTitle.Name,
                    QuotationNumber = x.QuotationNumber,
                    Date = x.Date.ToDateString(),
                    DeliveryTime = x.DeliveryTime,
                    Warehouse = new WarehouseViewModel
                    {
                        Address = (x.Warehouse != null ? x.Warehouse.Address : x.ManualWarehouse)
                    },
                    Observations = x.Observations,
                    PriceFileUrl = x.PriceFileUrl,
                    PdfFileUrl = x.PdfFileUrl,
                    SupportFileUrl = x.SupportFileUrl,
                    Status = x.Status,
                    AttentionStatus = x.AttentionStatus,
                    PaymentMethod = x.PaymentMethod,
                    CurrencyStr = ConstantHelpers.Biddings.CURRENCY[x.Currency],
                    Parcial = (x.Currency == ConstantHelpers.Currency.NUEVOS_SOLES) ? x.Parcial.ToString("N2", CultureInfo.InvariantCulture)
                    : ((x.ExchangeRate > 0) ? Math.Round(x.Parcial * x.ExchangeRate, 2).ToString("N2", CultureInfo.InvariantCulture)
                    : 0.ToString("N2", CultureInfo.InvariantCulture)),
                    DolarParcial = (x.Currency == ConstantHelpers.Currency.NUEVOS_SOLES) ? 0.ToString("N2", CultureInfo.InvariantCulture)
                    : (ConstantHelpers.Currency.SIGN_VALUES[x.Currency] + " " + x.Parcial.ToString("N2", CultureInfo.InvariantCulture)),
                    IssuedUserId = x.IssuedUserId
                }).ToList();

            foreach (var order in data)
                if (usuarios.FirstOrDefault(x => x.Id == order.IssuedUserId).WorkAreaEntity.Name == "Logística"
                        && userRoles.FirstOrDefault(x => x.Role.Name == "Oficina Técnica" && x.UserId == order.IssuedUserId) != null)
                    order.CorrelativeCodeStr += "-OB";

            return Ok(new
            {
                draw = ConstantHelpers.Datatable.ServerSide.SentParameters.DRAW_COUNTER,
                recordsTotal = totalRecords,
                recordsFiltered = count,
                data
            });
        }

        [HttpGet("servicio")]
        public IActionResult Service() => View();

        [HttpGet("servicio/listar")]
        public async Task<IActionResult> GetAllService()
        {
            var search = Request.Query[ConstantHelpers.Datatable.ServerSide.SentParameters.SEARCH_VALUE].ToString();
            var currentNumber = Convert.ToInt32(Request.Query[ConstantHelpers.Datatable.ServerSide.SentParameters.PAGING_FIRST_RECORD]);
            var recordsPerPage = Convert.ToInt32(Request.Query[ConstantHelpers.Datatable.ServerSide.SentParameters.RECORDS_PER_DRAW]);

            var pId = GetProjectId();

            var query = _context.Orders
                .Include(x => x.Provider)
                .Include(x => x.Warehouse)
                .Include(x => x.OrderItems)
                .ThenInclude(x => x.Supply)
                .Include(x => x.Requests)
                .ThenInclude(x => x.Request)
                .ThenInclude(x => x.BudgetTitle)
                .Include(x => x.Project)
                .Where(x => x.Type == ConstantHelpers.Logistics.RequestOrder.Type.SERVICE
                && (x.Status == ConstantHelpers.Logistics.RequestOrder.Status.PRE_ISSUED || x.Status == ConstantHelpers.Logistics.RequestOrder.Status.OBSERVED)
                && x.ProjectId == pId)
                .AsNoTracking().AsQueryable();

            var usuarios = await _context.Users
                .Include(x => x.WorkAreaEntity)
                .ToListAsync();

            var userRoles = await _context.UserRoles
                .Include(x => x.Role)
                .ToListAsync();

            var totalRecords = query.Count();

            var aux = query
                .ToList();

            if (!string.IsNullOrEmpty(search))
            {
                if (!search.Any(y => Char.IsLetter(y)))
                    aux = aux.Where(x => search == x.CorrelativeCode.ToString()).ToList();
                else if (!search.Any(y => Char.IsNumber(y)))
                    aux = aux.Where(x => search == x.CorrelativeCodeSuffix).ToList();
                else
                    aux = aux.Where(x => new String(search.Where(Char.IsNumber).ToArray()) == x.CorrelativeCode.ToString()
                    && new String(search.Where(Char.IsLetter).ToArray()) == x.CorrelativeCodeSuffix).ToList();
            }

            var count = aux.Count();

            aux = aux
                 .Skip(currentNumber)
                 .Take(recordsPerPage)
                 .ToList();

            var data = aux
                .Select(x => new OrderViewModel
                {
                    Id = x.Id,
                    CorrelativeCode = x.CorrelativeCode,
                    CorrelativeCodeStr = x.Project.CostCenter.ToString() + "-"
                    + x.CorrelativeCode.ToString("D4") + x.CorrelativeCodeSuffix,
                    Provider = new ProviderViewModel
                    {
                        RUC = x.Provider.RUC,
                        BusinessName = x.Provider.BusinessName
                    },
                    RequestsName = string.Join(" / ", x.Requests
                    .Select(y => y.Request.CorrelativePrefix + "-"
                    + y.Request.CorrelativeCode.ToString("D4"))),
                    CostCenter = x.Project.CostCenter.ToString(),
                    Abbreviation = x.Project.Abbreviation.ToString(),
                    BudgetTitle = x.Requests.FirstOrDefault().Request.BudgetTitle.Name,
                    QuotationNumber = x.QuotationNumber,
                    Date = x.Date.ToDateString(),
                    DeliveryTime = x.DeliveryTime,
                    Warehouse = new WarehouseViewModel
                    {
                        Address = (x.Warehouse != null ? x.Warehouse.Address : x.ManualWarehouse)
                    },
                    Observations = x.Observations,
                    PriceFileUrl = x.PriceFileUrl,
                    PdfFileUrl = x.PdfFileUrl,
                    SupportFileUrl = x.SupportFileUrl,
                    Status = x.Status,
                    AttentionStatus = x.AttentionStatus,
                    PaymentMethod = x.PaymentMethod,
                    CurrencyStr = ConstantHelpers.Biddings.CURRENCY[x.Currency],
                    Parcial = (x.Currency == ConstantHelpers.Currency.NUEVOS_SOLES) ? x.Parcial.ToString("N2", CultureInfo.InvariantCulture)
                    : ((x.ExchangeRate > 0) ? Math.Round(x.Parcial * x.ExchangeRate, 2).ToString("N2", CultureInfo.InvariantCulture)
                    : 0.ToString("N2", CultureInfo.InvariantCulture)),
                    DolarParcial = (x.Currency == ConstantHelpers.Currency.NUEVOS_SOLES) ? 0.ToString("N2", CultureInfo.InvariantCulture)
                    : (ConstantHelpers.Currency.SIGN_VALUES[x.Currency] + " " + x.Parcial.ToString("N2", CultureInfo.InvariantCulture)),
                    IssuedUserId = x.IssuedUserId
                }).ToList();

            foreach (var order in data)
                if (usuarios.FirstOrDefault(x => x.Id == order.IssuedUserId).WorkAreaEntity.Name == "Logística"
                        && userRoles.FirstOrDefault(x => x.Role.Name == "Oficina Técnica" && x.UserId == order.IssuedUserId) != null)
                    order.CorrelativeCodeStr += "-OB";

            return Ok(new
            {
                draw = ConstantHelpers.Datatable.ServerSide.SentParameters.DRAW_COUNTER,
                recordsTotal = totalRecords,
                recordsFiltered = count,
                data
            });
        }

        [HttpGet("servicio/detalles/listar")]
        public async Task<IActionResult> GetDetailService(Guid id)
        {
            var orderItems = await _context.OrderItems
                  .Where(x => x.OrderId == id)
                  .Include(x => x.Order)
                  .Include(x => x.Supply)
                  .Include(x => x.Supply.SupplyFamily)
                  .Include(x => x.Supply.SupplyGroup)
                  .Include(x => x.Supply.MeasurementUnit)
                  .ToListAsync();

            var order = await _context.Orders
                .Include(x => x.Requests)
                .FirstOrDefaultAsync(x => x.Id == id);

            var data = new List<OrderItemViewModel>();

            if (order == null)
                return Ok(data);

            var reqItems = await _context.RequestItems
                .Include(x => x.Supply)
                .Include(x => x.Request)
                .Where(x => order.Requests.Select(y => y.RequestId).Contains(x.RequestId))
                .ToListAsync();

            var observation = "";

            foreach (var item in orderItems)
            {
                var aux = reqItems.Where(x => x.SupplyId == item.SupplyId
                && x.Observations == item.Observations).ToList();
                var list = new List<string>();
                var measure = 0.0;
                var measureInAttention = 0.0;

                foreach (var itemReq in aux)
                {
                    measure += itemReq.Measure;
                    measureInAttention += itemReq.MeasureInAttention;
                    observation = itemReq.Observations;
                }

                foreach (var req in aux.GroupBy(x => x.RequestId))
                {
                    list.Add(req.FirstOrDefault().Request.CorrelativePrefix
                        + "-" + req.FirstOrDefault().Request.CorrelativeCode.ToString("D4"));
                }

                data.Add(new OrderItemViewModel
                {
                    Id = item.Id,
                    Supply = new SupplyViewModel
                    {
                        Description = item.Supply.Description,
                        SupplyFamily = new SupplyFamilyViewModel
                        {
                            Code = item.Supply.SupplyFamily.Code
                        },
                        SupplyGroupId = item.Supply.SupplyGroupId,
                        SupplyGroup = new SupplyGroupViewModel
                        {
                            Code = item.Supply.SupplyGroup.Code
                        },
                        CorrelativeCode = item.Supply.CorrelativeCode,
                        MeasurementUnit = new ViewModels.MeasurementUnitViewModels.MeasurementUnitViewModel
                        {
                            Abbreviation = item.Supply.MeasurementUnit.Abbreviation
                        }
                    },
                    RequestMeasure = measure,
                    RequestMeasureInAttention = measureInAttention,
                    RequestObservation = observation,
                    RequestCode = string.Join(" - ", list),
                    Order = new OrderViewModel
                    {
                        Status = item.Order.Status
                    },
                    Measure = Math.Round(item.Measure, 2).ToString(CultureInfo.InvariantCulture),
                    //UnitPrice = Math.Round(item.UnitPrice, 2).ToString(),
                    UnitPrice = Math.Round(item.UnitPrice, 6).ToString(CultureInfo.InvariantCulture),
                    Parcial = item.Parcial.ToString("N2", CultureInfo.InvariantCulture),
                    Glosa = item.Glosa
                });
            }

            /*
            if (orderItems.Count != 0 && (orderItems.FirstOrDefault().Order.Status == ConstantHelpers.Logistics.RequestOrder.Status.PRE_ISSUED 
                || orderItems.FirstOrDefault().Order.Status == ConstantHelpers.Logistics.RequestOrder.Status.CANCELED))
            {
                foreach (var item in data)
                {
                    item.RequestItem.MeasureInAttention = orderItems.FirstOrDefault(x => x.Id == item.Id).RequestItem.MeasureInAttention;
                }
            }
            */
            return Ok(data);
        }

        [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.FULL_ACCESS)]
        [DisableRequestSizeLimit]
        [HttpPost("servicio/crear")]
        public async Task<IActionResult> CreateService(OrderViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (model.RequestIds.Count == 0)
                return BadRequest("No se ha seleccionado los requerimientos");

            if ((model.Currency == ConstantHelpers.Currency.AMERICAN_DOLLARS || model.Currency == ConstantHelpers.Currency.EUROS) && model.ExchangeRate.ToDoubleString() <= 0)
                return BadRequest("El tipo de cambio no puede ser 0");

            var order = new Order
            {
                //CorrelativeCode = count,
                Type = ConstantHelpers.Logistics.RequestOrder.Type.SERVICE,
                ProviderId = model.ProviderId,
                QuotationNumber = model.QuotationNumber,
                Date = string.IsNullOrEmpty(model.Date) ? DateTime.Today : model.Date.ToDateTime(),
                Currency = model.Currency,
                PaymentMethod = model.PaymentMethod,
                DeliveryTime = model.DeliveryTime,
                WarehouseId = model.WarehouseId,
                BillTo = model.BillTo,
                Warranty = model.Warranty,
                Observations = model.Observations,
                QualityCertificate = model.QualityCertificate,
                Blueprint = model.Blueprint,
                SecurityDocument = model.SecurityDocument,
                CalibrationCertificate = model.CalibrationCertificate,
                CatalogAndStorageCriteria = model.CatalogAndStorageCriteria,
                Other = model.Other,
                OtherDescription = model.OtherDescription,
                Conditions = model.Conditions,
                IssuedUserId = GetUserId(),
                ExchangeRate = model.ExchangeRate.ToDoubleString(),
                ProjectId = GetProjectId()
            };

            var orders = await _context.Orders
                .Where(x => x.Type == ConstantHelpers.Logistics.RequestOrder.Type.SERVICE
                && x.ProjectId == GetProjectId()
                && x.Date.Year == DateTime.Now.Year).ToListAsync();

            if (model.CorrelativeCodeManual == true)
            {
                if (orders.FirstOrDefault(x => x.CorrelativeCode == model.CorrelativeCode
                 && order.CorrelativeCodeSuffix == model.CorrelativeCodeSuffix) == null)
                {
                    order.CorrelativeCode = model.CorrelativeCode.Value;
                    order.CorrelativeCodeSuffix = model.CorrelativeCodeSuffix;
                }
                else
                    BadRequest("La combinación de correlativo y sufijo ya están en uso en este año");
            }
            else
            {
                if (orders.Count() > 0)
                {
                    var count = orders.OrderBy(x => x.CorrelativeCode).Last();
                    order.CorrelativeCode = count.CorrelativeCode + 1;
                }
                else
                {
                    order.CorrelativeCode = 1;
                }
            }

            if (model.PriceFile != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                order.PriceFileUrl = await storage.UploadFile(model.PriceFile.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.LOGISTICS, System.IO.Path.GetExtension(model.PriceFile.FileName), ConstantHelpers.Storage.Blobs.ORDER_PRICE, order.CorrelativeCode.ToString());
            }
            if (model.SupportFile != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                order.SupportFileUrl = await storage.UploadFile(model.SupportFile.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.LOGISTICS, System.IO.Path.GetExtension(model.SupportFile.FileName), ConstantHelpers.Storage.Blobs.ORDER_SUPPORT, order.CorrelativeCode.ToString());
            }

            if (model.WarehouseCheckbox == true)
            {
                order.WarehouseId = null;
                order.ManualWarehouse = model.ManualWarehouse;
            }
            else
            {
                order.ManualWarehouse = null;
                order.WarehouseId = model.WarehouseId;
            }

            var reqs = new List<RequestsInOrder>();

            var orderItems = new List<OrderItem>();

            var total = 0.0;

            var reqItems = await _context.RequestItems
                .Include(x => x.Supply)
                .Where(x => model.RequestIds.Contains(x.RequestId))
                .ToListAsync();

            var approveAuths = await _context.LogisticResponsibles
                .Where(x => x.ProjectId == GetProjectId()
                && x.UserType == ConstantHelpers.Logistics.RequestOrder.UserTypes.OkOrder)
                .ToListAsync();

            var supplies = await _context.Supplies.ToListAsync();

            var requests = await _context.Requests.Where(x => model.RequestIds.Contains(x.Id)).ToListAsync();

            foreach (var stringItem in model.Items)
            {
                var stringSplit = stringItem.Split("|");

                if (stringSplit[1] == "undefined" || stringSplit[2] == "undefined")
                    return BadRequest("Quite lo filtros de la tabla de insumos para guardar");

                if (string.IsNullOrEmpty(stringSplit[1]))
                    return BadRequest("No se ha ingresado todas las cantidades a atender");

                if (string.IsNullOrEmpty(stringSplit[2]))
                    return BadRequest("No se ha ingresado todos los precios unitarios");

                Guid supplyId = Guid.Parse(stringSplit[0]);

                var measure = 0.0;
                var measureAttention = 0.0;
                var toAttend = 0.0;

                var aux = reqItems.Where(x => x.SupplyId == supplyId);

                var cant = stringSplit[1].ToDoubleString();

                var saldo = cant;

                foreach (var item in aux)
                {
                    measure += item.Measure;
                    measureAttention += item.MeasureInAttention;
                    toAttend = item.Measure - item.MeasureInAttention;
                    item.Supply.Status = ConstantHelpers.Supply.Status.IN_ORDER;

                    if (toAttend == 0)
                        continue;
                    else if (saldo > toAttend)
                    {
                        if (aux.Last() == item)
                            return BadRequest("Se está superando el límite atendido");
                        saldo -= toAttend;
                        item.MeasureInAttention += toAttend;
                    }
                    else
                    {
                        item.MeasureInAttention += saldo;
                        saldo = 0;
                    }
                }

                var price = double.Parse(stringSplit[2], CultureInfo.InvariantCulture);

                //if (Double.TryParse(stringSplit[2], out double result))
                //  price = result;

                var glosa = stringSplit[3].ToString();

                if (cant > (measure - measureAttention))
                    return BadRequest("Se ha superado el límite de metrado para atender");

                total += Math.Round(cant * price, 2);

                if (cant > 0)
                    orderItems.Add(new OrderItem
                    {
                        SupplyId = supplyId,
                        Order = order,
                        Measure = cant,
                        UnitPrice = price,
                        Parcial = Math.Round(cant * price, 2),
                        Glosa = glosa,
                        Observations = stringSplit[4].ToString()
                    });
            }

            order.Parcial = Math.Round(total, 2);

            foreach (var id in model.RequestIds)
            {
                var req = await _context.Requests.FirstOrDefaultAsync(x => x.Id == id);
                req.OrderStatus = ConstantHelpers.Logistics.RequestOrder.Status.ORDER_S;
                var reqInOrder = new RequestsInOrder
                {
                    Order = order,
                    RequestId = id
                };
                reqs.Add(reqInOrder);

                var completo = true;
                var pendiente = false;

                foreach (var item in reqItems)
                {
                    if (item.Measure != item.MeasureInAttention)
                    {
                        completo = false;
                        if (item.MeasureInAttention == 0)
                            pendiente = true;
                    }
                }

                if (completo == true)
                    req.AttentionStatus = ConstantHelpers.Logistics.RequestOrder.AttentionStatus.TOTAL;
                else
                {
                    if (pendiente == false)
                        req.AttentionStatus = ConstantHelpers.Logistics.RequestOrder.AttentionStatus.PARTIAL;
                    else
                        req.AttentionStatus = ConstantHelpers.Logistics.RequestOrder.AttentionStatus.PENDING;
                }
            }

            if (model.Liquidation)
            {
                order.ReviewDate = DateTime.Now;
                order.ApproveDate = DateTime.Now;
                order.Status = ConstantHelpers.Logistics.RequestOrder.Status.APPROVED;
                foreach (var item in orderItems)
                {
                    var saldo = item.Measure;
                    //foreach (var reqItem in reqItems.Where(x => x.GoalBudgetInput.SupplyId == item.SupplyId))
                    //    if (saldo > reqItem.Measure)
                    //    {
                    //        saldo -= reqItem.Measure;
                    //        reqItem.GoalBudgetInput.CurrentMetered -= reqItem.Measure;
                    //    }
                    //    else
                    //    {
                    //        reqItem.GoalBudgetInput.CurrentMetered -= saldo;
                    //        saldo = 0;
                    //    }
                    supplies.FirstOrDefault(x => x.Id == item.SupplyId).Status = ConstantHelpers.Supply.Status.IN_ORDER;
                }
                foreach (var req in reqs)
                {
                    var reqItem = reqItems.Where(x => x.RequestId == req.RequestId);
                    requests.FirstOrDefault(x => x.Id == req.RequestId).AttentionStatus = ConstantHelpers.Logistics.RequestOrder.AttentionStatus.TOTAL;

                    foreach (var item in reqItem)
                        if (item.Measure != item.MeasureInAttention)
                            requests.FirstOrDefault(x => x.Id == req.RequestId).AttentionStatus = ConstantHelpers.Logistics.RequestOrder.AttentionStatus.PARTIAL;
                }
                foreach (var authItem in approveAuths)
                {
                    var type = "";
                    if (order.Type == ConstantHelpers.Logistics.RequestOrder.Type.PURCHASE) type = "Compra";
                    else type = "Servicio";
                    var projectName = _context.Projects.FirstOrDefault(x => x.Id == GetProjectId());

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress("sistemaerp@ivc.pe", "Sistema IVC"),
                        Subject = $"Aviso de aprobación de Orden de {type} {projectName.CostCenter.ToString()}-{order.CorrelativeCode.ToString("D4")}{(order.CorrelativeCodeSuffix != null ? order.CorrelativeCodeSuffix : "")}-{order.ReviewDate.Value.Year.ToString()}"
                    };

                    var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == authItem.UserId);

                    mailMessage.To.Add(new MailAddress(user.Email, user.RawFullName));

                    mailMessage.Body =
                       $"Hola, {user.RawFullName}<br /><br /> " +
                       $"Se le informa que la orden de {type} {projectName.CostCenter.ToString()}-{order.CorrelativeCode.ToString("D4")}{(order.CorrelativeCodeSuffix != null ? order.CorrelativeCodeSuffix : "")}-{order.ReviewDate.Value.Year.ToString()} para el proyecto {projectName.Abbreviation} ha sido aprobada mediante liquidación.<br />" +
                       $"<br />" +
                       $"Saludos Cordiales<br /><br />Sistema IVC <br />" +
                       $"Control de Logística";
                    mailMessage.IsBodyHtml = true;
                    //Mandar Correo
                    using (var client = new SmtpClient("smtp.office365.com", 587))
                    {
                        client.UseDefaultCredentials = false;
                        client.Credentials = new NetworkCredential("sistemaerp@ivc.pe", "S1st3m4erp");
                        client.EnableSsl = true;
                        await client.SendMailAsync(mailMessage);
                    }
                }
            }
            await _context.OrderItems.AddRangeAsync(orderItems);
            await _context.RequestsInOrders.AddRangeAsync(reqs);
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            return Ok();
        }



        #endregion

        [HttpPut("cargar-pdf/{id}")]
        public async Task<IActionResult> LoadPdf(Guid id, IFormFile file)
        {
            var order = await _context.Orders
                .Include(x => x.Requests)
                .Include(x => x.Provider)
                .Include(x => x.Project)
                .FirstOrDefaultAsync(x => x.Id == id);

            var reqId = order.Requests.Select(i => i.RequestId).FirstOrDefault();

            var request = await _context.Requests
                .Include(x => x.Project)
                .Include(x => x.ProjectFormula)
                .FirstOrDefaultAsync(x => x.Id == reqId);

            var logRes = await _context.LogisticResponsibles
            .Where(x => x.ProjectId == request.ProjectId)
            .ToListAsync();

            if (order.PdfFileUrl == null && file != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                order.PdfFileUrl = await storage.UploadFile(file.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.LOGISTICS, System.IO.Path.GetExtension(file.FileName),
                    ConstantHelpers.Storage.Blobs.ORDER_PRICE, order.CorrelativeCode.ToString());
            }
            else if (order.PdfFileUrl != null && file != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete(order.PdfFileUrl, ConstantHelpers.Storage.Containers.LOGISTICS);
                order.PdfFileUrl = await storage.UploadFile(file.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.LOGISTICS, System.IO.Path.GetExtension(file.FileName),
                    ConstantHelpers.Storage.Blobs.ORDER_PDF, order.CorrelativeCode.ToString());
            }
            else if (file == null)
                return BadRequest("No se ha cargado el archivo PDF");


            var type = ConstantHelpers.Logistics.RequestOrder.Type.VALUES[order.Type];
            var code = order.Project.CostCenter + "-" + order.CorrelativeCode.ToString("D4") + "-" + order.ReviewDate.Value.Year.ToString();

            var aux = "OC";

            if (order.Type == 2)
                aux = "OS";

            Attachment attachment = new Attachment(file.OpenReadStream(), $"{aux} {order.Provider.Tradename} {code}.pdf");

            var mailMessage = new MailMessage
            {
                From = new MailAddress("sistemaerp@ivc.pe", "Sistema IVC"),
                Subject = $"{order.Requests.FirstOrDefault().Request.CorrelativePrefix} - Aviso de aprobación de Orden de {type} {code}"
            };

            /*
            foreach (var auth in logRes.Where(x => x.UserType == ConstantHelpers.Logistics.RequestOrder.UserTypes.OkOrder))
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == auth.UserId);

                mailMessage.To.Add(new MailAddress(user.Email, user.RawFullName));
            }*/
            mailMessage.To.Add(new MailAddress("henrrypaul_22@hotmail.com", "Henry"));

            mailMessage.Attachments.Add(attachment);

            mailMessage.Body =
                $"Hola, <br /><br /> " +
                $"Se ha aprobado una  Orden de {type} en el centro de costo {order.Project.Abbreviation}.<br />" +
                $"<br />" +
                $"Orden {code}<br />" +
                $"Proveedor {order.Provider.Tradename} <br />" +
                $"Monto {ConstantHelpers.Currency.SIGN_VALUES[order.Currency]} {order.Parcial.ToString("N2", CultureInfo.InvariantCulture)} + IGV<br />" +
                $"<br />" +
                $"Saludos <br />" +
                $"Sistema IVC <br />" +
                $"Control de Logística";
            mailMessage.IsBodyHtml = true;

            //Mandar Correo
            using (var client = new SmtpClient("smtp.office365.com", 587))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("sistemaerp@ivc.pe", "S1st3m4erp");
                client.EnableSsl = true;
                await client.SendMailAsync(mailMessage);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("pdf/{id}")]
        public FileResult ExportPdf(Guid id)
        {
            var order = _context.Orders
                .Include(x => x.Requests)
                .Include(x => x.Provider)
                .Include(x => x.Project)
                .FirstOrDefault(x => x.Id == id);

            if (order == null || order.PdfFileUrl == null)
                return null;

            WebClient client = new WebClient();
            Stream stream = client.OpenRead(order.PdfFileUrl);

            var type = "OC";

            if (order.Type == 2)
                type = "OS";

            return File(stream, "application/pdf", $"{type} {order.Provider.Tradename} {order.Project.CostCenter.ToString()}-{order.CorrelativeCode.ToString("D4")}-{order.ReviewDate.Value.Year.ToString()}.pdf");
        }


        [HttpGet("excel/{id}")]
        public FileResult ExportExcel(Guid id)
        {

            var order = _context.Orders
                .Select(x => new OrderViewModel
                {
                    Id = x.Id,
                    CorrelativeCode = x.CorrelativeCode,
                    ProviderId = x.ProviderId,
                    RequestIds = x.Requests.Select(i => i.RequestId).ToList(),
                    Provider = new ProviderViewModel
                    {
                        RUC = x.Provider.RUC,
                        BusinessName = x.Provider.BusinessName,
                        Tradename = x.Provider.Tradename,
                        Address = x.Provider.Address,
                        CollectionAreaContactName = x.Provider.CollectionAreaContactName,
                        CollectionAreaEmail = x.Provider.CollectionAreaEmail,
                        CollectionAreaPhoneNumber = x.Provider.CollectionAreaPhoneNumber,
                        PhoneNumber = x.Provider.PhoneNumber,
                        BankAccountType = x.Provider.BankAccountType,
                        BankAccountNumber = x.Provider.BankAccountNumber,
                        BankAccountCCI = x.Provider.BankAccountCCI,
                        Bank = new BankViewModel
                        {
                            Name = x.Provider.Bank.Name
                        }
                    },
                    QuotationNumber = x.QuotationNumber,
                    Date = x.Date.ToDateString(),
                    DeliveryTime = x.DeliveryTime,
                    ReviewDate = x.ReviewDate.Value.Year.ToString(),
                    ApproveDate = x.ApproveDate.Value.ToDateString(),
                    Currency = x.Currency,
                    PaymentMethod = x.PaymentMethod,
                    Type = x.Type,
                    Warehouse = new WarehouseViewModel
                    {
                        Address = (x.Warehouse != null ? x.Warehouse.Address : x.ManualWarehouse)
                    },
                    CorrelativeCodeSuffix = x.CorrelativeCodeSuffix,
                    BillTo = x.BillTo,
                    Warranty = x.Warranty,
                    Observations = x.Observations,
                    Status = x.Status,
                    AttentionStatus = x.AttentionStatus,
                    PriceFileUrl = x.PriceFileUrl,
                    SupportFileUrl = x.SupportFileUrl,
                    QualityCertificate = x.QualityCertificate,
                    Blueprint = x.Blueprint,
                    SecurityDocument = x.SecurityDocument,
                    CalibrationCertificate = x.CalibrationCertificate,
                    CatalogAndStorageCriteria = x.CatalogAndStorageCriteria,
                    Other = x.Other,
                    OtherDescription = x.OtherDescription,
                    Conditions = x.Conditions
                }).AsNoTracking()
                .FirstOrDefault(x => x.Id == id);

            var type = "OC";

            if (order.Type == 2)
                type = "OS";

            string fileName = $"{type} {order.Provider.Tradename} {order.CorrelativeCodeStr}.xlsx";

            XLWorkbook wb = ExcelGenerator(order);


            using (MemoryStream stream = new MemoryStream())
            {
                wb.SaveAs(stream);
                stream.Position = 0;
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }

        [HttpGet("file")]
        public Attachment GetFile(Guid id)
        {
            MemoryStream stream = new MemoryStream();

            var order = _context.Orders
                .Select(x => new OrderViewModel
                {
                    Id = x.Id,
                    CorrelativeCode = x.CorrelativeCode,
                    ProviderId = x.ProviderId,
                    RequestIds = x.Requests.Select(i => i.RequestId).ToList(),
                    Provider = new ProviderViewModel
                    {
                        RUC = x.Provider.RUC,
                        BusinessName = x.Provider.BusinessName,
                        Tradename = x.Provider.Tradename,
                        Address = x.Provider.Address,
                        CollectionAreaContactName = x.Provider.CollectionAreaContactName,
                        CollectionAreaEmail = x.Provider.CollectionAreaEmail,
                        CollectionAreaPhoneNumber = x.Provider.CollectionAreaPhoneNumber,
                        PhoneNumber = x.Provider.PhoneNumber,
                        BankAccountType = x.Provider.BankAccountType,
                        BankAccountNumber = x.Provider.BankAccountNumber,
                        BankAccountCCI = x.Provider.BankAccountCCI,
                        Bank = new BankViewModel
                        {
                            Name = x.Provider.Bank.Name
                        }
                    },
                    QuotationNumber = x.QuotationNumber,
                    Date = x.Date.ToDateString(),
                    DeliveryTime = x.DeliveryTime,
                    ReviewDate = x.ReviewDate.Value.Year.ToString(),
                    ApproveDate = x.ApproveDate.Value.ToDateString(),
                    Currency = x.Currency,
                    PaymentMethod = x.PaymentMethod,
                    Type = x.Type,
                    Warehouse = new WarehouseViewModel
                    {
                        Address = (x.Warehouse != null ? x.Warehouse.Address : x.ManualWarehouse)
                    },
                    CorrelativeCodeSuffix = x.CorrelativeCodeSuffix,
                    BillTo = x.BillTo,
                    Warranty = x.Warranty,
                    Observations = x.Observations,
                    Status = x.Status,
                    AttentionStatus = x.AttentionStatus,
                    PriceFileUrl = x.PriceFileUrl,
                    SupportFileUrl = x.SupportFileUrl,
                    QualityCertificate = x.QualityCertificate,
                    Blueprint = x.Blueprint,
                    SecurityDocument = x.SecurityDocument,
                    CalibrationCertificate = x.CalibrationCertificate,
                    CatalogAndStorageCriteria = x.CatalogAndStorageCriteria,
                    Other = x.Other,
                    OtherDescription = x.OtherDescription,
                    Conditions = x.Conditions
                }).AsNoTracking()
                .FirstOrDefault(x => x.Id == id);

            var type = "OC";

            if (order.Type == 2)
                type = "OS";

            string fileName = $"{type} {order.Provider.Tradename} {order.CorrelativeCodeStr}.xlsx";

            XLWorkbook wb = ExcelGenerator(order);
            wb.SaveAs(stream);
            stream.Position = 0;
            stream.Seek(0, SeekOrigin.Begin);


            Attachment attachment = new Attachment(stream, fileName);
            return attachment;
        }

        public XLWorkbook ExcelGenerator(OrderViewModel order)
        {
            var request = _context.Requests
                .Include(x => x.Project)
                .Include(x => x.ProjectFormula)
                .FirstOrDefault(x => x.Id == order.RequestIds.FirstOrDefault());

            order.CorrelativeCodeStr = request.Project.CostCenter.ToString() + "-" + order.CorrelativeCode.Value.ToString("D4");

            if (order.ReviewDate != null)
                order.CorrelativeCodeStr = order.CorrelativeCodeStr + "-" + order.ReviewDate;

            var reqsInOrder = _context.RequestsInOrders
                .Include(x => x.Request)
                .Include(x => x.Request.ProjectFormula)
                .Where(x => x.OrderId == order.Id).ToList();

            var summary = _context.RequestSummaries
                .FirstOrDefault(x => x.ProjectId == request.ProjectId);

            var project = _context.Projects.Include(x => x.Business).FirstOrDefault(x => x.Id == request.ProjectId);

            var items = _context.OrderItems
                .Include(x => x.Supply)
                .Include(x => x.Supply.MeasurementUnit)
                .Include(x => x.Supply)
                .Include(x => x.Supply.SupplyFamily)
                .Include(x => x.Supply.SupplyGroup)
                .Where(x => x.OrderId == order.Id)
                .ToList();

            var responsibles = _context.OrderAuthorizations
                .Where(x => x.OrderId == order.Id)
                .ToList();

            var users = _context.Users.ToList();

            var logRes = new LogisticResponsibleViewModel();

            var enlace = project.LogoUrl.ToString();

            var resPr = responsibles.ToList();
            var ordAuths = new List<string>();
            var ordOks = new List<string>();
            var ordFails = new List<string>();
            var ordReviews = new List<string>();

            foreach (var res in resPr)
            {
                var usName = users.First(x => x.Id == res.UserId).FullName;
                switch (res.UserType)
                {
                    case ConstantHelpers.Logistics.RequestOrder.UserTypes.FailRequest:
                        break;
                    case ConstantHelpers.Logistics.RequestOrder.UserTypes.ReviewRequest:
                        break;
                    case ConstantHelpers.Logistics.RequestOrder.UserTypes.OkRequest:
                        break;
                    case ConstantHelpers.Logistics.RequestOrder.UserTypes.AuthRequest:
                        break;
                    case ConstantHelpers.Logistics.RequestOrder.UserTypes.FailOrder:
                        ordFails.Add(usName);
                        break;
                    case ConstantHelpers.Logistics.RequestOrder.UserTypes.ReviewOrder:
                        ordReviews.Add(usName);
                        break;
                    case ConstantHelpers.Logistics.RequestOrder.UserTypes.OkOrder:
                        ordOks.Add(usName);
                        break;
                    default:
                        ordAuths.Add(usName);
                        break;
                }
            }

            logRes = new LogisticResponsibleViewModel
            {
                ProjectId = request.ProjectId,
                OrderAuthNames = string.Join(" - ", ordAuths),
                OrderOkNames = string.Join(" - ", ordOks),
                OrderFailNames = string.Join(" - ", ordFails),
                OrderReviewNames = string.Join(" - ", ordReviews)
            };
            int y = 22;
            var fila = 2;
            var cantFilas = items.Where(x => string.IsNullOrEmpty(x.Glosa)).Count()
                + (2 * items.Where(x => !string.IsNullOrEmpty(x.Glosa)).Count());

            //var cantPaginas = Math.Ceiling((double)cantFilas / 30);
            double cantFilasTemp = 0;

            var venta = 0.0;
            var igv = 0.0;
            var total = 0.0;
            foreach (var item in items)
            {
                if (string.IsNullOrEmpty(item.Glosa))
                {
                    cantFilasTemp++;
                } else
                {
                    cantFilasTemp += (item.Glosa.Length / 55);
                }
                venta += item.Measure * item.UnitPrice;
            }

            igv = Math.Round(venta * 0.18, 2);

            total = Math.Round(venta + igv, 2);
            var cantPaginas = Math.Ceiling((double)cantFilasTemp / 30);

            var pagina = 1;
            int n = 0;
            int nItems = items.Count();
            IXLWorksheet Cabecera(IXLWorksheet excel)
            {
                WebClient client = new WebClient();
                Stream img = client.OpenRead(enlace);
                Bitmap bitmap;
                bitmap = new Bitmap(img);

                excel.Range($"B{fila}:D{fila + 3}").Merge();

                var aux = excel.AddPicture(bitmap);
                aux.MoveTo(16, y);
                aux.Height = 81;
                aux.Width = 190;

                excel.Cell($"E{fila}").Value = "GESTIÓN DE COMPRAS";
                excel.Range($"E{fila}:K{fila}").Merge();
                excel.Range($"E{fila}:K{fila}").Style.Font.Bold = true;
                //2
                excel.Cell($"E{fila + 1}").Value = "ORDEN DE COMPRA / SERVICIO";
                excel.Range($"E{fila + 1}:K{fila + 3}").Merge();
                excel.Range($"E{fila + 1}:K{fila + 3}").Style.Font.Bold = true;

                excel.Cell($"L{fila}").Value = "Código";
                //excel.Cell($"M2").Value = summary.CodePrefix + "/GCO-For-06";
                excel.Cell($"M{fila}").Value = summary.CodePrefix + "/GCO-For-06";
                excel.Cell($"M{fila}").Style.Font.Bold = true;
                excel.Range($"M{fila}:N{fila}").Merge();

                excel.Cell($"L{fila + 1}").Value = "Versión";
                excel.Cell($"M{fila + 1}").Value = "2";
                excel.Cell($"M{fila + 1}").Style.Font.Bold = true;
                excel.Range($"M{fila + 1}:N{fila + 1}").Merge();

                excel.Cell($"L{fila + 2}").Value = "Fecha";
                excel.Cell($"M{fila + 2}").Value = "18/05/2022";
                excel.Cell($"M{fila + 2}").Style.Font.Bold = true;
                excel.Range($"M{fila + 2}:N{fila + 2}").Merge();

                excel.Cell($"L{fila + 3}").Value = "Página";
                excel.Cell($"M{fila + 3}").SetValue($"{pagina} de {cantPaginas}");
                //pagina = "2 de 2";
                //y += 1533;
                excel.Cell($"M{fila + 3}").Style.Font.Bold = true;
                excel.Range($"M{fila + 3}:N{fila + 3}").Merge();

                excel.Row(fila).Height = 22;
                excel.Row(fila + 1).Height = 15;
                excel.Row(fila + 2).Height = 15;
                excel.Row(fila + 3).Height = 15;

                excel.Range($"B{fila}:N{fila + 3}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                excel.Range($"B{fila}:N{fila + 3}").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                excel.Range($"B{fila}:N{fila + 3}").Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);

                //----------FILA-6-FILA-9---------

                excel.Cell($"B{fila + 4}").Value = "COMPRA";
                excel.Range($"B{fila + 4}:C{fila + 4}").Merge();
                excel.Range($"B{fila + 4}:C{fila + 4}").Style.Font.Bold = true;

                excel.Cell($"B{fila + 5}").Value = "SERVICIO";
                excel.Range($"B{fila + 5}:C{fila + 5}").Merge();
                excel.Range($"B{fila + 5}:C{fila + 5}").Style.Font.Bold = true;

                if (order.Type == 1)
                {
                    excel.Cell($"D{fila + 4}").Value = " ☒ ";
                    excel.Cell($"D{fila + 5}").Value = " ☐ ";
                }
                else
                {
                    excel.Cell($"D{fila + 4}").Value = " ☐ ";
                    excel.Cell($"D{fila + 5}").Value = " ☒ ";
                }

                excel.Row(8).Height = 5;

                excel.Range($"B{fila + 4}:N{fila + 5}").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);

                //----------FILA-9-FILA-13---------

                excel.Cell($"B{fila + 7}").Value = " PROVEEDOR:";
                excel.Range($"B{fila + 7}:C{fila + 7}").Merge();
                excel.Range($"B{fila + 7}:C{fila + 7}").Style.Font.Bold = true;

                excel.Cell($"D{fila + 7}").Value = order.Provider.BusinessName;
                excel.Range($"D{fila + 7}:F{fila + 7}").Merge();

                excel.Cell($"G{fila + 7}").Value = " RUC N°:";
                excel.Cell($"G{fila + 7}").Style.Font.Bold = true;

                excel.Cell($"H{fila + 7}").Value = order.Provider.RUC;
                excel.Range($"H{fila + 7}:I{fila + 7}").Merge();

                excel.Cell($"J{fila + 7}").Value = " N° ORDEN:";
                excel.Range($"J{fila + 7}:K{fila + 7}").Merge();
                excel.Range($"J{fila + 7}:K{fila + 7}").Style.Font.Bold = true;
                var req = _context.Requests.Include(x => x.Project).FirstOrDefault(x => x.Id == order.RequestIds.FirstOrDefault());
                var codeStr = req.Project.CostCenter.ToString() + "-" + order.CorrelativeCode.Value.ToString("D4") + order.CorrelativeCodeSuffix + "-" + order.ReviewDate;

                excel.Cell($"L{fila + 7}").SetValue(codeStr);
                excel.Range($"L{fila + 7}:N{fila + 7}").Merge();


                excel.Cell($"B{fila + 8}").Value = " DIRECCIÓN:";
                excel.Range($"B{fila + 8}:C{fila + 8}").Merge();
                excel.Range($"B{fila + 8}:C{fila + 8}").Style.Font.Bold = true;

                excel.Cell($"D{fila + 8}").Value = order.Provider.Address;
                excel.Range($"D{fila + 8}:I{fila + 8}").Merge();

                excel.Cell($"J{fila + 8}").Value = " COTIZACIÓN N°:";
                excel.Range($"J{fila + 8}:K{fila + 8}").Merge();
                excel.Range($"J{fila + 8}:K{fila + 8}").Style.Font.Bold = true;

                excel.Cell($"L{fila + 8}").Value = order.QuotationNumber;
                excel.Range($"L{fila + 8}:N{fila + 8}").Merge();


                excel.Cell($"B{fila + 9}").Value = " TELÉFONO:";
                excel.Range($"B{fila + 9}:C{fila + 9}").Merge();
                excel.Range($"B{fila + 9}:C{fila + 9}").Style.Font.Bold = true;

                excel.Cell($"D{fila + 9}").Value = order.Provider.PhoneNumber;
                excel.Range($"D{fila + 9}:I{fila + 9}").Merge();

                excel.Cell($"J{fila + 9}").Value = " FECHA DE LA ORDEN:";
                excel.Range($"J{fila + 9}:K{fila + 9}").Merge();
                excel.Range($"J{fila + 9}:K{fila + 9}").Style.Font.Bold = true;

                excel.Cell($"L{fila + 9}").SetValue(order.Date);
                excel.Range($"L{fila + 9}:N{fila + 9}").Merge();


                excel.Cell($"B{fila + 10}").Value = " ATENCIÓN:";
                excel.Range($"B{fila + 10}:C{fila + 10}").Merge();
                excel.Range($"B{fila + 10}:C{fila + 10}").Style.Font.Bold = true;

                excel.Cell($"D{fila + 10}").Value = order.Provider.CollectionAreaContactName + " - REPRESENTANTE DE VENTAS // " + order.Provider.CollectionAreaEmail;
                excel.Range($"D{fila + 10}:I{fila + 10}").Merge();

                excel.Cell($"J{fila + 10}").Value = " CENTRO DE COSTO:";
                excel.Range($"J{fila + 10}:K{fila + 10}").Merge();
                excel.Range($"J{fila + 10}:K{fila + 10}").Style.Font.Bold = true;

                //excel.Cell($"L12").Value = project.CostCenter;
                excel.Cell($"L{fila + 10}").SetValue(project.CostCenter.ToString());
                excel.Range($"L{fila + 10}:N{fila + 10}").Merge();

                excel.Range($"B{fila + 7}:C{fila + 7}").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);

                excel.Range($"B{fila + 7}:N{fila + 10}").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                excel.Range($"B{fila + 8}:N{fila + 10}").Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);

                excel.Range($"B{fila + 7}:N{fila + 10}").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                excel.Range($"B{fila + 8}:N{fila + 10}").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);

                fila += 11;

                excel.Row(fila).Height = 12;
                excel.Range($"B{fila}:M{fila}").Merge();

                excel.Cell($"B{fila + 1}").Value = "N°";
                excel.Cell($"B{fila + 1}").Style.Font.Bold = true;
                excel.Cell($"C{fila + 1}").Value = "CANT.";
                excel.Cell($"C{fila + 1}").Style.Font.Bold = true;
                excel.Cell($"D{fila + 1}").Value = "UND.";
                excel.Cell($"D{fila + 1}").Style.Font.Bold = true;
                excel.Cell($"E{fila + 1}").Value = "CÓDIGO";
                excel.Cell($"E{fila + 1}").Style.Font.Bold = true;
                excel.Cell($"F{fila + 1}").Value = "DESCRIPCIÓN";
                excel.Range($"F{fila + 1}:I{fila + 1}").Merge();
                excel.Range($"F{fila + 1}:I{fila + 1}").Style.Font.Bold = true;
                excel.Cell($"J{fila + 1}").Value = "DOC. ADJUNTA";
                excel.Cell($"J{fila + 1}").Style.Alignment.WrapText = true;
                excel.Cell($"J{fila + 1}").Style.Font.Bold = true;
                excel.Cell($"K{fila + 1}").Value = "P.U.";
                excel.Range($"K{fila + 1}:L{fila + 1}").Merge();
                excel.Range($"K{fila + 1}:L{fila + 1}").Style.Font.Bold = true;
                excel.Cell($"M{fila + 1}").Value = "IMPORTE TOTAL";
                excel.Cell($"M{fila + 1}").Style.Font.Bold = true;
                excel.Range($"M{fila + 1}:N{fila + 1}").Merge();

                excel.Range($"B{fila + 1}:N{fila + 1}").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                excel.Range($"B{fila + 1}:N{fila + 1}").Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
                excel.Range($"B{fila + 1}:N{fila + 1}").Style.Fill.SetBackgroundColor(XLColor.LightGray);

                fila += 2;

                return excel;
            }

            void Tabla(IXLWorkbook wb, int page, string glosa)
            {
                //if (count <= 0) return;
                //var itemsAux = items.Skip((page-1) * 30).Take(30);
                var hojaExcel = wb.Worksheets.Add($"Hoja {page}");

                hojaExcel.ShowGridLines = false;
                var pendiente = "-";
                hojaExcel.Column(1).Width = 1;
                hojaExcel.Column(2).Width = 8;
                hojaExcel.Column(3).Width = 8;
                hojaExcel.Column(4).Width = 10;
                hojaExcel.Column(5).Width = 12;
                hojaExcel.Column(6).Width = 31;
                hojaExcel.Column(7).Width = 7;
                hojaExcel.Column(8).Width = 9;
                hojaExcel.Column(9).Width = 10;
                hojaExcel.Column(10).Width = 20;
                hojaExcel.Column(11).Width = 4;
                hojaExcel.Column(12).Width = 8;
                hojaExcel.Column(13).Width = 20;
                hojaExcel.Column(14).Width = 4;
                hojaExcel.Column(15).Width = 1;

                WebClient client = new WebClient();

                hojaExcel.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                fila = 2;
                hojaExcel = Cabecera(hojaExcel);
                pagina++;
                var cantFilas = 0;

                if (!string.IsNullOrEmpty(glosa))
                {
                    var filasGlosa = (int)Math.Ceiling((double)glosa.Length / 55);
                    var maxCant = 0;
                    var inicio = fila;
                    for (int k = 0; k < filasGlosa; k++)
                    {

                        if (k * 55 + 55 > glosa.Length)
                            maxCant = glosa.Length - k * 55;
                        else
                            maxCant = 55;

                        //glosa.Substring(k * 72, maxCant);
                        hojaExcel.Range($"K${fila}:L${fila}").Merge();
                        hojaExcel.Range($"M${fila}:N${fila}").Merge();
                        fila++;
                        cantFilas++;
                    }
                    hojaExcel.Cell($"F${inicio}").Value = glosa;
                    hojaExcel.Cell($"F${inicio}").Style.Alignment.WrapText = true;
                    hojaExcel.Range($"F${inicio}:I${fila - 1}").Merge();
                }

                //var size = items.Count();

                for (int i = fila; i <= 53; i++)
                {
                    if (i <= 44)
                    {
                        hojaExcel.Cell($"F${i}").Style.Alignment.WrapText = true;
                        hojaExcel.Range($"F${i}:I${i}").Merge();
                    }
                    hojaExcel.Cell($"K${i}").Style.Alignment.WrapText = true;
                    hojaExcel.Range($"K${i}:L${i}").Merge();
                    hojaExcel.Cell($"M${i}").Style.Alignment.WrapText = true;
                    hojaExcel.Range($"M${i}:N${i}").Merge();
                }

                while (cantFilas < 30)
                {
                    if (n == items.Count()) break;
                    var filasGlosa = 1;
                    hojaExcel.Cell($"B${fila}").SetValue((n+1).ToString("D4"));
                    hojaExcel.Cell($"C${fila}").Value = items[n].Measure;
                    hojaExcel.Cell($"D${fila}").Value = items[n].Supply.MeasurementUnit.Abbreviation;
                    hojaExcel.Cell($"E${fila}").SetValue(items[n].Supply.FullCode.ToString());
                    hojaExcel.Cell($"F${fila}").Value = " " + items[n].Supply.Description;
                    hojaExcel.Cell($"F${fila}").Style.Alignment.WrapText = true;
                    hojaExcel.Range($"F${fila}:I${fila}").Merge();
                    hojaExcel.Cell($"J${fila}").Value = "1";
                    hojaExcel.Cell($"K${fila}").SetValue(items[n].UnitPrice.ToString("N2", CultureInfo.InvariantCulture));
                    hojaExcel.Range($"K${fila}:L${fila}").Merge();
                    var importe = items[n].UnitPrice * items[n].Measure;
                    hojaExcel.Cell($"M${fila}").SetValue(importe.ToString("N2", CultureInfo.InvariantCulture));
                    hojaExcel.Range($"M${fila}:N${fila}").Merge();

                    fila++;

                    cantFilas++;
                    nItems--;
                    if (!string.IsNullOrEmpty(items[n].Glosa))
                    {
                        if (items[n].Glosa.Length > 55) //////----->> CAMBIADO BAJO SUPERVISIÓN DE LUCHO
                        {
                            filasGlosa = (int)Math.Ceiling((double)items[n].Glosa.Length / 55);
                            var cantRestanteFilas = 30 - cantFilas;
                            var maxCant = 0;
                            var inicio = fila;
                            if (filasGlosa <= cantRestanteFilas)
                            {
                                for (int k = 0; k < filasGlosa; k++)
                                {
                                    hojaExcel.Range($"K${fila}:L${fila}").Merge();
                                    hojaExcel.Range($"M${fila}:N${fila}").Merge();
                                    cantFilas++;
                                    fila++;
                                }

                                hojaExcel.Cell($"F${inicio}").Value = items[n].Glosa;
                                hojaExcel.Cell($"F${inicio}").Style.Alignment.WrapText = true;
                                hojaExcel.Range($"F${inicio}:I${fila - 1}").Merge();
                                //n++ ; //---> esto deberia estar aqui?
                            }
                            else
                            {
                                //for (int k = 0; k < cantRestanteFilas; k++)
                                //{

                                //    if (k * 72 + 72 > items[n].Glosa.Length)
                                //        maxCant = items[n].Glosa.Length - k * 72;
                                //    else
                                //        maxCant = 72;

                                //    hojaExcel.Cell($"F${fila}").Value = items[n].Glosa.Substring(k * 72, maxCant);
                                //    hojaExcel.Cell($"F${fila}").Style.Alignment.WrapText = true;
                                //    hojaExcel.Range($"F${fila}:I${fila}").Merge();
                                //    hojaExcel.Range($"F${fila}:I${fila}").Merge();
                                //    hojaExcel.Range($"K${fila}:L${fila}").Merge();
                                //    hojaExcel.Range($"M${fila}:N${fila}").Merge();
                                //    fila++;
                                //    cantFilas++;
                                //}


                                for (int k = 0; k < cantRestanteFilas; k++)
                                {
                                    hojaExcel.Range($"K${fila}:L${fila}").Merge();
                                    hojaExcel.Range($"M${fila}:N${fila}").Merge();
                                    cantFilas++;
                                    fila++;
                                }

                                hojaExcel.Cell($"F${inicio}").Value = (items[n].Glosa.Substring(0, cantRestanteFilas * 55)
                                    .Replace(Environment.NewLine, String.Empty)).Replace("\n", String.Empty);
                                hojaExcel.Cell($"F${inicio}").Style.Alignment.WrapText = true;
                                hojaExcel.Range($"F${inicio}:I${fila - 1}").Merge();
                                n++;

                                //igv = Math.Round(venta * 0.18, 2);

                                //total = Math.Round(venta + igv, 2);

                                hojaExcel = Obs(hojaExcel);
                                hojaExcel = Footer(hojaExcel);
                                Tabla(wb, page + 1, items[n - 1].Glosa.Substring(cantRestanteFilas * 55, items[n - 1].Glosa.Length - cantRestanteFilas * 55));
                            }
                        }
                        else
                        {
                            hojaExcel.Cell($"F${fila}").Value = items[n].Glosa;
                            hojaExcel.Cell($"F${fila}").Style.Alignment.WrapText = true;
                            hojaExcel.Range($"F${fila}:I${fila}").Merge();
                            fila++;
                            cantFilas++;
                        }
                        n++;
                    }
                    else
                    {
                        n++;
                    }
                }

                //igv = Math.Round(venta * 0.18, 2);

                //total = Math.Round(venta + igv, 2);

                hojaExcel = Obs(hojaExcel);
                hojaExcel = Footer(hojaExcel);

                if (nItems > 0)
                {
                    Tabla(wb, page + 1, "");
                }

                return;
            };

            IXLWorksheet Obs(IXLWorksheet excel)
            {
                var solicitado = new List<string>();
                var observaciones = new List<string>();
                var formulas = new List<string>();
                var frentes = new List<string>();
                var requerimientos = new List<string>();

                fila = 45;

                excel.Cell($"B{fila}").Value = " CONDICIONES A CUMPLIR:";
                excel.Cell($"B{fila}").Style.Font.Bold = true;
                excel.Range($"B{fila}:J{fila}").Merge();

                excel.Cell($"B{fila + 1}").Value = order.Conditions;
                excel.Range($"B{fila + 1}:J{fila + 3}").Style.Font.FontSize = 7;
                excel.Range($"B{fila + 1}:J{fila + 3}").Merge();
                excel.Range($"B{fila + 1}:J{fila + 3}").Style.Alignment.WrapText = true;
                excel.Row(fila + 1).Height = 17;
                excel.Row(fila + 2).Height = 17;
                excel.Row(fila + 3).Height = 17;
                excel.Cell($"B{fila + 4}").Value = "SOLICITADO";
                excel.Cell($"B{fila + 4}").Style.Font.Bold = true;
                excel.Range($"B{fila + 4}:C{fila + 4}").Merge();

                var reqItems = _context.RequestItems
                   .Include(x => x.PreRequestItem)
                   .Include(x => x.PreRequestItem.PreRequest)
                   .Include(x => x.Supply)
                   .Include(x => x.Supply.SupplyGroup)
                   .Include(x => x.WorkFront)
                   .ToList();

                foreach (var req in reqsInOrder)
                {

                    var regReqs = reqItems.Where(x => x.RequestId == req.RequestId)
                                .ToList();
                    var reqsAux = regReqs.Select(y => y.PreRequestItemId)
                                .ToList();

                    var prerequests = _context.PreRequestItems
                       .Include(x => x.PreRequest.ProjectFormula)
                       .Include(x => x.PreRequest)
                       .Where(x => reqsAux.Contains(x.Id))
                       .ToList();

                    if (prerequests.Count() > 0)
                    {
                        var projectFormulas = prerequests
                            .Select(x => x.PreRequest.ProjectFormula)
                            .Distinct().ToList();
                        var mergeFormula = new List<string>();
                        foreach (var form in projectFormulas)
                        {
                            var auxForm = form.Code + "-" + form.Name;
                            mergeFormula.Add(auxForm);
                        }
                        mergeFormula = mergeFormula.Distinct().ToList();
                        req.Request.ProjectFormula.Code = string.Join("/", mergeFormula);

                        var authsNames = new List<string>();
                        foreach (var pr in prerequests)
                        {
                            var usu = users
                                .FirstOrDefault(x => x.Id == pr.PreRequest.IssuedUserId);
                            authsNames.Add(usu.FullName);
                        }
                        authsNames = authsNames.Distinct().ToList();
                        solicitado.Add(string.Join("/", authsNames));
                        //req.Request.RequestUsernames = string.Join("/", authsNames);
                    }
                    else
                    {

                        //var reqUsers = _context.RequestUsers.Where(x => x.RequestId == req.RequestId).ToList();

                        var reqUsers = req.Request.IssuedUserId;


                        if (!formulas.Contains(req.Request.ProjectFormula.Code))
                            formulas.Add(req.Request.ProjectFormula.Code);

                        requerimientos.Add(req.Request.CorrelativePrefix + "-" + req.Request.CorrelativeCode.ToString("D4"));

                        var user = _context.Users.FirstOrDefault(x => x.Id == reqUsers);

                        foreach(var iter in regReqs)
                        {
                            frentes.Add(iter.WorkFront.Code);
                        }

                        if (!solicitado.Contains(user.PaternalSurname + " " + user.MaternalSurname + ", " + user.Name))
                            solicitado.Add(user.PaternalSurname + " " + user.MaternalSurname + ", " + user.Name);
                    }
                    /*
                    foreach (var userId in reqUsers)
                    {
                        var user = _context.Users.FirstOrDefault(x => x.Id == userId.UserId);
                        if (!solicitado.Contains(user.PaternalSurname + " " + user.MaternalSurname + ", " + user.Name))
                            solicitado.Add(user.PaternalSurname + " " + user.MaternalSurname + ", " + user.Name);
                    }*/
                }

                frentes = frentes.Distinct().ToList();

                excel.Cell($"B{fila + 5}").Value = String.Join(" - ", solicitado);
                excel.Range($"B{fila + 5}:C{fila + 8}").Style.Alignment.WrapText = true;
                excel.Range($"B{fila + 5}:C{fila + 8}").Merge();

                excel.Cell($"D{fila + 4}").Value = "N° de Req.";
                excel.Cell($"D{fila + 4}").Style.Font.Bold = true;

                excel.Cell($"D{fila + 5}").Value = String.Join(" / ", requerimientos);
                excel.Range($"D{fila + 5}:D{fila + 8}").Merge();
                excel.Range($"D{fila + 5}:D{fila + 8}").Style.Alignment.WrapText = true;

                excel.Cell($"E{fila + 4}").Value = "OBSERVACIONES";
                excel.Cell($"E{fila + 4}").Style.Font.Bold = true;
                excel.Range($"E{fila + 4}:H{fila + 4}").Merge();

                //excel.Cell($"E{fila + 5}").Value = String.Join(" - ", observaciones);
                //excel.Range($"E{fila + 5}:H{fila + 6}").Merge();
                //excel.Range($"E{fila + 5}:H{fila + 6}").Style.Alignment.WrapText = true;

                excel.Cell($"E{fila + 5}").Value = order.Observations;
                excel.Range($"E{fila + 5}:H{fila + 8}").Merge();
                excel.Range($"E{fila + 5}:H{fila + 8}").Style.Alignment.WrapText = true;
                //excel.Range($"E{fila + 5}:H{fila + 8}").Rows();

                excel.Cell($"I{fila + 4}").Value = "FÓRMULA";
                excel.Cell($"I{fila + 4}").Style.Font.Bold = true;

                excel.Cell($"I{fila + 5}").Value = String.Join(", ", formulas);
                excel.Range($"I{fila + 5}:I{fila + 8}").Merge();
                excel.Range($"I{fila + 5}:I{fila + 8}").Style.Alignment.WrapText = true;

                excel.Cell($"J{fila + 4}").Value = "FRENTE";
                excel.Cell($"J{fila + 4}").Style.Font.Bold = true;

                excel.Cell($"J{fila + 5}").Value = String.Join(", ", frentes);
                excel.Range($"J{fila + 5}:J{fila + 8}").Merge();
                excel.Range($"J{fila + 5}:J{fila + 8}").Style.Alignment.WrapText = true;

                excel.Range($"B{fila + 4}:J{fila + 4}").Style.Font.FontSize = 8;
                excel.Range($"B{fila + 5}:J{fila + 8}").Style.Font.FontSize = 8;
                excel.Range($"J{fila + 5}:J{fila + 8}").Style.Font.FontSize = 7;

                excel.Range($"B14:N{fila + 8}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                excel.Range($"B14:N{fila + 8}").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                excel.Range($"B14:N{fila + 8}").Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
                excel.Range($"F15:I{fila - 1}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                excel.Range($"L11:N11").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                excel.Row(fila + 9).Height = 5;

                fila = 54;
                return excel;
            }

            IXLWorksheet Firma(IXLWorksheet excel)
            {
                var authUsers = responsibles.Where(x => x.UserType == ConstantHelpers.Logistics.RequestOrder.UserTypes.AuthOrder
                    && x.IsApproved == true).ToList();
                WebClient client = new WebClient();
                Stream imgFirma;
                Bitmap bitmapFirma;
                if (authUsers.Count() != 0)
                {
                    var user = users.FirstOrDefault(x => x.Id == authUsers[0].UserId);

                    imgFirma = client.OpenRead(user.SignatureUrl.ToString());
                    bitmapFirma = new Bitmap(imgFirma);

                    var auxFirma = excel.AddPicture(bitmapFirma);

                    auxFirma.MoveTo(880, 1425);
                    auxFirma.Height = 120;
                    auxFirma.Width = 240;

                    if (authUsers.Count() > 1)
                    {
                        user = users.FirstOrDefault(x => x.Id == authUsers[1].UserId);

                        imgFirma = client.OpenRead(user.SignatureUrl.ToString());
                        bitmapFirma = new Bitmap(imgFirma);

                        var auxFirma2 = excel.AddPicture(bitmapFirma);

                        auxFirma.MoveTo(870, 1415);
                        auxFirma2.MoveTo(985, 1470);

                        auxFirma2.Height = 70;
                        auxFirma2.Width = 140;
                        auxFirma.Height = 70;
                        auxFirma.Width = 140;
                    }
                }

                return excel;
            }

            IXLWorksheet Footer(IXLWorksheet excel)
            {
                //igv = Math.Round(venta * 0.18, 2);
                //total = Math.Round(venta + igv, 2);
                var totalInt = (int)Math.Truncate(total);

                var totalDec = (int)Math.Round(((total - totalInt) * 100));

                var moneda = "SOLES";

                if (order.Currency == 2)
                    moneda = "DÓLARES AMERICANOS";

                //36

                fila += 1;

                excel.Cell($"B{fila}").Value = "SON:";
                excel.Range($"B{fila}:B{fila + 2}").Merge();
                excel.Range($"B{fila}:B{fila + 2}").Style.Font.Bold = true;

                excel.Cell($"C{fila}").SetValue(totalInt.ToStringExcel() + $" CON {totalDec}/100 " + moneda);
                excel.Range($"C{fila}:I{fila + 2}").Merge();
                excel.Range($"C{fila}:I{fila}").Style.Font.FontSize = 10;
                excel.Cell($"J{fila}").Value = "VALOR VENTA";
                excel.Range($"J{fila}:K{fila}").Merge();
                excel.Range($"J{fila}:K{fila}").Style.Font.Bold = true;

                if (order.Currency == 1)
                    excel.Cell($"L{fila}").Value = "S/";
                else
                    excel.Cell($"L{fila}").Value = "US$";

                excel.Cell($"L{fila}").Style.Font.Bold = true;
                excel.Cell($"M{fila}").SetValue(venta.ToString("N2", CultureInfo.InvariantCulture));
                excel.Cell($"M{fila}").Style.Font.Bold = true;
                excel.Range($"M{fila}:N{fila}").Merge();

                excel.Cell($"J{fila + 1}").Value = "IGV (18%)";
                excel.Range($"J{fila + 1}:K{fila + 1}").Merge();
                excel.Range($"J{fila + 1}:K{fila + 1}").Style.Font.Bold = true;

                if (order.Currency == 1)
                    excel.Cell($"L{fila + 1}").Value = "S/";
                else
                    excel.Cell($"L{fila + 1}").Value = "US$";

                excel.Cell($"L{fila + 1}").Style.Font.Bold = true;

                excel.Cell($"M{fila + 1}").SetValue(igv.ToString("N2", CultureInfo.InvariantCulture));
                excel.Cell($"M{fila + 1}").Style.Font.Bold = true;
                excel.Range($"M{fila + 1}:N{fila + 1}").Merge();

                excel.Cell($"J{fila + 2}").Value = "TOTAL";
                excel.Range($"J{fila + 2}:K{fila + 2}").Merge();
                excel.Range($"J{fila + 2}:K{fila + 2}").Style.Font.Bold = true;

                if (order.Currency == 1)
                    excel.Cell($"L{fila + 2}").Value = "S/";
                else
                    excel.Cell($"L{fila + 2}").Value = "US$";
                excel.Cell($"L{fila + 2}").Style.Font.Bold = true;

                excel.Cell($"M{fila + 2}").SetValue(total.ToString("N", CultureInfo.InvariantCulture));
                excel.Cell($"M{fila + 2}").Style.Font.Bold = true;
                excel.Range($"M{fila + 2}:N{fila + 2}").Merge();

                excel.Range($"B{fila}:I{fila + 2}").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);

                excel.Range($"J{fila}:N{fila + 2}").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                excel.Range($"J{fila}:N{fila + 2}").Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);

                //-------------DETALLES------------

                excel.Cell($"B{fila + 3}").Value = " FORMA DE PAGO:";
                excel.Range($"B{fila + 3}:D{fila + 3}").Merge();
                excel.Range($"B{fila + 3}:D{fila + 3}").Style.Font.Bold = true;

                excel.Cell($"E{fila + 3}").Value = order.PaymentMethod;
                excel.Range($"E{fila + 3}:N{fila + 3}").Merge();


                excel.Cell($"B{fila + 4}").Value = " CONDICIONES DE PAGO:";
                excel.Range($"B{fila + 4}:D{fila + 4}").Merge();
                excel.Range($"B{fila + 4}:D{fila + 4}").Style.Font.Bold = true;

                excel.Cell($"E{fila + 4}")
                    .Value = "Se procederá con el pago según las condiciones pactadas debiendo adjuntar: Factura, Guias de Remision y Orden de Compra y/o Servicios.";
                excel.Range($"E{fila + 4}:N{fila + 4}").Merge();


                excel.Cell($"B{fila + 5}").Value = " TIEMPO DE ENTREGA:";
                excel.Range($"B{fila + 5}:D{fila + 5}").Merge();
                excel.Range($"B{fila + 5}:D{fila + 5}").Style.Font.Bold = true;

                excel.Cell($"E{fila + 5}").Value = order.DeliveryTime;
                excel.Range($"E{fila + 5}:N{fila + 5}").Merge();


                excel.Cell($"B{fila + 6}").Value = " LUGAR DE ENTREGA:";
                excel.Range($"B{fila + 6}:D{fila + 6}").Merge();
                excel.Range($"B{fila + 6}:D{fila + 6}").Style.Font.Bold = true;

                excel.Cell($"E{fila + 6}").Value = order.Warehouse.Address;
                excel.Range($"E{fila + 6}:N{fila + 6}").Merge();


                excel.Cell($"B{fila + 7}").Value = " GARANTIA:";
                excel.Range($"B{fila + 7}:D{fila + 7}").Merge();
                excel.Range($"B{fila + 7}:D{fila + 7}").Style.Font.Bold = true;

                excel.Cell($"E{fila + 7}").Value = order.Warranty;
                excel.Range($"E{fila + 7}:N{fila + 7}").Merge();


                excel.Cell($"B{fila + 8}").Value = " FACTURAR A:";
                excel.Range($"B{fila + 8}:D{fila + 9}").Merge();
                excel.Range($"B{fila + 8}:D{fila + 9}").Style.Font.Bold = true;

                excel.Cell($"E{fila + 8}").Value = project.Business.BusinessName;
                excel.Range($"E{fila + 8}:N{fila + 8}").Merge();
                excel.Range($"E{fila + 8}:N{fila + 8}").Style.Font.Bold = true;

                excel.Cell($"E{fila + 9}").Value = "RUC " + project.Business.RUC + " (" + project.Business.Address + ")";
                excel.Range($"E{fila + 9}:N{fila + 9}").Merge();
                excel.Range($"E{fila + 9}:N{fila + 9}").Style.Font.Bold = true;


                excel.Cell($"B{fila + 10}").Value = " LUGAR DE PAGO:";
                excel.Range($"B{fila + 10}:D{fila + 11}").Merge();
                excel.Range($"B{fila + 10}:D{fila + 11}").Style.Font.Bold = true;

                excel.Cell($"E{fila + 10}").Value = "TRANSFERENCIA BANCARIA";
                excel.Range($"E{fila + 10}:N{fila + 10}").Merge();
                excel.Range($"E{fila + 10}:N{fila + 10}").Style.Font.Bold = true;

                //36 - 47 | 11
                var tipoCuenta = "Corriente";

                if (order.Provider.BankAccountType == 2)
                    tipoCuenta = "Ahorros";

                excel.Cell($"E{fila + 11}").Value = order.Provider.Bank.Name + " " + tipoCuenta + " N° " + order.Provider.BankAccountNumber + " -  CCI: " + order.Provider.BankAccountCCI;
                excel.Range($"E{fila + 11}:N{fila + 11}").Merge();
                excel.Range($"E{fila + 11}:N{fila + 11}").Style.Font.Bold = true;


                excel.Cell($"B{fila + 12}").Value = " En la columna DOC. ADJUNTA, marcar el (los) número (s) que correspondan:";
                excel.Range($"B{fila + 12}:E{fila + 13}").Merge();
                excel.Range($"B{fila + 12}:E{fila + 13}").Style.Alignment.WrapText = true;

                excel.Cell($"F{fila + 12}").Value = " 1. Certificados de Calidad";

                if (order.QualityCertificate == true)
                    excel.Cell($"G{fila + 12}").Value = " ☒ ";
                else
                    excel.Cell($"G{fila + 12}").Value = " ☐ ";

                excel.Cell($"F{fila + 13}").Value = " 2. Certificados de Calibración";

                if (order.CalibrationCertificate == true)
                    excel.Cell($"G{fila + 13}").Value = " ☒ ";
                else
                    excel.Cell($"G{fila + 13}").Value = " ☐ ";

                excel.Cell($"H{fila + 12}").Value = " 3. Planos";
                excel.Range($"H{fila + 12}:J{fila + 12}").Merge();

                if (order.Blueprint == true)
                    excel.Cell($"K{fila + 12}").Value = " ☒ ";
                else
                    excel.Cell($"K{fila + 12}").Value = " ☐ ";

                excel.Cell($"H{fila + 13}").Value = " 4. Catálogos y Criterios de Almacenamiento";
                excel.Range($"H{fila + 13}:J{fila + 13}").Merge();

                if (order.CatalogAndStorageCriteria == true)
                    excel.Cell($"K{fila + 13}").Value = " ☒ ";
                else
                    excel.Cell($"K{fila + 13}").Value = " ☐ ";

                excel.Cell($"L{fila + 12}").Value = " 5. Hojas de Seguridad (MSDS)";
                excel.Range($"L{fila + 12}:M{fila + 12}").Merge();

                if (order.SecurityDocument == true)
                    excel.Cell($"N{fila + 12}").Value = " ☒ ";
                else
                    excel.Cell($"N{fila + 12}").Value = " ☐ ";

                excel.Cell($"L{fila + 13}").Value = " 6. Otros: ";
                excel.Range($"L{fila + 13}:M{fila + 13}").Merge();

                if (order.Other == true)
                    excel.Cell($"N{fila + 13}").Value = " ☒ ";
                else
                    excel.Cell($"N{fila + 13}").Value = " ☐ ";


                excel.Range($"B{fila + 3}:N{fila + 7}").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                excel.Range($"B{fila + 3}:N{fila + 7}").Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);

                excel.Range($"B{fila + 8}:N{fila + 9}").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                excel.Range($"B{fila + 8}:D{fila + 9}").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);

                excel.Range($"B{fila + 10}:N{fila + 11}").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                excel.Range($"B{fila + 10}:D{fila + 11}").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);

                excel.Range($"F{fila + 12}:G{fila + 13}").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                excel.Range($"L{fila + 12}:N{fila + 13}").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                excel.Range($"F{fila + 13}:N{fila + 13}").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                excel.Range($"B{fila + 12}:N{fila + 13}").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);

                excel.Range($"L15:N{fila + 2}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                excel.Range($"J{fila}:K{fila + 2}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                excel.Range($"B{fila + 3}:N{fila + 13}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                fila += 14;

                //-------------INFO TECNICA------------

                excel.Cell($"B{fila}").Value = " INFORMACIÓN TÉNICA ADICIONAL OBLIGATORIA";
                excel.Range($"B{fila}:J{fila}").Merge();
                excel.Range($"B{fila}:J{fila}").Style.Font.Bold = true;


                excel.Cell($"K{fila}").Value = "ESTADO";
                excel.Range($"K{fila}:N{fila}").Merge();

                excel.Cell($"B{fila + 1}")
                    .Value = " 1.1 El proveedor indicará en su cotización las normas o especificaciones técnicas aplicables al suministro";
                excel.Range($"B{fila + 1}:J{fila + 1}").Merge();

                excel.Cell($"B{fila + 2}")
                    .Value = " 1.2 El proveedor indicará la procedencia, el nombre del fabricante y el lote de producción en los casos aplicacbles.";
                excel.Range($"B{fila + 2}:J{fila + 2}").Merge();

                excel.Cell($"B{fila + 3}")
                    .Value = " 1.3 El proveedor entregará con el suministro los certificados de calidad.";
                excel.Range($"B{fila + 3}:J{fila + 3}").Merge();

                excel.Cell($"K{fila + 1}").Value = ConstantHelpers.Logistics.RequestOrder.Status.VALUES.Where(x => x.Key == order.Status).Select(x => x.Value);
                excel.Range($"K{fila + 1}:N{fila + 3}").Merge();

                excel.Cell($"B{fila + 4}").Value = " OBSERVACIONES:";
                excel.Range($"B{fila + 4}:J{fila + 4}").Merge();
                excel.Range($"B{fila + 4}:J{fila + 4}").Style.Font.Bold = true;

                excel.Cell($"B{fila + 5}")
                    .Value = " 1. Es obligatorio para la entrega de productos en Almacén o prestación de servicios en Obra, la presetación de:";
                excel.Range($"B{fila + 5}:J{fila + 5}").Merge();

                excel.Cell($"B{fila + 6}")
                    .Value = " - SCTR, Equipo de protección del personal (EPP) y Documento Nacional de identidad (DNI).";
                excel.Range($"B{fila + 6}:J{fila + 6}").Merge();

                excel.Cell($"B{fila + 7}")
                    .Value = " 2. Todo vehículo que ingrese para la atención  de materiales deberá contar con:";
                excel.Range($"B{fila + 7}:J{fila + 7}").Merge();

                excel.Cell($"B{fila + 8}")
                    .Value = " - Tarjeta de propiedad, SOAT vigente y Revisión Técnica vigente.";
                excel.Range($"B{fila + 8}:J{fila + 8}").Merge();

                excel.Cell($"B{fila + 9}")
                    .Value = " - Alarma de retroceso en buenas condiciones, conos de seguridad y tacos de madera.";
                excel.Range($"B{fila + 9}:J{fila + 9}").Merge();

                excel.Cell($"B{fila + 10}")
                    .Value = " - Kit antiderrame y el registro de su último mantenimiento preventivo.";
                excel.Range($"B{fila + 10}:J{fila + 10}").Merge();

                excel.Cell($"B{fila + 11}")
                    .Value = " 3.Se realizará una inducción de 15 minutos referente a las normas de SST y Medio Ambiente antes de su ingreso a Obra";
                excel.Range($"B{fila + 11}:J{fila + 11}").Merge();

                excel.Range($"K{fila + 4}:N{fila + 10}").Merge();

                excel.Cell($"K{fila + 11}").Value = "FIRMA Y SELLO";
                excel.Range($"K{fila + 11}:N{fila + 11}").Merge();


                var enlaceFirma = project.LogoUrl.ToString();
                var authUsers = responsibles.Where(x => x.UserType == ConstantHelpers.Logistics.RequestOrder.UserTypes.AuthOrder
                && x.IsApproved == true).ToList();
                excel = Firma(excel);
                /*
                Stream imgFirma;
                Bitmap bitmapFirma;
                excel = Firma(excel);
                if (authUsers.Count() != 0)
                {
                    var user = users.FirstOrDefault(x => x.Id == authUsers[0].UserId);

                    imgFirma = client.OpenRead(user.SignatureUrl.ToString());
                    bitmapFirma = new Bitmap(imgFirma);

                    var auxFirma = excel.AddPicture(bitmapFirma);

                    auxFirma.MoveTo(870, 1045);
                    auxFirma.Height = 120;
                    auxFirma.Width = 240;

                    if (authUsers.Count() > 1)
                    {
                        user = users.FirstOrDefault(x => x.Id == authUsers[1].UserId);

                        imgFirma = client.OpenRead(user.SignatureUrl.ToString());
                        bitmapFirma = new Bitmap(imgFirma);

                        var auxFirma2 = excel.AddPicture(bitmapFirma);

                        auxFirma.MoveTo(870, 1045);
                        auxFirma2.MoveTo(985, 1100);

                        auxFirma2.Height = 70;
                        auxFirma2.Width = 140;
                        auxFirma.Height = 70;
                        auxFirma.Width = 140;
                    }
                }*/


                excel.Range($"B{fila}:N{fila + 3}").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                excel.Range($"K{fila}:N{fila + 3}").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                excel.Range($"K{fila}:N{fila + 3}").Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);

                excel.Range($"B{fila + 4}:J{fila + 11}").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);

                excel.Range($"K{fila + 4}:N{fila + 10}").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);

                excel.Range($"K{fila + 11}:N{fila + 11}").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                excel.Range($"B{fila}:N{fila + 11}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                excel.Range($"K{fila}:N{fila + 11}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                excel.Range($"G{fila - 2}:G{fila - 1}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                excel.Range($"K{fila - 2}:K{fila - 1}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                excel.Range($"N{fila - 2}:N{fila - 1}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;


                excel.Rows(33, 34).AdjustToContents();
                excel.Row(41).AdjustToContents();

                return excel;
            };

            XLWorkbook wb = new XLWorkbook();
            Tabla(wb, 1, "");

            return wb;
        }

        [HttpGet("ingreso-material/detalles/listar")]
        public async Task<IActionResult> GetAllOrderItemsDetails(Guid ordId)
        {
            var data = await _context.OrderItems
                  .Where(x => x.OrderId == ordId)
                  .Include(x => x.Order)
                  .Include(x => x.Supply)
                  .Include(x => x.Supply.SupplyFamily)
                  .Include(x => x.Supply.SupplyGroup)
                  .Include(x => x.Supply.MeasurementUnit)
                  .Include(x => x.Order.Provider)
                  .Select(x => new OrderItemViewModel
                  {
                      Id = x.Id,
                      Supply = new SupplyViewModel
                      {
                          Description = x.Supply.Description,
                          SupplyFamily = new SupplyFamilyViewModel
                          {
                              Code = x.Supply.SupplyFamily.Code
                          },
                          SupplyGroupId = x.Supply.SupplyGroupId,
                          SupplyGroup = new SupplyGroupViewModel
                          {
                              Code = x.Supply.SupplyGroup.Code
                          },
                          CorrelativeCode = x.Supply.CorrelativeCode,
                          MeasurementUnit = new ViewModels.MeasurementUnitViewModels.MeasurementUnitViewModel
                          {
                              Abbreviation = x.Supply.MeasurementUnit.Abbreviation
                          }
                      },
                      Order = new OrderViewModel
                      {
                          Status = x.Order.Status,
                          Provider = new ProviderViewModel
                          {
                              BusinessName = x.Order.Provider.BusinessName
                          }
                      },
                      Measure = x.Measure.ToString(),
                      UnitPrice = Math.Round(x.UnitPrice, 6).ToString(CultureInfo.InvariantCulture),
                      Parcial = x.Parcial.ToString(),
                      Glosa = x.Glosa,
                      MeasureInAttention = x.MeasureInAttention
                  })
                  .ToListAsync();

            if (data == null)
                return Ok(new List<OrderItemViewModel>());

            return Ok(data);
        }

        [HttpPost("importar")]
        public async Task<IActionResult> Import(IFormFile file)
        {
            // ID = 0b224d20-bebe-4a9c-b16c-34cda18c9741 
            // CAROLINA	ALFARO SPELUCIN
            var pId = GetProjectId();

            var users = await _context.Users
                .ToListAsync();

            var warehouses = await _context.Warehouses
                .Include(x => x.WarehouseType)
                .Where(x => x.WarehouseType.ProjectId == pId)
                .ToListAsync();

            var requests = await _context.Requests
                .Where(x => x.ProjectId == pId)
                .ToListAsync();

            var providers = await _context.Providers
                .ToListAsync();

            var supplies = await _context.Supplies
                .Include(x => x.SupplyFamily)
                .Include(x => x.SupplyGroup)
                .ToListAsync();

            using (var mem = new MemoryStream())
            {
                await file.CopyToAsync(mem);
                using (var workBook = new XLWorkbook(mem))
                {
                    var workSheet = workBook.Worksheets.FirstOrDefault();
                    var counter = 2;

                    var orders = new List<Order>();
                    var items = new List<OrderItem>();
                    var requestInOrders = new List<RequestsInOrder>();
                    var requestInOrder = new RequestsInOrder();
                    var order = new Order();

                    var correlativeAux = 0;
                    var suffixAux = "";

                    while (!workSheet.Cell($"A{counter}").IsEmpty())
                    {
                        var correlativeExcel = workSheet.Cell($"A{counter}").GetString();
                        var correlative = Int32.Parse(correlativeExcel);
                        var suffix = workSheet.Cell($"Q{counter}").GetString();

                        if (correlative != correlativeAux || suffix != suffixAux)
                        {
                            correlativeAux = correlative;
                            suffixAux = suffix;
                            var orderCorrelativeExcel = workSheet.Cell($"D{counter}").GetString();
                            order = new Order();
                            order.Id = Guid.NewGuid();
                            var providerExcel = workSheet.Cell($"B{counter}").GetString();
                            var provider = providers.FirstOrDefault(x => x.RUC.Contains(providerExcel));

                            if (provider == null)
                                return BadRequest($"No se ha encontrao el povedor de la fila {counter}");

                            order.CorrelativeCode = Int32.Parse(orderCorrelativeExcel);
                            order.Type = ConstantHelpers.Logistics.RequestOrder.Type.PURCHASE;
                            order.ProviderId = provider.Id;

                            var warehouse = warehouses.FirstOrDefault(x => x.Address == workSheet.Cell($"M{counter}").GetString());

                            if (warehouse == null)
                                return BadRequest($"no se ha encontrado el almacen de la fila {counter}");

                            order.WarehouseId = warehouse.Id;
                            order.ProjectId = GetProjectId();
                            //order.WarehouseId
                            order.QuotationNumber = workSheet.Cell($"N{counter}").GetString();
                            var currencyType = workSheet.Cell($"E{counter}").GetString();
                            if (currencyType == "Nuevos Soles")
                                order.Currency = ConstantHelpers.Currency.NUEVOS_SOLES;
                            else if (currencyType == "Euros")
                                order.Currency = ConstantHelpers.Currency.EUROS;
                            else
                                order.Currency = ConstantHelpers.Currency.AMERICAN_DOLLARS;
                            order.Date = workSheet.Cell($"F{counter}").GetDateTime();
                            order.PaymentMethod = workSheet.Cell($"G{counter}").GetString();
                            order.DeliveryTime = workSheet.Cell($"O{counter}").GetString();
                            var number2 = 0.0;
                            if(double.TryParse(workSheet.Cell($"P{counter}").GetString(), out number2))
                            {
                                order.ExchangeRate = number2;
                            }
                            order.ApproveDate = workSheet.Cell($"F{counter}").GetDateTime();
                            order.ReviewDate = workSheet.Cell($"F{counter}").GetDateTime();
                            //order.Requests
                            order.OrderItems = new List<OrderItem>();

                            requestInOrder = new RequestsInOrder();
                            var requestExcel = requests.FirstOrDefault(x => x.CorrelativeCode == correlativeAux);
                            requestInOrder.Id = Guid.NewGuid();
                            requestInOrder.RequestId = requestExcel.Id;
                            requestInOrder.OrderId = order.Id;

                            var suffixExcel = workSheet.Cell($"Q{counter}").GetString();

                            if (!string.IsNullOrEmpty(suffixExcel))
                                order.CorrelativeCodeSuffix = suffixExcel;

                            order.Status = ConstantHelpers.Logistics.RequestOrder.Status.APPROVED;

                            var obFlag = Int32.Parse(workSheet.Cell($"R{counter}").GetString());
                            if (obFlag == 1)
                            {
                                order.IssuedUserId = "0b224d20-bebe-4a9c-b16c-34cda18c9741"; // Carolina Alfaro - Tiene OB
                            }
                            else if (obFlag == 0)
                            {
                                order.IssuedUserId = "1b388da3-547d-49ea-bd7c-15bf338ae034"; // Luis Fierro - No tiene OB
                            }
                            else
                            {
                                return BadRequest($"numero de OB inválido en la fila {counter}");
                            }

                            //order.ClosureReason
                            orders.Add(order);
                            requestInOrders.Add(requestInOrder);
                        }

                        var item = new OrderItem();
                        item.Id = Guid.NewGuid();
                        item.OrderId = order.Id;

                        item.Measure = double.Parse(workSheet.Cell($"J{counter}").GetString());
                        var number = 0.0;
                        if (double.TryParse(workSheet.Cell($"K{counter}").GetString(), out number))
                            item.UnitPrice = number;
                        item.Parcial = Math.Round(item.Measure * item.UnitPrice, 2);

                        var supplyExcel = supplies.FirstOrDefault(x => x.FullCode == workSheet.Cell($"H{counter}").GetString());

                        if (supplyExcel == null)
                            return BadRequest($"no se han ecnontrado el isnumo de la fila {counter}");

                        item.SupplyId = supplyExcel.Id;

                        items.Add(item);
                        order.Parcial += item.Parcial;

                        ++counter;
                    }


                    foreach (var auxOrder in orders)
                    {
                        var aux1 = requestInOrders.FirstOrDefault(y => y.OrderId == auxOrder.Id).RequestId;

                        var requestItems = await _context.RequestItems
                        .Include(x => x.Request)
                        .Include(x => x.Supply)
                        .Where(x => x.RequestId == aux1)
                        .ToListAsync();

                        foreach (var item in items.Where(x => x.OrderId == auxOrder.Id))
                        {
                            var saldo = item.Measure;
                            var aux = requestItems.Where(x => x.SupplyId == item.SupplyId).ToList();
                            foreach (var reqItem in aux)
                                if (saldo > reqItem.Measure)
                                {
                                    if (aux.Last() == reqItem)
                                        return BadRequest($"Se está superando el límite atendido del requerimiento {reqItem.Request.CorrelativeCode} de la orden {auxOrder.CorrelativeCode}");
                                    saldo -= reqItem.Measure;
                                    reqItem.MeasureInAttention += reqItem.Measure;
                                }
                                else
                                {
                                    reqItem.MeasureInAttention += saldo;
                                    saldo = 0;
                                }
                        }


                        foreach (var auxOrderItem in items.Where(x => x.OrderId == auxOrder.Id))
                        {
                            var saldo = auxOrderItem.Measure;
                            //foreach (var reqItem in requestItems.Where(x => x.GoalBudgetInput.SupplyId == auxOrderItem.SupplyId))
                            //    if (saldo > reqItem.Measure)
                            //    {
                            //        saldo -= reqItem.Measure;
                            //        reqItem.GoalBudgetInput.CurrentMetered -= reqItem.Measure;
                            //    }
                            //    else
                            //    {
                            //        reqItem.GoalBudgetInput.CurrentMetered -= saldo;
                            //        saldo = 0;
                            //    }
                            var auxSupply = _context.Supplies.FirstOrDefault(x => x.Id == auxOrderItem.SupplyId);
                            auxSupply.Status = ConstantHelpers.Supply.Status.IN_ORDER;
                        }

                        foreach (var req in requestInOrders.Where(x => x.OrderId == auxOrder.Id))
                        {
                            var reqItems = requestItems.Where(x => x.RequestId == req.RequestId);
                            var auxRequest = _context.Requests.FirstOrDefault(x => x.Id == req.RequestId);
                            auxRequest.AttentionStatus = ConstantHelpers.Logistics.RequestOrder.AttentionStatus.TOTAL;

                            foreach (var aux in reqItems)
                                if (aux.Measure != aux.MeasureInAttention)
                                    auxRequest.AttentionStatus = ConstantHelpers.Logistics.RequestOrder.AttentionStatus.PARTIAL;
                        }
                    }

                    await _context.RequestsInOrders.AddRangeAsync(requestInOrders);
                    await _context.OrderItems.AddRangeAsync(items);
                    await _context.Orders.AddRangeAsync(orders);
                    await _context.SaveChangesAsync();
                }
                mem.Close();
            }
            return Ok();
        }

        [HttpGet("exportarLista")]
        public async Task<IActionResult> ExportList()
        {
            var dt = new DataTable("LISTADO DE ORDENES DE COMPRA");
            dt.Columns.Add("N° ORDEN", typeof(string));
            dt.Columns.Add("PROVEEDOR", typeof(string));
            dt.Columns.Add("FECHA", typeof(DateTime));
            dt.Columns.Add("ESTADO", typeof(string));
            dt.Columns.Add("CÓDIGO IVC", typeof(string));
            dt.Columns.Add("DESCRIPCIÓN", typeof(string));
            dt.Columns.Add("CANT.", typeof(string));
            dt.Columns.Add("UNIDAD", typeof(string));
            dt.Columns.Add("P.U.", typeof(double));
            dt.Columns.Add("IMPORTE TOTAL", typeof(double));

            var data = await _context.Orders
                .Include(x => x.Provider)
                .Include(x => x.Warehouse)
                .Include(x => x.Requests)
                .Where(x => x.Type == ConstantHelpers.Logistics.RequestOrder.Type.PURCHASE
                && x.Status != ConstantHelpers.Logistics.RequestOrder.Status.PRE_ISSUED)
                .AsNoTracking()
                .ToListAsync();

            var orderItems = await _context.OrderItems
                  .Include(x => x.Order)
                  .Include(x => x.Order.Provider)
                  .Include(x => x.Supply)
                  .Include(x => x.Supply.SupplyFamily)
                  .Include(x => x.Supply.SupplyGroup)
                  .Include(x => x.Supply.MeasurementUnit)
                  .Where(x => x.Order.Status != ConstantHelpers.Logistics.RequestOrder.Status.PRE_ISSUED)
                  .ToListAsync();

            var status = "";

            var pId = GetProjectId();

            var requests = _context.RequestsInOrders
               .Include(x => x.Request)
               .Include(x => x.Request.Project)
               .Include(x => x.Request.BudgetTitle)
               .Where(x => x.Request.ProjectId == pId)
               .ToList();

            var usuarios = _context.Users
                .Include(x => x.WorkAreaEntity)
                .Include(x => x.UserRoles)
                .Where(x => x.WorkAreaEntity.Name == "Logística" && x.UserRoles.FirstOrDefault(y => y.Role.Name == "Oficina Técnica") != null)
                .Select(x => x.Id)
                .ToList();

            var project = await _context.Projects.FirstOrDefaultAsync(x => x.Id == pId);

            orderItems.ForEach(item =>
            {
                var req = requests.Where(x => x.OrderId == item.OrderId).ToList();

                if (req.Count == 0)
                    return;

                var costCenter = req.FirstOrDefault().Request.Project.CostCenter.ToString();
                var projectName = req.FirstOrDefault().Request.Project.Abbreviation;
                var code = req.FirstOrDefault().Request.Project.CostCenter.ToString()
                        + "-" + item.Order.CorrelativeCode.ToString("D4") + item.Order.CorrelativeCodeSuffix
                        + "-" + item.Order.ReviewDate.Value.Year.ToString();

                if (usuarios.Contains(item.Order.IssuedUserId))
                    code += "-OB";

                var soles = 0.0;
                var dolares = 0.0;
                /*
                if (item.Currency == 1)
                {
                    soles = item.Parcial;
                    if (item.ExchangeRate > 0)
                        dolares = Math.Round(item.Parcial / item.ExchangeRate, 2);
                }
                else
                {
                    soles = Math.Round(item.Parcial * item.ExchangeRate, 2);
                    dolares = item.Parcial;
                }
                */
                switch (item.Order.Status)
                {
                    case ConstantHelpers.Logistics.RequestOrder.Status.PRE_ISSUED:
                        status = "Pre-Emitido";
                        break;
                    case ConstantHelpers.Logistics.RequestOrder.Status.ISSUED:
                        status = "Emitido";
                        break;
                    case ConstantHelpers.Logistics.RequestOrder.Status.APPROVED:
                        status = "Aprobado";
                        break;
                    case ConstantHelpers.Logistics.RequestOrder.Status.CANCELED:
                        status = "Anulado";
                        break;
                    case ConstantHelpers.Logistics.RequestOrder.Status.ORDER_C:
                        status = "O/C Generada";
                        break;
                    case ConstantHelpers.Logistics.RequestOrder.Status.ORDER_S:
                        status = "O/S Generada";
                        break;
                    case ConstantHelpers.Logistics.RequestOrder.Status.OBSERVED:
                        status = "Observado";
                        break;
                    case ConstantHelpers.Logistics.RequestOrder.Status.APPROVED_PARTIALLY:
                        status = "Aprobado Parcialmente";
                        break;
                }
                dt.Rows.Add(code, item.Order.Provider.BusinessName,
                    item.Order.Date, status, item.Supply.FullCode, item.Supply.Description, item.Measure,
                    item.Supply.MeasurementUnit.Abbreviation, Math.Round(item.UnitPrice, 2), Math.Round(item.Parcial, 2));
            });

            var fileName = $"Listado - {project.Abbreviation} - Órdenes de Compra .xlsx";
            using (var wb = new XLWorkbook())
            {
                var workSheet = wb.Worksheets.Add(dt);

                workSheet.Column(1).Width = 18;
                workSheet.Column(2).Width = 40;
                workSheet.Column(3).Width = 10;
                workSheet.Column(4).Width = 12;
                workSheet.Column(5).Width = 13;
                workSheet.Column(6).Width = 55;
                workSheet.Column(7).Width = 8;
                workSheet.Column(8).Width = 8;
                workSheet.Column(9).Width = 9;
                workSheet.Column(10).Width = 12;


                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Seek(0, SeekOrigin.Begin);
                    Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        [HttpGet("exportar")]
        public async Task<IActionResult> Export()
        {
            var dt = new DataTable("LISTADO DE ORDENES DE COMPRA");
            dt.Columns.Add("N° ORDEN", typeof(string));
            dt.Columns.Add("PROVEEDOR", typeof(string));
            dt.Columns.Add("FECHA", typeof(DateTime));
            dt.Columns.Add("ESTADO", typeof(string));
            dt.Columns.Add("CÓDIGO IVC", typeof(string));
            dt.Columns.Add("DESCRIPCIÓN", typeof(string));
            dt.Columns.Add("CANT.", typeof(string));
            dt.Columns.Add("UNIDAD", typeof(string));
            dt.Columns.Add("P.U.", typeof(double));
            dt.Columns.Add("IMPORTE TOTAL", typeof(double));

            var data = await _context.Orders
                .Include(x => x.Provider)
                .Include(x => x.Warehouse)
                .Include(x => x.Requests)
                .Where(x => x.Type == ConstantHelpers.Logistics.RequestOrder.Type.PURCHASE
                && x.Status == ConstantHelpers.Logistics.RequestOrder.Status.PRE_ISSUED && x.ProjectId == GetProjectId())
                .AsNoTracking()
                .ToListAsync();

            var orderItems = await _context.OrderItems
                  .Include(x => x.Order)
                  .Include(x => x.Order.Provider)
                  .Include(x => x.Supply)
                  .Include(x => x.Supply.SupplyFamily)
                  .Include(x => x.Supply.SupplyGroup)
                  .Include(x => x.Supply.MeasurementUnit)
                  .Where(x => x.Order.Status == ConstantHelpers.Logistics.RequestOrder.Status.PRE_ISSUED)
                  .ToListAsync();

            var status = "";

            var pId = GetProjectId();

            var requests = _context.RequestsInOrders
               .Include(x => x.Request)
               .Include(x => x.Request.Project)
               .Include(x => x.Request.BudgetTitle)
               .Where(x => x.Request.ProjectId == pId)
               .ToList();

            var project = await _context.Projects.FirstOrDefaultAsync(x => x.Id == pId);

            orderItems.ForEach(item =>
            {
                var req = requests.Where(x => x.OrderId == item.OrderId).ToList();

                if (req.Count == 0)
                    return;

                var costCenter = req.FirstOrDefault().Request.Project.CostCenter.ToString();
                var projectName = req.FirstOrDefault().Request.Project.Abbreviation;
                var code = req.FirstOrDefault().Request.Project.CostCenter.ToString()
                        + "-" + item.Order.CorrelativeCode.ToString("D4") + item.Order.CorrelativeCodeSuffix
                        + "-" + item.Order.ReviewDate.Value.Year.ToString();
                var soles = 0.0;
                var dolares = 0.0;
                /*
                if (item.Currency == 1)
                {
                    soles = item.Parcial;
                    if (item.ExchangeRate > 0)
                        dolares = Math.Round(item.Parcial / item.ExchangeRate, 2);
                }
                else
                {
                    soles = Math.Round(item.Parcial * item.ExchangeRate, 2);
                    dolares = item.Parcial;
                }
                */
                switch (item.Order.Status)
                {
                    case ConstantHelpers.Logistics.RequestOrder.Status.PRE_ISSUED:
                        status = "Pre-Emitido";
                        break;
                    case ConstantHelpers.Logistics.RequestOrder.Status.ISSUED:
                        status = "Emitido";
                        break;
                    case ConstantHelpers.Logistics.RequestOrder.Status.APPROVED:
                        status = "Aprobado";
                        break;
                    case ConstantHelpers.Logistics.RequestOrder.Status.CANCELED:
                        status = "Anulado";
                        break;
                    case ConstantHelpers.Logistics.RequestOrder.Status.ORDER_C:
                        status = "O/C Generada";
                        break;
                    case ConstantHelpers.Logistics.RequestOrder.Status.ORDER_S:
                        status = "O/S Generada";
                        break;
                    case ConstantHelpers.Logistics.RequestOrder.Status.OBSERVED:
                        status = "Observado";
                        break;
                    case ConstantHelpers.Logistics.RequestOrder.Status.APPROVED_PARTIALLY:
                        status = "Aprobado Parcialmente";
                        break;
                }
                dt.Rows.Add(code, item.Order.Provider.BusinessName,
                    item.Order.Date, status, item.Supply.FullCode, item.Supply.Description, item.Measure,
                    item.Supply.MeasurementUnit.Abbreviation, Math.Round(item.UnitPrice, 2), Math.Round(item.Parcial, 2));
            });

            var fileName = $"Listado Generación - {project.Abbreviation} - Órdenes de Compra .xlsx";
            using (var wb = new XLWorkbook())
            {
                var workSheet = wb.Worksheets.Add(dt);

                workSheet.Column(1).Width = 15;
                workSheet.Column(2).Width = 40;
                workSheet.Column(3).Width = 10;
                workSheet.Column(4).Width = 12;
                workSheet.Column(5).Width = 13;
                workSheet.Column(6).Width = 55;
                workSheet.Column(7).Width = 8;
                workSheet.Column(8).Width = 8;
                workSheet.Column(9).Width = 9;
                workSheet.Column(10).Width = 12;


                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Seek(0, SeekOrigin.Begin);
                    Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        [HttpGet("evaluar/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> AskApproval(Guid id, string userId, bool isApproved)
        {
            var auth = _context.LogisticResponsibles.Where(x => x.UserId == userId);
            var order = _context.Orders.Include(x => x.Requests).Include(x => x.Project).FirstOrDefault(x => x.Id == id);
            var project = _context.Projects.FirstOrDefault(x => x.Id == order.ProjectId);
            var authorizations = await _context.OrderAuthorizations.FirstOrDefaultAsync(x => x.OrderId == id && x.UserId == userId);
            var logAuths = await _context.LogisticResponsibles
                .Where(x => x.ProjectId == order.ProjectId && x.UserType == ConstantHelpers.Logistics.RequestOrder.UserTypes.AuthOrder)
                .CountAsync();
            var type = ConstantHelpers.Logistics.RequestOrder.Type.VALUES[order.Type];
            var items = await _context.OrderItems
                .Include(x => x.Supply)
                .Where(x => x.OrderId == id)
                .ToListAsync();
            var requestItems = await _context.RequestItems
                .Include(x => x.Request)
                .Include(x => x.Supply)
                .Where(x => order.Requests.Select(y => y.RequestId).Contains(x.RequestId))
                .ToListAsync();

            var reqInorder = await _context.RequestsInOrders
                   .Include(x => x.Order)
                   .Include(x => x.Order.Requests)
                   .Include(x => x.Request)
                   .Where(x => x.OrderId == id)
                   .ToListAsync();

            if (authorizations.ApprovedDate != null)
            {
                return RedirectToAction("Index", "Message", new
                {
                    title = $"Error",
                    message = $"Usted ya ha aprobado o rechazado esta orden de {type}",
                    icon = Url.Content("~/media/file_rejected.png")
                });
            }

            var code = order.Project.CostCenter + " - " + order.CorrelativeCode.ToString("D4") + " - " + order.ReviewDate.Value.Year.ToString();
            if (isApproved)
            {
                if (order.Status == ConstantHelpers.Logistics.RequestOrder.Status.ANSWER_PENDING || order.Status == ConstantHelpers.Logistics.RequestOrder.Status.APPROVED_PARTIALLY)
                {
                    authorizations.IsApproved = true;
                    authorizations.ApprovedDate = DateTime.Now;
                    if (order.Status == ConstantHelpers.Logistics.RequestOrder.Status.APPROVED_PARTIALLY)
                    {
                        order.Status = ConstantHelpers.Logistics.RequestOrder.Status.APPROVED;

                        foreach (var item in items)
                        {
                            var saldo = item.Measure;
                            //foreach (var reqItem in requestItems.Where(x => x.GoalBudgetInput.SupplyId == item.SupplyId))
                            //    if (saldo > reqItem.Measure)
                            //    {
                            //        saldo -= reqItem.Measure;
                            //        reqItem.GoalBudgetInput.CurrentMetered -= reqItem.Measure;
                            //    }
                            //    else
                            //    {
                            //        reqItem.GoalBudgetInput.CurrentMetered -= saldo;
                            //        saldo = 0;
                            //    }
                            item.Supply.Status = ConstantHelpers.Supply.Status.IN_ORDER;
                        }
                        foreach (var req in reqInorder)
                        {
                            var reqItems = requestItems.Where(x => x.RequestId == req.RequestId);
                            req.Request.AttentionStatus = ConstantHelpers.Logistics.RequestOrder.AttentionStatus.TOTAL;

                            foreach (var item in reqItems)
                                if (item.Measure != item.MeasureInAttention)
                                    req.Request.AttentionStatus = ConstantHelpers.Logistics.RequestOrder.AttentionStatus.PARTIAL;
                        }

                    }
                    else
                    {
                        if (logAuths == 1)
                        {
                            order.Status = ConstantHelpers.Logistics.RequestOrder.Status.APPROVED;

                            foreach (var item in items)
                            {
                                var saldo = item.Measure;
                                //foreach (var reqItem in requestItems.Where(x => x.GoalBudgetInput.SupplyId == item.SupplyId))
                                //    if (saldo > reqItem.Measure)
                                //    {
                                //        saldo -= reqItem.Measure;
                                //        reqItem.GoalBudgetInput.CurrentMetered -= reqItem.Measure;
                                //    }
                                //    else
                                //    {
                                //        reqItem.GoalBudgetInput.CurrentMetered -= saldo;
                                //        saldo = 0;
                                //    }
                                item.Supply.Status = ConstantHelpers.Supply.Status.IN_ORDER;
                            }
                            foreach (var req in reqInorder)
                            {
                                var reqItems = requestItems.Where(x => x.RequestId == req.RequestId);
                                req.Request.AttentionStatus = ConstantHelpers.Logistics.RequestOrder.AttentionStatus.TOTAL;

                                foreach (var item in reqItems)
                                    if (item.Measure != item.MeasureInAttention)
                                        req.Request.AttentionStatus = ConstantHelpers.Logistics.RequestOrder.AttentionStatus.PARTIAL;
                            }

                        }
                        else
                        {
                            order.Status = ConstantHelpers.Logistics.RequestOrder.Status.APPROVED_PARTIALLY;
                        }
                    }
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Message", new
                    {
                        title = $"Orden de {type} {(isApproved ? "Aprobada" : "Rechazada")}",
                        message = $"La orden de {type} con código {code} para el proyecto {project.Abbreviation} ha sido {(isApproved ? "aprobada" : "rechazada")}.",
                        icon = isApproved ? Url.Content("~/media/file_approved.png") : Url.Content("~/media/file_rejected.png")
                    });
                }
                else
                {
                    authorizations.ApprovedDate = null;
                    authorizations.IsApproved = false;
                    return RedirectToAction("Index", "Message", new
                    {
                        title = $"Error",
                        message = $"La orden de {type} con código {code} para el proyecto {project.Abbreviation} ya ha sido observada.",
                        icon = Url.Content("~/media/file_rejected.png")
                    });
                }
            }

            if (!isApproved)
            {
                if (order.Status == ConstantHelpers.Logistics.RequestOrder.Status.OBSERVED)
                {
                    // el otro tambien tiene que estar en null
                    var author2 = _context.OrderAuthorizations.FirstOrDefault(x => x.OrderId == id && x.UserId != userId);
                    author2.ApprovedDate = null;
                    author2.IsApproved = false;
                    authorizations.ApprovedDate = null;
                    return RedirectToAction("Index", "Message", new
                    {
                        title = $"Error",
                        message = $"La orden de {type} con código {code} para el proyecto {project.Abbreviation} ya ha sido observado.",
                        icon = Url.Content("~/media/file_rejected.png")
                    });
                }
                authorizations.ApprovedDate = DateTime.Now;
                return RedirectToAction("OrderObs", "Message", new
                {
                    title = $"Orden de {type} {(isApproved ? "Aprobada" : "Rechazada")}",
                    message = $"La orden de {type} con código {code} para el proyecto {project.Abbreviation} ha sido {(isApproved ? "aprobada" : "rechazada")}.",
                    icon = isApproved ? Url.Content("~/media/file_approved.png") : Url.Content("~/media/file_rejected.png"),
                    id = id.ToString()
                });
            }
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("enviar-observacion/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> SendObs(Guid id, string observation)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == id);
            order.Status = ConstantHelpers.Logistics.RequestOrder.Status.OBSERVED;
            order.Observations = observation;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar-ordenes-masivo")]
        public async Task<IActionResult> DeleteMassive(IFormFile file)
        {
            using (var mem = new MemoryStream())
            {
                await file.CopyToAsync(mem);
                using (var workBook = new XLWorkbook(mem))
                {
                    var workSheet = workBook.Worksheets.FirstOrDefault();
                    var counter = 2;

                    while (!workSheet.Cell($"A{counter}").IsEmpty())
                    {
                        var correlativeExcel = workSheet.Cell($"A{counter}").GetString();
                        var correlative = Int32.Parse(correlativeExcel);
                        var suffix = workSheet.Cell($"B{counter}").GetString();

                        var order = await _context.Orders
                            .Include(x => x.Requests)
                            .Include(x => x.Warehouse)
                            .ThenInclude(x => x.WarehouseType)
                            .Where(x => x.ProjectId == GetProjectId())
                            .FirstOrDefaultAsync(x => x.CorrelativeCode == correlative 
                            && (!string.IsNullOrEmpty(suffix) ? x.CorrelativeCodeSuffix == suffix : true));

                        if(order == null)
                        {
                            return BadRequest($"No se ha encontrado una orden con el correlativo y el sufijo de la fila {counter}");
                        }

                        var requests = await _context.RequestsInOrders.Where(x => x.OrderId == order.Id)
                            .ToListAsync();

                        var ordItems = await _context.OrderItems
                            .Include(x => x.Supply)
                            .Where(x => x.OrderId == order.Id)
                            .ToListAsync();

                        var requestItems = _context.RequestItems
                            .Include(x => x.Request)
                            .Include(x => x.Supply)
                            .Where(x => order.Requests.Select(y => y.RequestId).Contains(x.RequestId))
                            .ToList();

                        var items = ordItems
                            .Where(x => x.OrderId == order.Id)
                            .ToList();

                        var orderAuths = _context.OrderAuthorizations.Where(x => x.OrderId == order.Id).ToList();

                        foreach (var item in items)
                        {
                            if (ordItems.FirstOrDefault(x => x.SupplyId == item.SupplyId
                             && x.OrderId != item.OrderId) == null)
                                item.Supply.Status = ConstantHelpers.Supply.Status.NO_ORDER;

                            var saldo = item.Measure;
                            foreach (var reqItem in requestItems.Where(x => x.SupplyId == item.SupplyId))
                                if (saldo > reqItem.Measure)
                                {
                                    saldo -= reqItem.MeasureInAttention;
                                    reqItem.MeasureInAttention = 0;
                                }
                                else
                                {
                                    reqItem.MeasureInAttention -= saldo;
                                    saldo = 0;
                                }
                        }


                        foreach (var req in order.Requests)
                        {
                            var completo = true;
                            var pendiente = false;

                            foreach (var item in requestItems.Where(x => x.RequestId == req.RequestId))
                                if (item.Measure != item.MeasureInAttention)
                                {
                                    completo = false;
                                    if (item.MeasureInAttention == 0)
                                        pendiente = true;
                                }
                            var reqAux = _context.Requests.FirstOrDefault(x => x.Id == req.RequestId);
                            if (completo == true)
                                reqAux.AttentionStatus = ConstantHelpers.Logistics.RequestOrder.AttentionStatus.TOTAL;
                            else
                            {
                                if (pendiente == false)
                                    reqAux.AttentionStatus = ConstantHelpers.Logistics.RequestOrder.AttentionStatus.PARTIAL;
                                else
                                    reqAux.AttentionStatus = ConstantHelpers.Logistics.RequestOrder.AttentionStatus.PENDING;
                            }

                            if (_context.RequestsInOrders.Where(x => x.RequestId == req.Id).Count() == 1)
                                reqAux.OrderStatus = ConstantHelpers.Logistics.RequestOrder.Status.APPROVED;
                        }

                        var storage = new CloudStorageService(_storageCredentials);

                        if (order.PriceFileUrl != null)
                            await storage.TryDelete(order.PriceFileUrl, ConstantHelpers.Storage.Containers.LOGISTICS);

                        if (order.SupportFileUrl != null)
                            await storage.TryDelete(order.SupportFileUrl, ConstantHelpers.Storage.Containers.LOGISTICS);

                        _context.OrderItems.RemoveRange(items);
                        _context.RequestsInOrders.RemoveRange(requests);
                        _context.OrderAuthorizations.RemoveRange(orderAuths);
                        _context.Orders.Remove(order);
                        await _context.SaveChangesAsync();
                        counter++;
                    }
                }
            }
            return Ok();
        }
    }
}