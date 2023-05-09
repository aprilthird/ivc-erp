using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.UspModels.TechnicalOffice;
using IVC.PE.WEB.Areas.Admin.ViewModels.SewerGroupViewModels;
using IVC.PE.WEB.Areas.Production.ViewModels.PdpViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.SewerBoxViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.SewerManifoldViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IVC.PE.WEB.Areas.TechnicalOffice.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.TECHNICAL_OFFICE)]
    [Route("oficina-tecnica/colectores-descarga")]
    public class ManifoldNetworkSummaryController : BaseController
    {
        public ManifoldNetworkSummaryController(IvcDbContext context,
            ILogger<ManifoldNetworkSummaryController> logger)
            : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("proyecto")]
        public async Task<IActionResult> GetAllForProject()
        {
            SqlParameter processTypeParam = new SqlParameter("@ProcessType", (object)ConstantHelpers.Sewer.Manifolds.Process.PROJECT);
            SqlParameter projectIdParam = new SqlParameter("@ProjectId", GetProjectId());

            var manifolds = await _context.Set<UspSewerManifold>().FromSqlRaw("execute TechnicalOffice_uspSewerManifolds @ProcessType, @ProjectId"
                , processTypeParam, projectIdParam)
                .IgnoreQueryFilters()
                .ToListAsync();

            return Ok(manifolds);
        }

        [HttpGet("replanteo")]
        public async Task<IActionResult> GetAllForReview()
        {
            SqlParameter processTypeParam = new SqlParameter("@ProcessType", ConstantHelpers.Sewer.Manifolds.Process.REVIEW);
            SqlParameter projectIdParam = new SqlParameter("@ProjectId", GetProjectId());

            var manifolds = await _context.Set<UspSewerManifoldReview>().FromSqlRaw("execute TechnicalOffice_uspSewerManifoldsReview @ProcessType, @ProjectId",
                processTypeParam, projectIdParam)
                .IgnoreQueryFilters()
                .ToListAsync();

            return Ok(manifolds);
        }

        [HttpGet("ejecucion")]
        public async Task<IActionResult> GetAllForExecution()
        {
            SqlParameter projectIdParam = new SqlParameter("@ProjectId", GetProjectId());

            var manifolds = await _context.Set<UspSewerManifoldExecution>().FromSqlRaw("execute TechnicalOffice_uspSewerManifoldsExecution @ProjectId",
                projectIdParam)
                .IgnoreQueryFilters()
                .ToListAsync();

            return Ok(manifolds);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid? id = null)
        {
            if (id == null)
                return Ok("Debe seleccionar un tramo.");

            //var manifoldReview = await _context.SewerManifoldReferences
            //    .Include(x => x.SewerManifoldReview)
            //    .Where(x => x.SewerManifoldExecutionId == id.Value)
            //    .Select(x => x.SewerManifoldReviewId)
            //    .FirstOrDefaultAsync();

            var manifold = await _context.SewerManifolds
                .Include(x => x.ProductionDailyPart)
                .Include(x => x.ProductionDailyPart.SewerGroup)
                .Include(x => x.SewerBoxStart)
                .Include(x => x.SewerBoxEnd)
                .Where(x => x.Id == id)
                .Select(x => new SewerManifoldViewModel
                {
                    Id = id.Value,
                    Name = x.Name,
                    ProductionDailyPartId = x.ProductionDailyPartId.Value,
                    ProductionDailyPart = new PdpViewModel
                    {
                        ProjectFormulaId = x.ProductionDailyPart.ProjectFormulaId,
                        SewerGroupId = x.ProductionDailyPart.SewerGroupId,
                        SewerGroup = new SewerGroupViewModel
                        {
                            Code = x.ProductionDailyPart.SewerGroup.Code,
                            WorkFrontHeadId = x.ProductionDailyPart.SewerGroup.WorkFrontHeadId
                        }
                    },
                    SewerBoxStartId = x.SewerBoxStartId.Value,
                    SewerBoxStart = new SewerBoxViewModel
                    {
                        Code = x.SewerBoxStart.Code,
                        CoverLevel = x.SewerBoxStart.CoverLevel,
                        ArrivalLevel = x.SewerBoxStart.ArrivalLevel,
                        BottomLevel = x.SewerBoxStart.BottomLevel,
                        TerrainType = x.SewerBoxStart.TerrainType,
                        SewerBoxType = x.SewerBoxStart.SewerBoxType,
                        Thickness = x.SewerBoxStart.Thickness
                    },
                    SewerBoxEndId = x.SewerBoxEndId.Value,
                    SewerBoxEnd = new SewerBoxViewModel
                    {
                        Code = x.SewerBoxEnd.Code,
                        CoverLevel = x.SewerBoxEnd.CoverLevel,
                        ArrivalLevel = x.SewerBoxEnd.ArrivalLevel,
                        BottomLevel = x.SewerBoxEnd.BottomLevel,
                        TerrainType = x.SewerBoxEnd.TerrainType,
                        SewerBoxType = x.SewerBoxEnd.SewerBoxType,
                        Thickness = x.SewerBoxEnd.Thickness
                    },
                    Address = x.Address,
                    PipeDiameter = x.PipelineDiameter,
                    PipeType = x.PipelineType,
                    LengthBetweenHAxles = x.LengthBetweenHAxles,
                    TerrainType = x.TerrainType,
                    LengthOfDigging = x.LengthOfDigging,
                    Pavement2In = x.Pavement2In.ToString(),
                    Pavement3In = x.Pavement3In.ToString(),
                    Pavement3InMixed = x.Pavement3InMixed.ToString(),
                    PavementWidth = x.PavementWidth.ToString()
                }).FirstOrDefaultAsync();

            return Ok(manifold);
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, SewerManifoldViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var sm = await _context.SewerManifolds.FirstOrDefaultAsync(x => x.Id == id);

            var sb_start = await _context.SewerBoxes.FirstOrDefaultAsync(x => x.Id == sm.SewerBoxStartId);
            sb_start.CoverLevel = model.SewerBoxStart.CoverLevel;
            sb_start.ArrivalLevel = model.SewerBoxStart.ArrivalLevel;
            sb_start.BottomLevel = model.SewerBoxStart.BottomLevel;
            sb_start.TerrainType = model.SewerBoxStart.TerrainType;
            sb_start.SewerBoxType = model.SewerBoxStart.SewerBoxType;
            sb_start.Thickness = model.SewerBoxStart.Thickness;
            sb_start.Height = sb_start.CoverLevel - sb_start.BottomLevel;
            sb_start.Diameter = sb_start.Thickness == 0.40 ? 1.50 : 1.80;

            var sb_end = await _context.SewerBoxes.FirstOrDefaultAsync(x => x.Id == sm.SewerBoxEndId);
            sb_end.CoverLevel = model.SewerBoxEnd.CoverLevel;
            sb_end.ArrivalLevel = model.SewerBoxEnd.ArrivalLevel;
            sb_end.BottomLevel = model.SewerBoxEnd.BottomLevel;
            sb_end.TerrainType = model.SewerBoxEnd.TerrainType;
            sb_end.SewerBoxType = model.SewerBoxEnd.SewerBoxType;
            sb_end.Thickness = model.SewerBoxEnd.Thickness;
            sb_end.Height = sb_end.CoverLevel - sb_end.BottomLevel;
            sb_end.Diameter = sb_end.Thickness == 0.40 ? 1.50 : 1.80;

            sm.Name = model.Name;
            sm.PipelineDiameter = model.PipeDiameter;
            sm.PipelineType = model.PipeType;
            sm.LengthBetweenHAxles = model.LengthBetweenHAxles;
            sm.TerrainType = model.TerrainType;
            sm.DitchHeight = Math.Round(((sb_start.CoverLevel - sb_start.BottomLevel) + (sb_end.CoverLevel - sb_end.ArrivalLevel)) / 2.0,2);
            sm.DitchLevelPercent = Math.Round((sb_start.BottomLevel - sb_end.ArrivalLevel) * 1000.0 / sm.LengthBetweenHAxles,2);
            sm.PipelineClass = sm.DitchHeight <= 5.0 ? ConstantHelpers.Pipeline.Class.SN4 : ConstantHelpers.Pipeline.Class.SN8;
            sm.LengthBetweenIAxles = Math.Round(Math.Pow(Math.Pow(sb_start.BottomLevel - sb_end.BottomLevel, 2) + Math.Pow(sm.LengthBetweenHAxles, 2),0.5),2);


            sm.Pavement2In = model.Pavement2In.ToDoubleString();
            sm.Pavement3In = model.Pavement3In.ToDoubleString();
            sm.Pavement3InMixed = model.Pavement3InMixed.ToDoubleString();
            sm.PavementWidth = model.PavementWidth.ToDoubleString();

            await _context.SaveChangesAsync();

            return Ok();
        }





        [HttpGet("programaciones/{id}")]
        public async Task<IActionResult> GetForSewerGroupSchedule(Guid? id = null)
        {
            if (id == null)
                return Ok("Debe seleccionar un tramo.");

            var manifold = await _context.SewerManifolds
                .Include(x => x.SewerBoxStart)
                .Include(x => x.SewerBoxEnd)
                .FirstOrDefaultAsync(x => x.Id == id);

            var manifoldSchedule = new SewerManifoldSewerGroupScheduleViewModel
            {
                LengthOfDigging = manifold.LengthOfDigging,
                TerrainType = manifold.TerrainType,
                DitchHeight = manifold.DitchHeight,
                LengthOfPipeInstalled = manifold.LengthOfPipelineInstalled,
                NumberOfLayers = CalculateNumberOfLayers(manifold.DitchHeight, manifold.PipelineDiameter),
                SewerBoxStartHeight = manifold.SewerBoxStart.Height,
                SewerBoxStartTerrainType = manifold.SewerBoxStart.TerrainType,
                SewerBoxEndHeight = manifold.SewerBoxEnd.Height,
                SewerBoxEndTerrainType = manifold.SewerBoxEnd.TerrainType
            };

            return Ok(manifoldSchedule);
        }

        private double CalculateNumberOfLayers(double ditchHeight, double pipelineDiameter)
        {
            var result = (ditchHeight - (pipelineDiameter / 1000.0) - 0.3 - 0.2) / 0.3;
            return Math.Floor(result);
        }

        [HttpGet("ejecucion/actualizar")]
        public async Task<IActionResult> Get()
        {
            var smExecution = await _context.SewerManifolds
                .Where(x => x.ProcessType == ConstantHelpers.Sewer.Manifolds.Process.EXECUTION)
                .ToListAsync();

            var smReferences = await _context.SewerManifoldReferences
                .Include(x => x.SewerManifoldReview)
                .ToListAsync();

            foreach (var sm in smExecution)
            {
                var smReview = smReferences.FirstOrDefault(x => x.SewerManifoldExecutionId == sm.Id);

                sm.Pavement2In = smReview.SewerManifoldReview.Pavement2In;
                sm.Pavement3In = smReview.SewerManifoldReview.Pavement3In;
                sm.Pavement3InMixed = smReview.SewerManifoldReview.Pavement3InMixed;
                sm.PavementWidth = smReview.SewerManifoldReview.PavementWidth;

                if (!sm.HasFor01)
                {
                    sm.DitchHeight = smReview.SewerManifoldReview.DitchHeight;
                    sm.DitchLevelPercent = smReview.SewerManifoldReview.DitchLevelPercent;
                    sm.LengthBetweenHAxles = smReview.SewerManifoldReview.LengthBetweenHAxles;
                    sm.LengthBetweenIAxles = smReview.SewerManifoldReview.LengthBetweenIAxles;
                    sm.LengthOfPipelineInstalled = smReview.SewerManifoldReview.LengthOfPipelineInstalled;
                    sm.LengthOfDigging = smReview.SewerManifoldReview.LengthOfDigging;
                }
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("formato-excel")]
        public async Task<IActionResult> GenerateExcelFormat()
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add("PROYECTO");

                ws.Cell($"E1").Value = "Ubicación";
                ws.Range("E1:E2").Merge();
                ws.Cell($"E3").Value = "Dirección";

                ws.Cell($"F1").Value = "BZ (i) Aguas Arriba";
                ws.Range("F1:N2").Merge();
                ws.Cell($"F3").Value = "Nº";
                ws.Cell($"G3").Value = "Cota Tapa";
                ws.Cell($"H3").Value = "Cota Llegada";
                ws.Cell($"I3").Value = "Cota Fondo";
                ws.Cell($"J3").Value = "h BZ";
                ws.Cell($"K3").Value = "TT";
                ws.Cell($"L3").Value = "Tipo BZ";
                ws.Cell($"M3").Value = "Diámetro";
                ws.Cell($"N3").Value = "Espesor";

                ws.Cell($"O1").Value = "BZ (j) Aguas Arriba";
                ws.Range("O1:W2").Merge();
                ws.Cell($"O3").Value = "Nº";
                ws.Cell($"P3").Value = "Cota Tapa";
                ws.Cell($"Q3").Value = "Cota Llegada";
                ws.Cell($"R3").Value = "Cota Fondo";
                ws.Cell($"S3").Value = "h BZ";
                ws.Cell($"T3").Value = "TT";
                ws.Cell($"U3").Value = "Tipo BZ";
                ws.Cell($"V3").Value = "Diámetro";
                ws.Cell($"W3").Value = "Espesor";

                ws.Cell($"X1").Value = "Tramos";
                ws.Range("X1:AH2").Merge();
                ws.Cell($"X3").Value = "Nombre";
                ws.Cell($"Y3").Value = "h Zanja";
                ws.Cell($"Z3").Value = "%";
                ws.Cell($"AA3").Value = "DN (mm)";
                ws.Cell($"AB3").Value = "Tipo Tubería";
                ws.Cell($"AC3").Value = "Clase";
                ws.Cell($"AD3").Value = "Long. entre Ejes H";
                ws.Cell($"AE3").Value = "Long. entre Ejes I";
                ws.Cell($"AF3").Value = "Long. Tubería Instalada";
                ws.Cell($"AG3").Value = "TT";
                ws.Cell($"AH3").Value = "Long. Excav.";

                ws.Cell($"AI1").Value = "Tipos de Pavimento";
                ws.Range("AI1:AL2").Merge();
                ws.Cell($"AI3").Value = "Asfalto 2'' (m2)";
                ws.Cell($"AJ3").Value = "Asfalto 3'' (m2)";
                ws.Cell($"AK3").Value = "Asfalto Mixto 3'' (m2)";
                ws.Cell($"AL3").Value = "Ancho";

                ws.Range("E1:AL2").Style.Fill.SetBackgroundColor(XLColor.AirForceBlue);
                ws.Range("E3:AL3").Style.Fill.SetBackgroundColor(XLColor.AliceBlue);

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ColectorDescargaFormato.xlsx");
                }
            }
        }
    }
}