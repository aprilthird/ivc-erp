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
    [Route("equipos/valorizacion-transporte")]
    public class EquipmentMachineryTransportValueController : BaseController
    {
        public EquipmentMachineryTransportValueController(IvcDbContext context,
        ILogger<EquipmentMachineryTransportValueController> logger) : base(context, logger)
        {

        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(int year,int month,Guid providerId)
        {
            var pId = GetProjectId();

            SqlParameter param1 = new SqlParameter("@Year", year);
            SqlParameter param2 = new SqlParameter("@Month", month);
            SqlParameter param3 = new SqlParameter("@Id", providerId);


            var data = await _context.Set<UspEquipmentTransportVal>().FromSqlRaw("execute EquipmentMachinery_uspTransportVal @Year, @Month, @Id"
                ,param1
                ,param2
                ,param3)
         .IgnoreQueryFilters()
         .ToListAsync();
            //if(providerId.HasValue)
            //data = data.Where(x => x.EquipmentProviderId == providerId.Value).ToList();
            //if (transportId.HasValue)
            //    data = data.Where(x => x.EquipmentMachineryTypeTransportId == transportId.Value).ToList();

            //if (equipmentProviderId.HasValue)
            //    data = data.Where(x => x.EquipmentProviderId == equipmentProviderId.Value).ToList();





            //if (month == 1)
            //    data = data.Where(x => x.FoldingId != null && x.MonthPartDate == 1).ToList();
            //if (month == 2)
            //    data = data.Where(x => x.FoldingId != null && x.MonthPartDate == 2).ToList();
            //if (month == 3)
            //    data = data.Where(x => x.FoldingId != null && x.MonthPartDate == 3).ToList();
            //if (month == 4)
            //    data = data.Where(x => x.FoldingId != null && x.MonthPartDate == 4).ToList();
            //if (month == 5)
            //    data = data.Where(x => x.FoldingId != null && x.MonthPartDate == 5).ToList();
            //if (month == 6)
            //    data = data.Where(x => x.FoldingId != null && x.MonthPartDate == 6).ToList();
            //if (month == 7)
            //    data = data.Where(x => x.FoldingId != null && x.MonthPartDate == 7).ToList();
            //if (month == 8)
            //    data = data.Where(x => x.FoldingId != null && x.MonthPartDate == 8).ToList();
            //if (month == 9)
            //    data = data.Where(x => x.FoldingId != null && x.MonthPartDate == 9).ToList();
            //if (month == 10)
            //    data = data.Where(x => x.FoldingId != null && x.MonthPartDate == 10).ToList();
            //if (month == 11)
            //    data = data.Where(x => x.FoldingId != null && x.MonthPartDate == 11).ToList();
            //if (month == 12)
            //    data = data.Where(x => x.FoldingId != null && x.MonthPartDate == 12).ToList();

            //if (year == 2021)
            //    data = data.Where(x => x.FoldingId != null && x.YearPartDate == 2021).ToList();

            //if (year == 2020)
            //    data = data.Where(x => x.FoldingId != null && x.YearPartDate == 2020).ToList();

            //if (year == 2019)
            //    data = data.Where(x => x.FoldingId != null && x.YearPartDate == 2019).ToList();

            //if (year == 2018)
            //    data = data.Where(x => x.FoldingId != null && x.YearPartDate == 2018).ToList();

            //if (week == 1)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 1).ToList();
            //if (week == 2)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 2).ToList();
            //if (week == 3)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 3).ToList();
            //if (week == 4)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 4).ToList();
            //if (week == 5)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 5).ToList();
            //if (week == 6)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 6).ToList();
            //if (week == 7)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 7).ToList();
            //if (week == 8)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 8).ToList();
            //if (week == 9)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 9).ToList();
            //if (week == 10)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 10).ToList();
            //if (week == 11)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 11).ToList();
            //if (week == 12)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 12).ToList();
            //if (week == 13)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 13).ToList();
            //if (week == 14)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 1).ToList();
            //if (week == 1)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 14).ToList();
            //if (week == 15)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 15).ToList();
            //if (week == 16)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 16).ToList();
            //if (week == 17)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 17).ToList();
            //if (week == 18)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 18).ToList();
            //if (week == 19)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 19).ToList();
            //if (week == 20)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 20).ToList();
            //if (week == 21)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 21).ToList();
            //if (week == 22)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 22).ToList();
            //if (week == 23)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 23).ToList();
            //if (week == 24)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 24).ToList();
            //if (week == 25)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 25).ToList();
            //if (week == 26)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 26).ToList();
            //if (week == 27)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 27).ToList();
            //if (week == 28)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 28).ToList();
            //if (week == 29)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 29).ToList();
            //if (week == 30)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 30).ToList();
            //if (week == 31)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 31).ToList();
            //if (week == 32)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 32).ToList();
            //if (week == 33)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 33).ToList();
            //if (week == 34)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 34).ToList();
            //if (week == 35)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 35).ToList();
            //if (week == 36)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 36).ToList();
            //if (week == 37)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 37).ToList();
            //if (week == 38)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 38).ToList();
            //if (week == 39)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 39).ToList();
            //if (week == 40)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 40).ToList();
            //if (week == 41)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 41).ToList();
            //if (week == 42)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 42).ToList();
            //if (week == 43)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 43).ToList();
            //if (week == 44)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 44).ToList();
            //if (week == 45)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 45).ToList();
            //if (week == 46)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 46).ToList();
            //if (week == 47)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 47).ToList();
            //if (week == 48)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 48).ToList();
            //if (week == 49)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 49).ToList();
            //if (week == 50)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 50).ToList();
            //if (week == 51)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 51).ToList();
            //if (week == 52)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 52).ToList();
            //if (week == 53)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 53).ToList();



            return Ok(data);
        }

        [HttpGet("detalle-folding-excel")]
        public async Task<IActionResult> ExcelReport(int year, int month, Guid providerId)
        {
            SqlParameter param1 = new SqlParameter("@Year", year);
            SqlParameter param2 = new SqlParameter("@Month", month);
            SqlParameter param3 = new SqlParameter("@Id", providerId);

            var data = await _context.Set<UspEquipmentTransportDetailFolding>()
                .FromSqlRaw("execute EquipmentMachinery_uspTransportPartFoldingsExcel @Year, @Month, @Id"
                , param1
                , param2
                , param3)
                .AsNoTracking()
                .IgnoreQueryFilters()
                .ToListAsync();

            var FathersId = data.Select(x => x.FatherId).Distinct().ToList();




            var data2 = await _context.Set<UspEquipmentTransportVal>().FromSqlRaw("execute EquipmentMachinery_uspTransportVal @Year, @Month, @Id"
                , param1
                , param2
                , param3)
                .AsNoTracking()
         .IgnoreQueryFilters()
         .ToListAsync();

            using (XLWorkbook wb = new XLWorkbook())
            { int countequips = 1;


                var ws = wb.Worksheets.Add("Resumen");
                var count = 3;
                var mnth = data2.Select(x => x.Month).FirstOrDefault();


                switch (mnth)
                {
                    case 1:
                    ws.Cell($"A1").Value = "Listado de Valorización de " + data2.Select(x => x.TradeName).FirstOrDefault() + " en el mes de Enero del "+ data2.Select(x => x.Year).FirstOrDefault();
                        ws.Range($"A1:E1").Merge();
                        break;
                    case 2:
                        ws.Cell($"A1").Value = "Listado de Valorización de " + data2.Select(x => x.TradeName).FirstOrDefault() + " en el mes de Febrero del " + data2.Select(x => x.Year).FirstOrDefault();
                        ws.Range($"A1:E1").Merge();
                        break;

                    case 3:
                        ws.Cell($"A1").Value = "Listado de Valorización de " + data2.Select(x => x.TradeName).FirstOrDefault() + " en el mes de Marzo del " + data2.Select(x => x.Year).FirstOrDefault();
                        break;

                    case 4:
                        ws.Cell($"A1").Value = "Listado de Valorización de " + data2.Select(x => x.TradeName).FirstOrDefault() + " en el mes de Abril del " + data2.Select(x => x.Year).FirstOrDefault();
                        break;

                    case 5:
                        ws.Cell($"A1").Value = "Listado de Valorización de " + data2.Select(x => x.TradeName).FirstOrDefault() + " en el mes de Mayo del " + data2.Select(x => x.Year).FirstOrDefault();
                        break;

                    case 6:
                        ws.Cell($"A1").Value = "Listado de Valorización de " + data2.Select(x => x.TradeName).FirstOrDefault() + " en el mes de Junio del " + data2.Select(x => x.Year).FirstOrDefault();
                        break;

                    case 7:
                        ws.Cell($"A1").Value = "Listado de Valorización de " + data2.Select(x => x.TradeName).FirstOrDefault() + " en el mes de Julio del " + data2.Select(x => x.Year).FirstOrDefault();
                        break;

                    case 8:
                        ws.Cell($"A1").Value = "Listado de Valorización de " + data2.Select(x => x.TradeName).FirstOrDefault() + " en el mes de Agosto del " + data2.Select(x => x.Year).FirstOrDefault();
                        break;

                    case 9:
                        ws.Cell($"A1").Value = "Listado de Valorización de " + data2.Select(x => x.TradeName).FirstOrDefault() + " en el mes de Setiembre del " + data2.Select(x => x.Year).FirstOrDefault();
                        break;

                    case 10:
                        ws.Cell($"A1").Value = "Listado de Valorización de " + data2.Select(x => x.TradeName).FirstOrDefault() + " en el mes de Octubre del " + data2.Select(x => x.Year).FirstOrDefault();
                        break;

                    case 11:
                        ws.Cell($"A1").Value = "Listado de Valorización de " + data2.Select(x => x.TradeName).FirstOrDefault() + " en el mes de Noviembre del " + data2.Select(x => x.Year).FirstOrDefault();
                        break;

                    case 12:
                        ws.Cell($"A1").Value = "Listado de Valorización de " + data2.Select(x => x.TradeName).FirstOrDefault() + " en el mes de Diciembre del " + data2.Select(x => x.Year).FirstOrDefault();
                        break;

                    

                }

                ws.Cell($"A{count}").Value = "Proveedor";
                ws.Cell($"B{count}").Value = "Tipo de Equipo";
                ws.Cell($"C{count}").Value = "Nombre del Equipo";

                ws.Cell($"D{count}").Value = "Operador";

                ws.Cell($"E{count}").Value = "Cantidad de Días";
                ws.Cell($"F{count}").Value = "Precio Unitario (Día) S/";
                ws.Cell($"G{count}").Value = "Monto S/";
                ws.Cell($"H{count}").Value = "IGV S/";
                ws.Cell($"I{count}").Value = "Total S/";
                SetRowBorderStyle(ws, count, "I");

                count = 4;
                var totalammount = 0.00;
                var igv = 0.00;
                var ammount = 0.00;
                var days = 0;
                foreach ( var first in data2)
                {
                    ws.Cell($"A{count}").Value = first.TradeName;
                    ws.Cell($"B{count}").Value = first.Description;
                    ws.Cell($"C{count}").Value = first.Brand+"-"+first.Model+"-"+first.EquipmentPlate;
                    switch (first.HiringType)
                    {
                        case 1:
                            ws.Cell($"D{count}").Value = first.OperatorName;
                            break;
                        case 2:
                            ws.Cell($"D{count}").Value = first.FromOtherName;
                            break;
                        case 3:
                            ws.Cell($"D{count}").Value = first.WorkerName +" "+ first.WorkerMiddleName+" "+ first.WorkerPaternalSurName + " " +first.WorkerMaternalSurName;
                            break;
                    }
                    
                    ws.Cell($"E{count}").Value = first.FoldingNumber;
                    ws.Cell($"F{count}").Value = first.UnitPriceFormatted;
                    ws.Cell($"G{count}").Value = first.AmmountFormatted;
                    ws.Cell($"H{count}").Value = first.IgvFormatted;
                    ws.Cell($"I{count}").Value = first.TotalAmmountFormatted;
                    totalammount += first.TotalAmmount;
                    ammount += first.Ammount;
                    igv += first.Igv;
                    days += first.FoldingNumber;
                    count++;
                    SetRowBorderStyle(ws, count-1, "J");
                    ws.Cell($"F{count - 1}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell($"G{count - 1}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell($"H{count - 1}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell($"I{count-1}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                }

                ws.Cell($"E{count}").Value = days;
                ws.Cell($"G{count}").Value = String.Format(new CultureInfo("es-PE"), "{0:C}", ammount);
                ws.Cell($"H{count}").Value = String.Format(new CultureInfo("es-PE"), "{0:C}", igv);
                ws.Cell($"I{count}").Value = String.Format(new CultureInfo("es-PE"), "{0:C}", totalammount);
                ws.Columns().AdjustToContents();

                ws.Cell($"G{count}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                ws.Cell($"H{count}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                ws.Cell($"I{count}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                ws.Range($"E{count}:E{count}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range($"E{count}:E{count}").Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                ws.Range($"G{count}:G{count}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range($"G{count}:G{count}").Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                ws.Range($"H{count}:H{count}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range($"H{count}:H{count}").Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                ws.Range($"I{count}:I{count}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range($"I{count}:I{count}").Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                foreach (var fid in FathersId)
                {
                    var equipname = data2.Where(X => X.FatherId == fid).FirstOrDefault();
                    var datafather = data.Where(x => x.FatherId == fid).ToList();
                    var data2father = data2.Where(x => x.FatherId == fid).ToList();
                    var workSheet = wb.Worksheets.Add(equipname.Brand+"-"+equipname.Model+"-"+equipname.EquipmentPlate);
                    var counter = 1;
                    var totgallon = 0.00;
                    SetProjectHeaderStyle(workSheet, counter, "J");
                    SetProjectHeaderStyle(workSheet, counter + 1, "J");
                    CreateHeaders(workSheet, counter);

                    counter = 3;

                    foreach (var item in datafather)
                    {
                        workSheet.Cell($"A{counter}").Value = item.PartDate;
                        workSheet.Cell($"B{counter}").Value = item.PartNumber;
                        workSheet.Cell($"C{counter}").Value = item.ActualName;
                        workSheet.Cell($"D{counter}").Value = item.Activities;
                        workSheet.Cell($"E{counter}").Value = item.InitMileage;
                        workSheet.Cell($"F{counter}").Value = item.EndMileage;
                        workSheet.Cell($"G{counter}").Value = item.AcumulatedRoute;
                        workSheet.Cell($"H{counter}").Value = item.Gallon.HasValue? item.Gallon.Value: 0;
                        workSheet.Cell($"I{counter}").Value = item.UserName;
                        workSheet.Cell($"J{counter}").Value = item.Code;
                        SetColumnsHeaderStyle(workSheet, counter, "J");
                        SetRowBorderStyle(workSheet, counter, "J");
                        totgallon += item.Gallon.HasValue ? item.Gallon.Value : 0.00;
                        counter++;
                    }

                    var item2 = data2father;
                    var itemtemp = datafather;
                    workSheet.Cell($"G{counter}").Value = item2.Select(x => x.AcumulatedMileage);
                    workSheet.Cell($"H{counter}").Value = totgallon;
                    SetTotalCellStyle(workSheet, counter);
                    workSheet.Cell($"A{counter + 2}").Value = "KILOMETRAJE INICIAL";
                    workSheet.Cell($"A{counter + 3}").Value = "KILOMETRAJE FINAL";
                    workSheet.Cell($"A{counter + 4}").Value = "TOTAL RECORRIDO MES(KM)";
                    workSheet.Cell($"A{counter + 5}").Value = "COMBUSTIBLE (GLN)";
                    workSheet.Cell($"A{counter + 6 }").Value = "DIAS TRABAJADOS";
                    workSheet.Cell($"B{counter + 2}").Value = item2.Select(x => x.LastInitMileage);
                    workSheet.Cell($"B{counter + 3}").Value = item2.Select(x => x.LastEndMileage);
                    workSheet.Cell($"B{counter + 4}").Value = item2.Select(x => x.AcumulatedMileage);
                    workSheet.Cell($"B{counter + 5}").Value = totgallon;
                    workSheet.Cell($"B{counter + 6}").Value = item2.Select(x => x.FoldingNumber);

                    SetRowBorderStyle(workSheet, counter + 2, "B");
                    SetRowBorderStyle(workSheet, counter + 3, "B");
                    SetRowBorderStyle(workSheet, counter + 4, "B");
                    SetRowBorderStyle(workSheet, counter + 5, "B");
                    SetRowBorderStyle(workSheet, counter + 6, "B");
                    workSheet.Cell($"A{counter + 7}").Value = "TARIFA DE ALQUILER POR MES";
                    workSheet.Cell($"A{counter + 7}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    workSheet.Cell($"B{counter + 7}").Value = "SUB TOTAL";
                    workSheet.Cell($"B{counter + 7}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    workSheet.Cell($"C{counter + 7}").Value = "IGV 18%";
                    workSheet.Cell($"C{counter + 7}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    workSheet.Cell($"D{counter + 7}").Value = "TOTAL";
                    workSheet.Cell($"D{counter + 7}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    SetRowBorderStyle(workSheet, counter + 7, "D");
                    SetRowBorderStyle(workSheet, counter + 8, "D");


                    workSheet.Cell($"A{counter + 8}").Value = item2.Select(x => x.UnitPriceMonthlyFormatted);
                    workSheet.Cell($"A{counter + 8}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    workSheet.Cell($"B{counter + 8}").Value = item2.Select(x => x.AmmountFormatted);
                    workSheet.Cell($"B{counter + 8}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    workSheet.Cell($"C{counter + 8}").Value = item2.Select(x => x.IgvFormatted);
                    workSheet.Cell($"C{counter + 8}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    workSheet.Cell($"D{counter + 8}").Value = item2.Select(x => x.TotalAmmountFormatted);
                    workSheet.Cell($"D{counter + 8}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    workSheet.Row(counter+8).Style.NumberFormat.Format = "_(* #,##0.00_);_(* (#,##0.00);_(* \" - \"??_);_(@_)";
                    workSheet.Column(5).Style.NumberFormat.Format = "_(* #,##0.00_);_(* (#,##0.00);_(* \" - \"??_);_(@_)";
                    workSheet.Column(6).Style.NumberFormat.Format = "_(* #,##0.00_);_(* (#,##0.00);_(* \" - \"??_);_(@_)";
                    workSheet.Column(7).Style.NumberFormat.Format = "_(* #,##0.00_);_(* (#,##0.00);_(* \" - \"??_);_(@_)";
                    workSheet.Column(8).Style.NumberFormat.Format = "_(* #,##0.00_);_(* (#,##0.00);_(* \" - \"??_);_(@_)";

                    workSheet.Columns().AdjustToContents();
                    workSheet.Column(2).Width = 25;
                    workSheet.Column(5).Width = 10;
                    workSheet.Column(6).Width = 10;
                    workSheet.Column(9).Width = 30;
                    workSheet.Column(8).Width = 20;

                    countequips++;

                    SetRowBorderStyle(workSheet, 1, "I");
                    SetRowBorderStyle(workSheet, 2, "I");
                }
                using (MemoryStream stream = new MemoryStream())
                {
                    var frs = data2.FirstOrDefault();
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet","Valorización "+frs.TradeName+".xlsx");
                }

            }

        }

        private void CreateHeaders(IXLWorksheet ws, int counter)
        {
            ws.Cell($"A{counter}").Value = "FECHA";
            ws.Range($"A{counter}:A{counter+1}").Merge();

            ws.Cell($"B{counter}").Value = "PARTE DIARIO";
            ws.Range($"B{counter}:B{counter + 1}").Merge();

            ws.Cell($"C{counter}").Value = "CONDUCTOR";
            ws.Range($"C{counter}:C{counter + 1}").Merge();

            ws.Cell($"D{counter}").Value = "ACTIVIDAD";
            ws.Range($"D{counter}:D{counter + 1}").Merge();

            ws.Cell($"E{counter}").Value = "KM. RECORRIDO";
            ws.Range($"E{counter}:G{counter}").Merge();
            ws.Cell($"E{counter+1}").Value = "INICIAL";
            ws.Cell($"F{counter+1}").Value = "FINAL";
            ws.Cell($"G{counter+1}").Value = "TOTAL RECORRIDO";

            ws.Cell($"H{counter}").Value = "COMBUSTIBLE";
            ws.Range($"H{counter}:H{counter + 1}").Merge();

            ws.Cell($"I{counter}").Value = "UNIDAD A CARGO DE";
            ws.Range($"I{counter}:I{counter+1}").Merge();

            ws.Cell($"J{counter}").Value = "FASE";
            ws.Range($"J{counter}:J{counter+1}").Merge();





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

            ws.Cell($"H{rowCount}").Style.Fill.SetBackgroundColor(XLColor.FromArgb(184, 204, 228));
            ws.Cell($"H{rowCount}").Style.Font.Bold = true;
            ws.Cell($"H{rowCount}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Cell($"H{rowCount}").Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        }

        [HttpGet("monto")]
        public async Task<IActionResult> GetTotalInstalled(int year, int month, Guid providerId)
        {
            SqlParameter param1 = new SqlParameter("@Year", year);
            SqlParameter param2 = new SqlParameter("@Month", month);
            SqlParameter param3 = new SqlParameter("@Id", providerId);

            var data = await _context.Set<UspEquipmentTransportVal>().FromSqlRaw("execute EquipmentMachinery_uspTransportVal @Year, @Month, @Id"
                           , param1
                           , param2
                           , param3)
                    .IgnoreQueryFilters()
                    .ToListAsync();


            var SumaAmmount = 0.0;
            var SumaIgv = 0.0;
            var SumaTotalAmmount = 0.0;
            foreach (var item in data)
            {
                SumaAmmount += item.Ammount;

                SumaIgv += item.Igv;
                SumaTotalAmmount += item.TotalAmmount;
            }
            string str = String.Format(new CultureInfo("es-PE"), "{0:C}", SumaAmmount);
            string str2 = String.Format(new CultureInfo("es-PE"), "{0:C}", SumaIgv);
            string str3 = String.Format(new CultureInfo("es-PE"), "{0:C}", SumaTotalAmmount);
            return Ok(str);
        }
        [HttpGet("igv")]
        public async Task<IActionResult> GetTotalInstalled2(int year, int month, Guid providerId)
        {
            SqlParameter param1 = new SqlParameter("@Year", year);
            SqlParameter param2 = new SqlParameter("@Month", month);
            SqlParameter param3 = new SqlParameter("@Id", providerId);

            var data = await _context.Set<UspEquipmentTransportVal>().FromSqlRaw("execute EquipmentMachinery_uspTransportVal @Year, @Month, @Id"
                           , param1
                           , param2
                           , param3)
                    .IgnoreQueryFilters()
                    .ToListAsync();


            var SumaAmmount = 0.0;
            var SumaIgv = 0.0;
            var SumaTotalAmmount = 0.0;
            foreach (var item in data)
            {
                SumaAmmount += item.Ammount;

                SumaIgv += item.Igv;
                SumaTotalAmmount += item.TotalAmmount;
            }
            string str = String.Format(new CultureInfo("es-PE"), "{0:C}", SumaAmmount);
            string str2 = String.Format(new CultureInfo("es-PE"), "{0:C}", SumaIgv);
            string str3 = String.Format(new CultureInfo("es-PE"), "{0:C}", SumaTotalAmmount);
            return Ok(str2);
        }
        [HttpGet("monto-total")]
        public async Task<IActionResult> GetTotalInstalled3(int year, int month, Guid providerId)
        {
            SqlParameter param1 = new SqlParameter("@Year", year);
            SqlParameter param2 = new SqlParameter("@Month", month);
            SqlParameter param3 = new SqlParameter("@Id", providerId);

            var data = await _context.Set<UspEquipmentTransportVal>().FromSqlRaw("execute EquipmentMachinery_uspTransportVal @Year, @Month, @Id"
                           , param1
                           , param2
                           , param3)
                    .IgnoreQueryFilters()
                    .ToListAsync();


            var SumaAmmount = 0.0;
            var SumaIgv = 0.0;
            var SumaTotalAmmount = 0.0;
            foreach (var item in data)
            {
                SumaAmmount += item.Ammount;

                SumaIgv += item.Igv;
                SumaTotalAmmount += item.TotalAmmount;
            }
            string str = String.Format(new CultureInfo("es-PE"), "{0:C}", SumaAmmount);
            string str2 = String.Format(new CultureInfo("es-PE"), "{0:C}", SumaIgv);
            string str3 = String.Format(new CultureInfo("es-PE"), "{0:C}", SumaTotalAmmount);
            return Ok(str3);

            //returnOk(new {str,str2,str3})
        }


    }
}
