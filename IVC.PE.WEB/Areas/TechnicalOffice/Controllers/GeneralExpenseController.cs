using ClosedXML.Excel;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.GeneralExpenseViewModels;
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
    [Route("oficina-tecnica/gastos-generales")]
    public class GeneralExpenseController : BaseController
    {

        public GeneralExpenseController(IvcDbContext context,
            ILogger<GeneralExpenseController> logger): base(context, logger)
        {

        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var expenses = await _context.GeneralExpenses
                .OrderBy(x=>x.OrderNumber)
                .Select(x => new GeneralExpenseViewModel
                {
                    Id = x.Id,
                    BudgetTitleId = x.BudgetTitleId,
                    ItemNumber = x.ItemNumber,
                    Description =  x.Description,
                    Quantity = x.Quantity.ToString("N", CultureInfo.InvariantCulture),
                    Unit = x.Unit,
                    Metered = x.Metered.ToString("N", CultureInfo.InvariantCulture),
                    Price = x.Price.ToString("N", CultureInfo.InvariantCulture),
                    Parcial = x.Parcial.ToString("N", CultureInfo.InvariantCulture)
                }).AsNoTracking()
                .ToListAsync();

            return Ok(expenses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var expenses = await _context.GeneralExpenses
                .Select(x => new GeneralExpenseViewModel
                {
                    Id = x.Id,
                    BudgetTitleId = x.BudgetTitleId,
                    ItemNumber = x.ItemNumber,
                    Description = x.Description,
                    Quantity = x.Quantity.ToString("N", CultureInfo.InvariantCulture),
                    Unit = x.Unit,
                    Metered = x.Metered.ToString("N", CultureInfo.InvariantCulture),
                    Price = x.Price.ToString("N", CultureInfo.InvariantCulture),
                    Parcial = x.Parcial.ToString("N", CultureInfo.InvariantCulture)
                }).AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return Ok(expenses);
        }

        [HttpPost("importar-datos")]
        public async Task<IActionResult> ImportData(IFormFile file, GeneralExpenseViewModel model)
        {
            var aux = _context.GeneralExpenses.Count();

            var expenses = new List<GeneralExpense>();
            using (var mem = new MemoryStream())
            {
                await file.CopyToAsync(mem);
                using (var workBook = new XLWorkbook(mem))
                {
                    var workSheet = workBook.Worksheets.FirstOrDefault();
                    var counter = 11;

                    while (!workSheet.Cell($"B{counter}").IsEmpty())
                    {
                        var expense = new GeneralExpense();
                        expense.Id = Guid.NewGuid();
                        expense.BudgetTitleId = model.BudgetTitleId;
                        expense.OrderNumber = aux;
                        expense.ItemNumber = workSheet.Cell($"B{counter}").GetString();
                        expense.Description = workSheet.Cell($"C{counter}").GetString();

                        var quantityExcel = workSheet.Cell($"D{counter}").GetString();
                        if (!string.IsNullOrEmpty(quantityExcel))
                        {
                            if (Double.TryParse(quantityExcel, out double quantity))
                                expense.Quantity = quantity;
                        }

                        expense.Unit = workSheet.Cell($"E{counter}").GetString();

                        var meteredExcel = workSheet.Cell($"F{counter}").GetString();
                        if (!string.IsNullOrEmpty(meteredExcel))
                        {
                            if (Double.TryParse(meteredExcel, out double metered))
                                expense.Metered = metered;
                        }

                        var unitPriceExcel = workSheet.Cell($"G{counter}").GetString();
                        if (!string.IsNullOrEmpty(unitPriceExcel))
                        {
                            if (Double.TryParse(unitPriceExcel, out double unitPrice))
                                expense.Price = unitPrice;
                        }

                        var totalPriceExcel = workSheet.Cell($"H{counter}").GetString();
                        if (!string.IsNullOrEmpty(totalPriceExcel))
                        {
                            if (Double.TryParse(totalPriceExcel, out double totalPrice))
                                expense.Parcial = totalPrice;
                        }

                        expenses.Add(expense);
                        counter++;
                        aux++;
                    }
                    await _context.GeneralExpenses.AddRangeAsync(expenses);
                    await _context.SaveChangesAsync();
                }
                mem.Close();
            }

            return Ok();
        }

        [HttpGet("excel-carga-masiva")]
        public FileResult ExportExcelMassiveLoad()
        {
            string fileName = "GastosGeneralesCargaMasiva.xlsx";
            using (XLWorkbook wb = new XLWorkbook())
            {
                var workSheet = wb.Worksheets.Add("CargaMasiva");

                workSheet.Cell($"B9").Value = "Item";

                workSheet.Cell($"C9").Value = "Descripción";

                workSheet.Cell($"D9").Value = "Cantidad";

                workSheet.Cell($"E9").Value = "Und.";

                workSheet.Cell($"F9").Value = "Metrado";

                workSheet.Cell($"G9").Value = "Precio S/";

                workSheet.Cell($"H9").Value = "Parcial S/";

                workSheet.Cell($"B11").Value = "Info Aquí";
                workSheet.Cell($"B11").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Column(1).Width = 1;
                workSheet.Column(2).Width = 13;
                workSheet.Column(3).Width = 70;
                workSheet.Column(4).Width = 13;
                workSheet.Column(5).Width = 10;
                workSheet.Column(6).Width = 13;
                workSheet.Column(7).Width = 13;
                workSheet.Column(8).Width = 13;

                workSheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                workSheet.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                workSheet.Range("B9:H9").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                workSheet.Range("B9:H9").Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);


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
