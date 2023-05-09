using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Production;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectFormulaViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.SewerGroupViewModels;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.ProjectCalendarWeekViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.MeasurementUnitViewModels;
using IVC.PE.WEB.Areas.Production.ViewModels.SewerGroupScheduleViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.SewerManifoldViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace IVC.PE.WEB.Areas.Production.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Production.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.PRODUCTION)]
    [Route("produccion/programacion-diaria")]
    public class SeweGroupScheduleActivityController : BaseController
    {
        public SeweGroupScheduleActivityController(IvcDbContext context,
            ILogger<SeweGroupScheduleActivityController> logger)
            : base(context, logger)
        {
        }

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? scheduleId = null)
        {
            if (scheduleId == null)
                return Ok(new List<SewerGroupScheduleActivityFullViewModel>());

            var activities = await _context.SewerGroupScheduleActivities
                .Include(x => x.SewerManifold)
                .Include(x => x.ProjectFormulaActivity)
                .Include(x => x.ProjectFormulaActivity.MeasurementUnit)
                .Where(x => x.SewerGroupDailyScheduleId == scheduleId.Value)
                .Select(x => new SewerGroupScheduleActivityFullViewModel
                {
                    Id = x.Id,
                    ProjectFormulaActivityId = x.ProjectFormulaActivityId,
                    ProjectFormulaActivity = new ProjectFormulaActivityViewModel
                    {
                        Description = x.ProjectFormulaActivity.Description,
                        MeasurementUnit = new MeasurementUnitViewModel
                        {
                            Abbreviation = x.ProjectFormulaActivity.MeasurementUnit.Abbreviation
                        }
                    },
                    SewerManifoldId = x.SewerManifoldId,
                    SewerManifold = new SewerManifoldViewModel
                    {
                        Name = x.SewerManifold.Name
                    }
                }).ToListAsync();

            var dailies = await _context.SewerGroupScheduleActivityDailies
                .Include(x => x.SewerGroupScheduleActivity)
                .Where(x => x.SewerGroupScheduleActivity.SewerGroupDailyScheduleId == scheduleId.Value)
                .ToListAsync();

            foreach (var activity in activities)
            {
                var daily = dailies.Where(x => x.SewerGroupScheduleActivityId == activity.Id).ToList();

                foreach (var day in daily)
                {
                    switch (day.ReportDate.DayOfWeek)
                    {
                        case DayOfWeek.Monday:
                            activity.FootageMonday = day.FootageDaily;
                            break;
                        case DayOfWeek.Tuesday:
                            activity.FootageTuesday = day.FootageDaily;
                            break;
                        case DayOfWeek.Wednesday:
                            activity.FootageWednesday = day.FootageDaily;
                            break;
                        case DayOfWeek.Thursday:
                            activity.FootageThrusday = day.FootageDaily;
                            break;
                        case DayOfWeek.Friday:
                            activity.FootageFriday = day.FootageDaily;
                            break;
                        case DayOfWeek.Saturday:
                            activity.FootageSaturday = day.FootageDaily;
                            break;
                        default:
                            break;
                    }
                }
            }

            return Ok(activities);
        }

        [HttpGet("f4-listar")]
        public async Task<IActionResult> GetAllF4(Guid? scheduleId = null)
        {
            if (scheduleId == null)
                return Ok(new List<SewerGroupScheduleActivityFullViewModel>());

            return Ok(new List<SewerGroupScheduleActivityFullViewModel>());
        }

        [HttpGet("f5-listar")]
        public async Task<IActionResult> GetAllF5(Guid? scheduleId = null)
        {
            if (scheduleId == null)
                return Ok(new List<SewerGroupScheduleActivityFullViewModel>());

            return Ok(new List<SewerGroupScheduleActivityFullViewModel>());
        }

        [HttpGet("f6-listar")]
        public async Task<IActionResult> GetAllF6(Guid? scheduleId = null)
        {
            if (scheduleId == null)
                return Ok(new List<SewerGroupScheduleActivityFullViewModel>());

            return Ok(new List<SewerGroupScheduleActivityFullViewModel>());
        }

        [HttpGet("f56-listar")]
        public async Task<IActionResult> GetAllF56(Guid? scheduleId = null)
        {
            if (scheduleId == null)
                return Ok(new List<SewerGroupScheduleActivityFullViewModel>());

            return Ok(new List<SewerGroupScheduleActivityFullViewModel>());
        }

        [HttpGet("f7-listar")]
        public async Task<IActionResult> GetAllF7(Guid? scheduleId = null)
        {
            if (scheduleId == null)
                return Ok(new List<SewerGroupScheduleActivityFullViewModel>());

            return Ok(new List<SewerGroupScheduleActivityFullViewModel>());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var activity = await _context.SewerGroupScheduleActivities
                .Include(x => x.SewerManifold)
                .Include(x => x.ProjectFormulaActivity)
                .Include(x => x.ProjectFormulaActivity.MeasurementUnit)
                .Where(x => x.Id == id)
                .Select(x => new SewerGroupScheduleActivityFullViewModel
                {
                    Id = x.Id,
                    ProjectFormulaActivityId = x.ProjectFormulaActivityId,
                    ProjectFormulaActivity = new ProjectFormulaActivityViewModel
                    {
                        Description = x.ProjectFormulaActivity.Description,
                        MeasurementUnit = new MeasurementUnitViewModel
                        {
                            Abbreviation = x.ProjectFormulaActivity.MeasurementUnit.Abbreviation
                        }
                    },
                    SewerManifoldId = x.SewerManifoldId,
                    SewerManifold = new SewerManifoldViewModel
                    {
                        Name = x.SewerManifold.Name
                    }
                }).FirstOrDefaultAsync();

            var dailies = await _context.SewerGroupScheduleActivityDailies
                .Where(x => x.SewerGroupScheduleActivityId == id)
                .ToListAsync();

            foreach (var day in dailies)
            {
                switch (day.ReportDate.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        activity.FootageMonday = day.FootageDaily;
                        break;
                    case DayOfWeek.Tuesday:
                        activity.FootageTuesday = day.FootageDaily;
                        break;
                    case DayOfWeek.Wednesday:
                        activity.FootageWednesday = day.FootageDaily;
                        break;
                    case DayOfWeek.Thursday:
                        activity.FootageThrusday = day.FootageDaily;
                        break;
                    case DayOfWeek.Friday:
                        activity.FootageFriday = day.FootageDaily;
                        break;
                    case DayOfWeek.Saturday:
                        activity.FootageSaturday = day.FootageDaily;
                        break;
                    default:
                        break;
                }
            }

            return Ok(activity);
        }

        [HttpGet("actividades/{id}")]
        public async Task<IActionResult> GetActivitiesForModal(Guid? id = null)
        {
            if (id == null)
                return Ok(new List<SewerGroupScheduleActivityFullViewModel>());

            var activities = await _context.SewerGroupScheduleActivities
                .Include(x => x.SewerManifold)
                .Include(x => x.ProjectFormulaActivity)
                .Where(x => x.SewerGroupDailyScheduleId == id.Value)
                .Select(x => new SewerGroupScheduleActivityFullViewModel
                {
                    Id = x.Id,
                    ProjectFormulaActivityId = x.ProjectFormulaActivityId,
                    ProjectFormulaActivity = new ProjectFormulaActivityViewModel
                    {
                        Description = x.ProjectFormulaActivity.Description
                    },
                    SewerManifoldId = x.SewerManifoldId,
                    SewerManifold = new SewerManifoldViewModel
                    {
                        Name = x.SewerManifold.Name
                    }
                }).ToListAsync();

            var dailies = await _context.SewerGroupScheduleActivityDailies
                .Include(x => x.SewerGroupScheduleActivity)
                .Where(x => x.SewerGroupScheduleActivity.SewerGroupDailyScheduleId == id.Value)
                .ToListAsync();

            foreach (var activity in activities)
            {
                var daily = dailies.Where(x => x.SewerGroupScheduleActivityId == activity.Id).ToList();

                foreach (var day in daily)
                {
                    switch (day.ReportDate.DayOfWeek)
                    {
                        case DayOfWeek.Monday:
                            activity.FootageMonday = day.FootageDaily;
                            break;
                        case DayOfWeek.Tuesday:
                            activity.FootageTuesday = day.FootageDaily;
                            break;
                        case DayOfWeek.Wednesday:
                            activity.FootageWednesday = day.FootageDaily;
                            break;
                        case DayOfWeek.Thursday:
                            activity.FootageThrusday = day.FootageDaily;
                            break;
                        case DayOfWeek.Friday:
                            activity.FootageFriday = day.FootageDaily;
                            break;
                        case DayOfWeek.Saturday:
                            activity.FootageSaturday = day.FootageDaily;
                            break;
                        default:
                            break;
                    }
                }
            }

            return PartialView("_ActivityListResult", activities);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(SewerGroupScheduleActivityFullViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var activity = new SewerGroupScheduleActivity
            {
                SewerManifoldId = model.SewerManifoldId,
                SewerGroupDailyScheduleId = model.SewerGroupScheduleId,
                ProjectFormulaActivityId = model.ProjectFormulaActivityId                
            };

            var week = await _context.SewerGroupSchedules
                .Include(x => x.ProjectCalendarWeek)
                .Where(x => x.Id == model.SewerGroupScheduleId)
                .Select(x => x.ProjectCalendarWeek)
                .FirstOrDefaultAsync();
            var dailies = new List<SewerGroupScheduleActivityDaily>();
            for (int i = 0; i < 6; i++)
            {
                var date = week.WeekStart.AddDays(i).DayOfWeek;
                dailies.Add(new SewerGroupScheduleActivityDaily
                {
                    SewerGroupScheduleActivity = activity,
                    ReportDate = week.WeekStart.AddDays(i),
                    FootageDaily = date == DayOfWeek.Monday     ? model.FootageMonday ?? 0 :
                                   date == DayOfWeek.Tuesday    ? model.FootageTuesday ?? 0 :
                                   date == DayOfWeek.Wednesday  ? model.FootageWednesday ?? 0 :
                                   date == DayOfWeek.Thursday   ? model.FootageThrusday ?? 0 :
                                   date == DayOfWeek.Friday     ? model.FootageFriday ?? 0 :
                                                                  model.FootageSaturday ?? 0
                });
            }

            var footageCero = dailies.Where(x => x.FootageDaily == 0).ToList();
            foreach (var footage in footageCero)
                dailies.Remove(footage);

            await _context.SewerGroupScheduleActivities.AddAsync(activity);
            await _context.SewerGroupScheduleActivityDailies.AddRangeAsync(dailies);
            await _context.SaveChangesAsync();

            return Ok(activity.Id);
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, SewerGroupScheduleActivityFullViewModel model)
        {
            var activity = await _context.SewerGroupScheduleActivities
                .FirstOrDefaultAsync(x => x.Id == id);

            activity.ProjectFormulaActivityId = model.ProjectFormulaActivityId;
            activity.SewerManifoldId = model.SewerManifoldId;

            var dailiesDb = await _context.SewerGroupScheduleActivityDailies
                .Where(x => x.SewerGroupScheduleActivityId == id)
                .ToListAsync();

            var week = await _context.SewerGroupSchedules
                .Include(x => x.ProjectCalendarWeek)
                .Where(x => x.Id == model.SewerGroupScheduleId)
                .Select(x => x.ProjectCalendarWeek)
                .FirstOrDefaultAsync();
            var dailies = new List<SewerGroupScheduleActivityDaily>();
            for (int i = 0; i < 6; i++)
            {
                var date = week.WeekStart.AddDays(i).DayOfWeek;
                dailies.Add(new SewerGroupScheduleActivityDaily
                {
                    SewerGroupScheduleActivityId = activity.Id,
                    ReportDate = week.WeekStart.AddDays(i),
                    FootageDaily = date == DayOfWeek.Monday ? model.FootageMonday ?? 0 :
                                   date == DayOfWeek.Tuesday ? model.FootageTuesday ?? 0 :
                                   date == DayOfWeek.Wednesday ? model.FootageWednesday ?? 0 :
                                   date == DayOfWeek.Thursday ? model.FootageThrusday ?? 0 :
                                   date == DayOfWeek.Friday ? model.FootageFriday ?? 0 :
                                                                  model.FootageSaturday ?? 0
                });
            }

            var footageCero = dailies.Where(x => x.FootageDaily == 0).ToList();
            foreach (var footage in footageCero)
                dailies.Remove(footage);


            _context.SewerGroupScheduleActivityDailies.RemoveRange(dailiesDb);
            await _context.SewerGroupScheduleActivityDailies.AddRangeAsync(dailies);

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var activity = await _context.SewerGroupScheduleActivities
                .FirstOrDefaultAsync(x => x.Id == id);

            var dailies = await _context.SewerGroupScheduleActivityDailies
                .Where(x => x.SewerGroupScheduleActivityId == id)
                .ToListAsync();

            _context.SewerGroupScheduleActivityDailies.RemoveRange(dailies);
            _context.SewerGroupScheduleActivities.Remove(activity);

            await _context.SaveChangesAsync();

            return Ok();
        }



        [HttpGet("reporte-excel")]
        public async Task<IActionResult> GenerateExcel()
        {
            var activities = await _context.SewerGroupScheduleActivities
                .Include(x => x.SewerGroupDailySchedule.SewerGroup)
                .Include(x => x.SewerGroupDailySchedule.ProjectCalendarWeek)
                .Include(x => x.SewerGroupDailySchedule.WorkFrontHead.User)
                .Include(x => x.SewerManifold)
                .Include(x => x.ProjectFormulaActivity)
                .Include(x => x.ProjectFormulaActivity.ProjectFormula)
                .Include(x => x.ProjectFormulaActivity.MeasurementUnit)
                .Select(x => new SewerGroupScheduleExcelViewModel
                {
                    Id = x.Id,
                    FormulaName = $"{x.ProjectFormulaActivity.ProjectFormula.Code} {x.ProjectFormulaActivity.ProjectFormula.Name}",
                    SewerManifoldAddress = x.SewerManifold.Address,
                    SewerManifoldName = x.SewerManifold.Name,
                    ActivityDescription = x.ProjectFormulaActivity.Description,
                    WorkFrontHeadName = x.SewerGroupDailySchedule.WorkFrontHead.User.FullName,
                    WorkFrontHeadContactNumber = x.SewerGroupDailySchedule.WorkFrontHead.User.PhoneNumber,
                    SewerGroupCode = x.SewerGroupDailySchedule.SewerGroup.Code,
                    SewerGroupForemanId = x.SewerGroupDailySchedule.SewerGroup.ForemanEmployeeId ?? x.SewerGroupDailySchedule.SewerGroup.ForemanWorkerId,
                    SewerGroupForemanName = string.Empty,
                    SewerGroupForemanContactNumber = string.Empty,
                    MeasurementUnitAbbrevitation = x.ProjectFormulaActivity.MeasurementUnit.Abbreviation
                }).ToListAsync();

            var employees = await _context.Employees.ToListAsync();
            var workers = await _context.Workers.ToListAsync();
            var dailies = await _context.SewerGroupScheduleActivityDailies.ToListAsync();

            foreach (var activity in activities)
            {
                var daily = dailies.Where(x => x.SewerGroupScheduleActivityId == activity.Id).ToList();

                if (activity.SewerGroupForemanId.HasValue)
                {
                    var employee = employees.FirstOrDefault(x => x.Id == activity.SewerGroupForemanId.Value);
                    if (employee != null)
                    {
                        activity.SewerGroupForemanName = employee.FullName;
                        activity.SewerGroupForemanContactNumber = employee.PhoneNumber;
                    } else
                    {
                        var worker = workers.FirstOrDefault(x => x.Id == activity.SewerGroupForemanId.Value);
                        if (worker != null)
                        {
                            activity.SewerGroupForemanName = worker.FullName;
                            activity.SewerGroupForemanContactNumber = worker.PhoneNumber;
                        }
                    }
                }

                foreach (var day in daily)
                {
                    switch (day.ReportDate.DayOfWeek)
                    {
                        case DayOfWeek.Monday:
                            activity.FootageMonday = day.FootageDaily;
                            break;
                        case DayOfWeek.Tuesday:
                            activity.FootageTuesday = day.FootageDaily;
                            break;
                        case DayOfWeek.Wednesday:
                            activity.FootageWednesday = day.FootageDaily;
                            break;
                        case DayOfWeek.Thursday:
                            activity.FootageThrusday = day.FootageDaily;
                            break;
                        case DayOfWeek.Friday:
                            activity.FootageFriday = day.FootageDaily;
                            break;
                        case DayOfWeek.Saturday:
                            activity.FootageSaturday = day.FootageDaily;
                            break;
                        default:
                            break;
                    }
                }
            }

            var formulas = new List<List<SewerGroupScheduleExcelViewModel>>();

            foreach (var f in activities.Select(x => x.FormulaName).Distinct())
            {
                formulas.Add(activities.Where(x => x.FormulaName == f).ToList());
            }

            TempData["historico-actividades"] = JsonConvert.SerializeObject(formulas);

            return Ok("historico-actividades");
        }

        [HttpGet("descargar-excel")]
        public async Task<IActionResult> DownloadExcel(string excelName)
        {
            if (TempData[excelName] == null)
            {
                RedirectToAction("Empty");
            }

            DataTable dt = new DataTable();
            string fileName = string.Empty;
            switch (excelName)
            {
                case "historico-actividades":
                    var activities = JsonConvert.DeserializeObject<List<SewerGroupScheduleExcelViewModel>>(TempData[excelName].ToString());
                    fileName = "actividades_semanales.xlsx";
                    dt = GetData(activities);
                    break;
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                //Add DataTable in worksheet
                wb.Worksheets.Add(dt);
                wb.Worksheet(dt.TableName).Columns().AdjustToContents();
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        private DataTable GetData(List<SewerGroupScheduleExcelViewModel> activities)
        {
            //Creating DataTable
            DataTable dt = new DataTable
            {
                //Setting Table Name
                TableName = "REPORTE ACTIVIDADES"
            };
            //Add Columns
            dt.Columns.Add("Tramo", typeof(string));
            dt.Columns.Add("Actividad", typeof(string));
            dt.Columns.Add("Jefe de Frente", typeof(string));
            dt.Columns.Add("Jefe de Frente Contacto", typeof(string));
            dt.Columns.Add("Cuadrilla", typeof(string));
            dt.Columns.Add("Cuadrilla Capataz", typeof(string));
            dt.Columns.Add("Cuadrilla Capataz Contacto", typeof(string));
            dt.Columns.Add("Unidad", typeof(string));
            dt.Columns.Add("Total", typeof(double));
            dt.Columns.Add("Lunes", typeof(double));
            dt.Columns.Add("Martes", typeof(double));
            dt.Columns.Add("Miercoles", typeof(double));
            dt.Columns.Add("Jueves", typeof(double));
            dt.Columns.Add("Viernes", typeof(double));
            dt.Columns.Add("Sábado", typeof(double));
            //Add Rows in DataTable
            foreach (var item in activities)
            {
                dt.Rows.Add(
                    item.SewerManifoldName,
                    item.ActivityDescription,
                    item.WorkFrontHeadName,
                    item.WorkFrontHeadContactNumber,
                    item.SewerGroupCode,
                    item.SewerGroupForemanName,
                    item.SewerGroupForemanContactNumber,
                    item.MeasurementUnitAbbrevitation,
                    item.FootageTotal,
                    item.FootageMonday,
                    item.FootageTuesday,
                    item.FootageWednesday,
                    item.FootageThrusday,
                    item.FootageFriday,
                    item.FootageSaturday
                );
            }
            dt.AcceptChanges();
            return dt;
        }


        [HttpGet("actividades-excel")]
        public FileResult ActivitiesToExcel(string tempData)
        {
            var activities = JsonConvert.DeserializeObject<List<List<SewerGroupScheduleExcelViewModel>>>(TempData[tempData].ToString());

            using (XLWorkbook wb = new XLWorkbook())
            {
                string fileName = "programaciones.xlsx";

                var wsSchedule = wb.Worksheets.Add("Programaciones");

                var nAct = 1;
                var row = 3;
                foreach (var item in activities)
                {
                    AddHeader(wsSchedule);

                    wsSchedule.Cell($"A{row}").Value = item.FirstOrDefault().FormulaName;
                    wsSchedule.Range($"A{row}:R{row}").Merge();
                    wsSchedule.Range($"A{row}:R{row}").Style.Fill.SetBackgroundColor(XLColor.LightBlue);
                    row++;

                    foreach (var act in item)
                    {
                        AddRow(wsSchedule, act, row, nAct);
                        row++;
                        nAct++;
                    }
                }

                wsSchedule.Columns().AdjustToContents();
                AddBorders(wsSchedule, row);

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        private void AddBorders(IXLWorksheet ws, int row)
        {
            ws.Range($"A1:R{row}").Style.Border.OutsideBorderColor = XLColor.Black;
            ws.Range($"A1:R{row}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Range($"A1:R{row}").Style.Border.InsideBorderColor = XLColor.Black;
            ws.Range($"A1:R{row}").Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        }

        private void AddRow(IXLWorksheet ws, SewerGroupScheduleExcelViewModel act, int row, int nAct)
        {
            ws.Cell($"A{row}").Value = nAct;
            ws.Cell($"B{row}").Value = act.SewerManifoldAddress;
            ws.Cell($"C{row}").Value = string.Empty;
            ws.Cell($"D{row}").Value = act.SewerManifoldName;
            ws.Cell($"E{row}").Value = act.ActivityDescription;
            ws.Cell($"F{row}").Value = act.WorkFrontHeadName;
            ws.Cell($"G{row}").Value = act.WorkFrontHeadContactNumber;
            ws.Cell($"H{row}").Value = act.SewerGroupCode;
            ws.Cell($"I{row}").Value = act.SewerGroupForemanName;
            ws.Cell($"J{row}").Value = act.SewerGroupForemanContactNumber;
            ws.Cell($"K{row}").Value = act.MeasurementUnitAbbrevitation;
            ws.Cell($"L{row}").Value = act.FootageTotal;
            ws.Cell($"M{row}").Value = act.FootageMonday;
            ws.Cell($"N{row}").Value = act.FootageTuesday;
            ws.Cell($"O{row}").Value = act.FootageWednesday;
            ws.Cell($"P{row}").Value = act.FootageThrusday;
            ws.Cell($"Q{row}").Value = act.FootageFriday;
            ws.Cell($"R{row}").Value = act.FootageSaturday;
        }

        private void AddHeader(IXLWorksheet ws)
        {
            ws.Cell($"A1").Value = "Item";
            ws.Range("A1:A2").Merge();
            ws.Range("A1:A2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

            ws.Cell($"B1").Value = "Ubicación";
            ws.Cell($"B2").Value = "Habilitación/Calle";
            ws.Cell($"C2").Value = "Código";
            ws.Cell($"D2").Value = "Tramo";
            ws.Range("B1:D1").Merge();
            ws.Range("B1:D2").Style.Fill.SetBackgroundColor(XLColor.Yellow);


            ws.Cell($"E1").Value = "Actividades";
            ws.Range("E1:E2").Merge();
            ws.Range("E1:E2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

            ws.Cell($"F1").Value = "Jefe de Frente";
            ws.Cell($"F2").Value = "Nombre";
            ws.Cell($"G2").Value = "# Contacto";
            ws.Range("F1:G1").Merge();
            ws.Range("F1:G2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

            ws.Cell($"H1").Value = "Cuadrilla";
            ws.Cell($"H2").Value = "Nombre";
            ws.Cell($"I2").Value = "Capataz";
            ws.Cell($"J2").Value = "# Contacto";
            ws.Range("H1:J1").Merge();
            ws.Range("H1:J2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

            ws.Cell($"K1").Value = "Metrado";
            ws.Cell($"K2").Value = "Unidad";
            ws.Cell($"L2").Value = "Total";
            ws.Range("K1:L1").Merge();
            ws.Range("K1:L2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

            ws.Cell($"M1").Value = "Días";
            ws.Cell($"M2").Value = "L";
            ws.Cell($"N2").Value = "M";
            ws.Cell($"O2").Value = "M";
            ws.Cell($"P2").Value = "J";
            ws.Cell($"Q2").Value = "V";
            ws.Cell($"R2").Value = "S";
            ws.Range("M1:R1").Merge();
            ws.Range("M1:R2").Style.Fill.SetBackgroundColor(XLColor.Yellow);
        }
    }
}
