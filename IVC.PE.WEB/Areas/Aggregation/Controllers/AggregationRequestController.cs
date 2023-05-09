using ClosedXML.Excel;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Aggregation.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Aggregation.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.AGGREGATION)]
    [Route("agregados/requerimiento")]
    public class AggregationRequestController : BaseController
    {
        public AggregationRequestController(IvcDbContext context,
            ILogger<AggregationRequestController> logger)
            : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.AggregationRequests
                .ToListAsync();

            return Ok(data);
        }

        [HttpGet("formato-carga")]
        public FileResult DownloadLoadExcelModel()
        {
            string fileName = "agregados_requerimientos.xlsx";
            using (XLWorkbook wb = new XLWorkbook())
            {
                var workSheet = wb.Worksheets.Add("Requerimientos");
                workSheet.Cell($"A1").Value = "Nº Req.";
                workSheet.Cell("A1").Style.Font.SetFontColor(XLColor.White);
                workSheet.Cell("A1").Style.Fill.SetBackgroundColor(XLColor.Navy);
                workSheet.Cell($"A2").Value = "41";
                workSheet.Cell($"A3").Value = "40";
                workSheet.Cell($"A4").Value = "39";

                workSheet.Cell($"B1").Value = "Centro Costo";
                workSheet.Cell("B1").Style.Font.SetFontColor(XLColor.White);
                workSheet.Cell("B1").Style.Fill.SetBackgroundColor(XLColor.Navy);
                workSheet.Cell($"B2").Value = "F0W-125";
                workSheet.Cell($"B3").Value = "F0W-125";
                workSheet.Cell($"B4").Value = "F0W-125";

                workSheet.Cell($"C1").Value = "Formula";
                workSheet.Cell("C1").Style.Font.SetFontColor(XLColor.White);
                workSheet.Cell("C1").Style.Fill.SetBackgroundColor(XLColor.Navy);
                workSheet.Cell($"C2").Value = "Colector de Descarga";
                workSheet.Cell($"C3").Value = "Redes y Conexiones de Alcantarillado";
                workSheet.Cell($"C4").Value = "Colector de Descarga";

                workSheet.Cell($"D1").Value = "Fase";
                workSheet.Cell("D1").Style.Font.SetFontColor(XLColor.White);
                workSheet.Cell("D1").Style.Fill.SetBackgroundColor(XLColor.Navy);
                workSheet.Cell($"D2").Value = "INST. DE TUB - SIN ZANJA";
                workSheet.Cell($"D3").Value = "OBRAS PROVISIONALES Y PRELIMINARES";
                workSheet.Cell($"D4").Value = "INST. DE TUB - SIN ZANJA";

                workSheet.Cell($"E1").Value = "Cuadrilla";
                workSheet.Cell("E1").Style.Font.SetFontColor(XLColor.White);
                workSheet.Cell("E1").Style.Fill.SetBackgroundColor(XLColor.Navy);
                workSheet.Cell($"E2").Value = "TUNEL LINNER";
                workSheet.Cell($"E3").Value = "F5/6-C05";
                workSheet.Cell($"E4").Value = "TUNEL LINNER";

                workSheet.Cell($"F1").Value = "Material";
                workSheet.Cell("F1").Style.Font.SetFontColor(XLColor.White);
                workSheet.Cell("F1").Style.Fill.SetBackgroundColor(XLColor.Navy);
                workSheet.Cell($"F2").Value = "Relleno Estructural";
                workSheet.Cell($"F3").Value = "Eliminación o Material Excedente";
                workSheet.Cell($"F4").Value = "Relleno Estructural";

                workSheet.Cell($"G1").Value = "Vol(m3)";
                workSheet.Cell("G1").Style.Font.SetFontColor(XLColor.White);
                workSheet.Cell("G1").Style.Fill.SetBackgroundColor(XLColor.Navy);
                workSheet.Cell($"G2").Value = "40";
                workSheet.Cell($"G3").Value = "50";
                workSheet.Cell($"G4").Value = "40";

                workSheet.Cell($"H1").Value = "Entrega";
                workSheet.Cell("H1").Style.Font.SetFontColor(XLColor.White);
                workSheet.Cell("H1").Style.Fill.SetBackgroundColor(XLColor.Navy);
                workSheet.Cell($"H2").Value = "07/07/2021";
                workSheet.Cell($"H3").Value = "02/07/2021";
                workSheet.Cell($"H4").Value = "03/07/2021";

                workSheet.Cell($"I1").Value = "Turno";
                workSheet.Cell("I1").Style.Font.SetFontColor(XLColor.White);
                workSheet.Cell("I1").Style.Fill.SetBackgroundColor(XLColor.Navy);
                workSheet.Cell($"I2").Value = "1) 7:30-10:30";
                workSheet.Cell($"I3").Value = "1) 7:30-10:30";
                workSheet.Cell($"I4").Value = "1) 7:30-10:30";

                workSheet.Cell($"J1").Value = "F. Registro";
                workSheet.Cell("J1").Style.Font.SetFontColor(XLColor.White);
                workSheet.Cell("J1").Style.Fill.SetBackgroundColor(XLColor.Navy);
                workSheet.Cell($"J2").Value = "07/07/2021";
                workSheet.Cell($"J3").Value = "02/07/2021";
                workSheet.Cell($"J4").Value = "02/07/2021";


                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        [HttpPost("importar")]
        public async Task<IActionResult> ImportRequest(IFormFile file)
        {
            using (var mem = new MemoryStream())
            {
                await file.CopyToAsync(mem);
                using (var workBook = new XLWorkbook(mem))
                {
                    var providerTypesWs = workBook.Worksheets.FirstOrDefault(x => x.Name.ToUpper().Equals("REQUERIMIENTOS"));                    

                    var requestDb = await _context.AggregationRequests
                        .Where(x => x.ProjectId == GetProjectId())
                        .ToListAsync();

                    //await _context.SaveChangesAsync();
                }
                mem.Close();
            }
            return Ok();
        }
    }
}
