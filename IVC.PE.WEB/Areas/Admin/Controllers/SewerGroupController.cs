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
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.ENTITIES.UspModels.TechnicalOffice;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectCollaboratorGroupViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectCollaboratorViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.SewerGroupViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.UserViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontHeadViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.ProviderViewModels;
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
    [Route("admin/cuadrillas")]
    public class SewerGroupController : BaseController
    {
        public SewerGroupController(IvcDbContext context,
            ILogger<SewerGroupController> logger)
            : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(int? workComponent = null, int? workStructure = null, int? destination = null, string isActive = null)
        {
            SqlParameter todayParam = new SqlParameter("@Today", DateTime.Now);
            SqlParameter projectParam = new SqlParameter("@ProjectId", GetProjectId());

            var data = _context.Set<UspSewerGroup>().FromSqlRaw("execute Admin_uspSewerGroups @Today, @ProjectId"
                , todayParam, projectParam)
                .IgnoreQueryFilters()
                .AsQueryable();

            var sgs = await data.ToListAsync();

            if (workComponent.HasValue)
                sgs = sgs.Where(x => x.WorkComponent == workComponent.Value).ToList();
            if (workStructure.HasValue)
                sgs = sgs.Where(x => x.WorkStructure == workStructure.Value).ToList();
            if (destination.HasValue)
                sgs = sgs.Where(x => x.Destination == destination.Value).ToList();
            if (isActive == "ac")
                sgs = sgs.Where(x => x.IsActive).ToList();
            if (isActive == "in")
                sgs = sgs.Where(x => !x.IsActive).ToList();

            return Ok(sgs.OrderBy(x => x.WorkStructure));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var sewerGroup = await _context.SewerGroups
                .Where(x => x.Id == id)
                .Select(x => new SewerGroupViewModel
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    Type = x.Type
                }).FirstOrDefaultAsync();

            var period = await _context.SewerGroupPeriods
                .Where(x => x.SewerGroupId == sewerGroup.Id)
                .OrderByDescending(x => x.DateStart)
                .FirstOrDefaultAsync();

            sewerGroup.Period = new SewerGroupPeriodViewModel
            {
                Id = period.Id,
                SewerGroupId = period.SewerGroupId,
                //WorkFrontId = period.WorkFrontId,
                WorkFrontHeadId = period.WorkFrontHeadId,
                Destination = period.Destination,
                WorkComponent = period.WorkComponent,
                WorkStructure = period.WorkStructure,
                ProviderId = period.ProviderId,
                ProjectCollaboratorId = period.ProjectCollaboratorId,
                ForemanId = period.ForemanEmployeeId ?? period.ForemanWorkerId,
                DateStart = period.DateStart.ToDateString(),
                DateEnd = period.DateEnd.HasValue ? period.DateEnd.Value.ToDateString() : string.Empty
            };

            sewerGroup.Period.WorkFrontIds = await _context.WorkFrontSewerGroups
                .Where(x => x.SewerGroupPeriodId == period.Id)
                .Select(x => x.WorkFrontId)
                .ToListAsync();

            return Ok(sewerGroup);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(SewerGroupViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var sewerGroup = new SewerGroup
            {
                Code = model.Code,
                Name = model.Name,
                Type = model.Type,
                ProjectId = GetProjectId()
            };
            var period = new SewerGroupPeriod
            {
                SewerGroup = sewerGroup,
                //WorkFrontId = model.Period.WorkFrontId,
                WorkFrontHeadId = model.Period.WorkFrontHeadId,
                Destination = model.Period.Destination,
                DateStart = model.Period.DateStart.ToDateTime()
            };
            if (!string.IsNullOrEmpty(model.Period.DateEnd))
                period.DateEnd = model.Period.DateEnd.ToDateTime();
            if (period.Destination == ConstantHelpers.Sewer.Group.Destination.COLLABORATOR)
            {
                period.ProviderId = model.Period.ProviderId;
                period.ProjectCollaboratorId = model.Period.ProjectCollaboratorId;
            }                
            if (period.Destination == ConstantHelpers.Sewer.Group.Destination.LOCAL)
            {
                if (await _context.Workers.AnyAsync(x => x.Id == model.Period.ForemanId))
                    period.ForemanWorkerId = model.Period.ForemanId;
                else if (await _context.Employees.AnyAsync(x => x.Id == model.Period.ForemanId))
                    period.ForemanEmployeeId = model.Period.ForemanId;
            }

            if (model.Period.WorkFrontIds != null)
            {
                await _context.WorkFrontSewerGroups.AddRangeAsync(
                    model.Period.WorkFrontIds.Select(x => new WorkFrontSewerGroup
                    {
                        SewerGroupPeriod = period,
                        WorkFrontId = x
                    }).ToList()
                );
            }            
            await _context.SewerGroups.AddAsync(sewerGroup);
            await _context.SewerGroupPeriods.AddAsync(period);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, SewerGroupViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var sewerGroup = await _context.SewerGroups.FirstOrDefaultAsync(x => x.Id == id);
            sewerGroup.Code = model.Code;
            sewerGroup.Name = model.Name;
            sewerGroup.Type = model.Type;
            var period = await _context.SewerGroupPeriods.FirstOrDefaultAsync(x => x.Id == model.Period.Id);
            period.WorkFrontHeadId = model.Period.WorkFrontHeadId;
            period.Destination = model.Period.Destination;
            period.WorkStructure = model.Period.WorkStructure;
            period.WorkComponent = model.Period.WorkComponent;
            period.DateEnd = null;
            period.ProviderId = null;
            period.ProjectCollaboratorId = null;
            period.ForemanWorkerId = null;
            period.ForemanEmployeeId = null;
            if (!string.IsNullOrEmpty(model.Period.DateEnd))
                period.DateEnd = model.Period.DateEnd.ToDateTime();
            if (period.Destination == ConstantHelpers.Sewer.Group.Destination.COLLABORATOR)
            {
                period.ProviderId = model.Period.ProviderId;
                period.ProjectCollaboratorId = model.Period.ProjectCollaboratorId;
            }                
            if (period.Destination == ConstantHelpers.Sewer.Group.Destination.LOCAL)
            {
                if (await _context.Workers.AnyAsync(x => x.Id == model.Period.ForemanId))
                    period.ForemanWorkerId = model.Period.ForemanId;
                else if (await _context.Employees.AnyAsync(x => x.Id == model.Period.ForemanId))
                    period.ForemanEmployeeId = model.Period.ForemanId;
            }
            var workFrontSgs = await _context.WorkFrontSewerGroups
                .Where(x => x.SewerGroupPeriodId == period.Id)
                .ToListAsync();
            _context.WorkFrontSewerGroups.RemoveRange(workFrontSgs);
            if (model.Period.WorkFrontIds != null)
            {
                await _context.WorkFrontSewerGroups.AddRangeAsync(
                    model.Period.WorkFrontIds.Select(x => new WorkFrontSewerGroup
                    {
                        SewerGroupPeriodId = period.Id,
                        WorkFrontId = x
                    }).ToList()
                );
            }
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]  
        public async Task<IActionResult> Delete(Guid id)
        {
            var sewerGroup = await _context.SewerGroups.FindAsync(id);
            if (sewerGroup == null)
                return BadRequest($"Cuadrilla con Id '{id}' no encontrada.");
            var periods = await _context.SewerGroupPeriods.Where(x => x.SewerGroupId == id).ToListAsync();
            var workfronts = new List<WorkFrontSewerGroup>();
            foreach (var period in periods)
            {
                workfronts.AddRange(await _context.WorkFrontSewerGroups
                    .Where(x => x.SewerGroupPeriodId == period.Id)
                    .ToListAsync());
            }
            if (workfronts.Count > 0)
                _context.WorkFrontSewerGroups.RemoveRange(workfronts);
            _context.SewerGroups.Remove(sewerGroup);
            _context.SewerGroupPeriods.RemoveRange(periods);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("importar")]
        public async Task<IActionResult> Import(IFormFile file)
        {
            var sgs = await _context.SewerGroups
                .Where(x => x.ProjectId == GetProjectId())
                .AsNoTracking()
                .ToListAsync();

            var workFronts = await _context.WorkFronts
                .Where(x => x.ProjectId == GetProjectId())
                .AsNoTracking()
                .ToListAsync();

            var collaborators = await _context.ProjectCollaborators
                .Where(x => x.ProjectId == GetProjectId())
                .AsNoTracking()
                .ToListAsync();

            var employees = await _context.EmployeeWorkPeriods
                .Include(x => x.Employee)
                .Where(x => x.ProjectId == GetProjectId())
                .AsNoTracking()
                .Select(x => x.Employee)
                .ToListAsync();

            var workers = await _context.WorkerWorkPeriods
                .Include(x => x.Worker)
                .Where(x => x.ProjectId == GetProjectId() &&
                        x.EntryDate.Value <= DateTime.Now.Date &&
                        (x.CeaseDate.HasValue ? x.CeaseDate.Value >= DateTime.Now.Date : true))
                .Select(x => x.Worker)
                .ToListAsync();

            using (var mem = new MemoryStream())
            {
                await file.CopyToAsync(mem);
                using (var workBook = new XLWorkbook(mem))
                {
                    var workSheet = workBook.Worksheets.FirstOrDefault(x => x.Name.ToUpper() == "CUADRILLAS");
                    var counter = 3;
                    var sewerGroups = new List<SewerGroup>();
                    while (!workSheet.Cell($"A{counter}").IsEmpty())
                    {
                        var code = workSheet.Cell($"A{counter}").GetString();
                        if (sgs.FirstOrDefault(x => x.Code.ToUpper() == code.ToUpper()) != null)
                        {
                            ++counter;
                            continue;
                        }

                        var sewerGroup = new SewerGroup();
                        sewerGroup.Code = code;
                        sewerGroup.ProjectId = GetProjectId();
                        var sGPeriod = new SewerGroupPeriod();
                        sGPeriod.SewerGroup = sewerGroup;
                        sGPeriod.IsActive = true;
                        if (Int32.TryParse(workSheet.Cell($"D{counter}").GetString(),out int wc))
                            sGPeriod.WorkComponent = wc;
                        else
                        {
                            ++counter;
                            continue;
                        }
                        if (Int32.TryParse(workSheet.Cell($"E{counter}").GetString(), out int ws))
                            sGPeriod.WorkStructure = ws;
                        else
                        {
                            ++counter;
                            continue;
                        }
                        if (Int32.TryParse(workSheet.Cell($"F{counter}").GetString(), out int d))
                            sGPeriod.Destination = d;
                        else
                        {
                            ++counter;
                            continue;
                        }
                        var resIdStr = workSheet.Cell($"G{counter}").GetString();
                        if (Guid.TryParse(resIdStr, out Guid resId))
                            switch (sGPeriod.Destination)
                            {
                                case 1:
                                    var resEmployee = employees.FirstOrDefault(x => x.Id == resId);
                                    if (resEmployee != null)
                                        sGPeriod.ForemanEmployeeId = resId;
                                    else
                                    {
                                        var resWorker = workers.FirstOrDefault(x => x.Id == resId);
                                        if (resWorker != null)
                                            sGPeriod.ForemanWorkerId = resId;
                                    }
                                    break;
                                default:
                                    var collab = collaborators.FirstOrDefault(x => x.Id == resId);
                                    if (collab != null)
                                    {
                                        sGPeriod.ProviderId = collab.ProviderId;
                                        sGPeriod.ProjectCollaboratorId = resId;
                                    }
                                    break;
                            }
                        if (GetDateTime(workSheet.Cell($"H{counter}"), out DateTime? dateStart).HasValue)
                            sGPeriod.DateStart = dateStart.Value;
                        else
                        {
                            ++counter;
                            continue;
                        }
                        sGPeriod.DateEnd = GetDateTime(workSheet.Cell($"I{counter}"), out DateTime? dateEnd);

                        var sGwF = new List<WorkFrontSewerGroup>();
                        var wfs = workSheet.Cell($"B{counter}").GetString().Split(",");
                        foreach (var wf in wfs)
                        {
                            var wfDb = workFronts.FirstOrDefault(x => x.Code.ToUpper() == wf.ToUpper());
                            if (wfDb != null)
                            {
                                sGwF.Add(new WorkFrontSewerGroup
                                {
                                    SewerGroupPeriod = sGPeriod,
                                    WorkFrontId = wfDb.Id
                                });
                            }
                        }
                        
                        sewerGroups.Add(sewerGroup);
                        ++counter;
                    }

                    await _context.SewerGroups.AddRangeAsync(sewerGroups);
                    await _context.SaveChangesAsync();
                }
                mem.Close();
            }
            return Ok();
        }

        [HttpGet("importar-formato")]
        public FileResult GetImportFormat()
        {
            var workFronts = _context.WorkFronts
                .Where(x => x.ProjectId == GetProjectId())
                .Select(x => x.Code)
                .ToList();

            var providers = _context.ProviderSupplyGroups
                .Include(x => x.Provider)
                .Include(x => x.SupplyGroup)
                .Where(x => x.SupplyGroup.Code == "200")
                .Select(x => x.ProviderId)
                .ToList();

            var proCollab = _context.ProjectCollaborators
                .AsQueryable();

            var collaborators = new List<ProjectCollaborator>();
            foreach (var p in providers)
            {
                collaborators.AddRange(
                    proCollab.Where(x => x.ProviderId == p && x.ProjectId == GetProjectId())
                        .ToList()
                );
            }

            var foremans = new List<Tuple<Guid,string>>();
            foremans.AddRange(
                _context.EmployeeWorkPeriods
                    .Include(x => x.Employee)
                    .Where(x => x.ProjectId == GetProjectId() && x.IsActive)
                    .Select(x => new Tuple<Guid, string>
                    (
                        x.EmployeeId, 
                        x.Employee.FullName
                    )).ToList()
            );
            foremans.AddRange(
                _context.WorkerWorkPeriods
                    .Include(x => x.Worker)
                    .Where(x => x.ProjectId == GetProjectId() &&
                            x.EntryDate.Value <= DateTime.Now.Date &&
                            (x.CeaseDate.HasValue ? x.CeaseDate.Value >= DateTime.Now.Date : true))
                    .Select(x => new Tuple<Guid, string>
                    (
                        x.WorkerId.Value,
                        x.Worker.FullName
                    )).ToList()
            );

            DataTable workFrontDt = GetWorkFrontData(workFronts);
            DataTable collabDt = GetCollaboratoData(collaborators);
            DataTable responDt = GetForemanData(foremans);

            using (XLWorkbook wb = new XLWorkbook())
            {
                var workSheet = wb.Worksheets.Add("Cuadrillas");

                workSheet.Cell($"A1").Value = "Código";
                workSheet.Range("A1:A2").Merge(false);
                workSheet.Cell($"A3").Value = "RRP-200";

                workSheet.Cell($"B1").Value = "Frentes";
                workSheet.Range($"B1:B2").Merge(false);
                workSheet.Cell($"B3").Value = "F1,F2,F3";

                workSheet.Cell($"C1").Value = "Jefe de Frente";
                workSheet.Range($"C1:C2").Merge(false);
                workSheet.Cell($"C3").Value = "Colocar Id del Frentes";

                workSheet.Cell($"D1").Value = "Componente de Obra";
                workSheet.Range($"D1:D2").Merge(false);
                workSheet.Cell($"D3").Value = "Colocar Id del Componente de Obra";

                workSheet.Cell($"E1").Value = "Estructura";
                workSheet.Range($"E1:E2").Merge(false);
                workSheet.Cell($"E3").Value = "Colocar Id de la Estructura";

                workSheet.Cell($"F1").Value = "Destino";
                workSheet.Range($"F1:F2").Merge(false);
                workSheet.Cell($"F3").Value = "Colocar Id del Destino";

                workSheet.Cell($"G1").Value = "Responsable";
                workSheet.Range($"G1:G2").Merge(false);
                workSheet.Cell($"G3").Value = "Colocar Id del Responsable";

                workSheet.Cell($"H1").Value = "Vigencia Inicio";
                workSheet.Range($"H1:H2").Merge(false);
                workSheet.Cell($"H3").Value = DateTime.Today.Date;

                workSheet.Cell($"I1").Value = "Vigencia Fin";
                workSheet.Range($"I1:I2").Merge(false);
                workSheet.Cell($"I3").Value = DateTime.Today.AddDays(30).Date;
                workSheet.Range("A1:I2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"K1").Value = "Id";
                workSheet.Cell($"L1").Value = "Componente de Obra";
                var row = 2;
                foreach (var ws in ConstantHelpers.Sewer.Group.WorkComponent.VALUES)
                {
                    workSheet.Cell($"K{row}").Value = ws.Key;
                    workSheet.Cell($"L{row}").Value = ws.Value;
                    row++;
                }
                workSheet.Range("K1:L1").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"N1").Value = "Id";
                workSheet.Cell($"O1").Value = "Estructura";
                row = 2;
                foreach (var ws in ConstantHelpers.Sewer.Group.WorkStructure.VALUES)
                {
                    workSheet.Cell($"N{row}").Value = ws.Key;
                    workSheet.Cell($"O{row}").Value = ws.Value;
                    row++;
                }
                workSheet.Range("N1:O1").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"Q1").Value = "Id";
                workSheet.Cell($"R1").Value = "Destino";
                row = 2;
                foreach (var ws in ConstantHelpers.Sewer.Group.Destination.VALUES)
                {
                    workSheet.Cell($"Q{row}").Value = ws.Key;
                    workSheet.Cell($"R{row}").Value = ws.Value;
                    row++;
                }
                workSheet.Range("Q1:R1").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                wb.Worksheets.Add(workFrontDt);
                wb.Worksheets.Add(collabDt);
                wb.Worksheets.Add(responDt);

                workSheet.Rows().AdjustToContents();
                workSheet.Columns().AdjustToContents();

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Cuadrillas.xlsx");
                }
            }
        }

        private DateTime? GetDateTime(IXLCell cell, out DateTime? datetime)
        {
            datetime = null;
            if (!cell.IsEmpty())
            {
                if (cell.DataType == XLDataType.DateTime)
                {
                    var dt = cell.GetDateTime();
                    datetime = new DateTime(dt.Year, dt.Month, dt.Day);
                }
                else
                {
                    var dtStr = cell.GetString();
                    if (!string.IsNullOrEmpty(dtStr) && DateTime.TryParse(dtStr, out DateTime dt))
                        datetime = new DateTime(dt.Year, dt.Month, dt.Day);
                }
            }
            return datetime;
        }

        private DataTable GetForemanData(List<Tuple<Guid, string>> data)
        {
            DataTable dt = new DataTable();
            dt.TableName = "Responsables";

            dt.Columns.Add("Id", typeof(string));
            dt.Columns.Add("Empleado/Obrero", typeof(string));

            foreach (var item in data)
            {
                dt.Rows.Add(
                    item.Item1,
                    item.Item2
                    );
            }

            dt.AcceptChanges();
            return dt;
        }

        private DataTable GetCollaboratoData(List<ProjectCollaborator> data)
        {
            DataTable dt = new DataTable();
            dt.TableName = "Colaboradores";

            dt.Columns.Add("Id", typeof(string));
            dt.Columns.Add("Colaborador", typeof(string));

            foreach (var item in data)
            {
                dt.Rows.Add(
                    item.Id.ToString(),
                    item.FullName
                    );
            }

            dt.AcceptChanges();
            return dt;
        }

        private DataTable GetWorkFrontData(List<string> data)
        {
            DataTable dt = new DataTable();
            dt.TableName = "Frentes";

            dt.Columns.Add("Frente", typeof(string));

            foreach (var item in data)
            {
                dt.Rows.Add(
                    item
                    );
            }

            dt.AcceptChanges();
            return dt;
        }
    }
}