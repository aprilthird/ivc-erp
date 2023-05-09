using ClosedXML.Excel;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectPhaseViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.Roles.SUPERADMIN)]
    [Area(ConstantHelpers.Areas.ADMIN)]
    [Route("admin/frentes/fases")]
    public class WorkFrontProjectPhaseController : BaseController
    {
        public WorkFrontProjectPhaseController(IvcDbContext context)
            : base(context)
        {
        }

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? workFrontId = null, Guid? formulaId = null)
        {
            if (!workFrontId.HasValue)
                return Ok(new List<WorkFrontProjectPhaseViewModel>());

            var phases = await _context.WorkFrontProjectPhases
                .Include(x => x.ProjectPhase)
                .Include(x => x.ProjectPhase.ProjectFormula)
                .Where(x => x.WorkFrontId == workFrontId.Value)
                .Select(x => new WorkFrontProjectPhaseViewModel
                {
                    WorkFrontId = x.WorkFrontId,
                    ProjectPhaseId = x.ProjectPhaseId,
                    ProjectPhase = new ProjectPhaseViewModel
                    {
                        Code = x.ProjectPhase.Code,
                        Description = x.ProjectPhase.Description
                    },
                    FormulaId = x.ProjectPhase.ProjectFormulaId.Value,
                    FormulaCode = x.ProjectPhase.ProjectFormula.Code
                }).ToListAsync();

            if (formulaId.HasValue)
                phases = phases.Where(x => x.FormulaId == formulaId.Value).ToList();

            return Ok(phases);
        }

        [HttpGet("importar-formato")]
        public FileResult GetImportFormat()
        {
            var phases = _context.ProjectPhases
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectId == GetProjectId())
                .ToList();

            var workFronts = _context.WorkFronts
                .Where(x => x.ProjectId == GetProjectId())
                .ToList();

            DataTable dtPhases = GetPhasesData(phases);
            DataTable dtWorkFronts = GetWorkFrontData(workFronts);

            string fileName = "FrentesFases.xlsx";
            using (XLWorkbook wb = new XLWorkbook())
            {
                var workSheet = wb.Worksheets.Add("FrentesFases");

                workSheet.Cell($"C1").Value = "Frente";
                workSheet.Range("C1:C2").Merge();
                workSheet.Range("C1:C2").Style.Font.SetFontColor(XLColor.White);
                workSheet.Range("C1:C2").Style.Fill.SetBackgroundColor(XLColor.Navy);
                workSheet.Cell($"C3").Value = "FT-01";
                workSheet.Cell($"C4").Value = "FT-01";
                workSheet.Cell($"C5").Value = "FT-02";

                workSheet.Cell($"D1").Value = "Fase";
                workSheet.Range("D1:D2").Merge();
                workSheet.Range("D1:D2").Style.Font.SetFontColor(XLColor.White);
                workSheet.Range("D1:D2").Style.Fill.SetBackgroundColor(XLColor.Navy);
                workSheet.Cell($"D3").Value = "010010";
                workSheet.Cell($"D4").Value = "010011";
                workSheet.Cell($"D5").Value = "010010";

                workSheet.Columns().AdjustToContents();
                workSheet.Rows().AdjustToContents();

                wb.Worksheets.Add(dtPhases);
                wb.Worksheets.Add(dtWorkFronts);

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
        public async Task<IActionResult> Import(IFormFile file)
        {
            using (var mem = new MemoryStream())
            {
                await file.CopyToAsync(mem);
                using (var workBook = new XLWorkbook(mem))
                {
                    var workSheet = workBook.Worksheets.First();
                    var counter = 3;
                    var workFronts = await _context.WorkFronts.Where(x => x.ProjectId == GetProjectId()).ToListAsync();
                    var phases = await _context.ProjectPhases.Where(x => x.ProjectId == GetProjectId()).ToListAsync();
                    var formulas = await _context.ProjectFormulas.Where(x => x.ProjectId == GetProjectId()).ToListAsync();

                    var workFrontPhasesDb = await _context.WorkFrontProjectPhases.ToListAsync();

                    var workFrontPhases = new List<WorkFrontProjectPhase>();
                    var newWorkFronts = new List<WorkFront>();
                    while (!workSheet.Cell($"C{counter}").IsEmpty())
                    {
                        string workFrontCode = workSheet.Cell($"C{counter}").GetString();
                        string phaseCode = workSheet.Cell($"D{counter}").GetString();

                        var phaseDb = phases.FirstOrDefault(x => x.Code == phaseCode);
                        var workFrontDb = workFronts.FirstOrDefault(x => x.Code == workFrontCode);

                        if (phaseDb != null)
                        {
                            if (workFrontDb != null)
                            {
                                var wfpExists = workFrontPhasesDb
                                    .FirstOrDefault(x => x.WorkFrontId == workFrontDb.Id &&
                                                        x.ProjectPhaseId == phaseDb.Id);

                                if (wfpExists == null)
                                {
                                    workFrontPhases.Add(new WorkFrontProjectPhase
                                    {
                                        WorkFront = workFrontDb,
                                        ProjectPhaseId = phaseDb.Id
                                    });
                                }
                            }
                            else
                            {
                                var workFrontAlreadyCreate = newWorkFronts
                                    .FirstOrDefault(x => x.Code == workFrontCode);

                                if(workFrontAlreadyCreate != null)
                                {
                                    workFrontPhases.Add(new WorkFrontProjectPhase
                                    {
                                        WorkFront = workFrontAlreadyCreate,
                                        ProjectPhaseId = phaseDb.Id
                                    });
                                }
                                else
                                {
                                    var newWF = new WorkFront
                                    {
                                        ProjectId = GetProjectId(),
                                        Code = workFrontCode
                                    };

                                    newWorkFronts.Add(newWF);
                                    workFrontPhases.Add(new WorkFrontProjectPhase
                                    {
                                        WorkFront = newWF,
                                        ProjectPhaseId = phaseDb.Id
                                    });
                                }
                            }
                        }

                        ++counter;
                    }

                    if (newWorkFronts.Count > 0)
                        await _context.WorkFronts.AddRangeAsync(newWorkFronts);

                    if (workFrontPhases.Count > 0)
                        await _context.WorkFrontProjectPhases.AddRangeAsync(workFrontPhases);

                    await _context.SaveChangesAsync();
                }
                mem.Close();
            }
            return Ok();
        }

        [HttpDelete("eliminar")]
        public async Task<IActionResult> Delete(Guid wfId, Guid ppId)
        {
            var phase = await _context.WorkFrontProjectPhases
                .FirstOrDefaultAsync(x => x.WorkFrontId == wfId && x.ProjectPhaseId == ppId);

            _context.WorkFrontProjectPhases.Remove(phase);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private DataTable GetWorkFrontData(List<WorkFront> workFronts)
        {
            DataTable dt = new DataTable();
            dt.TableName = "Frentes";

            dt.Columns.Add("Frente", typeof(string));

            foreach (var workfront in workFronts)
            {
                dt.Rows.Add(
                    workfront.Code
                    );
            }

            dt.AcceptChanges();
            return dt;
        }
        private DataTable GetPhasesData(List<ProjectPhase> phases)
        {
            DataTable dt = new DataTable();
            dt.TableName = "Fases";

            dt.Columns.Add("Formula", typeof(string));
            dt.Columns.Add("Fase", typeof(string));
            dt.Columns.Add("Descripción", typeof(string));

            foreach (var phase in phases)
            {
                dt.Rows.Add(
                    phase.ProjectFormula != null ? phase.ProjectFormula.Code : string.Empty,
                    phase.Code,
                    phase.Description
                    );
            }

            dt.AcceptChanges();
            return dt;
        }
    }
}
