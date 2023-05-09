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
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectFormulaViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectPhaseViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IVC.PE.WEB.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.Roles.SUPERADMIN)]
    [Area(ConstantHelpers.Areas.ADMIN)]
    [Route("admin/fases")]
    public class ProjectPhaseController : BaseController
    {
        public ProjectPhaseController(IvcDbContext context,
            ILogger<ProjectPhaseController> logger)
            : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? formulaId = null)
        {
            var pId = GetProjectId();
            var data = await _context.ProjectPhases
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectId == pId && 
                            (formulaId.HasValue ? x.ProjectFormulaId == formulaId.Value : true))
                .Select(x => new ProjectPhaseViewModel
                {
                    Id = x.Id,
                    Code = x.Code,
                    Description = x.Description,
                    Formula = x.ProjectFormula != null ? x.ProjectFormula.Code : string.Empty
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var workFront = await _context.ProjectPhases
                .Where(x => x.Id == id)
                .Select(x => new ProjectPhaseViewModel
                {
                    Id = x.Id,
                    Code = x.Code,
                    Description = x.Description,
                    ProjectFormulaId = x.ProjectFormulaId
                }).FirstOrDefaultAsync();
            return Ok(workFront);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(ProjectPhaseViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var projectPhase = new ProjectPhase
            {
                Code = model.Code,
                Description = model.Description,
                ProjectId = GetProjectId(),
                ProjectFormulaId = model.ProjectFormulaId
            };
            await _context.ProjectPhases.AddAsync(projectPhase);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, ProjectPhaseViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var projectPhase = await _context.ProjectPhases.FindAsync(id);
            projectPhase.Code = model.Code;
            projectPhase.Description = model.Description;
            projectPhase.ProjectFormulaId = model.ProjectFormulaId;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var projectPhase = await _context.ProjectPhases.FirstOrDefaultAsync(x => x.Id == id);
            if (projectPhase == null)
                return BadRequest($"Fase con Id '{id}' no encontrado.");
            var workFrontPhases = await _context.WorkFrontProjectPhases
                .Where(x => x.ProjectPhaseId == id)
                .ToListAsync();
            if (workFrontPhases.Count > 0)
                _context.WorkFrontProjectPhases.RemoveRange(workFrontPhases);

            _context.ProjectPhases.Remove(projectPhase);
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
                    var workSheet = workBook.Worksheets.FirstOrDefault();
                    var counter = 4;


                    while (!workSheet.Cell($"A{counter}").IsEmpty())
                    {
                        var selectedFormula = await _context.ProjectFormulas
                           .FirstOrDefaultAsync(x => x.Code == workSheet.Cell($"A{counter}").GetString() && x.ProjectId == GetProjectId());
                        var phlist = new List<ProjectPhase>();


                            var ph = new ProjectPhase();

                            ph.Id = Guid.NewGuid();
                            ph.ProjectFormulaId = selectedFormula.Id;
                            ph.ProjectId = GetProjectId();
                            ph.Code = workSheet.Cell($"B{counter}").GetString();
                            ph.Description = workSheet.Cell($"C{counter}").GetString();
                            
                            phlist.Add(ph);
                            await _context.ProjectPhases.AddRangeAsync(phlist);
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
            

            string fileName = "Fases.xlsx";
            using (XLWorkbook wb = new XLWorkbook())
            {
                var workSheet = wb.Worksheets.Add("Fases");

                workSheet.Cell($"A1").Value = "F1";
                workSheet.Cell($"A2").Value = "Formula";
                workSheet.Range("A2:A3").Merge(false);
                workSheet.Range("A2:A3").Style.Font.SetFontColor(XLColor.White);
                workSheet.Range("A2:A3").Style.Fill.SetBackgroundColor(XLColor.Navy);

                workSheet.Cell($"B1").Value = "011025";
                workSheet.Cell($"B2").Value = "Código Fase";
                workSheet.Range("B2:B3").Merge(false);
                workSheet.Range("B2:B3").Style.Font.SetFontColor(XLColor.White);
                workSheet.Range("B2:B3").Style.Fill.SetBackgroundColor(XLColor.Navy);


                workSheet.Cell($"C2").Value = "Descripción Fase";
                workSheet.Range("C2:C3").Merge(false);
                workSheet.Range("C2:C3").Style.Font.SetFontColor(XLColor.White);
                workSheet.Range("C2:C3").Style.Fill.SetBackgroundColor(XLColor.Navy);

                workSheet.Column(1).Width = 20;
                workSheet.Column(2).Width = 20;
                workSheet.Column(3).Width = 20;
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
        public async Task<IActionResult> Export()
        {
            var dt = new DataTable("Fases");
            dt.Columns.Add("Fórmula", typeof(string));
            dt.Columns.Add("Código", typeof(string));
            dt.Columns.Add("Descripción", typeof(string));

            var data = await _context.ProjectPhases
                .Include(x => x.ProjectFormula)
                .Where(x=>x.ProjectId == GetProjectId())
                .AsNoTracking()
                .ToListAsync();
            data.ForEach(item =>
            {
                dt.Rows.Add(item.ProjectFormula?.Name, 
                    item.Code,
                    item.Description);
            });

            var fileName = "Fases.xlsx";
            using (var wb = new XLWorkbook())
            {
                var workSheet = wb.Worksheets.Add(dt);

                workSheet.Column(1).Width = 20;
                workSheet.Column(2).Width = 20;
                workSheet.Column(3).Width = 20;

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