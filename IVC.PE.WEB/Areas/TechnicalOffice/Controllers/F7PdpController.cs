using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Production;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.ENTITIES.UspModels.Production;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectFormulaViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.SewerGroupViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.UserViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontHeadViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.F7PdpViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.FoldingF7ViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.SewerBoxViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.SewerManifoldViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.ShedulerF7ViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.TECHNICAL_OFFICE)]
    [Route("oficina-tecnica/f7-pdp")]
    public class F7PdpController : BaseController
    {
        public F7PdpController(IvcDbContext context,
            ILogger<F7PdpController> logger)
            : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? sewerGroupId = null, Guid? workFrontHeadId = null, string status = null)
        {
            //SqlParameter projectParam = new SqlParameter("@ProjectId", GetProjectId());

            //var query = await _context.Set<UspProductionDailyPart>().FromSqlRaw("execute Production_uspProductionDailyParts @ProjectId"
            //    , projectParam)
            //    .IgnoreQueryFilters()
            //    .ToListAsync();

            //var pId = GetProjectId();

            var pdps = _context.ProductionDailyParts
                .Include(x => x.ProjectFormula)
                .Include(x => x.WorkFrontHead)
                .Include(x => x.WorkFrontHead.User)
                .Include(x => x.WorkFront)
                .Include(x => x.SewerGroup)
                .Where(x => x.ProjectId == GetProjectId());

            var sewerManifoldRefs = _context.SewerManifoldReferences
                .Include(y => y.SewerManifoldReview)
                .Include(y => y.SewerManifoldExecution)
                .Include(y => y.SewerManifoldReview.SewerBoxStart)
                .Include(y => y.SewerManifoldReview.SewerBoxEnd)
                .ToList();

            if (sewerGroupId.HasValue)
                pdps = pdps.Where(x => x.SewerGroupId == sewerGroupId);

            if (workFrontHeadId.HasValue)
                pdps = pdps.Where(x => x.WorkFrontHeadId == workFrontHeadId);

            if (status != "Todos")
                pdps = pdps.Where(x => x.Status == status);



            //Cuando se arregle las referencias de PDP en la DB
            //var sewerManifoldRefs = _context.SewerManifoldReferences
            //    .Include(y => y.SewerManifoldReview)
            //    .ToList();

            var query = new List<F7PdpViewModel>();

            foreach (var pdp in pdps)
            {
                var smReview = sewerManifoldRefs
                    .Where(x => x.SewerManifoldExecution.ProductionDailyPartId == pdp.Id)
                    .Select(x => x.SewerManifoldReview)
                    .FirstOrDefault();

                var sewerManifoldRefsItem = await _context.SewerManifoldReferences
                .Include(y => y.SewerManifoldReview)
                .Include(y => y.SewerManifoldExecution)
                .FirstOrDefaultAsync(x => x.SewerManifoldExecution.ProductionDailyPartId == pdp.Id);

                decimal maxExcavated = (decimal)sewerManifoldRefsItem.SewerManifoldReview.LengthOfDigging - (decimal)pdp.ExcavatedLength;
                decimal maxInstalled = (decimal)sewerManifoldRefsItem.SewerManifoldReview.LengthOfPipelineInstalled - (decimal)pdp.InstalledLength;
                decimal maxRefilled = (decimal)pdp.FillLength - (decimal)pdp.RefilledLength;
                decimal maxGranularBase = (decimal)sewerManifoldRefsItem.SewerManifoldReview.LengthOfDigging - (decimal)pdp.GranularBaseLength;
                /*
                var FillingFormula = smReview.DitchHeight - (smReview.PipelineDiameter / 1000) - 0.30 - 0.20;
                var FillLengthFormula = smReview.LengthOfDigging * (int)(FillingFormula / 0.3);
                var TheoreticalLayerFormula = (int)(FillingFormula / 0.3);
                pdp.Filling = Math.Round(FillingFormula,2);
                pdp.FillLength = Math.Round(FillLengthFormula,2);
                pdp.TheoreticalLayer = TheoreticalLayerFormula;
                await _context.SaveChangesAsync();
                */

                //Cuando se arregle las referencias de PDP en la DB
                //var smReview = sewerManifoldRefs
                //    .Where(x => x.ProductionDailyPartId == pdp.Id)
                //    .Select(x => x.SewerManifoldReview)
                //    .FirstOrDefault();

                query.Add(new F7PdpViewModel
                {
                    Id = pdp.Id,
                    ProjectFormula = new ProjectFormulaViewModel
                    {
                        Code = pdp.ProjectFormula.Code
                    },
                    ReportDate = pdp.ReportDate.ToDateString(),
                    WorkFrontHead = new WorkFrontHeadViewModel
                    {
                        User = new UserViewModel
                        {
                            AuxFullName = pdp.WorkFrontHead.User.FullName
                        }
                    },
                    SewerGroup = new SewerGroupViewModel
                    {
                        Code = pdp.SewerGroup.Code
                    },
                    SewerManifold = new SewerManifoldViewModel
                    {
                        Name = smReview.Name,
                        LengthOfDigging = smReview.LengthOfDigging,
                        LengthOfPipeInstalled = smReview.LengthOfPipelineInstalled,
                        DitchHeight = smReview.DitchHeight,
                        SewerBoxStart = new SewerBoxViewModel
                        {
                            Code = smReview.SewerBoxStart.Code,
                            TerrainType = smReview.SewerBoxStart.TerrainType,
                            Height = smReview.SewerBoxStart.Height
                        },
                        SewerBoxEnd = new SewerBoxViewModel
                        {
                            Code = smReview.SewerBoxEnd.Code,
                            TerrainType = smReview.SewerBoxEnd.TerrainType,
                            Height = (smReview.SewerBoxEnd.CoverLevel - smReview.SewerBoxEnd.ArrivalLevel)
                        }
                    },
                    Filling = pdp.Filling,
                    TheoreticalLayer = pdp.TheoreticalLayer,
                    FillLength = pdp.FillLength,
                    ExcavatedLength = pdp.ExcavatedLength,
                    InstalledLength = pdp.InstalledLength,
                    RefilledLength = pdp.RefilledLength,
                    GranularBaseLength = pdp.GranularBaseLength,
                    ExcavatedLengthToExecute = maxExcavated.ToString(),
                    InstalledLengthToExecute = maxInstalled.ToString(),
                    RefilledLengthToExecute = maxRefilled.ToString(),
                    GranularBaseLengthToExecute = maxGranularBase.ToString(),
                    Excavation = pdp.Excavation,
                    Installation = pdp.Installation,
                    Filled = pdp.Filled,
                    Status = pdp.Status
                });
            }
            return Ok(query);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var smReview = await _context.SewerManifoldReferences
                .Include(x => x.SewerManifoldExecution)
                .Where(x => x.SewerManifoldExecution.ProductionDailyPartId == id)
                .Select(x => x.SewerManifoldReview)
                .FirstOrDefaultAsync();

            var FillingFormula = smReview.DitchHeight - (smReview.PipelineDiameter / 1000) - 0.30 - 0.25;

            var pdp = await _context.ProductionDailyParts
                .Where(x => x.Id == id)
                .Select(x => new F7PdpViewModel
                {
                    Id = x.Id,
                    ProjectFormulaId = x.ProjectFormulaId,
                    ProjectFormula = new ProjectFormulaViewModel
                    {
                        Code = x.ProjectFormula.Code
                    },
                    ReportDate = x.ReportDate.ToDateString(),
                    SewerGroupId = x.SewerGroupId,
                    SewerGroup = new SewerGroupViewModel
                    {
                        Code = x.SewerGroup.Code
                    },
                    SewerManifoldId = smReview.Id,
                    WorkFrontHeadId = x.WorkFrontHeadId,
                    WorkFrontHead = new WorkFrontHeadViewModel
                    {
                        User = new UserViewModel
                        {
                            AuxFullName = x.WorkFrontHead.User.FullName
                        }
                    },
                    WorkFrontId = x.WorkFrontId,
                    SewerManifold = new SewerManifoldViewModel
                    {
                        Name = smReview.Name,
                        LengthOfDigging = smReview.LengthOfDigging,
                        LengthOfPipeInstalled = smReview.LengthOfPipelineInstalled,
                        DitchHeight = smReview.DitchHeight
                    },
                    Filling = x.Filling,
                    TheoreticalLayer = x.TheoreticalLayer,
                    FillLength = x.FillLength,
                    ExcavatedLength = Math.Round(x.ExcavatedLength,2),
                    InstalledLength = Math.Round(x.InstalledLength,2),
                    RefilledLength = Math.Round(x.RefilledLength,2),
                    GranularBaseLength = Math.Round(x.GranularBaseLength,2),
                    Excavation = x.Excavation,
                    Installation = x.Installation,
                    Filled = x.Filled,
                    Status = x.Status
                }).FirstOrDefaultAsync();



            return Ok(pdp);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(F7PdpViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var formula = await _context.ProjectFormulas
                .FirstOrDefaultAsync(x => x.Code == "F7");

            var smReview = await _context.SewerManifolds
                .FirstOrDefaultAsync(x => x.Id == model.SewerManifoldId);

            var FillingFormula = smReview.DitchHeight - (smReview.PipelineDiameter / 1000) - 0.30 - 0.25;
            var TheoreticalLayerFormula = Math.Round((FillingFormula / 0.3), MidpointRounding.AwayFromZero);
            var FillLengthFormula = smReview.LengthOfDigging * TheoreticalLayerFormula;

            var pdp = new ProductionDailyPart
            {
                ProjectFormulaId = formula.Id,
                SewerGroupId = model.SewerGroupId,
                WorkFrontHeadId = model.WorkFrontHeadId,
                WorkFrontId = model.WorkFrontId,
                ProjectId = GetProjectId(),
                Status = "En Ejecución",
                Filling = Math.Round(FillingFormula, 2),
                TheoreticalLayer = TheoreticalLayerFormula,
                FillLength = Math.Round(FillLengthFormula, 2),
            };

            var manifoldDb = await _context.SewerManifolds
                .Include(x => x.SewerBoxStart)
                .Include(x => x.SewerBoxEnd)
                .FirstOrDefaultAsync(x => x.Id == model.SewerManifoldId);

            var sbStart = new SewerBox
            {
                ProjectId = manifoldDb.SewerBoxStart.ProjectId,
                Code = manifoldDb.SewerBoxStart.Code,
                CoverLevel = manifoldDb.SewerBoxStart.CoverLevel,
                ArrivalLevel = manifoldDb.SewerBoxStart.ArrivalLevel,
                BottomLevel = manifoldDb.SewerBoxStart.BottomLevel,
                Height = manifoldDb.SewerBoxStart.Height,
                TerrainType = manifoldDb.SewerBoxStart.TerrainType,
                SewerBoxType = manifoldDb.SewerBoxStart.SewerBoxType,
                Diameter = manifoldDb.SewerBoxStart.Diameter,
                Thickness = manifoldDb.SewerBoxStart.Thickness,
                ProcessType = ConstantHelpers.Sewer.Manifolds.Process.EXECUTION,
                SewerOrderNumber = manifoldDb.SewerBoxStart.SewerOrderNumber
            };

            var sbEnd = new SewerBox
            {
                ProjectId = manifoldDb.SewerBoxEnd.ProjectId,
                Code = manifoldDb.SewerBoxEnd.Code,
                CoverLevel = manifoldDb.SewerBoxEnd.CoverLevel,
                ArrivalLevel = manifoldDb.SewerBoxEnd.ArrivalLevel,
                BottomLevel = manifoldDb.SewerBoxEnd.BottomLevel,
                Height = manifoldDb.SewerBoxEnd.Height,
                TerrainType = manifoldDb.SewerBoxEnd.TerrainType,
                SewerBoxType = manifoldDb.SewerBoxEnd.SewerBoxType,
                Diameter = manifoldDb.SewerBoxEnd.Diameter,
                Thickness = manifoldDb.SewerBoxEnd.Thickness,
                ProcessType = ConstantHelpers.Sewer.Manifolds.Process.EXECUTION,
                SewerOrderNumber = manifoldDb.SewerBoxEnd.SewerOrderNumber
            };

            var manifold = new SewerManifold
            {
                ProjectId = manifoldDb.ProjectId,
                ProductionDailyPart = pdp,
                Address = manifoldDb.Address,
                SewerBoxStart = sbStart,
                SewerBoxEnd = sbEnd,
                Name = manifoldDb.Name,
                PipelineDiameter = manifoldDb.PipelineDiameter,
                PipelineType = manifoldDb.PipelineType,
                PipelineClass = manifoldDb.PipelineClass,
                LengthOfPipelineInstalled = manifoldDb.LengthOfPipelineInstalled,
                TerrainType = manifoldDb.TerrainType,
                LengthOfDigging = manifoldDb.LengthOfDigging,
                ProcessType = ConstantHelpers.Sewer.Manifolds.Process.EXECUTION,
                DitchHeight = manifoldDb.DitchHeight,
                DitchLevelPercent = manifoldDb.DitchLevelPercent,
                LengthBetweenHAxles = manifoldDb.LengthBetweenHAxles,
                LengthBetweenIAxles = manifoldDb.LengthBetweenIAxles,
                Pavement2In = manifoldDb.Pavement2In,
                Pavement3In = manifoldDb.Pavement3In,
                Pavement3InMixed = manifoldDb.Pavement3InMixed,
                PavementWidth = manifoldDb.PavementWidth
            };

            var reference = new SewerManifoldReference
            {
                ProductionDailyPart = pdp,
                SewerManifoldExecution = manifold,
                SewerManifoldReviewId = model.SewerManifoldId
            };

            await _context.ProductionDailyParts.AddAsync(pdp);
            await _context.SewerBoxes.AddAsync(sbStart);
            await _context.SewerBoxes.AddAsync(sbEnd);
            await _context.SewerManifolds.AddAsync(manifold);
            await _context.SewerManifoldReferences.AddAsync(reference);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, F7PdpViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pdp = await _context.ProductionDailyParts
                .FirstOrDefaultAsync(x => x.Id == id);

            pdp.WorkFrontHeadId = model.WorkFrontHeadId;
            pdp.WorkFrontId = model.WorkFrontId;
            pdp.SewerGroupId = model.SewerGroupId;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var pdp = await _context.ProductionDailyParts
                .FirstOrDefaultAsync(x => x.Id == id);

            var manifold = await _context.SewerManifolds
                .FirstOrDefaultAsync(x => x.ProductionDailyPartId == pdp.Id);

            var sbStart = await _context.SewerBoxes
                .FirstOrDefaultAsync(x => x.Id == manifold.SewerBoxStartId);

            var sbEnd = await _context.SewerBoxes
                .FirstOrDefaultAsync(x => x.Id == manifold.SewerBoxEndId);

            var sewerManifoldRefs = await _context.SewerManifoldReferences
                .Include(x => x.SewerManifoldReview)
                .FirstOrDefaultAsync(x => x.SewerManifoldExecutionId == manifold.Id);

            _context.SewerBoxes.Remove(sbStart);
            _context.SewerBoxes.Remove(sbEnd);
            _context.SewerManifoldReferences.Remove(sewerManifoldRefs);
            _context.SewerManifolds.Remove(manifold);
            _context.ProductionDailyParts.Remove(pdp);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("actualizar-todo")]
        public async Task<IActionResult> Update()
        {
            var pdps = await _context.ProductionDailyParts
                .Include(x => x.ProjectFormula)
                .Include(x => x.WorkFrontHead)
                .Include(x => x.WorkFrontHead.User)
                .Include(x => x.WorkFront)
                .Include(x => x.SewerGroup)
                .Where(x => x.ProjectId == GetProjectId()).ToListAsync();

            var sewerManifoldRefs = await _context.SewerManifoldReferences
                .Include(y => y.SewerManifoldReview)
                .Include(y => y.SewerManifoldExecution)
                .ToListAsync();

            foreach (var pdp in pdps)
            {
                var smReview = sewerManifoldRefs
                    .Where(x => x.SewerManifoldExecution.ProductionDailyPartId == pdp.Id)
                    .Select(x => x.SewerManifoldReview)
                    .FirstOrDefault();

                sewerManifoldRefs.FirstOrDefault(x => x.SewerManifoldExecution.ProductionDailyPartId == pdp.Id).ProductionDailyPartId = pdp.Id;

                var FillingFormula = smReview.DitchHeight - (smReview.PipelineDiameter / 1000) - 0.30 - 0.25;
                var TheoreticalLayerFormula = Math.Round((FillingFormula / 0.3), MidpointRounding.AwayFromZero);
                var FillLengthFormula = smReview.LengthOfDigging * TheoreticalLayerFormula;
                pdp.Filling = Math.Round(FillingFormula, 2);
                pdp.FillLength = Math.Round(FillLengthFormula, 2);
                pdp.TheoreticalLayer = TheoreticalLayerFormula;
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("listar-scheduler")]
        public async Task<IActionResult> GetAllScheduler(ShedulerF7ViewModel model)
        {
            if (model.SewerGroupShedulers == null)
                return BadRequest("No se ha ingresado ninguna cuadrilla");

            var pdps = _context.ProductionDailyParts
                .Include(x => x.ProjectFormula)
                .Include(x => x.WorkFrontHead)
                .Include(x => x.WorkFrontHead.User)
                .Include(x => x.WorkFront)
                .Include(x => x.SewerGroup)
                .Where(x => x.ProjectId == GetProjectId());

            var pdpsfiltered = new List<ProductionDailyPart>();

            foreach(var group in model.SewerGroupShedulers)
            {
                pdpsfiltered.AddRange(pdps.Where(x => x.SewerGroupId == group).ToList());
            }

            var sewerManifoldRefs = await _context.SewerManifoldReferences
                .Include(y => y.SewerManifoldReview)
                .Include(y => y.SewerManifoldExecution)
                .Include(y => y.SewerManifoldReview.SewerBoxStart)
                .Include(y => y.SewerManifoldReview.SewerBoxEnd)
                .ToListAsync();

            var query = new List<F7PdpViewModel>();

            foreach (var pdp in pdpsfiltered)
            {
                var smReview = sewerManifoldRefs
                    .Where(x => x.SewerManifoldExecution.ProductionDailyPartId == pdp.Id)
                    .Select(x => x.SewerManifoldReview)
                    .FirstOrDefault();

                var folding = await _context.FoldingF7s.Where(x => x.ProductionDailyPartId == pdp.Id && x.Date >= model.StartDate.ToDateTime() && x.Date <= model.EndDate.ToDateTime()).ToListAsync();

                if (folding.Count != 0)
                {
                    query.Add(new F7PdpViewModel
                    {
                        Id = pdp.Id,
                        ProjectFormula = new ProjectFormulaViewModel
                        {
                            Code = pdp.ProjectFormula.Code
                        },
                        ReportDate = pdp.ReportDate.ToDateString(),
                        WorkFrontHead = new WorkFrontHeadViewModel
                        {
                            User = new UserViewModel
                            {
                                AuxFullName = pdp.WorkFrontHead.User.FullName
                            }
                        },
                        SewerGroup = new SewerGroupViewModel
                        {
                            Code = pdp.SewerGroup.Code
                        },
                        SewerManifold = new SewerManifoldViewModel
                        {
                            Name = smReview.Name,
                            LengthOfDigging = smReview.LengthOfDigging,
                            LengthOfPipeInstalled = smReview.LengthOfPipelineInstalled,
                            DitchHeight = smReview.DitchHeight,
                        },
                        Filling = pdp.Filling,
                        TheoreticalLayer = pdp.TheoreticalLayer,
                        FillLength = pdp.FillLength,
                        ExcavatedLength = pdp.ExcavatedLength,
                        InstalledLength = pdp.InstalledLength,
                        RefilledLength = pdp.RefilledLength,
                        GranularBaseLength = pdp.GranularBaseLength,
                        Excavation = pdp.Excavation,
                        Installation = pdp.Installation,
                        Filled = pdp.Filled,
                        Status = pdp.Status,
                        Folding = folding.Select(x => new FoldingF7ViewModel()
                        {
                            Id = x.Id,
                            ExcavatedLength = Math.Round(x.ExcavatedLength, MidpointRounding.AwayFromZero).ToString(),
                            InstalledLength = Math.Round(x.InstalledLength, MidpointRounding.AwayFromZero).ToString(),
                            RefilledLength = Math.Round(x.RefilledLength, MidpointRounding.AwayFromZero).ToString(),
                            GranularBaseLength = Math.Round(x.GranularBaseLength, MidpointRounding.AwayFromZero).ToString(),
                            CalendarDate = x.Date
                        }).ToList()
                    });
                }
            }
            return Ok(query);
        }

        [HttpGet("instalado")]
        public async Task<IActionResult> GetTotalInstalled()
        {
            var foldings = await _context.FoldingF7s.ToListAsync();

            var Suma = 0.0;

            foreach (var item in foldings)
                Suma += item.InstalledLength;

            return Ok(Math.Round(Suma,2));
        }
    }
}