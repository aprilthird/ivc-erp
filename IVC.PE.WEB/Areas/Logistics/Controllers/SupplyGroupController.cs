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
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyGroupViewModels;
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
    [Route("logistica/grupos")]
    public class SupplyGroupController : BaseController
    {
        public SupplyGroupController(IvcDbContext context,
            ILogger<SupplyGroupController> logger)
            : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? supplyFamilyId = null)
        {
            var query = _context.SupplyGroups.Include(x => x.SupplyFamily).Where(x=>x.SupplyFamilyId!=null);

            if (supplyFamilyId.HasValue)
                query = query.Where(x => x.SupplyFamilyId == supplyFamilyId);

            var data = await query
                .Select(x => new SupplyGroupViewModel
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    SupplyFamilyId = x.SupplyFamilyId,
                    SupplyFamily = new SupplyFamilyViewModel
                    {
                        Name = $"{x.SupplyFamily.Code} - {x.SupplyFamily.Name}"
                    }
                }).ToListAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var supplyGroup = await _context.SupplyGroups
                .Where(x => x.Id == id)
                .Select(x => new SupplyGroupViewModel
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    SupplyFamilyId = x.SupplyFamilyId
                }).FirstOrDefaultAsync();
            return Ok(supplyGroup);
        }

        [Authorize(Roles = ConstantHelpers.Permission.Logistics.FULL_ACCESS)]
        [HttpPost("crear")]
        public async Task<IActionResult> Create(SupplyGroupViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var supplyGroup = new SupplyGroup
            {
                Code = model.Code,
                Name = model.Name,
                SupplyFamilyId = model.SupplyFamilyId
            };
            await _context.SupplyGroups.AddAsync(supplyGroup);
            await _context.SaveChangesAsync();
            return Ok();
        }
        
        [Authorize(Roles = ConstantHelpers.Permission.Logistics.FULL_ACCESS)]
        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, SupplyGroupViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var supplyGroup = await _context.SupplyGroups.FindAsync(id);
            supplyGroup.Code = model.Code;
            supplyGroup.Name = model.Name;
            supplyGroup.SupplyFamilyId = model.SupplyFamilyId;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.Logistics.FULL_ACCESS)]
        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var supplyGroup = await _context.SupplyGroups.FirstOrDefaultAsync(x => x.Id == id);
            if (supplyGroup == null)
                return BadRequest($"Grupo con Id '{id}' no encontrado.");
            _context.SupplyGroups.Remove(supplyGroup);
            await _context.SaveChangesAsync();
            return Ok();
        }
        
        [Authorize(Roles = ConstantHelpers.Permission.Logistics.FULL_ACCESS)]
        [HttpPost("importar")]
        public async Task<IActionResult> Import(IFormFile file, SupplyGroupViewModel model)
        {
            var grupos = await _context.SupplyGroups.Where(x => x.SupplyFamilyId == model.SupplyFamilyId).ToListAsync();

            using (var mem = new MemoryStream())
            {
                await file.CopyToAsync(mem);
                using (var workBook = new XLWorkbook(mem))
                {
                    var workSheet = workBook.Worksheets.FirstOrDefault(x => x.Name.ToUpper() == "TIPO DE INSUMO");
                    var counter = 3;
                    var supplyGroups = new List<SupplyGroup>();

                    while (!workSheet.Cell($"B{counter}").IsEmpty())
                    {
                        var code = workSheet.Cell($"B{counter}").GetString();
                        var supplyGroup = grupos.FirstOrDefault(x => x.Code == code);
                        if (supplyGroup == null)
                        {
                            supplyGroup = new SupplyGroup();
                            supplyGroup.Code = code;
                            supplyGroup.Name = workSheet.Cell($"C{counter}").GetString();
                            supplyGroup.SupplyFamilyId = model.SupplyFamilyId;
                            supplyGroups.Add(supplyGroup);
                        }
                        else
                        {
                            supplyGroup.Code = workSheet.Cell($"D{counter}").GetString();
                            supplyGroup.Name = workSheet.Cell($"C{counter}").GetString();
                        }

                        ++counter;
                    }
                    await _context.SupplyGroups.AddRangeAsync(supplyGroups);
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

                workSheet.Cell($"B2").Value = "Código";
                workSheet.Cell($"C2").Value = "Nombre";
                workSheet.Cell($"D2").Value = "Reemplazo Código (Opcional)";
                workSheet.Cell($"D2").Style.Alignment.WrapText = true;

                workSheet.Column(1).Width = 3;
                workSheet.Column(2).Width = 15;
                workSheet.Column(3).Width = 65;
                workSheet.Column(4).Width = 17;

                workSheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                workSheet.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                workSheet.Range("B2:D2").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                workSheet.Range("B2:D2").Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
                /*
                var families = _context.SupplyGroups.AsNoTracking().ToList();

                DataTable dtFamilies = new DataTable();
                dtFamilies.TableName = "Grupo de Insumos";
                dtFamilies.Columns.Add("Código", typeof(string));
                dtFamilies.Columns.Add("Nombre", typeof(string));
                foreach (var item in families)
                    dtFamilies.Rows.Add(item.Code, item.Name);
                dtFamilies.AcceptChanges();

                var workSheetFamily = wb.Worksheets.Add(dtFamilies);

                workSheetFamily.Column(2).Width = 70;
                */

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "InsumoGrupos.xlsx");
                }
            }
        }

        [HttpGet("exportar")]
        public async Task<IActionResult> Export()
        {
            var dt = new DataTable("GRUPOS DE INSUMOS");
            dt.Columns.Add("Familia Código", typeof(string));
            dt.Columns.Add("Familia Nombre", typeof(string));
            dt.Columns.Add("Código", typeof(string));
            dt.Columns.Add("Nombre", typeof(string));


            var data = await _context.SupplyGroups
                .Include(x => x.SupplyFamily)
                .AsNoTracking()
                .OrderBy(x => x.SupplyFamilyId)
                .ToListAsync();

            data.ForEach(item =>
            {
                dt.Rows.Add(item.SupplyFamily.Code, item.SupplyFamily.Name, item.Code, item.Name);
            });

            var fileName = "Grupos de Insumos.xlsx";
            using (var wb = new XLWorkbook())
            {
                var workSheet = wb.Worksheets.Add(dt);

                workSheet.Column(1).Width = 10;
                workSheet.Column(2).Width = 40;
                workSheet.Column(3).Width = 10;
                workSheet.Column(4).Width = 80;

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