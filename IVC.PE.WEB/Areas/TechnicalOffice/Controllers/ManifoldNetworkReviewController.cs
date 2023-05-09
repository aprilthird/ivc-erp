using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
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
    [Route("oficina-tecnica/replanteo-colectores-descarga")]
    public class ManifoldNetworkReviewController : BaseController
    {
        public ManifoldNetworkReviewController(IvcDbContext context,
            ILogger<ManifoldNetworkReviewController> logger)
            : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpPost("partir-tramo")]
        public async Task<IActionResult> CreatePartition(SewerBoxViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var smToPartition = await _context.SewerManifolds
                .Include(x => x.SewerBoxStart)
                .Include(x => x.SewerBoxEnd)
                .FirstOrDefaultAsync(x => x.Id == model.SewerManifoldToPartitionId);

            if (smToPartition.ProductionDailyPartId.HasValue)
                return BadRequest("El Tramo cuenta con un PDP registrado.");

            var sbNew = new SewerBox
            {
                ProjectId = smToPartition.ProjectId,
                ProcessType = smToPartition.ProcessType,
                Code = model.Code,
                CoverLevel = model.CoverLevel,
                ArrivalLevel = model.ArrivalLevel,
                BottomLevel = model.BottomLevel,
                TerrainType = model.TerrainType,
                SewerBoxType = model.SewerBoxType,
                Thickness = model.Thickness,
                Height = model.CoverLevel - model.BottomLevel,
                Diameter = model.Thickness == 0.40 ? 1.50 : 1.80,
                SewerOrderNumber = smToPartition.SewerBoxEnd.SewerOrderNumber
            };

            var smNewA = new SewerManifold
            {
                ProjectId = smToPartition.ProjectId,
                ProcessType = smToPartition.ProcessType,
                SewerBoxStart = smToPartition.SewerBoxStart,
                SewerBoxEnd = sbNew,
                Address = smToPartition.Address,
                Name = $"Del {smToPartition.SewerBoxStart.Code} al {sbNew.Code}",
                DitchHeight = smToPartition.DitchHeight,
                DitchLevelPercent = smToPartition.DitchLevelPercent,
                PipelineDiameter = smToPartition.PipelineDiameter,
                PipelineType = smToPartition.PipelineType,
                PipelineClass = smToPartition.PipelineClass,
                LengthBetweenHAxles = smToPartition.LengthBetweenHAxles,
                LengthBetweenIAxles = smToPartition.LengthBetweenIAxles,
                LengthOfPipelineInstalled = smToPartition.LengthOfPipelineInstalled,
                TerrainType = smToPartition.TerrainType,
                LengthOfDigging = smToPartition.LengthOfDigging,
                Pavement2In = smToPartition.Pavement2In,
                Pavement3In = smToPartition.Pavement3In,
                Pavement3InMixed = smToPartition.Pavement3InMixed,
                PavementWidth = smToPartition.PavementWidth
            };

            var smNewB = new SewerManifold
            {
                ProjectId = smToPartition.ProjectId,
                ProcessType = smToPartition.ProcessType,
                SewerBoxStart = sbNew,
                SewerBoxEnd = smToPartition.SewerBoxEnd,
                Address = smToPartition.Address,
                Name = $"Del {sbNew.Code} al {smToPartition.SewerBoxEnd.Code}",
                DitchHeight = smToPartition.DitchHeight,
                DitchLevelPercent = smToPartition.DitchLevelPercent,
                PipelineDiameter = smToPartition.PipelineDiameter,
                PipelineType = smToPartition.PipelineType,
                PipelineClass = smToPartition.PipelineClass,
                LengthBetweenHAxles = smToPartition.LengthBetweenHAxles,
                LengthBetweenIAxles = smToPartition.LengthBetweenIAxles,
                LengthOfPipelineInstalled = smToPartition.LengthOfPipelineInstalled,
                TerrainType = smToPartition.TerrainType,
                LengthOfDigging = smToPartition.LengthOfDigging,
                Pavement2In = smToPartition.Pavement2In,
                Pavement3In = smToPartition.Pavement3In,
                Pavement3InMixed = smToPartition.Pavement3InMixed,
                PavementWidth = smToPartition.PavementWidth
            };

            var smLetters = await _context.SewerManifoldLetters
                .Where(x => x.SewerManifoldId == model.SewerManifoldToPartitionId)
                .ToListAsync();
            if (smLetters.Count > 0)
            {
                _context.SewerManifoldLetters.RemoveRange(smLetters);
                await _context.SewerManifoldLetters.AddRangeAsync(
                    smLetters.Select(x => new SewerManifoldLetter
                    {
                        LetterId = x.LetterId,
                        SewerManifold = smNewA
                    }).ToList());
            }

            var sbUpdateOrder = await _context.SewerBoxes
                .Where(x => x.SewerOrderNumber >= smToPartition.SewerBoxEnd.SewerOrderNumber && 
                            x.ProcessType == ConstantHelpers.Sewer.Manifolds.Process.REVIEW)
                .ToListAsync();
            foreach (var sb in sbUpdateOrder)
                sb.SewerOrderNumber++;


            _context.SewerManifolds.Remove(smToPartition);
            await _context.SewerBoxes.AddAsync(sbNew);
            await _context.SewerManifolds.AddAsync(smNewA);
            await _context.SewerManifolds.AddAsync(smNewB);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
