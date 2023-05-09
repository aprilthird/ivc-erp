using ClosedXML.Excel;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Warehouse;
using IVC.PE.WEB.Areas.Accounting.ViewModels.InvoiceViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.MeasurementUnitViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.OrderViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.ProviderViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.RequestViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyFamilyViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyGroupViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.GoalBudgetInputViewModels;
using IVC.PE.WEB.Areas.Warehouse.ViewModels.SupplyEntryViewModels;
using IVC.PE.WEB.Areas.Warehouse.ViewModels.WarehouseViewModels;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Options;
using IVC.PE.WEB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using QRCoder;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Warehouse.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Warehouse.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.WAREHOUSE)]
    [Route("almacenes/ingreso-material")]
    public class SupplyEntryController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        public SupplyEntryController(IvcDbContext context,
            IOptions<CloudStorageCredentials> storageCredentials,
            ILogger<SupplyEntryController> logger) : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? providerId = null, Guid? supplyGroupId = null, Guid? supplyFamilyId = null, int fileStatus = 0)
        {
            var query = _context.SupplyEntries
                .Include(x => x.Order)
                .Include(x => x.Warehouse)
                .Include(x => x.Warehouse.WarehouseType)
                .Include(x => x.Order.Provider)
                .Include(x => x.Order.Warehouse.WarehouseType.Project)
                .Where(x => x.Warehouse.WarehouseType.ProjectId == GetProjectId());

            if (providerId.HasValue)
                query = query.Where(x => x.Order.ProviderId == providerId);

            //if(supplyGroupId.HasValue)

            var items = await _context.SupplyEntryItems
                .Include(x => x.OrderItem.Supply)
                .Include(x => x.OrderItem.Supply.SupplyGroup)
                .ToListAsync();

            var data = new List<SupplyEntryViewModel>();

            foreach (var entry in query)
            {
                var entryItems = items.Where(x => x.SupplyEntryId == entry.Id).ToList();

                //var grupo = entryItems.FirstOrDefault(x => x.OrderItem.Supply.SupplyGroupId == supplyGroupId);

                var groups = entryItems.Select(x => x.OrderItem.Supply.SupplyGroupId);
                var families = entryItems.Select(x => x.OrderItem.Supply.SupplyFamilyId);

                var soles = 0.0;
                var dolares = 0.0;

                if (entry.Order.Currency == 1)
                {
                    soles = entry.Parcial;
                    dolares = Math.Round(entry.Parcial * entry.Order.ExchangeRate, 2);
                }
                else
                {
                    soles = Math.Round(entry.Parcial * entry.Order.ExchangeRate, 2);
                    dolares = entry.Parcial;
                }

                if ((!supplyFamilyId.HasValue && supplyGroupId.HasValue && groups.Contains(supplyGroupId) == true) 
                    || (!supplyGroupId.HasValue && supplyFamilyId.HasValue && families.Contains((Guid)supplyFamilyId) == true)
                    || (supplyGroupId.HasValue && supplyFamilyId.HasValue
                    && groups.Contains(supplyGroupId) == true && families.Contains((Guid)supplyFamilyId) == true)
                    || (!supplyGroupId.HasValue && !supplyFamilyId.HasValue))
                    data.Add(new SupplyEntryViewModel
                    {
                        Id = entry.Id,
                        DocumentNumber = entry.DocumentNumber.ToString("D4"),
                        DeliveryDate = entry.DeliveryDate.ToDateString(),
                        RemissionGuideUrl = entry.RemissionGuideUrl,
                        Warehouse = new WarehouseViewModel
                        {
                            Address = entry.Warehouse.Address
                        },
                        RemissionGuideName = entry.RemissionGuide,
                        Order = new OrderViewModel
                        {
                            CorrelativeCode = entry.Order.CorrelativeCode,
                            CorrelativeCodeStr = entry.Order.Warehouse.WarehouseType.Project.CostCenter.ToString()
                        + "-" + entry.Order.CorrelativeCode.ToString("D4")
                        + "-" + entry.Order.ReviewDate.Value.Year.ToString(),
                            Date = entry.Order.Date.ToDateString(),
                            Provider = new ProviderViewModel
                            {
                                BusinessName = entry.Order.Provider.BusinessName
                            }
                        },
                        Parcial = soles,
                        DolarParcial = dolares,
                        ParcialString = soles.ToString("N2", CultureInfo.InvariantCulture),
                        DolarParcialString = dolares.ToString("N2", CultureInfo.InvariantCulture),
                        Status = entry.Status,
                        Groups = string.Join("-",
                            //items.Where(x => x.SupplyEntryId == entry.Id)
                            //.GroupBy(entry => entry.OrderItem.Supply.SupplyGroup.Id)
                            entryItems
                            .Select(x => x.OrderItem.Supply.SupplyGroup.Name)
                            .Distinct()
                            .ToList())
                        
                    });
            }
            if(fileStatus == 1)
                data = data.Where(x => x.RemissionGuideUrl != null).ToList();
            else if (fileStatus == 2)
                data = data.Where(x => x.RemissionGuideUrl == null).ToList();
            /*
            var entries = await query
                .Select(x => new SupplyEntryViewModel
                {
                    Id = x.Id,
                    DocumentNumber = x.DocumentNumber.ToString("D4"),
                    DeliveryDate = x.DeliveryDate.ToDateString(),
                    RemissionGuide = x.RemissionGuide,
                    Warehouse = new WarehouseViewModel
                    {
                        Address = x.Warehouse.Address
                    },
                    Provider = new ProviderViewModel
                    {
                        BusinessName = x.Provider.BusinessName
                    },
                    Order = new OrderViewModel
                    {
                        CorrelativeCode = x.Order.CorrelativeCode,
                        CorrelativeCodeStr = x.Order.Requests.FirstOrDefault().Request.Project.CostCenter.ToString()
                        + "-" + x.Order.CorrelativeCode.ToString("D4")
                        + "-" + x.Order.ReviewDate.Value.Year.ToString(),
                        Date = x.Order.Date.ToDateString()
                    },
                    Groups = string.Join("-",
                    items.Where(y => y.SupplyEntryId == x.Id)
                    .Select(x => x.OrderItem.GoalBudgetInput.Supply.SupplyGroup.Name)
                    .ToList())
                }).ToListAsync();
            */
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var entry = await _context.SupplyEntries
                .Include(x => x.Warehouse)
                .Include(x => x.Order)
                .Include(x => x.Order.Requests)
                .Include(x => x.Order.Provider)
                .Select(x => new SupplyEntryViewModel
                {
                    Id = x.Id,
                    DocumentNumber = x.DocumentNumber.ToString("D4"),
                    DeliveryDate = x.DeliveryDate.ToDateString(),
                    RemissionGuideUrl = x.RemissionGuideUrl,
                    OrderId = x.OrderId,
                    Order = new OrderViewModel
                    {
                        Provider = new ProviderViewModel
                        {
                            BusinessName = x.Order.Provider.BusinessName
                        },
                    },
                    WarehouseId = x.WarehouseId,
                    RemissionGuideName = x.RemissionGuide,
                    Invoice = new InvoiceViewModel
                    {
                        Id = x.InvoiceId,
                        FileUrl = x.Invoice.FileUrl
                    }
                }).FirstOrDefaultAsync(x => x.Id == id);

            return Ok(entry);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(SupplyEntryViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var count = 0;

            if (_context.SupplyEntries.Count() != 0)
                count = _context.SupplyEntries.OrderBy(x => x.DocumentNumber).Last().DocumentNumber;

            var entry = new SupplyEntry
            {
                DocumentNumber = count + 1,
                OrderId = model.OrderId,
                WarehouseId = model.WarehouseId,
                DeliveryDate = model.DeliveryDate.ToDateTime(),
                RemissionGuide = model.RemissionGuideName,
                Status = ConstantHelpers.Warehouse.SupplyEntry.Status.INPROCESS,
                InvoiceId = model.InvoiceId
            };

            if (model.RemissionGuide != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                entry.RemissionGuideUrl = await storage.UploadFile(model.RemissionGuide.OpenReadStream(),
                     ConstantHelpers.Storage.Containers.WAREHOUSE,
                     System.IO.Path.GetExtension(model.RemissionGuide.FileName),
                     ConstantHelpers.Storage.Blobs.SUPPLY_ENTRY,
                     $"Ingreso-Material{entry.DocumentNumber.ToString("D4")}-{entry.DeliveryDate.ToDateString()}");
            }

            var items = new List<SupplyEntryItem>();

            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == model.OrderId);

            var ordItems = await _context.OrderItems
                .Where(x => x.OrderId == model.OrderId)
                .ToListAsync();

            var total = 0.0;

            var isTotal = true;

            foreach (var stringItem in model.Items)
            {
                var stringSplit = stringItem.Split("|");
                Guid ordId = Guid.Parse(stringSplit[0]);

                if (stringSplit[1] == "undefined" || stringSplit[2] == "undefined")
                    return BadRequest("Quite lo filtros de la tabla de insumos para guardar");

                var ordItem =ordItems.FirstOrDefault(x => x.Id == ordId);

                if (ordItem == null)
                    return BadRequest("No se encontró el item");

                var cant = stringSplit[1].ToDoubleString();
                var obs = stringSplit[2];

                total += (cant * ordItem.UnitPrice);

                /*
                                var price = stringSplit[2].ToDoubleString();
                                var glosa = stringSplit[3].ToString();

                                if (cant > (reqItem.Measure - reqItem.MeasureInAttention))
                                    return BadRequest("Se ha superado el límite de metrado para atender");
                                */

                var limite = ordItem.Measure - ordItem.MeasureInAttention;

                if (cant > limite)
                    return BadRequest("Se ha superado el límite de lo requerido.");

                if (cant > 0)
                    items.Add(new SupplyEntryItem
                    {
                        SupplyEntry = entry,
                        OrderItemId = ordId,
                        Measure = cant,
                        Observations = obs,
                        PreviousAttention = ordItem.MeasureInAttention
                    });

                ordItem.MeasureInAttention += cant;

                if (ordItem.Measure != ordItem.MeasureInAttention)
                    isTotal = false;
            }

            if (isTotal == false)
                order.AttentionStatus = ConstantHelpers.Logistics.RequestOrder.AttentionStatus.PARTIAL;
            else
                order.AttentionStatus = ConstantHelpers.Logistics.RequestOrder.AttentionStatus.TOTAL;

            entry.Parcial = Math.Round(total, 2);

            await _context.SupplyEntries.AddAsync(entry);
            await _context.SupplyEntryItems.AddRangeAsync(items);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, SupplyEntryViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entry = await _context.SupplyEntries.FirstOrDefaultAsync(x => x.Id == id);

            if (entry.Status == ConstantHelpers.Warehouse.SupplyEntry.Status.CONFIRMED)
                return BadRequest("El ingreso ya ha sido confirmado");

            entry.OrderId = model.OrderId;
            entry.WarehouseId = model.WarehouseId;
            entry.DeliveryDate = model.DeliveryDate.ToDateTime();
            entry.RemissionGuide = model.RemissionGuideName;
            entry.InvoiceId = model.InvoiceId;

            if (model.RemissionGuide != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (entry.RemissionGuideUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.SUPPLY_ENTRY}/{entry.RemissionGuideUrl.AbsolutePath.Split('/').Last()}",
                        ConstantHelpers.Storage.Containers.WAREHOUSE);
                entry.RemissionGuideUrl = await storage.UploadFile(model.RemissionGuide.OpenReadStream(),
                     ConstantHelpers.Storage.Containers.WAREHOUSE,
                     System.IO.Path.GetExtension(model.RemissionGuide.FileName),
                     ConstantHelpers.Storage.Blobs.SUPPLY_ENTRY,
                    $"Ingreso-Material{entry.DocumentNumber.ToString("D4")}-{entry.DeliveryDate.ToDateString()}");
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entry = await _context.SupplyEntries.FirstOrDefaultAsync(x => x.Id == id);

            if (entry.Status == ConstantHelpers.Warehouse.SupplyEntry.Status.CONFIRMED)
                return BadRequest("El ingreso ya ha sido confirmado");

            var items = await _context.SupplyEntryItems.Where(x => x.SupplyEntryId == id).ToListAsync();

            var ordItems = await _context.OrderItems.ToListAsync();

            if (entry == null)
                return BadRequest("No se ha encontrado el ingreso al almacen");

            foreach (var item in items)
            {
                var aux = ordItems.FirstOrDefault(x => x.Id == item.OrderItemId);
                aux.MeasureInAttention -= item.Measure;
            }


            _context.SupplyEntryItems.RemoveRange(items);
            _context.SupplyEntries.Remove(entry);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("confirmar/{id}")]
        public async Task<IActionResult> Confirm(Guid id)
        {
            var entry = await _context.SupplyEntries
                .Include(x => x.Order)
                .Include(x => x.Order.Provider)
                .Include(x => x.Order.Warehouse.WarehouseType.Project)
                .FirstOrDefaultAsync(x => x.Id == id);

            var pId = GetProjectId();

            var correlative = await _context.RequestSummaries
               .Where(x => x.ProjectId == pId)
               .FirstOrDefaultAsync();
            
            if (entry.Status == ConstantHelpers.Warehouse.SupplyEntry.Status.CONFIRMED)
                return BadRequest("El ingreso ya ha sido confirmado");

            if(entry.RemissionGuide == "")
                return BadRequest("No se ha ingresado el nombre de la guía de remisión");

            entry.Status = ConstantHelpers.Warehouse.SupplyEntry.Status.CONFIRMED;

            await _context.SaveChangesAsync();
            
            var responsibles = await _context.WarehouseResponsibles
                .Where(x => x.UserType == ConstantHelpers.Warehouse.UserTypes.ThecnicalOfficeControl
                || x.UserType == ConstantHelpers.Warehouse.UserTypes.StoreKeepers)
                .Where(x => x.ProjectId == pId)
                .Select(x => x.UserId)
                .Distinct()
                .ToListAsync();

            var entryitems = await _context.SupplyEntryItems
                .Include(x => x.OrderItem)
                .Include(x => x.SupplyEntry)
                .Include(x => x.SupplyEntry.Order)
                .Include(x => x.OrderItem.Supply)
                .Include(x => x.OrderItem.Supply.SupplyGroup)
                .Include(x => x.OrderItem.Supply.SupplyFamily)
                .Include(x => x.OrderItem.Supply.MeasurementUnit)
                .ToListAsync();

            var items = entryitems
                .Where(x => x.SupplyEntryId == id)
                .ToList();

            var stock = await _context.Stocks
                .Include(x => x.Supply)
                .ToListAsync();

            var aux = entryitems
                .Where(x => x.SupplyEntry.Status == ConstantHelpers.Warehouse.SupplyEntry.Status.CONFIRMED)
                .ToList();

            var newStock = new List<Stock>();

            var listMessage = "";

            foreach (var item in items)
            {
                var existe = stock.FirstOrDefault(x => x.SupplyId == item.OrderItem.SupplyId);
                var auxItems = aux.Where(x => x.OrderItem.SupplyId == item.OrderItem.SupplyId).ToList();

                var sumaProducto = 0.0;
                var dividendo = 0.0;
                var divisor = 0.0;

                foreach (var auxItem in auxItems)
                {
                    if (auxItem.SupplyEntry.Order.Currency == ConstantHelpers.Currency.NUEVOS_SOLES)
                    {
                        dividendo += (auxItem.Measure * auxItem.OrderItem.UnitPrice);
                        divisor += auxItem.Measure;
                    }
                    else
                    {
                        dividendo += (auxItem.Measure * auxItem.OrderItem.UnitPrice * auxItem.SupplyEntry.Order.ExchangeRate);
                        divisor += auxItem.Measure;
                    }
                }
                
                dividendo = item.SupplyEntry.Order.Currency == ConstantHelpers.Currency.NUEVOS_SOLES ?
                    dividendo + item.Measure * item.OrderItem.UnitPrice : dividendo + item.Measure * item.OrderItem.UnitPrice * item.SupplyEntry.Order.ExchangeRate;
                divisor = divisor + item.Measure;
                sumaProducto = Math.Round(dividendo / divisor, 2);

                if (existe == null)
                    newStock.Add(new Stock
                    {
                        SupplyId = item.OrderItem.SupplyId,
                        Measure = item.Measure,
                        ProjectId = pId,
                        UnitPrice = sumaProducto,
                        Parcial = Math.Round(sumaProducto * item.Measure, 2)
                    });
                else
                {
                    existe.Measure += item.Measure;
                    existe.UnitPrice = sumaProducto;
                    existe.Parcial = Math.Round(sumaProducto * existe.Measure, 2);
                }
                
                listMessage += item.OrderItem.Supply.FullCode + " | " + item.OrderItem.Supply.Description
                    + " | " + item.OrderItem.Supply.MeasurementUnit.Abbreviation 
                    + " | " + item.Measure.ToString(CultureInfo.InvariantCulture) + "<br />";
            }

            var mailMessage = new MailMessage
            {
                From = new MailAddress("sistemaerp@ivc.pe", "Sistema IVC"),
                Subject = $"{correlative.CodePrefix} - Aviso de confirmación del Ingreso de Material {entry.Order.Provider.Tradename} | {entry.RemissionGuide}"
            };

            if (entry.RemissionGuideUrl != null)
            {
                WebClient webClient = new WebClient();
                Attachment data = new Attachment(webClient.OpenRead(entry.RemissionGuideUrl),
                    $"{entry.Order.Provider.Tradename} | {entry.RemissionGuide}.pdf");
                mailMessage.Attachments.Add(data);
            }
            
            foreach (var auth in responsibles)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == auth);

                mailMessage.To.Add(new MailAddress(user.Email, user.RawFullName));
            }
            //mailMessage.To.Add(new MailAddress("henrrypaul_22@hotmail.com", "Henry"));
            //mailMessage.To.Add(new MailAddress("arian.cc@hotmail.com", "Arian"));
            mailMessage.Body =
                $"Hola, <br /><br /> " +
                $"Con fecha {entry.DeliveryDate.ToString("dd/MM/yyyy")} ha ingresado en el centro de costo {entry.Order.Warehouse.WarehouseType.Project.Abbreviation} los siguientes insumos: <br />" +
                $"<br />" +
                //$"Ingreso de Material N° " + entry.DocumentNumber.ToString("D4") + "<br />" +
                $"Orden " + entry.Order.Warehouse.WarehouseType.Project.CostCenter.ToString()
                    + "-" + entry.Order.CorrelativeCode.ToString("D4") + "-" + entry.Order.ReviewDate.Value.Year.ToString() + "<br />" +
                $"Proveedor " + entry.Order.Provider.Tradename + "<br />" +
                $"Guía de Remisión " + entry.RemissionGuide + "<br />" +
                $"<br />" +
                $"Codigo IVC | Insumo | UND | Ingreso actual <br />" +
                listMessage +
                $"<br />" +
                $"<br />" +
                $"Saludos <br />" +
                $"Sistema IVC <br />" +
                $"Control de Almacén";
            mailMessage.IsBodyHtml = true;
            
            //Mandar Correo
            using (var client = new SmtpClient("smtp.office365.com", 587))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("sistemaerp@ivc.pe", "S1st3m4erp");
                client.EnableSsl = true;
                await client.SendMailAsync(mailMessage);
            }
            
            if (newStock.Count != 0)
                await _context.Stocks.AddRangeAsync(newStock);

            await _context.SaveChangesAsync();
            
            return Ok();

            /*
            foreach(var supply in supplies)
            {
                var sItems = items.Where(x => x.SupplyEntryId == supply.Id).ToList();

                var total = 0.0;

                foreach(var item in sItems)
                {
                    total += (item.Measure * item.OrderItem.UnitPrice);
                }

                supply.Parcial = Math.Round(total, 2);
            }
            */
        }



        [HttpGet("descargar/{id}")]
        public async Task<IActionResult> GetFile(Guid id)
        {
            var entry = await _context.SupplyEntries
                .FirstOrDefaultAsync(x => x.Id == id);

            WebClient client = new WebClient();
            Stream aux = client.OpenRead(entry.RemissionGuideUrl);

            var ms = new MemoryStream();

            aux.CopyTo(ms);
            ms.Position = 0;

            return File(ms.ToArray(), "application/pdf", $"Guía_{entry.RemissionGuide}.pdf");
        }

        #region ITEMS

        [HttpGet("detalles/listar")]
        public async Task<IActionResult> GetDetailSupplyEntry(Guid id)
        {
            var data = new List<SupplyEntryItemViewModel>();
            var editable = _context.SupplyEntries
                .FirstOrDefault(x => x.Id == id);
            if (editable == null) return Ok(data);
            var aux = editable.Status == ConstantHelpers.Warehouse.SupplyEntry.Status.INPROCESS ? true : false;

            var items = await _context.SupplyEntryItems
                .Include(x => x.SupplyEntry)
                .Include(x => x.OrderItem)
                .Include(x => x.OrderItem.Order)
                .Include(x => x.OrderItem.Supply.MeasurementUnit)
                .Include(x => x.OrderItem.Supply)
                .Include(x => x.OrderItem.Supply.SupplyFamily)
                .Include(x => x.OrderItem.Supply.SupplyGroup)
                .Where(x => x.SupplyEntryId == id)
                .Select(x => new SupplyEntryItemViewModel
                {
                    Id = x.Id,
                    OrderItemId = x.OrderItemId,
                    OrderItem = new OrderItemViewModel
                    {
                        Id = x.Id,
                        Supply = new SupplyViewModel
                        {
                            Description = x.OrderItem.Supply.Description,
                            SupplyFamily = new SupplyFamilyViewModel
                            {
                                Code = x.OrderItem.Supply.SupplyFamily.Code,
                                Name = x.OrderItem.Supply.SupplyFamily.Name
                            },
                            SupplyGroupId = x.OrderItem.Supply.SupplyGroupId,
                            SupplyGroup = new SupplyGroupViewModel
                            {
                                Code = x.OrderItem.Supply.SupplyGroup.Code,
                                Name = x.OrderItem.Supply.SupplyGroup.Name
                            },
                            CorrelativeCode = x.OrderItem.Supply.CorrelativeCode,
                            MeasurementUnit = new MeasurementUnitViewModel
                            {
                                Abbreviation = x.OrderItem.Supply.MeasurementUnit.Abbreviation
                            }
                        },
                        OrderId = x.OrderItem.OrderId,
                        Order = new OrderViewModel
                        {
                            Status = x.OrderItem.Order.Status
                        },
                        Measure = x.OrderItem.Measure.ToString()
                    },
                    Measure = x.Measure.ToString(),
                    Observations = x.Observations,
                    PreviousAttention = x.PreviousAttention,
                    isEditable = aux
                })
                .AsNoTracking()
                .ToListAsync();

            data.AddRange(items);

            if (editable.Status == ConstantHelpers.Warehouse.SupplyEntry.Status.INPROCESS)
            {
                var orderitems = await _context.OrderItems
                    .Include(x => x.Supply)
                    .Include(x => x.Supply.SupplyFamily)
                    .Include(x => x.Supply.SupplyGroup)
                    .Include(x => x.Supply.MeasurementUnit)
                    .Include(x => x.Order)
                    .Where(x => items
                    .Select(y => y.OrderItem.OrderId).Contains(x.OrderId) && !items.Select(y => y.OrderItemId).Contains(x.Id))
                    .Select(x => new SupplyEntryItemViewModel
                    {
                        Id = x.Id,
                        OrderItem = new OrderItemViewModel
                        {
                            Id = x.Id,
                            Supply = new SupplyViewModel
                            {
                                Description = x.Supply.Description,
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
                                MeasurementUnit = new MeasurementUnitViewModel
                                {
                                    Abbreviation = x.Supply.MeasurementUnit.Abbreviation
                                }
                            },
                            Order = new OrderViewModel
                            {
                                Status = x.Order.Status
                            },
                            Measure = x.Measure.ToString()
                        },
                        Measure = "0",
                        Observations = "",
                        PreviousAttention = 0,
                        isEditable = aux
                    })
                    .AsNoTracking()
                    .ToListAsync();
                data.AddRange(orderitems);
            }
            return Ok(data);
        }

        [HttpPut("editar-items/{id}")]
        public async Task<IActionResult> EditItems(Guid id, SupplyEntryViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entry = await _context.SupplyEntries.FirstOrDefaultAsync(x => x.Id == id);

            //if (entry.Status == ConstantHelpers.Warehouse.SupplyEntry.Status.CONFIRMED)
              //  return BadRequest("El ingreso ya ha sido confirmado");

            var items = await _context.SupplyEntryItems
                    .Include(x => x.OrderItem)
                    .Where(x => x.SupplyEntryId == id)
                    .ToListAsync();

            var ordItems = await _context.OrderItems
                .Where(x => x.OrderId == items.FirstOrDefault().OrderItem.OrderId)
                .ToListAsync();

            var addItems = new List<SupplyEntryItem>();

            var isTotal = true;

            if (entry.Status == ConstantHelpers.Warehouse.SupplyEntry.Status.CONFIRMED)
            {
                foreach (var stringItem in model.Items)
                {
                    var stringSplit = stringItem.Split("|");
                    Guid itemId = Guid.Parse(stringSplit[0]);

                    var item = items
                        .FirstOrDefault(x => x.Id == itemId);
                    if (item == null) continue;
                    var obs = stringSplit[2];

                    item.Observations = obs;
                }
            }
            else
            {
                var total = 0.0;
                var ordId = Guid.NewGuid();
                foreach (var stringItem in model.Items)
                {
                    var stringSplit = stringItem.Split("|");
                    Guid itemId = Guid.Parse(stringSplit[0]);
                    double limite;
                    var item = items
                        .FirstOrDefault(x => x.Id == itemId);
                    if (item != null) ordId = item.OrderItem.OrderId;
                    var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == ordId);
                    var cant = stringSplit[1].ToDoubleString();
                    var obs = stringSplit[2];

                    if (item == null && cant > 0) // Leyó un item que no está en SupplyEntryItems, entonces es un OrderItem
                    {
                        if (stringSplit[1] == "undefined" || stringSplit[2] == "undefined")
                            return BadRequest("Quite lo filtros de la tabla de insumos para guardar");

                        var ordItem = ordItems.FirstOrDefault(x => x.Id == itemId);

                        if (ordItem == null)
                            return BadRequest("No se encontró el item");

                        total += (cant * ordItem.UnitPrice);

                        limite = ordItem.Measure - ordItem.MeasureInAttention;

                        if (cant > limite)
                            return BadRequest("Se ha superado el límite de lo requerido.");

                        if (cant > 0)
                            addItems.Add(new SupplyEntryItem
                            {
                                SupplyEntry = entry,
                                OrderItemId = itemId,
                                Measure = cant,
                                Observations = obs,
                                PreviousAttention = ordItem.MeasureInAttention
                            });

                        ordItem.MeasureInAttention += cant;

                        if (ordItem.Measure != ordItem.MeasureInAttention)
                            isTotal = false;
                    }
                    else if(item != null)
                    {
                        total += (cant * item.OrderItem.UnitPrice);
                        /*
                                        var price = stringSplit[2].ToDoubleString();
                                        var glosa = stringSplit[3].ToString();

                                        if (cant > (reqItem.Measure - reqItem.MeasureInAttention))
                                            return BadRequest("Se ha superado el límite de metrado para atender");
                                        */


                        limite = item.OrderItem.Measure - item.OrderItem.MeasureInAttention + item.Measure;

                        if (cant > limite)
                            return BadRequest("Se ha superado el límite de lo requerido.");

                        item.OrderItem.MeasureInAttention -= item.Measure;
                        item.OrderItem.MeasureInAttention += cant;

                        if (cant > 0)
                            item.Measure = cant;

                        item.Observations = obs;
                    }
                    else if(item == null)
                    {
                        continue;
                    }
                }
                //if (isTotal == false)
                //    order.AttentionStatus = ConstantHelpers.Logistics.RequestOrder.AttentionStatus.PARTIAL;
                //else
                //    order.AttentionStatus = ConstantHelpers.Logistics.RequestOrder.AttentionStatus.TOTAL;

                var aux = await _context.SupplyEntries.FirstOrDefaultAsync(x => x.Id == id);
                aux.Parcial = total;
            }
            //await _context.SupplyEntryItems.AddRangeAsync(addItems);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("qr/{id}")]
        public async Task<IActionResult> QrGenerator(Guid id)
        {
            var entry = await _context.SupplyEntryItems
                .Include(x => x.OrderItem.Order)
                .Include(x => x.OrderItem.Supply)
                .Include(x => x.OrderItem.Supply.SupplyFamily)
                .Include(x => x.OrderItem.Supply.SupplyGroup)
                .Include(x => x.OrderItem.Order.Requests)
                .Include(x => x.SupplyEntry)
                .Include(x => x.SupplyEntry.Order.Provider)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            var project = await _context.Projects.FirstOrDefaultAsync(x => x.Id == GetProjectId());

            if (entry == null)
                return BadRequest("No se ha encontrado el insumo");

            if (project == null)
                return BadRequest("No proyecto");
            /*
            var code = project.CostCenter.ToString() + "-" + entry.OrderItem.Order.CorrelativeCode.ToString("D4")
                + "-" + entry.OrderItem.Order.ReviewDate.Value.Year.ToString();
            
            */

            using (MemoryStream ms = new MemoryStream())
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(entry.OrderItem.Supply.FullCode + "/" + entry.SupplyEntry.RemissionGuide, QRCodeGenerator.ECCLevel.L);
                QRCode qrCode = new QRCode(qrCodeData);

                PdfDocument doc = new PdfDocument();

                PdfPageBase page = doc.Pages.Add(new SizeF(580, 220), new PdfMargins(0));

                PdfTrueTypeFont font = new PdfTrueTypeFont("Helvetica", 20f, PdfFontStyle.Bold, true);

                //page.Canvas.DrawString(entry.OrderItem.Supply.Description, new PdfFont(PdfFontFamily.Helvetica, 12f), new PdfSolidBrush(Color.Black), 180, 30);
                page.Canvas.DrawString(entry.OrderItem.Supply.Description, font, new PdfSolidBrush(Color.Black), new RectangleF(220, 30, 350, 65));

                //page.Canvas.DrawString(s1, font, PdfBrushes.Black, new RectangleF(0, 0, 180, 200));
                //page.Canvas.DrawString(s1, font, PdfBrushes.Black, new RectangleF(180, 40, 250, 50));


                page.Canvas.DrawString(project.CostCenter + "-" + entry.OrderItem.Order.CorrelativeCode.ToString("D4")
                  + "-" + entry.OrderItem.Order.ReviewDate.Value.Year.ToString(), new PdfFont(PdfFontFamily.Helvetica, 20f, PdfFontStyle.Bold), new PdfSolidBrush(Color.Black), 220, 85);

                page.Canvas.DrawString(entry.OrderItem.Order.Provider.Tradename, new PdfFont(PdfFontFamily.Helvetica, 20f, PdfFontStyle.Bold), new PdfSolidBrush(Color.Black), 220, 110);

                page.Canvas.DrawString(entry.SupplyEntry.RemissionGuide, new PdfFont(PdfFontFamily.Helvetica, 20f, PdfFontStyle.Bold), new PdfSolidBrush(Color.Black), 220, 135);

                page.Canvas.DrawString(entry.SupplyEntry.DeliveryDate.ToDateString(), new PdfFont(PdfFontFamily.Helvetica, 20f, PdfFontStyle.Bold), new PdfSolidBrush(Color.Black), 220, 160);

                MemoryStream img = new MemoryStream();


                using (Bitmap bitMap = qrCode.GetGraphic(10))
                {
                    Bitmap resized = new Bitmap(bitMap, new Size(290, 290));

                    resized.Save(img, ImageFormat.Png);

                    PdfImage image = PdfImage.FromStream(img);

                    page.Canvas.DrawImage(image, new PointF(0, 0));
                    doc.SaveToStream(ms);
                    doc.Close();
                    /*
                    ViewBag.QRCodeImage = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                    ms.Position = 0;
                    ms.Seek(0, SeekOrigin.Begin);
                    return File(ms.ToArray(), "image/jpeg", "QR_" + entry.OrderItem.Supply.FullCode
                        + "-" + entry.OrderItem.Supply.Description + ".jpg");
                    */
                    return File(ms.ToArray(), "application/pdf", entry.OrderItem.Supply.FullCode
                        + "-" + entry.OrderItem.Supply.Description + ".pdf");
                }

            }

        }

        [HttpPost("importar")]
        public async Task<IActionResult> Import(IFormFile file)
        {
            var items = await _context.OrderItems
                .Include(x => x.Order)
                .Include(x => x.Supply)
                .Include(x => x.Supply.SupplyFamily)
                .Include(x => x.Supply.SupplyGroup)
                .ToListAsync();

            var almacenes = await _context.Warehouses
                .ToListAsync();

            var usuarios = await _context.Users
                .Include(x => x.WorkAreaEntity)
                .Include(x => x.UserRoles)
                .Where(x => x.WorkAreaEntity.Name == "Logística" && x.UserRoles.FirstOrDefault(y => y.Role.Name == "Oficina Técnica") != null)
                .Select(x => x.Id)
                .ToListAsync();

            var entries = new List<SupplyEntry>();

            var entryItems = new List<SupplyEntryItem>();

            using (var mem = new MemoryStream())
            {
                await file.CopyToAsync(mem);
                using (var workBook = new XLWorkbook(mem))
                {
                    var workSheet = workBook.Worksheets.FirstOrDefault();
                    var counter = 3;
                    var orderAux = _context.Orders.FirstOrDefault();
                    var count = 0;

                    if (_context.SupplyEntries.Count() != 0)
                        count = _context.SupplyEntries.OrderBy(x => x.DocumentNumber).Last().DocumentNumber;
                    
                    var remNumber = "";
                    var aux = remNumber;

                    var entry = new SupplyEntry();

                    var total = 0.0;

                    while (!workSheet.Cell($"B{counter}").IsEmpty())
                    {
                        var remExcel = workSheet.Cell($"H{counter}").GetString();

                        if(remExcel != remNumber) // Entra un supply nuevo, sino es un item del actual supply
                        {
                            remNumber = remExcel;
                            count++;
                            var remselected = await _context.SupplyEntries
                            .FirstOrDefaultAsync(x => x.RemissionGuide == remExcel);
                            /*if (remselected != null)
                            {
                                return BadRequest("Ya existe un Ingreso por compra con esa Guia de Remisión");
                            }*/
                            var correlativeExcel = workSheet.Cell($"B{counter}").GetString();

                            var correlative = Int32.Parse(correlativeExcel);
                            var suffix = workSheet.Cell($"C{counter}").GetString();
                            var flagOB = workSheet.Cell($"D{counter}").GetString();
                            var year = workSheet.Cell($"E{counter}").GetString();

                            var orderAuxList = _context.Orders.Where(x => x.CorrelativeCode == correlative
                                && ((suffix == null || suffix == "" || suffix == " ") ? x.CorrelativeCodeSuffix == null : x.CorrelativeCodeSuffix == suffix)
                                && x.ReviewDate != null &&  x.ReviewDate.Value.Year == Int32.Parse(year) && x.Status == ConstantHelpers.Logistics.RequestOrder.Status.APPROVED);

                            if(orderAuxList.Count() == 0)
                            {
                                return BadRequest($"No se ha encontrado la orden de la fila {counter}");
                            }

                            if (flagOB == "1")
                            {
                                
                                orderAux = orderAuxList.FirstOrDefault(x => x.CorrelativeCode == correlative
                                && x.CorrelativeCodeSuffix == suffix && usuarios.Contains(x.IssuedUserId));
                            }
                            else if (flagOB == "0")
                            {
                                orderAux = orderAuxList.FirstOrDefault(x => x.CorrelativeCode == correlative
                                && x.CorrelativeCodeSuffix == suffix && !usuarios.Contains(x.IssuedUserId));
                            }
                            else
                            {
                                return BadRequest($"El OB de la fila {counter} debe ser 1 para SÍ o 0 para NO");
                            }
                          
                            if(orderAux == null)
                            {
                                return BadRequest($"No se ha encontrado la orden de la fila {counter}");
                            }

                            var almacen = almacenes.FirstOrDefault(x => x.Address == workSheet.Cell($"F{counter}").GetString());

                            if (almacen == null)
                                return BadRequest($"La dirección del almacén ingresado es incorrecto en la fila {counter}");

                            entry = new SupplyEntry();
                            entry.Id = Guid.NewGuid();
                            entry.DocumentNumber = count;
                            entry.RemissionGuide = workSheet.Cell($"H{counter}").GetString();
                            entry.OrderId = orderAux.Id;
                            entry.Status = ConstantHelpers.Warehouse.SupplyEntry.Status.INPROCESS;
                            entry.WarehouseId = almacen.Id;
                            entry.Parcial = total;
                            entry.DeliveryDate = workSheet.Cell($"G{counter}").GetDateTime();
                            total = 0.0;

                            entries.Add(entry);
                        }

                        var item = items.FirstOrDefault(x => x.OrderId == orderAux.Id && x.Supply.FullCode == workSheet.Cell($"I{counter}").GetString());
                        if (item == null)
                            return BadRequest($"No se encontró el insumo de la fila {counter} en la orden");

                        var entryItem = new SupplyEntryItem();

                        entryItem.Id = Guid.NewGuid();
                        entryItem.SupplyEntryId = entry.Id; 
                        entryItem.OrderItemId = item.Id;
                        entryItem.Measure = workSheet.Cell($"J{counter}").GetString().ToDoubleString();
                        entryItem.PreviousAttention = item.MeasureInAttention;
                        entryItem.Observations = workSheet.Cell($"K{counter}").GetString();

                        item.MeasureInAttention += entryItem.Measure;

                        total += entryItem.Measure;

                        entry.Parcial = total;

                        entryItems.Add(entryItem);

                        ++counter;
                    }
                    await _context.SupplyEntries.AddRangeAsync(entries);
                    await _context.SupplyEntryItems.AddRangeAsync(entryItems);
                    await _context.SaveChangesAsync();
                }
                mem.Close();
            }
            return Ok();
        }

        [HttpGet("excel-modelo")]
        public FileResult GetExcelSample()
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                var workSheet = wb.Worksheets.Add("Ingreso de Material");
                workSheet.Column(2).Style.NumberFormat.Format = "@";
                workSheet.Column(7).Style.NumberFormat.Format = "@";
                workSheet.Column(8).Style.NumberFormat.Format = "@";

                workSheet.Cell($"B1").Value = "0064";
                workSheet.Cell($"C1").Value = "C";
                workSheet.Cell($"D1").Value = "1";
                workSheet.Cell($"E1").Value = "2022";
                workSheet.Cell($"F1").Value = "Prolongación Aija S / N, distrito de Huarmey, Provincia de Huarmey y Departamento de Ancash";
                workSheet.Cell($"G1").Value = "24/03/2022";
                workSheet.Cell($"H1").Value = "TPP1-001013";
                workSheet.Cell($"I1").Value = "01008002012";
                workSheet.Cell($"J1").Value = 4;
                workSheet.Cell($"K1").Value = "Ejemplo de observación";

                workSheet.Range("B1:K1").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"B2").Value = "Correlativo Orden";
                workSheet.Cell($"C2").Value = "Sufijo Orden";
                workSheet.Cell($"D2").Value = "OB";
                workSheet.Cell($"E2").Value = "Año";
                workSheet.Cell($"F2").Value = "Almacen";
                workSheet.Cell($"G2").Value = "Fecha de Entrega";
                workSheet.Cell($"H2").Value = "Guía de Remisión";
                workSheet.Cell($"I2").Value = "Código IVC";
                workSheet.Cell($"J2").Value = "Ingreso Actual";
                workSheet.Cell($"K2").Value = "Observaciones";

                workSheet.Column(1).Width = 3;
                workSheet.Column(2).Width = 17;
                workSheet.Column(3).Width = 12;
                workSheet.Column(4).Width = 7;
                workSheet.Column(5).Width = 7;
                workSheet.Column(6).Width = 90;
                workSheet.Column(7).Width = 16;
                workSheet.Column(8).Width = 16;
                workSheet.Column(9).Width = 16;
                workSheet.Column(10).Width = 13;
                workSheet.Column(11).Width = 28;

                workSheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                workSheet.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                workSheet.Range("B2:K2").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                workSheet.Range("B2:K2").Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);

                var aux = _context.Warehouses.Include(x => x.WarehouseType)
                    .Where(x => x.WarehouseType.ProjectId == GetProjectId()).ToList();

                DataTable dtFamilies = new DataTable();
                dtFamilies.TableName = "Almacenes";
                dtFamilies.Columns.Add("Direccion", typeof(string));
                foreach (var item in aux)
                    dtFamilies.Rows.Add(item.Address);
                dtFamilies.AcceptChanges();

                var workSheetFamily = wb.Worksheets.Add(dtFamilies);

                workSheetFamily.Column(1).Width = 90;
                

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "IngresoMateriales.xlsx");
                }
            }
        }

        #endregion
        /*
        [HttpGet("correos")]
        public async Task<IActionResult> SendMails()
        {
            var entries = await _context.SupplyEntries
                .Include(x => x.Order)
                .Include(x => x.Order.Provider)
                .Include(x => x.Order.Warehouse.WarehouseType.Project)
                .Where(x => x.Status == ConstantHelpers.Warehouse.SupplyEntry.Status.CONFIRMED)
                .ToListAsync();

            var pId = GetProjectId();

            var correlative = await _context.RequestSummaries
               .Where(x => x.ProjectId == pId)
               .FirstOrDefaultAsync();

            var responsibles = await _context.WarehouseResponsibles
                .Where(x => x.UserType == ConstantHelpers.Warehouse.UserTypes.ThecnicalOfficeControl
                || x.UserType == ConstantHelpers.Warehouse.UserTypes.StoreKeepers)
                .Where(x => x.ProjectId == pId)
                .Select(x => x.UserId)
                .Distinct()
                .ToListAsync();

            var entryitems = await _context.SupplyEntryItems
                .Include(x => x.OrderItem)
                .Include(x => x.SupplyEntry)
                .Include(x => x.SupplyEntry.Order)
                .Include(x => x.OrderItem.Supply)
                .Include(x => x.OrderItem.Supply.SupplyGroup)
                .Include(x => x.OrderItem.Supply.SupplyFamily)
                .Include(x => x.OrderItem.Supply.MeasurementUnit)
                .ToListAsync();

            foreach(var entry in entries)
            {
                var items = entryitems
               .Where(x => x.SupplyEntryId == entry.Id)
               .ToList();

                var listMessage = "";

                foreach (var item in items)
                {
                    listMessage += item.OrderItem.Supply.FullCode + " | " + item.OrderItem.Supply.Description
                        + " | " + item.OrderItem.Supply.MeasurementUnit.Abbreviation
                        + " | " + item.Measure.ToString(CultureInfo.InvariantCulture) + "<br />";
                }

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("sistemaerp@ivc.pe", "Sistema IVC"),
                    Subject = $"{correlative.CodePrefix} - Aviso de confirmación del Ingreso de Material {entry.Order.Provider.Tradename} | {entry.RemissionGuide}"
                };

                if (entry.RemissionGuideUrl != null)
                {
                    WebClient webClient = new WebClient();
                    Attachment data = new Attachment(webClient.OpenRead(entry.RemissionGuideUrl),
                        $"{entry.Order.Provider.Tradename} | {entry.RemissionGuide}.pdf");
                    mailMessage.Attachments.Add(data);
                }

                
                foreach (var auth in responsibles)
                {
                    var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == auth);

                    mailMessage.To.Add(new MailAddress(user.Email, user.RawFullName));
                }
                //mailMessage.To.Add(new MailAddress("henrrypaul_22@hotmail.com", "Henry"));

                mailMessage.Body =
                    $"Hola, <br /><br /> " +
                    $"El día de hoy se ha ingresado en el centro de costo {entry.Order.Warehouse.WarehouseType.Project.Abbreviation} los siguientes insumos: <br />" +
                    $"<br />" +
                    //$"Ingreso de Material N° " + entry.DocumentNumber.ToString("D4") + "<br />" +
                    $"Orden " + entry.Order.Warehouse.WarehouseType.Project.CostCenter.ToString()
                        + "-" + entry.Order.CorrelativeCode.ToString("D4") + "-" + entry.Order.ReviewDate.Value.Year.ToString() + "<br />" +
                    $"Proveedor " + entry.Order.Provider.Tradename + "<br />" +
                    $"Guía de Remisión " + entry.RemissionGuide + "<br />" +
                    $"<br />" +
                    $"Codigo IVC | Insumo | UND | Ingreso actual <br />" +
                    listMessage +
                    $"<br />" +
                    $"<br />" +
                    $"Saludos <br />" +
                    $"Sistema IVC <br />" +
                    $"Control de Almacén";
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

        [HttpGet("exportar")]
        public async Task<IActionResult> Export(IFormFile file)
        {
            var dt = new DataTable("INGRESOS DE MATERIAL AL ALMACÉN");
            dt.Columns.Add("N° DOCUMENTO", typeof(string));
            dt.Columns.Add("ALMACÉN", typeof(string));
            dt.Columns.Add("FECHA INGRESO", typeof(DateTime));
            dt.Columns.Add("GUIA DE REMISIÓN", typeof(string));
            dt.Columns.Add("PROVEEDOR", typeof(string));
            dt.Columns.Add("ORDEN DE COMPRA", typeof(string));
            dt.Columns.Add("FECHA ORDEN COMPRA", typeof(DateTime));
            dt.Columns.Add("MONTO S/", typeof(double));
            dt.Columns.Add("MONTO US$", typeof(double));
            dt.Columns.Add("ESTADO", typeof(string));
            dt.Columns.Add("CODIGO IVC", typeof(string));
            dt.Columns.Add("FAMILIA", typeof(string));
            dt.Columns.Add("GRUPO", typeof(string));
            dt.Columns.Add("INSUMO", typeof(string));
            dt.Columns.Add("UND", typeof(string));
            dt.Columns.Add("METRADO SOLICITADO", typeof(string));
            dt.Columns.Add("ATENCIÓN ACUMULADO", typeof(string));
            dt.Columns.Add("ATENCIÓN ACTUAL", typeof(string));

            var supplyItems = await _context.SupplyEntryItems
                .Include(x => x.OrderItem.Supply)
                .Include(x => x.OrderItem.Supply.MeasurementUnit)
                .Include(x => x.OrderItem.Supply.SupplyFamily)
                .Include(x => x.OrderItem.Supply.SupplyGroup)
                .Include(x => x.OrderItem)
                .Include(x => x.OrderItem.Order)
                .Include(x => x.SupplyEntry.Warehouse.WarehouseType)
                .Include(x => x.SupplyEntry.Warehouse.WarehouseType.Project)
                .Where(x => x.SupplyEntry.Warehouse.WarehouseType.ProjectId == GetProjectId())
                .ToListAsync();
            var usuarios = _context.Users
                .Include(x => x.WorkAreaEntity)
                .Include(x => x.UserRoles)
                .Where(x => x.WorkAreaEntity.Name == "Logística" && x.UserRoles.FirstOrDefault(y => y.Role.Name == "Oficina Técnica") != null)
                .Select(x => x.Id)
                .ToList();

            foreach (var item in supplyItems)
            {
                var supply = _context.SupplyEntries
                    .Include(x => x.Warehouse)
                    .Include(x => x.Order.Provider)
                    .Include(x => x.Order)
                    .FirstOrDefault(x => x.Id == item.SupplyEntryId);

                var soles = 0.0;
                var dolares = 0.0;

                if (supply.Order.Currency == 1)
                {
                    soles = supply.Parcial;
                    dolares = Math.Round(supply.Parcial * supply.Order.ExchangeRate, 2);
                }
                else
                {
                    soles = Math.Round(supply.Parcial * supply.Order.ExchangeRate, 2);
                    dolares = supply.Parcial;
                }

                var code = supply.Warehouse.WarehouseType.Project.CostCenter.ToString() + "-" +
                        supply.Order.CorrelativeCode.ToString("D4") + supply.Order.CorrelativeCodeSuffix + "-" +
                        supply.Order.ReviewDate.Value.Year.ToString();

                //if (item.OrderItem.Order.IssuedUserId == "0b224d20-bebe-4a9c-b16c-34cda18c9741")
                //    code += "-OB";
                if (usuarios.Contains(item.OrderItem.Order.IssuedUserId))
                    code += "-OB";

                dt.Rows.Add(supply.DocumentNumber, supply.Warehouse.Address, supply.DeliveryDate, supply.RemissionGuide, supply.Order.Provider.BusinessName,
                    code, supply.Order.Date, Math.Round(soles,2), Math.Round(dolares,2), ConstantHelpers.Warehouse.SupplyEntry.Status.VALUES[supply.Status], 
                    item.OrderItem.Supply.FullCode, item.OrderItem.Supply.SupplyFamily.Name,
                    item.OrderItem.Supply.SupplyGroup.Name, item.OrderItem.Supply.Description,
                    item.OrderItem.Supply.MeasurementUnit.Abbreviation, item.OrderItem.Measure, item.PreviousAttention, item.Measure);
            }

            var project = await _context.Projects.FirstOrDefaultAsync(x => x.Id == GetProjectId());
            var fileName = $"Ingresos de Material - {project.Abbreviation}.xlsx";
            using (var wb = new XLWorkbook())
            {
                var workSheet = wb.Worksheets.Add(dt);

                workSheet.Column(1).Width = 10;
                workSheet.Column(2).Width = 85;
                workSheet.Column(3).Width = 16;
                workSheet.Column(4).Width = 19;
                workSheet.Column(5).Width = 30;
                workSheet.Column(6).Width = 17;
                workSheet.Column(7).Width = 18;
                workSheet.Column(8).Width = 13;
                workSheet.Column(9).Width = 13;
                workSheet.Column(10).Width = 15;
                workSheet.Column(11).Width = 15;
                workSheet.Column(12).Width = 32;
                workSheet.Column(13).Width = 39;
                workSheet.Column(14).Width = 59;
                workSheet.Column(15).Width = 6;
                workSheet.Column(16).Width = 15;
                workSheet.Column(17).Width = 18;
                workSheet.Column(18).Width = 18;

                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Seek(0, SeekOrigin.Begin);
                    Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        [HttpGet("vale/{id}")]
        public async Task<IActionResult> Test(Guid id)
        {
            var items = await _context.SupplyEntryItems
                .Include(x => x.OrderItem)
                .ThenInclude(x => x.Supply)
                .ThenInclude(x => x.SupplyFamily)
                .Include(x => x.OrderItem)
                .ThenInclude(x => x.Supply)
                .ThenInclude(x => x.SupplyGroup)
                .Include(x => x.OrderItem)
                .ThenInclude(x => x.Supply)
                .ThenInclude(x => x.MeasurementUnit)
                .Include(x => x.SupplyEntry)
                .ThenInclude(x => x.Order)
                .ThenInclude(x => x.Warehouse)
                .Include(x => x.SupplyEntry)
                .ThenInclude(x => x.Order)
                .ThenInclude(x => x.Provider)
                .Where(x => x.SupplyEntryId == id)
                .ToListAsync();

            var project = await _context.Projects.FirstOrDefaultAsync(x => x.Id == GetProjectId());

            var entry = items.FirstOrDefault().SupplyEntry;

            var summary = await _context.RequestSummaries.FirstOrDefaultAsync(x => x.ProjectId == GetProjectId());

            var code = project.CostCenter.ToString() + "-"
                    + entry.Order.CorrelativeCode.ToString("D4") + entry.Order.CorrelativeCodeSuffix + "-"
                    + entry.Order.ReviewDate.Value.Year.ToString();

            if (_context.Users.Include(x => x.WorkAreaEntity).FirstOrDefault(x => x.Id == entry.Order.IssuedUserId).WorkAreaEntity.Name == "Logística"
                        && _context.UserRoles.FirstOrDefault(x => x.Role.Name == "Oficina Técnica" && x.UserId == entry.Order.IssuedUserId) != null)
                code += "-OB";

            //var request = items.FirstOrDefault().FieldRequest;

            //var usuario = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.IssuedUserId);

            //var formulas = await _context.FieldRequestProjectFormulas
            //    .Include(x => x.ProjectFormula)
            //    .Where(x => x.FieldRequestId == id)
            //    .Select(x => x.ProjectFormula.Code + " - " + x.ProjectFormula.Name)
            //    .ToListAsync();

            var stocks = await _context.Stocks
                .Where(x => x.ProjectId == GetProjectId())
                .ToListAsync();

            using (var ms = new MemoryStream())
            {
                using (TextWriter tw = new StreamWriter(ms))
                {
                    int limite = 142;
                    int currentPag = 1;
                    int maxPag = (int)Math.Ceiling((double)items.Count() / 25);
                    int itemNumber = 1;
                    int aux = limite;
                    var limiteItems = 25;

                    var tLine = "";
                    var tLine2 = "";
                    var tLine3 = "";

                    var tAux = "";

                    void Cabecera()
                    {
                        tLine = "ERP-IVC";

                        tAux = "CODIGO : CSH/GAL-For-01A";

                        tLine += new string(' ', limite - tAux.Length - tLine.Length);
                        tLine += tAux;

                        tw.WriteLine(tLine);

                        /// -----------------------
                        tLine = project.Abbreviation;
                        tAux = "GESTIÓN DE ALMACÉN";
                        aux = limite / 2;
                        aux -= (tAux.Length / 2);
                        aux -= tLine.Length;
                        while (aux > 0)
                        {
                            tLine += " ";
                            aux -= 1;
                        }
                        tLine += tAux;
                        aux = limite;
                        tAux = $"VERSIÓN  :       1       ";
                        aux -= tLine.Length;
                        aux -= tAux.Length;
                        while (aux > 0)
                        {
                            tLine += " ";
                            aux -= 1;
                        }
                        tLine += tAux;
                        tw.WriteLine(tLine);

                        /// -----------------------

                        tLine = "";

                        tAux = "  FECHA  :  04/04/2022   ";

                        tLine += new string(' ', limite - tAux.Length);
                        tLine += tAux;

                        tw.WriteLine(tLine);

                        /// -----------------------

                        tAux = $"NOTA DE INGRESO N° {summary.CodePrefix}-"
                            + entry.DocumentNumber.ToString("D6");
                        tLine = "";
                        aux = limite / 2;
                        aux -= (tAux.Length / 2);
                        while (aux > 0)
                        {

                            tLine += " ";
                            aux -= 1;
                        }
                        tLine += tAux;
                        tAux = $" PÁGINA  :    {currentPag} de {maxPag}     ";

                        aux = limite;
                        aux -= tLine.Length;
                        aux -= tAux.Length;
                        while (aux > 0)
                        {

                            tLine += " ";
                            aux -= 1;
                        }
                        tLine += tAux;
                        tw.WriteLine(tLine);

                        /// -----------------------

                        tw.WriteLine();
                        tw.WriteLine("   ALMACÉN           :    "
                            + entry.Order.Warehouse.Address);
                        tw.WriteLine("   N°DOC. REFERENCIA :    "
                            + entry.RemissionGuide);
                        tw.WriteLine("   N°ORDEN DE COMPRA :    "
                            + code);
                        tw.WriteLine("   PROVEEDOR         :    "
                            + entry.Order.Provider.Tradename);
                        tw.WriteLine("   N° RUC            :    "
                            + entry.Order.Provider.RUC);
                        tw.WriteLine("   CÓDIGO MOVIMIENTO :    "
                            + "CO ENTRADA POR COMPRA");
                        tw.WriteLine("   FECHA DE ENTREGA  :    "
                            + entry.DeliveryDate.Date.ToDateString());

                        tw.WriteLine();

                        tLine3 = " ------ ";

                        tLine = "  ITEM  ";
                        tAux = "CÓDIGO";

                        void Columna(int largo)
                        {
                            aux = largo / 2;
                            aux -= tAux.Length / 2;
                            while (aux > 0)
                            {
                                tLine += " ";
                                tLine3 += "-";
                                aux -= 1;
                            }

                            tLine += tAux;
                            tLine3 += new string('-', tAux.Length);

                            aux = largo / 2;
                            aux -= tAux.Length / 2;
                            while (aux > 0)
                            {
                                tLine += " ";
                                tLine3 += "-";
                                aux -= 1;
                            }

                            tLine += " ";
                            tLine3 += " ";
                        }

                        Columna(14);

                        tAux = "DESCRIPCIÓN";

                        Columna(92);

                        tAux = "UND";

                        Columna(9);
                        /*
                        tAux = "FASE";

                        Columna(10);

                        tLine2 = new string(' ', tLine.Length + 1);

                        tAux = "CANTIDAD";
                        tLine2 += "SOLICITADA";
                        tLine2 += new string(' ', 4);

                        Columna(12);
                        */
                        tAux = "CANTIDAD";
                        tLine2 = new string(' ', tLine.Length + 3);
                        tLine2 += "RECIBIDA";

                        Columna(14);

                        tw.WriteLine(tLine);
                        tw.WriteLine(tLine2);
                        tw.WriteLine(tLine3);
                    }

                    Cabecera();

                    var counter = 1;

                    foreach (var item in items)
                    {
                        if (counter > limiteItems)
                        {
                            tLine3 = " ";
                            tLine3 += new string('-', limite - 2);
                            tw.WriteLine(tLine3);
                            limiteItems += limiteItems;
                            currentPag++;
                            tw.WriteLine();
                            tw.WriteLine();
                            tw.WriteLine();
                            tw.WriteLine();
                            tw.WriteLine();
                            tw.WriteLine();
                            tw.WriteLine();
                            tw.WriteLine();
                            tw.WriteLine();
                            Cabecera();
                        }

                        var isLong = false;
                        tLine = "  ";
                        tLine += itemNumber.ToString("D4");
                        tLine += "   ";
                        tLine += item.OrderItem.Supply.FullCode;
                        tLine += "    ";

                        tLine2 = new string(' ', tLine.Length);
                        tLine2 += "     ";

                        var description = item.OrderItem.Supply.Description;


                        counter++;
                        if (item.OrderItem.Supply.Description.Length > 89)
                        {
                            isLong = true;
                            tLine += description.Substring(0, 89);
                            tLine2 += description.Substring(89, description.Length - 89);
                            counter++;
                        }
                        else
                        {
                            tLine += description;
                        }

                        while (tLine.Length < 116)
                        {
                            tLine += " ";
                        }

                        tLine += "   ";
                        tLine += item.OrderItem.Supply.MeasurementUnit.Abbreviation;

                        while (tLine.Length < 126)
                        {
                            tLine += " ";
                        }

                        tLine += "   ";
                        tLine += "";

                        var cant = item.Measure.ToString("N2", CultureInfo.InvariantCulture);

                        tLine += new string(' ', 11 - cant.Length);

                        tLine += cant;

                        itemNumber++;

                        tw.WriteLine(tLine);

                        if (isLong == true)
                            tw.WriteLine(tLine2);
                    }
                    /*
                    while (counter < limiteItems)
                    {
                        tw.WriteLine();
                        counter++;
                    }*/

                    tw.WriteLine();

                    tLine3 = new string(' ', 129);
                    tLine3 += "------------";
                    tw.WriteLine(tLine3);
                    tLine3 = new string(' ', 129);
                    var total = items.Sum(x => x.Measure).ToString("N2", CultureInfo.InvariantCulture);

                    tLine3 += new string(' ', 11 - total.Length);

                    tLine3 += total;

                    tw.WriteLine(tLine3);
                    tLine3 = " ";
                    tLine3 += new string('-', limite - 2);
                    tw.WriteLine(tLine3);
                    /*
                    var zone = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
                    if(zone == null)
                        zone = TimeZoneInfo.FindSystemTimeZoneById("America/New_York");
                    DateTime time = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, zone);
                    */

                    DateTime time = DateTime.UtcNow.ToDefaultTimeZone();

                    tw.WriteLine();
                    tLine3 = " AUTORIZANTE    :  " + "";

                    tw.WriteLine(tLine3);
                    tw.WriteLine();
                    tLine3 = " FECHA DESPACHO :  " + time.Date.ToDateString();

                    tw.WriteLine(tLine3);
                    tw.WriteLine();
                    tLine3 = " HORA           :  " + time.ToString("HH:mm");
                    tw.WriteLine(tLine3);

                    tw.Flush();
                    ms.Position = 0;


                    return File(ms.ToArray(), "text/plain", entry.DocumentNumber.ToString("D6") + ".txt");
                }

            }
        }


        [HttpPut("degradar/{id}")]
        public async Task<IActionResult> Degrade(Guid id)
        {
            var entry = await _context.SupplyEntries.FirstOrDefaultAsync(x => x.Id == id);

            if (entry.Status != ConstantHelpers.Warehouse.SupplyEntry.Status.CONFIRMED)
                return BadRequest("Solo se pueden degradar los ingresos confirmados");

            entry.Status = ConstantHelpers.Warehouse.SupplyEntry.Status.INPROCESS;

            var entryitems = await _context.SupplyEntryItems
                .Include(x => x.OrderItem)
                .Include(x => x.SupplyEntry)
                .Include(x => x.SupplyEntry.Order)
                .Include(x => x.OrderItem.Supply)
                .Include(x => x.OrderItem.Supply.SupplyGroup)
                .Include(x => x.OrderItem.Supply.SupplyFamily)
                .Include(x => x.OrderItem.Supply.MeasurementUnit)
                .ToListAsync();

            var pId = GetProjectId();

            var items = entryitems
                .Where(x => x.SupplyEntryId == id)
                .ToList();

            var aux = entryitems
                .Where(x => x.SupplyEntry.Status == ConstantHelpers.Warehouse.SupplyEntry.Status.CONFIRMED)
                .ToList();

            var stock = await _context.Stocks
                .Include(x => x.Supply)
                .ToListAsync();

            foreach (var item in items)
            {
                var existe = stock.FirstOrDefault(x => x.SupplyId == item.OrderItem.SupplyId);
                var auxItems = aux.Where(x => x.OrderItem.SupplyId == item.OrderItem.SupplyId).ToList();

                var sumaProducto = 0.0;
                var dividendo = 0.0;
                var divisor = 0.0;

                foreach (var auxItem in auxItems)
                {
                    if (auxItem.SupplyEntry.Order.Currency == ConstantHelpers.Currency.NUEVOS_SOLES)
                    {
                        dividendo += (auxItem.Measure * auxItem.OrderItem.UnitPrice);
                        divisor += auxItem.Measure;
                    }
                    else
                    {
                        dividendo += (auxItem.Measure * auxItem.OrderItem.UnitPrice * auxItem.SupplyEntry.Order.ExchangeRate);
                        divisor += auxItem.Measure;
                    }
                }
                dividendo = item.SupplyEntry.Order.Currency == ConstantHelpers.Currency.NUEVOS_SOLES ?
                    dividendo - item.Measure * item.OrderItem.UnitPrice : dividendo - item.Measure * item.OrderItem.UnitPrice * item.SupplyEntry.Order.ExchangeRate;
                divisor = divisor - item.Measure;
                sumaProducto = Math.Round(dividendo / divisor, 2);

                if (existe == null)
                {
                    return BadRequest("No se está usando el ingreso en stock");
                }
                else
                {
                    existe.Measure -= item.Measure;
                    if (existe.Measure < 0) return BadRequest("Al degradar el stock será menor que cero");
                    existe.UnitPrice = sumaProducto;
                    existe.Parcial = Math.Round(sumaProducto * existe.Measure, 2);
                }
            }

            await _context.SaveChangesAsync();
            return Ok();
        }



        [HttpPost("importar-facturas")]
        public async Task<IActionResult> ImportInvoices(IFormFile file)
        {
            var pId = GetProjectId();

            var entries = await _context.SupplyEntries
                .Include(x => x.Order)
                .Where(x => x.Order.ProjectId == pId)
                .ToListAsync();

            var invoices = await _context.Invoices.Where(x => x.ProjectId == pId).ToListAsync();

            using (var mem = new MemoryStream())
            {
                await file.CopyToAsync(mem);
                using (var workBook = new XLWorkbook(mem))
                {
                    var workSheet = workBook.Worksheets.FirstOrDefault();
                    var counter = 3;

                    while (!workSheet.Cell($"B{counter}").IsEmpty())
                    {
                        var entryExcel = entries.FirstOrDefault(x => x.RemissionGuide == workSheet.Cell($"B{counter}").GetString());

                        if (entryExcel == null)
                            return BadRequest("No se ha encontrado el número de Guía de Remisión en la fila " + counter);

                        var invoiceExcel = invoices.FirstOrDefault(x => x.Serie == workSheet.Cell($"C{counter}").GetString());

                        if (invoiceExcel == null)
                            return BadRequest("No se ha encontrado la serie de la Factura en la fila " + counter);

                        ++counter;
                    }

                    await _context.SaveChangesAsync();
                }
                mem.Close();
            }
            return Ok();
        }

        [HttpGet("excel-modelo-facturas")]
        public FileResult GetExcelSampleInvoice()
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                var workSheet = wb.Worksheets.Add("Facturas en Ingreso de Material");

                workSheet.Cell($"B2").Value = "Guía de Remisión";
                workSheet.Cell($"C2").Value = "N° Serie Factura";

                workSheet.Column(1).Width = 3;
                workSheet.Column(2).Width = 20;
                workSheet.Column(3).Width = 20;

                workSheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                workSheet.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                workSheet.Range("B2:C2").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                workSheet.Range("B2:C2").Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);


                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "FacturasEnIngreso.xlsx");
                }
            }
        }
    }
}
