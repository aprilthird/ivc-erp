using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Logistics;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyFamilyViewModels;
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
    [Route("logistica/familias")]
    public class SupplyFamilyController : BaseController
    {
        public SupplyFamilyController(IvcDbContext context,
            ILogger<SupplyFamilyController> logger)
            : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.SupplyFamilies
                .Select(x => new SupplyFamilyViewModel
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name
                }).ToListAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var supplyFamily = await _context.SupplyFamilies
                .Where(x => x.Id == id)
                .Select(x => new SupplyFamilyViewModel
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name
                }).FirstOrDefaultAsync();
            return Ok(supplyFamily);
        }

        [Authorize(Roles = ConstantHelpers.Permission.Logistics.FULL_ACCESS)]
        [HttpPost("crear")]
        public async Task<IActionResult> Create(SupplyFamilyViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var supplyFamily = new SupplyFamily
            {
                Code = model.Code,
                Name = model.Name
            };
            await _context.SupplyFamilies.AddAsync(supplyFamily);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.Logistics.FULL_ACCESS)]
        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, SupplyFamilyViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var supplyFamily = await _context.SupplyFamilies.FindAsync(id);
            supplyFamily.Code = model.Code;
            supplyFamily.Name = model.Name;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.Logistics.FULL_ACCESS)]
        [HttpDelete("eliminar")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var supplyFamily = await _context.SupplyFamilies.FirstOrDefaultAsync(x => x.Id == id);
            if (supplyFamily == null)
                return BadRequest($"Familia con Id '{id}' no encontrado.");
            _context.SupplyFamilies.Remove(supplyFamily);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.Logistics.FULL_ACCESS)]
        [HttpPost("importar")]
        public async Task<IActionResult> Import(IFormFile file)
        {
            using (var mem = new MemoryStream())
            {
                await file.CopyToAsync(mem);
                using (var workBook = new XLWorkbook(mem))
                {
                    var workSheet = workBook.Worksheets.FirstOrDefault(x => x.Name.ToUpper() == "TIPO DE INSUMO");
                    var counter = 7;
                    var supplyFamilies = new List<SupplyFamily>();
                    while (!workSheet.Cell($"B{counter}").IsEmpty())
                    {
                        var supplyFamily = new SupplyFamily();
                        supplyFamily.Code = workSheet.Cell($"B{counter}").GetString();
                        supplyFamily.Name = workSheet.Cell($"C{counter}").GetString();
                        supplyFamilies.Add(supplyFamily);
                        ++counter;
                    }
                    await _context.SupplyFamilies.AddRangeAsync(supplyFamilies);
                    await _context.SaveChangesAsync();
                }
                mem.Close();
            }
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.Logistics.FULL_ACCESS)]
        [HttpGet("excel-modelo")]
        public FileResult GetExcelSample()
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                var workSheet = wb.Worksheets.Add("TIPO DE INSUMO");

                workSheet.Cell($"B5").Value = "Familias de Insumo";
                workSheet.Range("B5:C5").Merge();
                workSheet.Cell($"B6").Value = "Codigo";
                workSheet.Cell($"C6").Value = "Nombre";

                workSheet.Range("B5:C6").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Column("B").Style.NumberFormat.Format = "@";
                workSheet.Cell($"B7").Value = "01001";
                workSheet.Cell($"C7").Value = "ACTIVOS FIJOS TANGIBLES";

                workSheet.Cell($"B8").Value = "01002";
                workSheet.Cell($"C8").Value = "ACTIVOS FIJOS INTANGIBLES";

                workSheet.Columns().AdjustToContents();

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "InsumoFamilias.xlsx");
                }
            }
        }

        [HttpGet("exportar")]
        public async Task<IActionResult> Export()
        {
            var dt = new DataTable("FAMILIAS DE INSUMOS");
            dt.Columns.Add("Código", typeof(string));
            dt.Columns.Add("Nombre", typeof(string));


            var data = await _context.SupplyFamilies
                .AsNoTracking()
                .ToListAsync();

            data.ForEach(item =>
            {
                dt.Rows.Add(item.Code, item.Name);
            });

            var fileName = "Familias de Insumos.xlsx";
            using (var wb = new XLWorkbook())
            {
                var workSheet = wb.Worksheets.Add(dt);

                workSheet.Column(1).Width = 10;
                workSheet.Column(2).Width = 60;

                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Seek(0, SeekOrigin.Begin);
                    Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

    }
}