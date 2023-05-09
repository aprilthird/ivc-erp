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
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.EquipmentMachinery.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.EquipmentMachinery.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.EQUIPMENT_MACHINERY)]
    [Route("equipos/valorizacion-maquinaria")]

    public class EquipmentMachValueController : BaseController
    {
        public EquipmentMachValueController(IvcDbContext context,
        ILogger<EquipmentMachValueController> logger) : base(context, logger)
        {

        }


        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll( int year, Guid providerId,int month, int? week = null)
        {
            var pId = GetProjectId();

            SqlParameter param1 = new SqlParameter("@WeekofYear", System.Data.SqlDbType.Int);
            param1.Value = (object)week ?? DBNull.Value;
            SqlParameter param2 = new SqlParameter("@Year", year);
            SqlParameter param3 = new SqlParameter("@Id", providerId);
            SqlParameter param4 = new SqlParameter("@Month", month);

            var data = await _context.Set<UspMachValTable>().FromSqlRaw("execute EquipmentMachinery_uspMachValTable @WeekofYear, @Year, @Id, @Month"
                , param1
                , param2
                , param3
                , param4)
 
                .IgnoreQueryFilters()
         
         .ToListAsync()
         ;

            data = data.OrderBy(x => x.TradeName).ToList();



            return Ok(data);
        }

        [HttpGet("detalle-folding-excel")]
        public async Task<IActionResult> ExcelReport(int week , int year , Guid providerId )
        {
            SqlParameter param1 = new SqlParameter("@WeekofYear", week);
            SqlParameter param2 = new SqlParameter("@Year", year);
            SqlParameter param3 = new SqlParameter("@Id", providerId);

            var data = await _context.Set<UspEquipmentMachDetailFolding>()
                .FromSqlRaw("execute EquipmentMachinery_uspMachPartFoldingsExcel @WeekofYear, @Year, @Id"
                , param1
                , param2
                , param3)
                .AsNoTracking()
                .IgnoreQueryFilters()
                .ToListAsync();

            var FathersId = data.Select(x => x.FatherId).Distinct().ToList();


            var data2 = await _context.Set<UspEquipmentMachVal>().FromSqlRaw("execute EquipmentMachinery_uspMachVal @WeekofYear, @Year, @Id"
                , param1
                , param2
                , param3)
                .AsNoTracking()
         .IgnoreQueryFilters()
         .ToListAsync();

            var data3 = await _context.Set<UspMachValCustom>().FromSqlRaw("execute EquipmentMachinery_uspMachValCustom @WeekofYear, @Year, @Id"
    , param1
    , param2
    , param3)
    .AsNoTracking()
.IgnoreQueryFilters()
.ToListAsync();

            var pid = GetProjectId();
            var project = _context.Projects.Where(x => x.Id == pid).FirstOrDefault();
            using (XLWorkbook wb = new XLWorkbook())
            {
                int countequips = 1;


                var ws = wb.Worksheets.Add("Resumen");
                var count = 3;

                ws.Cell($"A1").Value = "Listado de Valorización de " + data2.Select(x => x.TradeName).FirstOrDefault() + " en la semana " + data2.Select(x => x.Week).FirstOrDefault()+" del año "+ data2.Select(x => x.Year).FirstOrDefault(); ;
                ws.Range($"A1:E1").Merge();

                ws.Cell($"A{count}").Value = "Proveedor";
                ws.Cell($"B{count}").Value = "Tipo de Equipo";
                ws.Cell($"C{count}").Value = "Nombre del Equipo";
                ws.Cell($"D{count}").Value = "Asignado";
                ws.Cell($"E{count}").Value = "Cuadrilla";
                ws.Cell($"F{count}").Value = "Operador";
                ws.Cell($"G{count}").Value = "Horometro Acumulado";
                ws.Cell($"H{count}").Value = "Precio Unitario (hm)";
                ws.Cell($"I{count}").Value = "Monto";
                ws.Cell($"J{count}").Value = "IGV";
                ws.Cell($"K{count}").Value = "Total";
                SetRowBorderStyle(ws, count, "K");
                count = 4;
                var totalammount = 0.00;
                var igv = 0.00;
                var ammount = 0.00;
                var days = 0.00;
                foreach (var first in data2)
                {
                    ws.Cell($"A{count}").Value = first.TradeName;
                    ws.Cell($"B{count}").Value = first.Description;
                    ws.Cell($"C{count}").Value = first.Brand + "-" + first.Model + "-" + first.Plate;
                    ws.Cell($"D{count}").Value = first.UserName;
                    ws.Cell($"E{count}").Value = first.SewerCode;
                    ws.Cell($"F{count}").Value = first.ActualName;
                    ws.Cell($"G{count}").Value = first.AcumulatedHorometer;
                    ws.Cell($"H{count}").Value = first.UnitPriceFormatted;
                    ws.Cell($"I{count}").Value = first.AmmountFormatted;
                    ws.Cell($"J{count}").Value = first.IgvFormatted;
                    ws.Cell($"K{count}").Value = first.TotalAmmountFormatted;
                    totalammount += first.TotalAmmount;
                    ammount += first.Ammount;
                    igv += first.Igv;
                    days += first.CountIds;
                    count++;
                    SetRowBorderStyle(ws, count - 1, "K");

                }

                ws.Cell($"G{count}").Value = days;
                ws.Cell($"I{count}").Value = String.Format(new CultureInfo("es-PE"), "{0:C}", ammount);
                ws.Cell($"J{count}").Value = String.Format(new CultureInfo("es-PE"), "{0:C}", igv);
                ws.Cell($"K{count}").Value = String.Format(new CultureInfo("es-PE"), "{0:C}", totalammount);
                ws.Columns().AdjustToContents();


                ws.Range($"G{count}:G{count}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range($"G{count}:G{count}").Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                ws.Range($"J{count}:J{count}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range($"J{count}:J{count}").Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                ws.Range($"K{count}:K{count}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range($"K{count}:K{count}").Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                ws.Range($"I{count}:I{count}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range($"I{count}:I{count}").Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                foreach (var fid in FathersId)
                {
                    var equipname = data2.Where(X => X.FatherId == fid).FirstOrDefault();
                    var datafather = data.Where(x => x.FatherId == fid).ToList();
                    var data2father = data2.Where(x => x.FatherId == fid).ToList();
                    var data3father = data3.Where(x => x.FatherId == fid).ToList();
                    var workSheet = wb.Worksheets.Add(equipname.Brand + "-" + equipname.Model + "-" + equipname.Plate);

                    var enlace = project.LogoUrl.ToString();

                    WebClient client = new WebClient();
                    Stream img = client.OpenRead(enlace);
                    Bitmap bitmap; bitmap = new Bitmap(img);

                    Image image = (Image)bitmap;

                    workSheet.Range($"A1:A3").Merge();

                    var aux = workSheet.AddPicture(bitmap);
                    aux.MoveTo(10, 10);
                    aux.Height = 45;
                    aux.Width = 250;

                    var counter = 14;
                    var totgallon = 0.00;
                    SetProjectHeaderStyle(workSheet, counter, "L");
                    SetProjectHeaderStyle(workSheet, counter + 1, "L");
                    CreateHeaders(workSheet, counter);
                    SetColumnsHeaderStyle(workSheet, counter, "L");
                    var item2 = data2father;
                    var item3 = data3father;
                    var itemtemp = datafather;
                    counter = 16;
                    workSheet.Range($"A1:A3").Merge();
                    workSheet.Cell($"B1").Value = "GESTIÓN DE CONTROL DE EQUIPOS";
                    workSheet.Cell($"B1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    workSheet.Range($"B1:J1").Merge();
                    workSheet.Cell($"B2").Value = "CONTROL DE HORAS DE EQUIPO";
                    workSheet.Range($"B2:J3").Merge();
                    workSheet.Cell($"B2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    workSheet.Cell($"K1").Value = "Código:";
                    workSheet.Cell($"L1").Value = "JIC/GEQ-For-11";
                    workSheet.Cell($"K2").Value = "Versión:";
                    workSheet.Cell($"L2").Value = "02";
                    workSheet.Cell($"K3").Value = "Fecha:";
                    workSheet.Cell($"L3").Value = DateTime.UtcNow.ToShortDateString();
                    workSheet.Range($"K1:L3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    workSheet.Cell($"A7").Value = "OBRA:";
                    workSheet.Cell($"A8").Value = "PROVEEDOR:";
                    workSheet.Cell($"A9").Value = "EQUIPO:";
                    workSheet.Cell($"A10").Value = "CODIGO:";
                    workSheet.Cell($"A11").Value = "SERIE:";
                    workSheet.Cell($"A12").Value = "AÑO DE FABRICACIÓN:";

                    workSheet.Cell($"B7").Value = project.Name;
                    workSheet.Cell($"B8").Value = item2.Select(x => x.TradeName);
                    workSheet.Cell($"B9").Value = item2.Select(x => x.Description);
                    workSheet.Cell($"B10").Value = item2.Select(x => x.Model);
                    workSheet.Cell($"B11").Value = item2.Select(x => x.SerieNumber);
                    workSheet.Cell($"B12").Value = item2.Select(x => x.EquipmentYear);
                    //var aux = workSheet.AddPicture(bitmap);
                    //aux.MoveTo(16, 22);
                    //aux.Height = 85;
                    //aux.Width = 190;


                    foreach (var item in datafather)
                    {
                        workSheet.Cell($"A{counter}").Value = item.PartDate;
                        workSheet.Cell($"B{counter}").Value = item.PartNumber;
                        workSheet.Cell($"C{counter}").Value = item.ActualName;
                        workSheet.Cell($"D{counter}").Value = item.Description;
                        workSheet.Cell($"E{counter}").Value = item.Specific;

                        workSheet.Cell($"F{counter}").Value = item.InitHorometer;
                        workSheet.Cell($"G{counter}").Value = item.EndHorometer;
                        workSheet.Cell($"H{counter}").Value = item.Dif;
                        workSheet.Cell($"I{counter}").Value = item.Gallon.HasValue ? item.Gallon.Value : 0;

                        workSheet.Cell($"J{counter}").Value = item.UserName;
                        workSheet.Cell($"K{counter}").Value = item.Code;
                        workSheet.Cell($"L{counter}").Value = item.SewerCode;


                        SetRowBorderStyle(workSheet, counter, "L");
                        totgallon += item.Gallon.HasValue ? item.Gallon.Value : 0.00;
                        counter++;
                    }


                    workSheet.Cell($"H{counter}").Value = item2.Select(x => x.AcumulatedHorometer);
                    workSheet.Cell($"I{counter}").Value = totgallon;
                    SetTotalCellStyle(workSheet, counter);
                    workSheet.Cell($"A{counter + 2}").Value = "HOROMETRO INICIAL";
                    workSheet.Cell($"A{counter + 3}").Value = "HOROMETRO FINAL";
                    //workSheet.Cell($"A{counter + 4}").Value = "HORAS TOTALES";
                    workSheet.Cell($"A{counter + 4}").Value = "COMBUSTIBLE (GLN)";
                    workSheet.Cell($"A{counter + 5}").Value = "HORAS TRABAJADAS";
                    workSheet.Cell($"B{counter + 2}").Value = item2.Select(x => x.InitHorometer);
                    workSheet.Cell($"B{counter + 3}").Value = item2.Select(x => x.EndHorometer);
                    //workSheet.Cell($"B{counter + 4}").Value = item2.Select(x => x.AcumulatedHorometer);
                    workSheet.Cell($"B{counter + 4}").Value = totgallon;
                    workSheet.Cell($"B{counter + 5}").Value = item2.Select(x => x.CountIds);

                    SetRowBorderStyle(workSheet, 1, "L");
                    SetRowBorderStyle(workSheet, 2, "L");
                    SetRowBorderStyle(workSheet, 3, "L");


                    SetRowBorderStyle(workSheet, counter + 2, "B");
                    SetRowBorderStyle(workSheet, counter + 3, "B");
                    SetRowBorderStyle(workSheet, counter + 4, "B");
                    SetRowBorderStyle(workSheet, counter + 5, "B");
                    //SetRowBorderStyle(workSheet, counter + 6, "B");

                    workSheet.Cell($"J{counter + 2}").Value = "IVC CONTRATISTAS GENERALES";
                    workSheet.Range($"J{counter + 2}:L{counter + 2}").Merge();
                    workSheet.Cell($"J{counter + 2}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    workSheet.Cell($"J{counter + 3}").Value = "Destino";
                    workSheet.Cell($"J{counter + 3}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    workSheet.Cell($"K{counter + 3}").Value = "Fase";
                    workSheet.Cell($"K{counter + 3}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    workSheet.Cell($"L{counter + 3}").Value = "Importe sin IGV";
                    workSheet.Cell($"L{counter + 3}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    workSheet.Range($"J{counter + 2}:L{counter + 2}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    workSheet.Range($"J{counter + 2}:L{counter + 2}").Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                    workSheet.Range($"J{counter + 3}:L{counter + 3}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    workSheet.Range($"J{counter + 3}:L{counter + 3}").Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                    var c = counter + 4;
                    foreach (var item in item3)
                    {

                        workSheet.Range($"J{c}:L{c}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        workSheet.Range($"J{c}:L{c}").Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                        workSheet.Cell($"J{c}").Value = item.SgCode;
                        //workSheet.Cell($"K{c}").Style.NumberFormat.Format = "@";
                        workSheet.Cell($"K{c}").Value = item.MpCode;




                        var temp = item2.Select(x => x.UnitPrice).FirstOrDefault();
                        var tempstr = item.Acumulated * temp;
                        workSheet.Cell($"L{c}").Value = String.Format(new CultureInfo("es-PE"), "{0:C}", tempstr);
                        c++;
                    }

                    workSheet.Cell($"A{counter + 6}").Value = "TARIFA DE ALQUILER POR HORA MAQUINA";
                    workSheet.Cell($"A{counter + 6}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    workSheet.Cell($"B{counter + 6}").Value = "SUB TOTAL";
                    workSheet.Cell($"B{counter + 6}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    workSheet.Cell($"C{counter + 6}").Value = "IGV 18%";
                    workSheet.Cell($"C{counter + 6}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    workSheet.Cell($"D{counter + 6}").Value = "TOTAL";
                    workSheet.Cell($"D{counter + 6}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    SetRowBorderStyle(workSheet, counter + 6, "D");
                    SetRowBorderStyle(workSheet, counter + 7, "D");


                    workSheet.Cell($"A{counter + 7}").Value = item2.Select(x => x.UnitPriceFormatted);
                    workSheet.Cell($"B{counter + 7}").Value = item2.Select(x => x.AmmountFormatted);

                    workSheet.Cell($"C{counter + 7}").Value = item2.Select(x => x.IgvFormatted);
                    workSheet.Cell($"D{counter + 7}").Value = item2.Select(x => x.TotalAmmountFormatted);

                    workSheet.Columns().AdjustToContents();
                    workSheet.Column(2).Width = 25;
                    workSheet.Column(3).Width = 30;
                    workSheet.Column(4).Width = 15;
                    //workSheet.Column(5).Width = 60;
                    workSheet.Column(6).Width = 13;
                    workSheet.Column(7).Width = 13;

                    workSheet.Column(8).Width = 16;
                    workSheet.Column(9).Width = 16;
                    workSheet.Column(10).Width = 25;
                    workSheet.Column(11).Width = 25;
                    workSheet.Column(12).Width = 25;

                    //workSheet.Row(counter + 6).Style.NumberFormat.Format = "_(* #,##0.00_);_(* (#,##0.00);_(* \" - \"??_);_(@_)";
                    workSheet.Column(6).Style.NumberFormat.Format = "_(* #,##0.00_);_(* (#,##0.00);_(* \" - \"??_);_(@_)";
                    workSheet.Column(7).Style.NumberFormat.Format = "_(* #,##0.00_);_(* (#,##0.00);_(* \" - \"??_);_(@_)";
                    workSheet.Column(8).Style.NumberFormat.Format = "_(* #,##0.00_);_(* (#,##0.00);_(* \" - \"??_);_(@_)";
                    workSheet.Column(9).Style.NumberFormat.Format = "_(* #,##0.00_);_(* (#,##0.00);_(* \" - \"??_);_(@_)";
                    countequips++;
                    SetRowBorderStyle(workSheet, 14, "L");
                    SetRowBorderStyle(workSheet, 15, "L");
                }
                using (MemoryStream stream = new MemoryStream())
                {
                    var frs = data2.FirstOrDefault();
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Val." +frs.TradeName+ "Sem "+ frs.Week+".xlsx");
                }

            }

        }

        [HttpGet("detalle-folding-excel-mes")]
        public async Task<IActionResult> ExcelReportMonth(int month, int year, Guid providerId,int? wk = null)
        {
            
            
            SqlParameter param1 = new SqlParameter("@Month", month);
            SqlParameter param2 = new SqlParameter("@Year", year);
            SqlParameter param3 = new SqlParameter("@Id", providerId);
            SqlParameter param4 = new SqlParameter("@WeekofYear", System.Data.SqlDbType.Int);
            param4.Value = (object)wk ?? DBNull.Value;
            var data = await _context.Set<UspEquipmentMachDetailFolding>()
                .FromSqlRaw("execute EquipmentMachinery_uspMachPartFoldingsExcelMonth @Month, @Year, @Id"
                , param1
                , param2
                , param3)
                .AsNoTracking()
                .IgnoreQueryFilters()
                .ToListAsync();

            var FathersId = data.Select(x => x.FatherId).Distinct().ToList();


            var data2 = await _context.Set<UspEquipmentMachVal>().FromSqlRaw("execute EquipmentMachinery_uspMachValTable @WeekofYear, @Year, @Id, @Month"
                , param4
                , param2
                , param3
                , param1)
                .AsNoTracking()
         .IgnoreQueryFilters()
         .ToListAsync();


            var data3 = await _context.Set<UspMachValCustom>().FromSqlRaw("execute EquipmentMachinery_uspMachValCustomMonth @Month, @Year, @Id"
    , param1
    , param2
    , param3)
    .AsNoTracking()
.IgnoreQueryFilters()
.ToListAsync();
            var pid = GetProjectId();
            var project = _context.Projects.Where(x=>x.Id == pid).FirstOrDefault();


            using (XLWorkbook wb = new XLWorkbook())
            {
                int countequips = 1;


                var ws = wb.Worksheets.Add("Resumen");
                var count = 3;

                ws.Cell($"A1").Value = "Listado de Valorización de " + data2.Select(x => x.TradeName).FirstOrDefault() + " en el Mes " + data2.Select(x => x.Month).FirstOrDefault() + " del año " + data2.Select(x => x.Year).FirstOrDefault(); ;
                ws.Range($"A1:E1").Merge();



                ws.Cell($"A{count}").Value = "Proveedor";
                ws.Cell($"B{count}").Value = "Tipo de Equipo";
                ws.Cell($"C{count}").Value = "Nombre del Equipo";
                ws.Cell($"D{count}").Value = "Asignado";
                ws.Cell($"E{count}").Value = "Cuadrilla";
                ws.Cell($"F{count}").Value = "Operador";
                ws.Cell($"G{count}").Value = "Horometro Acumulado";
                ws.Cell($"H{count}").Value = "Precio Unitario (hm)";
                ws.Cell($"I{count}").Value = "Monto";
                ws.Cell($"J{count}").Value = "IGV";
                ws.Cell($"K{count}").Value = "Total";
                SetRowBorderStyle(ws, count, "K");
                count = 4;
                var totalammount = 0.00;
                var igv = 0.00;
                var ammount = 0.00;
                var days = 0.00;
                foreach (var first in data2)
                {
                    ws.Cell($"A{count}").Value = first.TradeName;
                    ws.Cell($"B{count}").Value = first.Description;
                    ws.Cell($"C{count}").Value = first.Brand + "-" + first.Model + "-" + first.Plate;
                    ws.Cell($"D{count}").Value = first.UserName;
                    ws.Cell($"E{count}").Value = first.SewerCode;
                    ws.Cell($"F{count}").Value = first.ActualName;
                    ws.Cell($"G{count}").Value = first.AcumulatedHorometer;
                    ws.Cell($"H{count}").Value = first.UnitPriceFormatted;
                    ws.Cell($"I{count}").Value = first.AmmountFormatted;
                    ws.Cell($"J{count}").Value = first.IgvFormatted;
                    ws.Cell($"K{count}").Value = first.TotalAmmountFormatted;
                    totalammount += first.TotalAmmount;
                    ammount += first.Ammount;
                    igv += first.Igv;
                    days += first.CountIds;
                    count++;
                    SetRowBorderStyle(ws, count - 1, "K");

                }

                ws.Cell($"G{count}").Value = days;
                ws.Cell($"I{count}").Value = String.Format(new CultureInfo("es-PE"), "{0:C}", ammount);
                ws.Cell($"J{count}").Value = String.Format(new CultureInfo("es-PE"), "{0:C}", igv);
                ws.Cell($"K{count}").Value = String.Format(new CultureInfo("es-PE"), "{0:C}", totalammount);
                ws.Columns().AdjustToContents();


                ws.Range($"G{count}:G{count}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range($"G{count}:G{count}").Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                ws.Range($"J{count}:J{count}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range($"J{count}:J{count}").Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                ws.Range($"K{count}:K{count}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range($"K{count}:K{count}").Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                ws.Range($"I{count}:I{count}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range($"I{count}:I{count}").Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                foreach (var fid in FathersId)
                {

   

                    var equipname = data2.Where(X => X.FatherId == fid).FirstOrDefault();
                    var datafather = data.Where(x => x.FatherId == fid).ToList();
                    var data2father = data2.Where(x => x.FatherId == fid).ToList();
                    var data3father = data3.Where(x => x.FatherId == fid).ToList();
                    var workSheet = wb.Worksheets.Add(equipname.Brand + "-" + equipname.Model + "-" + equipname.Plate);

                    var enlace = project.LogoUrl.ToString();

                    WebClient client = new WebClient();
                    Stream img = client.OpenRead(enlace);
                    Bitmap bitmap; bitmap = new Bitmap(img);

                    Image image = (Image)bitmap;

                    workSheet.Range($"A1:A3").Merge();

                    var aux = workSheet.AddPicture(bitmap);
                    aux.MoveTo(10, 10);
                    aux.Height = 45;
                    aux.Width = 250;

                    var counter = 14;
                    var totgallon = 0.00;
                    SetProjectHeaderStyle(workSheet, counter, "L");
                    SetProjectHeaderStyle(workSheet, counter + 1, "L");
                    CreateHeaders(workSheet, counter);
                    SetColumnsHeaderStyle(workSheet, counter, "L");
                    var item2 = data2father;
                    var item3 = data3father;
                    var itemtemp = datafather;
                    counter = 16;
                    workSheet.Range($"A1:A3").Merge();
                    workSheet.Cell($"B1").Value = "GESTIÓN DE CONTROL DE EQUIPOS";
                    workSheet.Cell($"B1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    workSheet.Range($"B1:J1").Merge();
                    workSheet.Cell($"B2").Value = "CONTROL DE HORAS DE EQUIPO";
                    workSheet.Range($"B2:J3").Merge();
                    workSheet.Cell($"B2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    workSheet.Cell($"K1").Value = "Código:";
                    workSheet.Cell($"L1").Value = "JIC/GEQ-For-11";
                    workSheet.Cell($"K2").Value = "Versión:";
                    workSheet.Cell($"L2").Value = "02";
                    workSheet.Cell($"K3").Value = "Fecha:";
                    workSheet.Cell($"L3").Value = DateTime.UtcNow.ToShortDateString();
                    workSheet.Range($"K1:L3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    workSheet.Cell($"A7").Value = "OBRA:";
                    workSheet.Cell($"A8").Value = "PROVEEDOR:";
                    workSheet.Cell($"A9").Value = "EQUIPO:";
                    workSheet.Cell($"A10").Value = "CODIGO:";
                    workSheet.Cell($"A11").Value = "SERIE:";
                    workSheet.Cell($"A12").Value = "AÑO DE FABRICACIÓN:";

                    workSheet.Cell($"B7").Value = project.Name;
                    workSheet.Cell($"B8").Value = item2.Select(x=>x.TradeName);
                    workSheet.Cell($"B9").Value = item2.Select(x => x.Description);
                    workSheet.Cell($"B10").Value = item2.Select(x => x.Model);
                    workSheet.Cell($"B11").Value = item2.Select(x=>x.SerieNumber);
                    workSheet.Cell($"B12").Value = item2.Select(x => x.EquipmentYear);
                    //var aux = workSheet.AddPicture(bitmap);
                    //aux.MoveTo(16, 22);
                    //aux.Height = 85;
                    //aux.Width = 190;

                    foreach (var item in datafather)
                    {
                        workSheet.Cell($"A{counter}").Value = item.PartDate;
                        workSheet.Cell($"B{counter}").Value = item.PartNumber;
                        workSheet.Cell($"C{counter}").Value = item.ActualName;
                        workSheet.Cell($"D{counter}").Value = item.Description;
                        workSheet.Cell($"E{counter}").Value = item.Specific;

                        workSheet.Cell($"F{counter}").Value = item.InitHorometer;
                        workSheet.Cell($"G{counter}").Value = item.EndHorometer;
                        workSheet.Cell($"H{counter}").Value = item.Dif;
                        workSheet.Cell($"I{counter}").Value = item.Gallon.HasValue ? item.Gallon.Value : 0;

                        workSheet.Cell($"J{counter}").Value = item.UserName;
                        workSheet.Cell($"K{counter}").Value = item.Code;
                        workSheet.Cell($"L{counter}").Value = item.SewerCode;
                        
                        
                        SetRowBorderStyle(workSheet, counter, "L");
                        totgallon += item.Gallon.HasValue ? item.Gallon.Value : 0.00;
                        counter++;
                    }

                   
                    workSheet.Cell($"H{counter}").Value = item2.Select(x => x.AcumulatedHorometer);
                    workSheet.Cell($"I{counter}").Value = totgallon;
                    SetTotalCellStyle(workSheet, counter);
                    workSheet.Cell($"A{counter + 2}").Value = "HOROMETRO INICIAL";
                    workSheet.Cell($"A{counter + 3}").Value = "HOROMETRO FINAL";
                    //workSheet.Cell($"A{counter + 4}").Value = "HORAS TOTALES";
                    workSheet.Cell($"A{counter + 4}").Value = "COMBUSTIBLE (GLN)";
                    workSheet.Cell($"A{counter + 5}").Value = "HORAS TRABAJADAS";
                    workSheet.Cell($"B{counter + 2}").Value = item2.Select(x => x.InitHorometer);
                    workSheet.Cell($"B{counter + 3}").Value = item2.Select(x => x.EndHorometer);
                    //workSheet.Cell($"B{counter + 4}").Value = item2.Select(x => x.AcumulatedHorometer);
                    workSheet.Cell($"B{counter + 4}").Value = totgallon;
                    workSheet.Cell($"B{counter + 5}").Value = item2.Select(x => x.CountIds);

                    SetRowBorderStyle(workSheet, 1, "L");
                    SetRowBorderStyle(workSheet, 2, "L");
                    SetRowBorderStyle(workSheet, 3, "L");


                    SetRowBorderStyle(workSheet, counter + 2, "B");
                    SetRowBorderStyle(workSheet, counter + 3, "B");
                    SetRowBorderStyle(workSheet, counter + 4, "B");
                    SetRowBorderStyle(workSheet, counter + 5, "B");
                    //SetRowBorderStyle(workSheet, counter + 6, "B");

                    workSheet.Cell($"J{counter + 2}").Value = "IVC CONTRATISTAS GENERALES";
                    workSheet.Range($"J{counter + 2}:L{counter + 2}").Merge();
                    workSheet.Cell($"J{counter + 2}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    workSheet.Cell($"J{counter + 3}").Value = "Destino";
                    workSheet.Cell($"J{counter + 3}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    workSheet.Cell($"K{counter + 3}").Value = "Fase";
                    workSheet.Cell($"K{counter + 3}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    workSheet.Cell($"L{counter + 3}").Value = "Importe sin IGV";
                    workSheet.Cell($"L{counter + 3}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    
                    workSheet.Range($"J{counter + 2}:L{counter + 2}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    workSheet.Range($"J{counter + 2}:L{counter + 2}").Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                    workSheet.Range($"J{counter + 3}:L{counter + 3}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    workSheet.Range($"J{counter + 3}:L{counter + 3}").Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                    var c = counter + 4;
                    foreach (var item in item3)
                    {

                        workSheet.Range($"J{c}:L{c}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        workSheet.Range($"J{c}:L{c}").Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                        workSheet.Cell($"J{c}").Value = item.SgCode;
                        //workSheet.Cell($"K{c}").Style.NumberFormat.Format = "@";
                        workSheet.Cell($"K{c}").Value = item.MpCode;
                        



                        var temp = item2.Select(x => x.UnitPrice).FirstOrDefault();
                        var tempstr = item.Acumulated * temp;
                        workSheet.Cell($"L{c}").Value = String.Format(new CultureInfo("es-PE"), "{0:C}", tempstr);
                        c++;
                    }

                    workSheet.Cell($"A{counter + 6}").Value = "TARIFA DE ALQUILER POR HORA MAQUINA";
                    workSheet.Cell($"A{counter + 6}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    workSheet.Cell($"B{counter + 6}").Value = "SUB TOTAL";
                    workSheet.Cell($"B{counter + 6}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    workSheet.Cell($"C{counter + 6}").Value = "IGV 18%";
                    workSheet.Cell($"C{counter + 6}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    workSheet.Cell($"D{counter + 6}").Value = "TOTAL";
                    workSheet.Cell($"D{counter + 6}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    SetRowBorderStyle(workSheet, counter + 6, "D");
                    SetRowBorderStyle(workSheet, counter + 7, "D");


                    workSheet.Cell($"A{counter + 7}").Value = item2.Select(x => x.UnitPriceFormatted);
                    workSheet.Cell($"B{counter + 7}").Value = item2.Select(x => x.AmmountFormatted);

                    workSheet.Cell($"C{counter + 7}").Value = item2.Select(x => x.IgvFormatted);
                    workSheet.Cell($"D{counter + 7}").Value = item2.Select(x => x.TotalAmmountFormatted);

                    workSheet.Columns().AdjustToContents();
                    workSheet.Column(2).Width = 25;
                    workSheet.Column(3).Width = 30;
                    workSheet.Column(4).Width = 15;
                    //workSheet.Column(5).Width = 60;
                    workSheet.Column(6).Width = 13;
                    workSheet.Column(7).Width = 13;

                    workSheet.Column(8).Width = 16;
                    workSheet.Column(9).Width = 16;
                    workSheet.Column(10).Width = 25;
                    workSheet.Column(11).Width = 25;
                    workSheet.Column(12).Width = 25;

                    //workSheet.Row(counter + 6).Style.NumberFormat.Format = "_(* #,##0.00_);_(* (#,##0.00);_(* \" - \"??_);_(@_)";
                    workSheet.Column(6).Style.NumberFormat.Format = "_(* #,##0.00_);_(* (#,##0.00);_(* \" - \"??_);_(@_)";
                    workSheet.Column(7).Style.NumberFormat.Format = "_(* #,##0.00_);_(* (#,##0.00);_(* \" - \"??_);_(@_)";
                    workSheet.Column(8).Style.NumberFormat.Format = "_(* #,##0.00_);_(* (#,##0.00);_(* \" - \"??_);_(@_)";
                    workSheet.Column(9).Style.NumberFormat.Format = "_(* #,##0.00_);_(* (#,##0.00);_(* \" - \"??_);_(@_)";
                    countequips++;
                    SetRowBorderStyle(workSheet, 14, "L");
                    SetRowBorderStyle(workSheet, 15, "L");
                }
                using (MemoryStream stream = new MemoryStream())
                {
                    var frs = data2.FirstOrDefault();
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Val." + frs.TradeName + "Mes " + frs.Month + ".xlsx");
                }

            }

        }

        private void CreateHeaders(IXLWorksheet ws, int counter)
        {
            ws.Cell($"A{counter}").Value = "FECHA";
            ws.Range($"A{counter}:A{counter + 1}").Merge();

            ws.Cell($"B{counter}").Value = "PARTE DIARIO";
            ws.Range($"B{counter}:B{counter + 1}").Merge();

            ws.Cell($"C{counter}").Value = "OPERADOR";
            ws.Range($"C{counter}:C{counter + 1}").Merge();

            ws.Cell($"D{counter}").Value = "ACTIVIDAD";
            ws.Range($"D{counter}:D{counter + 1}").Merge();

            ws.Cell($"E{counter}").Value = "DESCRIPCION";
            ws.Range($"E{counter}:E{counter + 1}").Merge();

            ws.Cell($"F{counter}").Value = "HOROMETRO";
            ws.Range($"F{counter}:H{counter}").Merge();
            ws.Cell($"F{counter + 1}").Value = "INICIAL";
            ws.Cell($"G{counter + 1}").Value = "FINAL";
            ws.Cell($"H{counter + 1}").Value = "ACUMULADO";

            ws.Cell($"I{counter}").Value = "COMBUSTIBLE";
            ws.Range($"I{counter}:I{counter + 1}").Merge();

            ws.Cell($"J{counter}").Value = "UNIDAD A CARGO DE";
            ws.Range($"J{counter}:J{counter + 1}").Merge();

            ws.Cell($"K{counter}").Value = "FASE";
            ws.Column(11).Style.NumberFormat.Format = "@";
            ws.Column(12).Style.NumberFormat.Format = "@";
            ws.Range($"K{counter}:K{counter + 1}").Merge();

            ws.Cell($"L{counter}").Value = "CUADRILLA";
            ws.Range($"L{counter}:L{counter + 1}").Merge();






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
            ws.Cell($"H{rowCount}").Style.Fill.SetBackgroundColor(XLColor.FromArgb(184, 204, 228));
            ws.Cell($"H{rowCount}").Style.Font.Bold = true;
            ws.Cell($"H{rowCount}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Cell($"H{rowCount}").Style.Border.InsideBorder = XLBorderStyleValues.Thin;

            ws.Cell($"I{rowCount}").Style.Fill.SetBackgroundColor(XLColor.FromArgb(184, 204, 228));
            ws.Cell($"I{rowCount}").Style.Font.Bold = true;
            ws.Cell($"I{rowCount}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Cell($"I{rowCount}").Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        }

        [HttpGet("monto")]
        public async Task<IActionResult> GetTotalInstalled(int year, Guid providerId, int month, int? week = null)
        {
            SqlParameter param1 = new SqlParameter("@WeekofYear", System.Data.SqlDbType.Int);
            param1.Value = (object)week ?? DBNull.Value;
            SqlParameter param2 = new SqlParameter("@Year", year);
            SqlParameter param3 = new SqlParameter("@Id", providerId);
            SqlParameter param4 = new SqlParameter("@Month", month);

            var data = await _context.Set<UspMachValTable>().FromSqlRaw("execute EquipmentMachinery_uspMachValTable @WeekofYear, @Year, @Id, @Month"
                , param1
                , param2
                , param3
                , param4)

                .IgnoreQueryFilters()

         .ToListAsync()
         ;


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
        public async Task<IActionResult> GetTotalInstalled2(int year, Guid providerId, int month, int? week = null)
        {
            SqlParameter param1 = new SqlParameter("@WeekofYear", System.Data.SqlDbType.Int);
            param1.Value = (object)week ?? DBNull.Value;
            SqlParameter param2 = new SqlParameter("@Year", year);
            SqlParameter param3 = new SqlParameter("@Id", providerId);
            SqlParameter param4 = new SqlParameter("@Month", month);

            var data = await _context.Set<UspMachValTable>().FromSqlRaw("execute EquipmentMachinery_uspMachValTable @WeekofYear, @Year, @Id, @Month"
                , param1
                , param2
                , param3
                , param4)

                .IgnoreQueryFilters()

         .ToListAsync()
         ;


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
        public async Task<IActionResult> GetTotalInstalled3(int year, Guid providerId, int month, int? week = null)
        {
            SqlParameter param1 = new SqlParameter("@WeekofYear", System.Data.SqlDbType.Int);
            param1.Value = (object)week ?? DBNull.Value;
            SqlParameter param2 = new SqlParameter("@Year", year);
            SqlParameter param3 = new SqlParameter("@Id", providerId);
            SqlParameter param4 = new SqlParameter("@Month", month);

            var data = await _context.Set<UspMachValTable>().FromSqlRaw("execute EquipmentMachinery_uspMachValTable @WeekofYear, @Year, @Id, @Month"
                , param1
                , param2
                , param3
                , param4)

                .IgnoreQueryFilters()

         .ToListAsync()
         ;


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
        }
        
    
    }
}
