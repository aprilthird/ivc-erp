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
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectFormulaViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.MeasurementUnitViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyFamilyViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyGroupViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BudgetInputViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BudgetTitleViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IVC.PE.WEB.Areas.TechnicalOffice.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.TECHNICAL_OFFICE)]
    [Route("oficina-tecnica/insumos")]
    public class BudgetInputController : BaseController
    {
        public BudgetInputController(IvcDbContext context,
            ILogger<BudgetInputController> logger) : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? measurementUnitId = null, Guid? supplyFamilyId = null, Guid? supplyGroupId = null,
            Guid? projectFormulaId = null, Guid? budgetTitleId = null)
        {
            var paginationParameter = GetPaginationParameters();

            var query = _context.BudgetInputs
                .Where(x => x.ProjectId == GetProjectId())
                .AsNoTracking()
                .AsQueryable();

            var totalRecords = await query.CountAsync();

            if (!string.IsNullOrEmpty(paginationParameter.Search))
            {
                query = query.Where(x => x.Code.Contains(paginationParameter.Search) ||
                        x.Description.Contains(paginationParameter.Search));
            }

            if (measurementUnitId.HasValue)
                query = query.Where(x => x.MeasurementUnitId == measurementUnitId.Value);
            if (supplyFamilyId.HasValue)
                query = query.Where(x => x.SupplyFamilyId == supplyFamilyId.Value);
            if (supplyGroupId.HasValue)
                query = query.Where(x => x.SupplyGroupId == supplyGroupId.Value);
            if (projectFormulaId.HasValue)
                query = query.Where(x => x.ProjectFormulaId == projectFormulaId.Value);
            if (budgetTitleId.HasValue)
                query = query.Where(x => x.BudgetTitleId == budgetTitleId.Value);

            var data = await query
                .Skip(paginationParameter.CurrentNumber)
                .Take(paginationParameter.RecordsPerPage)
                .Select(x => new BudgetInputViewModel
                {
                    Id = x.Id,
                    Code = x.Code,
                    Description = x.Description,
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
                    MeasurementUnitId = x.MeasurementUnitId,
                    MeasurementUnit = new MeasurementUnitViewModel
                    {
                        Abbreviation = x.MeasurementUnit.Abbreviation
                    },
                    SaleUnitPrice = x.SaleUnitPrice.ToString("N", CultureInfo.InvariantCulture),
                    GoalUnitPrice = x.GoalUnitPrice.ToString("N", CultureInfo.InvariantCulture),
                    Metered = x.Metered.ToString("N", CultureInfo.InvariantCulture),
                    Parcial = x.Parcial.ToString("N", CultureInfo.InvariantCulture),
                    ProjectFormula = new ProjectFormulaViewModel
                    {
                        Code = x.ProjectFormula.Code,
                        Name = x.ProjectFormula.Name
                    },
                    BudgetTitle = new BudgetTitleViewModel
                    {
                        Name = x.BudgetTitle.Name
                    }
                }).AsNoTracking().ToListAsync();

            var result = GetPagedList(totalRecords, await query.CountAsync(), data);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.BudgetInputs
                .Where(x => x.Id == id)
                .Select(x => new BudgetInputViewModel
                {
                    Id = x.Id,
                    Code = x.Code,
                    Description = x.Description,
                    SupplyFamilyId = x.SupplyFamilyId,
                    SupplyGroupId = x.SupplyGroupId,
                    MeasurementUnitId = x.MeasurementUnitId,
                    SaleUnitPrice = x.SaleUnitPrice.ToString(),
                    GoalUnitPrice = x.GoalUnitPrice.ToString(),
                    Metered = x.Metered.ToString(),
                    Parcial = x.Parcial.ToString(),
                    ProjectFormulaId = x.ProjectFormulaId,
                    BudgetTypeId = x.BudgetTypeId,
                    BudgetTitleId = x.BudgetTitleId,
                    Group = x.Group
                }).AsNoTracking()
                .FirstOrDefaultAsync();
            return Ok(data);
        }

        [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.FULL_ACCESS)]
        [HttpPost("crear")]
        public async Task<IActionResult> Create(BudgetInputViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var parcial = Math.Round(model.SaleUnitPrice.ToDoubleString() * model.Metered.ToDoubleString(), 2);

            var budgetInput = new BudgetInput
            {
                Code = model.Code,
                Description = model.Description,
                MeasurementUnitId = model.MeasurementUnitId,
                SupplyFamilyId = model.SupplyFamilyId,
                SupplyGroupId = model.SupplyGroupId,
                SaleUnitPrice = model.SaleUnitPrice.ToDoubleString(),
                //GoalUnitPrice = model.GoalUnitPrice.ToDoubleString(),
                Metered = model.Metered.ToDoubleString(),
                Parcial = parcial,
                ProjectFormulaId = model.ProjectFormulaId,
                BudgetTitleId = model.BudgetTitleId,
                BudgetTypeId = model.BudgetTypeId,
                Group = model.Group,
                ProjectId = Guid.Parse(HttpContext.Session.GetString("ProjectId"))
            };
            await _context.BudgetInputs.AddAsync(budgetInput);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.FULL_ACCESS)]
        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, BudgetInputViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var parcial = Math.Round(model.Metered.ToDoubleString() * model.SaleUnitPrice.ToDoubleString(), 2);

            var budgetInput = await _context.BudgetInputs.FindAsync(id);
            budgetInput.Code = model.Code;
            budgetInput.Description = model.Description;
            budgetInput.MeasurementUnitId = model.MeasurementUnitId;
            budgetInput.SupplyFamilyId = model.SupplyFamilyId;
            budgetInput.SupplyGroupId = model.SupplyGroupId;
            budgetInput.SaleUnitPrice = model.SaleUnitPrice.ToDoubleString();
            //budgetInput.GoalUnitPrice = model.GoalUnitPrice.ToDoubleString();
            budgetInput.Metered = model.Metered.ToDoubleString();
            budgetInput.Parcial = parcial;
            budgetInput.BudgetTitleId = model.BudgetTitleId;
            budgetInput.ProjectFormulaId = model.ProjectFormulaId;
            budgetInput.BudgetTypeId = model.BudgetTypeId;
            budgetInput.Group = model.Group;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.FULL_ACCESS)]
        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var budgetInput = await _context.BudgetInputs.FirstOrDefaultAsync(x => x.Id == id);
            if (budgetInput == null)
                return BadRequest($"Insumo con Id '{id}' no encontrado.");
            _context.BudgetInputs.Remove(budgetInput);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.FULL_ACCESS)]
        [HttpPost("importar-datos")]
        public async Task<IActionResult> Import(IFormFile file, BudgetInputViewModel model)
        {
            using (var mem = new MemoryStream())
            {
                await file.CopyToAsync(mem);
                using (var workBook = new XLWorkbook(mem))
                {
                    var workSheet = workBook.Worksheets.FirstOrDefault();
                    var counter = 3;
                    var defaultMeasurementUnit = await _context.MeasurementUnits.FirstOrDefaultAsync();
                    var defaultSupplyFamily = await _context.SupplyFamilies.FirstOrDefaultAsync(x => x.Code == "01003");
                    var newInputs = new List<BudgetInput>();
                    var existingInputs = new List<BudgetInput>();
                    while (!workSheet.Cell($"B{counter}").IsEmpty())
                    {
                        var inputCodeStr = workSheet.Cell($"B{counter}").GetString();
                        var input = (await _context.BudgetInputs
                            .AsNoTracking().FirstOrDefaultAsync(x => x.Code == inputCodeStr && x.ProjectFormulaId == model.ProjectFormulaId
                            && x.BudgetTitleId == model.BudgetTitleId && x.BudgetTypeId == model.BudgetTypeId))
                            ?? new BudgetInput();

                        if (input.Id == Guid.Empty)
                        {
                            input.ProjectId = GetProjectId();
                            input.Code = inputCodeStr;
                            input.BudgetTypeId = model.BudgetTypeId;
                            input.BudgetTitleId = model.BudgetTitleId;
                            input.ProjectFormulaId = model.ProjectFormulaId;
                            input.Group = model.Group;
                            input.Description = workSheet.Cell($"C{counter}").GetString();

                            /*
                            var unitPriceGoalStr = workSheet.Cell($"G{counter}").GetString();
                            if (!string.IsNullOrEmpty(unitPriceGoalStr))
                            {
                                if (Double.TryParse(unitPriceGoalStr, out double unitPrice))
                                    input.GoalUnitPrice = unitPrice;
                            }*/

                            var measurementUnitStr = workSheet.Cell($"D{counter}").GetString();
                            if (!string.IsNullOrEmpty(measurementUnitStr))
                            {
                                var measurementUnit = await _context.MeasurementUnits.FirstOrDefaultAsync(x => x.Abbreviation == measurementUnitStr || x.Abbreviation == measurementUnitStr.ToUpper());
                                if (measurementUnit == null)
                                    return BadRequest($"La Unidad de Medición de la celda D{counter} es incorrecto");
                                else
                                    input.MeasurementUnitId = measurementUnit.Id;
                            }

                            var meteredStr = workSheet.Cell($"E{counter}").GetString();
                            if (!string.IsNullOrEmpty(meteredStr))
                            {
                                if (Double.TryParse(meteredStr, out double metered))
                                    input.Metered = metered;
                            }

                            var unitPriceSaleStr = workSheet.Cell($"F{counter}").GetString();
                            if (!string.IsNullOrEmpty(unitPriceSaleStr))
                            {
                                if (Double.TryParse(unitPriceSaleStr, out double unitPrice))
                                    input.SaleUnitPrice = unitPrice;
                            }

                            var parcialStr = workSheet.Cell($"G{counter}").GetString();
                            if (!string.IsNullOrEmpty(parcialStr))
                            {
                                if (Double.TryParse(parcialStr, out double parcial))
                                    input.Parcial = parcial;

                            }

                            input.Parcial = Math.Round(input.SaleUnitPrice * input.Metered, 2);

                            var supplyGroupStr = workSheet.Cell($"G{counter}").GetString();
                            if (!string.IsNullOrEmpty(supplyGroupStr))
                            {
                                var supplyGroup = await _context.SupplyGroups.AsNoTracking().FirstOrDefaultAsync(x => x.Name == supplyGroupStr);
                                if (supplyGroup == null)
                                    return BadRequest($"El Grupo de la celda H{counter} es incorrecto");
                                else
                                    input.SupplyGroupId = supplyGroup.Id;
                            }

                            var supplyFamilyStr = workSheet.Cell($"H{counter}").GetString();
                            if (!string.IsNullOrEmpty(supplyFamilyStr))
                            {
                                var supplyFamily = await _context.SupplyFamilies.AsNoTracking().FirstOrDefaultAsync(x => x.Name == supplyFamilyStr);
                                if (supplyFamily == null)
                                    return BadRequest($"La Familia de la celda I{counter} es incorrecto");
                                else
                                    input.SupplyFamilyId = supplyFamily.Id;
                            }
                            newInputs.Add(input);
                        }
                        else
                            existingInputs.Add(input);
                        ++counter;
                    }
                    await _context.BudgetInputs.AddRangeAsync(newInputs);
                    //_context.BudgetInputs.UpdateRange(existingInputs);
                    await _context.SaveChangesAsync();
                }
                mem.Close();
            }
            return Ok();
        }

        [HttpGet("exportar")]
        public async Task<IActionResult> Export()
        {
            var dt = new DataTable("INSUMOS");
            dt.Columns.Add("Código S10", typeof(string));
            dt.Columns.Add("Descripción", typeof(string));
            dt.Columns.Add("Unidad", typeof(string));
            dt.Columns.Add("Familia", typeof(string));
            dt.Columns.Add("Grupo", typeof(string));
            dt.Columns.Add("Fórmula", typeof(string));
            dt.Columns.Add("Título de Presupuesto", typeof(string));
            dt.Columns.Add("Metrados", typeof(string));
            dt.Columns.Add("P.U. Venta", typeof(string));
            dt.Columns.Add("Parcial (S/)", typeof(string));


            var data = await _context.BudgetInputs
                .Include(x => x.MeasurementUnit)
                .Include(x => x.SupplyFamily)
                .Include(x => x.SupplyGroup)
                .Include(x => x.ProjectFormula)
                .Include(x => x.BudgetTitle)
                .AsNoTracking()
                .ToListAsync();
            data.ForEach(item =>
            {
                dt.Rows.Add(item.Code, item.Description, item.MeasurementUnit.Abbreviation,
                    item.SupplyFamily?.Code, item.SupplyGroup?.Code, item.ProjectFormula.Code + " - " + item.ProjectFormula.Name, 
                    item.BudgetTitle.Abbreviation, item.Metered.ToString("N", CultureInfo.InvariantCulture),
                    item.SaleUnitPrice.ToString("N", CultureInfo.InvariantCulture),
                    item.Parcial.ToString("N", CultureInfo.InvariantCulture));
            });

            var fileName = "Insumos (Presupuesto).xlsx";
            using (var wb = new XLWorkbook())
            {
                var workSheet = wb.Worksheets.Add(dt);

                workSheet.Column(1).Width = 15;
                workSheet.Column(2).Width = 80;
                workSheet.Column(3).Width = 9;
                workSheet.Column(4).Width = 9;
                workSheet.Column(5).Width = 9;
                workSheet.Column(6).Width = 50;
                workSheet.Column(7).Width = 40;
                workSheet.Column(8).Width = 14;
                workSheet.Column(9).Width = 12;
                workSheet.Column(10).Width = 21;

                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Seek(0, SeekOrigin.Begin);
                    Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        [HttpGet("excel-carga-masiva")]
        public FileResult ExportExcelMassiveLoad()
        {
            string fileName = "InsumosCargaMasiva.xlsx";
            using (XLWorkbook wb = new XLWorkbook())
            {
                var workSheet = wb.Worksheets.Add("CargaMasiva");

                workSheet.Cell($"B2").Value = "CODIGO S10";

                workSheet.Cell($"C2").Value = "MATERIAL";

                workSheet.Cell($"D2").Value = "UND";

                workSheet.Cell($"E2").Value = "METRADO";

                workSheet.Cell($"F2").Value = "P.U.";

                workSheet.Cell($"G2").Value = "GRUPO";

                workSheet.Cell($"H2").Value = "Familia";

                workSheet.Column(1).Width = 3;
                workSheet.Column(2).Width = 15;
                workSheet.Column(3).Width = 60;
                workSheet.Column(4).Width = 7;
                workSheet.Column(5).Width = 15;
                workSheet.Column(6).Width = 10;
                workSheet.Column(8).Width = 18;
                workSheet.Column(9).Width = 63;

                workSheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                workSheet.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                workSheet.Range("B2:H2").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                workSheet.Range("B2:H2").Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);

                var groups = _context.SupplyGroups.AsNoTracking().ToList();

                DataTable dtGroups = new DataTable();
                dtGroups.TableName = "Grupo de Insumos";
                dtGroups.Columns.Add("Código", typeof(string));
                dtGroups.Columns.Add("Descripción", typeof(string));
                foreach (var item in groups)
                    dtGroups.Rows.Add(item.Code, item.Name);
                dtGroups.AcceptChanges();

                var workSheetGroup = wb.Worksheets.Add(dtGroups);

                workSheetGroup.Column(2).Width = 70;

                var families = _context.SupplyFamilies.AsNoTracking().ToList();

                DataTable dtFamilies = new DataTable();
                dtFamilies.TableName = "Familia de Grupo";
                dtFamilies.Columns.Add("Código", typeof(string));
                dtFamilies.Columns.Add("Descripción", typeof(string));
                foreach (var item in families)
                    dtFamilies.Rows.Add(item.Code, item.Name);
                dtFamilies.AcceptChanges();

                var workSheetFamily = wb.Worksheets.Add(dtFamilies);

                workSheetFamily.Column(2).Width = 70;


                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        //Metodos adicionales

        [HttpGet("obtener-unidad/{id}")]
        public async Task<IActionResult> GetMeasureUnit(Guid id)
        {
            var unit = (await _context.BudgetInputs
                .Include(x => x.MeasurementUnit)
                .FirstOrDefaultAsync(x => x.Id == id))
                    .MeasurementUnit
                    .Abbreviation;

            return Ok(unit);
        }

        [HttpGet("parciales")]
        public async Task<IActionResult> GetParcial(Guid? measurementUnitId = null, Guid? supplyFamilyId = null, Guid? supplyGroupId = null,
            Guid? projectFormulaId = null, Guid? budgetTitleId = null)
        {
            var query = _context.BudgetInputs
                .Where(x => x.ProjectId == GetProjectId())
                .AsNoTracking()
                .AsQueryable();

            if (measurementUnitId.HasValue)
                query = query.Where(x => x.MeasurementUnitId == measurementUnitId.Value);
            if (supplyFamilyId.HasValue)
                query = query.Where(x => x.SupplyFamilyId == supplyFamilyId.Value);
            if (supplyGroupId.HasValue)
                query = query.Where(x => x.SupplyGroupId == supplyGroupId.Value);
            if (projectFormulaId.HasValue)
                query = query.Where(x => x.ProjectFormulaId == projectFormulaId.Value);
            if (budgetTitleId.HasValue)
                query = query.Where(x => x.BudgetTitleId == budgetTitleId.Value);

            var data = await query.ToListAsync();

            var Suma = 0.0;

            foreach (var item in data)
                Suma += item.Parcial;

            return Ok(Suma.ToString("N", CultureInfo.InvariantCulture));
        }

        [HttpDelete("eliminar-filtro")]
        public async Task<IActionResult> DeleteByFilters(Guid? supplyFamilyId = null, Guid? supplyGroupId = null)
        {
            var pId = GetProjectId();

            var query = _context.BudgetInputs
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

            _context.BudgetInputs.RemoveRange(data);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}