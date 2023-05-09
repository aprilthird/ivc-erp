using ClosedXML.Excel;
using EFCore.BulkExtensions;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.OCBudgetViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.TECHNICAL_OFFICE)]
    [Route("oficina-tecnica/oc-presupuestos")]
    public class OCBudgetController : BaseController
    {
        public OCBudgetController(IvcDbContext context,
            ILogger<BudgetController> logger) : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? budgetTypeId = null, int group = 0)
        {
            var pId = GetProjectId();

            var query = _context.OCBudgets
               .Include(x => x.ProjectFormula)
               .Where(x => x.ProjectFormula.ProjectId == pId);

            if (projectFormulaId.HasValue)
                query = query.Where(x => x.ProjectFormulaId == projectFormulaId);
            if (budgetTitleId.HasValue)
                query = query.Where(x => x.BudgetTitleId == budgetTitleId);
            if (budgetTypeId.HasValue)
                query = query.Where(x => x.BudgetTypeId == budgetTypeId);
            if (group != 0)
                query = query.Where(x => x.Group == group);

            var oCBudgets = await query
                .OrderBy(x => x.OrderNumber)
                .Select(x => new OCBudgetViewModel
                {
                    Id = x.Id,
                    NumberItem = x.NumberItem,
                    Description = x.Description,
                    Unit = x.Unit,
                    Metered = x.Metered.ToString("N", CultureInfo.InvariantCulture),
                    UnitPrice = x.UnitPrice.ToString("N", CultureInfo.InvariantCulture),
                    TotalPrice = x.TotalPrice.ToString("N", CultureInfo.InvariantCulture),
                }).AsNoTracking()
                .ToListAsync();

            return Ok(oCBudgets);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var oCBudget = await _context.OCBudgets
                .Where(x => x.Id == id)
                .Select(x => new OCBudgetViewModel
                {
                    Id = x.Id,
                    NumberItem = x.NumberItem,
                    Description = x.Description,
                    Unit = x.Unit,
                    ProjectFormulaId = x.ProjectFormulaId,
                    BudgetTitleId = x.BudgetTitleId,
                    BudgetTypeId = x.BudgetTypeId,
                    Group = x.Group,
                    Metered = x.Metered.ToString(),
                    UnitPrice = x.UnitPrice.ToString(),
                    TotalPrice = x.TotalPrice.ToString("F", CultureInfo.InvariantCulture),
                }).AsNoTracking().FirstOrDefaultAsync();

            return Ok(oCBudget);
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, OCBudgetViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var oCBudget = await _context.OCBudgets
                .FirstOrDefaultAsync(x => x.Id == id);

            oCBudget.NumberItem = model.NumberItem;
            oCBudget.Description = model.Description;
            oCBudget.Unit = model.Unit ?? "";
            oCBudget.Metered = model.Metered.ToDoubleString();
            oCBudget.UnitPrice = model.UnitPrice.ToDoubleString();
            oCBudget.TotalPrice = model.TotalPrice.ToDoubleString();

            await _context.SaveChangesAsync();

            return Ok(oCBudget);
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var oCBudget = await _context.OCBudgets.FirstOrDefaultAsync(x => x.Id == id);

            if (oCBudget == null)
                return BadRequest("No se ha encontrado el budget");

            _context.OCBudgets.Remove(oCBudget);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("importar-datos")]
        public async Task<IActionResult> ImportData(IFormFile file, OCBudgetViewModel model)
        {
            //NumberFormatInfo provider = new NumberFormatInfo();
            //provider.NumberDecimalSeparator = ",";
            //provider.NumberGroupSeparator = ".";
            //provider.NumberGroupSizes = new int[] { 3 };

            var aux = _context.OCBudgets.Count();

            var budgets = new List<OCBudget>();
            using (var mem = new MemoryStream())
            {
                await file.CopyToAsync(mem);
                using (var workBook = new XLWorkbook(mem))
                {
                    var workSheet = workBook.Worksheets.FirstOrDefault();
                    var counter = 11;

                    while (!workSheet.Cell($"B{counter}").IsEmpty())
                    {
                        var budget = new OCBudget();
                        budget.Id = Guid.NewGuid();
                        budget.NumberItem = workSheet.Cell($"B{counter}").GetString();

                        budget.ProjectFormulaId = model.ProjectFormulaId;
                        budget.BudgetTitleId = model.BudgetTitleId;
                        budget.OrderNumber = aux;
                        budget.BudgetTypeId = model.BudgetTypeId;
                        budget.Group = model.Group;
                        budget.Description = workSheet.Cell($"C{counter}").GetString();
                        budget.Unit = workSheet.Cell($"D{counter}").GetString();

                        var meteredExcel = workSheet.Cell($"E{counter}").GetString();
                        if (!string.IsNullOrEmpty(meteredExcel))
                        {
                            if (Double.TryParse(meteredExcel, out double metered))
                                budget.Metered = metered;
                        }

                        var unitPriceExcel = workSheet.Cell($"F{counter}").GetString();
                        if (!string.IsNullOrEmpty(unitPriceExcel))
                        {
                            if (Double.TryParse(unitPriceExcel, out double unitPrice))
                                budget.UnitPrice = unitPrice;
                        }

                        var totalPriceExcel = workSheet.Cell($"G{counter}").GetString();
                        if (!string.IsNullOrEmpty(totalPriceExcel))
                        {
                            if (Double.TryParse(totalPriceExcel, out double totalPrice))
                                budget.TotalPrice = totalPrice;
                        }

                        budgets.Add(budget);
                        counter++;
                        aux++;
                    }

                }
                mem.Close();
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                var bulkConfig = new BulkConfig { PreserveInsertOrder = true, SetOutputIdentity = true };
                await _context.BulkInsertAsync(budgets);

                transaction.Commit();
            }

            return Ok();
        }


        [HttpGet("excel-carga-masiva")]
        public FileResult ExportExcelMassiveLoad()
        {
            string fileName = "OCPresupuestoCargaMasiva.xlsx";
            using (XLWorkbook wb = new XLWorkbook())
            {
                var workSheet = wb.Worksheets.Add("CargaMasiva");

                workSheet.Cell($"B9").Value = "Item";

                workSheet.Cell($"C9").Value = "Descripción";

                workSheet.Cell($"D9").Value = "Und.";

                workSheet.Cell($"E9").Value = "Metrado";

                workSheet.Cell($"F9").Value = "Precio S/";

                workSheet.Cell($"G9").Value = "Parcial S/";

                workSheet.Cell($"B11").Value = "Info Aquí";
                workSheet.Cell($"B11").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Column(1).Width = 1;
                workSheet.Column(2).Width = 13;
                workSheet.Column(3).Width = 70;
                workSheet.Column(4).Width = 10;
                workSheet.Column(5).Width = 13;
                workSheet.Column(6).Width = 13;
                workSheet.Column(7).Width = 13;

                workSheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                workSheet.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                workSheet.Range("B9:G9").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                workSheet.Range("B9:G9").Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);


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
