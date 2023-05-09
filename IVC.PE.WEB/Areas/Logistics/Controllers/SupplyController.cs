using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Logistics;
using IVC.PE.WEB.Areas.Logistics.ViewModels.MeasurementUnitViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.OrderViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.ProviderViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyFamilyViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyGroupViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IVC.PE.WEB.Areas.Logistics.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Logistics.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.LOGISTICS)]
    [Route("logistica/insumos")]
    public class SupplyController : BaseController
    {
        public SupplyController(IvcDbContext context,
            ILogger<SupplyController> logger)
            : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? measurementUnitId = null, Guid? supplyFamilyId = null, Guid? supplyGroupId = null)
        {
            var search = Request.Query[ConstantHelpers.Datatable.ServerSide.SentParameters.SEARCH_VALUE].ToString();
            var currentNumber = Convert.ToInt32(Request.Query[ConstantHelpers.Datatable.ServerSide.SentParameters.PAGING_FIRST_RECORD]);
            var recordsPerPage = Convert.ToInt32(Request.Query[ConstantHelpers.Datatable.ServerSide.SentParameters.RECORDS_PER_DRAW]);

            var query = _context.Supplies
                .OrderBy(x => x.SupplyFamily.Code)
                .ThenBy(x => x.SupplyGroup.Code)
                .ThenBy(x => x.CorrelativeCode)
                .AsNoTracking()
                .AsQueryable();

            if (measurementUnitId.HasValue)
                query = query.Where(x => x.MeasurementUnitId == measurementUnitId.Value);
            if (supplyFamilyId.HasValue)
                query = query.Where(x => x.SupplyFamilyId == supplyFamilyId.Value);
            if (supplyGroupId.HasValue)
                query = query.Where(x => x.SupplyGroupId == supplyGroupId.Value);

            var totalRecords = await query.CountAsync();

            if (!string.IsNullOrEmpty(search))
            {
                if (int.TryParse(search, out int searchInteger))
                    query = query.Where(x => x.Description.Contains(search) ||
                        x.SupplyFamily.Name.Contains(search) ||
                        x.SupplyGroup.Name.Contains(search) || 
                        x.CorrelativeCode == searchInteger);
                else
                    query = query.Where(x => x.Description.Contains(search) ||
                        x.SupplyFamily.Name.Contains(search) ||
                        x.SupplyGroup.Name.Contains(search));
            }

            var data = await query
                .Skip(currentNumber)
                .Take(recordsPerPage)
                .Select(x => new SupplyViewModel
                {
                    Id = x.Id,
                    Description = x.Description,
                    MeasurementUnitId = x.MeasurementUnitId,
                    MeasurementUnit = new MeasurementUnitViewModel
                    {
                        Abbreviation = x.MeasurementUnit.Abbreviation
                    },
                    SupplyFamilyId = x.SupplyFamilyId,
                    SupplyFamily = new SupplyFamilyViewModel
                    {
                        Code = x.SupplyFamily.Code,
                        Name = x.SupplyFamily.Name
                    },
                    SupplyGroupId = x.SupplyGroupId,
                    SupplyGroup = new SupplyGroupViewModel
                    {
                        Code = x.SupplyGroup.Code,
                        Name = x.SupplyGroup.Name
                    },
                    CorrelativeCode = x.CorrelativeCode,
                    Status = x.Status
                }).ToListAsync();

            return Ok(new
            {
                draw = ConstantHelpers.Datatable.ServerSide.SentParameters.DRAW_COUNTER,
                recordsTotal = totalRecords,
                recordsFiltered = await query.CountAsync(),
                data
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var supply = await _context.Supplies
                .Where(x => x.Id == id)
                .Select(x => new SupplyViewModel
                {
                    Id = x.Id,
                    Description = x.Description,
                    MeasurementUnitId = x.MeasurementUnitId,
                    SupplyFamilyId = x.SupplyFamilyId,
                    SupplyGroupId = x.SupplyGroupId,
                    CorrelativeCode = x.CorrelativeCode
                }).FirstOrDefaultAsync();
            return Ok(supply);
        }

        [Authorize(Roles = ConstantHelpers.Permission.Logistics.FULL_ACCESS)]
        [HttpPost("crear")]
        public async Task<IActionResult> Create(SupplyViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var table = await _context.Supplies
                .Where(x => x.SupplyFamilyId == model.SupplyFamilyId && x.SupplyGroupId == model.SupplyGroupId).ToListAsync();
            var maxCorrelative = 0;
            if (table.Count() > 0)
            {
                maxCorrelative = table.Select(x => x.CorrelativeCode).Max();
            }
            

            var supply = new Supply
            {
                Description = model.Description,
                MeasurementUnitId = model.MeasurementUnitId,
                SupplyFamilyId = model.SupplyFamilyId,
                SupplyGroupId = model.SupplyGroupId,
                CorrelativeCode = maxCorrelative + 1
            };
            await _context.Supplies.AddAsync(supply);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.Logistics.FULL_ACCESS)]
        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, SupplyViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var supply = await _context.Supplies.FindAsync(id);
            supply.Description = model.Description;
            supply.MeasurementUnitId = model.MeasurementUnitId;
            if(supply.SupplyFamilyId != model.SupplyFamilyId || supply.SupplyGroupId != model.SupplyGroupId)
            {
                var table = await _context.Supplies
                .Where(x => x.SupplyFamilyId == model.SupplyFamilyId && x.SupplyGroupId == model.SupplyGroupId).ToListAsync();
                var maxCorrelative = 0;
                if(table.Count() > 0)
                {
                    maxCorrelative = table.Select(x => x.CorrelativeCode).Max();
                }         
                supply.CorrelativeCode = maxCorrelative + 1;
            }
            supply.SupplyFamilyId = model.SupplyFamilyId;
            supply.SupplyGroupId = model.SupplyGroupId;

            

            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.Logistics.FULL_ACCESS)]
        [HttpDelete("eliminar")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var supply = await _context.Supplies.FirstOrDefaultAsync(x => x.Id == id);
            if (supply == null)
                return BadRequest($"Insumo con Id '{id}' no encontrado.");
            _context.Supplies.Remove(supply);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.Logistics.FULL_ACCESS)]
        [HttpPost("importar")]
        public async Task<IActionResult> Import(IFormFile file)
        {
            var unidades = await _context.MeasurementUnits
                .ToListAsync();
            var familias = await _context.SupplyFamilies
                .ToListAsync();
            var grupos = await _context.SupplyGroups
                .Include(x => x.SupplyFamily)
                .ToListAsync();
            var insumos = await _context.Supplies
                .AsNoTracking()
                .ToListAsync();
            using (var mem = new MemoryStream())
            {
                await file.CopyToAsync(mem);
                using (var workBook = new XLWorkbook(mem))
                {
                    var workSheet = workBook.Worksheets.FirstOrDefault(x => x.Name.ToUpper().Contains("INSUMOS"));
                    var counter = 7;
                    var supplies = new List<Supply>();
                    while (!workSheet.Cell($"C{counter}").IsEmpty())
                    {
                        var supply = new Supply();
                        supply.Description = workSheet.Cell($"C{counter}").GetString();
                        
                        var measurementUnitStr = workSheet.Cell($"D{counter}").GetString();
                        var measurementUnit = unidades.FirstOrDefault(x => x.Abbreviation == measurementUnitStr);

                        if (measurementUnit == null)
                            return BadRequest("No se ha encontrado la medida de medida de la fila " + counter);
                        supply.MeasurementUnitId = measurementUnit.Id;

                        var supplyFamilyStr = workSheet.Cell($"E{counter}").GetString();
                        var supplyFamily = familias.FirstOrDefault(x => x.Code == supplyFamilyStr);
                        if (supplyFamily == null)
                            return BadRequest("No se ha encontrado la familia de la fila " + counter);
                        supply.SupplyFamilyId = supplyFamily.Id;

                        var supplyGroupStr = workSheet.Cell($"F{counter}").GetString();
                        var supplyGroup = grupos.FirstOrDefault(x => x.Code == supplyGroupStr && supplyFamilyStr == x.SupplyFamily.Code);
                        if (supplyGroup == null)
                            return BadRequest("No se ha encontrado el grupo de la fila " + counter);
                        supply.SupplyGroupId = supplyGroup.Id;

                        var auxCorrelative = 0;
                        var auxSupplies = insumos
                            .Where(x => x.SupplyFamilyId == supplyFamily.Id && x.SupplyGroupId == supplyGroup.Id)
                            .Select(x => x.CorrelativeCode).ToList();
                        if (auxSupplies.Count() > 0)
                        {
                            auxCorrelative = auxSupplies.Max();
                        }
                        supply.CorrelativeCode = auxCorrelative + 1;
                        /*
                        var correlativeCodeStr = workSheet.Cell($"G{counter}").GetString();
                        int.TryParse(correlativeCodeStr, out int correlativeCodeInteger);
                        supply.CorrelativeCode = correlativeCodeInteger;
                        */
                        supplies.Add(supply);
                        insumos.Add(supply);
                        ++counter;
                    }
                    await _context.Supplies.AddRangeAsync(supplies);
                    await _context.SaveChangesAsync();
                }
                mem.Close();
            }
            return Ok();
        }

        
        [HttpGet("exportar")]
        public async Task<IActionResult> Export()
        {
            var dt = new DataTable("CATÁLOGO DE INSUMOS");
            dt.Columns.Add("Código Artículo", typeof(string));
            dt.Columns.Add("Descripción", typeof(string));
            dt.Columns.Add("Unidad", typeof(string));
            dt.Columns.Add("Familia", typeof(string));
            dt.Columns.Add("Grupo", typeof(string));
            dt.Columns.Add("Correlativo", typeof(string));


            var data = await _context.Supplies
                .Include(x => x.MeasurementUnit)
                .Include(x => x.SupplyFamily)
                .Include(x => x.SupplyGroup)
                .AsNoTracking()
                .ToListAsync();
            data.ForEach(item =>
            {
                dt.Rows.Add(item.FullCode, item.Description, item.MeasurementUnit.Abbreviation,
                    item.SupplyFamily?.Code, item.SupplyGroup?.Code, item.CorrelativeCode.ToString("000"));
            });

            var fileName = "Catálogo Insumos.xlsx";
            using (var wb = new XLWorkbook())
            {
                var workSheet = wb.Worksheets.Add(dt);

                workSheet.Column(1).Width = 15;
                workSheet.Column(2).Width = 80;
                workSheet.Column(3).Width = 9;
                workSheet.Column(4).Width = 9;
                workSheet.Column(5).Width = 9;

                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Seek(0, SeekOrigin.Begin);
                    Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }
        

        [Authorize(Roles = ConstantHelpers.Permission.Logistics.FULL_ACCESS)]
        [HttpGet("excel-modelo")]
        public FileResult GetExcelSample()
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                var workSheet = wb.Worksheets.Add("INSUMOS");

                workSheet.Cell($"C6").Value = "Descripción";
                workSheet.Cell($"D6").Value = "Unidad";
                workSheet.Cell($"E6").Value = "Familia";
                workSheet.Cell($"F6").Value = "Grupo";
                //workSheet.Cell($"G6").Value = "Correlativo";

                workSheet.Column("E").Style.NumberFormat.Format = "@";
                workSheet.Column("F").Style.NumberFormat.Format = "@";
                //workSheet.Column("G").Style.NumberFormat.Format = "@";

                //workSheet.Range("C6:G6").Style.Fill.SetBackgroundColor(XLColor.Yellow);
                workSheet.Range("C6:F6").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"C7").Value = "PC COMPATIBLE INTEL CORE I7-8700, 16 GB MEMORIA RAM, 1 TB";
                workSheet.Cell($"D7").Value = "UND";
                workSheet.Cell($"E7").Value = "01001";
                workSheet.Cell($"F7").Value = "001";
                //workSheet.Cell($"G7").Value = "001";

                workSheet.Cell($"C8").Value = "PC COMPATIBLE INTEL CORE I5-8400, 08 GB MEMORIA RAM, 1 TB";
                workSheet.Cell($"D8").Value = "UND";
                workSheet.Cell($"E8").Value = "01001";
                workSheet.Cell($"F8").Value = "001";
                //workSheet.Cell($"G8").Value = "002";

                workSheet.Columns().AdjustToContents();

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Insumos.xlsx");
                }
            }
        }

        [HttpDelete("eliminar-filtro")]
        public async Task<IActionResult> DeleteByFilters(Guid? supplyFamilyId = null, Guid? supplyGroupId = null)
        {
            var pId = GetProjectId();

            var query = _context.Supplies
                .Where(x => x.SupplyFamilyId != null);

            if (supplyFamilyId.HasValue)
                query = query.Where(x => x.SupplyFamilyId == supplyFamilyId);
            else
                return BadRequest("No se ha escogido la familia");

            if (supplyGroupId.HasValue)
                query = query.Where(x => x.SupplyGroupId == supplyGroupId);
            else
                return BadRequest("No se ha escogido el grupo");

            var data = await query.ToListAsync();

            _context.Supplies.RemoveRange(data);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("actulizar")]
        public async Task<IActionResult> UpdateStatus()
        {
            var items = await _context.OrderItems
                .Include(x => x.Supply)
                .ToListAsync();

            foreach (var item in items)
                item.Supply.Status = ConstantHelpers.Supply.Status.IN_ORDER;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("obtener-unidad/{id}")]
        public async Task<IActionResult> GetMeasurementUnit(Guid id)
        {
            var unit = await _context.Supplies
                .Include(x => x.MeasurementUnit)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (unit == null)
                return BadRequest();

            return Ok(unit.MeasurementUnit
                    .Abbreviation);
        }

        [HttpGet("ordenes/listar")]
        public async Task<IActionResult> GetOrders(Guid id)
        {
            var ordIds = await _context.OrderItems
                .Where(x => x.SupplyId == id)
                .Select(x => x.OrderId)
                .ToListAsync();

            var result = new List<OrderViewModel>();

            var orders = await _context.Orders
                .Include(x => x.Provider)
                .Include(x => x.Warehouse.WarehouseType.Project)
                .Where(x => ordIds.Contains(x.Id))
                //&& x.Status == ConstantHelpers.Logistics.RequestOrder.Status.APPROVED)
                .ToListAsync();

            foreach(var order in orders)
            {
                var soles = 0.0;
                var dolares = 0.0;

                if (order.Currency == 1)
                {
                    soles = order.Parcial;
                    if (order.ExchangeRate > 0)
                        dolares = Math.Round(order.Parcial / order.ExchangeRate, 2);
                }
                else
                {
                    soles = Math.Round(order.Parcial * order.ExchangeRate, 2);
                    dolares = order.Parcial;
                }

                result.Add(new OrderViewModel
                {
                    Provider = new ProviderViewModel
                    {
                        Tradename = order.Provider.Tradename
                    },
                    Status = order.Status,
                    CorrelativeCodeStr = order.Warehouse.WarehouseType.Project.CostCenter
                    + "-" + order.CorrelativeCode.ToString("D4")
                    + (order.ReviewDate.HasValue ? "-" + order.ReviewDate.Value.Year.ToString() : ""),
                    Date = order.Date.ToDateString(),
                    Currency = order.Currency,
                    ExchangeRate = order.ExchangeRate.ToString(CultureInfo.InvariantCulture),
                    Parcial = soles.ToString("N2", CultureInfo.InvariantCulture),
                    DolarParcial = dolares.ToString("N2", CultureInfo.InvariantCulture)
                });
            }

            return Ok(result);
        }
    }
}