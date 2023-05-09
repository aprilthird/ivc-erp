using ClosedXML.Excel;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.UspModels.EquipmentMachinery;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.EquipmentMachinery.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.EquipmentMachinery.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.EQUIPMENT_MACHINERY)]
    [Route("equipos/valorizacion-combustible-mixto")]
    public class UspFuelMixController : BaseController
    {
        public UspFuelMixController(IvcDbContext context,
        ILogger<UspFuelMixController> logger) : base(context, logger)
        {

        }
        public IActionResult Index() => View();

        [HttpGet("detalle-folding-excel-junto")]
        public async Task<IActionResult> ExcelReportAgrrouped(int week, int year)
        {
            SqlParameter param1 = new SqlParameter("@Week", week);
            SqlParameter param2 = new SqlParameter("@Year", year);




            var data = await _context.Set<UspFuelTransportValDetailExcel>().FromSqlRaw("execute EquipmentMachinery_uspFuelTransportValDetailExcel @Week"
                , param1)
                .AsNoTracking()
         .IgnoreQueryFilters()
         .ToListAsync();

            var data2 = await _context.Set<UspFuelTransportValDetailExcel>().FromSqlRaw("execute EquipmentMachinery_uspFuelMachValDetailExcel @Week"
    , param1)
    .AsNoTracking()
.IgnoreQueryFilters()
.ToListAsync();

            var data3 = await _context.Set<UspFuelDates>().FromSqlRaw("execute EquipmentMachinery_uspFuelDates @Week, @Year"
                , param1
                , param2)
                .AsNoTracking()
         .IgnoreQueryFilters()
         .ToListAsync();

            var d3 = data3.FirstOrDefault();

            using (XLWorkbook wb = new XLWorkbook())
            {




                var workSheet = wb.Worksheets.Add("Resumen");

                var count2 = 2;
                workSheet.Cell($"A{count2}").Value = "Fecha";
                workSheet.Cell($"B{count2}").Value = "Total (gln)";
                workSheet.Cell($"C{count2}").Value = "Precio (x gln)";
                workSheet.Cell($"D{count2}").Value = "Total S/.";
                SetRowBorderStyle(workSheet, count2, "D");
                count2 = 3;

                var tot = 0.00;



                var tot1 = data.Select(x => x.TotalGallon).FirstOrDefault() + data2.Select(x => x.TotalGallon).FirstOrDefault();
                var tot2 = data.Select(x => x.TotalGallon).Skip(1).FirstOrDefault() + data2.Select(x => x.TotalGallon).Skip(1).FirstOrDefault();
                var tot3 = data.Select(x => x.TotalGallon).Skip(2).FirstOrDefault() + data2.Select(x => x.TotalGallon).Skip(1).FirstOrDefault();
                var tot4 = data.Select(x => x.TotalGallon).Skip(3).FirstOrDefault() + data2.Select(x => x.TotalGallon).Skip(1).FirstOrDefault();
                var tot5 = data.Select(x => x.TotalGallon).Skip(4).FirstOrDefault() + data2.Select(x => x.TotalGallon).Skip(1).FirstOrDefault();
                var tot6 = data.Select(x => x.TotalGallon).Skip(5).FirstOrDefault() + data2.Select(x => x.TotalGallon).Skip(1).FirstOrDefault();
                var tot7 = data.Select(x => x.TotalGallon).Skip(6).FirstOrDefault() + data2.Select(x => x.TotalGallon).Skip(1).FirstOrDefault();

                var price1 = data.Select(x => x.Price).FirstOrDefault() == double.NaN ? data2.Select(x => x.Price).FirstOrDefault() : data.Select(x => x.Price).FirstOrDefault();
                var price2 = data.Select(x => x.Price).Skip(1).FirstOrDefault() == double.NaN ? data2.Select(x => x.Price).Skip(1).FirstOrDefault() : data.Select(x => x.Price).Skip(1).FirstOrDefault();
                var price3 = data.Select(x => x.Price).Skip(2).FirstOrDefault() == double.NaN ? data2.Select(x => x.Price).Skip(2).FirstOrDefault() : data.Select(x => x.Price).Skip(2).FirstOrDefault();
                var price4 = data.Select(x => x.Price).Skip(3).FirstOrDefault() == double.NaN ? data2.Select(x => x.Price).Skip(3).FirstOrDefault() : data.Select(x => x.Price).Skip(3).FirstOrDefault();
                var price5 = data.Select(x => x.Price).Skip(4).FirstOrDefault() == double.NaN ? data2.Select(x => x.Price).Skip(4).FirstOrDefault() : data.Select(x => x.Price).Skip(4).FirstOrDefault();
                var price6 = data.Select(x => x.Price).Skip(5).FirstOrDefault() == double.NaN ? data2.Select(x => x.Price).Skip(5).FirstOrDefault() : data.Select(x => x.Price).Skip(5).FirstOrDefault();
                var price7 = data.Select(x => x.Price).Skip(6).FirstOrDefault() == double.NaN ? data2.Select(x => x.Price).Skip(6).FirstOrDefault() : data.Select(x => x.Price).Skip(6).FirstOrDefault();

                var total1 = data.Select(x => x.Total).FirstOrDefault() + data2.Select(x => x.Total).FirstOrDefault();
                var total2 = data.Select(x => x.Total).Skip(1).FirstOrDefault() + data2.Select(x => x.Total).Skip(1).FirstOrDefault();
                var total3 = data.Select(x => x.Total).Skip(2).FirstOrDefault() + data2.Select(x => x.Total).Skip(1).FirstOrDefault();
                var total4 = data.Select(x => x.Total).Skip(3).FirstOrDefault() + data2.Select(x => x.Total).Skip(1).FirstOrDefault();
                var total5 = data.Select(x => x.Total).Skip(4).FirstOrDefault() + data2.Select(x => x.Total).Skip(1).FirstOrDefault();
                var total6 = data.Select(x => x.Total).Skip(5).FirstOrDefault() + data2.Select(x => x.Total).Skip(1).FirstOrDefault();
                var total7 = data.Select(x => x.Total).Skip(6).FirstOrDefault() + data2.Select(x => x.Total).Skip(1).FirstOrDefault();




                workSheet.Cell($"A3").Value = d3.MondayDateString2;
                workSheet.Cell($"A4").Value = d3.TuesdayDateString2;
                workSheet.Cell($"A5").Value = d3.WednesdayDateString2;
                workSheet.Cell($"A6").Value = d3.ThursdayDateString2;
                workSheet.Cell($"A7").Value = d3.FridayDateString2;
                workSheet.Cell($"A8").Value = d3.SaturdayDateString2;
                workSheet.Cell($"A9").Value = d3.SundayDateString2;

                workSheet.Cell($"B3").Value = tot1;
                workSheet.Cell($"B4").Value = tot2;
                workSheet.Cell($"B5").Value = tot3;
                workSheet.Cell($"B6").Value = tot4;
                workSheet.Cell($"B7").Value = tot5;
                workSheet.Cell($"B8").Value = tot6;
                workSheet.Cell($"B9").Value = tot7;


                workSheet.Cell($"C3").Value = price1;
                workSheet.Cell($"C4").Value = price2;
                workSheet.Cell($"C5").Value = price3;
                workSheet.Cell($"C6").Value = price4;
                workSheet.Cell($"C7").Value = price5;
                workSheet.Cell($"C8").Value = price6;
                workSheet.Cell($"C9").Value = price7;

                workSheet.Cell($"D3").Value = total1;
                workSheet.Cell($"D4").Value = total2;
                workSheet.Cell($"D5").Value = total3;
                workSheet.Cell($"D6").Value = total4;
                workSheet.Cell($"D7").Value = total5;
                workSheet.Cell($"D8").Value = total6;
                workSheet.Cell($"D9").Value = total7;
                //workSheet.Cell($"B{count2}").Value = second.TotalGallon;
                //workSheet.Cell($"C{count2}").Value = second.Price;
                //workSheet.Cell($"D{count2}").Value = second.TotalFormatted;
                SetRowBorderStyle(workSheet, 3, "D");
                SetRowBorderStyle(workSheet, 4, "D");
                SetRowBorderStyle(workSheet, 5, "D");
                SetRowBorderStyle(workSheet, 6, "D");
                SetRowBorderStyle(workSheet, 7, "D");
                SetRowBorderStyle(workSheet, 8, "D");
                SetRowBorderStyle(workSheet, 9, "D");

                SetRowBorderStyle(workSheet, 11, "B");
                SetRowBorderStyle(workSheet, 12, "B");
                SetRowBorderStyle(workSheet, 13, "B");

                tot = total1 + total2 + total3 + total4 + total5 + total6 + total7;

                workSheet.Cell($"A11").Value = "Sub Total";
                workSheet.Cell($"A12").Value = "IGV";
                workSheet.Cell($"A13").Value = "Total";
                workSheet.Cell($"B11").Value = String.Format(new CultureInfo("es-PE"), "{0:C}", tot);
                workSheet.Cell($"B12").Value = String.Format(new CultureInfo("es-PE"), "{0:C}", tot*0.18);
                workSheet.Cell($"B13").Value = String.Format(new CultureInfo("es-PE"), "{0:C}", tot*1.18);

                //workSheet.Cell($"A{count2 + 2}").Value = "Sub Total";

                //workSheet.Cell($"A{count2 + 3}").Value = "Igv";
                //workSheet.Cell($"A{count2 + 4}").Value = "Total";

                //workSheet.Cell($"B{count2 + 2}").Value = String.Format(new CultureInfo("es-PE"), "{0:C}", tot);
                //workSheet.Cell($"B{count2 + 3}").Value = String.Format(new CultureInfo("es-PE"), "{0:C}", tot * 0.18);
                //workSheet.Cell($"B{count2 + 4}").Value = String.Format(new CultureInfo("es-PE"), "{0:C}", tot * 1.18);

                //SetRowBorderStyle(workSheet, count2 + 2, "B");

                //SetRowBorderStyle(workSheet, count2 + 3, "B");

                //SetRowBorderStyle(workSheet, count2 + 4, "B");
                workSheet.Columns().AdjustToContents();
                //ws.Cell($"G{count}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                //ws.Cell($"H{count}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                //ws.Cell($"I{count}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                //ws.Range($"E{count}:E{count}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                //ws.Range($"E{count}:E{count}").Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                //ws.Range($"G{count}:G{count}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                //ws.Range($"G{count}:G{count}").Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                //ws.Range($"H{count}:H{count}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                //ws.Range($"H{count}:H{count}").Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                //ws.Range($"I{count}:I{count}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                //ws.Range($"I{count}:I{count}").Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                //foreach (var fid in FathersId)
                //{
                //    var equipname = data2.Where(X => X.FatherId == fid).FirstOrDefault();
                //    var datafather = data.Where(x => x.FatherId == fid).ToList();
                //    var data2father = data2.Where(x => x.FatherId == fid).ToList();
                //    var workSheet = wb.Worksheets.Add(equipname.Brand + "-" + equipname.Model + "-" + equipname.EquipmentPlate);
                //    var counter = 1;
                //    SetProjectHeaderStyle(workSheet, counter, "I");
                //    SetProjectHeaderStyle(workSheet, counter + 1, "I");
                //    CreateHeaders(workSheet, counter);

                //    counter = 3;

                //    foreach (var item in datafather)
                //    {
                //        workSheet.Cell($"A{counter}").Value = item.PartDate;
                //        workSheet.Cell($"B{counter}").Value = item.PartNumber;
                //        workSheet.Cell($"C{counter}").Value = item.ActualName;
                //        workSheet.Cell($"D{counter}").Value = item.Activities;
                //        workSheet.Cell($"E{counter}").Value = item.InitMileage;
                //        workSheet.Cell($"F{counter}").Value = item.EndMileage;
                //        workSheet.Cell($"G{counter}").Value = item.AcumulatedRoute;
                //        workSheet.Cell($"H{counter}").Value = item.UserName;
                //        workSheet.Cell($"I{counter}").Value = item.Code;
                //        SetColumnsHeaderStyle(workSheet, counter, "I");
                //        SetRowBorderStyle(workSheet, counter, "I");

                //        counter++;
                //    }

                //    var item2 = data2father;

                //    workSheet.Cell($"G{counter}").Value = item2.Select(x => x.AcumulatedMileage);
                //    SetTotalCellStyle(workSheet, counter);
                //    workSheet.Cell($"A{counter + 2}").Value = "KILOMETRAJE INICIAL";
                //    workSheet.Cell($"A{counter + 3}").Value = "KILOMETRAJE FINAL";
                //    workSheet.Cell($"A{counter + 4}").Value = "TOTAL RECORRIDO MES(KM)";
                //    workSheet.Cell($"A{counter + 5 }").Value = "DIAS TRABAJADOS";
                //    workSheet.Cell($"B{counter + 2}").Value = item2.Select(x => x.LastInitMileage);
                //    workSheet.Cell($"B{counter + 3}").Value = item2.Select(x => x.LastEndMileage);
                //    workSheet.Cell($"B{counter + 4}").Value = item2.Select(x => x.AcumulatedMileage);
                //    workSheet.Cell($"B{counter + 5}").Value = item2.Select(x => x.FoldingNumber);

                //    SetRowBorderStyle(workSheet, counter + 2, "B");
                //    SetRowBorderStyle(workSheet, counter + 3, "B");
                //    SetRowBorderStyle(workSheet, counter + 4, "B");
                //    SetRowBorderStyle(workSheet, counter + 5, "B");
                //    workSheet.Cell($"A{counter + 6}").Value = "TARIFA DE ALQUILER POR MES";
                //    workSheet.Cell($"A{counter + 6}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //    workSheet.Cell($"B{counter + 6}").Value = "SUB TOTAL";
                //    workSheet.Cell($"B{counter + 6}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                //    workSheet.Cell($"C{counter + 6}").Value = "IGV 18%";
                //    workSheet.Cell($"C{counter + 6}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //    workSheet.Cell($"D{counter + 6}").Value = "TOTAL";
                //    workSheet.Cell($"D{counter + 6}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                //    SetRowBorderStyle(workSheet, counter + 6, "D");
                //    SetRowBorderStyle(workSheet, counter + 7, "D");


                //    workSheet.Cell($"A{counter + 7}").Value = item2.Select(x => x.UnitPriceMonthlyFormatted);
                //    workSheet.Cell($"A{counter + 7}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                //    workSheet.Cell($"B{counter + 7}").Value = item2.Select(x => x.AmmountFormatted);
                //    workSheet.Cell($"B{counter + 7}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                //    workSheet.Cell($"C{counter + 7}").Value = item2.Select(x => x.IgvFormatted);
                //    workSheet.Cell($"C{counter + 7}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                //    workSheet.Cell($"D{counter + 7}").Value = item2.Select(x => x.TotalAmmountFormatted);
                //    workSheet.Cell($"D{counter + 7}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                //    workSheet.Row(counter + 7).Style.NumberFormat.Format = "_(* #,##0.00_);_(* (#,##0.00);_(* \" - \"??_);_(@_)";
                //    workSheet.Column(5).Style.NumberFormat.Format = "_(* #,##0.00_);_(* (#,##0.00);_(* \" - \"??_);_(@_)";
                //    workSheet.Column(6).Style.NumberFormat.Format = "_(* #,##0.00_);_(* (#,##0.00);_(* \" - \"??_);_(@_)";
                //    workSheet.Column(7).Style.NumberFormat.Format = "_(* #,##0.00_);_(* (#,##0.00);_(* \" - \"??_);_(@_)";

                //    workSheet.Columns().AdjustToContents();
                //    workSheet.Column(2).Width = 25;
                //    workSheet.Column(9).Width = 25;

                //    countequips++;

                //    SetRowBorderStyle(workSheet, 1, "I");
                //    SetRowBorderStyle(workSheet, 2, "I");
                //}
                using (MemoryStream stream = new MemoryStream())
                {
                    var frs = data2.FirstOrDefault();
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Val. Combustible La Unión Semana (" + d3.FirstDay + "." + d3.FirstMonth + "- " + d3.LastDay + "." + d3.LastMonth + ") " + d3.Year + "Eq. Transporte" + ".xlsx");
                }

            }

        }

        private void CreateHeaders(IXLWorksheet ws, int counter)
        {
            ws.Cell($"A{counter}").Value = "FECHA";
            ws.Range($"A{counter}:A{counter + 1}").Merge();

            ws.Cell($"B{counter}").Value = "PARTE DIARIO";
            ws.Range($"B{counter}:B{counter + 1}").Merge();

            ws.Cell($"C{counter}").Value = "CONDUCTOR";
            ws.Range($"C{counter}:C{counter + 1}").Merge();

            ws.Cell($"D{counter}").Value = "ACTIVIDAD";
            ws.Range($"D{counter}:D{counter + 1}").Merge();

            ws.Cell($"E{counter}").Value = "KM. RECORRIDO";
            ws.Range($"E{counter}:G{counter}").Merge();
            ws.Cell($"E{counter + 1}").Value = "INICIAL";
            ws.Cell($"F{counter + 1}").Value = "FINAL";
            ws.Cell($"G{counter + 1}").Value = "TOTAL RECORRIDO";

            ws.Cell($"H{counter}").Value = "UNIDAD A CARGO DE";
            ws.Range($"H{counter}:H{counter + 1}").Merge();

            ws.Cell($"I{counter}").Value = "FASE";
            ws.Range($"I{counter}:I{counter + 1}").Merge();





        }


        private void SetProjectHeaderStyle(IXLWorksheet ws, int rowCount, string v)
        {
            ws.Cell($"A{rowCount}").Style.Font.Bold = true;
            ws.Cell($"A{rowCount}").Style.Font.FontSize = 14.0;

            ws.Range($"A{rowCount}:{v}{rowCount}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Range($"A{rowCount}:{v}{rowCount}").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            ws.Range($"A{rowCount}:{v}{rowCount}").Style.Fill.SetBackgroundColor(XLColor.FromArgb(167, 250, 229));
            ws.Range($"A{rowCount}:{v}{rowCount}").Merge();
        }

        private void SetColumnsHeaderStyle(IXLWorksheet ws, int rowCount, string v)
        {

            ws.Range($"A{rowCount}:{v}{rowCount}").Style.Font.Bold = true;
        }

        private void SetRowBorderStyle(IXLWorksheet ws, int rowCount, string v)
        {
            ws.Range($"A{rowCount}:{v}{rowCount}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range($"A{rowCount}:{v}{rowCount}").Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        }

        private void SetTotalCellStyle(IXLWorksheet ws, int rowCount)
        {
            ws.Cell($"G{rowCount}").Style.Fill.SetBackgroundColor(XLColor.FromArgb(184, 204, 228));
            ws.Cell($"G{rowCount}").Style.Font.Bold = true;
            ws.Cell($"G{rowCount}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Cell($"G{rowCount}").Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        }

    }
}
