using ClosedXML.Excel;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectFormulaViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectPhaseViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.Roles.SUPERADMIN)]
    [Area(ConstantHelpers.Areas.ADMIN)]
    [Route("admin/formulas/fases")]
    public class ProjectFormulaPhaseController : BaseController
    {
        public ProjectFormulaPhaseController(IvcDbContext context)
            : base(context)
        {
        }

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? formulaId = null)
        {
            if (!formulaId.HasValue)
                return Ok(new List<ProjectPhaseViewModel>());

            var phases = await _context.ProjectPhases
                .Where(x => x.ProjectFormulaId == formulaId.Value)
                .Select(x => new ProjectPhaseViewModel
                {
                    Id = x.Id,
                    Code = x.Code,
                    Description = x.Description
                }).ToListAsync();

            return Ok(phases);
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var phaseDb = await _context.ProjectPhases.FindAsync(id);

            phaseDb.ProjectFormulaId = null;
            
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("importar")]
        public async Task<IActionResult> Import(IFormFile file)
        {
            var phasesDb = await _context.ProjectPhases.Where(x => x.ProjectId == GetProjectId()).ToListAsync();
            var formulasDb = await _context.ProjectFormulas.Where(x => x.ProjectId == GetProjectId()).ToListAsync();
            using (var mem = new MemoryStream())
            {
                await file.CopyToAsync(mem);
                using (var workBook = new XLWorkbook(mem))
                {
                    var workSheet = workBook.Worksheets.FirstOrDefault(x => x.Name == "Fases por Formula");
                    var counter = 2;
                    var newPhases = new List<ProjectPhase>();
                    while (!workSheet.Cell($"A{counter}").IsEmpty())
                    {
                        var formulaCode = workSheet.Cell($"A{counter}").GetString();
                        var formula = formulasDb.FirstOrDefault(x => x.Code.ToUpper() == formulaCode.ToUpper());
                        if (formula!=null)
                        {
                            var phaseCode = workSheet.Cell($"B{counter}").GetString();
                            var phase = phasesDb.FirstOrDefault(x => x.Code.ToUpper() == phaseCode.ToUpper());
                            if (phase!=null)
                            {
                                phase.ProjectFormulaId = formula.Id;
                                phase.Description = workSheet.Cell($"C{counter}").GetString();
                            } else
                            {
                                var alreadyExist = newPhases.FirstOrDefault(x => x.Code.ToUpper() == phaseCode.ToUpper());
                                if (alreadyExist != null)
                                {
                                    alreadyExist.ProjectFormulaId = formula.Id;
                                    alreadyExist.Description = workSheet.Cell($"C{counter}").GetString();
                                } else
                                {
                                    newPhases.Add(new ProjectPhase
                                    {
                                        ProjectFormulaId = formula.Id,
                                        Code = phaseCode,
                                        Description = workSheet.Cell($"C{counter}").GetString(),
                                        ProjectId = GetProjectId()
                                    });
                                }                                
                            }
                        }
                        ++counter;
                    }
                    if (newPhases.Count > 0)
                        await _context.ProjectPhases.AddRangeAsync(newPhases);
                    await _context.SaveChangesAsync();
                }
                mem.Close();
            }
            return Ok();
        }

        [HttpGet("modelo-excel")]
        public FileResult GetExcelSample()
        {
            var formulas = _context.ProjectFormulas.Where(x => x.ProjectId == GetProjectId()).ToList();
            var phases = _context.ProjectPhases.Where(x => x.ProjectId == GetProjectId()).ToList();

            using (XLWorkbook wb = new XLWorkbook())
            {
                var workSheet = wb.Worksheets.Add("Fases por Formula");

                workSheet.Cell($"A1").Value = "Formula";
                workSheet.Cell($"A2").Value = "F1";

                workSheet.Column("B").Style.NumberFormat.Format = "@";
                workSheet.Cell($"B1").Value = "Fase";
                workSheet.Cell($"B2").Value = "01010102";
                
                workSheet.Cell($"C1").Value = "Descripción";
                workSheet.Cell($"C2").Value = "Obras Preliminares";
                workSheet.Range("A1:C1").Style.Fill.SetBackgroundColor(XLColor.Yellow);
                

                workSheet.Cell($"E1").Value = "Formula";
                workSheet.Column("E").Style.NumberFormat.Format = "@";
                workSheet.Cell($"F1").Value = "Descripción";
                workSheet.Range("E1:F1").Style.Fill.SetBackgroundColor(XLColor.Yellow);
                var frow = 2;
                foreach (var formula in formulas)
                {
                    workSheet.Cell($"E{frow}").Value = formula.Code;
                    workSheet.Cell($"F{frow}").Value = formula.Name;
                    frow++;
                }

                workSheet.Cell($"H1").Value = "Fase";
                workSheet.Column("H").Style.NumberFormat.Format = "@";
                workSheet.Cell($"I1").Value = "Descripción";
                workSheet.Range("H1:I1").Style.Fill.SetBackgroundColor(XLColor.Yellow);
                var prow = 2;
                foreach (var phase in phases)
                {
                    workSheet.Cell($"H{prow}").Value = phase.Code;
                    workSheet.Cell($"I{prow}").Value = phase.Description;
                    prow++;
                }

                workSheet.Columns().AdjustToContents();
                workSheet.Rows().AdjustToContents();

                workSheet.Cell($"A5").Value = "En caso no exista la fase, se creará una con la información ingresada.";
                workSheet.Cell($"A6").Value = "En caso exista la fase, se actualizará con la información ingresada.";

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "FormulaFase.xlsx");
                }
            }
        }
    }
}
