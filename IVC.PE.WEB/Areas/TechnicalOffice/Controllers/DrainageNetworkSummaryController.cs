using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.WEB.Areas.Admin.ViewModels.QualificationZoneViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.SewerGroupViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.SystemPhaseViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.DrainageNetworkSummaryViewModels;
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
    [Route("oficina-tecnica/consolidado-alcantarillado")]
    public class DrainageNetworkSummaryController : BaseController
    {
        public DrainageNetworkSummaryController(IvcDbContext context,
            ILogger<DrainageNetworkSummaryController> logger)
            : base(context, logger)
        {
        }

        public IActionResult Index() => View();
        /*
        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid projectId, int? stage = null, Guid? workFrontId = null, Guid? sewerGroupId = null, int? terrainType = null, bool? hasFor47 = null)
        {
            var search = Request.Query[ConstantHelpers.Datatable.ServerSide.SentParameters.SEARCH_VALUE].ToString();
            var currentNumber = Convert.ToInt32(Request.Query[ConstantHelpers.Datatable.ServerSide.SentParameters.PAGING_FIRST_RECORD]);
            var recordsPerPage = Convert.ToInt32(Request.Query[ConstantHelpers.Datatable.ServerSide.SentParameters.RECORDS_PER_DRAW]);

            var query = _context.SewerLines
                .Where(x => x.SewerGroup.Type == ConstantHelpers.Sewer.Group.Type.DRAINAGE)
                .AsNoTracking()
                .AsQueryable();

            if (stage.HasValue)
                query = query.Where(x => x.Stage == stage.Value);
            if(workFrontId.HasValue)
                query = query.Where(x => x.SewerGroup.WorkFrontId == workFrontId.Value);
            if (sewerGroupId.HasValue)
                query = query.Where(x => x.SewerGroupId == sewerGroupId.Value);
            if(terrainType.HasValue)
                query = query.Where(x => x.TerrainType == terrainType.Value);
            if(hasFor47.HasValue)
                query = query.Where(x => x.HasFor47 == hasFor47.Value);

            var totalRecords = await query.CountAsync();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Address.Contains(search) ||
                    x.SewerGroup.Code.Contains(search) ||
                    x.InitialSewerBox.Code.Contains(search) ||
                    x.FinalSewerBox.Code.Contains(search));

            var data = await query
                .Skip(currentNumber)
                .Take(recordsPerPage)
                .Select(x => new SewerLineViewModel
                {
                    Id = x.Id,
                    SewerGroupId = x.SewerGroupId,
                    SewerGroup = new SewerGroupViewModel
                    {
                        Code = x.SewerGroup.Code,
                        WorkFront = new WorkFrontViewModel
                        {
                            Code = x.SewerGroup.WorkFront.Code,
                            SystemPhase = new SystemPhaseViewModel
                            {
                                Code = x.SewerGroup.WorkFront.SystemPhase.Code
                            }
                        }
                    },
                    QualificationZone = new QualificationZoneViewModel
                    {
                        Code = x.QualificationZone.Code,
                        Name = x.QualificationZone.Name
                    },
                    Address = x.Address,
                    DrainageArea = x.DrainageArea,
                    InitialSewerDrainageArea = x.InitialSewerBox.DrainageArea,
                    InitialSewerBoxCode = x.InitialSewerBox.Code,
                    InitialSewerBoxHeight = x.InitialSewerBox.Height,
                    InitialSewerBoxInternalDiameter = x.InitialSewerBox.InternalDiameter,
                    InitialSewerBoxThickness = x.InitialSewerBox.Thickness,
                    InitialSewerBoxTerrainType = x.InitialSewerBox.TerrainType,
                    InitialSewerBoxType = x.InitialSewerBox.Type,
                    FinalSewerDrainageArea = x.FinalSewerBox.DrainageArea,
                    FinalSewerBoxCode = x.FinalSewerBox.Code,
                    FinalSewerBoxHeight = x.FinalSewerBox.Height,
                    FinalSewerBoxInternalDiameter = x.FinalSewerBox.InternalDiameter,
                    FinalSewerBoxThickness = x.FinalSewerBox.Thickness,
                    FinalSewerBoxTerrainType = x.FinalSewerBox.TerrainType,
                    FinalSewerBoxType = x.FinalSewerBox.Type,
                    AverageDepthSewerBox = x.AverageDepthSewerBox,
                    AverageDepthSewerLine = x.AverageDepthSewerLine,
                    WaterUpCover = x.InitialSewerBox.Cover,
                    WaterUpOutput = x.InitialSewerBox.InputOutput,
                    WaterUpBottom = x.InitialSewerBox.Bottom,
                    DownstreamCover = x.FinalSewerBox.Cover,
                    DownstreamInput = x.FinalSewerBox.InputOutput,
                    DownstreamBottom = x.FinalSewerBox.Bottom,
                    TiltedPipelineLengthOnAxis = x.TiltedPipelineLengthOnAxis,
                    HorizontalDistanceOnAxis = x.HorizontalDistanceOnAxis,
                    InstalledPipelineLength = x.InstalledPipelineLength,
                    ExcavationLength = x.ExcavationLength,
                    Slope = x.Slope,
                    NominalDiameter = x.NominalDiameter,
                    PipelineType = x.PipelineType,
                    PipelineClass = x.PipelineClass,
                    Piping = x.Piping,
                    TerrainType = x.TerrainType,
                    HasFor47 = x.HasFor47,
                    IsReviewed = x.IsReviewed,
                    ExcavationLengthPercentForNormal = x.ExcavationLengthPercentForNormal,
                    ExcavationLengthPercentForRocky = x.ExcavationLengthPercentForRocky,
                    ExcavationLengthPercentForSemirocous = x.ExcavationLengthPercentForSemirocous,
                    ExcavationLengthForNormal = x.ExcavationLengthForNormal,
                    ExcavationLengthForRocky = x.ExcavationLengthForRocky,
                    ExcavationLengthForSemirocous = x.ExcavationLengthForSemirocous,
                    AddedLately = x.AddedLately
                }).ToListAsync();

            return Ok(new { 
                draw = ConstantHelpers.Datatable.ServerSide.SentParameters.DRAW_COUNTER,
                recordsTotal = totalRecords,
                recordsFiltered = totalRecords,
                data
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var sewerLineSummary = await _context.SewerLines
                .Where(x => x.Id == id)
                .Select(x => new SewerLineViewModel
                {
                    Id = x.Id,
                    SewerGroupId = x.SewerGroupId,
                    Address = x.Address,
                    DrainageArea = x.DrainageArea,
                    InitialSewerDrainageArea = x.InitialSewerBox.DrainageArea,
                    InitialSewerBoxCode = x.InitialSewerBox.Code,
                    InitialSewerBoxHeight = x.InitialSewerBox.Height,
                    InitialSewerBoxInternalDiameter = x.InitialSewerBox.InternalDiameter,
                    InitialSewerBoxThickness = x.InitialSewerBox.Thickness,
                    InitialSewerBoxTerrainType = x.InitialSewerBox.TerrainType,
                    InitialSewerBoxType = x.InitialSewerBox.Type,
                    FinalSewerDrainageArea = x.FinalSewerBox.DrainageArea,
                    FinalSewerBoxCode = x.FinalSewerBox.Code,
                    FinalSewerBoxHeight = x.FinalSewerBox.Height,
                    FinalSewerBoxInternalDiameter = x.FinalSewerBox.InternalDiameter,
                    FinalSewerBoxThickness = x.FinalSewerBox.Thickness,
                    FinalSewerBoxTerrainType = x.FinalSewerBox.TerrainType,
                    FinalSewerBoxType = x.FinalSewerBox.Type,
                    AverageDepthSewerBox = x.AverageDepthSewerBox,
                    AverageDepthSewerLine = x.AverageDepthSewerLine,
                    WaterUpCover = x.InitialSewerBox.Cover,
                    WaterUpOutput = x.InitialSewerBox.InputOutput,
                    WaterUpBottom = x.InitialSewerBox.Bottom,
                    DownstreamCover = x.FinalSewerBox.Cover,
                    DownstreamInput = x.FinalSewerBox.InputOutput,
                    DownstreamBottom = x.FinalSewerBox.Bottom,
                    TiltedPipelineLengthOnAxis = x.TiltedPipelineLengthOnAxis,
                    HorizontalDistanceOnAxis = x.HorizontalDistanceOnAxis,
                    InstalledPipelineLength = x.InstalledPipelineLength,
                    ExcavationLength = x.ExcavationLength,
                    Slope = x.Slope,
                    NominalDiameter = x.NominalDiameter,
                    PipelineType = x.PipelineType,
                    PipelineClass = x.PipelineClass,
                    Piping = x.Piping,
                    TerrainType = x.TerrainType,
                    HasFor47 = x.HasFor47,
                    IsReviewed = x.IsReviewed,  
                    ExcavationLengthPercentForNormal = x.ExcavationLengthPercentForNormal,
                    ExcavationLengthPercentForRocky = x.ExcavationLengthPercentForRocky,
                    ExcavationLengthPercentForSemirocous = x.ExcavationLengthPercentForSemirocous,
                    ExcavationLengthForNormal = x.ExcavationLengthForNormal,
                    ExcavationLengthForRocky = x.ExcavationLengthForRocky,
                    ExcavationLengthForSemirocous = x.ExcavationLengthForSemirocous
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return Ok(sewerLineSummary);
        }

        [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.FULL_ACCESS)]
        [HttpPost("crear")]
        public async Task<IActionResult> Create(SewerLineViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var initialSewerBoxes = await _context.SewerBoxes
                .Where(x => x.Code == model.InitialSewerBoxCode && x.SewerGroupId == model.SewerGroupId)
                .AsNoTracking().ToListAsync();
            var finalSewerBoxes = await _context.SewerBoxes
                .Where(x => x.Code == model.FinalSewerBoxCode && x.SewerGroupId == model.SewerGroupId)
                .AsNoTracking().ToListAsync();

            var sewerLine = new SewerLine
            {
                Address = model.Address,
                DrainageArea = model.DrainageArea,
                SewerGroupId = model.SewerGroupId,
                InitialSewerBox = initialSewerBoxes.Any() 
                    ? initialSewerBoxes.FirstOrDefault(x => x.Stage == ConstantHelpers.Stage.CONTRACTUAL)
                    : new SewerBox
                    {
                        Code = model.InitialSewerBoxCode,
                        Stage = ConstantHelpers.Stage.CONTRACTUAL,
                        TerrainType = model.InitialSewerBoxTerrainType,
                        Type = model.InitialSewerBoxType,
                        Address = model.Address,
                        SewerGroupId = model.SewerGroupId,
                        DrainageArea = model.InitialSewerDrainageArea,
                        Cover = model.WaterUpCover,
                        Bottom = model.WaterUpBottom,
                        InputOutput = model.WaterUpOutput
                    },
                FinalSewerBox = finalSewerBoxes.Any()
                    ? finalSewerBoxes.FirstOrDefault(x => x.Stage == ConstantHelpers.Stage.CONTRACTUAL)
                    : new SewerBox
                    {
                        Code = model.FinalSewerBoxCode,
                        Stage = ConstantHelpers.Stage.CONTRACTUAL,
                        TerrainType = model.FinalSewerBoxTerrainType,
                        Type = model.FinalSewerBoxType,
                        Address = model.Address,
                        SewerGroupId = model.SewerGroupId,
                        DrainageArea = model.FinalSewerDrainageArea,
                        Cover = model.DownstreamCover,
                        Bottom = model.DownstreamBottom,
                        InputOutput = model.DownstreamInput
                    },
                Stage = ConstantHelpers.Stage.CONTRACTUAL,
                HorizontalDistanceOnAxis = model.HorizontalDistanceOnAxis,
                NominalDiameter = model.NominalDiameter,
                PipelineType = model.PipelineType,
                TerrainType = model.TerrainType
            };
            sewerLine.Calculate();
            await _context.SewerLines.AddAsync(sewerLine);
            await _context.SaveChangesAsync();
            if(initialSewerBoxes.Any())
                sewerLine.InitialSewerBox = initialSewerBoxes.FirstOrDefault(x => x.Stage == ConstantHelpers.Stage.STAKING);            
            else
                sewerLine.InitialSewerBox.Stage = ConstantHelpers.Stage.STAKING;
            if(finalSewerBoxes.Any())
                sewerLine.FinalSewerBox = finalSewerBoxes.FirstOrDefault(x => x.Stage == ConstantHelpers.Stage.STAKING);
            else
                sewerLine.FinalSewerBox.Stage = ConstantHelpers.Stage.STAKING;
            sewerLine.Stage = ConstantHelpers.Stage.STAKING;
            sewerLine.Id = Guid.NewGuid();
            await _context.SewerLines.AddAsync(sewerLine);
            await _context.SaveChangesAsync();
            if (initialSewerBoxes.Any())
                sewerLine.InitialSewerBox = initialSewerBoxes.FirstOrDefault(x => x.Stage == ConstantHelpers.Stage.REAL);
            else
                sewerLine.InitialSewerBox.Stage = ConstantHelpers.Stage.REAL;
            if (finalSewerBoxes.Any())
                sewerLine.FinalSewerBox = finalSewerBoxes.FirstOrDefault(x => x.Stage == ConstantHelpers.Stage.REAL);
            else
                sewerLine.FinalSewerBox.Stage = ConstantHelpers.Stage.REAL;
            sewerLine.Stage = ConstantHelpers.Stage.REAL;
            sewerLine.Id = Guid.NewGuid();
            await _context.SewerLines.AddAsync(sewerLine);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.FULL_ACCESS)]
        [HttpPut("editar-estado/{id}")]
        public async Task<IActionResult> EditReviewed(Guid id, bool isReviewed)
        {
            var sewerLine = await _context.SewerLines.FindAsync(id);
            sewerLine.IsReviewed = isReviewed;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.FULL_ACCESS)]
        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, SewerLineViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var sewerLine = await _context.SewerLines
                .Include(x => x.InitialSewerBox)
                .ThenInclude(x => x.InitialSewerLines)
                .Include(x => x.InitialSewerBox)
                .ThenInclude(x => x.FinalSewerLines)
                .Include(x => x.FinalSewerBox)
                .ThenInclude(x => x.InitialSewerLines)
                .Include(x => x.FinalSewerBox)
                .ThenInclude(x => x.FinalSewerLines)
                .FirstOrDefaultAsync(x => x.Id == id);
            sewerLine.Address = model.Address;
            sewerLine.DrainageArea = model.DrainageArea;
            sewerLine.SewerGroupId = model.SewerGroupId;
            sewerLine.InitialSewerBox.Code = model.InitialSewerBoxCode;
            sewerLine.InitialSewerBox.SewerGroupId = model.SewerGroupId;
            sewerLine.InitialSewerBox.TerrainType = model.InitialSewerBoxTerrainType;
            sewerLine.InitialSewerBox.Type = model.InitialSewerBoxType;
            sewerLine.InitialSewerBox.Address = model.Address;
            sewerLine.InitialSewerBox.DrainageArea = model.InitialSewerDrainageArea;
            sewerLine.InitialSewerBox.Cover = model.WaterUpCover;
            sewerLine.InitialSewerBox.InputOutput = model.WaterUpOutput;
            sewerLine.InitialSewerBox.Bottom = model.WaterUpBottom;
            sewerLine.FinalSewerBox.Code = model.FinalSewerBoxCode;
            sewerLine.FinalSewerBox.SewerGroupId = model.SewerGroupId;
            sewerLine.FinalSewerBox.TerrainType = model.FinalSewerBoxTerrainType;
            sewerLine.FinalSewerBox.Type = model.FinalSewerBoxType;
            sewerLine.FinalSewerBox.Address = model.Address;
            sewerLine.FinalSewerBox.DrainageArea = model.FinalSewerDrainageArea;
            sewerLine.FinalSewerBox.Cover = model.DownstreamCover;
            sewerLine.FinalSewerBox.InputOutput = model.DownstreamInput;
            sewerLine.FinalSewerBox.Bottom = model.DownstreamBottom;
            sewerLine.HorizontalDistanceOnAxis = model.HorizontalDistanceOnAxis;
            sewerLine.NominalDiameter = model.NominalDiameter;
            sewerLine.PipelineType = model.PipelineType;
            sewerLine.TerrainType = model.TerrainType;
            sewerLine.HasFor47 = model.HasFor47;
            sewerLine.Calculate();
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.FULL_ACCESS)]
        [HttpPost("agregar-buzon/{id}")]
        public async Task<IActionResult> AddSewerBox(Guid id, AddSewerBoxViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var sewerLine = await _context.SewerLines
                .Include(x => x.InitialSewerBox)
                .Include(x => x.FinalSewerBox)
                .FirstOrDefaultAsync(x => x.Id == id);
            var newSewerBox = new SewerBox
            {
                SewerGroupId = sewerLine.SewerGroupId,
                Address = sewerLine.Address,
                DrainageArea = model.DrainageArea,
                Code = model.Code,
                Cover = model.Cover,
                InputOutput = model.InputOutput,
                Bottom = model.Bottom,
                TerrainType = model.TerrainType,
                Stage = sewerLine.Stage
            };
            var newSewerLine = new SewerLine
            {
                SewerGroupId = sewerLine.SewerGroupId,
                Address = sewerLine.Address,
                DrainageArea = sewerLine.DrainageArea,
                InitialSewerBox = newSewerBox,
                FinalSewerBox = sewerLine.FinalSewerBox,
                Stage = sewerLine.Stage,
                HorizontalDistanceOnAxis = model.HorizontalDistanceOnAxis,
                NominalDiameter = sewerLine.NominalDiameter,
                PipelineType = sewerLine.PipelineType,
                TerrainType = sewerLine.TerrainType,
                AddedLately = true
            };
            sewerLine.FinalSewerBox = newSewerBox;
            sewerLine.HorizontalDistanceOnAxis -= model.HorizontalDistanceOnAxis;
            sewerLine.Calculate();
            newSewerLine.Calculate();
            await _context.SewerBoxes.AddAsync(newSewerBox);
            await _context.SewerLines.AddAsync(newSewerLine);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.FULL_ACCESS)]
        [HttpDelete("anular-buzon/{id}")]
        public async Task<IActionResult> DeleteSewerBox(Guid id)
        {
            var sewerLine = await _context.SewerLines
                .Include(x => x.FinalSewerBox)
                .Include(x => x.InitialSewerBox)
                .Where(x => x.AddedLately && x.Id == id)
                .FirstOrDefaultAsync();
            if (sewerLine == null)
                return BadRequest($"El tramo con Id '{id}' no se encuentra o no es un buzón agregado.");
            var affectedSewerLine = await _context.SewerLines
                .Include(x => x.FinalSewerBox)
                .Include(x => x.InitialSewerBox)
                .Where(x => x.FinalSewerBoxId == sewerLine.InitialSewerBoxId)
                .FirstOrDefaultAsync();
            if (affectedSewerLine == null)
                return BadRequest($"No se encontró el tramo original para la restauración.");
            affectedSewerLine.FinalSewerBox = sewerLine.FinalSewerBox;
            affectedSewerLine.HorizontalDistanceOnAxis += sewerLine.HorizontalDistanceOnAxis;
            affectedSewerLine.Calculate();
            _context.SewerLines.Remove(sewerLine);
            await _context.SaveChangesAsync();
            _context.SewerBoxes.Remove(sewerLine.InitialSewerBox);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.FULL_ACCESS)]
        [HttpPost("for-47/{id}")]
        public async Task<IActionResult> For47(Guid id, For47ViewModel model)
        {
            if (model.ExcavationLengthPercentForNormal + model.ExcavationLengthPercentForSemirocous + model.ExcavationLengthPercentForRocky > 100)
                return BadRequest("La sumatoria de los porcentajes no puede ser mayor al 100%");
            if(model.ExcavationLengthPercentForNormal > 0 ||
                model.ExcavationLengthPercentForRocky > 0 ||
                model.ExcavationLengthPercentForSemirocous > 0)
            {
                var sewerLine = await _context.SewerLines
                    .Include(x => x.InitialSewerBox)
                    .Include(x => x.FinalSewerBox)
                    .FirstOrDefaultAsync(x => x.Id == id);
                if (sewerLine == null)
                    return BadRequest($"No se encontró el tramo con Id '{id}'.");
                sewerLine.HasFor47 = true;
                sewerLine.ExcavationLengthPercentForNormal = model.ExcavationLengthPercentForNormal / 100;
                sewerLine.ExcavationLengthPercentForSemirocous = model.ExcavationLengthPercentForSemirocous / 100;
                sewerLine.ExcavationLengthPercentForRocky = model.ExcavationLengthPercentForRocky / 100;
                sewerLine.Calculate();
                await _context.SaveChangesAsync();
            }
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.FULL_ACCESS)]
        [HttpPost("cambiar-direccion/{id}")]
        public async Task<IActionResult> ChangeDirection(Guid id)
        {
            var sewerLine = await _context.SewerLines.FindAsync(id);
            var tmpSewerBox = sewerLine.InitialSewerBoxId;
            sewerLine.InitialSewerBoxId = sewerLine.FinalSewerBoxId;
            sewerLine.FinalSewerBoxId = tmpSewerBox;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.FULL_ACCESS)]
        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var sewerLine = await _context.SewerLines.FindAsync(id);
            if (sewerLine == null)
                return BadRequest($"Consolidado de Línea de Alcantarillado con Id '{id}' no encontrado.");
            _context.SewerLines.Remove(sewerLine);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.PARTIAL_ACCESS)]
        [HttpGet("exportar")]
        public async Task Export()
        {
            using (var mem = new MemoryStream())
            {
                using(var workBook = new XLWorkbook())
                {
                    var workSheet = workBook.Worksheets.Add("Tramos");
                    workSheet.Cell("A1").Value = "Cuadrilla";
                    workSheet.Cell("B1").Value = "Dirección";
                    workSheet.Cell("C1").Value = "BZ (i)";
                    workSheet.Cell("D1").Value = "h BZ (i)";
                    workSheet.Cell("E1").Value = "dm BZ (i)";
                    workSheet.Cell("F1").Value = "BZ (j)";
                    workSheet.Cell("G1").Value = "h BZ (j)";
                    workSheet.Cell("H1").Value = "dm BZ (j)";

                    workSheet.Row(1).Style.Font.Bold = true;

                    var data = await _context.SewerLines
                        .Include(x => x.SewerGroup)
                        .Include(x => x.InitialSewerBox)
                        .Include(x => x.FinalSewerBox)
                        .Where(x => x.Stage == ConstantHelpers.Stage.CONTRACTUAL)
                        .ToListAsync();
                    var counter = 2d;
                    foreach(var item in data)
                    {
                        workSheet.Cell($"A{counter}").Value = item.SewerGroup.Code;
                        workSheet.Cell($"B{counter}").Value = item.Address;
                        workSheet.Cell($"C{counter}").Value = item.InitialSewerBox.Code;
                        workSheet.Cell($"D{counter}").Value = item.InitialSewerBox.Height;
                        workSheet.Cell($"E{counter}").Value = item.InitialSewerBox.InternalDiameter;
                        workSheet.Cell($"F{counter}").Value = item.FinalSewerBox.Code;
                        workSheet.Cell($"G{counter}").Value = item.FinalSewerBox.Height;
                        workSheet.Cell($"H{counter}").Value = item.FinalSewerBox.InternalDiameter;
                        ++counter;
                    }

                    workSheet.Columns(1, 8).AdjustToContents();
                    workBook.SaveAs(mem);
                }

                // Download file                
                const string text = "attachment;filename=\"Consolidado Alcantarillado.xlsx\"";
                Response.Headers["Content-Disposition"] = text;
                Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                mem.Position = 0;
                await mem.CopyToAsync(HttpContext.Response.Body);
            }
        }

        [HttpGet("descargar-plantilla")]
        public async Task DownloadTemplate()
        {
            using (var mem = new MemoryStream())
            {
                using (var workBook = new XLWorkbook())
                {
                    var workSheet = workBook.Worksheets.Add("Tramos");
                    workSheet.Cell("A1").Value = "Ubicación";

                    workSheet.Range("A1:B1").Merge();
                    workSheet.Range("A1:B1").Merge();

                    workSheet.Cell("A1").Value = "Cuadrilla";
                    workSheet.Cell("B1").Value = "Dirección";
                    workSheet.Cell("C1").Value = "BZ (i)";
                    workSheet.Cell("D1").Value = "h BZ (i)";
                    workSheet.Cell("E1").Value = "dm BZ (i)";
                    workSheet.Cell("F1").Value = "BZ (j)";
                    workSheet.Cell("G1").Value = "h BZ (j)";
                    workSheet.Cell("H1").Value = "dm BZ (j)";

                    workSheet.Row(1).Style.Font.Bold = true;

                    var data = await _context.SewerLines
                        .Include(x => x.SewerGroup)
                        .Include(x => x.InitialSewerBox)
                        .Include(x => x.FinalSewerBox)
                        .Where(x => x.Stage == ConstantHelpers.Stage.CONTRACTUAL)
                        .ToListAsync();
                    var counter = 2d;
                    foreach (var item in data)
                    {
                        //workSheet.Cell($"A{counter}").Value = item.SewerLine.SewerGroup.Code;
                        //workSheet.Cell($"B{counter}").Value = item.SewerLine.Address;
                        //workSheet.Cell($"C{counter}").Value = item.SewerLine.InitialSewerBoxCode;
                        //workSheet.Cell($"D{counter}").Value = item.InitialSewerBoxHeight;
                        //workSheet.Cell($"E{counter}").Value = item.InitialSewerBoxInternalDiameter;
                        //workSheet.Cell($"F{counter}").Value = item.SewerLine.FinalSewerBoxCode;
                        //workSheet.Cell($"G{counter}").Value = item.FinalSewerBoxHeight;
                        //workSheet.Cell($"H{counter}").Value = item.FinalSewerBoxInternalDiameter;
                        ++counter;
                    }

                    workSheet.Columns(1, 8).AdjustToContents();
                    workBook.SaveAs(mem);
                }

                // Download file                
                const string text = "attachment;filename=\"Consolidado Alcantarillado.xlsx\"";
                Response.Headers["Content-Disposition"] = text;
                Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                mem.Position = 0;
                await mem.CopyToAsync(HttpContext.Response.Body);
            }
        }

        [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.FULL_ACCESS)]
        [HttpPost("importar")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Import(IFormFile file)
        {
            using(var mem = new MemoryStream())
            {
                await file.CopyToAsync(mem);
                using (var workBook = new XLWorkbook(mem))
                {
                    var workSheet = workBook.Worksheet("PROYECTO");
                    var counter = 5;
                    var project = await _context.Projects.FirstOrDefaultAsync();
                    var systemPhases = new List<SystemPhase>();
                    var workFronts = new List<WorkFront>();
                    var sewerGroups = new List<SewerGroup>();
                    var qualificationZones = new List<QualificationZone>();
                    var sewerBoxes = new List<SewerBox>();
                    var sewerLines = new List<SewerLine>();
                    while (!workSheet.Cell($"A{counter}").IsEmpty())
                    {
                        var systemPhaseCode = workSheet.Cell($"A{counter}").GetString();
                        var systemPhase = (await _context.SystemPhases.FirstOrDefaultAsync(x => x.Code == systemPhaseCode))
                            ?? systemPhases.FirstOrDefault(x => x.Code == systemPhaseCode);
                        if(systemPhase == null)
                        {
                            systemPhase = new SystemPhase
                            {
                                Code = systemPhaseCode,
                                ProjectId = project.Id
                            };
                            systemPhases.Add(systemPhase);
                        }

                        var workFrontCode = workSheet.Cell($"B{counter}").GetString();
                        var workFront = (await _context.WorkFronts.FirstOrDefaultAsync(x => x.Code == workFrontCode))
                            ?? workFronts.FirstOrDefault(x => x.Code == workFrontCode);
                        if(workFront == null)
                        {
                            workFront = new WorkFront
                            {
                                Code = workFrontCode,
                                SystemPhase = systemPhase
                            };
                            workFronts.Add(workFront);
                        }

                        var sewerGroupCode = workSheet.Cell($"C{counter}").GetString();
                        var sewerGroup = (await _context.SewerGroups.FirstOrDefaultAsync(x => x.Code == sewerGroupCode))
                            ?? sewerGroups.FirstOrDefault(x => x.Code == sewerGroupCode);
                        if(sewerGroup == null)
                        {
                            sewerGroup = new SewerGroup
                            {
                                Code = sewerGroupCode,
                                WorkFront = workFront
                            };
                            sewerGroups.Add(sewerGroup);
                        }

                        var qualificationZoneCode = workSheet.Cell($"D{counter}").GetString();
                        var qualificationZone = (await _context.QualificationZones.FirstOrDefaultAsync(x => x.Code == qualificationZoneCode))
                            ?? qualificationZones.FirstOrDefault(x => x.Code == qualificationZoneCode);
                        if(qualificationZone == null)
                        {
                            qualificationZone = new QualificationZone
                            {
                                Code = qualificationZoneCode,
                                Name = workSheet.Cell($"E{counter}").GetString(),
                                ProjectId = project.Id
                            };
                            qualificationZones.Add(qualificationZone);
                        }

                        var initialSewerBoxCode = workSheet.Cell($"H{counter}").GetString();
                        var finalSewerBoxCode = workSheet.Cell($"R{counter}").GetString();

                        //var initialSewerBoxes = await _context.SewerBoxes
                        //    .Where(x => x.Code == initialSewerBoxCode && x.SewerGroupId == sewerGroup.Id)
                        //    .OrderBy(x => x.Stage)
                        //    .AsNoTracking().ToListAsync();
                        //var finalSewerBoxes = await _context.SewerBoxes
                        //    .Where(x => x.Code == finalSewerBoxCode && x.SewerGroupId == sewerGroup.Id)
                        //    .OrderBy(x => x.Stage)
                        //    .AsNoTracking().ToListAsync();
                        //if (!initialSewerBoxes.Any())
                        //    initialSewerBoxes = sewerBoxes.Where(x => x.Any(y => y.Code == initialSewerBoxCode && y.SewerGroupId == sewerGroup.Id)).FirstOrDefault();
                        //if (!finalSewerBoxes.Any())
                        //    finalSewerBoxes = sewerBoxes.Where(x => x.Any(y => y.Code == finalSewerBoxCode && y.SewerGroupId == sewerGroup.Id)).FirstOrDefault();
                        //initialSewerBoxes.OrderBy(x => x.Stage);
                        //finalSewerBoxes.OrderBy(x => x.Stage);

                        var sewerLine = new SewerLine();
                        sewerLine.Stage = ConstantHelpers.Stage.CONTRACTUAL;
                        sewerLine.Address = workSheet.Cell($"F{counter}").GetString();
                        sewerLine.SewerGroup = sewerGroup;
                        sewerLine.QualificationZone = qualificationZone;
                        // B (i)
                        //sewerLine.InitialSewerBox = initialSewerBoxes.Any()
                        //    ? initialSewerBoxes.FirstOrDefault(x => x.Stage == ConstantHelpers.Stage.CONTRACTUAL)
                        //    : new SewerBox();
                        var initialSewerBox = sewerBoxes.FirstOrDefault(x => x.Code == initialSewerBoxCode) ?? new SewerBox();
                        initialSewerBox.SewerGroup = sewerGroup;
                        initialSewerBox.Address = sewerLine.Address;
                        initialSewerBox.DrainageArea = workSheet.Cell($"G{counter}").GetString();
                        initialSewerBox.Code = workSheet.Cell($"H{counter}").GetString();
                        initialSewerBox.Cover = workSheet.Cell($"J{counter}").GetDouble();
                        initialSewerBox.InputOutput = workSheet.Cell($"K{counter}").IsEmpty() ? 0 : (float)workSheet.Cell($"J{counter}").GetDouble();
                        initialSewerBox.Bottom = workSheet.Cell($"L{counter}").GetDouble();
                        initialSewerBox.TerrainType = ConstantHelpers.Terrain.Type.VALUES.FirstOrDefault(x => x.Value == workSheet.Cell($"M{counter}").GetString()).Key;
                        if (initialSewerBox.InitialSewerLines == null)
                            initialSewerBox.InitialSewerLines = new List<SewerLine>();
                        initialSewerBox.InitialSewerLines.Add(sewerLine);
                        sewerLine.InitialSewerBox = initialSewerBox;
                        sewerBoxes.Add(initialSewerBox);
                        // B (j)
                        //sewerLine.FinalSewerBox = finalSewerBoxes.Any()
                        //    ? finalSewerBoxes.FirstOrDefault(x => x.Stage == ConstantHelpers.Stage.CONTRACTUAL)
                        //    : new SewerBox();
                        var finalSewerBox = sewerBoxes.FirstOrDefault(x => x.Code == finalSewerBoxCode) ?? new SewerBox();
                        finalSewerBox.SewerGroup = sewerGroup;
                        finalSewerBox.Address = sewerLine.Address;
                        finalSewerBox.DrainageArea = workSheet.Cell($"Q{counter}").GetString();
                        finalSewerBox.Code = workSheet.Cell($"R{counter}").GetString();
                        finalSewerBox.Cover = workSheet.Cell($"T{counter}").GetDouble();
                        finalSewerBox.InputOutput = workSheet.Cell($"U{counter}").IsEmpty() ? 0 : (float)workSheet.Cell($"T{counter}").GetDouble();
                        finalSewerBox.Bottom= (float)workSheet.Cell($"V{counter}").GetDouble();
                        finalSewerBox.TerrainType = ConstantHelpers.Terrain.Type.VALUES.FirstOrDefault(x => x.Value == workSheet.Cell($"W{counter}").GetString()).Key;
                        if (finalSewerBox.FinalSewerLines == null)
                            finalSewerBox.FinalSewerLines = new List<SewerLine>();
                        finalSewerBox.FinalSewerLines.Add(sewerLine);
                        sewerLine.FinalSewerBox = finalSewerBox;
                        sewerBoxes.Add(finalSewerBox);
                        // Line
                        sewerLine.DrainageArea = workSheet.Cell($"AA{counter}").GetString();
                        sewerLine.HorizontalDistanceOnAxis = (float)workSheet.Cell($"AC{counter}").GetDouble();
                        sewerLine.NominalDiameter = (float)workSheet.Cell($"AF{counter}").GetDouble();
                        sewerLine.PipelineType = ConstantHelpers.Pipeline.Type.VALUES.FirstOrDefault(x => x.Value == workSheet.Cell($"AG{counter}").GetString()).Key;
                        sewerLine.TerrainType = ConstantHelpers.Terrain.Type.VALUES.FirstOrDefault(x => x.Value == workSheet.Cell($"AI{counter}").GetString()).Key;
                        sewerLine.Calculate();
                        sewerLines.Add(sewerLine);
                        ++counter;
                    }
                    await _context.SystemPhases.AddRangeAsync(systemPhases);
                    await _context.WorkFronts.AddRangeAsync(workFronts);
                    await _context.SewerGroups.AddRangeAsync(sewerGroups);
                    await _context.QualificationZones.AddRangeAsync(qualificationZones);
                    await _context.SewerBoxes.AddRangeAsync(sewerBoxes);
                    await _context.SewerLines.AddRangeAsync(sewerLines);
                    await _context.SaveChangesAsync();
                    sewerBoxes.ForEach(x => {
                        x.Stage = ConstantHelpers.Stage.STAKING;
                        x.Id = Guid.NewGuid();
                    });
                    sewerLines.ForEach(x => {
                        x.Stage = ConstantHelpers.Stage.STAKING;
                        x.Id = Guid.NewGuid();
                    });
                    await _context.SewerBoxes.AddRangeAsync(sewerBoxes);
                    await _context.SewerLines.AddRangeAsync(sewerLines);
                    await _context.SaveChangesAsync();
                    sewerBoxes.ForEach(x => {
                        x.Stage = ConstantHelpers.Stage.REAL;
                        x.Id = Guid.NewGuid();
                    });
                    sewerLines.ForEach(x => {
                        x.Stage = ConstantHelpers.Stage.REAL;
                        x.Id = Guid.NewGuid();
                    });
                    await _context.SewerBoxes.AddRangeAsync(sewerBoxes);
                    await _context.SewerLines.AddRangeAsync(sewerLines);
                    await _context.SaveChangesAsync();
                }
                mem.Close();
            }
            return Ok();
        }*/
    }
}