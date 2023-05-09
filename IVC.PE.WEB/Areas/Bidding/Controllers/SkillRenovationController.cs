using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Bidding;
using IVC.PE.WEB.Areas.Bidding.ViewModels.SkillRenovationViewModels;
using IVC.PE.WEB.Areas.Bidding.ViewModels.SkillViewModels;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Options;
using IVC.PE.WEB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Bidding.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Bidding.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.BIDDING)]
    [Route("licitaciones/habilidades/renovaciones")]

    public class SkillRenovationController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public SkillRenovationController(IvcDbContext context,
        IOptions<CloudStorageCredentials> storageCredentials,
        ILogger<SkillRenovationController> logger) : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }
        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid skillRenovationId)
        {
            if (skillRenovationId == Guid.Empty)
                return Ok(new List<SkillRenovationViewModel>());

            var renovations = await _context.SkillRenovations
                .Include(x => x.Skill)
                .Where(x => x.SkillId == skillRenovationId)
                .Select(x => new SkillRenovationViewModel
                {
                    Id = x.Id,
                    SkillId = x.SkillId,
                    CreateDate = x.CreateDate.Date.ToDateString(),
                    EndDate = x.EndDate.Date.ToDateString(),
                    SkillOrder = x.SkillOrder,
                    IsTheLast = x.IsTheLast,
                }).OrderByDescending(x => x.SkillOrder)
                .ToListAsync();

            return Ok(renovations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var renovation = await _context.SkillRenovations
                .Include(x => x.Skill)
                .Where(x => x.Id == id)
                .Select(x => new SkillRenovationViewModel
                {
                    Id = x.Id,
                    SkillId = x.SkillId,
                    CreateDate = x.CreateDate.Date.ToDateString(),
                    EndDate = x.EndDate.Date.ToDateString(),
                    SkillOrder = x.SkillOrder,
                    IsTheLast = x.IsTheLast,
                    FileUrl = x.FileUrl,
                }).FirstOrDefaultAsync();

            return Ok(renovation);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(SkillRenovationViewModel model)
        {
            if (model.SkillRenovationId == Guid.Empty)
                return BadRequest("No se envió ID de la habilidad a renovar.");

            var toRenovation = await _context.SkillRenovations.Include(x => x.Skill).FirstOrDefaultAsync(x => x.Id == model.SkillRenovationId);

            var bond = await _context.Skills.FirstOrDefaultAsync(x => x.Id == toRenovation.SkillId);
            bond.NumberOfRenovations++;

            var newRenovation = new SkillRenovation()
            {
                SkillId = toRenovation.SkillId,
                SkillOrder = bond.NumberOfRenovations,
                CreateDate = model.CreateDate.ToDateTime(),
                EndDate = model.EndDate.ToDateTime(),
                Days15 = false,
                Days30 = false,
                IsTheLast = model.IsTheLast
            };

            if (newRenovation.CreateDate > newRenovation.EndDate)
                return BadRequest("Rango de fechas equivocado.");
            if (newRenovation.CreateDate < toRenovation.EndDate)
                return BadRequest("Fecha de Creación de la Renovación no puede ser menor a la Fecha de Fin de la documentacion legal a renovar.");

            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                newRenovation.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.BIDDING,
                    System.IO.Path.GetExtension(model.File.FileName),
                    ConstantHelpers.Storage.Blobs.SKILL,
                    $"habilidad_{newRenovation.SkillId}_nro-{newRenovation.SkillOrder}");
            }


            await _context.SkillRenovations.AddAsync(newRenovation);
            await _context.SaveChangesAsync();


            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, SkillRenovationViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var renovation = await _context.SkillRenovations.Include(x => x.Skill).FirstOrDefaultAsync(x => x.Id == id);

            renovation.CreateDate = model.CreateDate.ToDateTime();
            renovation.EndDate = model.EndDate.ToDateTime();
            renovation.IsTheLast = model.IsTheLast;

            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (renovation.FileUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.SKILL}/{renovation.FileUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.BIDDING);
                renovation.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.BIDDING,
                    System.IO.Path.GetExtension(model.File.FileName),
                    ConstantHelpers.Storage.Blobs.SKILL,
                     $"habilidad_{renovation.SkillId}_nro-{renovation.SkillOrder}");
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var renovation = await _context.SkillRenovations.FirstOrDefaultAsync(x => x.Id == id);

            if (renovation.SkillOrder == 1)
            {
                return BadRequest("No se puede eliminar la inicial.");
            }

            if (renovation.FileUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.SKILL}/{renovation.FileUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.BIDDING);
            }

            var skill = await _context.Skills.
                FirstOrDefaultAsync(x => x.Id == renovation.SkillId);
            skill.NumberOfRenovations--;

            _context.SkillRenovations.Remove(renovation);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
