using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.UspModels.General;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IVC.PE.WEB.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.Roles.SUPERADMIN)]
    [Area(ConstantHelpers.Areas.ADMIN)]
    [Route("admin/frentes")]
    public class WorkFrontController : BaseController
    {
        public WorkFrontController(IvcDbContext context,
            ILogger<WorkFrontController> logger)
            : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? formulaId = null)
        {
            SqlParameter projectParam = new SqlParameter("@ProjectId", GetProjectId());
            SqlParameter formulaParam = new SqlParameter("@FormulaId", SqlDbType.UniqueIdentifier);
            formulaParam.Value = (object)formulaId ?? DBNull.Value;

            var data = await _context.Set<UspWorkFront>().FromSqlRaw("execute Admin_uspWorkFronts @ProjectId, @FormulaId"
                , projectParam, formulaParam)
                .IgnoreQueryFilters()
                .ToListAsync();

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var workFront = await _context.WorkFronts
                .Where(x => x.Id == id)
                .Select(x => new WorkFrontViewModel
                {
                    Id = x.Id,
                    Code = x.Code,
                    ProjectId = x.ProjectId
                }).FirstOrDefaultAsync();

            var formulaPhases = await _context.WorkFrontProjectPhases
                .Include(x => x.ProjectPhase)
                .Where(x => x.WorkFrontId == id)
                .ToListAsync();

            workFront.FormulaIds = formulaPhases.Select(x => x.ProjectPhase.ProjectFormulaId.Value).Distinct();
            workFront.ProjectPhaseIds = formulaPhases.Select(x => x.ProjectPhaseId).Distinct();

            return Ok(workFront);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(WorkFrontViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existCode = await _context.WorkFronts.FirstOrDefaultAsync(x => x.Code == model.Code);
            if (existCode != null)
                return BadRequest("Ya existe un Frente con el código ingresado.");

            var workFront = new WorkFront
            {
                Code = model.Code,
                ProjectId = GetProjectId()
            };

            if (model.ProjectPhaseIds != null)
                await _context.WorkFrontProjectPhases.AddRangeAsync(
                    model.ProjectPhaseIds.Select(x => new WorkFrontProjectPhase
                    {
                        WorkFront = workFront,
                        ProjectPhaseId = x
                    }).ToList());
            

            await _context.WorkFronts.AddAsync(workFront);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, WorkFrontViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var workFront = await _context.WorkFronts.FindAsync(id);
            workFront.Code = model.Code;

            var phasesDb = await _context.WorkFrontProjectPhases
                .Where(x => x.WorkFrontId == id)
                .ToListAsync();
            _context.WorkFrontProjectPhases.RemoveRange(phasesDb);
            if (model.ProjectPhaseIds != null)
                await _context.WorkFrontProjectPhases.AddRangeAsync(
                    model.ProjectPhaseIds.Select(x => new WorkFrontProjectPhase
                    {
                        WorkFront = workFront,
                        ProjectPhaseId = x
                    }).ToList());

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var workFront = await _context.WorkFronts.FirstOrDefaultAsync(x => x.Id == id);
            if (workFront == null)
                return BadRequest($"Frente con Id '{id}' no encontrado.");
            var phases = await _context.WorkFrontProjectPhases
                .Where(x => x.WorkFrontId == id)
                .ToListAsync();

            _context.WorkFrontProjectPhases.RemoveRange(phases);
            _context.WorkFronts.Remove(workFront);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("importar")]
        public async Task<IActionResult> Import(IFormFile file)
        {
            using (var mem = new MemoryStream())
            {
                await file.CopyToAsync(mem);
                using (var workBook = new XLWorkbook(mem))
                {
                    var workSheet = workBook.Worksheets.FirstOrDefault(x => x.Name.ToUpper() == "FRENTE");
                    var counter = 4;
                    var workFronts = new List<WorkFront>();
                    while (!workSheet.Cell($"D{counter}").IsEmpty())
                    {
                        var workFront = new WorkFront();
                        var codeStr = workSheet.Cell($"D{counter}").GetString();
                        if (await _context.WorkFronts.AnyAsync(x => x.Code == codeStr))
                        {
                            ++counter;
                            continue;
                        }
                        workFront.Code = codeStr;
                        workFronts.Add(workFront);
                        ++counter;
                    }

                    await _context.WorkFronts.AddRangeAsync(workFronts);
                    await _context.SaveChangesAsync();
                }
                mem.Close();
            }
            return Ok();
        }

        [HttpPost("importar-solo-frentes")]
        public async Task<IActionResult> ImportWorkFronts(IFormFile file)
        {
            using (var mem = new MemoryStream())
            {

                await file.CopyToAsync(mem);
                using (var workBook = new XLWorkbook(mem))
                {
                    var workSheet = workBook.Worksheets.FirstOrDefault();
                    var counter = 4;


                    while (!workSheet.Cell($"A{counter}").IsEmpty())
                    {
                        //var selectedFormula = await _context.ProjectFormulas
                        //   .FirstOrDefaultAsync(x => x.Code == workSheet.Cell($"A{counter}").GetString() && x.ProjectId == GetProjectId());
                        var wflist = new List<WorkFront>();
                        var wf = new WorkFront();
                        wf.Id = Guid.NewGuid();
                        wf.ProjectId = GetProjectId();
                        wf.Code = workSheet.Cell($"A{counter}").GetString();

                        wflist.Add(wf);
                        await _context.WorkFronts.AddRangeAsync(wflist);
                        await _context.SaveChangesAsync();
                        ++counter;
                    }
                }
                mem.Close();
            }
            return Ok();
        }

        [HttpGet("importar-formato")]
        public FileResult GetImportFormat()
        {


            string fileName = "Frentes.xlsx";
            using (XLWorkbook wb = new XLWorkbook())
            {
                var workSheet = wb.Worksheets.Add("Frentes");


                workSheet.Cell($"A1").Value = "FT-01";
                workSheet.Cell($"A2").Value = "Código";
                workSheet.Range("A2:A3").Merge(false);
                workSheet.Range("A2:A3").Style.Font.SetFontColor(XLColor.White);
                workSheet.Range("A2:A3").Style.Fill.SetBackgroundColor(XLColor.Navy);

                workSheet.Column(1).Width = 20;

                //workSheet.Columns().AdjustToContents();
                //workSheet.Rows().AdjustToContents();

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        [HttpGet("exportar")]
        public async Task<IActionResult> ExcelReport(Guid? formulaId = null)
        {

            SqlParameter projectParam = new SqlParameter("@ProjectId", GetProjectId());
            SqlParameter formulaParam = new SqlParameter("@FormulaId", SqlDbType.UniqueIdentifier);
            formulaParam.Value = (object)formulaId ?? DBNull.Value;

            var data = await _context.Set<UspWorkFront>().FromSqlRaw("execute Admin_uspWorkFronts @ProjectId, @FormulaId"
                , projectParam, formulaParam)
                .IgnoreQueryFilters()
                .ToListAsync();



            using (XLWorkbook wb = new XLWorkbook())
            {

                var ws = wb.Worksheets.Add("Fases");


                var count = 1;
                //ws.Cell($"A{count}").Value = "Proveedor";
                ws.Cell($"A{count}").Value = "Código";
                ws.Range($"A{count}:A{count + 1}").Merge();

                ws.Cell($"B{count}").Value = "Fórmulas";
                ws.Range($"B{count}:B{count + 1}").Merge();



                SetRowBorderStyle2(ws, count, "B");
                SetRowBorderStyle2(ws, count + 1, "B");
                ws.Row(count).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                ws.Row(count + 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                ws.Range($"A{count}:B{count}").Style.Fill.SetBackgroundColor(XLColor.FromArgb(211, 211, 211));
                ws.Range($"A{count + 1}:B{count + 1}").Style.Fill.SetBackgroundColor(XLColor.FromArgb(211, 211, 211));


                count = 3;
                //ws.Column(8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                //ws.Column(8).Style.NumberFormat.Format = "d-mm-yy";

                foreach (var first in data)
                {
                    ws.Cell($"A{count}").Value = first.Code;
                    ws.Cell($"B{count}").Value = first.FormulaCodes;
                   


                    count++;
                    SetRowBorderStyle2(ws, count - 1, "B");

                }


                ws.Column(1).Width = 15;
                ws.Column(2).Width = 15;
                
                ws.Rows().AdjustToContents();






                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reporte de Frentes.xlsx");
                }

            }

        }

        private void SetRowBorderStyle(IXLWorksheet ws, int rowCount, string v)
        {
            ws.Range($"B{rowCount}:{v}{rowCount}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range($"B{rowCount}:{v}{rowCount}").Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        }

        private void SetRowBorderStyle2(IXLWorksheet ws, int rowCount, string v)
        {
            ws.Range($"A{rowCount}:{v}{rowCount}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range($"A{rowCount}:{v}{rowCount}").Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        }
    }
}