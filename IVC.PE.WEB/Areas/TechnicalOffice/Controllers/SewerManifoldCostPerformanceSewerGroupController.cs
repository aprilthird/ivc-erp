using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.WEB.Areas.Admin.ViewModels.SewerGroupViewModels;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.ProjectCalendarWeekViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.SewerManifoldCostPerformanceViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IVC.PE.WEB.Areas.TechnicalOffice.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.TECHNICAL_OFFICE)]
    [Route("oficina-tecnica/cuadrillas-colector-descarga-cyr")]
    public class SewerManifoldCostPerformanceSewerGroupController : BaseController
    {
        public SewerManifoldCostPerformanceSewerGroupController(IvcDbContext context,
            ILogger<SewerManifoldCostPerformanceSewerGroupController> logger) : base(context, logger)
        {
        }

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? weekId = null)
        {
            if (!weekId.HasValue)
                return Ok(new List<SewerManifoldCostPerformanceSewerGroupViewModel>());

            var sgs = await _context.SewerManifoldCostPerformanceSewerGroups
                .Include(x => x.SewerManifoldCostPerformance)
                .Include(x => x.ProjectCalendarWeek)
                .Where(x => x.SewerManifoldCostPerformance.ProjectId == GetProjectId() &&
                            x.ProjectCalendarWeekId == weekId.Value)
                .Select(x => new SewerManifoldCostPerformanceSewerGroupViewModel
                {
                    Id = x.Id,
                    ProjectCalendarWeekId = x.ProjectCalendarWeekId,
                    ProjectCalendarWeek = new ProjectCalendarWeekViewModel
                    {
                        YearWeekNumber = x.ProjectCalendarWeek.YearWeekNumber
                    },
                    SewerManifoldCostPerformanceId = x.SewerManifoldCostPerformanceId,
                    SewerManifoldCostPerformance = new SewerManifoldCostPerformanceViewModel
                    {
                        Description = x.SewerManifoldCostPerformance.Description,
                        TerrainType = x.SewerManifoldCostPerformance.TerrainType,
                        SecurityFactor = x.SewerManifoldCostPerformance.SecurityFactor,
                        Workforce = x.SewerManifoldCostPerformance.Workforce,
                        Equipment = x.SewerManifoldCostPerformance.Equipment,
                        Services = x.SewerManifoldCostPerformance.Services
                    },
                    SewerGroupId = x.SewerGroupId,
                    SewerGroup = new SewerGroupViewModel
                    {
                        Code = x.SewerGroup.Code
                    },
                    WorkforceEquipment = x.WorkforceEquipment,
                    WorkforceEquipmentService = x.WorkforceEquipmentService,
                    SecurityFactor = x.SecurityFactor
                })
                .ToListAsync();

            return Ok(sgs);
        }

        [HttpGet("programaciones")]
        public async Task<IActionResult> GetForSewerGroupSchedule(Guid sgId, Guid wId, double ditch)
        {
            var cp = await _context.SewerManifoldCostPerformances
                .FirstOrDefaultAsync(x => x.HeightMin <= ditch && x.HeightMax >= ditch);

            var cpsgs = await _context.SewerManifoldCostPerformanceSewerGroups
                .Include(x => x.SewerManifoldCostPerformance)
                .Include(x => x.ProjectCalendarWeek)
                .Where(x => x.SewerManifoldCostPerformance.ProjectId == GetProjectId() &&
                            x.SewerManifoldCostPerformanceId == cp.Id)
                .OrderByDescending(x => x.ProjectCalendarWeek.WeekStart)
                .ToListAsync();

            var sgs = cpsgs
                .Where(x => x.ProjectCalendarWeekId == wId &&
                            x.SewerGroupId == sgId)
                .Select(x => new SewerManifoldCostPerformanceSewerGroupViewModel
                {
                    Id = x.Id,
                    SewerManifoldCostPerformance = new SewerManifoldCostPerformanceViewModel
                    {
                        SecurityFactor = x.SewerManifoldCostPerformance.SecurityFactor,
                        Workforce = x.SewerManifoldCostPerformance.Workforce,
                        Equipment = x.SewerManifoldCostPerformance.Equipment,
                        Services = x.SewerManifoldCostPerformance.Services
                    },
                    WorkforceEquipment = x.WorkforceEquipment,
                    WorkforceEquipmentService = x.WorkforceEquipmentService,
                    SecurityFactor = x.SecurityFactor
                })
                .FirstOrDefault();

            if (sgs == null)
            {
                sgs = cpsgs
                .Where(x => x.SewerGroupId == sgId)
                .Select(x => new SewerManifoldCostPerformanceSewerGroupViewModel
                {
                    Id = x.Id,
                    SewerManifoldCostPerformance = new SewerManifoldCostPerformanceViewModel
                    {
                        SecurityFactor = x.SewerManifoldCostPerformance.SecurityFactor,
                        Workforce = x.SewerManifoldCostPerformance.Workforce,
                        Equipment = x.SewerManifoldCostPerformance.Equipment,
                        Services = x.SewerManifoldCostPerformance.Services
                    },
                    WorkforceEquipment = x.WorkforceEquipment,
                    WorkforceEquipmentService = x.WorkforceEquipmentService,
                    SecurityFactor = x.SecurityFactor
                })
                .FirstOrDefault();

                if (sgs == null)
                    return Ok(0);
            }

            return Ok(sgs.WorkforceEquipmentMinLength < sgs.WorkforceEquipmentServiceMinLength ? sgs.WorkforceEquipmentServiceMinLength : sgs.WorkforceEquipmentMinLength);
        }

        [HttpGet("cargar-costos-modelo")]
        public FileResult GenerateCPExcel()
        {
            var cps = _context.SewerManifoldCostPerformances
                .Where(x => x.ProjectId == GetProjectId())
                .ToList();            

            using (XLWorkbook wb = new XLWorkbook())
            {
                var workSheet = wb.Worksheets.Add("CyR");

                workSheet.Cell($"A1").Value = "Id";
                workSheet.Range("A1:A3").Merge();
                workSheet.Range("A1:A3").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"B1").Value = "CyR";
                workSheet.Range("B1:B3").Merge();
                workSheet.Range("B1:B3").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"C1").Value = "Semana";
                workSheet.Range("C1:C3").Merge();
                workSheet.Range("C1:C3").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"D1").Value = "Cuadrilla";
                workSheet.Range("D1:D3").Merge();
                workSheet.Range("D1:D3").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"E1").Value = "Venta Contractual";
                workSheet.Range("E1:F2").Merge();
                workSheet.Cell($"E3").Value = "MO+EQ";
                workSheet.Cell($"F3").Value = "MO+EQ+S/C";
                workSheet.Range("E1:F3").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                var row = 4;
                foreach (var cp in cps)
                {
                    workSheet.Cell($"A{row}").Value = cp.Id;
                    workSheet.Cell($"B{row}").Value = cp.Description;
                    row++;
                }

                workSheet.Columns().AdjustToContents();

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "FormatoCargaCPSg.xlsx");
                }
            }
        }

        [HttpPost("cargar-costos")]
        public async Task<IActionResult> LoadCPExcel(SewerManifoldLoadSewerGroupCPViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var sgs = ReadCPExcel(model);

            await _context.SewerManifoldCostPerformanceSewerGroups.AddRangeAsync(sgs);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private List<SewerManifoldCostPerformanceSewerGroup> ReadCPExcel(SewerManifoldLoadSewerGroupCPViewModel model)
        {
            var sgs = new List<SewerManifoldCostPerformanceSewerGroup>();

            using (var mem = new MemoryStream())
            {
                model.File.CopyTo(mem);

                using (var workBook = new XLWorkbook(mem))
                {
                    var ws = workBook.Worksheets.First();
                    var counter = 4;

                    var weeks = _context.ProjectCalendarWeeks.ToList();
                    var sewergroups = _context.SewerGroups.ToList();

                    while (!ws.Cell($"A{counter}").IsEmpty())
                    {
                        var sg = new SewerManifoldCostPerformanceSewerGroup();

                        sg.SewerManifoldCostPerformanceId = Guid.Parse(ws.Cell($"A{counter}").GetString());
                        sg.SecurityFactor = model.SecurityFactor;

                        var yearkWeek = ws.Cell($"C{counter}").GetString();
                        sg.ProjectCalendarWeekId = weeks.FirstOrDefault(x => x.YearWeekNumber == yearkWeek).Id;

                        var sgCode = ws.Cell($"D{counter}").GetString();
                        sg.SewerGroupId = sewergroups.FirstOrDefault(x => x.Code == sgCode).Id;

                        sg.WorkforceEquipment = ws.Cell($"E{counter}").GetDouble();
                        sg.WorkforceEquipmentService = ws.Cell($"F{counter}").GetDouble();

                        sgs.Add(sg);

                        counter++;
                    }
                }
            }

            return sgs;
        }
    }
}
