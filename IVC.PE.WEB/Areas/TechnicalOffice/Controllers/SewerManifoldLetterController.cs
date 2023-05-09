using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.SewerManifoldViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IVC.PE.WEB.Areas.TechnicalOffice.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.TECHNICAL_OFFICE)]
    [Route("oficina-tecnica/cartas-colectores-descarga")]
    public class SewerManifoldLetterController : BaseController
    {
        public SewerManifoldLetterController(IvcDbContext context,
            ILogger<SewerManifoldLetterController> logger)
            : base(context, logger)
        {
        }

        [HttpGet("listar/{id}")]
        public async Task<IActionResult> GetAllLetters(Guid id)
        {
            var letters = await _context.SewerManifoldLetters
                .Where(x => x.SewerManifoldId == id)
                .ToListAsync();

            var vm = new SewerManifoldLetterListViewModel
            {
                RequestLetterIds = letters.Where(x => x.LetterType == ConstantHelpers.Sewer.Manifolds.Letters.REQUEST).Select(x => x.LetterId).ToList(),
                ApprovalLetterIds = letters.Where(x => x.LetterType == ConstantHelpers.Sewer.Manifolds.Letters.APPROVAL).Select(x => x.LetterId).ToList()
            };

            return Ok(vm);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(SewerManifoldLetterListViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            foreach (var smId in model.SewerManifoldIds)
            {
                var smLettersDb = await _context.SewerManifoldLetters
                .Where(x => x.SewerManifoldId == smId)
                .ToListAsync();

                _context.SewerManifoldLetters.RemoveRange(smLettersDb);

                if (model.RequestLetterIds != null)
                {
                    await _context.SewerManifoldLetters.AddRangeAsync(
                        model.RequestLetterIds.Select(x => new SewerManifoldLetter
                        {
                            LetterId = x,
                            SewerManifoldId = smId,
                            LetterType = ConstantHelpers.Sewer.Manifolds.Letters.REQUEST
                        }).ToList());
                }

                if (model.ApprovalLetterIds != null)
                {
                    await _context.SewerManifoldLetters.AddRangeAsync(
                        model.ApprovalLetterIds.Select(x => new SewerManifoldLetter
                        {
                            LetterId = x,
                            SewerManifoldId = smId,
                            LetterType = ConstantHelpers.Sewer.Manifolds.Letters.APPROVAL
                        }).ToList());
                }
                
            }
            
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, SewerManifoldLetterListViewModel model)
        {
            var smLettersDb = await _context.SewerManifoldLetters
                .Where(x => x.SewerManifoldId == id)
                .ToListAsync();

            _context.SewerManifoldLetters.RemoveRange(smLettersDb);

            if (model.RequestLetterIds != null)
            {
                await _context.SewerManifoldLetters.AddRangeAsync(
            model.RequestLetterIds.Select(x => new SewerManifoldLetter
            {
                LetterId = x,
                SewerManifoldId = id,
                LetterType = ConstantHelpers.Sewer.Manifolds.Letters.REQUEST
            }).ToList());
            }

            if (model.ApprovalLetterIds != null)
            {
                await _context.SewerManifoldLetters.AddRangeAsync(
                model.ApprovalLetterIds.Select(x => new SewerManifoldLetter
                {
                    LetterId = x,
                    SewerManifoldId = id,
                    LetterType = ConstantHelpers.Sewer.Manifolds.Letters.APPROVAL
                }).ToList());
            }

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
