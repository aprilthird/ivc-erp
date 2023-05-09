using ClosedXML.Excel;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
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
    [Route("admin/frentes/cuadrillas")]
    public class WorkFrontSewerGroupController : BaseController
    {
        public WorkFrontSewerGroupController(IvcDbContext context)
            : base(context)
        {
        }

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? wfId = null)
        {
            if (!wfId.HasValue)
                return Ok(new List<WorkFrontSewerGroupViewModel>());

            var data = await _context.WorkFrontSewerGroups
                .Include(x => x.SewerGroupPeriod)
                .Include(x => x.SewerGroupPeriod.SewerGroup)
                .Where(x => x.WorkFrontId == wfId.Value &&
                    (x.SewerGroupPeriod.DateStart.Date <= DateTime.Today.Date &&
                    x.SewerGroupPeriod.DateEnd.HasValue ? x.SewerGroupPeriod.DateEnd >= DateTime.Today.Date : true))
                .Select(x => new WorkFrontSewerGroupViewModel
                {
                    WorkFrontId = x.WorkFrontId,
                    SewerGroupId = x.SewerGroupPeriodId,
                    SewerGroupCode = x.SewerGroupPeriod.SewerGroup.Code
                }).ToListAsync();

            data = data.OrderByDescending(x => x.SewerGroupCode).ToList();

            return Ok(data);
        }

        [HttpGet("importar-formato")]
        public FileResult GetImportFormat()
        {
            var workfronts = _context.WorkFronts
                .AsNoTracking()
                .ToList();

            var sewerGroupPeriods = _context.SewerGroupPeriods
                .Include(x => x.SewerGroup)
                .Where(x => x.DateStart.Date <= DateTime.Today.Date &&
                    x.DateEnd.HasValue ? x.DateEnd >= DateTime.Today.Date : true)
                .AsNoTracking()
                .ToList();

            DataTable dtWorkFronts = GetWorkFrontsData(workfronts);
            DataTable dtSewerGroups = GetSewerGroupsData(sewerGroupPeriods);

            string fileName = "FrentesCuadrillas.xlsx";
            using (XLWorkbook wb = new XLWorkbook())
            {
                var workSheet = wb.Worksheets.Add("FrentesFases");

                workSheet.Cell($"A1").Value = "Frente";
                workSheet.Range("A1:A2").Merge(false);
                workSheet.Range("A1:A2").Style.Font.SetFontColor(XLColor.White);
                workSheet.Range("A1:A2").Style.Fill.SetBackgroundColor(XLColor.Navy);

                workSheet.Cell($"B1").Value = "Cuadrilla";
                workSheet.Range("B1:B2").Merge(false);
                workSheet.Range("B1:B2").Style.Font.SetFontColor(XLColor.White);
                workSheet.Range("B1:B2").Style.Fill.SetBackgroundColor(XLColor.Navy);

                workSheet.Columns().AdjustToContents();
                workSheet.Rows().AdjustToContents();

                wb.Worksheets.Add(dtWorkFronts);
                wb.Worksheets.Add(dtSewerGroups);

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
                    var workFronts = _context.WorkFronts.ToList();
                    var sewerGroupPeriods = _context.SewerGroupPeriods
                        .Include(x => x.SewerGroup)
                        .Where(x => x.DateStart.Date <= DateTime.Today.Date &&
                            x.DateEnd.HasValue ? x.DateEnd >= DateTime.Today.Date : true)
                        .ToList();

                    var wfSgDb = _context.WorkFrontSewerGroups
                            .ToList();

                    var wfSg = new List<WorkFrontSewerGroup>();
                    while (!workSheet.Cell($"A{counter}").IsEmpty())
                    {
                        string workFrontCode = workSheet.Cell($"A{counter}").GetString();
                        string sewerGroupCode = workSheet.Cell($"B{counter}").GetString();

                        var workFrontDb = workFronts.FirstOrDefault(x => x.Code == workFrontCode);
                        var sewerGroupDb = sewerGroupPeriods.FirstOrDefault(x => x.SewerGroup.Code == sewerGroupCode);
                        
                        if (workFrontDb != null && sewerGroupDb != null)
                        {
                            if (!wfSgDb.Any(x => x.WorkFrontId == workFrontDb.Id &&
                                        x.SewerGroupPeriodId == sewerGroupDb.Id))
                            {
                                if (!wfSg.Any(x => x.WorkFrontId == workFrontDb.Id &&
                                        x.SewerGroupPeriodId == sewerGroupDb.Id))
                                {
                                    wfSg.Add(new WorkFrontSewerGroup
                                    {
                                        SewerGroupPeriodId = sewerGroupDb.Id,
                                        WorkFrontId = workFrontDb.Id
                                    });
                                }
                            }
                        }
                        ++counter;
                    }

                    if (wfSg.Count > 0)
                        await _context.WorkFrontSewerGroups.AddRangeAsync(wfSg);

                    await _context.SaveChangesAsync();
                }
                mem.Close();
            }
            return Ok();
        }

        [HttpDelete("eliminar")]
        public async Task<IActionResult> Delete(Guid wfId, Guid sgpId)
        {
            var wfsgp = await _context.WorkFrontSewerGroups
                .FirstAsync(x => x.WorkFrontId == wfId && x.SewerGroupPeriodId == sgpId);

            _context.WorkFrontSewerGroups.Remove(wfsgp);

            await _context.SaveChangesAsync();

            return Ok();
        }

        private DataTable GetSewerGroupsData(List<SewerGroupPeriod> sewerGroupPeriods)
        {
            DataTable dt = new DataTable();
            dt.TableName = "Cuadrillas";

            dt.Columns.Add("Cuadrilla", typeof(string));

            foreach (var sewergroup in sewerGroupPeriods)
            {
                dt.Rows.Add(
                    sewergroup.SewerGroup.Code
                    );
            }

            dt.AcceptChanges();
            return dt;
        }

        private DataTable GetWorkFrontsData(List<WorkFront> workfronts)
        {
            DataTable dt = new DataTable();
            dt.TableName = "Frentes";

            dt.Columns.Add("Frente", typeof(string));

            foreach (var workfront in workfronts)
            {
                dt.Rows.Add(
                    workfront.Code
                    );
            }

            dt.AcceptChanges();
            return dt;
        }
    }
}
