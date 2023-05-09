using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Production;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.ENTITIES.UspModels.Production;
using IVC.PE.WEB.Areas.Production.ViewModels.PdpViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IVC.PE.WEB.Areas.Production.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Production.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.PRODUCTION)]
    [Route("produccion/pdp")]
    public class PdpController : BaseController
    {
        public PdpController(IvcDbContext context,
            ILogger<PdpController> logger)
            : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            SqlParameter projectParam = new SqlParameter("@ProjectId", GetProjectId());

            var query = await _context.Set<UspProductionDailyPart>().FromSqlRaw("execute Production_uspProductionDailyParts @ProjectId"
                , projectParam)
                .IgnoreQueryFilters()
                .ToListAsync();

            return Ok(query);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var sewerManifoldId = await _context.SewerManifoldReferences
                .Include(x => x.SewerManifoldExecution)
                .Where(x => x.SewerManifoldExecution.ProductionDailyPartId == id)
                .Select(x => x.SewerManifoldReviewId)
                .FirstOrDefaultAsync();
                

            var pdp = await _context.ProductionDailyParts
                .Where(x => x.Id == id)
                .Select(x => new PdpViewModel
                {
                    Id = x.Id,
                    ProjectFormulaId = x.ProjectFormulaId,
                    ReportDate = x.ReportDate.ToDateString(),
                    SewerGroupId = x.SewerGroupId,
                    SewerManifoldId = sewerManifoldId,
                    WorkFrontHeadId = x.WorkFrontHeadId,
                    WorkFrontId = x.WorkFrontId
                }).FirstOrDefaultAsync();

            return Ok(pdp);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(PdpViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var formula = await _context.ProjectFormulas
                .FirstOrDefaultAsync(x => x.Id == model.ProjectFormulaId);

            if(formula.Code != "F7")
            {
                return BadRequest("Formula en desarrollo.");
            }

            var pdp = new ProductionDailyPart
            { 
                ProjectFormulaId = model.ProjectFormulaId,
                ReportDate = model.ReportDate.ToDateTime(),
                SewerGroupId = model.SewerGroupId,
                WorkFrontHeadId = model.WorkFrontHeadId,
                WorkFrontId = model.WorkFrontId,
                ProjectId = GetProjectId()
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
        public async Task<IActionResult> Edit(Guid id, PdpViewModel model)
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

            _context.ProductionDailyParts.Remove(pdp);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
