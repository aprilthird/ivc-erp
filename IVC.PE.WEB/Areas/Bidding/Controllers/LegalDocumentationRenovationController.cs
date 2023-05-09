using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Bidding;
using IVC.PE.WEB.Areas.Bidding.ViewModels.LegalDocumentationRenovationViewModels;
using IVC.PE.WEB.Areas.Bidding.ViewModels.LegalDocumentationViewModels;
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
    [Route("licitaciones/documentos-legal/renovaciones")]
    public class LegalDocumentationRenovationController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        public LegalDocumentationRenovationController(IvcDbContext context,
        IOptions<CloudStorageCredentials> storageCredentials,
        ILogger<LegalDocumentationRenovationController> logger) : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid legalDocumentationId)
        {
            if (legalDocumentationId == Guid.Empty)
                return Ok(new List<LegalDocumentationRenovationViewModel>());

            var renovations = await _context.LegalDocumentationRenovations
                .Include(x => x.LegalDocumentation)
                .Where(x => x.LegalDocumentationId == legalDocumentationId)
                .Select(x => new LegalDocumentationRenovationViewModel
                {
                    Id = x.Id,
                    LegalDocumentationId = x.LegalDocumentationId,
                    CreateDate = x.CreateDate.Date.ToDateString(),
                    EndDate = x.EndDate.Date.ToDateString(),
                    daysLimitTerm = x.DaysLimitTerm,
                    LegalDocumentationOrder = x.LegalDocumentationOrder,
                    IsTheLast = x.IsTheLast,
                }).OrderByDescending(x => x.LegalDocumentationOrder)
                .ToListAsync();

            return Ok(renovations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var renovation = await _context.LegalDocumentationRenovations
                .Include(x => x.LegalDocumentation)
                .Where(x => x.Id == id)
                .Select(x => new LegalDocumentationRenovationViewModel
                {
                    Id = x.Id,
                    LegalDocumentationId = x.LegalDocumentationId,
                    CreateDate = x.CreateDate.Date.ToDateString(),
                    EndDate = x.EndDate.Date.ToDateString(),
                    daysLimitTerm = x.DaysLimitTerm,
                    LegalDocumentationOrder = x.LegalDocumentationOrder,
                    IsTheLast = x.IsTheLast,
                    FileUrl = x.FileUrl,
                }).FirstOrDefaultAsync();

            

            return Ok(renovation);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(LegalDocumentationRenovationViewModel model)
        {
            if (model.LegalDocumentationRenovationId == Guid.Empty)
                return BadRequest("No se envió ID de la documentación legal a renovar.");

            var toRenovation = await _context.LegalDocumentationRenovations.Include(x => x.LegalDocumentation).FirstOrDefaultAsync(x => x.Id == model.LegalDocumentationRenovationId);

            var bond = await _context.LegalDocumentations.FirstOrDefaultAsync(x => x.Id == toRenovation.LegalDocumentationId);
            bond.NumberOfRenovations++;

            var newRenovation = new LegalDocumentationRenovation()
            {
                LegalDocumentationId = toRenovation.LegalDocumentationId,
                LegalDocumentationOrder = bond.NumberOfRenovations,
                CreateDate = model.CreateDate.ToDateTime(),
                EndDate = model.EndDate.ToDateTime(),
                DaysLimitTerm = 0,
                Days5 = false,
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
                    ConstantHelpers.Storage.Blobs.LEGAL_DOCUMENTATION,
                    $"documento-legal_{newRenovation.LegalDocumentationId}_nro-{newRenovation.LegalDocumentationOrder}");
            }

            


            await _context.LegalDocumentationRenovations.AddAsync(newRenovation);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, LegalDocumentationRenovationViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var renovation = await _context.LegalDocumentationRenovations.Include(x => x.LegalDocumentation).FirstOrDefaultAsync(x => x.Id == id);

            renovation.CreateDate = model.CreateDate.ToDateTime();
            renovation.EndDate = model.EndDate.ToDateTime();
            renovation.IsTheLast = model.IsTheLast;

            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (renovation.FileUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.LEGAL_DOCUMENTATION}/{renovation.FileUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.BIDDING);
                renovation.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.BIDDING,
                    System.IO.Path.GetExtension(model.File.FileName),
                    ConstantHelpers.Storage.Blobs.LEGAL_DOCUMENTATION,
                     $"documento-legal_{renovation.LegalDocumentationId}_nro-{renovation.LegalDocumentationOrder}");
            }

            

            

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var renovation = await _context.LegalDocumentationRenovations.FirstOrDefaultAsync(x => x.Id == id);

            if (renovation.LegalDocumentationOrder == 1)
            {
                return BadRequest("No se puede eliminar la inicial.");
            }

            

            

            if (renovation.FileUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.LEGAL_DOCUMENTATION}/{renovation.FileUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.BIDDING);
            }

            var legaldoc = await _context.LegalDocumentations.
                FirstOrDefaultAsync(x => x.Id == renovation.LegalDocumentationId);
            legaldoc.NumberOfRenovations--;


            _context.LegalDocumentationRenovations.Remove(renovation);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
