using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.IntegratedManagementSystem;
using IVC.PE.WEB.Areas.IntegratedManagementSystem.ViewModels.SewerManifoldFor24ViewModels;
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

namespace IVC.PE.WEB.Areas.IntegratedManagementSystem.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.IntegratedManagementSystem.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.INTEGRATED_MANAGEMENT_SYSTEM)]
    [Route("sistema-de-manejo-integrado/third-part-for24")]
    public class SewerManifoldFor24ThirdPartController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        public SewerManifoldFor24ThirdPartController(IvcDbContext context,
            IOptions<CloudStorageCredentials> storageCredentials,
            ILogger<SewerManifoldFor24ThirdPartController> logger) : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var for24s = await _context.SewerManifoldFor24ThirdParts
                .Select(x => new SewerManifoldFor24ThirdPartViewModel
                {
                    Id = x.Id,
                    ActionTaken = x.ActionTaken,
                    PreventiveCorrectiveAction = x.PreventiveCorrectiveAction,
                    ClosingDate = x.ClosingDate.ToString(),
                    FileUrl=x.FileUrl
                }).ToListAsync();

            return Ok(for24s);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var for24 = await _context.SewerManifoldFor24ThirdParts
                .Where(x => x.Id == id)
                .Select(x => new SewerManifoldFor24ThirdPartViewModel
                {
                    Id = x.Id,
                    ActionTaken = x.ActionTaken,
                    PreventiveCorrectiveAction = x.PreventiveCorrectiveAction,
                    ClosingDate = x.ClosingDate.ToString(),
                    FileUrl = x.FileUrl
                }).FirstOrDefaultAsync();

            return Ok(for24);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(SewerManifoldFor24ThirdPartViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var for24 = new SewerManifoldFor24ThirdPart
            {
                SewerManifoldFor24SecondPartId = model.SewerManifoldFor24SecondPartId,
                ActionTaken = model.ActionTaken,
                PreventiveCorrectiveAction = model.PreventiveCorrectiveAction,
                ClosingDate = model.ClosingDate.ToDateTime(),
            };

            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                for24.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.INTEGRATED_MANAGEMENT_SYSTEM,
                    System.IO.Path.GetExtension(model.File.Name),
                    ConstantHelpers.Storage.Blobs.FOR_24_THIRD_PART,
                    $"consolidated_for47_tercera_parte_accion_{model.ActionTaken}_{model.File.FileName}");
            }

            await _context.SewerManifoldFor24ThirdParts.AddAsync(for24);

            var for11 = await _context.SewerManifoldFor24s.FirstOrDefaultAsync(x => x.SewerManifoldFor24SecondPartId == model.SewerManifoldFor24SecondPartId);
            for11.SewerManifoldFor24ThirdpartId = for24.Id;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var for11 = await _context.SewerManifoldFor24s
                .FirstOrDefaultAsync(x => x.SewerManifoldFor24ThirdpartId == id);

            for11.SewerManifoldFor24ThirdpartId = null;

            var for24 = await _context.SewerManifoldFor24ThirdParts
                .FirstOrDefaultAsync(x => x.Id == id);

            if (for24 == null)
                return BadRequest("Tercera parte no encontrada");

            if (for24.FileUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.FOR_24_THIRD_PART}/{for24.FileUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.INTEGRATED_MANAGEMENT_SYSTEM);
            }

            _context.SewerManifoldFor24ThirdParts.Remove(for24);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
